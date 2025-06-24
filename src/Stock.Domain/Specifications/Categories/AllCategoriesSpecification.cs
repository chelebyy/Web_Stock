using Stock.Domain.Entities;
using System.Linq.Expressions;
using System;

namespace Stock.Domain.Specifications.Categories;

public class AllCategoriesSpecification : BaseSpecification<Category>
{
    public AllCategoriesSpecification()
        : base(c => true)
    {
        // Query.Where(c => true); // Bu satır artık gereksiz.
    }
} 