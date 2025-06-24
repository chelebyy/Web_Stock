using System;

namespace Stock.Domain.Exceptions
{
    /// <summary>
    /// Veritabanında zaten mevcut olan bir varlığın kopyası oluşturulmaya çalışıldığında fırlatılan istisna.
    /// </summary>
    public class DuplicateEntityException : Exception
    {
        /// <summary>
        /// DuplicateEntityException sınıfının varsayılan yapıcı metodu.
        /// </summary>
        public DuplicateEntityException() : base("Varlık zaten mevcut.")
        {
        }

        /// <summary>
        /// DuplicateEntityException sınıfının mesaj parametreli yapıcı metodu.
        /// </summary>
        /// <param name="message">Hata mesajı.</param>
        public DuplicateEntityException(string message) : base(message)
        {
        }

        /// <summary>
        /// DuplicateEntityException sınıfının mesaj ve iç istisna parametreli yapıcı metodu.
        /// </summary>
        /// <param name="message">Hata mesajı.</param>
        /// <param name="innerException">İç istisna.</param>
        public DuplicateEntityException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Belirtilen entity ve değer için bir DuplicateEntityException oluşturur.
        /// </summary>
        /// <param name="entityName">Entity adı.</param>
        /// <param name="propertyName">Çakışan özellik adı.</param>
        /// <param name="value">Çakışan değer.</param>
        /// <returns>Oluşturulan DuplicateEntityException.</returns>
        public static DuplicateEntityException Create(string entityName, string propertyName, string value)
        {
            return new DuplicateEntityException($"{entityName} entity'si için aynı {propertyName} değerine ({value}) sahip kayıt zaten mevcut.");
        }
    }
} 