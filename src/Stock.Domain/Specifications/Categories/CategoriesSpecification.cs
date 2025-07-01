using Stock.Domain.Entities;
using System;
using System.Linq.Expressions;

namespace Stock.Domain.Specifications.Categories
{
    public class CategoriesSpecification : BaseSpecification<Category>
    {
        public CategoriesSpecification(string name, string sortField, string sortOrder, int pageNumber, int pageSize)
            : base(c => string.IsNullOrEmpty(name) || c.Name.Value.Contains(name))
        {
            ApplySorting(sortField, sortOrder);
            ApplyPaging((pageNumber - 1) * pageSize, pageSize);
        }

        public CategoriesSpecification(string name)
            : base(c => string.IsNullOrEmpty(name) || c.Name.Value.Contains(name))
        {
        }

        private void ApplySorting(string sortField, string sortOrder)
        {
            if (string.IsNullOrEmpty(sortField))
            {
                ApplyOrderBy(c => c.Name.Value);
                return;
            }

            Expression<Func<Category, object>> keySelector = sortField.ToLower() switch
            {
                "name" => c => c.Name.Value,
                _ => c => c.Id
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