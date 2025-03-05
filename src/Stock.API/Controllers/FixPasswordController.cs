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

        [HttpGet("fix-users")]
        public async Task<IActionResult> FixUsers()
        {
            // Admin kullanıcısına sicil numarası ekle
            var adminUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == "admin");
            if (adminUser != null)
            {
                adminUser.Sicil = "A001";
                await _context.SaveChangesAsync();
            }

            // User kullanıcısına sicil numarası ekle
            var normalUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == "user");
            if (normalUser != null)
            {
                normalUser.Sicil = "U001";
                await _context.SaveChangesAsync();
            }

            return Ok(new { message = "Kullanıcılara sicil numarası eklendi. Admin: A001, User: U001" });
        }

        [HttpGet("fix-passwords")]
        public async Task<IActionResult> FixPasswords()
        {
            try
            {
                _logger.LogInformation("Kullanıcı şifrelerini ve sicil alanlarını düzeltme işlemi başlatıldı");

                // Admin kullanıcısının şifresini ve sicil alanını düzelt
                var adminUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == "admin");
                if (adminUser != null)
                {
                    var adminPassword = "admin123";
                    adminUser.PasswordHash = _passwordHasher.HashPassword(adminPassword);
                    adminUser.Sicil = "A001";
                    _logger.LogInformation($"Admin kullanıcısının şifresi güncellendi. Yeni hash: {adminUser.PasswordHash}");
                    _logger.LogInformation($"Admin kullanıcısının sicil alanı güncellendi: {adminUser.Sicil}");
                    await _context.SaveChangesAsync();
                }
                else
                {
                    _logger.LogWarning("Admin kullanıcısı bulunamadı");
                }

                // Normal kullanıcının şifresini ve sicil alanını düzelt
                var normalUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == "user");
                if (normalUser != null)
                {
                    var userPassword = "user123";
                    normalUser.PasswordHash = _passwordHasher.HashPassword(userPassword);
                    normalUser.Sicil = "U001";
                    _logger.LogInformation($"User kullanıcısının şifresi güncellendi. Yeni hash: {normalUser.PasswordHash}");
                    _logger.LogInformation($"User kullanıcısının sicil alanı güncellendi: {normalUser.Sicil}");
                    await _context.SaveChangesAsync();
                }
                else
                {
                    _logger.LogWarning("User kullanıcısı bulunamadı, oluşturuluyor...");
                    
                    // User kullanıcısı yoksa oluştur
                    var userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "User");
                    if (userRole == null)
                    {
                        _logger.LogWarning("User rolü bulunamadı, oluşturuluyor...");
                        userRole = new Domain.Entities.Role
                        {
                            Name = "User",
                            Description = "Normal kullanıcı",
                            CreatedAt = DateTime.UtcNow
                        };
                        await _context.Roles.AddAsync(userRole);
                        await _context.SaveChangesAsync();
                    }
                    
                    var newUser = new Domain.Entities.User
                    {
                        Username = "user",
                        PasswordHash = _passwordHasher.HashPassword("user123"),
                        IsAdmin = false,
                        RoleId = userRole.Id,
                        Sicil = "U001",
                        CreatedAt = DateTime.UtcNow
                    };
                    
                    await _context.Users.AddAsync(newUser);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("User kullanıcısı başarıyla oluşturuldu");
                }

                return Ok(new {
                    message = "Kullanıcı şifreleri ve sicil alanları başarıyla güncellendi.",
                    adminInfo = "Kullanıcı adı: admin, Sicil: A001, Şifre: admin123",
                    userInfo = "Kullanıcı adı: user, Sicil: U001, Şifre: user123"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Şifre ve sicil alanları güncellenirken hata oluştu: {ex.Message}");
                return StatusCode(500, "Bir hata oluştu: " + ex.Message);
            }
        }
    }
} 