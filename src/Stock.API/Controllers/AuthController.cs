using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stock.Application.Common.Interfaces;
using Stock.Application.Models.DTOs;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace Stock.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
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