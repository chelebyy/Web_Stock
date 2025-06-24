using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stock.Application.Constants;
using Stock.Application.Features.Users.Queries.GetAllUsers;
using Stock.Application.Features.Users.Queries.GetUserById;
using Stock.Application.Features.Users.Commands;
using Stock.Application.Features.Users.Commands.DeleteUser;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;
using Stock.Application.Models.DTOs;
using System.Collections.Generic;
using FluentValidation;
using Stock.Domain.Exceptions;

namespace Stock.API.Controllers
{
    /// <summary>
    /// Kullanıcı yönetimi ile ilgili API endpoint'lerini içerir.
    /// Kullanıcıları listeleme, detaylarını görme, oluşturma, güncelleme ve silme işlemlerini sağlar.
    /// Bu işlemler MediatR aracılığıyla ilgili Handler'lara yönlendirilir.
    /// </summary>
    [ApiController]
    [Route(ApiConstants.ApiBaseRoute + "/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UsersController> _logger;

        /// <summary>
        /// UsersController sınıfının yapıcı metodu.
        /// Bağımlılıkları (IMediator, ILogger) enjekte eder.
        /// </summary>
        /// <param name="mediator">CQRS Mediator arayüzü.</param>
        /// <param name="logger">Loglama servisi.</param>
        public UsersController(IMediator mediator, ILogger<UsersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Sistemdeki tüm kullanıcıları listeler (Sadece Admin yetkisine sahip kullanıcılar).
        /// </summary>
        /// <returns>Tüm kullanıcıların listesi (UserListItemDto).</returns>
        /// <response code="200">Kullanıcılar başarıyla listelendi.</response>
        /// <response code="401">Yetkisiz erişim (Admin rolü gerekli).</response>
        /// <response code="500">Kullanıcılar listelenirken bir sunucu hatası oluştu.</response>
        [HttpGet]
        [Authorize(Roles = RoleNames.Admin)]
        [ProducesResponseType(typeof(IEnumerable<UserListItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation(LogMessages.FetchingAllUsers);
                var query = new GetAllUsersQuery();
                var result = await _mediator.Send(query);
                _logger.LogInformation(string.Format(LogMessages.UsersFetchedSuccessfully, result.Count()));
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, LogMessages.ErrorFetchingUsers);
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = string.Format(ErrorMessages.GeneralServerError, ex.Message) });
            }
        }

        /// <summary>
        /// Belirtilen ID'ye sahip kullanıcının detaylarını getirir.
        /// Giriş yapmış herhangi bir kullanıcı kendi bilgilerini veya Admin rolüne sahip kullanıcılar diğer kullanıcıların bilgilerini getirebilir.
        /// </summary>
        /// <param name="id">Detayları getirilecek kullanıcının ID'si.</param>
        /// <returns>Kullanıcı detayları (UserDto).</returns>
        /// <response code="200">Kullanıcı detayları başarıyla getirildi.</response>
        /// <response code="401">Kimlik doğrulanmamış veya token geçersiz.</response>
        /// <response code="403">Kullanıcının bu bilgilere erişim yetkisi yok (Admin değil ve kendi ID'si değilse - bu kontrol servis katmanında yapılmalı).</response>
        /// <response code="404">Belirtilen ID'ye sahip kullanıcı bulunamadı.</response>
        /// <response code="500">Kullanıcı detayları getirilirken bir sunucu hatası oluştu.</response>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                _logger.LogInformation(string.Format(LogMessages.FetchingUserById, id));
                var query = new GetUserByIdQuery { Id = id };
                var result = await _mediator.Send(query);

                if (result == null)
                {
                    _logger.LogWarning(string.Format(LogMessages.UserNotFound, id));
                    return NotFound(string.Format(ErrorMessages.UserNotFoundSpecific, id));
                }

                _logger.LogInformation(string.Format(LogMessages.UserFetchedSuccessfully, id));
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(LogMessages.ErrorFetchingUser, id));
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = string.Format(ErrorMessages.GeneralServerError, ex.Message) });
            }
        }

        /// <summary>
        /// Yeni bir kullanıcı oluşturur (Sadece Admin yetkisine sahip kullanıcılar).
        /// Bu endpoint AuthController'daki CreateUser ile benzer işlevi görür ancak direkt UserDto döner.
        /// </summary>
        /// <param name="command">Kullanıcı oluşturma komutu (kullanıcı adı, şifre, sicil, rol ID vb.).</param>
        /// <returns>Oluşturulan kullanıcı bilgileri (UserDto).</returns>
        /// <response code="201">Kullanıcı başarıyla oluşturuldu. Oluşturulan kullanıcının detaylarını döner.</response>
        /// <response code="400">İstek geçersiz veya eksik parametre içeriyor (örn. boş kullanıcı adı, geçersiz rol ID, zaten kullanılan sicil).</response>
        /// <response code="401">Yetkisiz erişim (Admin rolü gerekli).</response>
        /// <response code="500">Kullanıcı oluşturma sırasında beklenmeyen bir sunucu hatası oluştu.</response>
        [HttpPost]
        [Authorize(Roles = RoleNames.Admin)]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
        {
            try
            {
                _logger.LogInformation(string.Format(LogMessages.UserCreating, command.Sicil));
                var result = await _mediator.Send(command);
                _logger.LogInformation(string.Format(LogMessages.UserCreated, result.Sicil, result.Id));
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Stock.Domain.Exceptions.ValidationException ex)
            {
                _logger.LogWarning(LogMessages.ValidationErrorDuringUserCreation, ex.Message);
                var problemDetails = new ValidationProblemDetails(ex.Errors
                    .GroupBy(kvp => kvp.Key)
                    .ToDictionary(g => g.Key, g => g.SelectMany(kvp => kvp.Value).ToArray()));
                return BadRequest(problemDetails);
            }
            catch (ConflictException ex)
            {
                _logger.LogWarning(LogMessages.ConflictDuringUserCreation, command.Sicil, ex.Message);
                return Conflict(new { error = ex.Message });
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(LogMessages.NotFoundDuringUserCreation, ex.Message);
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, LogMessages.ErrorDuringUserCreation);
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ErrorMessages.UserCreationError });
            }
        }

        /// <summary>
        /// Mevcut bir kullanıcıyı günceller (Sadece Admin yetkisine sahip kullanıcılar).
        /// </summary>
        /// <param name="id">Güncellenecek kullanıcının ID'si.</param>
        /// <param name="command">Güncellenmiş kullanıcı bilgilerini içeren komut.</param>
        /// <returns>Güncellenmiş kullanıcı bilgileri (UserDto).</returns>
        /// <response code="200">Kullanıcı başarıyla güncellendi.</response>
        /// <response code="400">İstek geçersiz (örn. ID uyuşmazlığı, validasyon hatası, zaten kullanılan sicil).</response>
        /// <response code="401">Yetkisiz erişim (Admin rolü gerekli).</response>
        /// <response code="404">Belirtilen ID'ye sahip kullanıcı bulunamadı.</response>
        /// <response code="500">Kullanıcı güncelleme sırasında beklenmeyen bir sunucu hatası oluştu.</response>
        [HttpPut("{id}")]
        [Authorize(Roles = RoleNames.Admin)]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserCommand command)
        {
            try
            {
                if (id != command.Id)
                {
                    _logger.LogWarning(string.Format(LogMessages.RouteBodyIdMismatch, id, command.Id));
                    return BadRequest(new { error = ErrorMessages.RouteBodyIdMismatch });
                }
                
                _logger.LogInformation(string.Format(LogMessages.UserUpdating, id));
                var result = await _mediator.Send(command);
                
                if (result == null)
                {
                    _logger.LogWarning(string.Format(LogMessages.UserNotFoundForUpdate, id));
                    return NotFound(new { error = string.Format(ErrorMessages.UserNotFoundSpecific, id) });
                }
                
                _logger.LogInformation(string.Format(LogMessages.UserUpdated, result.Id));
                return Ok(result);
            }
            catch (Stock.Domain.Exceptions.ValidationException ex)
            {
                _logger.LogWarning(LogMessages.ValidationErrorDuringUserUpdate, ex.Message);
                var problemDetails = new ValidationProblemDetails(ex.Errors
                    .GroupBy(kvp => kvp.Key)
                    .ToDictionary(g => g.Key, g => g.SelectMany(kvp => kvp.Value).ToArray()));
                return BadRequest(problemDetails);
            }
            catch (ConflictException ex)
            {
                _logger.LogWarning(LogMessages.ConflictDuringUserUpdate, id, ex.Message);
                return Conflict(new { error = ex.Message });
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(LogMessages.NotFoundDuringUserUpdate, id, ex.Message);
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(LogMessages.ErrorUpdatingUser, id));
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ErrorMessages.UserUpdateError });
            }
        }

        /// <summary>
        /// Belirtilen ID'ye sahip kullanıcıyı siler (Sadece Admin yetkisine sahip kullanıcılar).
        /// </summary>
        /// <param name="id">Silinecek kullanıcının ID'si.</param>
        /// <returns>Başarılı olursa NoContent (204) yanıtı döner.</returns>
        /// <response code="204">Kullanıcı başarıyla silindi.</response>
        /// <response code="401">Yetkisiz erişim (Admin rolü gerekli).</response>
        /// <response code="404">Belirtilen ID'ye sahip kullanıcı bulunamadı.</response>
        /// <response code="500">Kullanıcı silme sırasında beklenmeyen bir sunucu hatası oluştu.</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = RoleNames.Admin)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation(string.Format(LogMessages.UserDeleting, id));
                var command = new DeleteUserCommand { Id = id };
                await _mediator.Send(command);
                _logger.LogInformation(string.Format(LogMessages.UserDeleted, id));
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(LogMessages.NotFoundDuringUserDelete, id, ex.Message);
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(LogMessages.ErrorDuringUserDelete, id));
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ErrorMessages.UserDeleteError });
            }
        }
    }
} 