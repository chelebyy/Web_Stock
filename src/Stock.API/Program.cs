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

// Program sınıfını public yaparak erişilebilirlik sorununu çözüyoruz
public class Program
{
    public static async Task Main(string[] args) // Main metodunu async yap
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
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

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
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
                    // await SeedRoles.SeedAsync(services); // Bu satırı kaldır
                    // await SeedUsers.SeedDefaultUsersAsync(services); // Bu satırı kaldır
                    // Seeding işlemi artık ApplicationDbContext içinde yapılıyor.
                    // Bu bloğu tamamen kaldırmak veya sadece bir log mesajı bırakmak daha iyi olabilir.
                    appLogger.LogInformation("Başlangıç verileri ApplicationDbContext içinde işleniyor."); 
                    // appLogger.LogInformation("Başlangıç verileri başarıyla eklendi."); // Bu satırı kaldır veya güncelle
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
                app.UseSwagger();
                app.UseSwaggerUI();
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
}
