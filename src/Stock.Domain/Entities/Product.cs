using System;

namespace Stock.Domain.Entities
{
    /// <summary>
    /// Ürün entity sınıfı
    /// </summary>
    public class Product : BaseEntity
    {
        /// <summary>
        /// Ürün adı
        /// </summary>
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// Ürün açıklaması
        /// </summary>
        public string? Description { get; set; }
        
        /// <summary>
        /// Fiyat
        /// </summary>
        public decimal Price { get; set; }
        
        /// <summary>
        /// Stok miktarı
        /// </summary>
        public int StockQuantity { get; set; }
        
        /// <summary>
        /// Stok Kodu
        /// </summary>
        public string? SKU { get; set; }
        
        /// <summary>
        /// Barkod
        /// </summary>
        public string? Barcode { get; set; }
        
        /// <summary>
        /// Kategori ID
        /// </summary>
        public int CategoryId { get; set; }
        
        /// <summary>
        /// Kategori
        /// </summary>
        public virtual Category Category { get; set; } = null!;
    }
} 