using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockAPI.Data;
using StockAPI.Models;
using StockAPI.Services;
using System.Security.Claims;

namespace StockAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class RoleController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IAuditLogService _auditLogService;
    private readonly ILogger<RoleController> _logger;

    private static readonly Action<ILogger, string, Exception?> _logRoleCreationError =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(1, nameof(CreateRole)),
            "Rol oluşturulurken hata oluştu - Name: {RoleName}");

    private static readonly Action<ILogger, int, Exception?> _logRoleUpdateError =
        LoggerMessage.Define<int>(
            LogLevel.Error,
            new EventId(2, nameof(UpdateRole)),
            "Rol güncellenirken hata oluştu - ID: {RoleId}");

    private static readonly Action<ILogger, int, Exception?> _logRoleDeleteError =
        LoggerMessage.Define<int>(
            LogLevel.Error,
            new EventId(3, nameof(DeleteRole)),
            "Rol silinirken hata oluştu - ID: {RoleId}");

    public RoleController(
        ApplicationDbContext context,
        IAuditLogService auditLogService,
        ILogger<RoleController> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _auditLogService = auditLogService ?? throw new ArgumentNullException(nameof(auditLogService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoleDto>>> GetRoles()
    {
        try
        {
            var roles = await _context.Roles
                .Include(r => r.Users)
                .Select(r => new RoleDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    CreatedAt = r.CreatedAt,
                    UserCount = r.Users.Count
                })
                .ToListAsync();

            return Ok(roles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Roller listelenirken hata oluştu");
            return StatusCode(500, new { message = "Roller listelenirken bir hata oluştu" });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RoleDetailDto>> GetRole(int id)
    {
        try
        {
            var role = await _context.Roles
                .Include(r => r.Users)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (role == null)
            {
                return NotFound(new { message = "Rol bulunamadı" });
            }

            return Ok(new RoleDetailDto
            {
                Id = role.Id,
                Name = role.Name,
                CreatedAt = role.CreatedAt,
                Users = role.Users.Select(u => new UserInRoleDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    IsAdmin = u.IsAdmin,
                    CreatedAt = u.CreatedAt
                }).ToList()
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Rol getirilirken hata oluştu - ID: {RoleId}", id);
            return StatusCode(500, new { message = "Rol getirilirken bir hata oluştu" });
        }
    }

    [HttpPost]
    public async Task<ActionResult<Role>> CreateRole([FromBody] CreateRoleDto createRoleDto)
    {
        ArgumentNullException.ThrowIfNull(createRoleDto);

        try
        {
            var existingRole = await _context.Roles
                .FirstOrDefaultAsync(r => r.Name == createRoleDto.Name)
                .ConfigureAwait(false);

            if (existingRole != null)
            {
                return BadRequest(new { message = "Bu rol adı zaten kullanılıyor" });
            }

            var role = new Role
            {
                Name = createRoleDto.Name,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Roles.AddAsync(role).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(currentUserId))
            {
                await _auditLogService.LogAsync<Role>(
                    int.Parse(currentUserId),
                    "CreateRole",
                    role.Id.ToString(),
                    null,
                    role).ConfigureAwait(false);
            }

            return CreatedAtAction(nameof(GetRole), new { id = role.Id }, role);
        }
        catch (DbUpdateException ex)
        {
            _logRoleCreationError(_logger, createRoleDto.Name, ex);
            return StatusCode(500, new { message = "Veritabanı hatası oluştu" });
        }
        catch (Exception ex)
        {
            _logRoleCreationError(_logger, createRoleDto.Name, ex);
            return StatusCode(500, new { message = "Rol oluşturulurken bir hata oluştu" });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRole(int id, [FromBody] UpdateRoleDto updateRoleDto)
    {
        ArgumentNullException.ThrowIfNull(updateRoleDto);

        try
        {
            var role = await _context.Roles.FindAsync(id).ConfigureAwait(false);
            if (role == null)
            {
                return NotFound(new { message = "Rol bulunamadı" });
            }

            if (role.Name == "Admin")
            {
                return BadRequest(new { message = "Admin rolü güncellenemez" });
            }

            var oldRole = new { role.Name };

            role.Name = updateRoleDto.Name;
            await _context.SaveChangesAsync().ConfigureAwait(false);

            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(currentUserId))
            {
                await _auditLogService.LogAsync<object>(
                    int.Parse(currentUserId),
                    "UpdateRole",
                    id.ToString(),
                    oldRole,
                    new { role.Name }).ConfigureAwait(false);
            }

            return Ok(new { message = "Rol başarıyla güncellendi" });
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logRoleUpdateError(_logger, id, ex);
            return StatusCode(409, new { message = "Rol başka bir kullanıcı tarafından güncellenmiş olabilir" });
        }
        catch (DbUpdateException ex)
        {
            _logRoleUpdateError(_logger, id, ex);
            return StatusCode(500, new { message = "Veritabanı hatası oluştu" });
        }
        catch (Exception ex)
        {
            _logRoleUpdateError(_logger, id, ex);
            return StatusCode(500, new { message = "Rol güncellenirken bir hata oluştu" });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRole(int id)
    {
        try
        {
            var role = await _context.Roles
                .Include(r => r.Users)
                .FirstOrDefaultAsync(r => r.Id == id)
                .ConfigureAwait(false);

            if (role == null)
            {
                return NotFound(new { message = "Rol bulunamadı" });
            }

            if (role.Name == "Admin")
            {
                return BadRequest(new { message = "Admin rolü silinemez" });
            }

            if (role.Users.Any())
            {
                return BadRequest(new { message = "Bu role sahip kullanıcılar var. Önce kullanıcıların rollerini değiştirin." });
            }

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(currentUserId))
            {
                await _auditLogService.LogAsync<Role>(
                    int.Parse(currentUserId),
                    "DeleteRole",
                    id.ToString(),
                    role,
                    null).ConfigureAwait(false);
            }

            return Ok(new { message = "Rol başarıyla silindi" });
        }
        catch (DbUpdateException ex)
        {
            _logRoleDeleteError(_logger, id, ex);
            return StatusCode(500, new { message = "Veritabanı hatası oluştu" });
        }
        catch (Exception ex)
        {
            _logRoleDeleteError(_logger, id, ex);
            return StatusCode(500, new { message = "Rol silinirken bir hata oluştu" });
        }
    }
}

public class RoleDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public int UserCount { get; set; }
}

public class RoleDetailDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public required List<UserInRoleDto> Users { get; set; }
}

public class UserInRoleDto
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public bool IsAdmin { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateRoleDto
{
    public required string Name { get; set; }
}

public class UpdateRoleDto
{
    public required string Name { get; set; }
} 