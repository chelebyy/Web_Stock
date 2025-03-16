using System.Collections.Generic;

namespace Stock.Domain.Entities
{
    /// <summary>
    /// Kategori entity sınıfı
    /// </summary>
    public class Category : BaseEntity
    {
        /// <summary>
        /// Kategori adı
        /// </summary>
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// Kategori açıklaması
        /// </summary>
        public string? Description { get; set; }
        
        /// <summary>
        /// Üst kategori ID
        /// </summary>
        public int? ParentCategoryId { get; set; }
        
        /// <summary>
        /// Kategoriye ait ürünler
        /// </summary>
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
} 