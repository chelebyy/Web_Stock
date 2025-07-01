using System;
using Stock.Domain.Entities;
using Stock.Domain.Entities.Permissions;
using Stock.Domain.Specifications;

namespace Stock.Domain.Specifications.UserPermissions
{
    public class UserCustomPermissionsSpecification : BaseSpecification<UserPermission>
    {
        public UserCustomPermissionsSpecification(int userId)
            : base(up => up.UserId == userId)
        {
            AddInclude(up => up.Permission);
            AddInclude(up => up.User); // Kullanıcı adını almak için User'ı da include et
        }
    }
} 