using Stock.Domain.Entities;
using System.Linq.Expressions;
using System;

namespace Stock.Domain.Specifications
{
    public class RolesSpecification : BaseSpecification<Role>
    {
        public RolesSpecification(string name) : base(r => string.IsNullOrEmpty(name) || r.Name.Contains(name))
        {
        }
        
        public RolesSpecification(string name, string sortField, string sortOrder)
            : base(r => string.IsNullOrEmpty(name) || r.Name.Contains(name))
        {
            if (!string.IsNullOrEmpty(sortField))
            {
                Expression<Func<Role, object>> keySelector = sortField.ToLower() switch
                {
                    "name" => r => r.Name,
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
            else
            {
                ApplyOrderBy(r => r.Name);
            }
        }
    }
} 