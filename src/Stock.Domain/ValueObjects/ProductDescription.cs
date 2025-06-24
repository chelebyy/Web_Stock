using Stock.Domain.Common;
using Stock.Domain.Exceptions;
using System.Collections.Generic;

namespace Stock.Domain.ValueObjects
{
    public class ProductDescription : ValueObject
    {
        private const int MaxLength = 500;
        public string Value { get; }

        private ProductDescription(string value)
        {
            // Description null veya boş olabilir
            if (value != null && value.Length > MaxLength)
            {
                throw new DomainException($"Product description cannot exceed {MaxLength} characters.");
            }
            Value = value;
        }

        public static ProductDescription From(string value)
        {
            return new ProductDescription(value);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator string(ProductDescription description) => description?.Value;

        public override string ToString() => Value ?? string.Empty;
    }
} 