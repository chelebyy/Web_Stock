using System;

namespace Stock.Domain.Exceptions
{
    /// <summary>
    /// Geçersiz istek hatası durumunda fırlatılan exception sınıfı.
    /// HTTP 400 Bad Request ile eşleşir.
    /// </summary>
    public class BadRequestException : DomainException
    {
        /// <summary>
        /// BadRequestException oluşturur.
        /// </summary>
        public BadRequestException() : base("İstek geçersiz format içeriyor.") { }

        /// <summary>
        /// Özel hata mesajı ile BadRequestException oluşturur.
        /// </summary>
        /// <param name="message">Hata mesajı</param>
        public BadRequestException(string message) : base(message) { }

        /// <summary>
        /// Özel hata mesajı ve InnerException ile BadRequestException oluşturur.
        /// </summary>
        /// <param name="message">Hata mesajı</param>
        /// <param name="innerException">Orijinal exception</param>
        public BadRequestException(string message, Exception innerException) : base(message, innerException) { }
    }
} 