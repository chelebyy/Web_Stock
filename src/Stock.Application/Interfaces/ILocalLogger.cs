using System;

namespace Stock.Application.Interfaces
{
    /// <summary>
    /// Uygulama için özel loglama arayüzü.
    /// </summary>
    public interface ILocalLogger
    {
        void LogInformation(string message, params object[] args);
        void LogWarning(string message, params object[] args);
        void LogError(Exception exception, string message, params object[] args);
        // Gerekirse başka log seviyeleri (Debug, Trace, Critical) eklenebilir.
    }
} 