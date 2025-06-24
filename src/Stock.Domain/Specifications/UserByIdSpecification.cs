using System.Linq.Expressions;
using Stock.Domain.Entities;

namespace Stock.Domain.Specifications
{
    /// <summary>
    /// Belirli bir ID'ye göre kullanıcıyı getirmek için specification.
    /// İlişkili veri içermez.
    /// </summary>
    public sealed class UserByIdSpecification : BaseSpecification<User>
    {
        /// <summary>
        /// Belirtilen ID'ye sahip kullanıcıyı filtreleyen bir specification oluşturur.
        /// </summary>
        /// <param name="userId">Aranacak kullanıcı ID'si.</param>
        public UserByIdSpecification(int userId)
            : base(u => u.Id == userId)
        {
        }
    }
} 