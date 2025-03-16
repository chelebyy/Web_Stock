using System;
using Stock.Domain.Exceptions;
using Stock.Domain.ValueObjects;

namespace Stock.Domain.Entities
{
    /// <summary>
    /// Ürün varyantı entity'si
    /// </summary>
    public class ProductVariant : BaseEntity
    {
        /// <summary>
        /// Ürün ID
        /// </summary>
        public int ProductId { get; private set; }
        
        /// <summary>
        /// Varyant kodu
        /// </summary>
        public string VariantCode { get; private set; }
        
        /// <summary>
        /// Varyant adı
        /// </summary>
        public string VariantName { get; private set; }
        
        /// <summary>
        /// Stok miktarı
        /// </summary>
        public int StockQuantity { get; private set; }
        
        /// <summary>
        /// Birim fiyatı
        /// </summary>
        public Money UnitPrice { get; private set; }
        
        /// <summary>
        /// Ürün
        /// </summary>
        public virtual Product Product { get; private set; }
        
        // Private constructor for EF Core
        private ProductVariant() { }
        
        /// <summary>
        /// Yeni bir ürün varyantı oluşturur
        /// </summary>
        public ProductVariant(int productId, string variantCode, string variantName, int stockQuantity, Money unitPrice)
        {
            if (string.IsNullOrWhiteSpace(variantCode))
                throw new DomainException("Varyant kodu boş olamaz.");
                
            if (string.IsNullOrWhiteSpace(variantName))
                throw new DomainException("Varyant adı boş olamaz.");
                
            if (stockQuantity < 0)
                throw new DomainException("Stok miktarı negatif olamaz.");
                
            ProductId = productId;
            VariantCode = variantCode;
            VariantName = variantName;
            StockQuantity = stockQuantity;
            UnitPrice = unitPrice;
        }
        
        /// <summary>
        /// Varyant bilgilerini günceller
        /// </summary>
        public void Update(string variantName, Money unitPrice)
        {
            if (string.IsNullOrWhiteSpace(variantName))
                throw new DomainException("Varyant adı boş olamaz.");
                
            VariantName = variantName;
            UnitPrice = unitPrice;
            UpdatedAt = DateTime.UtcNow;
        }
        
        /// <summary>
        /// Stok miktarını günceller
        /// </summary>
        public void UpdateStock(int quantity)
        {
            if (StockQuantity + quantity < 0)
                throw new DomainException("Stok miktarı yetersiz.");
                
            StockQuantity += quantity;
            UpdatedAt = DateTime.UtcNow;
        }
    }
} 