using Stock.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stock.Domain.Interfaces
{
    /// <summary>
    /// Ürün repository arayüzü
    /// </summary>
    public interface IProductRepository : IRepository<Product>
    {
        /// <summary>
        /// Kategori ID'sine göre ürünleri getirir
        /// </summary>
        Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId);
    }
} 