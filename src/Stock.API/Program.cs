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
using NLog;
using NLog.Web;
using System.IO;
using Stock.API.Middleware;
using Stock.Infrastructure.Extensions;
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;
using Stock.API.Extensions;

// NLog yapılandırmasını yükle
var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
logger.Debug("Uygulama başlatılıyor...");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // NLog yapılandırması
    LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

    // Log klasörlerini oluştur
    EnsureLogDirectoryExists();

    // NLog'u yapılandır
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // Add services to the container.
    builder.Services.AddControllers();

    // Infrastructure services
    builder.Services.AddInfrastructureServices(builder.Configuration);

    // Add Application and Infrastructure services
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);

    // JWT yapılandırması
    builder.Services.ConfigureJWT(builder.Configuration);

    // JSON serialization ayarları
    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            // Döngüsel referansları koru
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
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
        options.AddPolicy("AllowAll", builder =>
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader());
    });
    builder.Services.AddHttpContextAccessor();

    var app = builder.Build();

    // Admin kullanıcısını oluştur
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            var adminLogger = loggerFactory.CreateLogger<CreateAdminUser>();
            var adminCreator = new CreateAdminUser(adminLogger);
            adminCreator.InitializeAsync(services).Wait();
        }
        catch (Exception ex)
        {
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            var adminLogger = loggerFactory.CreateLogger<CreateAdminUser>();
            adminLogger.LogError(ex, "Admin kullanıcısı oluşturulurken bir hata oluştu.");
        }
    }

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    else
    {
        // Üretim ortamında global exception handler kullan
        app.UseGlobalExceptionHandler();
    }

    // CORS politikasını ekle
    app.UseCors("AllowAll");

    app.UseHttpsRedirection();

    // Kimlik doğrulama ve yetkilendirme middleware'lerini ekle
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    // Seed data - geçici olarak devre dışı bırakıldı
    // SeedData.InitializeAsync(app.Services).Wait();

    app.Run();
}
catch (Exception ex)
{
    // NLog ile kritik hataları yakala
    logger.Error(ex, "Program beklenmedik şekilde sonlandı");
    throw;
}
finally
{
    // NLog'u düzgün şekilde kapat
    LogManager.Shutdown();
}

void EnsureLogDirectoryExists()
{
    var logDirectories = new[] { "logs", "logs/archive" };
    foreach (var dir in logDirectories)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), dir);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
}
