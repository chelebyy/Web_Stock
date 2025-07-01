using Stock.Domain.Entities;

namespace Stock.Domain.Specifications.Products
{
    public class ProductByIdSpecification : BaseSpecification<Product>
    {
        public ProductByIdSpecification(int id)
            : base(p => p.Id == id)
        {
            // Simple specification that filters products by their ID
            // No need to add includes or other criteria here
        }
    }
} 