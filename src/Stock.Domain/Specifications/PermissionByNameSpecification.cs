using System.Linq.Expressions;
using Stock.Domain.Entities.Permissions;

namespace Stock.Domain.Specifications
{
    /// <summary>
    /// Belirli bir izin adına göre izni getirmek için specification.
    /// </summary>
    public sealed class PermissionByNameSpecification : BaseSpecification<Permission>
    {
        /// <summary>
        /// İzin adına göre filtreleyen bir specification oluşturur.
        /// </summary>
        /// <param name="name">Aranacak izin adı.</param>
        public PermissionByNameSpecification(string name)
            : base(p => p.Name.Value == name)
        {
        }
    }
} 