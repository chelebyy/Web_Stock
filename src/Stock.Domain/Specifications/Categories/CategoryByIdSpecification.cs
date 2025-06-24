using Stock.Domain.Entities;
using Stock.Domain.Specifications; // Namespace düzeltildi

namespace Stock.Domain.Specifications.Categories
{
    public class CategoryByIdSpecification : BaseSpecification<Category>
    {
        // Belirli bir ID'ye sahip kategoriyi getirmek için Specification
        public CategoryByIdSpecification(int categoryId)
            : base(c => c.Id == categoryId)
        {
            // İlişkili verileri (Include) burada ekleyebiliriz, ancak Category için şimdilik gerekmiyor.
            // AddInclude(c => c.Products); 
        }
    }
} 