using System;
using System.Linq;
using System.Linq.Expressions;
using Stock.Domain.Entities;

namespace Stock.Domain.Specifications
{
    /// <summary>
    /// Tüm kullanıcıları rolleriyle birlikte getirmek için specification.
    /// İsteğe bağlı olarak sıralama ve sayfalama uygulayabilir.
    /// </summary>
    public sealed class UsersWithRolesSpecification : BaseSpecification<User>
    {
        /// <summary>
        /// Tüm kullanıcıları rolleriyle birlikte getiren bir specification oluşturur.
        /// </summary>
        public UsersWithRolesSpecification()
        {
            AddInclude(u => u.Role);
            AddInclude(u => u.UserPermissions);
        }

        /// <summary>
        /// Filtreleme ve sıralama kriterlerine göre kullanıcıları rolleriyle birlikte getiren bir specification oluşturur.
        /// </summary>
        public UsersWithRolesSpecification(string name, int? roleId, string sortField, string sortOrder)
        {
            AddInclude(u => u.Role);
            AddInclude(u => u.UserPermissions);

            // Name filter
            if (!string.IsNullOrEmpty(name))
            {
                AddCriteria(u => u.FullName.ToString().ToLower().Contains(name.ToLower()));
            }

            // Role filter
            if (roleId.HasValue)
            {
                AddCriteria(u => u.RoleId == roleId);
            }

            ApplySorting(sortField, sortOrder);
        }

        /// <summary>
        /// Filtreleme, sıralama ve sayfalama kriterlerine göre kullanıcıları rolleriyle birlikte getiren bir specification oluşturur.
        /// </summary>
        public UsersWithRolesSpecification(string name, int? roleId, string sortField, string sortOrder, int pageNumber, int pageSize)
        {
            AddInclude(u => u.Role);
            AddInclude(u => u.UserPermissions);

            // Name filter
            if (!string.IsNullOrEmpty(name))
            {
                AddCriteria(u => u.FullName.ToString().ToLower().Contains(name.ToLower()));
            }

            // Role filter
            if (roleId.HasValue)
            {
                AddCriteria(u => u.RoleId == roleId);
            }

            ApplySorting(sortField, sortOrder);
            ApplyPaging(pageNumber, pageSize);
        }

        /// <summary>
        /// Belirli bir ID'ye sahip kullanıcıyı rolüyle getiren bir specification oluşturur.
        /// </summary>
        /// <param name="userId">Getirilecek kullanıcının ID'si.</param>
        public UsersWithRolesSpecification(int userId)
        {
            AddInclude(u => u.Role);
            AddInclude(u => u.UserPermissions);
            AddCriteria(u => u.Id == userId);
        }

        private void ApplySorting(string sortField, string sortOrder)
        {
            Expression<Func<User, object>> keySelector = sortField?.ToLower() switch
            {
                "name" or "fullname" => u => u.FullName.ToString(),
                "sicil" => u => u.Sicil.Value,
                "role" => u => u.Role != null ? u.Role.Name.Value : "",
                "isadmin" => u => u.IsAdmin,
                "createdat" => u => u.CreatedAt,
                _ => u => u.Id
            };

            if (sortOrder?.ToLower() == "desc")
            {
                AddOrderByDescending(keySelector);
            }
            else
            {
                AddOrderBy(keySelector);
            }
        }
    }
} 