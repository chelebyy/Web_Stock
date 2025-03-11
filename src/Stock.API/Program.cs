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

// Program sınıfını public yaparak erişilebilirlik sorununu çözüyoruz
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
                   .EnableSensitiveDataLogging() // Hassas verileri de logla
                   .LogTo(Console.WriteLine, LogLevel.Information)); // SQL sorgularını konsola yazdır

        // Logging ayarlarını yapılandır
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
        builder.Logging.AddDebug();
        builder.Logging.SetMinimumLevel(LogLevel.Information);

        // Add Application and Infrastructure services
        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration);

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
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                // Null değerleri dahil etme
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                // Enum değerlerini string olarak serialize et
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddCors();
        builder.Services.AddHttpContextAccessor();

        var app = builder.Build();

        // Admin kullanıcısını oluştur
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                CreateAdminUser.InitializeAsync(services, logger).Wait();
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "Admin kullanıcısı oluşturulurken bir hata oluştu.");
            }
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseDeveloperExceptionPage(); // Geliştirme ortamında hata sayfasını göster
        }

        // CORS politikasını ekle
        app.UseCors(builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

        app.UseHttpsRedirection();

        // Kimlik doğrulama ve yetkilendirme middleware'lerini ekle
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        // Seed data
        SeedData.InitializeAsync(app.Services).Wait();

        app.Run();
    }
}
