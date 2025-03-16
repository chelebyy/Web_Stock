using System;
using System.Collections.Generic;
using System.Linq;

namespace Stock.Domain.ValueObjects
{
    /// <summary>
    /// Değer nesneleri için temel sınıf
    /// </summary>
    public abstract class ValueObject
    {
        /// <summary>
        /// Eşitlik karşılaştırması için kullanılacak bileşenleri döndürür
        /// </summary>
        /// <returns>Eşitlik karşılaştırması için kullanılacak bileşenler</returns>
        protected abstract IEnumerable<object> GetEqualityComponents();

        /// <summary>
        /// Eşitlik karşılaştırması
        /// </summary>
        /// <param name="obj">Karşılaştırılacak nesne</param>
        /// <returns>Eşitlik durumu</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            var other = (ValueObject)obj;

            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        /// <summary>
        /// Hash kodu üretir
        /// </summary>
        /// <returns>Hash kodu</returns>
        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }

        /// <summary>
        /// Eşitlik operatörü
        /// </summary>
        /// <param name="left">Sol operand</param>
        /// <param name="right">Sağ operand</param>
        /// <returns>Eşitlik durumu</returns>
        public static bool operator ==(ValueObject left, ValueObject right)
        {
            if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
                return true;

            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
                return false;

            return left.Equals(right);
        }

        /// <summary>
        /// Eşitsizlik operatörü
        /// </summary>
        /// <param name="left">Sol operand</param>
        /// <param name="right">Sağ operand</param>
        /// <returns>Eşitsizlik durumu</returns>
        public static bool operator !=(ValueObject left, ValueObject right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Nesnenin bir kopyasını oluşturur
        /// </summary>
        /// <returns>Nesnenin kopyası</returns>
        public ValueObject GetCopy()
        {
            return this.MemberwiseClone() as ValueObject;
        }
    }
} 