using Stock.Domain.Common; // BaseEntity ve Result için
using System.Collections.Generic; // ICollection için
using Stock.Domain.Exceptions; // DomainErrors için (varsayılan)

namespace Stock.Domain.Entities
{
    public class Category : BaseEntity
    {
        // Property'ler private setter ile korunuyor
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public ICollection<Product> Products { get; private set; } // EF Core ilişkisi

        // EF Core için gerekli protected constructor
        protected Category() 
        {
            Products = new HashSet<Product>(); // Koleksiyonu başlat
        }

        // Private constructor ile kontrollü nesne oluşturma
        private Category(string name, string description)
        {
            Name = name;
            Description = description;
            Products = new HashSet<Product>(); // Koleksiyonu başlat
        }

        // Factory metodu
        public static Result<Category> Create(string name, string? description = null) // description nullable
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                // İdeal: return Result<Category>.Failure(DomainErrors.Category.NameEmpty);
                return Result<Category>.Failure("Category name cannot be empty."); 
            }
            
            // İsteğe bağlı: Uzunluk kontrolü eklenebilir
            // if (name.Length > MaxNameLength) ...

            var category = new Category(name, description ?? string.Empty);
            return Result<Category>.Success(category);
        }

        // Davranış metotları
        public Result UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                 // İdeal: return Result.Failure(DomainErrors.Category.NameEmpty);
                 return Result.Failure("Category name cannot be empty.");
            }

            // İsteğe bağlı: Uzunluk kontrolü eklenebilir

            Name = newName;
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