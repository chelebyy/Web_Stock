# Merkezi Loglama Mekanizması

## Genel Bakış

Bu belge, Stock uygulamasında uygulanan merkezi loglama mekanizmasını açıklamaktadır. Merkezi loglama mekanizması, uygulamanın farklı bölümlerinden gelen log mesajlarını tutarlı bir şekilde yönetmek, depolamak ve analiz etmek için tasarlanmıştır.

## Bileşenler

### 1. ILoggerManager Arayüzü

`ILoggerManager` arayüzü, uygulamanın farklı bölümlerinde tutarlı bir loglama API'si sağlar:

```csharp
public interface ILoggerManager
{
    void LogInfo(string message, params object[] args);
    void LogWarn(string message, params object[] args);
    void LogDebug(string message, params object[] args);
    void LogError(string message, params object[] args);
    void LogError(Exception ex, string message, params object[] args);
}
```

### 2. LoggerManager Sınıfı

`LoggerManager<T>` sınıfı, `ILoggerManager` arayüzünü uygular ve Microsoft.Extensions.Logging'i kullanarak log mesajlarını yönetir:

```csharp
public class LoggerManager<T> : ILoggerManager where T : class
{
    private readonly ILogger<T> _logger;

    public LoggerManager(ILogger<T> logger)
    {
        _logger = logger;
    }

    public void LogInfo(string message, params object[] args)
    {
        _logger.LogInformation(message, args);
    }

    // Diğer metotlar...
}
```

### 3. NLog Yapılandırması

NLog yapılandırması `nlog.config` dosyasında tanımlanmıştır:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
      autoReload="true">
  
  <targets>
    <target name="allfile" xsi:type="File" fileName="logs/all-${shortdate}.log" />
    <target name="errorfile" xsi:type="File" fileName="logs/error-${shortdate}.log" />
    <target name="performancefile" xsi:type="File" fileName="logs/performance-${shortdate}.log" />
    <target name="console" xsi:type="Console" />
  </targets>

  <rules>
    <logger name="*" minlevel="Debug" writeTo="allfile" />
    <logger name="*" minlevel="Error" writeTo="errorfile" />
    <logger name="*" levels="Info" writeTo="performancefile">
      <filters>
        <when condition="not starts-with('${message}','Performance:')" action="Ignore" />
      </filters>
    </logger>
    <logger name="*" minlevel="Info" writeTo="console" />
  </rules>
</nlog>
```

### 4. Program.cs'de Yapılandırma

```csharp
public static void Main(string[] args)
{
    var logger = NLog.LogManager.Setup()
                              .LoadConfigurationFromFile("nlog.config")
                              .GetCurrentClassLogger();
    try
    {
        logger.Info("Uygulama başlatılıyor...");
        
        var builder = WebApplication.CreateBuilder(args);
        builder.Logging.ClearProviders();
        builder.Host.UseNLog();
        
        // ...
    }
    catch (Exception ex)
    {
        logger.Error(ex, "Uygulama başlatılırken bir hata oluştu");
        throw;
    }
    finally
    {
        LogManager.Shutdown();
    }
}
```

## Kullanım Örnekleri

### 1. Servislerde Kullanım

```csharp
public class UserService
{
    private readonly ILoggerManager<UserService> _logger;

    public UserService(ILoggerManager<UserService> logger)
    {
        _logger = logger;
    }

    public async Task<User> GetUserByIdAsync(int id)
    {
        try
        {
            _logger.LogInfo("GetUserByIdAsync çağrıldı. UserId: {0}", id);
            var user = await _userRepository.GetByIdAsync(id);
            
            if (user == null)
            {
                _logger.LogWarn("Kullanıcı bulunamadı. UserId: {0}", id);
                return null;
            }

            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kullanıcı getirilirken hata oluştu. UserId: {0}", id);
            throw;
        }
    }
}
```

### 2. Middleware'de Kullanım

```csharp
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILoggerManager<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILoggerManager<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "İşlenmemiş bir hata oluştu");
            await HandleExceptionAsync(context, ex);
        }
    }
}
```

## Log Dosyaları

Uygulama aşağıdaki log dosyalarını oluşturur:

1. **all-{date}.log**: Tüm log mesajlarını içerir (Debug ve üzeri seviyeler)
2. **error-{date}.log**: Sadece hata mesajlarını içerir (Error seviyesi)
3. **performance-{date}.log**: Performans ölçümlerini içerir
4. **internal-nlog.txt**: NLog'un kendi iç logları

## Log Seviyeleri

1. **Debug**: Geliştirme sırasında faydalı olan detaylı bilgiler
2. **Info**: Normal operasyonel bilgiler
3. **Warn**: Potansiyel sorunlar veya dikkat edilmesi gereken durumlar
4. **Error**: Uygulama akışını bozan hatalar

## Performans ve Bakım

1. **Log Rotasyonu**: Log dosyaları günlük olarak oluşturulur
2. **Disk Alanı**: Log dosyaları düzenli olarak arşivlenmeli/silinmelidir
3. **Performans Etkisi**: Debug seviyesi loglar production ortamında kapatılmalıdır

## Güvenlik Notları

1. Hassas bilgiler (şifreler, tokenlar vb.) loglanmamalıdır
2. Log dosyalarına erişim kısıtlanmalıdır
3. Kişisel veriler KVKK/GDPR uyumlu şekilde loglanmalıdır 