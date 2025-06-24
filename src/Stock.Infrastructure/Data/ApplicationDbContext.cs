using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Stock.Domain.Entities;
using Stock.Domain.Entities.Permissions;
using Stock.Infrastructure.Data.Configurations;
using BCrypt.Net;
using Stock.Application.Constants;
using System.Linq;
using System.Collections.Generic;

namespace Stock.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<AuditLog> AuditLogs { get; set; } = null!;
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<Permission> Permissions { get; set; } = null!;
        public DbSet<RolePermission> RolePermissions { get; set; } = null!;
        public DbSet<UserPermission> UserPermissions { get; set; } = null!;

        // Yeni DbSet'ler
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        // Currency kullanılmayacak
        // public DbSet<Currency> Currencies { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply entity configurations
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Use a fixed date for seed data
            var seedDate = new DateTime(2025, 3, 1, 0, 0, 0, DateTimeKind.Utc);

            // Seed admin role
            // DDD için factory metot kullanarak Role oluştur
            var adminRoleResult = Role.Create("Admin", "Administrator role with full access");
            if (adminRoleResult.IsSuccess)
            {
                var adminRole = adminRoleResult.Value;
                // Seed data için gerekli özellikler anonim nesne ile eklenir
                modelBuilder.Entity<Role>().HasData(
                    new 
                    {
                        Id = 1,
                        Name = adminRole.Name,
                        Description = adminRole.Description,
                        CreatedAt = seedDate,
                        IsDeleted = false,
                        UpdatedAt = (DateTime?)null,
                        CreatedBy = (string)null,
                        UpdatedBy = (string)null
                    }
                );
            }

            // Seed admin user - Using User factory method would be ideal but EF Core seeding requires 
            // explicit property assignments, so we'll use the Entity direct initialization here
            // and then ensure our model is correctly configured in UserConfiguration
            var passwordHash = "$2a$11$w1QwQn1QwQn1QwQn1QwQnOQn1QwQn1QwQn1QwQn1QwQn1QwQn1Q";
            modelBuilder.Entity<User>().HasData(
                new 
                {
                    Id = 1,
                    Adi = "Admin",
                    Soyadi = "Kullanıcısı",
                    Sicil = "00000",
                    PasswordHash = passwordHash,
                    IsAdmin = true,
                    RoleId = 1,
                    CreatedAt = seedDate,
                    IsDeleted = false,
                    LastLoginAt = (DateTime?)null,
                    UpdatedAt = (DateTime?)null,
                    CreatedBy = (string)null,
                    UpdatedBy = (string)null
                }
            );

            // Seed permissions
            var allPermissionFields = typeof(PermissionNames).GetNestedTypes()
                .SelectMany(type => type.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static))
                .Where(f => f.IsLiteral && !f.IsInitOnly && f.FieldType == typeof(string))
                .ToList();

            int permissionIdCounter = 1;
            var permissionNameToIdMap = new Dictionary<string, int>();
            
            // Seed data
            foreach (var field in allPermissionFields)
            {
                string permissionName = (string)field.GetValue(null);
                string groupName = field.DeclaringType.Name;
                
                // Factory metodu yerine doğrudan HasData için anonim nesne kullanıyoruz
                modelBuilder.Entity<Permission>().HasData(
                    new 
                    {
                        Id = permissionIdCounter,
                        Name = permissionName,
                        Group = groupName, 
                        Description = $"{groupName} - {permissionName}",
                        ResourceType = "",
                        ResourceName = "",
                        Action = "",
                        CreatedAt = seedDate,
                        IsDeleted = false,
                        UpdatedAt = (DateTime?)null,
                        CreatedBy = (string)null,
                        UpdatedBy = (string)null
                    }
                );
                
                permissionNameToIdMap[permissionName] = permissionIdCounter;
                permissionIdCounter++;
            }

            // Seed role permissions for admin (RoleId = 1)
            var rolePermissionsToSeed = new List<object>();
            foreach (var entry in permissionNameToIdMap)
            {
                rolePermissionsToSeed.Add(new 
                {
                    Id = entry.Value, // RolePermission Id = Permission Id
                    RoleId = 1,
                    PermissionId = entry.Value,
                    CreatedAt = seedDate,
                    IsDeleted = false,
                    UpdatedAt = (DateTime?)null,
                    CreatedBy = (string)null,
                    UpdatedBy = (string)null
                });
            }
            
            modelBuilder.Entity<RolePermission>().HasData(rolePermissionsToSeed);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
} 