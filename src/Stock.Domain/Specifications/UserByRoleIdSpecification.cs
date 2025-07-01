using System;
using System.Linq.Expressions;
using Stock.Domain.Entities;

namespace Stock.Domain.Specifications
{
    /// <summary>
    /// Belirli bir RoleId'ye sahip kullanıcıları bulmak için specification.
    /// </summary>
    public class UserByRoleIdSpecification : BaseSpecification<User>
    {
        /// <summary>
        /// Belirtilen RoleId'ye sahip kullanıcıları filtreleyen bir specification oluşturur.
        /// </summary>
        /// <param name="roleId">Filtrelenecek rol ID'si.</param>
        public UserByRoleIdSpecification(int? roleId)
        {
            AddInclude(u => u.Role);
            AddCriteria(u => u.RoleId == roleId);
        }
    }
} 