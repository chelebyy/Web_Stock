using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stock.Domain.Interfaces;
using Stock.Infrastructure.Data;
using Stock.Infrastructure.Repositories;
using System;

namespace Stock.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Database
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
            );

            // Repositories
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            
            // Önbellekli repository kullanımı
            services.AddScoped<IRoleRepository, CachedRoleRepository>();
            
            services.AddScoped<IPermissionRepository, PermissionRepository>();

            // Caching
            services.AddMemoryCache();

            return services;
        }
    }
} 