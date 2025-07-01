using System;
using Stock.Domain.Entities.Permissions;
using System.Linq.Expressions;

namespace Stock.Domain.Specifications.RolePermissions
{
    public class RolePermissionByNameSpecification : BaseSpecification<RolePermission>
    {
        public RolePermissionByNameSpecification(int roleId, string permissionName)
            : base(rp => rp.RoleId == roleId && rp.Permission.Name.Value == permissionName)
        {
            // İlişkili Permission verisini de çekmek için include ekledik
            AddInclude(rp => rp.Permission);
        }
    }
} 