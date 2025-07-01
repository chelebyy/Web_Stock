using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stock.Application.Common.Interfaces;
using Stock.Domain.Interfaces;
using Stock.Infrastructure.Data;
using Stock.Infrastructure.Data.Repositories;
using Stock.Infrastructure.Logging;
using Stock.Infrastructure.Services;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using Stock.Infrastructure.Identity;

namespace Stock.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Caching
            AddCaching(services, configuration);

            // Database
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            // Repositories
            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            // UnitOfWork
            services.AddScoped<IUnitOfWork, Data.UnitOfWork>();
            // Handler'lara doğrudan repository inject edilecekse IUnitOfWork'e gerek kalmayabilir,
            // ancak transaction yönetimi için hala gerekli olabilir. Şimdilik yorumda bırakalım.

            // Cache service is configured in AddCaching method
            // services.AddScoped<ICacheService, CacheService>(); // This is now handled in AddCaching
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IAuditService, AuditService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IAuthorizationService, AuthorizationService>();

            // LoggerManager
            services.AddSingleton(typeof(ILoggerManager<>), typeof(LoggerManager<>));

            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
            services.AddHostedService<QueuedHostedService>();

            services.AddSwaggerGen(options =>
            {
                // Simple static Swagger configuration
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Stock API",
                    Version = "v1",
                    Description = "An ASP.NET Core Web API for managing a stock system"
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });

                // XML comments are disabled for now to avoid file not found errors
                // To enable XML comments, add <GenerateDocumentationFile>true</GenerateDocumentationFile> to csproj
                // var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                // var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                // options.IncludeXmlComments(xmlPath);
            });

            return services;
        }

        private static void AddCaching(IServiceCollection services, IConfiguration configuration)
        {
            // Always add IMemoryCache service first
            services.AddMemoryCache();
            
            var cacheSettings = configuration.GetSection("CacheSettings");
            if (!cacheSettings.Exists() || !cacheSettings.GetValue<bool>("Enabled"))
            {
                services.AddDistributedMemoryCache();
                services.AddSingleton<ICacheService, InMemoryCacheService>();
                return;
            }

            var connectionString = cacheSettings.GetValue<string>("ConnectionString");
            if (string.IsNullOrEmpty(connectionString))
            {
                services.AddDistributedMemoryCache();
                services.AddSingleton<ICacheService, InMemoryCacheService>();
                return;
            }

            try
            {
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = connectionString;
                    options.InstanceName = cacheSettings.GetValue<string>("InstanceName", "StockAPI_");
                });

                // Register IConnectionMultiplexer for direct Redis access
                services.AddSingleton<IConnectionMultiplexer>(sp => 
                    ConnectionMultiplexer.Connect(connectionString));

                // Register CacheService that now depends on IConnectionMultiplexer
                services.AddSingleton<ICacheService, CacheService>();
            }
            catch (Exception)
            {
                // If Redis connection fails, fallback to in-memory cache
                services.AddDistributedMemoryCache();
                services.AddSingleton<ICacheService, InMemoryCacheService>();
            }
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Stock API V1");
                options.RoutePrefix = "swagger";
            });

            // ... (existing middleware)
            return app;
        }
    }

    // Marker interface for logging
    public interface IInfrastructureMarker { }
} 