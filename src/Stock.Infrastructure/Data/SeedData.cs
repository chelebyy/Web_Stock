using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.Infrastructure.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;
            
            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();
                await context.Database.MigrateAsync();
                
                await SeedPermissionsAsync(context);
                await SeedRolesAsync(context);
                await SeedUsersAsync(context, services);
                await SeedRolePermissionsAsync(context);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<ApplicationDbContext>>();
                logger.LogError(ex, "Seed data oluşturulurken bir hata oluştu.");
                throw;
            }
        }

        private static async Task SeedPermissionsAsync(ApplicationDbContext context)
        {
            if (await context.Permissions.AnyAsync())
                return;

            var permissions = new List<Permission>
            {
                // Kullanıcı Yönetimi İzinleri
                new Permission { Name = "Users.Create", Description = "Kullanıcı oluşturma", Group = "Kullanıcı Yönetimi", CreatedAt = DateTime.UtcNow },
                new Permission { Name = "Users.Read", Description = "Kullanıcıları görüntüleme", Group = "Kullanıcı Yönetimi", CreatedAt = DateTime.UtcNow },
                new Permission { Name = "Users.Update", Description = "Kullanıcı güncelleme", Group = "Kullanıcı Yönetimi", CreatedAt = DateTime.UtcNow },
                new Permission { Name = "Users.Delete", Description = "Kullanıcı silme", Group = "Kullanıcı Yönetimi", CreatedAt = DateTime.UtcNow },
                
                // Rol Yönetimi İzinleri
                new Permission { Name = "Roles.Create", Description = "Rol oluşturma", Group = "Rol Yönetimi", CreatedAt = DateTime.UtcNow },
                new Permission { Name = "Roles.Read", Description = "Rolleri görüntüleme", Group = "Rol Yönetimi", CreatedAt = DateTime.UtcNow },
                new Permission { Name = "Roles.Update", Description = "Rol güncelleme", Group = "Rol Yönetimi", CreatedAt = DateTime.UtcNow },
                new Permission { Name = "Roles.Delete", Description = "Rol silme", Group = "Rol Yönetimi", CreatedAt = DateTime.UtcNow },
                
                // Stok Yönetimi İzinleri
                new Permission { Name = "Stock.Create", Description = "Stok oluşturma", Group = "Stok Yönetimi", CreatedAt = DateTime.UtcNow },
                new Permission { Name = "Stock.Read", Description = "Stokları görüntüleme", Group = "Stok Yönetimi", CreatedAt = DateTime.UtcNow },
                new Permission { Name = "Stock.Update", Description = "Stok güncelleme", Group = "Stok Yönetimi", CreatedAt = DateTime.UtcNow },
                new Permission { Name = "Stock.Delete", Description = "Stok silme", Group = "Stok Yönetimi", CreatedAt = DateTime.UtcNow },
                
                // Rapor İzinleri
                new Permission { Name = "Reports.View", Description = "Raporları görüntüleme", Group = "Raporlar", CreatedAt = DateTime.UtcNow },
                new Permission { Name = "Reports.Export", Description = "Raporları dışa aktarma", Group = "Raporlar", CreatedAt = DateTime.UtcNow },
                
                // Dashboard İzinleri
                new Permission { Name = "Dashboard.View", Description = "Dashboard görüntüleme", Group = "Dashboard", CreatedAt = DateTime.UtcNow },
                new Permission { Name = "Dashboard.Admin", Description = "Admin dashboard görüntüleme", Group = "Dashboard", CreatedAt = DateTime.UtcNow },
                
                // Sayfa Erişim İzinleri
                new Permission { Name = "Pages.AdminDashboard", Description = "Admin paneline erişim", Group = "Sayfa Erişimi", CreatedAt = DateTime.UtcNow },
                new Permission { Name = "Pages.UserDashboard", Description = "Kullanıcı paneline erişim", Group = "Sayfa Erişimi", CreatedAt = DateTime.UtcNow },
                new Permission { Name = "Pages.UserManagement", Description = "Kullanıcı yönetimi sayfasına erişim", Group = "Sayfa Erişimi", CreatedAt = DateTime.UtcNow },
                new Permission { Name = "Pages.RoleManagement", Description = "Rol yönetimi sayfasına erişim", Group = "Sayfa Erişimi", CreatedAt = DateTime.UtcNow },
                new Permission { Name = "Pages.StockManagement", Description = "Stok yönetimi sayfasına erişim", Group = "Sayfa Erişimi", CreatedAt = DateTime.UtcNow },
                new Permission { Name = "Pages.Reports", Description = "Raporlar sayfasına erişim", Group = "Sayfa Erişimi", CreatedAt = DateTime.UtcNow },
                new Permission { Name = "Pages.Settings", Description = "Ayarlar sayfasına erişim", Group = "Sayfa Erişimi", CreatedAt = DateTime.UtcNow }
            };

            await context.Permissions.AddRangeAsync(permissions);
            await context.SaveChangesAsync();
        }

        private static async Task SeedRolesAsync(ApplicationDbContext context)
        {
            if (await context.Roles.AnyAsync())
                return;

            var roles = new List<Role>
            {
                new Role { Name = "Admin", Description = "Sistem yöneticisi", CreatedAt = DateTime.UtcNow },
                new Role { Name = "User", Description = "Normal kullanıcı", CreatedAt = DateTime.UtcNow }
            };

            await context.Roles.AddRangeAsync(roles);
            await context.SaveChangesAsync();
        }

        private static async Task SeedUsersAsync(ApplicationDbContext context, IServiceProvider services)
        {
            var passwordHasher = services.GetRequiredService<IPasswordHasher>();
            var logger = services.GetRequiredService<ILogger<ApplicationDbContext>>();

            var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
            var userRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "User");

            if (adminRole == null || userRole == null)
            {
                logger.LogWarning("Admin veya User rolü bulunamadı. Roller oluşturulduktan sonra tekrar deneyin.");
                return;
            }

            // Admin kullanıcısını kontrol et ve güncelle
            var adminUser = await context.Users.FirstOrDefaultAsync(u => u.Username == "admin");
            if (adminUser == null)
            {
                // Admin kullanıcısı yoksa oluştur
                adminUser = new User
                {
                    Username = "admin",
                    PasswordHash = passwordHasher.HashPassword("admin123"),
                    IsAdmin = true,
                    RoleId = adminRole.Id,
                    Sicil = "A001",
                    CreatedAt = DateTime.UtcNow
                };
                
                await context.Users.AddAsync(adminUser);
                logger.LogInformation("Admin kullanıcısı oluşturuldu.");
            }
            else
            {
                // Admin kullanıcısı varsa kontrol et ve güncelle
                if (string.IsNullOrEmpty(adminUser.Sicil))
                {
                    adminUser.Sicil = "A001";
                    logger.LogInformation("Admin kullanıcısının sicil alanı güncellendi: A001");
                }
                
                adminUser.PasswordHash = passwordHasher.HashPassword("admin123");
                adminUser.IsAdmin = true;
                adminUser.RoleId = adminRole.Id;
                
                context.Users.Update(adminUser);
                logger.LogInformation("Admin kullanıcısı güncellendi.");
            }
            
            await context.SaveChangesAsync();

            // Normal kullanıcıyı kontrol et ve güncelle
            var normalUser = await context.Users.FirstOrDefaultAsync(u => u.Username == "user");
            if (normalUser == null)
            {
                // Normal kullanıcı yoksa oluştur
                normalUser = new User
                {
                    Username = "user",
                    PasswordHash = passwordHasher.HashPassword("user123"),
                    IsAdmin = false,
                    RoleId = userRole.Id,
                    Sicil = "U001",
                    CreatedAt = DateTime.UtcNow
                };
                
                await context.Users.AddAsync(normalUser);
                logger.LogInformation("User kullanıcısı oluşturuldu.");
            }
            else
            {
                // Normal kullanıcı varsa kontrol et ve güncelle
                if (string.IsNullOrEmpty(normalUser.Sicil))
                {
                    normalUser.Sicil = "U001";
                    logger.LogInformation("User kullanıcısının sicil alanı güncellendi: U001");
                }
                
                normalUser.PasswordHash = passwordHasher.HashPassword("user123");
                normalUser.IsAdmin = false;
                normalUser.RoleId = userRole.Id;
                
                context.Users.Update(normalUser);
                logger.LogInformation("User kullanıcısı güncellendi.");
            }
            
            await context.SaveChangesAsync();
        }

        private static async Task SeedRolePermissionsAsync(ApplicationDbContext context)
        {
            if (await context.RolePermissions.AnyAsync())
                return;

            var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
            var userRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "User");
            
            if (adminRole == null || userRole == null)
                return;

            // Tüm izinleri al
            var allPermissions = await context.Permissions.ToListAsync();
            
            // Admin rolüne tüm izinleri ekle
            var adminRolePermissions = allPermissions.Select(p => new RolePermission
            {
                RoleId = adminRole.Id,
                PermissionId = p.Id
            }).ToList();
            
            // User rolüne sadece belirli izinleri ekle
            var userPermissions = await context.Permissions.Where(p => 
                p.Name == "Users.Read" || 
                p.Name == "Stock.Read" || 
                p.Name == "Reports.View" ||
                p.Name == "Dashboard.View"
            ).ToListAsync();
            
            var userRolePermissions = userPermissions.Select(p => new RolePermission
            {
                RoleId = userRole.Id,
                PermissionId = p.Id
            }).ToList();
            
            await context.RolePermissions.AddRangeAsync(adminRolePermissions);
            await context.RolePermissions.AddRangeAsync(userRolePermissions);
            await context.SaveChangesAsync();
        }
    }
} 