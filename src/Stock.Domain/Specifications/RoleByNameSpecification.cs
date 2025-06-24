using System.Linq.Expressions;
using Stock.Domain.Entities;

namespace Stock.Domain.Specifications
{
    /// <summary>
    /// Belirli bir role adına göre rolü getirmek için specification.
    /// İsteğe bağlı olarak ilişkili kullanıcıları da içerebilir.
    /// </summary>
    public sealed class RoleByNameSpecification : BaseSpecification<Role>
    {
        /// <summary>
        /// Role adına göre filtreleyen ve isteğe bağlı olarak kullanıcıları include eden bir specification oluşturur.
        /// </summary>
        /// <param name="name">Aranacak rol adı.</param>
        /// <param name="includeUsers">Kullanıcıları dahil et (varsayılan: false).</param>
        public RoleByNameSpecification(string name, bool includeUsers = false)
            : base(r => r.Name.ToLower() == name.ToLower())
        {
            if (includeUsers)
            {
                AddInclude(r => r.Users);
            }
        }
    }
} 