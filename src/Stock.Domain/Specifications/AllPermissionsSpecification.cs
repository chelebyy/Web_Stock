using Stock.Domain.Entities.Permissions;

namespace Stock.Domain.Specifications
{
    /// <summary>
    /// Tüm izinleri getirmek için kullanılan specification.
    /// </summary>
    public class AllPermissionsSpecification : BaseSpecification<Permission>
    {
        public AllPermissionsSpecification()
        {
            // BaseSpecification constructor'ına kriter geçilmediği için tümünü getirir.
            // Gerekirse sıralama eklenebilir:
            // ApplyOrderBy(p => p.Name);
        }
    }
} 