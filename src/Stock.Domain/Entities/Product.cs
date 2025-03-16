using System;
using System.Collections.Generic;
using System.Linq;
using Stock.Domain.Events;
using Stock.Domain.Exceptions;
using Stock.Domain.ValueObjects;

namespace Stock.Domain.Entities
{
    /// <summary>
    /// Ürün entity'si
    /// </summary>
    public class Product : BaseEntity
    {
        private List<ProductVariant> _variants = new List<ProductVariant>();
        
        /// <summary>
        /// Ürün adı
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// Ürün kodu
        /// </summary>
        public string Code { get; private set; }
        
        /// <summary>
        /// Ürün açıklaması
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// Kategori ID
        /// </summary>
        public int CategoryId { get; private set; }
        
        /// <summary>
        /// Stok miktarı
        /// </summary>
        public int StockQuantity { get; private set; }
        
        /// <summary>
        /// Birim fiyatı
        /// </summary>
        public Money UnitPrice { get; private set; }
        
        /// <summary>
        /// Ürün varyantları
        /// </summary>
        public IReadOnlyCollection<ProductVariant> Variants => _variants.AsReadOnly();
        
        /// <summary>
        /// Kategori
        /// </summary>
        public virtual Category Category { get; private set; }
        
        /// <summary>
        /// Aktif mi?
        /// </summary>
        public bool IsActive { get; set; }
        
        /// <summary>
        /// Silinmiş mi?
        /// </summary>
        public bool IsDeleted { get; set; }
        
        /// <summary>
        /// Oluşturulma tarihi
        /// </summary>
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// Güncellenme tarihi
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
        
        // Private constructor for EF Core
        private Product() { }
        
        /// <summary>
        /// Yeni bir ürün oluşturur
        /// </summary>
        public Product(string name, string code, string description, int categoryId, int stockQuantity, Money unitPrice)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Ürün adı boş olamaz.");
                
            if (string.IsNullOrWhiteSpace(code))
                throw new DomainException("Ürün kodu boş olamaz.");
                
            if (stockQuantity < 0)
                throw new DomainException("Stok miktarı negatif olamaz.");
                
            Name = name;
            Code = code;
            Description = description;
            CategoryId = categoryId;
            StockQuantity = stockQuantity;
            UnitPrice = unitPrice;
        }
        
        /// <summary>
        /// Ürün bilgilerini günceller
        /// </summary>
        public void Update(string name, string description, int categoryId, Money unitPrice)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Ürün adı boş olamaz.");
                
            Name = name;
            Description = description;
            CategoryId = categoryId;
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
            
            // Domain event yayınla
            AddDomainEvent(new ProductStockChangedEvent(Id, StockQuantity));
        }
        
        /// <summary>
        /// Varyant ekler
        /// </summary>
        public void AddVariant(ProductVariant variant)
        {
            if (_variants.Any(v => v.VariantCode == variant.VariantCode))
                throw new DomainException($"'{variant.VariantCode}' kodlu varyant zaten mevcut.");
                
            _variants.Add(variant);
            UpdatedAt = DateTime.UtcNow;
        }
        
        /// <summary>
        /// Varyant siler
        /// </summary>
        public void RemoveVariant(string variantCode)
        {
            var variant = _variants.FirstOrDefault(v => v.VariantCode == variantCode);
            if (variant == null)
                throw new DomainException($"'{variantCode}' kodlu varyant bulunamadı.");
                
            _variants.Remove(variant);
            UpdatedAt = DateTime.UtcNow;
        }
    }
} 