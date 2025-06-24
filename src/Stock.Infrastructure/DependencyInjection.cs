using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stock.Application.Common.Interfaces;
using Stock.Domain.Interfaces;
using Stock.Infrastructure.Data;
using Stock.Infrastructure.Data.Repositories;
using Stock.Infrastructure.Logging;
using Stock.Infrastructure.Services;

namespace Stock.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
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

            // Services
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAuditService, AuditService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IUserPermissionService, UserPermissionService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            // LoggerManager
            services.AddSingleton(typeof(ILoggerManager<>), typeof(LoggerManager<>));

            return services;
        }
    }
} 