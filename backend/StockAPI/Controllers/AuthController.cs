using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockAPI.Data;
using StockAPI.Models;
using StockAPI.Services;
using System.Text;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;

namespace StockAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly StockAPI.Data.StockContext _context;
        private readonly JwtService _jwtService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(StockAPI.Data.StockContext context, JwtService jwtService, ILogger<AuthController> logger)
        {
            _context = context;
            _jwtService = jwtService;
            _logger = logger;
        }

        private string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                return Convert.ToBase64String(bytes);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                _logger.LogInformation($"Giriş denemesi - Kullanıcı adı: {request.Username}");

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

                if (user == null)
                {
                    _logger.LogWarning($"Kullanıcı bulunamadı: {request.Username}");
                    return Unauthorized(new { message = "Kullanıcı adı veya şifre hatalı" });
                }

                _logger.LogInformation($"Kullanıcı bulundu - ID: {user.Id}, Username: {user.Username}, IsAdmin: {user.IsAdmin}");

                var hashedPassword = ComputeSha256Hash(request.Password);
                _logger.LogInformation($"Şifre karşılaştırması:");
                _logger.LogInformation($"Gelen şifre hash'i: {hashedPassword}");
                _logger.LogInformation($"DB'deki hash: {user.PasswordHash}");

                if (user.PasswordHash != hashedPassword)
                {
                    _logger.LogWarning($"Şifre eşleşmedi - Kullanıcı: {request.Username}");
                    return Unauthorized(new { message = "Kullanıcı adı veya şifre hatalı" });
                }

                var token = _jwtService.GenerateToken(user.Id.ToString(), user.Username, new List<string> { user.IsAdmin ? "Admin" : "User" });
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

                var hashedPassword = ComputeSha256Hash(request.Password);
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
                PasswordHash = ComputeSha256Hash(request.Password),
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
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class CreateUserRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
        public UserResponse User { get; set; }
    }

    public class UserResponse
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public bool IsAdmin { get; set; }
        public string CreatedAt { get; set; }
        public string LastLoginAt { get; set; }
    }
}
