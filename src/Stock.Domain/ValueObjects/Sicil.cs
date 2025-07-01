using Stock.Domain.Common;
using Stock.Domain.Exceptions;

namespace Stock.Domain.ValueObjects
{
    public class Sicil : ValueObject
    {
        public const int MaxLength = 50;

        public string Value { get; }

        private Sicil(string value)
        {
            Value = value;
        }

        public static Result<Sicil> Create(string sicil)
        {
            if (string.IsNullOrWhiteSpace(sicil))
            {
                return Result<Sicil>.Failure(DomainErrors.User.SicilEmpty);
            }

            if (sicil.Length > MaxLength)
            {
                return Result<Sicil>.Failure(DomainErrors.User.SicilTooLong(MaxLength));
            }

            return Result<Sicil>.Success(new Sicil(sicil));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator string(Sicil sicil) => sicil.Value;
    }
} 