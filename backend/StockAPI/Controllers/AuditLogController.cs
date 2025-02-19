using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockAPI.Data;
using StockAPI.Models;

namespace StockAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AuditLogController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AuditLogController> _logger;

    public AuditLogController(
        ApplicationDbContext context,
        ILogger<AuditLogController> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuditLogDto>>> GetAuditLogs(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] int? userId,
        [FromQuery] string? action,
        [FromQuery] string? entityType,
        [FromQuery] string? entityId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            var query = _context.AuditLogs
                .Include(a => a.User)
                .AsQueryable();

            if (startDate.HasValue)
            {
                query = query.Where(a => a.Timestamp >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(a => a.Timestamp <= endDate.Value);
            }

            if (userId.HasValue)
            {
                query = query.Where(a => a.UserId == userId.Value);
            }

            if (!string.IsNullOrEmpty(action))
            {
                query = query.Where(a => a.Action == action);
            }

            if (!string.IsNullOrEmpty(entityType))
            {
                query = query.Where(a => a.EntityType == entityType);
            }

            if (!string.IsNullOrEmpty(entityId))
            {
                query = query.Where(a => a.EntityId == entityId);
            }

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var auditLogs = await query
                .OrderByDescending(a => a.Timestamp)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new AuditLogDto
                {
                    Id = a.Id,
                    UserId = a.UserId,
                    Username = a.User != null ? a.User.Username : string.Empty,
                    Action = a.Action,
                    EntityType = a.EntityType,
                    EntityId = a.EntityId,
                    Path = a.Path,
                    Timestamp = a.Timestamp,
                    OldData = a.OldData,
                    NewData = a.NewData,
                    Details = a.Details,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt
                })
                .ToListAsync();

            var response = new PaginatedResponse<AuditLogDto>
            {
                Items = auditLogs,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Denetim kayıtları listelenirken hata oluştu");
            return StatusCode(500, new { message = "Denetim kayıtları listelenirken bir hata oluştu" });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AuditLogDto>> GetAuditLog(int id)
    {
        try
        {
            var auditLog = await _context.AuditLogs
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (auditLog == null)
            {
                return NotFound(new { message = "Denetim kaydı bulunamadı" });
            }

            return Ok(new AuditLogDto
            {
                Id = auditLog.Id,
                UserId = auditLog.UserId,
                Username = auditLog.User != null ? auditLog.User.Username : string.Empty,
                Action = auditLog.Action,
                EntityType = auditLog.EntityType,
                EntityId = auditLog.EntityId,
                Path = auditLog.Path,
                Timestamp = auditLog.Timestamp,
                OldData = auditLog.OldData,
                NewData = auditLog.NewData,
                Details = auditLog.Details,
                CreatedAt = auditLog.CreatedAt,
                UpdatedAt = auditLog.UpdatedAt
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Denetim kaydı getirilirken hata oluştu - ID: {AuditLogId}", id);
            return StatusCode(500, new { message = "Denetim kaydı getirilirken bir hata oluştu" });
        }
    }
}

public class AuditLogDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public string EntityId { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string? OldData { get; set; }
    public string? NewData { get; set; }
    public string? Details { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class PaginatedResponse<T>
{
    public List<T> Items { get; set; } = new();
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
} 