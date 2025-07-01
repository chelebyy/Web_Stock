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
            Value = value;
        }

        public static Result<StockLevel> From(int value)
        {
            if (value < 0)
            {
                return Result<StockLevel>.Failure("Stock level cannot be negative.");
            }
            return Result<StockLevel>.Success(new StockLevel(value));
        }

        public Result<StockLevel> Increase(int amount)
        {
            if (amount < 0)
                return Result<StockLevel>.Failure("Amount to increase must be non-negative.");
            return From(Value + amount);
        }

        public Result<StockLevel> Decrease(int amount)
        {
            if (amount < 0)
                return Result<StockLevel>.Failure("Amount to decrease must be non-negative.");
            if (Value - amount < 0)
                return Result<StockLevel>.Failure("Cannot decrease stock level below zero.");
            return From(Value - amount);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator int(StockLevel level) => level.Value;
    }
} 