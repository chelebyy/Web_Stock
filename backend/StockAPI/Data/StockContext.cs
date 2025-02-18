using Microsoft.EntityFrameworkCore;
using StockAPI.Models;

namespace StockAPI.Data
{
    public class StockContext : DbContext
    {
        public StockContext(DbContextOptions<StockContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<ITEquipment> ITEquipments { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Name)
                .IsUnique();

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Name)
                .IsUnique();

            modelBuilder.Entity<AuditLog>()
                .HasIndex(a => a.Timestamp);

            modelBuilder.Entity<ITEquipment>()
                .HasIndex(e => e.SerialNumber)
                .IsUnique();

            modelBuilder.Entity<Role>()
                .HasIndex(r => r.Name)
                .IsUnique();

            // Varsayılan rolleri ekle
            var seedTime = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            
            modelBuilder.Entity<Role>().HasData(
                new Role { 
                    Id = 1, 
                    Name = "administrator", 
                    Description = "Sistem Yöneticisi", 
                    CreatedAt = seedTime 
                },
                new Role { 
                    Id = 2, 
                    Name = "it_support", 
                    Description = "Bilgi İşlem", 
                    CreatedAt = seedTime 
                },
                new Role { 
                    Id = 3, 
                    Name = "standard_user", 
                    Description = "Standart Kullanıcı", 
                    CreatedAt = seedTime 
                }
            );

            // Admin kullanıcısını ekle
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    PasswordHash = "JAvlGPq9JyTdtvBO6x2llnRI1+gxwIyPqCKAn3THIKk=", // admin123
                    IsAdmin = true,
                    RoleId = 1,
                    CreatedAt = seedTime
                }
            );
        }
    }
}
