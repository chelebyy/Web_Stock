using System.Linq.Expressions;
using Stock.Domain.Entities;

namespace Stock.Domain.Specifications
{
    /// <summary>
    /// Belirli bir sicil numarasına göre kullanıcıyı getirmek için specification.
    /// İsteğe bağlı olarak rol bilgisini de içerebilir.
    /// </summary>
    public sealed class UserBySicilSpecification : BaseSpecification<User>
    {
        /// <summary>
        /// Sicil numarasına göre filtreleyen ve isteğe bağlı olarak rolü include eden bir specification oluşturur.
        /// </summary>
        /// <param name="sicil">Aranacak sicil numarası.</param>
        /// <param name="includeRole">Rol bilgisini dahil et (varsayılan: true).</param>
        public UserBySicilSpecification(string sicil, bool includeRole = true)
            : base(u => u.Sicil.Value == sicil)
        {
            if (includeRole)
            {
                AddInclude(u => u.Role);
            }
        }
    }
} 