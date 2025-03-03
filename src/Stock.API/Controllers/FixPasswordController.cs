using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stock.Infrastructure.Data;
using Stock.Domain.Interfaces;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;

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
                _logger.LogInformation("Admin şifresini düzeltme işlemi başlatıldı");
                
                var admin = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == "admin" && !u.IsDeleted);

                if (admin == null)
                {
                    _logger.LogWarning("Admin kullanıcısı bulunamadı");
                    return NotFound("Admin kullanıcısı bulunamadı.");
                }

                _logger.LogInformation($"Admin kullanıcısı bulundu. ID: {admin.Id}");
                _logger.LogInformation($"Mevcut hash: {admin.PasswordHash}");

                // IPasswordHasher kullanarak yeni şifreyi hash'liyoruz
                var newPassword = "Admin123";
                var newPasswordHash = _passwordHasher.HashPassword(newPassword);

                _logger.LogInformation($"Yeni hash oluşturuldu: {newPasswordHash}");

                // Yeni hash'i atıyoruz
                admin.PasswordHash = newPasswordHash;
                
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