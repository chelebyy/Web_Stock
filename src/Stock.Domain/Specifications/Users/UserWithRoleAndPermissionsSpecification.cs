using Stock.Domain.Entities;
using Stock.Domain.Entities.Permissions; // RolePermission için
using Stock.Domain.Specifications;

namespace Stock.Domain.Specifications.Users
{
    public class UserWithRoleAndPermissionsSpecification : BaseSpecification<User>
    {
        public UserWithRoleAndPermissionsSpecification(int userId) : base(u => u.Id == userId)
        {
            AddInclude(u => u.Role);
            AddInclude($"{nameof(User.Role)}.{nameof(Role.RolePermissions)}"); // String-based include (EF Core önerir)
            AddInclude($"{nameof(User.Role)}.{nameof(Role.RolePermissions)}.{nameof(RolePermission.Permission)}");
            AddInclude(u => u.UserPermissions);
        }
    }
} 