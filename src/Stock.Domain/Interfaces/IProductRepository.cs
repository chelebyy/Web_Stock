using System.Collections.Generic;
using System.Threading.Tasks;
using Stock.Domain.Entities;

namespace Stock.Domain.Interfaces
{
    /// <summary>
    /// Ürün repository arayüzü
    /// </summary>
    public interface IProductRepository : IRepository<Product>
    {
        /// <summary>
        /// Ürün koduna göre ürün getirir
        /// </summary>
        Task<Product> GetByCodeAsync(string code);
        
        /// <summary>
        /// Kategori ID'sine göre ürünleri getirir
        /// </summary>
        Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId);
        
        /// <summary>
        /// Stok miktarı belirtilen değerin altında olan ürünleri getirir
        /// </summary>
        Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold);
        
        /// <summary>
        /// Ürünü varyantlarıyla birlikte getirir
        /// </summary>
        Task<Product> GetWithVariantsAsync(int id);
        
        /// <summary>
        /// Aktif ürünleri getirir
        /// </summary>
        Task<IEnumerable<Product>> GetActiveProductsAsync();
    }
} 