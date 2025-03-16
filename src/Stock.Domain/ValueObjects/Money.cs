using System;
using System.Collections.Generic;

namespace Stock.Domain.ValueObjects
{
    /// <summary>
    /// Para birimi ve miktar value object'i
    /// </summary>
    public class Money : ValueObject
    {
        /// <summary>
        /// Miktar
        /// </summary>
        public decimal Amount { get; private set; }
        
        /// <summary>
        /// Para birimi
        /// </summary>
        public string Currency { get; private set; }
        
        // Private constructor for EF Core
        private Money() { }
        
        /// <summary>
        /// Yeni bir Money nesnesi oluşturur
        /// </summary>
        public Money(decimal amount, string currency)
        {
            if (string.IsNullOrWhiteSpace(currency))
                throw new ArgumentException("Para birimi boş olamaz.", nameof(currency));
                
            Amount = amount;
            Currency = currency;
        }
        
        /// <summary>
        /// Para birimlerini toplar
        /// </summary>
        public static Money operator +(Money left, Money right)
        {
            if (left.Currency != right.Currency)
                throw new InvalidOperationException("Farklı para birimleri toplanamaz.");
                
            return new Money(left.Amount + right.Amount, left.Currency);
        }
        
        /// <summary>
        /// Para birimlerini çıkarır
        /// </summary>
        public static Money operator -(Money left, Money right)
        {
            if (left.Currency != right.Currency)
                throw new InvalidOperationException("Farklı para birimleri çıkarılamaz.");
                
            return new Money(left.Amount - right.Amount, left.Currency);
        }
        
        /// <summary>
        /// Para birimini bir sayı ile çarpar
        /// </summary>
        public static Money operator *(Money money, decimal multiplier)
        {
            return new Money(money.Amount * multiplier, money.Currency);
        }
        
        /// <summary>
        /// Value object karşılaştırması için kullanılır
        /// </summary>
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Amount;
            yield return Currency;
        }
        
        /// <summary>
        /// String temsilini döndürür
        /// </summary>
        public override string ToString()
        {
            return $"{Amount} {Currency}";
        }
    }
} 