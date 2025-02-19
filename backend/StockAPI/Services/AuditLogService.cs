using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using StockAPI.Models;
using StockAPI.Data;

namespace StockAPI.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AuditLogService> _logger;

        public AuditLogService(ApplicationDbContext context, ILogger<AuditLogService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task LogAsync<TData>(
            int userId,
            string action,
            string entityId,
            TData? oldData = default,
            TData? newData = default,
            string? details = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var auditLog = new AuditLog
                {
                    UserId = userId,
                    Action = action,
                    EntityId = entityId,
                    Timestamp = DateTime.UtcNow,
                    OldData = oldData != null ? JsonSerializer.Serialize(oldData) : null,
                    NewData = newData != null ? JsonSerializer.Serialize(newData) : null,
                    Details = details
                };

                _context.AuditLogs.Add(auditLog);
                await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                _logger.LogInformation(
                    "Denetim günlüğü kaydı oluşturuldu. Kullanıcı: {UserId}, Eylem: {Action}, Varlık: {EntityId}",
                    userId, action, entityId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Denetim günlüğü kaydı oluşturulurken hata oluştu. Kullanıcı: {UserId}, Eylem: {Action}, Varlık: {EntityId}",
                    userId, action, entityId);
                throw;
            }
        }

        public async Task<IEnumerable<AuditLog>> GetLogsAsync(
            DateTime? startDate = null,
            DateTime? endDate = null,
            string? action = null,
            string? entityId = null,
            int? userId = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var query = _context.AuditLogs
                    .Include(a => a.User)
                    .AsNoTracking()
                    .AsQueryable();

                if (startDate.HasValue)
                    query = query.Where(a => a.Timestamp >= startDate.Value);

                if (endDate.HasValue)
                    query = query.Where(a => a.Timestamp <= endDate.Value);

                if (!string.IsNullOrEmpty(action))
                    query = query.Where(a => a.Action == action);

                if (!string.IsNullOrEmpty(entityId))
                    query = query.Where(a => a.EntityId == entityId);

                if (userId.HasValue)
                    query = query.Where(a => a.UserId == userId.Value);

                var logs = await query
                    .OrderByDescending(a => a.Timestamp)
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false);

                _logger.LogInformation(
                    "Denetim günlükleri getirildi. Kayıt sayısı: {Count}",
                    logs.Count);

                return logs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Denetim günlükleri getirilirken hata oluştu");
                throw;
            }
        }
    }
} 