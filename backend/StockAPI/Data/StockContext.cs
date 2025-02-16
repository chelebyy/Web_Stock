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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

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
        }
    }
}
