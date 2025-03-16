using System.Reflection;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
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
            
            return services;
        }
    }
} 