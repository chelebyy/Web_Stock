using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stock.Domain.Entities;
using Stock.Domain.Entities.Permissions;

namespace Stock.Infrastructure.Data.Configurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("Permissions");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.Description)
                .HasMaxLength(200);

            builder.Property(p => p.Group)
                .HasMaxLength(50);

            builder.Property(p => p.ResourceType)
                .HasMaxLength(50);

            builder.Property(p => p.ResourceName)
                .HasMaxLength(50);

            builder.Property(p => p.Action)
                .HasMaxLength(50);

            builder.Property(p => p.CreatedAt)
                .IsRequired();

            builder.Property(p => p.IsDeleted)
                .HasDefaultValue(false);

            // İsim benzersiz olmalı
            builder.HasIndex(p => p.Name)
                .IsUnique();
        }
    }
} 