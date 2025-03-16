using Stock.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stock.Domain.Interfaces
{
    /// <summary>
    /// Kategori repository arayüzü
    /// </summary>
    public interface ICategoryRepository : IRepository<Category>
    {
        /// <summary>
        /// Üst kategorileri getirir
        /// </summary>
        Task<IEnumerable<Category>> GetParentCategoriesAsync();
        
        /// <summary>
        /// Belirli bir kategorinin alt kategorilerini getirir
        /// </summary>
        Task<IEnumerable<Category>> GetSubcategoriesAsync(int parentCategoryId);
    }
} 