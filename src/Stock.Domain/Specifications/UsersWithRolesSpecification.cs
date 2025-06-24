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
        /// <param name="orderByAdiSoyadi">Kullanıcı adına göre artan sırala (varsayılan: false).</param>
        /// <param name="skip">Sayfalama için atlanacak kayıt sayısı.</param>
        /// <param name="take">Sayfalama için alınacak kayıt sayısı.</param>
        public UsersWithRolesSpecification(bool orderByAdiSoyadi = false, int? skip = null, int? take = null)
            : base() // Kriter yok, tüm kullanıcıları alır
        {
            AddInclude(u => u.Role);

            if (orderByAdiSoyadi)
            {
                ApplyOrderBy(u => u.Adi);
                ApplyThenBy(u => u.Soyadi);
            }

            if (skip.HasValue && take.HasValue)
            {
                ApplyPaging(skip.Value, take.Value);
            }
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
    }
} 