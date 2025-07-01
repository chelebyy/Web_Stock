using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Stock.Domain.Entities;
using Stock.Domain.Specifications;

namespace Stock.Domain.Specifications.Products
{
    /// <summary>
    /// Belirli bir ID'ye sahip ürünü kategorisi ile birlikte getiren specification.
    /// </summary>
    public class ProductByIdWithCategorySpecification : BaseSpecification<Product>
    {
        /// <summary>
        /// Belirli bir ID'ye sahip ürünü kategorisi ile birlikte getiren bir specification oluşturur.
        /// </summary>
        /// <param name="productId">Getirilecek ürünün ID'si.</param>
        public ProductByIdWithCategorySpecification(int productId)
            : base(p => p.Id == productId)
        {
            AddInclude(p => p.Category); // Category bilgisini dahil et
        }
    }
} 