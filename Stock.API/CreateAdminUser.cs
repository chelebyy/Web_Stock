using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Stock.Domain.Entities;
using Stock.Infrastructure.Data;

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
                if (context.Users.Any(u => u.Username == "Admin"))
                {
                    Console.WriteLine("Admin kullanıcısı zaten mevcut.");
                    return;
                }

                // Admin kullanıcısını oluştur
                var adminUser = new User
                {
                    Username = "Admin",
                    // BCrypt.Net-Next paketini kullanarak şifreyi hashle
                    PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword("Admin123", 13),
                    IsAdmin = true
                };

                context.Users.Add(adminUser);
                context.SaveChanges();

                Console.WriteLine("Admin kullanıcısı başarıyla oluşturuldu.");
            }
        }
    }
} 