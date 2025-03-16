using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stock.Domain.Entities;

namespace Stock.Infrastructure.Data.Config
{
    /// <summary>
    /// StockMovement entity için konfigürasyon sınıfı
    /// </summary>
    public class StockMovementConfiguration : IEntityTypeConfiguration<StockMovement>
    {
        public void Configure(EntityTypeBuilder<StockMovement> builder)
        {
            builder.HasKey(s => s.Id);
            
            builder.Property(s => s.ProductId)
                .IsRequired();
            
            builder.Property(s => s.Quantity)
                .IsRequired();
            
            builder.Property(s => s.MovementType)
                .HasMaxLength(50)
                .IsRequired();
            
            builder.Property(s => s.Description)
                .HasMaxLength(500);
            
            builder.Property(s => s.CreatedBy)
                .HasMaxLength(50)
                .IsRequired();
            
            builder.Property(s => s.CreatedAt)
                .IsRequired();
            
            // İlişkiler
            builder.HasOne(s => s.Product)
                .WithMany()
                .HasForeignKey(s => s.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Indeksler
            builder.HasIndex(s => s.ProductId);
            builder.HasIndex(s => s.CreatedAt);
            builder.HasIndex(s => s.MovementType);
        }
    }
} 