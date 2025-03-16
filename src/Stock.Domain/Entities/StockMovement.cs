using System;

namespace Stock.Domain.Entities
{
    /// <summary>
    /// Stok hareketleri için entity sınıfı
    /// </summary>
    public class StockMovement : BaseEntity
    {
        /// <summary>
        /// Ürün ID
        /// </summary>
        public int ProductId { get; set; }
        
        /// <summary>
        /// Miktar
        /// </summary>
        public int Quantity { get; set; }
        
        /// <summary>
        /// Hareket tipi (Giriş, Çıkış, Transfer, vb.)
        /// </summary>
        public string MovementType { get; set; }
        
        /// <summary>
        /// Açıklama
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Oluşturan kullanıcı
        /// </summary>
        public string CreatedBy { get; set; }
        
        /// <summary>
        /// Oluşturulma tarihi
        /// </summary>
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// Ürün
        /// </summary>
        public virtual Product Product { get; set; }
    }
} 