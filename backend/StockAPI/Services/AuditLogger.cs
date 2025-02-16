using Microsoft.EntityFrameworkCore;
using StockAPI.Data;
using StockAPI.Models;

namespace StockAPI.Services;

public class AuditLogger : IAuditLogger
{
    private readonly StockContext _context;
    private readonly ILogger<AuditLogger> _logger;

    public AuditLogger(StockContext context, ILogger<AuditLogger> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task LogActionAsync(string userId, string action, string entityId, string entityType, string path, string? details = null)
    {
        try
        {
            var auditLog = new AuditLog
            {
                UserId = userId,
                Action = action,
                EntityId = entityId,
                EntityType = entityType,
                Path = path,
                Details = details,
                Timestamp = DateTime.UtcNow
            };

            _context.AuditLogs.Add(auditLog);
            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Audit log created: {Action} on {EntityType} {EntityId} by {UserId}",
                action, entityType, entityId, userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error creating audit log: {Action} on {EntityType} {EntityId} by {UserId}",
                action, entityType, entityId, userId);
            throw;
        }
    }

    public async Task<IEnumerable<AuditLog>> GetUserActionsAsync(string userId, DateTime? startDate = null, DateTime? endDate = null)
    {
        try
        {
            var query = _context.AuditLogs.AsNoTracking()
                .Where(log => log.UserId == userId);

            if (startDate.HasValue)
                query = query.Where(log => log.Timestamp >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(log => log.Timestamp <= endDate.Value);

            return await query.OrderByDescending(log => log.Timestamp).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving audit logs for user {UserId}", userId);
            throw;
        }
    }
}
