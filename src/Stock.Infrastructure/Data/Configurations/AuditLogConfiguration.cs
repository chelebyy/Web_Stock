using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stock.Domain.Entities;

namespace Stock.Infrastructure.Data.Configurations
{
    public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.ToTable("AuditLogs");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Action)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(a => a.EntityType)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.EntityId)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(a => a.UserId);

            builder.Property(a => a.Path)
                .HasMaxLength(200);

            builder.Property(a => a.CreatedAt)
                .IsRequired();

            builder.Property(a => a.IsDeleted)
                .HasDefaultValue(false);
                
            // User tablosuyla ilişki tanımlıyoruz
            builder.HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .IsRequired(false)  // İlişki nullable olmalı
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
} 