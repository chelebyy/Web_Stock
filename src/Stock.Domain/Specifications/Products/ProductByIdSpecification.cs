using Stock.Domain.Entities;

namespace Stock.Domain.Specifications.Products
{
    public class ProductByIdSpecification : BaseSpecification<Product>
    {
        public ProductByIdSpecification(int productId)
            : base(p => p.Id == productId)
        {
            // Simple specification that filters products by their ID
            // No need to add includes or other criteria here
        }
    }
} 