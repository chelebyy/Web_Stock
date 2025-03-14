using System;
using NLog;

namespace Stock.Infrastructure.Logging
{
    /// <summary>
    /// NLog kütüphanesini kullanarak ILoggerManager arayüzünü uygulayan sınıf
    /// </summary>
    public class LoggerManager : ILoggerManager
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Debug seviyesinde log kaydı oluşturur
        /// </summary>
        public void LogDebug(string message)
        {
            logger.Debug(message);
        }

        /// <summary>
        /// Debug seviyesinde exception ile birlikte log kaydı oluşturur
        /// </summary>
        public void LogDebug(string message, Exception exception)
        {
            logger.Debug(exception, message);
        }

        /// <summary>
        /// Info seviyesinde log kaydı oluşturur
        /// </summary>
        public void LogInfo(string message)
        {
            logger.Info(message);
        }

        /// <summary>
        /// Info seviyesinde exception ile birlikte log kaydı oluşturur
        /// </summary>
        public void LogInfo(string message, Exception exception)
        {
            logger.Info(exception, message);
        }

        /// <summary>
        /// Warning seviyesinde log kaydı oluşturur
        /// </summary>
        public void LogWarn(string message)
        {
            logger.Warn(message);
        }

        /// <summary>
        /// Warning seviyesinde exception ile birlikte log kaydı oluşturur
        /// </summary>
        public void LogWarn(string message, Exception exception)
        {
            logger.Warn(exception, message);
        }

        /// <summary>
        /// Error seviyesinde log kaydı oluşturur
        /// </summary>
        public void LogError(string message)
        {
            logger.Error(message);
        }

        /// <summary>
        /// Error seviyesinde exception ile birlikte log kaydı oluşturur
        /// </summary>
        public void LogError(string message, Exception exception)
        {
            logger.Error(exception, message);
        }
    }
} 