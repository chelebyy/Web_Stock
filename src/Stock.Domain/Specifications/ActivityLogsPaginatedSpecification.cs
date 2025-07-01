using Stock.Domain.Entities;

namespace Stock.Domain.Specifications
{
    public class ActivityLogsPaginatedSpecification : BaseSpecification<ActivityLog>
    {
        public ActivityLogsPaginatedSpecification(int page, int pageSize)
            : base()
        {
            ApplyPaging((page - 1) * pageSize, pageSize);
            ApplyOrderByDescending(l => l.Timestamp);
            AddInclude(l => l.User);
        }
    }
} 