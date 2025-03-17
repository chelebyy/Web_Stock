using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stock.API.Constants;
using Stock.Domain.Entities;
using Stock.Infrastructure.Data;
using System.Text.Json;

namespace Stock.API.Controllers
{
    [ApiController]
    [Route(ApiConstants.ApiBaseRoute + "/[controller]")]
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
                _logger.LogInformation(LogMessages.RoleCreating);
                
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

                _logger.LogInformation(string.Format("Toplam {0} rol başarıyla getirildi.", roles.Count));
                return Ok(roles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, LogMessages.RoleCreating);
                return StatusCode(500, string.Format(ErrorMessages.RoleCreateError, ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetRole(int id)
        {
            try
            {
                _logger.LogInformation(string.Format("ID: {0} olan rol getiriliyor...", id));
                
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
                    _logger.LogWarning(string.Format("ID: {0} olan rol bulunamadı.", id));
                    return NotFound(ErrorMessages.RoleNotFound);
                }

                _logger.LogInformation(string.Format("ID: {0} olan rol başarıyla getirildi.", id));
                return Ok(role);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format("ID: {0} olan rol getirilirken hata oluştu", id));
                return StatusCode(500, string.Format(ErrorMessages.RoleCreateError, ex.Message));
            }
        }

        [HttpPost]
        public async Task<ActionResult<Role>> CreateRole(Role role)
        {
            try
            {
                _logger.LogInformation(LogMessages.RoleCreating);
                _context.Roles.Add(role);
                await _context.SaveChangesAsync();

                _logger.LogInformation(string.Format(LogMessages.RoleCreated, role.Name, role.Id));
                return CreatedAtAction(nameof(GetRole), new { id = role.Id }, role);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Rol oluşturulurken hata oluştu");
                return StatusCode(500, string.Format(ErrorMessages.RoleCreateError, ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, Role role)
        {
            if (id != role.Id)
            {
                _logger.LogWarning(string.Format("Route ID {0} ile body ID {1} eşleşmiyor.", id, role.Id));
                return BadRequest("Route ve body'deki ID'ler eşleşmiyor.");
            }

            try
            {
                _logger.LogInformation(string.Format(LogMessages.RoleUpdating, id));
                
                // Sadece izin verilen alanları güncelle
                var existingRole = await _context.Roles.FindAsync(id);
                if (existingRole == null)
                {
                    _logger.LogWarning(string.Format("ID: {0} olan rol bulunamadı.", id));
                    return NotFound(ErrorMessages.RoleNotFound);
                }
                
                // Alanları güncelle
                existingRole.Name = role.Name;
                existingRole.Description = role.Description;
                existingRole.UpdatedAt = DateTime.UtcNow;
                
                await _context.SaveChangesAsync();
                
                _logger.LogInformation(string.Format(LogMessages.RoleUpdated, id));
                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!await RoleExists(id))
                {
                    _logger.LogWarning(string.Format("ID: {0} olan rol bulunamadı.", id));
                    return NotFound(ErrorMessages.RoleNotFound);
                }
                else
                {
                    _logger.LogError(ex, string.Format("ID: {0} olan rol güncellenirken eşzamanlılık hatası oluştu", id));
                    return StatusCode(500, "Rol güncellenirken bir eşzamanlılık hatası oluştu.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format("ID: {0} olan rol güncellenirken hata oluştu", id));
                return StatusCode(500, string.Format(ErrorMessages.RoleUpdateError, ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            try
            {
                _logger.LogInformation(string.Format(LogMessages.RoleDeleting, id));
                
                var role = await _context.Roles.FindAsync(id);
                if (role == null)
                {
                    _logger.LogWarning(string.Format("ID: {0} olan rol bulunamadı.", id));
                    return NotFound(ErrorMessages.RoleNotFound);
                }

                // İlgili rolü kullanan kullanıcı var mı kontrol et
                var usersWithRole = await _context.Users.AnyAsync(u => u.RoleId == id);
                if (usersWithRole)
                {
                    _logger.LogWarning(string.Format("ID: {0} olan rol, kullanıcılar tarafından kullanılıyor ve silinemez.", id));
                    return BadRequest("Bu rol, bir veya daha fazla kullanıcı tarafından kullanılıyor ve silinemez.");
                }
                
                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();

                _logger.LogInformation(string.Format(LogMessages.RoleDeleted, id));
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format("ID: {0} olan rol silinirken hata oluştu", id));
                return StatusCode(500, string.Format(ErrorMessages.RoleDeleteError, ex.Message));
            }
        }

        private async Task<bool> RoleExists(int id)
        {
            return await _context.Roles.AnyAsync(e => e.Id == id);
        }
    }
} 