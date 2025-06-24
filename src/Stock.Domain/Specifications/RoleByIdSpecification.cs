using System.Linq.Expressions;
using Stock.Domain.Entities;

namespace Stock.Domain.Specifications
{
    /// <summary>
    /// Belirli bir ID'ye göre rolü getirmek için specification.
    /// İsteğe bağlı olarak ilişkili kullanıcıları da içerebilir.
    /// </summary>
    public sealed class RoleByIdSpecification : BaseSpecification<Role>
    {
        /// <summary>
        /// Belirtilen ID'ye sahip rolü filtreleyen ve isteğe bağlı olarak kullanıcıları include eden bir specification oluşturur.
        /// </summary>
        /// <param name="id">Aranacak rol ID'si.</param>
        public RoleByIdSpecification(int id)
            : base(r => r.Id == id)
        {
            // İsteğe bağlı olarak ilişkili verileri (örn. kullanıcılar, izinler) ekleyebilirsiniz.
            // AddInclude(r => r.Users);
            // AddInclude(r => r.RolePermissions);
        }
    }
} 