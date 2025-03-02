using Microsoft.AspNetCore.Mvc;
using Stock.Domain.Interfaces;
using Stock.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Stock.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FixPasswordController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<FixPasswordController> _logger;

        public FixPasswordController(
            ApplicationDbContext context,
            IPasswordHasher passwordHasher,
            ILogger<FixPasswordController> logger)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        [HttpGet("fix-admin")]
        public async Task<IActionResult> FixAdminPassword()
        {
            try
            {
                var admin = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == "admin");

                if (admin == null)
                {
                    _logger.LogWarning("Admin kullanıcısı bulunamadı");
                    return NotFound("Admin kullanıcısı bulunamadı");
                }

                // Admin123 şifresini BCrypt ile hashleme
                admin.PasswordHash = _passwordHasher.HashPassword("Admin123");
                
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("Admin şifresi başarıyla güncellendi");
                return Ok("Admin şifresi başarıyla güncellendi");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Admin şifresi güncellenirken hata oluştu: {ex.Message}");
                return StatusCode(500, "Bir hata oluştu");
            }
        }
    }
} 