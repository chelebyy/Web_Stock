using Stock.Domain.Entities.Permissions;
using System.Linq.Expressions;
using System;

namespace Stock.Domain.Specifications.UserPermissions
{
    public class UserPermissionByNameSpecification : BaseSpecification<UserPermission>
    {
        public UserPermissionByNameSpecification(int userId, string permissionName)
            : base(up => up.UserId == userId && up.Permission.Name.Value == permissionName)
        {
            // İlişkili Permission verisini de çekmek için include ekledik
            AddInclude(up => up.Permission);
        }
    }
} 