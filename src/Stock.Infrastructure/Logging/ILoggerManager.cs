using System;

namespace Stock.Infrastructure.Logging
{
    /// <summary>
    /// Uygulama genelinde kullanılacak merkezi loglama arayüzü
    /// </summary>
    public interface ILoggerManager
    {
        /// <summary>
        /// Debug seviyesinde log kaydı oluşturur
        /// </summary>
        void LogDebug(string message);
        
        /// <summary>
        /// Debug seviyesinde exception ile birlikte log kaydı oluşturur
        /// </summary>
        void LogDebug(string message, Exception exception);
        
        /// <summary>
        /// Info seviyesinde log kaydı oluşturur
        /// </summary>
        void LogInfo(string message);
        
        /// <summary>
        /// Info seviyesinde exception ile birlikte log kaydı oluşturur
        /// </summary>
        void LogInfo(string message, Exception exception);
        
        /// <summary>
        /// Warning seviyesinde log kaydı oluşturur
        /// </summary>
        void LogWarn(string message);
        
        /// <summary>
        /// Warning seviyesinde exception ile birlikte log kaydı oluşturur
        /// </summary>
        void LogWarn(string message, Exception exception);
        
        /// <summary>
        /// Error seviyesinde log kaydı oluşturur
        /// </summary>
        void LogError(string message);
        
        /// <summary>
        /// Error seviyesinde exception ile birlikte log kaydı oluşturur
        /// </summary>
        void LogError(string message, Exception exception);
    }
} 