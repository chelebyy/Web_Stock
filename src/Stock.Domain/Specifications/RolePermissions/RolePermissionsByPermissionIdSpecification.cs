using Stock.Domain.Entities.Permissions;

namespace Stock.Domain.Specifications.RolePermissions
{
    public class RolePermissionsByPermissionIdSpecification : BaseSpecification<RolePermission>
    {
        public RolePermissionsByPermissionIdSpecification(int permissionId)
            : base(rp => rp.PermissionId == permissionId)
        {
            AddInclude(rp => rp.Role);
        }
    }
} 