using System;
using Stock.Domain.Entities.Permissions;
using System.Linq.Expressions;

namespace Stock.Domain.Specifications.RolePermissions
{
    public class RolePermissionsByRoleIdSpecification : BaseSpecification<RolePermission>
    {
        public RolePermissionsByRoleIdSpecification(int roleId)
            : base(rp => rp.RoleId == roleId)
        {
            AddInclude(rp => rp.Permission);
        }
    }
} 