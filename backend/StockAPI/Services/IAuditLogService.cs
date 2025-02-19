using StockAPI.Models;

namespace StockAPI.Services
{
    public interface IAuditLogService
    {
        Task LogAsync<TData>(
            int userId,
            string action,
            string entityId,
            TData? oldData = default,
            TData? newData = default,
            string? details = null,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<AuditLog>> GetLogsAsync(
            DateTime? startDate = null,
            DateTime? endDate = null,
            string? action = null,
            string? entityId = null,
            int? userId = null,
            CancellationToken cancellationToken = default);
    }
} 