using Stock.Domain.Entities.Permissions;
using System.Linq.Expressions;
using System;

namespace Stock.Domain.Specifications.UserPermissions
{
    public class UserPermissionsByUserIdSpecification : BaseSpecification<UserPermission>
    {
        public UserPermissionsByUserIdSpecification(int userId)
            : base(up => up.UserId == userId)
        {
            // İlişkili Permission verisini de çekmek isteyebiliriz
            AddInclude(up => up.Permission);
        }
    }
} 