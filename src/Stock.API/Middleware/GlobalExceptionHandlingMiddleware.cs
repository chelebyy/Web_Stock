using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Stock.Domain.Exceptions; // Doğru using ifadesi eklendi
using Stock.Application.Constants;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;

namespace Stock.API.Middleware
{
    /// <summary>
    /// Uygulama genelinde oluşan hataları merkezi bir şekilde yöneten middleware sınıfı.
    /// </summary>
    public class GlobalExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        /// <summary>
        /// GlobalExceptionHandlingMiddleware sınıfının yapıcı metodu.
        /// </summary>
        /// <param name="logger">Loglama için ILogger nesnesi.</param>
        /// <param name="env">Uygulama ortamı bilgisi için IWebHostEnvironment nesnesi.</param>
        public GlobalExceptionHandlingMiddleware(
            ILogger<GlobalExceptionHandlingMiddleware> logger,
            IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        /// <summary>
        /// HTTP isteğini işler ve oluşan hataları yakalar.
        /// </summary>
        /// <param name="context">HTTP bağlam nesnesi.</param>
        /// <param name="next">İstek işleme pipeline'ında bir sonraki middleware.</param>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "İşlenmemiş bir hata oluştu");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            
            var response = new
            {
                Status = "Error",
                Message = GetUserFriendlyMessage(exception),
                DetailedMessage = _env.IsDevelopment() ? exception.ToString() : null
            };

            context.Response.StatusCode = GetStatusCode(exception);

            var result = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(result);
        }

        private string GetUserFriendlyMessage(Exception exception)
        {
            return exception switch
            {
                NotFoundException or KeyNotFoundException => exception.Message,
                FluentValidation.ValidationException => ErrorMessages.ValidationError,
                UnauthorizedAccessException => ErrorMessages.Unauthorized,
                _ => _env.IsDevelopment() ? exception.Message : ErrorMessages.GeneralServerError
            };
        }

        private int GetStatusCode(Exception exception)
        {
            return exception switch
            {
                NotFoundException or KeyNotFoundException => (int)HttpStatusCode.NotFound,
                FluentValidation.ValidationException => (int)HttpStatusCode.BadRequest,
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                _ => (int)HttpStatusCode.InternalServerError
            };
        }
    }
} 