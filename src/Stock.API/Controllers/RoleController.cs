using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stock.Domain.Entities;
using Stock.Infrastructure.Data;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace Stock.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RoleController> _logger;

        // Sabit hata mesajları
        private const string ERROR_ROLE_NOT_FOUND = "ID: {0} olan rol bulunamadı.";
        private const string ERROR_ROLE_CREATE = "Rol oluşturulurken bir hata oluştu. Detaylar: {0}";
        private const string ERROR_ROLE_UPDATE = "Rol güncellenirken bir hata oluştu. Detaylar: {0}";
        private const string ERROR_ROLE_DELETE = "Rol silinirken bir hata oluştu. Detaylar: {0}";
        private const string ERROR_ROLE_FETCH = "Roller getirilirken bir hata oluştu. Detaylar: {0}";
        private const string ERROR_ROLE_FETCH_BY_ID = "ID: {0} olan rol getirilirken hata oluştu. Detaylar: {1}";
        private const string ERROR_ID_MISMATCH = "Route ve body'deki ID'ler eşleşmiyor.";
        private const string ERROR_ROLE_IN_USE = "Bu rol, bir veya daha fazla kullanıcı tarafından kullanılıyor ve silinemez.";
        private const string ERROR_CONCURRENCY = "Rol güncellenirken bir eşzamanlılık hatası oluştu.";

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

                _logger.LogInformation("Roller getirildi. Toplam: {Count}", roles.Count);
                return Ok(roles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Roller getirilirken hata oluştu");
                return StatusCode(StatusCodes.Status500InternalServerError, string.Format(ERROR_ROLE_FETCH, ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetRole(int id)
        {
            try
            {
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
                    _logger.LogWarning("ID: {Id} olan rol bulunamadı", id);
                    return NotFound(new { error = string.Format(ERROR_ROLE_NOT_FOUND, id) });
                }

                return Ok(role);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ID: {Id} olan rol getirilirken hata oluştu", id);
                return StatusCode(StatusCodes.Status500InternalServerError, string.Format(ERROR_ROLE_FETCH_BY_ID, id, ex.Message));
            }
        }

        [HttpPost]
        public async Task<ActionResult<Role>> CreateRole(Role role)
        {
            try
            {
                _context.Roles.Add(role);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Yeni rol oluşturuldu. ID: {Id}", role.Id);
                return CreatedAtAction(nameof(GetRole), new { id = role.Id }, role);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Rol oluşturulurken hata oluştu");
                return StatusCode(StatusCodes.Status500InternalServerError, string.Format(ERROR_ROLE_CREATE, ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, Role role)
        {
            if (id != role.Id)
            {
                _logger.LogWarning("Route ID {RouteId} ile body ID {BodyId} eşleşmiyor", id, role.Id);
                return BadRequest(ERROR_ID_MISMATCH);
            }

            try
            {
                // Sadece izin verilen alanları güncelle
                var existingRole = await _context.Roles.FindAsync(id);
                if (existingRole == null)
                {
                    _logger.LogWarning("ID: {Id} olan rol bulunamadı", id);
                    return NotFound(new { error = string.Format(ERROR_ROLE_NOT_FOUND, id) });
                }
                
                // Alanları güncelle
                existingRole.Name = role.Name;
                existingRole.Description = role.Description;
                existingRole.UpdatedAt = DateTime.UtcNow;
                
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("ID: {Id} olan rol güncellendi", id);
                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!await RoleExists(id))
                {
                    _logger.LogWarning("ID: {Id} olan rol bulunamadı", id);
                    return NotFound(new { error = string.Format(ERROR_ROLE_NOT_FOUND, id) });
                }
                else
                {
                    _logger.LogError(ex, "ID: {Id} olan rol güncellenirken eşzamanlılık hatası oluştu", id);
                    return StatusCode(StatusCodes.Status500InternalServerError, ERROR_CONCURRENCY);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ID: {Id} olan rol güncellenirken hata oluştu", id);
                return StatusCode(StatusCodes.Status500InternalServerError, string.Format(ERROR_ROLE_UPDATE, ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            try
            {
                var role = await _context.Roles.FindAsync(id);
                if (role == null)
                {
                    _logger.LogWarning("ID: {Id} olan rol bulunamadı", id);
                    return NotFound(new { error = string.Format(ERROR_ROLE_NOT_FOUND, id) });
                }

                // İlgili rolü kullanan kullanıcı var mı kontrol et
                var usersWithRole = await _context.Users.AnyAsync(u => u.RoleId == id);
                if (usersWithRole)
                {
                    _logger.LogWarning("ID: {Id} olan rol, kullanıcılar tarafından kullanılıyor ve silinemez", id);
                    return BadRequest(new { error = ERROR_ROLE_IN_USE });
                }
                
                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();

                _logger.LogInformation("ID: {Id} olan rol silindi", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ID: {Id} olan rol silinirken hata oluştu", id);
                return StatusCode(StatusCodes.Status500InternalServerError, string.Format(ERROR_ROLE_DELETE, ex.Message));
            }
        }

        private async Task<bool> RoleExists(int id)
        {
            return await _context.Roles.AnyAsync(e => e.Id == id);
        }
    }
} 