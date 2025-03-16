using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using Stock.Application.Common.CQRS;

namespace Stock.Infrastructure.CQRS
{
    /// <summary>
    /// CQRS bileşenlerini DI container'a kaydetmek için extension method'lar
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// CQRS bileşenlerini DI container'a kaydeder
        /// </summary>
        /// <param name="services">Servis koleksiyonu</param>
        /// <returns>Servis koleksiyonu</returns>
        public static IServiceCollection AddCQRS(this IServiceCollection services)
        {
            // Mediator'ı kaydet
            services.AddScoped<IMediator, Mediator>();

            // Command ve Query handler'ları kaydet
            var applicationAssembly = Assembly.GetAssembly(typeof(ICommand<>));
            var infrastructureAssembly = Assembly.GetAssembly(typeof(Mediator));
            
            // Command handler'ları kaydet
            services.Scan(scan => scan
                .FromAssemblies(applicationAssembly, infrastructureAssembly)
                .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());
            
            // Query handler'ları kaydet
            services.Scan(scan => scan
                .FromAssemblies(applicationAssembly, infrastructureAssembly)
                .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());
            
            return services;
        }
    }
} 