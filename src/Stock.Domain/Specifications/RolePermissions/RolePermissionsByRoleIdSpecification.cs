using Stock.Domain.Entities.Permissions;
using System.Linq.Expressions;
using System;

namespace Stock.Domain.Specifications.RolePermissions
{
    public class RolePermissionsByRoleIdSpecification : BaseSpecification<RolePermission>
    {
        public RolePermissionsByRoleIdSpecification(int roleId)
            : base(rp => rp.RoleId == roleId)
        {
        }
    }
} 