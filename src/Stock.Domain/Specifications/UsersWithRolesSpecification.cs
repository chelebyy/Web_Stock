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
        public UsersWithRolesSpecification() : base()
        {
            AddInclude(u => u.Role);
        }

        /// <summary>
        /// Filtreleme kriterlerine göre kullanıcıları rolleriyle birlikte getiren bir specification oluşturur.
        /// </summary>
        /// <param name="name">Aranacak kullanıcı adı veya soyadı parçası.</param>
        /// <param name="roleId">Filtrelenecek rol ID'si.</param>
        /// <param name="sortField">Sıralama için kullanılacak alan.</param>
        /// <param name="sortOrder">Sıralama için kullanılacak sıralama türü.</param>
        public UsersWithRolesSpecification(string name, int? roleId, string sortField, string sortOrder)
            : base(u =>
                (string.IsNullOrEmpty(name) || (u.Adi + " " + u.Soyadi).Contains(name) || u.Sicil.Contains(name)) &&
                (!roleId.HasValue || u.RoleId == roleId.Value)
            )
        {
            AddInclude(u => u.Role);

            if (string.IsNullOrEmpty(sortField))
            {
                ApplyOrderBy(u => u.Adi);
                return;
            }

            var keySelector = GetSortExpression(sortField);

            if (keySelector != null)
            {
                if (sortOrder?.ToLower() == "desc")
                {
                    ApplyOrderByDescending(keySelector);
                }
                else
                {
                    ApplyOrderBy(keySelector);
                }
            }
        }

        /// <summary>
        /// Filtreleme kriterlerine göre kullanıcıları rolleriyle birlikte getiren bir specification oluşturur.
        /// </summary>
        /// <param name="name">Aranacak kullanıcı adı veya soyadı parçası.</param>
        /// <param name="roleId">Filtrelenecek rol ID'si.</param>
        public UsersWithRolesSpecification(string name, int? roleId)
            : this(name, roleId, null, null)
        {
        }

        /// <summary>
        /// Belirli bir ID'ye sahip kullanıcıyı rolüyle getiren bir specification oluşturur.
        /// </summary>
        /// <param name="userId">Getirilecek kullanıcının ID'si.</param>
        public UsersWithRolesSpecification(int userId)
            : base(u => u.Id == userId)
        {
            AddInclude(u => u.Role);
        }

        private System.Linq.Expressions.Expression<System.Func<User, object>> GetSortExpression(string sortField)
        {
            return sortField.ToLower() switch
            {
                "fullname" => u => u.Adi + " " + u.Soyadi,
                "sicil" => u => u.Sicil,
                "rolename" => u => u.Role.Name,
                _ => u => u.Id
            };
        }
    }
} 