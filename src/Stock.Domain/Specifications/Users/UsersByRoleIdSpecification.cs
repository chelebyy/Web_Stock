using Stock.Domain.Common;
using Stock.Domain.Entities;
using System;
using System.Linq.Expressions;

namespace Stock.Domain.Specifications.Users
{
    public class UsersByRoleIdSpecification : BaseSpecification<User>
    {
        public UsersByRoleIdSpecification(int roleId)
            : base(u => u.RoleId == roleId)
        {
            AddInclude(u => u.Role);
        }
    }
} 