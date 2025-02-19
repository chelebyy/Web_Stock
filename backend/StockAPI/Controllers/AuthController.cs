using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockAPI.Data;
using StockAPI.Models;
using StockAPI.Models.DTOs;
using StockAPI.Services;
using StockAPI.Logging;

namespace StockAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtService _jwtService;
        private readonly ILogger<AuthController> _logger;
        private readonly IHashService _hashService;
        private readonly IAuditLogService _auditLogService;

        public AuthController(
            ApplicationDbContext context,
            JwtService jwtService,
            ILogger<AuthController> logger,
            IHashService hashService,
            IAuditLogService auditLogService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _hashService = hashService ?? throw new ArgumentNullException(nameof(hashService));
            _auditLogService = auditLogService ?? throw new ArgumentNullException(nameof(auditLogService));
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                _logger.LogInformation("Login isteği alındı: {@LoginRequest}", loginRequest);

                if (loginRequest == null)
                {
                    _logger.LogWarning("Giriş bilgileri boş");
                    return BadRequest("Giriş bilgileri boş olamaz");
                }

                if (string.IsNullOrEmpty(loginRequest.Username) || string.IsNullOrEmpty(loginRequest.Password))
                {
                    _logger.LogWarning("Kullanıcı adı veya şifre boş");
                    return BadRequest("Kullanıcı adı ve şifre gereklidir");
                }

                var user = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Username == loginRequest.Username)
                    .ConfigureAwait(false);

                if (user == null)
                {
                    _logger.LogWarning("Kullanıcı bulunamadı: {Username}", loginRequest.Username);
                    return Unauthorized(new { message = "Kullanıcı adı veya şifre hatalı" });
                }

                var hashedPassword = _hashService.HashPassword(loginRequest.Password);
                if (user.PasswordHash != hashedPassword)
                {
                    _logger.LogWarning("Hatalı şifre denemesi: {Username}", loginRequest.Username);
                    return Unauthorized(new { message = "Kullanıcı adı veya şifre hatalı" });
                }

                var token = _jwtService.GenerateToken(user);

                await _auditLogService.LogAsync(
                    user.Id,
                    "Login",
                    user.Id.ToString(),
                    null,
                    new { Username = user.Username, LoginTime = DateTime.UtcNow })
                    .ConfigureAwait(false);

                return Ok(new LoginResponse 
                { 
                    Token = token,
                    User = new UserDto 
                    { 
                        Id = user.Id,
                        Username = user.Username,
                        Role = user.Role?.Name ?? "User",
                        IsAdmin = user.IsAdmin,
                        LastLoginAt = user.LastLoginAt
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Giriş işlemi sırasında hata oluştu");
                throw;
            }
        }

        [HttpPost("create-user")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            try
            {
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == request.Username)
                    .ConfigureAwait(false);

                if (existingUser != null)
                {
                    return BadRequest(new { message = "Bu kullanıcı adı zaten kullanılıyor" });
                }

                var user = new User
                {
                    Username = request.Username,
                    PasswordHash = _hashService.HashPassword(request.Password),
                    IsAdmin = request.IsAdmin,
                    RoleId = request.RoleId,
                    CreatedAt = DateTime.UtcNow
                };

                await _context.Users.AddAsync(user).ConfigureAwait(false);
                await _context.SaveChangesAsync().ConfigureAwait(false);

                _logger.UserCreated(user.Username);
                return Ok(new { message = "Kullanıcı başarıyla oluşturuldu" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı oluşturma sırasında hata oluştu");
                return StatusCode(500, new { message = "Bir hata oluştu" });
            }
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

                var user = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Id == int.Parse(userId));

                if (user == null)
                {
                    return NotFound();
                }

                return Ok(new UserResponse
                {
                    Id = user.Id,
                    Username = user.Username,
                    IsAdmin = user.IsAdmin,
                    Role = user.Role?.Name ?? "User",
                    LastLoginAt = user.LastLoginAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
                    CreatedAt = user.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
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

                var currentPasswordHash = _hashService.HashPassword(request.CurrentPassword);
                if (user.PasswordHash != currentPasswordHash)
                {
                    _logger.LogWarning($"Mevcut şifre hatalı - Kullanıcı ID: {userId}");
                    return BadRequest(new { message = "Mevcut şifre hatalı" });
                }

                user.PasswordHash = _hashService.HashPassword(request.NewPassword);
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
        public required string Username { get; set; }
        public required string Password { get; set; }
    }

    public class CreateUserRequest
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public bool IsAdmin { get; set; }
        public int RoleId { get; set; }
    }

    public class LoginResponse
    {
        public required string Token { get; set; }
        public required UserDto User { get; set; }
    }

    public class UserResponse
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public bool IsAdmin { get; set; }
        public required string Role { get; set; }
        public required string CreatedAt { get; set; }
        public required string LastLoginAt { get; set; }
    }

    public class ChangePasswordRequest
    {
        public required string CurrentPassword { get; set; }
        public required string NewPassword { get; set; }
    }
}
