using System;

namespace Stock.Application.Common.Exceptions
{
    /// <summary>
    /// Kayıt bulunamadığında fırlatılan istisna
    /// </summary>
    public class NotFoundException : Exception
    {
        /// <summary>
        /// Yeni bir NotFoundException örneği oluşturur
        /// </summary>
        public NotFoundException()
            : base()
        {
        }

        /// <summary>
        /// Belirtilen hata mesajıyla yeni bir NotFoundException örneği oluşturur
        /// </summary>
        /// <param name="message">Hata mesajı</param>
        public NotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Belirtilen hata mesajı ve iç istisnayla yeni bir NotFoundException örneği oluşturur
        /// </summary>
        /// <param name="message">Hata mesajı</param>
        /// <param name="innerException">İç istisna</param>
        public NotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Belirtilen entity adı ve anahtarla yeni bir NotFoundException örneği oluşturur
        /// </summary>
        /// <param name="name">Entity adı</param>
        /// <param name="key">Entity anahtarı</param>
        public NotFoundException(string name, object key)
            : base($"{name} ({key}) bulunamadı.")
        {
        }
    }
} 