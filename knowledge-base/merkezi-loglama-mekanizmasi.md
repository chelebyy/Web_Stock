# Merkezi Loglama Mekanizması ve Global Exception Handler

Bu belge, Stock uygulamasında uygulanan merkezi loglama mekanizması ve global exception handler'ın tasarımını, uygulanmasını ve kullanımını açıklar.

## İçindekiler

1. [Genel Bakış](#genel-bakış)
2. [Merkezi Loglama Mekanizması](#merkezi-loglama-mekanizması)
   - [NLog Yapılandırması](#nlog-yapılandırması)
   - [ILoggerManager Arayüzü](#iloggermanager-arayüzü)
   - [LoggerManager Sınıfı](#loggermanager-sınıfı)
3. [Global Exception Handler](#global-exception-handler)
   - [ExceptionMiddleware](#exceptionmiddleware)
   - [Middleware Entegrasyonu](#middleware-entegrasyonu)
4. [Kullanım Örnekleri](#kullanım-örnekleri)
5. [Faydaları](#faydaları)
6. [Gelecek İyileştirmeler](#gelecek-iyileştirmeler)

## Genel Bakış

Merkezi loglama mekanizması ve global exception handler, uygulamanın hata yönetimini ve izlenebilirliğini iyileştirmek için uygulanmıştır. Bu iyileştirmeler, aşağıdaki faydaları sağlar:

- Tutarlı ve merkezi loglama
- Yapılandırılabilir log seviyeleri
- Log rotasyonu ve arşivleme
- Beklenmeyen hataların merkezi yönetimi
- Kullanıcı dostu hata mesajları

## Merkezi Loglama Mekanizması

### NLog Yapılandırması

NLog, uygulamanın loglama ihtiyaçlarını karşılamak için kullanılan bir loglama kütüphanesidir. `nlog.config` dosyası, loglama hedeflerini, kuralları ve formatları tanımlar:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
      autoReload="true"
      internalLogLevel="Info"
      internalLogFile="logs/internal-nlog.txt">

  <!-- NLog'un dahili loglarını etkinleştir -->
  <extensions>
    <add assembly="NLog.Web.AspNetCore"/>
  </extensions>

  <!-- Log hedefleri tanımla -->
  <targets>
    <!-- Dosya hedefi - tüm loglar -->
    <target xsi:type="File" name="allfile" 
            fileName="logs/all-${shortdate}.log"
            layout="${longdate}|${event-properties:item=EventId_Id:whenEmpty=0}|${uppercase:${level}}|${logger}|${message} ${exception:format=tostring}" 
            archiveFileName="logs/archives/all-${shortdate}-{#}.log"
            archiveEvery="Day"
            archiveNumbering="Rolling"
            maxArchiveFiles="7"
            concurrentWrites="true"
            keepFileOpen="false" />

    <!-- Dosya hedefi - sadece hata logları -->
    <target xsi:type="File" name="error" 
            fileName="logs/error-${shortdate}.log"
            layout="${longdate}|${event-properties:item=EventId_Id:whenEmpty=0}|${uppercase:${level}}|${logger}|${message} ${exception:format=tostring}|${callsite}|${stacktrace}" 
            archiveFileName="logs/archives/error-${shortdate}-{#}.log"
            archiveEvery="Day"
            archiveNumbering="Rolling"
            maxArchiveFiles="30"
            concurrentWrites="true"
            keepFileOpen="false" />

    <!-- Konsol hedefi -->
    <target xsi:type="Console" name="console" 
            layout="${date:format=HH\:mm\:ss}|${event-properties:item=EventId_Id:whenEmpty=0}|${uppercase:${level}}|${logger}|${message} ${exception:format=tostring}" />
  </targets>

  <!-- Log kuralları -->
  <rules>
    <!-- Tüm logları dosyaya yaz -->
    <logger name="*" minlevel="Info" writeTo="allfile" />
    
    <!-- Hata loglarını ayrı dosyaya yaz -->
    <logger name="*" minlevel="Error" writeTo="error" />
    
    <!-- Konsola yaz -->
    <logger name="*" minlevel="Info" writeTo="console" />
    
    <!-- Microsoft loglarını filtrele -->
    <logger name="Microsoft.*" maxlevel="Info" final="true" />
  </rules>
</nlog>
```

### ILoggerManager Arayüzü

`ILoggerManager` arayüzü, uygulamanın loglama ihtiyaçlarını karşılamak için tasarlanmıştır:

```csharp
public interface ILoggerManager
{
    void LogDebug(string message);
    void LogDebug(string message, Exception exception);
    void LogInfo(string message);
    void LogInfo(string message, Exception exception);
    void LogWarn(string message);
    void LogWarn(string message, Exception exception);
    void LogError(string message);
    void LogError(string message, Exception exception);
}
```

### LoggerManager Sınıfı

`LoggerManager` sınıfı, `ILoggerManager` arayüzünü uygular ve NLog kütüphanesini kullanarak logları yönetir:

```csharp
public class LoggerManager : ILoggerManager
{
    private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

    public void LogDebug(string message)
    {
        logger.Debug(message);
    }

    public void LogDebug(string message, Exception exception)
    {
        logger.Debug(exception, message);
    }

    // Diğer metotlar...
}
```

## Global Exception Handler

### ExceptionMiddleware

`ExceptionMiddleware`, beklenmeyen hataları yakalamak ve uygun şekilde işlemek için tasarlanmıştır:

```csharp
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILoggerManager _logger;

    public ExceptionMiddleware(RequestDelegate next, ILoggerManager logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Beklenmeyen bir hata oluştu: {ex.Message}", ex);
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var errorDetails = new ErrorDetails
        {
            StatusCode = context.Response.StatusCode,
            Message = "İşlem sırasında bir hata oluştu.",
            Details = exception.Message
        };

        // Geliştirme ortamında daha detaylı hata bilgisi
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
        {
            errorDetails.StackTrace = exception.StackTrace;
        }

        var json = JsonSerializer.Serialize(errorDetails);
        await context.Response.WriteAsync(json);
    }
}
```

### Middleware Entegrasyonu

Middleware'i uygulamaya eklemek için extension method:

```csharp
public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionMiddleware>();
    }
}
```

Program.cs'de middleware'in kullanımı:

```csharp
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // Üretim ortamında global exception handler kullan
    app.UseGlobalExceptionHandler();
}
```

## Kullanım Örnekleri

Controller'larda loglama kullanımı:

```csharp
[HttpGet]
[Authorize(Roles = "Admin")]
public async Task<IActionResult> GetAll()
{
    _logger.LogInfo("Tüm kullanıcılar getiriliyor.");
    var query = new GetAllUsersQuery();
    var result = await _mediator.Send(query);
    _logger.LogInfo($"{result.Count} kullanıcı başarıyla getirildi.");
    return Ok(result);
}
```

## Faydaları

1. **Tutarlı Loglama**: Tüm uygulama genelinde tutarlı bir loglama yaklaşımı sağlar.
2. **Merkezi Hata Yönetimi**: Beklenmeyen hataları merkezi bir şekilde yakalar ve işler.
3. **Gelişmiş İzlenebilirlik**: Uygulamanın davranışını ve performansını izlemek için daha fazla bilgi sağlar.
4. **Yapılandırılabilir Log Seviyeleri**: Farklı ortamlar için farklı log seviyeleri yapılandırılabilir.
5. **Log Rotasyonu ve Arşivleme**: Logların otomatik olarak döndürülmesi ve arşivlenmesi sağlanır.
6. **Kullanıcı Dostu Hata Mesajları**: Son kullanıcılara daha anlaşılır hata mesajları sunulur.

## Gelecek İyileştirmeler

1. **Yapılandırılabilir Log Formatları**: Farklı log hedefleri için özelleştirilebilir log formatları.
2. **Log Filtreleme**: Belirli log türlerini filtrelemek için daha gelişmiş mekanizmalar.
3. **Performans İzleme**: Uygulama performansını izlemek için özel log olayları.
4. **Özel Hata Tipleri**: Farklı hata türleri için özel işleyiciler.
5. **Entegrasyon**: Serilog, Elasticsearch, Kibana gibi diğer loglama ve izleme araçlarıyla entegrasyon. 