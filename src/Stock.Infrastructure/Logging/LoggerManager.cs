using Microsoft.Extensions.Logging;
using Stock.Application.Common.Interfaces;
using System;

namespace Stock.Infrastructure.Logging // Namespace'i Logging olarak ayarladık
{
    /// <summary>
    /// ILoggerManager arayüzünü implemente eden ve Microsoft.Extensions.Logging kullanarak loglama yapan sınıf.
    /// Generic tip parametresi (T) ile hangi sınıf adına loglama yapılacağını belirtir.
    /// </summary>
    /// <typeparam name="T">Loglamanın yapıldığı sınıfın türü.</typeparam>
    public class LoggerManager<T> : ILoggerManager<T> where T : class
    {
        // Microsoft.Extensions.Logging'den gelen ILogger'ı kullanacağız.
        private readonly ILogger<T> _logger;

        /// <summary>
        /// LoggerManager sınıfının yapıcı metodu.
        /// </summary>
        /// <param name="logger">Microsoft.Extensions.Logging.ILogger<T> örneği.</param>
        public LoggerManager(ILogger<T> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Bilgilendirme seviyesinde log mesajı yazar.
        /// </summary>
        /// <param name="message">Log mesajı veya mesaj şablonu.</param>
        /// <param name="args">Mesaj şablonu için argümanlar.</param>
        public void LogInfo(string message, params object[] args)
        {
            _logger.LogInformation(message, args);
        }

        /// <summary>
        /// Uyarı seviyesinde log mesajı yazar.
        /// </summary>
        /// <param name="message">Log mesajı veya mesaj şablonu.</param>
        /// <param name="args">Mesaj şablonu için argümanlar.</param>
        public void LogWarn(string message, params object[] args)
        {
            _logger.LogWarning(message, args);
        }

        /// <summary>
        /// Hata ayıklama seviyesinde log mesajı yazar.
        /// </summary>
        /// <param name="message">Log mesajı veya mesaj şablonu.</param>
        /// <param name="args">Mesaj şablonu için argümanlar.</param>
        public void LogDebug(string message, params object[] args)
        {
            _logger.LogDebug(message, args);
        }

        /// <summary>
        /// Hata seviyesinde log mesajı yazar.
        /// </summary>
        /// <param name="message">Log mesajı veya mesaj şablonu.</param>
        /// <param name="args">Mesaj şablonu için argümanlar.</param>
        public void LogError(string message, params object[] args)
        {
            _logger.LogError(message, args);
        }

        /// <summary>
        /// Hata seviyesinde istisna (exception) ile birlikte log mesajı yazar.
        /// </summary>
        /// <param name="ex">Loglanacak istisna.</param>
        /// <param name="message">Log mesajı veya mesaj şablonu.</param>
        /// <param name="args">Mesaj şablonu için argümanlar.</param>
        public void LogError(Exception ex, string message, params object[] args)
        {
            _logger.LogError(ex, message, args);
        }
    }
} 