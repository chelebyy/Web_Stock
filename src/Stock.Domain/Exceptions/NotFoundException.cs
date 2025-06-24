using System;

namespace Stock.Domain.Exceptions
{
    /// <summary>
    /// Kaynak bulunamadığında fırlatılacak özel istisna sınıfı.
    /// </summary>
    public class NotFoundException : Exception
    {
        public NotFoundException() : base()
        {
        }

        public NotFoundException(string message) : base(message)
        {
        }

        public NotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Belirli bir varlık türü ve anahtarı ile NotFoundException oluşturur.
        /// </summary>
        /// <param name="entityName">Bulunamayan varlığın tür adı.</param>
        /// <param name="key">Bulunamayan varlığın anahtar değeri.</param>
        public NotFoundException(string entityName, object key) : base($"Entity \"{entityName}\" ({key}) was not found.")
        {
        }
    }
} 