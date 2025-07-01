using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stock.Domain.Entities;
using Stock.Domain.ValueObjects; // Value Objects için eklendi

namespace Stock.Infrastructure.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);

            // Complex Types yerine OwnsOne kullanarak Value Object eşlemeleri
            builder.OwnsOne(p => p.Name, nameBuilder =>
            {
                nameBuilder.Property(pn => pn.Value)
                    .HasColumnName("Name") // Sütun adını belirtiyoruz
                    .IsRequired()
                    .HasMaxLength(100);
                
                // Index'i OwnsOne yapılandırması içinde tanımlama
                nameBuilder.HasIndex(pn => pn.Value)
                    .HasDatabaseName("IX_Products_Name");
            });

            builder.OwnsOne(p => p.Description, descBuilder =>
            {
                descBuilder.Property(pd => pd.Value)
                    .HasColumnName("Description")
                    .IsRequired(false)
                    .HasMaxLength(500);
            });

            builder.OwnsOne(p => p.StockLevel, stockBuilder =>
            {
                stockBuilder.Property(sl => sl.Value)
                    .HasColumnName("StockQuantity")
                    .IsRequired();
            });

            // Category ile ilişki CategoryConfiguration.cs'te tanımlandığı için
            // burada tekrar tanımlamaya gerek yok - çifte tanım sorunu yaratıyor

            // BaseEntity'den gelen CreatedAt ve UpdatedAt
            builder.Property(p => p.CreatedAt).IsRequired();
            builder.Property(p => p.UpdatedAt); // Null olabilir
        }
    }
} 