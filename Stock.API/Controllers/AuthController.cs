using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stock.Application.Common.Interfaces;
using Stock.Application.Models.DTOs;
using System.Threading.Tasks;
using BCrypt.Net;

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
            try
            {
                _logger.LogInformation($"Login isteği alındı: {loginDto.Username}");
                
                var result = await _authService.LoginAsync(loginDto);
                
                if (!result.Success)
                {
                    _logger.LogWarning($"Login başarısız: {loginDto.Username}, Hata: {result.ErrorMessage}");
                    return Unauthorized(result);
                }
                
                _logger.LogInformation($"Login başarılı: {loginDto.Username}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Login hatası: {ex.Message}");
                return StatusCode(500, new { success = false, message = "Sunucu hatası oluştu." });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                _logger.LogInformation($"Kayıt isteği alındı: {registerDto.Username}");
                
                var result = await _authService.RegisterAsync(registerDto);
                
                if (!result.Success)
                {
                    _logger.LogWarning($"Kayıt başarısız: {registerDto.Username}, Hata: {result.ErrorMessage}");
                    return BadRequest(result);
                }
                
                _logger.LogInformation($"Kayıt başarılı: {registerDto.Username}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Kayıt hatası: {ex.Message}");
                return StatusCode(500, new { success = false, message = "Sunucu hatası oluştu." });
            }
        }
    }
} 