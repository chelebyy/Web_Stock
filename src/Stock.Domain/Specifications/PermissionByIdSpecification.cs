using System;
using System.Linq.Expressions;
using Stock.Domain.Entities.Permissions;

namespace Stock.Domain.Specifications
{
    /// <summary>
    /// Belirli bir ID'ye göre izni getirmek için specification.
    /// </summary>
    public sealed class PermissionByIdSpecification : BaseSpecification<Permission>
    {
        /// <summary>
        /// Belirtilen ID'ye sahip izni filtreleyen bir specification oluşturur.
        /// </summary>
        /// <param name="permissionId">Aranacak izin ID'si.</param>
        public PermissionByIdSpecification(int permissionId)
        {
            AddCriteria(p => p.Id == permissionId);
        }
    }
} 