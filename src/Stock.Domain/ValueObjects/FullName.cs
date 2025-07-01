using Stock.Domain.Common;
using Stock.Domain.Exceptions;

namespace Stock.Domain.ValueObjects
{
    public class FullName : ValueObject
    {
        public string Adi { get; }
        public string Soyadi { get; }

        private FullName(string adi, string soyadi)
        {
            Adi = adi;
            Soyadi = soyadi;
        }

        public static Result<FullName> Create(string adi, string soyadi)
        {
            if (string.IsNullOrWhiteSpace(adi))
            {
                return Result<FullName>.Failure(DomainErrors.User.AdiEmpty);
            }

            if (string.IsNullOrWhiteSpace(soyadi))
            {
                return Result<FullName>.Failure(DomainErrors.User.SoyadiEmpty);
            }

            // Burada istenirse maksimum uzunluk kontrolü eklenebilir.
            // Şimdilik eklenmiyor.

            return Result<FullName>.Success(new FullName(adi, soyadi));
        }

        public override string ToString()
        {
            return $"{Adi} {Soyadi}";
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Adi;
            yield return Soyadi;
        }
    }
} 