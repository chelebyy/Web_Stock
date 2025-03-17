using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stock.API.Constants;
using Stock.Application.Common.Interfaces;
using Stock.Application.Models.DTOs;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Stock.Application.Features.Users.Commands;
using System;

namespace Stock.API.Controllers
{
    [ApiController]
    [Route(ApiConstants.ApiBaseRoute + "/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        private readonly IMediator _mediator;

        public AuthController(IAuthService authService, ILogger<AuthController> logger, IMediator mediator)
        {
            _authService = authService;
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto);
            
            if (!result.Success)
                return Unauthorized(result);
                
            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var result = await _authService.RegisterAsync(registerDto);
            
            if (!result.Success)
                return BadRequest(result);
                
            return Ok(result);
        }

        [Authorize(Roles = RoleNames.Admin)]
        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
        {
            try
            {
                _logger.LogInformation(LogMessages.UserCreationRequest, command.Username, command.Sicil);
                
                // Parametre doğrulamalarını kontrol et
                if (string.IsNullOrWhiteSpace(command.Username))
                {
                    _logger.LogWarning(LogMessages.MissingParameter, "Username");
                    return BadRequest(new { error = ErrorMessages.UsernameEmpty, field = "username" });
                }
                
                if (string.IsNullOrWhiteSpace(command.Password))
                {
                    _logger.LogWarning(LogMessages.MissingParameter, "Password");
                    return BadRequest(new { error = ErrorMessages.PasswordEmpty, field = "password" });
                }
                
                if (string.IsNullOrWhiteSpace(command.Sicil))
                {
                    _logger.LogWarning(LogMessages.MissingParameter, "Sicil");
                    return BadRequest(new { error = ErrorMessages.SicilEmpty, field = "sicil" });
                }
                
                // RoleId kontrol et
                if (!command.RoleId.HasValue || command.RoleId <= 0)
                {
                    _logger.LogWarning(LogMessages.InvalidRoleId, command.RoleId);
                    return BadRequest(new { error = ErrorMessages.ValidRoleRequired, field = "roleId" });
                }

                var result = await _mediator.Send(command);
                _logger.LogInformation(LogMessages.UserCreatedSuccessfully, result.Username, result.Id);
                return CreatedAtAction(nameof(CreateUser), new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                // Sicil benzersizlik hatası veya diğer doğrulama hataları
                string errorMessage = ex.Message;
                _logger.LogWarning(LogMessages.ValidationErrorDuringUserCreation, errorMessage);
                
                // Özel olarak sicil numarası hatası için kontrol et
                if (errorMessage.Contains(ErrorMessages.SicilAlreadyInUsePartial))
                {
                    return BadRequest(new { 
                        error = string.Format(ErrorMessages.SicilConflict, errorMessage),
                        code = "DuplicateSicil",
                        field = "sicil"
                    });
                }
                
                return BadRequest(new { error = errorMessage });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, LogMessages.ErrorDuringUserCreation, ex.Message);
                
                // Daha detaylı hata bilgisi
                if (ex.InnerException != null)
                {
                    _logger.LogError(LogMessages.InnerError, ex.InnerException.Message);
                }
                
                // Hata detaylarını ekleyerek daha açıklayıcı bir yanıt döndür
                return StatusCode(500, new { 
                    error = ErrorMessages.UserCreationError, 
                    details = ex.Message,
                    innerException = ex.InnerException?.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            _logger.LogInformation(LogMessages.PasswordChangeRequestStarted);
            
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                _logger.LogWarning(LogMessages.UserIdNotFoundInToken);
                return Unauthorized(new { message = ErrorMessages.InvalidUserId });
            }

            _logger.LogInformation(string.Format(LogMessages.PasswordChangeRequestForUser, userId));
            
            var result = await _authService.ChangePasswordAsync(changePasswordDto, userId);
            
            if (!result.Success)
            {
                _logger.LogWarning(string.Format(LogMessages.PasswordChangeFailed, userId, result.ErrorMessage));
                return BadRequest(result);
            }
            
            _logger.LogInformation(string.Format(LogMessages.PasswordChangeSuccessful, userId));
            return Ok(result);
        }
    }
} 