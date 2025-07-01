using System;
using Stock.Domain.Entities.Permissions;

namespace Stock.Domain.Specifications.UserPermissions
{
    public class UserPermissionsByPermissionIdSpecification : BaseSpecification<UserPermission>
    {
        public UserPermissionsByPermissionIdSpecification(int permissionId) : base(up => up.PermissionId == permissionId)
        {
            AddInclude(up => up.User);
        }
    }
} 