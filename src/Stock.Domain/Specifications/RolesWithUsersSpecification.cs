using Stock.Domain.Entities;

namespace Stock.Domain.Specifications
{
    /// <summary>
    /// Tüm rolleri ilişkili kullanıcılarıyla birlikte getirmek için specification.
    /// </summary>
    public sealed class RolesWithUsersSpecification : BaseSpecification<Role>
    {
        /// <summary>
        /// Tüm rolleri kullanıcılarıyla birlikte getiren bir specification oluşturur.
        /// </summary>
        public RolesWithUsersSpecification()
            : base() // Kriter yok, tüm rolleri alır
        {
            AddInclude(r => r.Users);
            ApplyOrderBy(r => r.Name); // Varsayılan olarak role adına göre sırala
        }
    }
} 