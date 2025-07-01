using Stock.Domain.Entities;
using System;
using System.Linq.Expressions;

namespace Stock.Domain.Specifications
{
    public class RolesSpecification : BaseSpecification<Role>
    {
        // Parametresiz constructor - tüm rolleri getirir
        public RolesSpecification() : base()
        {
            ApplyOrderBy(r => r.Name.Value);
        }

        public RolesSpecification(string name) 
            : base(r => string.IsNullOrEmpty(name) || r.Name.Value.Contains(name))
        {
            ApplyOrderBy(r => r.Name.Value);
        }

        public RolesSpecification(string name, string sortField, string sortOrder)
            : base(r => string.IsNullOrEmpty(name) || r.Name.Value.Contains(name))
        {
            ApplySorting(sortField, sortOrder);
        }

        public RolesSpecification(string name, string sortField, string sortOrder, int pageNumber, int pageSize)
            : base(r => string.IsNullOrEmpty(name) || r.Name.Value.Contains(name))
        {
            ApplySorting(sortField, sortOrder);
            ApplyPaging((pageNumber - 1) * pageSize, pageSize);
        }

        private void ApplySorting(string sortField, string sortOrder)
        {
            if (string.IsNullOrEmpty(sortField))
            {
                ApplyOrderBy(r => r.Name.Value);
                return;
            }

            Expression<Func<Role, object>> keySelector = sortField.ToLower() switch
            {
                "name" => r => r.Name.Value,
                _ => r => r.Id
            };

            if (sortOrder?.ToLower() == "desc")
            {
                ApplyOrderByDescending(keySelector);
            }
            else
            {
                ApplyOrderBy(keySelector);
            }
        }
    }
} 