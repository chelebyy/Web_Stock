using Stock.Application.Common.Interfaces;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace Stock.Infrastructure.Services
{
    public class AuditService : IAuditService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuditService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

            await _unitOfWork.GetRepository<AuditLog>().AddAsync(auditLog);
            await _unitOfWork.SaveChangesAsync();
        }
    }
} 