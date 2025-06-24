using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stock.Infrastructure.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AdminController> _logger;

        public AdminController(ApplicationDbContext context, ILogger<AdminController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpDelete("cleanup-unused-permissions")]
        public async Task<IActionResult> CleanupUnusedPermissions()
        {
            try
            {
                _logger.LogInformation("Kullanılmayan sayfa izinleri temizleniyor...");

                // Reports sayfası izinleri
                var reportsPermission = await _context.Permissions.FirstOrDefaultAsync(p => p.Name == "Pages.Reports");
                if (reportsPermission != null)
                {
                    await RemovePermission(reportsPermission.Id, "Pages.Reports");
                }

                // Settings sayfası izinleri
                var settingsPermission = await _context.Permissions.FirstOrDefaultAsync(p => p.Name == "Pages.Settings");
                if (settingsPermission != null)
                {
                    await RemovePermission(settingsPermission.Id, "Pages.Settings");
                }

                // Eğitim sayfası izinleri
                var egitimPermissions = await _context.Permissions.Where(p => p.Name.StartsWith("Pages.Egitim")).ToListAsync();
                foreach (var permission in egitimPermissions)
                {
                    await RemovePermission(permission.Id, permission.Name);
                }

                // StockManagement sayfası izinleri
                var stockManagementPermission = await _context.Permissions.FirstOrDefaultAsync(p => p.Name == "Pages.StockManagement");
                if (stockManagementPermission != null)
                {
                    await RemovePermission(stockManagementPermission.Id, "Pages.StockManagement");
                }

                // Stok Yönetimi grubundaki tüm izinler
                var stockPermissions = await _context.Permissions.Where(p => p.Group == "Stok Yönetimi").ToListAsync();
                foreach (var permission in stockPermissions)
                {
                    await RemovePermission(permission.Id, permission.Name);
                }

                _logger.LogInformation("Kullanılmayan sayfa izinleri başarıyla temizlendi.");
                return Ok(new { message = "Kullanılmayan sayfa izinleri başarıyla temizlendi." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "İzinler temizlenirken bir hata oluştu");
                return StatusCode(500, "İzinler temizlenirken bir hata oluştu: " + ex.Message);
            }
        }

        private async Task RemovePermission(int permissionId, string permissionName)
        {
            // Kullanıcı izinlerini temizle
            var userPermissions = await _context.UserPermissions
                .Where(up => up.PermissionId == permissionId)
                .ToListAsync();
            
            if (userPermissions.Any())
            {
                _context.RemoveRange(userPermissions);
                _logger.LogInformation($"'{permissionName}' ile ilişkili {userPermissions.Count} kullanıcı izni kaldırıldı.");
            }

            // Rol izinlerini temizle
            var rolePermissions = await _context.RolePermissions
                .Where(rp => rp.PermissionId == permissionId)
                .ToListAsync();
            
            if (rolePermissions.Any())
            {
                _context.RemoveRange(rolePermissions);
                _logger.LogInformation($"'{permissionName}' ile ilişkili {rolePermissions.Count} rol izni kaldırıldı.");
            }

            // İzni sil
            var permission = await _context.Permissions.FindAsync(permissionId);
            if (permission != null)
            {
                _context.Permissions.Remove(permission);
                _logger.LogInformation($"'{permissionName}' izni kaldırıldı.");
            }

            await _context.SaveChangesAsync();
        }
    }
} 