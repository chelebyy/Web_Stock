using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stock.Application.Constants;
using Stock.Application.Common.Interfaces;
using Stock.Application.Models.DTOs;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Stock.Application.Features.Users.Commands;
using System;
using System.Linq;
using Stock.Application.Features.Auth.Commands.Login;
using Stock.Application.Features.Auth.Commands.Register;
using Stock.Application.Features.Auth.Commands.ChangePassword;


namespace Stock.API.Controllers
{
    /// <summary>
    /// Handles user authentication.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AuthController> _logger;

        /// <summary>
        /// AuthController sınıfının yapıcı metodu.
        /// Bağımlılıkları (IMediator, ILogger) enjekte eder.
        /// </summary>
        /// <param name="mediator">CQRS Mediator arayüzü.</param>
        /// <param name="logger">Loglama servisi.</param>
        public AuthController(IMediator mediator, ILogger<AuthController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token.
        /// </summary>
        /// <param name="loginDto">The login request containing user credentials.</param>
        /// <returns>A JWT token if authentication is successful.</returns>
        /// <response code="200">Giriş başarılı. JWT token ve kullanıcı bilgileri döner.</response>
        /// <response code="401">Giriş başarısız. Geçersiz kullanıcı adı/sicil veya şifre.</response>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var command = new LoginCommand 
            { 
                Sicil = loginDto.Sicil, 
                Password = loginDto.Password 
            };
            var result = await _mediator.Send(command);

            if (result == null || !result.Success)
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Yeni kullanıcı kaydı yapar.
        /// </summary>
        /// <param name="registerDto">Kayıt bilgileri (kullanıcı adı, şifre, sicil, rol vb.).</param>
        /// <returns>
        /// Başarılı kayıtta kullanıcı bilgileri içeren bir AuthResponseDto.
        /// Başarısız kayıtta BadRequest (400) yanıtı döner.
        /// </returns>
        /// <response code="200">Kayıt başarılı. Kullanıcı bilgileri döner.</response>
        /// <response code="400">Kayıt başarısız. Geçersiz veya eksik bilgi.</response>
        [HttpPost("register")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (registerDto.Password != registerDto.ConfirmPassword)
            {
                return BadRequest(new AuthResponseDto { Success = false, ErrorMessage = "Şifreler uyuşmuyor." });
            }

            var command = new RegisterCommand
            {
                Adi = registerDto.Adi,
                Soyadi = registerDto.Soyadi,
                Password = registerDto.Password,
                Sicil = registerDto.Sicil,
                RoleId = registerDto.RoleId
            };

            var result = await _mediator.Send(command);

            if (result == null || !result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// Yeni bir kullanıcı oluşturur (Sadece Admin yetkisine sahip kullanıcılar).
        /// </summary>
        /// <param name="command">Kullanıcı oluşturma komutu (kullanıcı adı, şifre, sicil, rol ID vb.).</param>
        /// <returns>
        /// Başarılı oluşturmada yeni kullanıcının bilgilerini içeren bir UserDto.
        /// Hata durumunda BadRequest (400) veya InternalServerError (500) yanıtı döner.
        /// </returns>
        /// <response code="201">Kullanıcı başarıyla oluşturuldu. Oluşturulan kullanıcının detaylarını döner.</response>
        /// <response code="400">İstek geçersiz veya eksik parametre içeriyor (örn. boş kullanıcı adı, geçersiz rol ID, zaten kullanılan sicil).</response>
        /// <response code="401">Yetkisiz erişim (Admin rolü gerekli).</response>
        /// <response code="500">Kullanıcı oluşturma sırasında beklenmeyen bir sunucu hatası oluştu.</response>
        [Authorize(Roles = RoleNames.Admin)]
        [HttpPost("create-user")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)] // Hata nesnesi çeşitli formatlarda olabilir
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)] // Hata nesnesi çeşitli formatlarda olabilir
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
        {
            try
            {
                _logger.LogInformation(LogMessages.UserCreationRequest, $"{command.Adi} {command.Soyadi}", command.Sicil);
                
                // Parametre doğrulamalarını kontrol et
                if (string.IsNullOrWhiteSpace(command.Adi))
                {
                    _logger.LogWarning(LogMessages.MissingParameter, "Adi");
                    return BadRequest(new { error = ErrorMessages.NameEmpty, field = "adi" });
                }
                
                if (string.IsNullOrWhiteSpace(command.Soyadi))
                {
                    _logger.LogWarning(LogMessages.MissingParameter, "Soyadi");
                    return BadRequest(new { error = ErrorMessages.SurnameEmpty, field = "soyadi" });
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

                if (result.IsSuccess)
                {
                    _logger.LogInformation(LogMessages.UserCreatedSuccessfully, $"{result.Value.Adi} {result.Value.Soyadi}", result.Value.Id);
                    return CreatedAtAction(nameof(CreateUser), new { id = result.Value.Id }, result.Value);
                }
                else
                {
                    // Handle MediatR result failure
                     _logger.LogWarning(LogMessages.ValidationErrorDuringUserCreation, result.Error);
                    return BadRequest(new { error = result.Error });
                }
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

        /// <summary>
        /// Giriş yapmış kullanıcının şifresini değiştirir.
        /// </summary>
        /// <param name="changePasswordDto">Eski şifre ve yeni şifre bilgilerini içeren DTO.</param>
        /// <returns>
        /// Başarılı olursa Ok (200) yanıtı.
        /// Başarısız olursa BadRequest (400) veya Unauthorized (401) yanıtı döner.
        /// </returns>
        /// <response code="200">Şifre başarıyla değiştirildi.</response>
        /// <response code="400">İstek geçersiz (örn. eski şifre yanlış, yeni şifre politikaya uymuyor).</response>
        /// <response code="401">Kullanıcı girişi yapılmamış veya token geçersiz.</response>
        [Authorize]
        [HttpPost("change-password")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "uid");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized(new AuthResponseDto { Success = false, ErrorMessage = "Geçersiz token." });
            }

            var command = new ChangePasswordCommand
            {
                UserId = userId,
                CurrentPassword = changePasswordDto.CurrentPassword,
                NewPassword = changePasswordDto.NewPassword
            };

            var result = await _mediator.Send(command);

            if (result == null || !result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
} 