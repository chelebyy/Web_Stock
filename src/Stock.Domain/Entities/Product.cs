using Stock.Domain.Entities;
using Stock.Domain.Common;
using Stock.Domain.ValueObjects;
using Stock.Domain.Exceptions;

namespace Stock.Domain.Entities
{
    public class Product : BaseEntity
    {
        public ProductName Name { get; private set; }
        public ProductDescription Description { get; private set; }
        public StockLevel StockLevel { get; private set; }

        public int CategoryId { get; private set; }
        public Category Category { get; private set; }

        private Product() { }

        public static Product Create(ProductName name, ProductDescription description, StockLevel stockLevel, int categoryId)
        {
            if (name == null) throw new DomainException("Product name cannot be null.");
            if (stockLevel == null) throw new DomainException("Stock level cannot be null.");
            if (categoryId <= 0) throw new DomainException("Invalid CategoryId.");

            return new Product
            {
                Name = name,
                Description = description,
                StockLevel = stockLevel,
                CategoryId = categoryId
            };
        }

        public void UpdateName(ProductName newName)
        {
            if (newName == null) throw new DomainException("Product name cannot be null.");
            Name = newName;
        }

        public void UpdateDescription(ProductDescription newDescription)
        {
            Description = newDescription;
        }

        public void IncreaseStock(int amount)
        {
            StockLevel = StockLevel.Increase(amount);
        }

        public void DecreaseStock(int amount)
        {
            StockLevel = StockLevel.Decrease(amount);
        }

        public void ChangeCategory(int newCategoryId)
        {
            if (newCategoryId <= 0) throw new DomainException("Invalid CategoryId.");
            if (newCategoryId == CategoryId) return;

            CategoryId = newCategoryId;
            Category = null;
        }
    }
} 