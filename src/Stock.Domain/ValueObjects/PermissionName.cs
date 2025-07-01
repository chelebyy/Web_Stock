using Stock.Domain.Common;
using System.Collections.Generic;

namespace Stock.Domain.ValueObjects
{
    public class PermissionName : ValueObject
    {
        public const int MaxLength = 100;

        public string Value { get; }

        private PermissionName(string value)
        {
            Value = value;
        }

        public static Result<PermissionName> Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Result<PermissionName>.Failure(DomainErrors.Permission.NameEmpty);
            }

            if (name.Length > MaxLength)
            {
                return Result<PermissionName>.Failure(DomainErrors.Permission.NameTooLong);
            }

            return Result<PermissionName>.Success(new PermissionName(name));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator string(PermissionName permissionName) => permissionName.Value;
        
        public override string ToString() => Value;
    }
} 