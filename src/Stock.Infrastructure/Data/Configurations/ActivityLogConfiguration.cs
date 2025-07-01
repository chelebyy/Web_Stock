using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stock.Domain.Entities;

namespace Stock.Infrastructure.Data.Configurations
{
    public class ActivityLogConfiguration : IEntityTypeConfiguration<ActivityLog>
    {
        public void Configure(EntityTypeBuilder<ActivityLog> builder)
        {
            builder.ToTable("ActivityLogs");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.UserId);

            builder.Property(a => a.Username)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(a => a.ActivityType)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(a => a.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(a => a.Timestamp)
                .IsRequired();

            builder.Property(a => a.AdditionalInfo);

            builder.Property(a => a.IsSynchronized)
                .HasDefaultValue(false);

            builder.Property(a => a.CreatedAt)
                .IsRequired();

            builder.Property(a => a.IsDeleted)
                .HasDefaultValue(false);

            // Sorgu performansını artırmak için index'ler
            builder.HasIndex(a => a.UserId);
            builder.HasIndex(a => a.ActivityType);
            builder.HasIndex(a => a.Timestamp);

            builder.HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);
        }
    }
} 