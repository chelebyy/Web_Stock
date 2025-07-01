using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using Stock.Infrastructure.Data.Specifications;

namespace Stock.Infrastructure.Data.Repositories
{
    public class ActivityLogRepository : GenericRepository<ActivityLog>, IActivityLogRepository
    {
        public ActivityLogRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
} 