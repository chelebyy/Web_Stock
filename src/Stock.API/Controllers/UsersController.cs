using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stock.API.Constants;
using Stock.Application.Features.Users.Queries;
using Stock.Application.Features.Users.Commands;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;

namespace Stock.API.Controllers
{
    [ApiController]
    [Route(ApiConstants.ApiBaseRoute + "/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IMediator mediator, ILogger<UsersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = RoleNames.Admin)]
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
                return NotFound(ErrorMessages.UserNotFound);
                
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = RoleNames.Admin)]
        public async Task<IActionResult> Create(Stock.Application.Features.Users.Commands.CreateUserCommand command)
        {
            try
            {
                _logger.LogInformation(string.Format(LogMessages.UserCreating, command.Username));
                var result = await _mediator.Send(command);
                _logger.LogInformation(string.Format(LogMessages.UserCreated, result.Username, result.Id));
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                // Sicil benzersizlik hatası veya diğer doğrulama hataları
                string errorMessage = ex.Message;
                _logger.LogWarning(string.Format(LogMessages.ValidationErrorDuringUserCreation, errorMessage));
                
                // Özel olarak sicil numarası hatası için kontrol et
                if (errorMessage.Contains(ErrorMessages.SicilAlreadyInUsePartial))
                {
                    return BadRequest(new { error = errorMessage });
                }
                
                return BadRequest(new { error = errorMessage });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, LogMessages.ErrorDuringUserCreation);
                return StatusCode(500, new { error = ErrorMessages.UserCreateError });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = RoleNames.Admin)]
        public async Task<IActionResult> Update(int id, Stock.Application.Features.Users.Commands.UpdateUserCommand command)
        {
            try
            {
                if (id != command.Id)
                {
                    _logger.LogWarning(string.Format("ID uyuşmazlığı: URL'deki {0} ile gönderilen {1} eşleşmiyor", id, command.Id));
                    return BadRequest(new { error = "URL'deki ID ile request body'deki ID eşleşmiyor." });
                }
                
                _logger.LogInformation(string.Format(LogMessages.UserUpdating, id));
                var result = await _mediator.Send(command);
                
                if (result == null)
                {
                    _logger.LogWarning(string.Format("Güncellenecek kullanıcı bulunamadı: ID: {0}", id));
                    return NotFound(new { error = string.Format(ErrorMessages.UserNotFound) });
                }
                
                _logger.LogInformation(string.Format(LogMessages.UserUpdated, result.Id));
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                // Sicil benzersizlik hatası veya diğer doğrulama hataları
                string errorMessage = ex.Message;
                _logger.LogWarning(string.Format(LogMessages.ValidationErrorDuringUserCreation, errorMessage));
                
                // Özel olarak sicil numarası hatası için kontrol et
                if (errorMessage.Contains(ErrorMessages.SicilAlreadyInUsePartial))
                {
                    return BadRequest(new { error = errorMessage });
                }
                
                return BadRequest(new { error = errorMessage });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı güncelleme işlemi sırasında hata oluştu");
                return StatusCode(500, new { error = ErrorMessages.UserUpdateError });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = RoleNames.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation(string.Format(LogMessages.UserDeleting, id));
                var command = new Stock.Application.Features.Users.Commands.DeleteUserCommand { Id = id };
                var result = await _mediator.Send(command);
                
                if (!result)
                {
                    _logger.LogWarning(string.Format("Silinecek kullanıcı bulunamadı: ID: {0}", id));
                    return NotFound(new { error = ErrorMessages.UserNotFound });
                }
                
                _logger.LogInformation(string.Format(LogMessages.UserDeleted, id));
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı silme işlemi sırasında hata oluştu");
                return StatusCode(500, new { error = ErrorMessages.UserDeleteError });
            }
        }
    }
} 