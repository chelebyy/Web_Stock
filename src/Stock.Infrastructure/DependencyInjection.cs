using System;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stock.Application.Common.Interfaces;
using Stock.Domain.Interfaces;
using Stock.Infrastructure.Data;
using Stock.Infrastructure.Logging;
using Stock.Infrastructure.Repositories;
using Stock.Infrastructure.Services;
using Stock.Infrastructure.UnitOfWork;
using Stock.Infrastructure.Caching;
using Stock.Infrastructure.BackgroundServices;
using Stock.Infrastructure.CQRS;

namespace Stock.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Database Context
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

            // Services
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IUserPermissionService, UserPermissionService>();
            services.AddScoped<IAuditService, AuditService>();
            services.AddScoped<IReportService, ReportService>();

            // Logging
            services.AddSingleton<ILoggerManager, LoggerManager>();
            
            // Caching
            services.AddMemoryCache();
            services.AddSingleton<ICacheService, MemoryCacheService>();

            // BackgroundService'leri ekle
            services.AddHostedService<ReportGenerationService>();

            // CQRS bileşenlerini ekle
            services.AddCQRS();

            // Thread Pool yapılandırması
            ConfigureThreadPool();

            return services;
        }

        private static void ConfigureThreadPool()
        {
            // Minimum iş parçacığı sayısını ayarla
            ThreadPool.SetMinThreads(Environment.ProcessorCount * 2, Environment.ProcessorCount);
            
            // Maksimum iş parçacığı sayısını ayarla
            ThreadPool.SetMaxThreads(Environment.ProcessorCount * 8, Environment.ProcessorCount * 4);
        }
    }
} 