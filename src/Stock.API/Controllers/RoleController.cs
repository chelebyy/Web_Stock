using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stock.Domain.Entities;
using Stock.Infrastructure.Data;
using System.Text.Json;

namespace Stock.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RoleController> _logger;

        public RoleController(ApplicationDbContext context, ILogger<RoleController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetRoles()
        {
            try
            {
                _logger.LogInformation("Tüm roller getiriliyor...");
                
                // Döngüsel referans sorununu önlemek için Users dahil edilmiyor
                var roles = await _context.Roles
                    .AsNoTracking() // Daha iyi performans için
                    .Select(r => new 
                    {
                        r.Id,
                        r.Name,
                        r.Description,
                        r.CreatedAt,
                        r.UpdatedAt,
                        UserCount = r.Users.Count
                    })
                    .ToListAsync();

                _logger.LogInformation($"{roles.Count} rol başarıyla getirildi.");
                return Ok(roles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Roller getirilirken hata oluştu");
                return StatusCode(500, "Roller getirilirken bir hata oluştu. Detaylar: " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetRole(int id)
        {
            try
            {
                _logger.LogInformation($"ID: {id} olan rol getiriliyor...");
                
                var role = await _context.Roles
                    .AsNoTracking() // Daha iyi performans için
                    .Select(r => new
                    {
                        r.Id,
                        r.Name,
                        r.Description,
                        r.CreatedAt,
                        r.UpdatedAt,
                        UserCount = r.Users.Count,
                        Users = r.Users.Select(u => new
                        {
                            u.Id,
                            u.Username,
                            u.Sicil,
                            u.IsAdmin,
                            u.LastLoginAt
                        }).ToList()
                    })
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (role == null)
                {
                    _logger.LogWarning($"ID: {id} olan rol bulunamadı.");
                    return NotFound();
                }

                _logger.LogInformation($"ID: {id} olan rol başarıyla getirildi.");
                return Ok(role);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ID: {id} olan rol getirilirken hata oluştu");
                return StatusCode(500, $"Rol getirilirken bir hata oluştu. Detaylar: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Role>> CreateRole(Role role)
        {
            try
            {
                _logger.LogInformation("Yeni rol oluşturuluyor...");
                _context.Roles.Add(role);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Yeni rol başarıyla oluşturuldu. ID: {role.Id}");
                return CreatedAtAction(nameof(GetRole), new { id = role.Id }, role);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Rol oluşturulurken hata oluştu");
                return StatusCode(500, "Rol oluşturulurken bir hata oluştu. Detaylar: " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, Role role)
        {
            if (id != role.Id)
            {
                _logger.LogWarning($"Route ID {id} ile body ID {role.Id} eşleşmiyor.");
                return BadRequest("Route ve body'deki ID'ler eşleşmiyor.");
            }

            try
            {
                _logger.LogInformation($"ID: {id} olan rol güncelleniyor...");
                
                // Sadece izin verilen alanları güncelle
                var existingRole = await _context.Roles.FindAsync(id);
                if (existingRole == null)
                {
                    _logger.LogWarning($"ID: {id} olan rol bulunamadı.");
                    return NotFound();
                }
                
                // Alanları güncelle
                existingRole.Name = role.Name;
                existingRole.Description = role.Description;
                existingRole.UpdatedAt = DateTime.UtcNow;
                
                await _context.SaveChangesAsync();
                
                _logger.LogInformation($"ID: {id} olan rol başarıyla güncellendi.");
                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!await RoleExists(id))
                {
                    _logger.LogWarning($"ID: {id} olan rol bulunamadı.");
                    return NotFound();
                }
                else
                {
                    _logger.LogError(ex, $"ID: {id} olan rol güncellenirken eşzamanlılık hatası oluştu");
                    return StatusCode(500, "Rol güncellenirken bir eşzamanlılık hatası oluştu.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ID: {id} olan rol güncellenirken hata oluştu");
                return StatusCode(500, "Rol güncellenirken bir hata oluştu. Detaylar: " + ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            try
            {
                _logger.LogInformation($"ID: {id} olan rol siliniyor...");
                
                var role = await _context.Roles.FindAsync(id);
                if (role == null)
                {
                    _logger.LogWarning($"ID: {id} olan rol bulunamadı.");
                    return NotFound();
                }

                // İlgili rolü kullanan kullanıcı var mı kontrol et
                var usersWithRole = await _context.Users.AnyAsync(u => u.RoleId == id);
                if (usersWithRole)
                {
                    _logger.LogWarning($"ID: {id} olan rol, kullanıcılar tarafından kullanılıyor ve silinemez.");
                    return BadRequest("Bu rol, bir veya daha fazla kullanıcı tarafından kullanılıyor ve silinemez.");
                }
                
                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"ID: {id} olan rol başarıyla silindi.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ID: {id} olan rol silinirken hata oluştu");
                return StatusCode(500, "Rol silinirken bir hata oluştu. Detaylar: " + ex.Message);
            }
        }

        private async Task<bool> RoleExists(int id)
        {
            return await _context.Roles.AnyAsync(e => e.Id == id);
        }
    }
} 