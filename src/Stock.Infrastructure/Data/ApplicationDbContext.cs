using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Stock.Domain.Entities;
using Stock.Domain.Entities.Permissions;
using Stock.Domain.ValueObjects;
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

            // Seed data - Value Object'ler nedeniyle Program.cs'de yapılacak
            // SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Use a fixed date for seed data
            var seedDate = new DateTime(2025, 3, 1, 0, 0, 0, DateTimeKind.Utc);

            // Seed admin role
            modelBuilder.Entity<Role>().HasData(
                new 
                {
                    Id = 1,
                    Name_Value = "Admin", // Value Object için doğru property adı
                    Description = "Administrator role with full access",
                    CreatedAt = seedDate,
                    IsDeleted = false,
                    UpdatedAt = (DateTime?)null,
                    CreatedBy = (string)null,
                    UpdatedBy = (string)null
                }
            );

            // Seed admin user
            var passwordHash = "$2a$11$3J5..wF0T4iP.gR2Y5tA5.L/E.CHd2/x3O5aZl4Y3k58S.s/3x8ge"; // Admin123!
            modelBuilder.Entity<User>().HasData(
                new 
                {
                    Id = 1,
                    FullName_Adi = "Admin", // Value Object için doğru property adı
                    FullName_Soyadi = "Kullanıcısı", // Value Object için doğru property adı
                    Sicil_Value = "00000", // Value Object için doğru property adı
                    PasswordHash = passwordHash,
                    IsAdmin = true,
                    RoleId = 1,
                    CreatedAt = seedDate,
                    IsDeleted = false,
                    IsActive = true,
                    Username = "admin",
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
                
                modelBuilder.Entity<Permission>().HasData(
                    new 
                    {
                        Id = permissionIdCounter,
                        Name_Value = permissionName, // Value Object için doğru property adı
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