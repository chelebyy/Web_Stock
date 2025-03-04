using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Stock.Domain.Entities;
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
                await SeedUsersAsync(context);
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
                new Permission { Name = "Dashboard.Admin", Description = "Admin dashboard görüntüleme", Group = "Dashboard", CreatedAt = DateTime.UtcNow }
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

        private static async Task SeedUsersAsync(ApplicationDbContext context)
        {
            if (await context.Users.AnyAsync())
                return;

            var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
            var userRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "User");

            var users = new List<User>
            {
                new User
                {
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    IsAdmin = true,
                    RoleId = adminRole?.Id,
                    CreatedAt = DateTime.UtcNow
                },
                new User
                {
                    Username = "user",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("user123"),
                    IsAdmin = false,
                    RoleId = userRole?.Id,
                    CreatedAt = DateTime.UtcNow
                }
            };

            await context.Users.AddRangeAsync(users);
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