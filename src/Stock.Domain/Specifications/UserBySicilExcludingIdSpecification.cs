using System.Linq.Expressions;
using Stock.Domain.Entities;

namespace Stock.Domain.Specifications
{
    /// <summary>
    /// Belirli bir sicil numarasına sahip ve belirtilen ID dışındaki kullanıcıları bulmak için specification.
    /// Kullanıcı güncelleme sırasında sicil benzersizliğini kontrol etmek için kullanılır.
    /// </summary>
    public sealed class UserBySicilExcludingIdSpecification : BaseSpecification<User>
    {
        /// <summary>
        /// Belirtilen sicil numarasına sahip ve verilen ID dışındaki kullanıcıları filtreleyen bir specification oluşturur.
        /// </summary>
        /// <param name="sicil">Aranacak sicil numarası.</param>
        /// <param name="userIdToExclude">Kontrol dışında tutulacak kullanıcı ID'si.</param>
        public UserBySicilExcludingIdSpecification(string sicil, int userIdToExclude)
            : base(u => u.Sicil == sicil && u.Id != userIdToExclude)
        {
            // Bu kontrol için rol bilgisine gerek yok.
        }
    }
} 