using System.Linq.Expressions;
using Stock.Domain.Entities.Permissions;

namespace Stock.Domain.Specifications
{
    /// <summary>
    /// Belirli bir gruba ait izinleri getirmek için specification.
    /// </summary>
    public sealed class PermissionsByGroupSpecification : BaseSpecification<Permission>
    {
        /// <summary>
        /// İzin grubuna göre filtreleyen bir specification oluşturur.
        /// </summary>
        /// <param name="groupName">Aranacak grup adı.</param>
        public PermissionsByGroupSpecification(string groupName)
            : base(p => p.Group == groupName)
        {
            // Varsayılan olarak isme göre sırala
            ApplyOrderBy(p => p.Name);
        }
    }
} 