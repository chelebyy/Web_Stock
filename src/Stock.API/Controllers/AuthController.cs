using MediatR;
using Microsoft.AspNetCore.Mvc;
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
    [Route("api/[controller]")]
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

        [Authorize(Roles = "Admin")]
        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
        {
            try
            {
                _logger.LogInformation("Yeni kullanıcı oluşturma isteği alındı: {Username}, Sicil: {Sicil}", command.Username, command.Sicil);
                
                // Parametre doğrulamalarını kontrol et
                if (string.IsNullOrWhiteSpace(command.Username))
                {
                    _logger.LogWarning("Eksik parametre: Username boş");
                    return BadRequest(new { error = "Kullanıcı adı boş olamaz", field = "username" });
                }
                
                if (string.IsNullOrWhiteSpace(command.Password))
                {
                    _logger.LogWarning("Eksik parametre: Password boş");
                    return BadRequest(new { error = "Şifre boş olamaz", field = "password" });
                }
                
                if (string.IsNullOrWhiteSpace(command.Sicil))
                {
                    _logger.LogWarning("Eksik parametre: Sicil boş");
                    return BadRequest(new { error = "Sicil numarası boş olamaz", field = "sicil" });
                }
                
                // RoleId kontrol et
                if (!command.RoleId.HasValue || command.RoleId <= 0)
                {
                    _logger.LogWarning("Geçersiz Rol ID: {RoleId}", command.RoleId);
                    return BadRequest(new { error = "Geçerli bir rol seçilmelidir", field = "roleId" });
                }

                var result = await _mediator.Send(command);
                _logger.LogInformation("Kullanıcı başarıyla oluşturuldu: {Username}, ID: {Id}", result.Username, result.Id);
                return CreatedAtAction(nameof(CreateUser), new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                // Sicil benzersizlik hatası veya diğer doğrulama hataları
                string errorMessage = ex.Message;
                _logger.LogWarning("Kullanıcı oluşturma işlemi sırasında doğrulama hatası: {Message}", errorMessage);
                
                // Özel olarak sicil numarası hatası için kontrol et
                if (errorMessage.Contains("sicil numarası zaten kullanılmaktadır"))
                {
                    return BadRequest(new { 
                        error = $"Sicil numarası çakışması: {errorMessage}",
                        code = "DuplicateSicil",
                        field = "sicil"
                    });
                }
                
                return BadRequest(new { error = errorMessage });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı oluşturma sırasında bir hata meydana geldi: {Message}", ex.Message);
                
                // Daha detaylı hata bilgisi
                if (ex.InnerException != null)
                {
                    _logger.LogError("İç hata: {InnerMessage}", ex.InnerException.Message);
                }
                
                // Hata detaylarını ekleyerek daha açıklayıcı bir yanıt döndür
                return StatusCode(500, new { 
                    error = "Kullanıcı oluşturulurken bir hata oluştu", 
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
            _logger.LogInformation("Şifre değiştirme isteği başlatılıyor...");
            
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                _logger.LogWarning("Token'da kullanıcı ID'si bulunamadı veya geçersiz");
                return Unauthorized(new { message = "Geçersiz kullanıcı kimliği" });
            }

            _logger.LogInformation($"Şifre değiştirme isteği - Kullanıcı ID: {userId}");
            
            var result = await _authService.ChangePasswordAsync(changePasswordDto, userId);
            
            if (!result.Success)
            {
                _logger.LogWarning($"Şifre değiştirme başarısız - Kullanıcı ID: {userId}, Hata: {result.ErrorMessage}");
                return BadRequest(result);
            }
            
            _logger.LogInformation($"Şifre başarıyla değiştirildi - Kullanıcı ID: {userId}");
            return Ok(result);
        }
    }
} 