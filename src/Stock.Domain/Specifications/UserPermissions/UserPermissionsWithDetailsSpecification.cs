using Stock.Domain.Entities.Permissions;
using Stock.Domain.Specifications;

namespace Stock.Domain.Specifications.UserPermissions
{
    public class UserPermissionsWithDetailsSpecification : BaseSpecification<UserPermission>
    {
        public UserPermissionsWithDetailsSpecification(int userId) : base(up => up.UserId == userId)
        {
            AddInclude(up => up.User);
            AddInclude(up => up.Permission);
        }
    }
} 