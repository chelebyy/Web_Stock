using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Stock.Domain.Entities;
using Stock.Domain.Specifications;

namespace Stock.Domain.Specifications.Products
{
    /// <summary>
    /// ID'ye göre ürünü ve kategorisini getiren specification
    /// </summary>
    public class ProductByIdWithCategorySpecification : BaseSpecification<Product>
    {
        /// <summary>
        /// ProductByIdWithCategorySpecification constructor'ı
        /// </summary>
        /// <param name="productId">Getirilecek ürünün ID'si</param>
        public ProductByIdWithCategorySpecification(int productId)
            : base(p => p.Id == productId && !p.IsDeleted)
        {
            // Category ilişkisini dahil et
            AddInclude(p => p.Category);
        }
    }
} 