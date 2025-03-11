using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Stock.Domain.Entities;
using Stock.Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Stock.API
{
    public static class CreateAdminUser
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // Veritabanında Admin kullanıcısı var mı kontrol et
                if (context.Users.Any(u => u.Username == "admin"))
                {
                    Console.WriteLine("Admin kullanıcısı zaten mevcut.");
                    return;
                }

                // Admin kullanıcısını oluştur
                var adminUser = new User
                {
                    Username = "admin",
                    // BCrypt.Net-Next paketini kullanarak şifreyi hashle
                    PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword("Admin123", 13),
                    IsAdmin = true
                };

                context.Users.Add(adminUser);
                context.SaveChanges();

                Console.WriteLine("Admin kullanıcısı başarıyla oluşturuldu.");
            }
        }

        public static async Task InitializeAsync(IServiceProvider serviceProvider, ILogger<Program> logger)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // Veritabanında Admin kullanıcısı var mı kontrol et
                var adminUser = await context.Users.FirstOrDefaultAsync(u => u.Username == "admin");

                // Admin kullanıcısı yoksa oluştur
                if (adminUser == null)
                {
                    logger.LogInformation("Admin kullanıcısı bulunamadı, yeni admin kullanıcısı oluşturuluyor...");
                    
                    // BCrypt.Net-Next paketini kullanarak şifreyi hashle
                    var passwordHash = BCrypt.Net.BCrypt.HashPassword("admin123", 11);
                    
                    adminUser = new User
                    {
                        Username = "Admin",
                        PasswordHash = passwordHash,
                        Sicil = "A001",
                        IsAdmin = true,
                        CreatedAt = DateTime.UtcNow,
                        LastLoginAt = DateTime.UtcNow
                    };
                    
                    context.Users.Add(adminUser);
                    await context.SaveChangesAsync();
                    logger.LogInformation("Admin kullanıcısı başarıyla oluşturuldu.");
                }
                else
                {
                    logger.LogInformation("Admin kullanıcısı zaten mevcut, şifre güncelleniyor...");
                    
                    // Admin kullanıcısının şifresini güncelle
                    var passwordHash = BCrypt.Net.BCrypt.HashPassword("admin123", 11);
                    adminUser.PasswordHash = passwordHash;
                    
                    context.Users.Update(adminUser);
                    await context.SaveChangesAsync();
                    logger.LogInformation("Admin kullanıcısının şifresi güncellendi.");
                }
            }
        }
    }
} 