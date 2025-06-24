using Stock.Domain.Common;
using Stock.Domain.Exceptions;
using System.Collections.Generic;

namespace Stock.Domain.ValueObjects
{
    public class ProductName : ValueObject
    {
        public string Value { get; }

        private ProductName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new DomainException("Product name cannot be empty or whitespace.");
            }
            // Gerekirse maksimum uzunluk kontrolü eklenebilir.
            // if (value.Length > 100)
            // {
            //     throw new DomainException("Product name cannot exceed 100 characters.");
            // }
            Value = value;
        }

        public static ProductName From(string value)
        {
            return new ProductName(value);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        // Implicit conversion to string for convenience (optional)
        public static implicit operator string(ProductName name) => name.Value;
    }
} 