using System;

namespace Stock.Application.Common.Interfaces
{
    public interface ILoggerManager
    {
        void LogInfo(string message);
        void LogWarn(string message);
        void LogDebug(string message);
        void LogError(string message);
        void LogError(string message, params object[] args);
        void LogError(Exception exception, string message);
    }
} 