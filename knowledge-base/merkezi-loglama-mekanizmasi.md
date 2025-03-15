# Merkezi Loglama Mekanizması

## Genel Bakış

Bu belge, Stock uygulamasında uygulanan merkezi loglama mekanizmasını açıklamaktadır. Merkezi loglama mekanizması, uygulamanın farklı bölümlerinden gelen log mesajlarını tutarlı bir şekilde yönetmek, depolamak ve analiz etmek için tasarlanmıştır.

## Bileşenler

### 1. NLog Entegrasyonu

NLog, .NET uygulamaları için esnek ve yapılandırılabilir bir loglama çerçevesidir. Aşağıdaki özellikleri sağlar:

- Farklı log hedeflerine (dosya, konsol, veritabanı) yazma
- Log rotasyonu ve arşivleme
- Yapılandırılabilir log formatları
- Farklı log seviyeleri (Debug, Info, Warn, Error, Fatal)

### 2. ILoggerManager Arayüzü

`ILoggerManager` arayüzü, uygulamanın farklı bölümlerinde tutarlı bir loglama API'si sağlar:

```csharp
public interface ILoggerManager
{
    void LogDebug(string message);
    void LogInfo(string message);
    void LogWarn(string message);
    void LogError(string message);
    void LogError(string message, Exception ex);
    void LogPerformance(string operation, long elapsedMilliseconds, string additionalInfo = null);
}
```

### 3. LoggerManager Sınıfı

`LoggerManager` sınıfı, `ILoggerManager` arayüzünü uygular ve NLog'u kullanarak log mesajlarını yönetir:

```csharp
public class LoggerManager : ILoggerManager
{
    private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

    public void LogDebug(string message)
    {
        logger.Debug(message);
    }

    public void LogInfo(string message)
    {
        logger.Info(message);
    }

    public void LogWarn(string message)
    {
        logger.Warn(message);
    }

    public void LogError(string message)
    {
        logger.Error(message);
    }

    public void LogError(string message, Exception ex)
    {
        logger.Error(ex, message);
    }

    public void LogPerformance(string operation, long elapsedMilliseconds, string additionalInfo = null)
    {
        string message = $"Performance: {operation} completed in {elapsedMilliseconds}ms";
        if (!string.IsNullOrEmpty(additionalInfo))
        {
            message += $" | {additionalInfo}";
        }
        logger.Info(message);
    }
}
```

### 4. Global Exception Handler Middleware

`ExceptionMiddleware`, beklenmeyen hataları merkezi olarak yakalamak ve loglamak için kullanılır:

```csharp
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILoggerManager _logger;
    private readonly IWebHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, ILoggerManager logger, IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Beklenmeyen hata: {ex.Message}", ex);
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = _env.IsDevelopment()
            ? new ApiException(context.Response.StatusCode, exception.Message, exception.StackTrace?.ToString())
            : new ApiException(context.Response.StatusCode, "Internal Server Error");

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var json = JsonSerializer.Serialize(response, options);

        await context.Response.WriteAsync(json);
    }
}
```

## Yapılandırma

### nlog.config

NLog yapılandırması `nlog.config` dosyasında tanımlanmıştır:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
      autoReload="true"
      internalLogLevel="Info"
      internalLogFile="logs/internal-nlog.txt">

  <!-- Log hedefleri tanımları -->
  <targets>
    <!-- Dosya hedefi -->
    <target xsi:type="File" name="allfile" fileName="logs/all-${shortdate}.log"
            layout="${longdate}|${event-properties:item=EventId_Id:whenEmpty=0}|${uppercase:${level}}|${logger}|${message} ${exception:format=tostring}" />

    <!-- Hata dosyası hedefi -->
    <target xsi:type="File" name="errorfile" fileName="logs/error-${shortdate}.log"
            layout="${longdate}|${event-properties:item=EventId_Id:whenEmpty=0}|${uppercase:${level}}|${logger}|${message} ${exception:format=tostring}" />

    <!-- Performans dosyası hedefi -->
    <target xsi:type="File" name="performancefile" fileName="logs/performance-${shortdate}.log"
            layout="${longdate}|${message}" />

    <!-- Konsol hedefi -->
    <target xsi:type="Console" name="console"
            layout="${date:format=HH\:mm\:ss}|${uppercase:${level}}|${message} ${exception:format=message}" />
  </targets>

  <!-- Log kuralları -->
  <rules>
    <!-- Tüm logları dosyaya yaz -->
    <logger name="*" minlevel="Debug" writeTo="allfile" />

    <!-- Hata loglarını ayrı dosyaya yaz -->
    <logger name="*" minlevel="Error" writeTo="errorfile" />

    <!-- Performans loglarını ayrı dosyaya yaz -->
    <logger name="*" levels="Info" writeTo="performancefile">
      <filters>
        <when condition="not starts-with('${message}','Performance:')" action="Ignore" />
      </filters>
    </logger>

    <!-- Konsola yaz -->
    <logger name="*" minlevel="Info" writeTo="console" />
  </rules>
</nlog>
```

### Program.cs'de Yapılandırma

Loglama mekanizması, `Program.cs` dosyasında yapılandırılır:

```csharp
// NLog yapılandırmasını yükle
LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

// Servis kayıtları
builder.Services.AddSingleton<ILoggerManager, LoggerManager>();

// Middleware kayıtları
app.UseMiddleware<ExceptionMiddleware>();
```

## Kullanım Örnekleri

### Controller'larda Kullanım

```csharp
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILoggerManager _logger;

    public UsersController(IMediator mediator, ILoggerManager logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
    {
        _logger.LogInfo("GetAllUsers endpoint çağrıldı");
        
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            var query = new GetAllUsersQuery();
            var result = await _mediator.Send(query);
            
            stopwatch.Stop();
            _logger.LogPerformance("GetAllUsers", stopwatch.ElapsedMilliseconds, $"Toplam {result.Count()} kullanıcı getirildi");
            
            // Performans bilgisini response header'a ekle
            Response.Headers.Add("X-Performance-MS", stopwatch.ElapsedMilliseconds.ToString());
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError("GetAllUsers işlemi sırasında hata oluştu", ex);
            throw;
        }
    }
}
```

### Query Handler'larda Kullanım

```csharp
public class GetPaginatedUsersQueryHandler : IRequestHandler<GetPaginatedUsersQuery, PaginatedResult<UserDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;

    public GetPaginatedUsersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILoggerManager logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PaginatedResult<UserDto>> Handle(GetPaginatedUsersQuery request, CancellationToken cancellationToken)
    {
        _logger.LogDebug($"Sayfalı kullanıcı sorgusu işleniyor: Sayfa={request.PageNumber}, Boyut={request.PageSize}");
        
        var stopwatch = Stopwatch.StartNew();
        
        var (users, totalCount) = await _unitOfWork.Users.GetPaginatedUsersAsync(
            request.PageNumber, 
            request.PageSize,
            cancellationToken);
            
        var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);
        
        stopwatch.Stop();
        _logger.LogPerformance("GetPaginatedUsers", stopwatch.ElapsedMilliseconds, 
            $"Sayfa={request.PageNumber}, Boyut={request.PageSize}, Toplam={totalCount}, Getirilen={userDtos.Count()}");
        
        return new PaginatedResult<UserDto>
        {
            Items = userDtos,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}
```

## Log Dosyaları

Loglama mekanizması, aşağıdaki log dosyalarını oluşturur:

1. **all-{date}.log**: Tüm log mesajlarını içerir (Debug, Info, Warn, Error)
2. **error-{date}.log**: Sadece hata mesajlarını içerir (Error ve üstü)
3. **performance-{date}.log**: Performans ölçümlerini içerir
4. **internal-nlog.txt**: NLog'un kendi iç loglarını içerir

## Log Rotasyonu ve Arşivleme

Log dosyaları günlük olarak oluşturulur ve tarih bilgisi dosya adına eklenir. Eski log dosyaları otomatik olarak arşivlenir ve belirli bir süre sonra silinir.

## Faydalar

Merkezi loglama mekanizması aşağıdaki faydaları sağlar:

1. **Tutarlılık**: Tüm uygulama bileşenleri aynı loglama API'sini kullanır
2. **Merkezi Yapılandırma**: Loglama davranışı tek bir yerden yapılandırılabilir
3. **Esnek Log Hedefleri**: Loglar farklı hedeflere (dosya, konsol, veritabanı) yazılabilir
4. **Performans İzleme**: İşlem sürelerini ölçmek ve izlemek için özel metotlar
5. **Hata Yönetimi**: Beklenmeyen hatalar merkezi olarak yakalanır ve loglanır
6. **Log Rotasyonu**: Otomatik log rotasyonu ve arşivleme

## Sonuç

Merkezi loglama mekanizması, Stock uygulamasının izlenebilirliğini ve hata ayıklama yeteneklerini önemli ölçüde artırmıştır. NLog entegrasyonu, esnek ve yapılandırılabilir bir loglama çözümü sağlarken, özel arayüz ve sınıflar, uygulamanın farklı bölümlerinde tutarlı bir loglama API'si sağlar. 