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

namespace Stock.API.Controllers
{
    /// <summary>
    /// Kullanıcı kimlik doğrulama ve yetkilendirme işlemlerini yöneten API Controller.
    /// Login, register, kullanıcı oluşturma ve şifre değiştirme gibi endpoint'leri içerir.
    /// </summary>
    [ApiController]
    [Route(ApiConstants.ApiBaseRoute + "/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        private readonly IMediator _mediator;

        /// <summary>
        /// AuthController sınıfının yapıcı metodu.
        /// Bağımlılıkları (IAuthService, ILogger, IMediator) enjekte eder.
        /// </summary>
        /// <param name="authService">Kimlik doğrulama işlemleri servisi.</param>
        /// <param name="logger">Loglama servisi.</param>
        /// <param name="mediator">CQRS Mediator arayüzü.</param>
        public AuthController(IAuthService authService, ILogger<AuthController> logger, IMediator mediator)
        {
            _authService = authService;
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        /// Kullanıcı girişi yapar.
        /// </summary>
        /// <param name="loginDto">Giriş bilgileri (kullanıcı adı/sicil ve şifre).</param>
        /// <returns>
        /// Başarılı girişte JWT token ve kullanıcı bilgileri içeren bir AuthResponseDto.
        /// Başarısız girişte Unauthorized (401) yanıtı döner.
        /// </returns>
        /// <response code="200">Giriş başarılı. JWT token ve kullanıcı bilgileri döner.</response>
        /// <response code="401">Giriş başarısız. Geçersiz kullanıcı adı/sicil veya şifre.</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto);
            
            if (!result.Success)
                return Unauthorized(result);
                
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
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var result = await _authService.RegisterAsync(registerDto);
            
            if (!result.Success)
                return BadRequest(result);
                
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
                _logger.LogInformation(LogMessages.UserCreatedSuccessfully, $"{result.Adi} {result.Soyadi}", result.Id);
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
        [ProducesResponseType(typeof(object), StatusCodes.Status401Unauthorized)]
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