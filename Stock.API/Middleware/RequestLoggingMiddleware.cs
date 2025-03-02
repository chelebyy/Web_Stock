using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Stock.API.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation($"İstek {context.Request.Method} {context.Request.Path} başladı");

            var startTime = DateTime.UtcNow;
            try
            {
                await _next(context);
            }
            finally
            {
                var elapsedTime = DateTime.UtcNow - startTime;
                _logger.LogInformation($"İstek {context.Request.Method} {context.Request.Path} {context.Response.StatusCode} durum koduyla {elapsedTime.TotalMilliseconds}ms içinde tamamlandı");
            }
        }
    }
} 