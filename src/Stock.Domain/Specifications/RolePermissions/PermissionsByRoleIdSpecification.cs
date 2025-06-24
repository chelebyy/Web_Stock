using Stock.Domain.Entities.Permissions;
using Stock.Domain.Specifications;

namespace Stock.Domain.Specifications.RolePermissions
{
    public class PermissionsByRoleIdSpecification : BaseSpecification<RolePermission>
    {
        public PermissionsByRoleIdSpecification(int roleId)
            : base(rp => rp.RoleId == roleId)
        {
            AddInclude(rp => rp.Permission);
        }
    }
} 