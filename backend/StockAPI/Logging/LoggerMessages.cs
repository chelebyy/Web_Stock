using Microsoft.Extensions.Logging;

namespace StockAPI.Logging;

public static class LoggerMessages
{
    private static readonly Action<ILogger, string, Exception?> _userLoginAttempt =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(1000, nameof(UserLoginAttempt)),
            "Kullanıcı giriş denemesi - Kullanıcı: {Username}");

    private static readonly Action<ILogger, string, Exception?> _userLoginSuccess =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(1001, nameof(UserLoginSuccess)),
            "Kullanıcı girişi başarılı - Kullanıcı: {Username}");

    private static readonly Action<ILogger, string, Exception?> _userLoginFailed =
        LoggerMessage.Define<string>(
            LogLevel.Warning,
            new EventId(1002, nameof(UserLoginFailed)),
            "Kullanıcı girişi başarısız - Kullanıcı: {Username}");

    private static readonly Action<ILogger, string, Exception?> _userCreated =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(1003, nameof(UserCreated)),
            "Yeni kullanıcı oluşturuldu - Kullanıcı: {Username}");

    private static readonly Action<ILogger, string, Exception?> _userUpdated =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(1004, nameof(UserUpdated)),
            "Kullanıcı güncellendi - Kullanıcı: {Username}");

    private static readonly Action<ILogger, string, Exception?> _userDeleted =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(1005, nameof(UserDeleted)),
            "Kullanıcı silindi - Kullanıcı: {Username}");

    public static void UserLoginAttempt(this ILogger logger, string username)
        => _userLoginAttempt(logger, username, null);

    public static void UserLoginSuccess(this ILogger logger, string username)
        => _userLoginSuccess(logger, username, null);

    public static void UserLoginFailed(this ILogger logger, string username)
        => _userLoginFailed(logger, username, null);

    public static void UserCreated(this ILogger logger, string username)
        => _userCreated(logger, username, null);

    public static void UserUpdated(this ILogger logger, string username)
        => _userUpdated(logger, username, null);

    public static void UserDeleted(this ILogger logger, string username)
        => _userDeleted(logger, username, null);
} 