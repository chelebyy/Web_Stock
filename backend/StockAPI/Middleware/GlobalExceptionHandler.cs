using Microsoft.AspNetCore.Http;
using StockAPI.Exceptions;
using StockAPI.Services;
using System.Net;
using System.Text.Json;

namespace StockAPI.Middleware;

public class GlobalExceptionHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public GlobalExceptionHandler(
        RequestDelegate next,
        ILogger<GlobalExceptionHandler> logger,
        IServiceScopeFactory serviceScopeFactory)
    {
        _next = next;
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Beklenmeyen hata oluştu: {ErrorMessage}\nStack Trace: {StackTrace}", ex.Message, ex.StackTrace);
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var userId = context.User?.Identity?.Name ?? "anonymous";
        var path = context.Request.Path;

        var (statusCode, userMessage) = exception switch
        {
            ApiException apiException => ((int)apiException.StatusCode, apiException.UserMessage),
            ArgumentException _ => (StatusCodes.Status400BadRequest, "Geçersiz argüman."),
            UnauthorizedAccessException _ => (StatusCodes.Status401Unauthorized, "Yetkisiz erişim."),
            _ => (StatusCodes.Status500InternalServerError, "Bir hata oluştu. Lütfen daha sonra tekrar deneyin.")
        };

        var response = new
        {
            StatusCode = statusCode,
            Message = userMessage,
            Path = path,
            Timestamp = DateTime.UtcNow,
            Details = exception.Message
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var auditLogger = scope.ServiceProvider.GetRequiredService<IAuditLogger>();
            try
            {
                await auditLogger.LogActionAsync(
                    userId: userId,
                    action: "ERROR",
                    entityId: exception.GetHashCode().ToString(),
                    entityType: "Exception",
                    path: path,
                    details: JsonSerializer.Serialize(new { 
                        ExceptionType = exception.GetType().Name,
                        ExceptionMessage = exception.Message,
                        StackTrace = exception.StackTrace
                    }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Hata loglanırken bir sorun oluştu");
            }
        }

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
