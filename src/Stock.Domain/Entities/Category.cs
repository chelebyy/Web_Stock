using System;
using System.Collections.Generic;
using Stock.Domain.Exceptions;

namespace Stock.Domain.Entities
{
    /// <summary>
    /// Kategori entity'si
    /// </summary>
    public class Category : BaseEntity
    {
        private List<Category> _subCategories = new List<Category>();
        private List<Product> _products = new List<Product>();
        
        /// <summary>
        /// Kategori adı
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// Kategori açıklaması
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// Üst kategori ID
        /// </summary>
        public int? ParentCategoryId { get; private set; }
        
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
        
        /// <summary>
        /// Üst kategori
        /// </summary>
        public virtual Category ParentCategory { get; private set; }
        
        /// <summary>
        /// Alt kategoriler
        /// </summary>
        public IReadOnlyCollection<Category> SubCategories => _subCategories.AsReadOnly();
        
        /// <summary>
        /// Ürünler
        /// </summary>
        public IReadOnlyCollection<Product> Products => _products.AsReadOnly();
        
        // Private constructor for EF Core
        private Category() { }
        
        /// <summary>
        /// Yeni bir kategori oluşturur
        /// </summary>
        public Category(string name, string description, int? parentCategoryId = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Kategori adı boş olamaz.");
                
            Name = name;
            Description = description;
            ParentCategoryId = parentCategoryId;
        }
        
        /// <summary>
        /// Kategori bilgilerini günceller
        /// </summary>
        public void Update(string name, string description, int? parentCategoryId = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Kategori adı boş olamaz.");
                
            // Döngüsel bağımlılık kontrolü
            if (parentCategoryId.HasValue && parentCategoryId.Value == Id)
                throw new DomainException("Kategori kendisinin üst kategorisi olamaz.");
                
            Name = name;
            Description = description;
            ParentCategoryId = parentCategoryId;
            UpdatedAt = DateTime.UtcNow;
        }
        
        /// <summary>
        /// Alt kategori ekler
        /// </summary>
        public void AddSubCategory(Category subCategory)
        {
            if (subCategory == null)
                throw new ArgumentNullException(nameof(subCategory));
                
            if (subCategory.Id == Id)
                throw new DomainException("Kategori kendisinin alt kategorisi olamaz.");
                
            _subCategories.Add(subCategory);
            subCategory.ParentCategoryId = Id;
            UpdatedAt = DateTime.UtcNow;
        }
        
        /// <summary>
        /// Ürün ekler
        /// </summary>
        public void AddProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));
                
            _products.Add(product);
            UpdatedAt = DateTime.UtcNow;
        }
    }
} 