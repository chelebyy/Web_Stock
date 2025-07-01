using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Stock.Infrastructure.Data;
using Stock.Application;
using Stock.Infrastructure;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json.Serialization;
using Stock.API; // CreateAdminUser sınıfını import ediyoruz
using Stock.API.Middleware; // Middleware namespace'ini ekle
using NLog;
using NLog.Web;
using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics; // Eklendi
using FluentValidation.AspNetCore;
using FluentValidation; // Eklendi
using Stock.Application.Common.Behaviors; // 'u' olmadan düzeltildi
using Microsoft.AspNetCore.RateLimiting; // Eklendi
using System.Threading.RateLimiting; // Eklendi
using Stock.Application.Models.DTOs;
using Stock.Domain.Entities;
using Stock.Domain.ValueObjects;
using System.Reflection;
using Microsoft.OpenApi.Models;

// Program sınıfını public yaparak erişilebilirlik sorununu çözüyoruz
public class Program
{
    public static async Task Main(string[] args)
    {
        // NLog yapılandırmasını yükle
        var logger = NLog.LogManager.Setup()
                                  .LoadConfigurationFromFile("nlog.config")
                                  .GetCurrentClassLogger();
        try
        {
            logger.Info("Uygulama başlatılıyor...");
            
            var builder = WebApplication.CreateBuilder(args);

            // NLog'u DI container'a ekle
            builder.Logging.ClearProviders();
            builder.Host.UseNLog();

            // Add services to the container.
            // Removed direct DbContext registration from Program.cs
            // Configuration is now handled within AddInfrastructure
            // builder.Services.AddDbContext<ApplicationDbContext>(options =>
            //     options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
            //            .EnableSensitiveDataLogging() 
            //            .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)); 

            // Logging ayarlarını yapılandır
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();
            builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information); // Düzeltildi

            // Global Exception Handling Middleware'i servis olarak ekle
            builder.Services.AddScoped<GlobalExceptionHandlingMiddleware>();

            // Add Application and Infrastructure services
            builder.Services.AddApplication();
            builder.Services.AddInfrastructure(builder.Configuration);

            // Register MediatR services and scan the Application assembly
            builder.Services.AddMediatR(cfg => 
                cfg.RegisterServicesFromAssembly(typeof(Stock.Application.DependencyInjection).Assembly)); // AssemblyReference yerine DependencyInjection kullanıldı

            // Register FluentValidation validators from Application assembly
            builder.Services.AddValidatorsFromAssembly(typeof(Stock.Application.DependencyInjection).Assembly); // Yorum kaldırıldı ve AssemblyReference düzeltildi

            // FluentValidation ASP.NET Core entegrasyonunu ekle
            builder.Services.AddFluentValidationAutoValidation(); // Yorum kaldırıldı
            builder.Services.AddFluentValidationClientsideAdapters(); // Yorum kaldırıldı

            // MediatR doğrulama pipeline behavior'ı
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>)); // Yorum kaldırıldı

            var jwtKey = builder.Configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new InvalidOperationException("JWT Key not found in configuration.");
            }

            // JWT yapılandırması
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };
            });

            // JSON serialization ayarları
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    // Döngüsel referansları koru
                    // options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve; // Bu satır soruna neden oluyor
                    // Null değerleri dahil etme
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                    // Enum değerlerini string olarak serialize et
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            // Swagger configuration is handled in Infrastructure layer
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost4200", policy =>
                {
                    policy.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown_ip",
                        factory: partition => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 100,
                            Window = TimeSpan.FromMinutes(1)
                        }));
            });

            // API versioning removed for simplicity

            var app = builder.Build();

            // Veritabanı başlangıç verilerini (seed) ekle
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var appLogger = services.GetRequiredService<ILogger<Program>>(); // Logger'ı scope'tan al
                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>(); // DbContext'i al
                    appLogger.LogInformation("Veritabanı durumu kontrol ediliyor (Otomatik migration devre dışı).");
                    // context.Database.Migrate(); // Geçici olarak yorum satırı yapıldı
                    appLogger.LogInformation("Otomatik migration atlandı. Migration'ları manuel olarak 'dotnet ef database update' ile uygulayın.");

                    // Mevcut Seed Data logları devam edebilir
                    appLogger.LogInformation("Başlangıç verileri ekleniyor...");
                    // Value Object'ler ile seeding yapıyoruz
                    await SeedInitialDataAsync(context, appLogger);
                    appLogger.LogInformation("Başlangıç verileri başarıyla eklendi.");
                }
                catch (Exception ex)
                {
                    appLogger.LogError(ex, "Başlangıç verileri işlenirken bir hata oluştu (Program.cs)."); // Log mesajını güncelle
                    // Uygulamanın başlatılmasına devam etmeyebiliriz, veya hatayı loglayıp devam edebiliriz.
                    // Şimdilik loglayıp devam ediyoruz.
                }
            }

            // Global Exception Handling Middleware'i pipeline'a ekle
            app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

            // Security Headers Middleware'i pipeline'a ekle
            app.UseMiddleware<SecurityHeadersMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                // Swagger configuration is handled in Infrastructure layer
                app.UseInfrastructure();
                // app.UseDeveloperExceptionPage(); // Global middleware varken buna gerek kalmayabilir
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts(); // Production'da HSTS
            }

            // CORS middleware'i genellikle routing'den önce gelir
            app.UseCors("AllowLocalhost4200");

            // app.UseRouting(); // Eğer UseRouting kullanılıyorsa, Auth middleware'leri genellikle bundan sonra gelir

            //app.UseHttpsRedirection();

            // Rate Limiting middleware'ini ekle
            app.UseRateLimiter();

            // Kimlik doğrulama ve yetkilendirme middleware'lerini ekle
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Uygulama başlatılırken kritik bir hata oluştu");
            throw; // Uygulamanın çökmesine izin ver
        }
        finally
        {
            LogManager.Shutdown();
        }
    }

    private static async Task SeedInitialDataAsync(ApplicationDbContext context, ILogger<Program> logger)
    {
        try
        {
            // Admin rolünü kontrol et ve oluştur
            if (!await context.Roles.AnyAsync())
            {
                logger.LogInformation("Admin rolü oluşturuluyor...");
                
                var roleNameResult = RoleName.Create("Admin");
                if (roleNameResult.IsFailure)
                {
                    logger.LogError($"Admin rol adı oluşturulamadı: {roleNameResult.Error}");
                    return;
                }

                var adminRoleResult = Role.Create(roleNameResult.Value, "Administrator role with full access");
                if (adminRoleResult.IsFailure)
                {
                    logger.LogError($"Admin rolü oluşturulamadı: {adminRoleResult.Error}");
                    return;
                }
                
                context.Roles.Add(adminRoleResult.Value);
                await context.SaveChangesAsync();
                
                logger.LogInformation("Admin rolü başarıyla oluşturuldu.");
            }

            // Admin kullanıcısını kontrol et ve oluştur
            if (!await context.Users.AnyAsync())
            {
                logger.LogInformation("Admin kullanıcısı oluşturuluyor...");
                
                // Admin rolünü getir
                var adminRole = await context.Roles.FirstOrDefaultAsync();
                if (adminRole == null)
                {
                    logger.LogError("Admin rolü bulunamadı.");
                    return;
                }

                // Value Object'leri oluştur
                var sicilResult = Sicil.Create("00000");
                var fullNameResult = FullName.Create("Admin", "User");
                
                if (sicilResult.IsFailure || fullNameResult.IsFailure)
                {
                    logger.LogError("Admin kullanıcısı için Value Object'ler oluşturulamadı.");
                    return;
                }

                // Admin kullanıcısını oluştur
                var adminUserResult = User.Create(
                    fullNameResult.Value,  // FullName (1. parametre)
                    sicilResult.Value,     // Sicil (2. parametre)
                    "$2a$11$3J5..wF0T4iP.gR2Y5tA5.L/E.CHd2/x3O5aZl4Y3k58S.s/3x8ge", // PasswordHash (3. parametre)
                    adminRole.Id,          // RoleId (4. parametre)
                    true                   // IsAdmin (5. parametre)
                );
                
                if (adminUserResult.IsFailure)
                {
                    logger.LogError($"Admin kullanıcısı oluşturulamadı: {adminUserResult.Error}");
                    return;
                }
                
                context.Users.Add(adminUserResult.Value);
                await context.SaveChangesAsync();
                
                logger.LogInformation("Admin kullanıcısı başarıyla oluşturuldu.");
            }
            else
            {
                logger.LogInformation("Seeding atlandı: Veriler zaten mevcut.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Seeding işlemi sırasında hata oluştu.");
        }
    }
}
