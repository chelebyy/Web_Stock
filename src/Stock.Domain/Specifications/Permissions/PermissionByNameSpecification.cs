using Stock.Domain.Entities.Permissions;
using System;
using System.Linq.Expressions;

namespace Stock.Domain.Specifications.Permissions
{
    public class PermissionByNameSpecification : BaseSpecification<Permission>
    {
        public PermissionByNameSpecification(string permissionName)
            : base(p => p.Name.Value == permissionName)
        {
        }
    }
} 