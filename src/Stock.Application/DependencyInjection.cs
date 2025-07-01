using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Stock.Application.Features.Auth;
using Stock.Application.Features.Products.Services;
using System.Reflection;

namespace Stock.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Register AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Register MediatR
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            // Register FluentValidation
            var assembly = Assembly.GetExecutingAssembly();
            var validatorType = typeof(IValidator<>);
            var validatorTypes = assembly.GetTypes()
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == validatorType))
                .ToList();

            foreach (var validator in validatorTypes)
            {
                var validatorInterface = validator.GetInterfaces()
                    .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == validatorType);
                services.AddTransient(validatorInterface, validator);
            }

            // Register Application Services
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<AuthService>();

            return services;
        }
    }
} 