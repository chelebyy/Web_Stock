using System.Collections.Generic;
using System.Threading.Tasks;
using Stock.Domain.Entities;

namespace Stock.Domain.Interfaces
{
    /// <summary>
    /// Kategori repository arayüzü
    /// </summary>
    public interface ICategoryRepository : IRepository<Category>
    {
        /// <summary>
        /// Kategoriyi alt kategorileriyle birlikte getirir
        /// </summary>
        Task<Category> GetWithSubCategoriesAsync(int id);
        
        /// <summary>
        /// Üst kategorileri getirir
        /// </summary>
        Task<IEnumerable<Category>> GetParentCategoriesAsync();
        
        /// <summary>
        /// Belirtilen üst kategoriye ait alt kategorileri getirir
        /// </summary>
        Task<IEnumerable<Category>> GetSubCategoriesAsync(int parentId);
        
        /// <summary>
        /// Kategoriyi ürünleriyle birlikte getirir
        /// </summary>
        Task<Category> GetWithProductsAsync(int id);
        
        /// <summary>
        /// Aktif kategorileri getirir
        /// </summary>
        Task<IEnumerable<Category>> GetActiveCategoriesAsync();
    }
} 