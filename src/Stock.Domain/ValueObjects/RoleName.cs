using Stock.Domain.Common;

namespace Stock.Domain.ValueObjects
{
    public class RoleName : ValueObject
    {
        public const int MaxLength = 50;

        public string Value { get; }

        private RoleName(string value)
        {
            Value = value;
        }

        public static Result<RoleName> Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Result<RoleName>.Failure(DomainErrors.Role.NameEmpty);
            }

            if (name.Length > MaxLength)
            {
                return Result<RoleName>.Failure(DomainErrors.Role.NameTooLong);
            }

            return Result<RoleName>.Success(new RoleName(name));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator string(RoleName roleName) => roleName.Value;
        
        public override string ToString() => Value;
    }
} 