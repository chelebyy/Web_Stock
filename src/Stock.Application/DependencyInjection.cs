using System.Reflection;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Stock.Application.Common.Behaviors;
using Stock.Application.Common.Mappings;

namespace Stock.Application
{
    /// <summary>
    /// Application katmanı için DI yapılandırması
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Application servislerini DI container'a kaydeder
        /// </summary>
        /// <param name="services">Servis koleksiyonu</param>
        /// <returns>Servis koleksiyonu</returns>
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // AutoMapper yapılandırması
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            
            // MediatR yapılandırması
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            
            // MediatR davranışlarını kaydet
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            
            // FluentValidation yapılandırması
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            
            return services;
        }
    }
} 