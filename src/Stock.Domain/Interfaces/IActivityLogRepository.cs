using Stock.Domain.Entities;

namespace Stock.Domain.Interfaces
{
    public interface IActivityLogRepository : IRepository<ActivityLog>
    {
        // Add any specific methods for ActivityLog if needed in the future
    }
} 