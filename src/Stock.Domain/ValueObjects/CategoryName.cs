using Stock.Domain.Common;
using System.Collections.Generic;

namespace Stock.Domain.ValueObjects
{
    public class CategoryName : ValueObject
    {
        public const int MaxLength = 100;

        public string Value { get; }

        private CategoryName(string value)
        {
            Value = value;
        }

        public static Result<CategoryName> Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Result<CategoryName>.Failure(DomainErrors.Category.NameEmpty);
            }

            if (name.Length > MaxLength)
            {
                return Result<CategoryName>.Failure(DomainErrors.Category.NameTooLong);
            }

            return Result<CategoryName>.Success(new CategoryName(name));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        // Implicit conversion to string for convenience
        public static implicit operator string(CategoryName categoryName) => categoryName?.Value;
    }
} 