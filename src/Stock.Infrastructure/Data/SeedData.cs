using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Stock.Domain.Entities;
using Stock.Domain.Entities.Permissions;
using Stock.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;

namespace Stock.Infrastructure.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var services = scope.ServiceProvider;
                
                var logger = services.GetRequiredService<ILogger<ApplicationDbContext>>();
                logger.LogInformation("Seed data oluşturma işlemi başlatılıyor...");
                
                var context = services.GetRequiredService<ApplicationDbContext>();
                
                // Migration kontrolünü geçici olarak devre dışı bırakıyoruz
                // await context.Database.MigrateAsync();
                
                // Kullanılmayan izinleri temizle
                await CleanupUnusedPermissionsAsync(context, logger);
                
                await SeedPermissionsAsync(context);
                await SeedRolesAsync(context);
                await SeedUsersAsync(context, services);
                await SeedRolePermissionsAsync(context);
                await SeedUserPermissionsAsync(context);
                
                logger.LogInformation("Seed data oluşturma işlemi başarıyla tamamlandı.");
            }
            catch (Exception ex)
            {
                // Servis sağlayıcıdan logger alınamayabilir, bu durumda konsola yazdır
                Console.WriteLine($"Seed data oluşturulurken bir hata oluştu: {ex.Message}");
                throw;
            }
        }

        // Kullanılmayan izinleri temizleyen metot
        private static async Task CleanupUnusedPermissionsAsync(ApplicationDbContext context, ILogger logger)
        {
            try
            {
                logger.LogInformation("Kullanılmayan sayfa izinleri temizleniyor...");

                // Reports sayfası izinleri
                var reportsPermission = await context.Permissions.FirstOrDefaultAsync(p => p.Name == "Pages.Reports");
                if (reportsPermission != null)
                {
                    await RemovePermissionAsync(context, reportsPermission.Id, "Pages.Reports", logger);
                }

                // Settings sayfası izinleri
                var settingsPermission = await context.Permissions.FirstOrDefaultAsync(p => p.Name == "Pages.Settings");
                if (settingsPermission != null)
                {
                    await RemovePermissionAsync(context, settingsPermission.Id, "Pages.Settings", logger);
                }

                // StockManagement sayfası izinleri
                var stockManagementPermission = await context.Permissions.FirstOrDefaultAsync(p => p.Name == "Pages.StockManagement");
                if (stockManagementPermission != null)
                {
                    await RemovePermissionAsync(context, stockManagementPermission.Id, "Pages.StockManagement", logger);
                }

                // Stok Yönetimi grubundaki tüm izinler
                var stockPermissions = await context.Permissions.Where(p => p.Group == "Stok Yönetimi").ToListAsync();
                foreach (var permission in stockPermissions)
                {
                    await RemovePermissionAsync(context, permission.Id, permission.Name, logger);
                }

                // NOT: Revir ve BilgiIslem izinleri artık temizlenmeyecek çünkü bu sayfalar oluşturuldu
                // Revir ve BilgiIslem sayfaları için izinler korunacak

                // Eğitim sayfası izinleri
                var egitimPermissions = await context.Permissions.Where(p => p.Name.StartsWith("Pages.Egitim")).ToListAsync();
                foreach (var permission in egitimPermissions)
                {
                    await RemovePermissionAsync(context, permission.Id, permission.Name, logger);
                }

                logger.LogInformation("Kullanılmayan sayfa izinleri başarıyla temizlendi.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "İzinler temizlenirken bir hata oluştu");
                throw;
            }
        }

        private static async Task RemovePermissionAsync(ApplicationDbContext context, int permissionId, string permissionName, ILogger logger)
        {
            // Kullanıcı izinlerini temizle
            var userPermissions = await context.UserPermissions
                .Where(up => up.PermissionId == permissionId)
                .ToListAsync();
            
            if (userPermissions.Any())
            {
                context.RemoveRange(userPermissions);
                logger.LogInformation($"'{permissionName}' ile ilişkili {userPermissions.Count} kullanıcı izni kaldırıldı.");
            }

            // Rol izinlerini temizle
            var rolePermissions = await context.RolePermissions
                .Where(rp => rp.PermissionId == permissionId)
                .ToListAsync();
            
            if (rolePermissions.Any())
            {
                context.RemoveRange(rolePermissions);
                logger.LogInformation($"'{permissionName}' ile ilişkili {rolePermissions.Count} rol izni kaldırıldı.");
            }

            // İzni sil
            var permission = await context.Permissions.FindAsync(permissionId);
            if (permission != null)
            {
                context.Permissions.Remove(permission);
                logger.LogInformation($"'{permissionName}' izni kaldırıldı.");
            }

            await context.SaveChangesAsync();
        }

        private static async Task SeedPermissionsAsync(ApplicationDbContext context)
        {
            if (await context.Permissions.AnyAsync())
                return;

            var permissions = new List<Stock.Domain.Entities.Permissions.Permission>
            {
                new Stock.Domain.Entities.Permissions.Permission { 
                    Name = "Pages.Dashboard.View", 
                    Description = "Dashboard sayfasını görüntüleme", 
                    Group = "Sayfa Erişimi", 
                    ResourceType = "Page", 
                    ResourceName = "Dashboard", 
                    Action = "View", 
                    CreatedAt = DateTime.UtcNow 
                },
                // Kullanıcı Yönetimi İzinleri
                new Stock.Domain.Entities.Permissions.Permission { 
                    Name = "Users.Create", 
                    Description = "Kullanıcı oluşturma", 
                    Group = "Kullanıcı Yönetimi", 
                    ResourceType = "Function", 
                    ResourceName = "Users", 
                    Action = "Create", 
                    CreatedAt = DateTime.UtcNow 
                },
                new Stock.Domain.Entities.Permissions.Permission { 
                    Name = "Users.Read", 
                    Description = "Kullanıcıları görüntüleme", 
                    Group = "Kullanıcı Yönetimi", 
                    ResourceType = "Function", 
                    ResourceName = "Users", 
                    Action = "Read", 
                    CreatedAt = DateTime.UtcNow 
                },
                new Stock.Domain.Entities.Permissions.Permission { 
                    Name = "Users.Update", 
                    Description = "Kullanıcı güncelleme", 
                    Group = "Kullanıcı Yönetimi", 
                    ResourceType = "Function", 
                    ResourceName = "Users", 
                    Action = "Update", 
                    CreatedAt = DateTime.UtcNow 
                },
                new Stock.Domain.Entities.Permissions.Permission { 
                    Name = "Users.Delete", 
                    Description = "Kullanıcı silme", 
                    Group = "Kullanıcı Yönetimi", 
                    ResourceType = "Function", 
                    ResourceName = "Users", 
                    Action = "Delete", 
                    CreatedAt = DateTime.UtcNow 
                },
                
                // Rol Yönetimi İzinleri
                new Stock.Domain.Entities.Permissions.Permission { 
                    Name = "Roles.Create", 
                    Description = "Rol oluşturma", 
                    Group = "Rol Yönetimi", 
                    ResourceType = "Function", 
                    ResourceName = "Roles", 
                    Action = "Create", 
                    CreatedAt = DateTime.UtcNow 
                },
                new Stock.Domain.Entities.Permissions.Permission { 
                    Name = "Roles.Read", 
                    Description = "Rolleri görüntüleme", 
                    Group = "Rol Yönetimi", 
                    ResourceType = "Function", 
                    ResourceName = "Roles", 
                    Action = "Read", 
                    CreatedAt = DateTime.UtcNow 
                },
                new Stock.Domain.Entities.Permissions.Permission { 
                    Name = "Roles.Update", 
                    Description = "Rol güncelleme", 
                    Group = "Rol Yönetimi", 
                    ResourceType = "Function", 
                    ResourceName = "Roles", 
                    Action = "Update", 
                    CreatedAt = DateTime.UtcNow 
                },
                new Stock.Domain.Entities.Permissions.Permission { 
                    Name = "Roles.Delete", 
                    Description = "Rol silme", 
                    Group = "Rol Yönetimi", 
                    ResourceType = "Function", 
                    ResourceName = "Roles", 
                    Action = "Delete", 
                    CreatedAt = DateTime.UtcNow 
                },
                
                // Stok Yönetimi İzinleri
                new Stock.Domain.Entities.Permissions.Permission { 
                    Name = "Stock.Create", 
                    Description = "Stok oluşturma", 
                    Group = "Stok Yönetimi", 
                    ResourceType = "Function", 
                    ResourceName = "Stock", 
                    Action = "Create", 
                    CreatedAt = DateTime.UtcNow 
                },
                new Stock.Domain.Entities.Permissions.Permission { 
                    Name = "Stock.Read", 
                    Description = "Stokları görüntüleme", 
                    Group = "Stok Yönetimi", 
                    ResourceType = "Function", 
                    ResourceName = "Stock", 
                    Action = "Read", 
                    CreatedAt = DateTime.UtcNow 
                },
                new Stock.Domain.Entities.Permissions.Permission { 
                    Name = "Stock.Update", 
                    Description = "Stok güncelleme", 
                    Group = "Stok Yönetimi", 
                    ResourceType = "Function", 
                    ResourceName = "Stock", 
                    Action = "Update", 
                    CreatedAt = DateTime.UtcNow 
                },
                new Stock.Domain.Entities.Permissions.Permission { 
                    Name = "Stock.Delete", 
                    Description = "Stok silme", 
                    Group = "Stok Yönetimi", 
                    ResourceType = "Function", 
                    ResourceName = "Stock", 
                    Action = "Delete", 
                    CreatedAt = DateTime.UtcNow 
                },
                
                // Rapor İzinleri
                new Stock.Domain.Entities.Permissions.Permission { 
                    Name = "Reports.View", 
                    Description = "Raporları görüntüleme", 
                    Group = "Raporlar", 
                    ResourceType = "Function", 
                    ResourceName = "Reports", 
                    Action = "View", 
                    CreatedAt = DateTime.UtcNow 
                },
                new Stock.Domain.Entities.Permissions.Permission { 
                    Name = "Reports.Export", 
                    Description = "Raporları dışa aktarma", 
                    Group = "Raporlar", 
                    ResourceType = "Function", 
                    ResourceName = "Reports", 
                    Action = "Export", 
                    CreatedAt = DateTime.UtcNow 
                },
                
                // Dashboard İzinleri
                new Stock.Domain.Entities.Permissions.Permission { 
                    Name = "Dashboard.Admin", 
                    Description = "Admin dashboard görüntüleme", 
                    Group = "Dashboard", 
                    ResourceType = "Function", 
                    ResourceName = "Dashboard", 
                    Action = "AdminView", 
                    CreatedAt = DateTime.UtcNow 
                },
                
                // Sayfa Erişim İzinleri - Ana Sayfalar
                new Stock.Domain.Entities.Permissions.Permission { 
                    Name = "Pages.AdminDashboard", 
                    Description = "Admin paneline erişim", 
                    Group = "Sayfa Erişimi", 
                    ResourceType = "Page", 
                    ResourceName = "AdminDashboard", 
                    Action = "View", 
                    CreatedAt = DateTime.UtcNow 
                },
                new Stock.Domain.Entities.Permissions.Permission { 
                    Name = "Pages.UserDashboard", 
                    Description = "Kullanıcı paneline erişim", 
                    Group = "Sayfa Erişimi", 
                    ResourceType = "Page", 
                    ResourceName = "UserDashboard", 
                    Action = "View", 
                    CreatedAt = DateTime.UtcNow 
                },
                new Stock.Domain.Entities.Permissions.Permission { 
                    Name = "Pages.UserManagement", 
                    Description = "Kullanıcı yönetimi sayfasına erişim", 
                    Group = "Sayfa Erişimi", 
                    ResourceType = "Page", 
                    ResourceName = "UserManagement", 
                    Action = "View", 
                    CreatedAt = DateTime.UtcNow 
                },
                new Stock.Domain.Entities.Permissions.Permission { 
                    Name = "Pages.RoleManagement", 
                    Description = "Rol yönetimi sayfasına erişim", 
                    Group = "Sayfa Erişimi", 
                    ResourceType = "Page", 
                    ResourceName = "RoleManagement", 
                    Action = "View", 
                    CreatedAt = DateTime.UtcNow 
                },
                new Stock.Domain.Entities.Permissions.Permission { 
                    Name = "Pages.StockManagement", 
                    Description = "Stok yönetimi sayfasına erişim", 
                    Group = "Sayfa Erişimi", 
                    ResourceType = "Page", 
                    ResourceName = "StockManagement", 
                    Action = "View", 
                    CreatedAt = DateTime.UtcNow 
                },
                new Stock.Domain.Entities.Permissions.Permission { 
                    Name = "Pages.Reports", 
                    Description = "Raporlar sayfasına erişim", 
                    Group = "Sayfa Erişimi", 
                    ResourceType = "Page", 
                    ResourceName = "Reports", 
                    Action = "View", 
                    CreatedAt = DateTime.UtcNow 
                },
                new Stock.Domain.Entities.Permissions.Permission { 
                    Name = "Pages.Settings", 
                    Description = "Ayarlar sayfasına erişim", 
                    Group = "Sayfa Erişimi", 
                    ResourceType = "Page", 
                    ResourceName = "Settings", 
                    Action = "View", 
                    CreatedAt = DateTime.UtcNow 
                },
                
                // Revir Sayfası İzinleri
                new Stock.Domain.Entities.Permissions.Permission { 
                    Name = "Pages.Revir.View", 
                    Description = "Revir sayfasını görüntüleme", 
                    Group = "Sayfa Erişimi", 
                    ResourceType = "Page", 
                    ResourceName = "Revir", 
                    Action = "View", 
                    CreatedAt = DateTime.UtcNow 
                },
                new Stock.Domain.Entities.Permissions.Permission { 
                    Name = "Pages.Revir.Create", 
                    Description = "Revir sayfasında ekleme", 
                    Group = "Sayfa Erişimi", 
                    ResourceType = "Page", 
                    ResourceName = "Revir", 
                    Action = "Create", 
                    CreatedAt = DateTime.UtcNow 
                },
                new Stock.Domain.Entities.Permissions.Permission { 
                    Name = "Pages.Revir.Edit", 
                    Description = "Revir sayfasında düzenleme", 
                    Group = "Sayfa Erişimi", 
                    ResourceType = "Page", 
                    ResourceName = "Revir", 
                    Action = "Edit", 
                    CreatedAt = DateTime.UtcNow 
                },
                new Stock.Domain.Entities.Permissions.Permission { 
                    Name = "Pages.Revir.Delete", 
                    Description = "Revir sayfasında silme", 
                    Group = "Sayfa Erişimi", 
                    ResourceType = "Page", 
                    ResourceName = "Revir", 
                    Action = "Delete", 
                    CreatedAt = DateTime.UtcNow 
                },
                
                // Bilgi İşlem Sayfası İzinleri
                new Stock.Domain.Entities.Permissions.Permission { 
                    Name = "Pages.BilgiIslem.View", 
                    Description = "Bilgi İşlem sayfasını görüntüleme", 
                    Group = "Sayfa Erişimi", 
                    ResourceType = "Page", 
                    ResourceName = "BilgiIslem", 
                    Action = "View", 
                    CreatedAt = DateTime.UtcNow 
                },
                new Stock.Domain.Entities.Permissions.Permission { 
                    Name = "Pages.BilgiIslem.Create", 
                    Description = "Bilgi İşlem sayfasında ekleme", 
                    Group = "Sayfa Erişimi", 
                    ResourceType = "Page", 
                    ResourceName = "BilgiIslem", 
                    Action = "Create", 
                    CreatedAt = DateTime.UtcNow 
                },
                new Stock.Domain.Entities.Permissions.Permission { 
                    Name = "Pages.BilgiIslem.Edit", 
                    Description = "Bilgi İşlem sayfasında düzenleme", 
                    Group = "Sayfa Erişimi", 
                    ResourceType = "Page", 
                    ResourceName = "BilgiIslem", 
                    Action = "Edit", 
                    CreatedAt = DateTime.UtcNow 
                },
                new Stock.Domain.Entities.Permissions.Permission { 
                    Name = "Pages.BilgiIslem.Delete", 
                    Description = "Bilgi İşlem sayfasında silme", 
                    Group = "Sayfa Erişimi", 
                    ResourceType = "Page", 
                    ResourceName = "BilgiIslem", 
                    Action = "Delete", 
                    CreatedAt = DateTime.UtcNow 
                },
                
                // Eğitim Sayfası İzinleri
                new Stock.Domain.Entities.Permissions.Permission { 
                    Name = "Pages.Egitim.View", 
                    Description = "Eğitim sayfasını görüntüleme", 
                    Group = "Sayfa Erişimi", 
                    ResourceType = "Page", 
                    ResourceName = "Egitim", 
                    Action = "View", 
                    CreatedAt = DateTime.UtcNow 
                },
                new Stock.Domain.Entities.Permissions.Permission { 
                    Name = "Pages.Egitim.Create", 
                    Description = "Eğitim sayfasında ekleme", 
                    Group = "Sayfa Erişimi", 
                    ResourceType = "Page", 
                    ResourceName = "Egitim", 
                    Action = "Create", 
                    CreatedAt = DateTime.UtcNow 
                },
                new Stock.Domain.Entities.Permissions.Permission { 
                    Name = "Pages.Egitim.Edit", 
                    Description = "Eğitim sayfasında düzenleme", 
                    Group = "Sayfa Erişimi", 
                    ResourceType = "Page", 
                    ResourceName = "Egitim", 
                    Action = "Edit", 
                    CreatedAt = DateTime.UtcNow 
                },
                new Stock.Domain.Entities.Permissions.Permission { 
                    Name = "Pages.Egitim.Delete", 
                    Description = "Eğitim sayfasında silme", 
                    Group = "Sayfa Erişimi", 
                    ResourceType = "Page", 
                    ResourceName = "Egitim", 
                    Action = "Delete", 
                    CreatedAt = DateTime.UtcNow 
                }
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
            // Eğer zaten RolePermission verileri varsa, tekrar oluşturma
            if (await context.RolePermissions.AnyAsync())
                return;
            
            // Roller
            var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
            var userRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "User");
            
            if (adminRole == null || userRole == null)
            {
                // Roller henüz yok, seed roles çalıştırılmalı
                return;
            }
            
            // İzinler
            var allPermissions = await context.Permissions.ToListAsync();
            var userPermissions = await context.Permissions.Where(p => 
                p.Name.StartsWith("Pages.UserDashboard") || 
                p.Name.StartsWith("Pages.Revir.View") ||
                p.Name.StartsWith("Stock.Read") ||
                p.Name.StartsWith("Reports.View")
            ).ToListAsync();
            
            // Admin rolü tüm izinlere sahip olacak
            var adminRolePermissions = allPermissions.Select(p => new Stock.Domain.Entities.Permissions.RolePermission
            {
                RoleId = adminRole.Id,
                PermissionId = p.Id
            }).ToList();
            
            var userRolePermissions = userPermissions.Select(p => new Stock.Domain.Entities.Permissions.RolePermission
            {
                RoleId = userRole.Id,
                PermissionId = p.Id
            }).ToList();
            
            await context.RolePermissions.AddRangeAsync(adminRolePermissions);
            await context.RolePermissions.AddRangeAsync(userRolePermissions);
            await context.SaveChangesAsync();
        }

        private static async Task SeedUserPermissionsAsync(ApplicationDbContext context)
        {
            // Eğer zaten UserPermission verileri varsa, tekrar oluşturma
            if (await context.UserPermissions.AnyAsync())
                return;
            
            // Örnek kullanıcılar
            var user1 = await context.Users.FirstOrDefaultAsync(u => u.Username == "ayse");
            var user2 = await context.Users.FirstOrDefaultAsync(u => u.Username == "ahmet");
            var user3 = await context.Users.FirstOrDefaultAsync(u => u.Username == "mehmet");
            
            // İzinler
            var egitimEditPermission = await context.Permissions.FirstOrDefaultAsync(p => p.Name == "Pages.Egitim.Edit");
            var revirViewPermission = await context.Permissions.FirstOrDefaultAsync(p => p.Name == "Pages.Revir.View");
            var revirEditPermission = await context.Permissions.FirstOrDefaultAsync(p => p.Name == "Pages.Revir.Edit");
            var bilgiIslemViewPermission = await context.Permissions.FirstOrDefaultAsync(p => p.Name == "Pages.BilgiIslem.View");
            
            if (egitimEditPermission == null || revirViewPermission == null || 
                revirEditPermission == null || bilgiIslemViewPermission == null)
            {
                // İzinler henüz yok, seed permissions çalıştırılmalı
                return;
            }
            
            // Kullanıcı özel izinleri oluştur
            var userPermissions = new List<Stock.Domain.Entities.Permissions.UserPermission>();
            
            // Ayşe: Diğer adminlerden farklı olarak sadece Eğitim sayfasını düzenleyebilir
            if (user1 != null)
            {
                userPermissions.Add(new Stock.Domain.Entities.Permissions.UserPermission
                {
                    UserId = user1.Id,
                    PermissionId = egitimEditPermission.Id,
                    IsGranted = true,
                    CreatedAt = DateTime.UtcNow
                });
            }
            
            // Ahmet: Standart kullanıcı rolünden farklı olarak Revir sayfasını görüntüleyebilir ve düzenleyebilir
            if (user2 != null)
            {
                userPermissions.Add(new Stock.Domain.Entities.Permissions.UserPermission
                {
                    UserId = user2.Id,
                    PermissionId = revirViewPermission.Id,
                    IsGranted = true,
                    CreatedAt = DateTime.UtcNow
                });
                
                userPermissions.Add(new Stock.Domain.Entities.Permissions.UserPermission
                {
                    UserId = user2.Id,
                    PermissionId = revirEditPermission.Id,
                    IsGranted = true,
                    CreatedAt = DateTime.UtcNow
                });
            }
            
            // Mehmet: Standart kullanıcı rolünden farklı olarak Bilgi İşlem sayfasını görüntüleyebilir, ama düzenleyemez
            if (user3 != null)
            {
                userPermissions.Add(new Stock.Domain.Entities.Permissions.UserPermission
                {
                    UserId = user3.Id,
                    PermissionId = bilgiIslemViewPermission.Id,
                    IsGranted = true,
                    CreatedAt = DateTime.UtcNow
                });
            }
            
            await context.UserPermissions.AddRangeAsync(userPermissions);
            await context.SaveChangesAsync();
        }
    }
} 