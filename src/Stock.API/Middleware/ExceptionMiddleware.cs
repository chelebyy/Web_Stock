using Microsoft.AspNetCore.Http;
using Stock.Infrastructure.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Stock.API.Middleware
{
    /// <summary>
    /// Global exception handler middleware
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerManager _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        public ExceptionMiddleware(RequestDelegate next, ILoggerManager logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Invoke middleware
        /// </summary>
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

        /// <summary>
        /// Handle exception
        /// </summary>
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

    /// <summary>
    /// Error details model
    /// </summary>
    public class ErrorDetails
    {
        /// <summary>
        /// HTTP status code
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Error message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Error details
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// Stack trace (only in development)
        /// </summary>
        public string StackTrace { get; set; }
    }
} 