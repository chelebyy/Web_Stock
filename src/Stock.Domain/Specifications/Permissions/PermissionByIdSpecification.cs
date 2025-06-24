using Stock.Domain.Entities.Permissions;

namespace Stock.Domain.Specifications.Permissions
{
    public class PermissionByIdSpecification : BaseSpecification<Permission>
    {
        public PermissionByIdSpecification(int permissionId)
            : base(p => p.Id == permissionId)
        {
        }
    }
} 