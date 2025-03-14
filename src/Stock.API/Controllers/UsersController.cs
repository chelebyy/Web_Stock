using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stock.Application.Features.Users.Queries;
using Stock.Application.Features.Users.Commands;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.AspNetCore.Http;

namespace Stock.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UsersController> _logger;

        // Sabit hata mesajları
        private const string ERROR_USER_NOT_FOUND = "ID: {0} olan kullanıcı bulunamadı.";
        private const string ERROR_USER_CREATE = "Kullanıcı eklenirken bir hata oluştu.";
        private const string ERROR_USER_UPDATE = "Kullanıcı güncellenirken bir hata oluştu.";
        private const string ERROR_USER_DELETE = "Kullanıcı silinirken bir hata oluştu.";
        private const string ERROR_ID_MISMATCH = "URL'deki ID ile request body'deki ID eşleşmiyor.";
        private const string ERROR_VALIDATION = "{0}";

        public UsersController(IMediator mediator, ILogger<UsersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllUsersQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetUserByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            
            if (result == null)
                return NotFound(new { error = string.Format(ERROR_USER_NOT_FOUND, id) });
                
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Stock.Application.Features.Users.Commands.CreateUserCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                _logger.LogInformation("Kullanıcı oluşturuldu. ID: {Id}", result.Id);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                // Sicil benzersizlik hatası veya diğer doğrulama hataları
                string errorMessage = ex.Message;
                _logger.LogWarning("Kullanıcı ekleme işlemi sırasında doğrulama hatası: {Message}", errorMessage);
                
                // Özel olarak sicil numarası hatası için kontrol et
                if (errorMessage.Contains("sicil numarası zaten kullanılmaktadır"))
                {
                    return BadRequest(new { error = errorMessage });
                }
                
                return BadRequest(new { error = string.Format(ERROR_VALIDATION, errorMessage) });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı ekleme işlemi sırasında hata oluştu");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ERROR_USER_CREATE });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, Stock.Application.Features.Users.Commands.UpdateUserCommand command)
        {
            try
            {
                if (id != command.Id)
                {
                    _logger.LogWarning("ID uyuşmazlığı: URL'deki {UrlId} ile gönderilen {CommandId} eşleşmiyor", id, command.Id);
                    return BadRequest(new { error = ERROR_ID_MISMATCH });
                }
                
                var result = await _mediator.Send(command);
                
                if (result == null)
                {
                    _logger.LogWarning("Güncellenecek kullanıcı bulunamadı: ID: {Id}", id);
                    return NotFound(new { error = string.Format(ERROR_USER_NOT_FOUND, id) });
                }
                
                _logger.LogInformation("Kullanıcı güncellendi. ID: {Id}", result.Id);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                // Sicil benzersizlik hatası veya diğer doğrulama hataları
                string errorMessage = ex.Message;
                _logger.LogWarning("Kullanıcı güncelleme işlemi sırasında doğrulama hatası: {Message}", errorMessage);
                
                // Özel olarak sicil numarası hatası için kontrol et
                if (errorMessage.Contains("sicil numarası zaten"))
                {
                    return BadRequest(new { error = errorMessage });
                }
                
                return BadRequest(new { error = string.Format(ERROR_VALIDATION, errorMessage) });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı güncelleme işlemi sırasında hata oluştu");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ERROR_USER_UPDATE });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation("Kullanıcı silme isteği başlatıldı: ID: {Id}", id);
                var command = new Stock.Application.Features.Users.Commands.DeleteUserCommand { Id = id };
                var result = await _mediator.Send(command);
                
                if (!result)
                {
                    _logger.LogWarning("Silinecek kullanıcı bulunamadı: ID: {Id}", id);
                    return NotFound(new { error = string.Format(ERROR_USER_NOT_FOUND, id) });
                }
                
                _logger.LogInformation("Kullanıcı silindi. ID: {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı silme işlemi sırasında hata oluştu");
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = ERROR_USER_DELETE });
            }
        }
    }
} 