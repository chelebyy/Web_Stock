using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockAPI.Data;
using StockAPI.Models;
using StockAPI.Services;
using System.Text;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

namespace StockAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly StockAPI.Data.StockContext _context;
        private readonly JwtService _jwtService;
        private readonly ILogger<AuthController> _logger;
        private readonly HashService _hashService;

        public AuthController(
            StockAPI.Data.StockContext context, 
            JwtService jwtService, 
            ILogger<AuthController> logger,
            HashService hashService)
        {
            _context = context;
            _jwtService = jwtService;
            _logger = logger;
            _hashService = hashService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                _logger.LogInformation("Giriş denemesi - Kullanıcı adı: {Username}", request.Username);

                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == request.Username && !u.IsDeleted);

                if (user == null)
                {
                    _logger.LogWarning($"Kullanıcı bulunamadı: {request.Username}");
                    return Unauthorized(new { message = "Kullanıcı adı veya şifre hatalı" });
                }

                _logger.LogInformation("Kullanıcı bulundu - ID: {Id}, Username: {Username}, IsAdmin: {IsAdmin}", 
                    user.Id, user.Username, user.IsAdmin);

                _logger.LogInformation("Hash Algoritması Detayları:");
                _logger.LogInformation("Gelen şifre: {Password}", request.Password);
                _logger.LogInformation("Hash algoritması: SHA256");
                _logger.LogInformation("Encoding: UTF8");
                
                var hashedPassword = _hashService.ComputeSha256Hash(request.Password);
                _logger.LogInformation("Şifre karşılaştırması:");
                _logger.LogInformation("Gelen şifre hash'i: {Hash}", hashedPassword);
                _logger.LogInformation("DB'deki hash: {Hash}", user.PasswordHash);

                if (hashedPassword != user.PasswordHash)
                {
                    _logger.LogWarning($"Şifre eşleşmedi - Kullanıcı: {request.Username}");
                    return Unauthorized(new { message = "Kullanıcı adı veya şifre hatalı" });
                }

                var token = _jwtService.GenerateToken(user);
                _logger.LogInformation($"Token oluşturuldu: {token}");

                user.LastLoginAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Başarılı giriş: {request.Username}");

                return Ok(new LoginResponse
                {
                    Token = token,
                    User = new UserResponse
                    {
                        Id = user.Id,
                        Username = user.Username,
                        IsAdmin = user.IsAdmin,
                        CreatedAt = user.CreatedAt.ToString("o"),
                        LastLoginAt = user.LastLoginAt?.ToString("o")
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Login işleminde hata: {ex.Message}");
                _logger.LogError($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, new { message = "Sunucu hatası oluştu." });
            }
        }

        [HttpPost("create-first-admin")]
        public async Task<IActionResult> CreateFirstAdmin([FromBody] CreateUserRequest request)
        {
            try
            {
                _logger.LogInformation("İlk admin oluşturma denemesi");

                if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                {
                    _logger.LogWarning("İlk admin oluşturma başarısız: Kullanıcı adı veya şifre boş");
                    return BadRequest(new { message = "Kullanıcı adı ve şifre gereklidir" });
                }

                var existingAdmin = await _context.Users.AnyAsync(u => u.IsAdmin);
                if (existingAdmin)
                {
                    _logger.LogWarning("İlk admin oluşturma başarısız: Zaten bir admin kullanıcısı mevcut!");
                    return BadRequest(new { message = "Zaten bir admin kullanıcısı mevcut!" });
                }

                var hashedPassword = _hashService.ComputeSha256Hash(request.Password);
                _logger.LogInformation($"Oluşturulan şifre hash'i: {hashedPassword}");

                var admin = new User
                {
                    Username = request.Username,
                    PasswordHash = hashedPassword,
                    IsAdmin = true,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Users.Add(admin);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"İlk admin başarıyla oluşturuldu - ID: {admin.Id}, Username: {admin.Username}");

                return Ok(new { message = "İlk admin kullanıcı başarıyla oluşturuldu" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"İlk admin oluşturmada hata: {ex.Message}");
                _logger.LogError($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, new { message = "Sunucu hatası oluştu." });
            }
        }

        [HttpPost("create-user")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { message = "Kullanıcı adı ve şifre gereklidir" });
            }

            if (await _context.Users.AnyAsync(u => u.Username == request.Username))
            {
                return BadRequest(new { message = "Bu kullanıcı adı zaten kullanılıyor" });
            }

            var user = new User
            {
                Username = request.Username,
                PasswordHash = _hashService.ComputeSha256Hash(request.Password),
                IsAdmin = request.IsAdmin,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Kullanıcı başarıyla oluşturuldu" });
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                var userId = User.FindFirst("userId")?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }

                var user = await _context.Users.FindAsync(int.Parse(userId));
                if (user == null)
                {
                    return NotFound();
                }

                return Ok(new UserResponse
                {
                    Id = user.Id,
                    Username = user.Username,
                    IsAdmin = user.IsAdmin
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Get current user error: {ex.Message}");
                return StatusCode(500, new { message = "Sunucu hatası oluştu." });
            }
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("Token'da kullanıcı ID'si bulunamadı");
                    return Unauthorized();
                }

                _logger.LogInformation($"Şifre değiştirme isteği - Kullanıcı ID: {userId}");

                var user = await _context.Users.FindAsync(int.Parse(userId));
                if (user == null)
                {
                    _logger.LogWarning($"Kullanıcı bulunamadı - ID: {userId}");
                    return NotFound();
                }

                var currentPasswordHash = _hashService.ComputeSha256Hash(request.CurrentPassword);
                if (user.PasswordHash != currentPasswordHash)
                {
                    _logger.LogWarning($"Mevcut şifre hatalı - Kullanıcı ID: {userId}");
                    return BadRequest(new { message = "Mevcut şifre hatalı" });
                }

                user.PasswordHash = _hashService.ComputeSha256Hash(request.NewPassword);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Şifre başarıyla değiştirildi - Kullanıcı ID: {userId}");
                return Ok(new { message = "Şifre başarıyla değiştirildi" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Şifre değiştirme hatası: {ex.Message}");
                return StatusCode(500, new { message = "Sunucu hatası oluştu." });
            }
        }
    }

    public class LoginRequest
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class CreateUserRequest
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
    }

    public class LoginResponse
    {
        [Required]
        public string Token { get; set; } = string.Empty;
        [Required]
        public UserResponse User { get; set; } = new();
    }

    public class UserResponse
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
        [Required]
        public string CreatedAt { get; set; } = string.Empty;
        public string? LastLoginAt { get; set; }
    }

    public class ChangePasswordRequest
    {
        [Required]
        public string CurrentPassword { get; set; } = string.Empty;
        [Required]
        public string NewPassword { get; set; } = string.Empty;
    }
}
