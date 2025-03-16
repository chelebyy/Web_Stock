using System;

namespace Stock.Domain.Exceptions
{
    /// <summary>
    /// Domain katmanı için özel istisna sınıfı
    /// </summary>
    public class DomainException : Exception
    {
        /// <summary>
        /// Yeni bir DomainException oluşturur
        /// </summary>
        public DomainException()
        {
        }

        /// <summary>
        /// Belirtilen mesajla yeni bir DomainException oluşturur
        /// </summary>
        /// <param name="message">Hata mesajı</param>
        public DomainException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Belirtilen mesaj ve iç istisna ile yeni bir DomainException oluşturur
        /// </summary>
        /// <param name="message">Hata mesajı</param>
        /// <param name="innerException">İç istisna</param>
        public DomainException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
} 