using StockAPI.Models;

namespace StockAPI.Services;

public interface IAuditLogger
{
    Task LogActionAsync(string userId, string action, string entityId, string entityType, string path, string? details = null);
    Task<IEnumerable<AuditLog>> GetUserActionsAsync(string userId, DateTime? startDate = null, DateTime? endDate = null);
}
