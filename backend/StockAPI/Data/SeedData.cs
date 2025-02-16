using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace StockAPI.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<StockContext>();

            // Veritabanında hiç kullanıcı yoksa admin kullanıcısı oluştur
            if (!await context.Users.AnyAsync())
            {
                context.Users.Add(new Models.User
                {
                    Username = "admin",
                    PasswordHash = HashPassword("admin123"), // Varsayılan şifre: admin123
                    IsAdmin = true,
                    CreatedAt = DateTime.UtcNow
                });

                await context.SaveChangesAsync();
            }
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}
