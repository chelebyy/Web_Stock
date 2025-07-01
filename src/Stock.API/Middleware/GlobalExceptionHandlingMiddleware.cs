using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stock.Domain.Exceptions;
using Stock.Application.Constants;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Stock.API.Middleware
{
    /// <summary>
    /// Provides centralized exception handling for the application.
    /// This middleware catches unhandled exceptions and formats them into a standardized RFC 7807 ProblemDetails response.
    /// </summary>
    public class GlobalExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalExceptionHandlingMiddleware"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="env">The hosting environment instance.</param>
        public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        /// <summary>
        /// Processes the HTTP request and handles any exceptions that occur.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <param name="next">The next middleware in the request pipeline.</param>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception has occurred: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/problem+json";

            var problemDetails = CreateProblemDetails(context, exception);

            var result = JsonSerializer.Serialize(problemDetails, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });
            await context.Response.WriteAsync(result);
        }

        private ProblemDetails CreateProblemDetails(HttpContext context, Exception exception)
        {
            var problemDetails = new ProblemDetails
            {
                Instance = context.Request.Path
            };

            switch (exception)
            {
                case FluentValidation.ValidationException validationException:
                    problemDetails.Title = "Validation Error";
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Detail = "One or more validation errors occurred.";
                    problemDetails.Extensions["errors"] = validationException.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
                    break;
                case NotFoundException notFoundException:
                    problemDetails.Title = "Resource Not Found";
                    problemDetails.Status = StatusCodes.Status404NotFound;
                    problemDetails.Detail = notFoundException.Message;
                    break;
                case ConflictException conflictException:
                    problemDetails.Title = "Conflict";
                    problemDetails.Status = StatusCodes.Status409Conflict;
                    problemDetails.Detail = conflictException.Message;
                    break;
                case UnauthorizedAccessException unauthorizedAccessException:
                    problemDetails.Title = "Unauthorized Access";
                    problemDetails.Status = StatusCodes.Status401Unauthorized;
                    problemDetails.Detail = unauthorizedAccessException.Message;
                    break;
                default:
                    problemDetails.Title = "An unexpected error occurred.";
                    problemDetails.Status = StatusCodes.Status500InternalServerError;
                    problemDetails.Detail = _env.IsDevelopment() ? exception.ToString() : "An internal server error has occurred.";
                    break;
            }

            context.Response.StatusCode = problemDetails.Status.Value;
            
            return problemDetails;
        }
    }
} 