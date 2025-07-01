using System;
using Stock.Domain.Entities.Permissions;
using System.Linq.Expressions;

namespace Stock.Domain.Specifications.UserPermissions
{
    public class UserPermissionSpecification : BaseSpecification<UserPermission>
    {
        public UserPermissionSpecification(int userId, int permissionId)
            : base(up => up.UserId == userId && up.PermissionId == permissionId)
        {
            // İlişkili Permission verisini de çekmek isteyebiliriz
            // AddInclude(up => up.Permission);
        }
    }
} 