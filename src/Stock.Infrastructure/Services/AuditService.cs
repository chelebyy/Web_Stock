using Stock.Application.Common.Interfaces;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace Stock.Infrastructure.Services
{
    public class AuditService : IAuditService
    {
        private readonly IRepository<AuditLog> _auditRepository;

        public AuditService(IRepository<AuditLog> auditRepository)
        {
            _auditRepository = auditRepository;
        }

        public async Task LogActionAsync(string action, string entityType, string entityId, int? userId = null, string? path = null, string? details = null)
        {
            var auditLog = new AuditLog
            {
                Action = action,
                EntityType = entityType,
                EntityId = entityId,
                UserId = userId,
                Path = path,
                Details = details,
                CreatedAt = DateTime.UtcNow
            };

            await _auditRepository.AddAsync(auditLog);
            await _auditRepository.SaveChangesAsync();
        }
    }
} 