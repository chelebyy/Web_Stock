using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Stock.Domain.Entities;
using Stock.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Stock.API
{
    public class CreateAdminUser
    {
        private readonly ILogger _logger;

        public CreateAdminUser(ILogger<CreateAdminUser> logger)
        {
            _logger = logger;
        }

        public async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            try
            {
                _logger.LogInformation("Admin kullanıcısı oluşturma işlemi başlatılıyor...");
                
                using (var scope = serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    
                    // Veritabanında Admin kullanıcısı var mı kontrol et
                    var adminUser = await context.Users.FirstOrDefaultAsync(u => u.Username == "admin");

                    // Admin kullanıcısı yoksa oluştur
                    if (adminUser == null)
                    {
                        _logger.LogInformation("Admin kullanıcısı bulunamadı, yeni admin kullanıcısı oluşturuluyor...");
                        
                        // BCrypt.Net-Next paketini kullanarak şifreyi hashle
                        var passwordHash = BCrypt.Net.BCrypt.HashPassword("admin123", 11);
                        
                        adminUser = new User
                        {
                            Username = "admin",
                            PasswordHash = passwordHash,
                            Sicil = "A001",
                            IsAdmin = true,
                            CreatedAt = DateTime.UtcNow,
                            LastLoginAt = DateTime.UtcNow
                        };
                        
                        context.Users.Add(adminUser);
                        await context.SaveChangesAsync();
                        _logger.LogInformation("Admin kullanıcısı başarıyla oluşturuldu.");
                    }
                    else
                    {
                        _logger.LogInformation("Admin kullanıcısı zaten mevcut.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Admin kullanıcısı oluşturulurken bir hata oluştu.");
                throw;
            }
        }
    }
} 