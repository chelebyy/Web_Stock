using Microsoft.AspNetCore.Builder;

namespace Stock.API.Middleware
{
    /// <summary>
    /// Middleware extension methods
    /// </summary>
    public static class MiddlewareExtensions
    {
        /// <summary>
        /// Configure global exception handler middleware
        /// </summary>
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionMiddleware>();
        }
    }
} 