using Microsoft.EntityFrameworkCore;
using StockAPI.Models;
using System.Security.Cryptography;
using System.Text;

namespace StockAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Category yapılandırması
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasOne(c => c.ParentCategory)
                    .WithMany(c => c.SubCategories)
                    .HasForeignKey(c => c.ParentCategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Product yapılandırması
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasOne(p => p.Category)
                    .WithMany(c => c.Products)
                    .HasForeignKey(p => p.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(p => p.Price)
                    .HasPrecision(18, 2);
            });

            // Admin kullanıcısı oluştur
            var password = "Admin123!";
            var passwordHash = ComputeSha256Hash(password);

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                Username = "admin",
                PasswordHash = passwordHash,
                IsAdmin = true,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            });
        }

        private string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (
                    e.State == EntityState.Added
                    || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseEntity)entityEntry.Entity).UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
