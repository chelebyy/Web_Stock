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

        private Product() 
        {
            Name = null!;
            Description = null!;
            StockLevel = null!;
            Category = null!;
        }

        public static Result<Product> Create(ProductName name, ProductDescription description, StockLevel stockLevel, int categoryId)
        {
            if (name == null) return Result<Product>.Failure("Product name cannot be null.");
            if (stockLevel == null) return Result<Product>.Failure("Stock level cannot be null.");
            if (categoryId <= 0) return Result<Product>.Failure("Invalid CategoryId.");

            return Result<Product>.Success(new Product
            {
                Name = name,
                Description = description,
                StockLevel = stockLevel,
                CategoryId = categoryId
            });
        }

        public Result UpdateName(ProductName newName)
        {
            if (newName == null) return Result.Failure("Product name cannot be null.");
            Name = newName;
            return Result.Success();
        }

        public Result UpdateDescription(ProductDescription newDescription)
        {
            Description = newDescription;
            return Result.Success();
        }

        public Result IncreaseStock(int amount)
        {
            var result = StockLevel.Increase(amount);
            if (!result.IsSuccess) return Result.Failure(result.Error);
            StockLevel = result.Value;
            return Result.Success();
        }

        public Result DecreaseStock(int amount)
        {
            var result = StockLevel.Decrease(amount);
            if (!result.IsSuccess) return Result.Failure(result.Error);
            StockLevel = result.Value;
            return Result.Success();
        }

        public Result ChangeCategory(int newCategoryId)
        {
            if (newCategoryId <= 0) return Result.Failure("Invalid CategoryId.");
            if (newCategoryId == CategoryId) return Result.Success();

            CategoryId = newCategoryId;
            Category = null;
            return Result.Success();
        }
    }
} 