using Stock.Domain.Common;
using Stock.Domain.Exceptions;
using System.Collections.Generic;

namespace Stock.Domain.ValueObjects
{
    public class StockLevel : ValueObject
    {
        public int Value { get; }

        private StockLevel(int value)
        {
            if (value < 0)
            {
                throw new DomainException("Stock level cannot be negative.");
            }
            Value = value;
        }

        public static StockLevel From(int value)
        {
            return new StockLevel(value);
        }

        // Stok seviyesini artırma/azaltma metotları eklenebilir
        public StockLevel Increase(int amount)
        {
            if (amount < 0)
                throw new DomainException("Amount to increase must be non-negative.");
            return new StockLevel(Value + amount);
        }

        public StockLevel Decrease(int amount)
        {
            if (amount < 0)
                throw new DomainException("Amount to decrease must be non-negative.");
            if (Value - amount < 0)
                throw new DomainException("Cannot decrease stock level below zero.");
            return new StockLevel(Value - amount);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator int(StockLevel level) => level.Value;
    }
} 