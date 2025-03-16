using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stock.Domain.Entities;

namespace Stock.Infrastructure.Data.Config
{
    /// <summary>
    /// Report entity için konfigürasyon sınıfı
    /// </summary>
    public class ReportConfiguration : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            builder.HasKey(r => r.Id);
            
            builder.Property(r => r.Id)
                .HasMaxLength(50)
                .IsRequired();
            
            builder.Property(r => r.ReportType)
                .HasMaxLength(50)
                .IsRequired();
            
            builder.Property(r => r.Parameters)
                .HasColumnType("nvarchar(max)");
            
            builder.Property(r => r.UserId)
                .HasMaxLength(50)
                .IsRequired();
            
            builder.Property(r => r.CreatedAt)
                .IsRequired();
            
            builder.Property(r => r.Status)
                .IsRequired();
            
            builder.Property(r => r.ErrorMessage)
                .HasMaxLength(500);
            
            builder.Property(r => r.FileName)
                .HasMaxLength(255);
            
            // Indeksler
            builder.HasIndex(r => r.UserId);
            builder.HasIndex(r => r.Status);
            builder.HasIndex(r => r.CreatedAt);
        }
    }
} 