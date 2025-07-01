using Stock.Domain.Common; // BaseEntity ve Result için
using System.Collections.Generic; // ICollection için
using Stock.Domain.Exceptions; // DomainErrors için (varsayılan)
using Stock.Domain.ValueObjects; // CategoryName için eklendi

namespace Stock.Domain.Entities
{
    public class Category : BaseEntity
    {
        // Property'ler private setter ile korunuyor
        public CategoryName Name { get; private set; }
        public string Description { get; private set; } = string.Empty;
        public ICollection<Product> Products { get; private set; } // EF Core ilişkisi

        // EF Core için gerekli protected constructor
        protected Category() 
        {
            Name = null!;
            Products = new HashSet<Product>(); // Koleksiyonu başlat
        }

        // Private constructor ile kontrollü nesne oluşturma
        private Category(CategoryName name, string description)
        {
            Name = name;
            Description = description;
            Products = new HashSet<Product>(); // Koleksiyonu başlat
        }

        // Factory metodu
        public static Result<Category> Create(string name, string? description = null) // description nullable
        {
            var categoryNameResult = CategoryName.Create(name);
            if (categoryNameResult.IsFailure)
            {
                return Result<Category>.Failure(categoryNameResult.Error);
            }
            
            // İsteğe bağlı: Uzunluk kontrolü eklenebilir
            // if (name.Length > MaxNameLength) ...

            var category = new Category(categoryNameResult.Value, description ?? string.Empty);
            return Result<Category>.Success(category);
        }

        // Davranış metotları
        public Result UpdateName(string newName)
        {
            var categoryNameResult = CategoryName.Create(newName);
            if (categoryNameResult.IsFailure)
            {
                return Result.Failure(categoryNameResult.Error);
            }

            Name = categoryNameResult.Value;
            return Result.Success();
        }

        public Result UpdateDescription(string? newDescription) // description nullable
        {
            // İsteğe bağlı: Uzunluk kontrolü eklenebilir

            Description = newDescription ?? string.Empty;
            return Result.Success();
        }
    }
} 