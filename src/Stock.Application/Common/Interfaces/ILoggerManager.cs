using System;

namespace Stock.Application.Common.Interfaces
{
    /// <summary>
    /// Uygulama genelinde loglama işlemleri için soyutlama sağlayan generic arayüz.
    /// Farklı loglama kütüphanelerinin kullanılabilmesini sağlar.
    /// </summary>
    /// <typeparam name="T">Log mesajlarının kaynağı olan sınıf türü.</typeparam>
    public interface ILoggerManager<T>
    {
        /// <summary>
        /// Bilgilendirme seviyesinde log mesajı yazar.
        /// </summary>
        /// <param name="message">Log mesajı veya mesaj şablonu.</param>
        /// <param name="args">Mesaj şablonu için argümanlar.</param>
        void LogInfo(string message, params object[] args);

        /// <summary>
        /// Uyarı seviyesinde log mesajı yazar.
        /// </summary>
        /// <param name="message">Log mesajı veya mesaj şablonu.</param>
        /// <param name="args">Mesaj şablonu için argümanlar.</param>
        void LogWarn(string message, params object[] args);

        /// <summary>
        /// Hata ayıklama seviyesinde log mesajı yazar.
        /// </summary>
        /// <param name="message">Log mesajı veya mesaj şablonu.</param>
        /// <param name="args">Mesaj şablonu için argümanlar.</param>
        void LogDebug(string message, params object[] args);

        /// <summary>
        /// Hata seviyesinde log mesajı yazar.
        /// </summary>
        /// <param name="message">Log mesajı veya mesaj şablonu.</param>
        /// <param name="args">Mesaj şablonu için argümanlar.</param>
        void LogError(string message, params object[] args);

        /// <summary>
        /// Hata seviyesinde istisna (exception) ile birlikte log mesajı yazar.
        /// </summary>
        /// <param name="ex">Loglanacak istisna.</param>
        /// <param name="message">Log mesajı veya mesaj şablonu.</param>
        /// <param name="args">Mesaj şablonu için argümanlar.</param>
        void LogError(Exception ex, string message, params object[] args);
    }
} 