using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stock.Domain.Entities;
using Stock.Domain.Entities.Permissions;
using Stock.Domain.ValueObjects;

namespace Stock.Infrastructure.Data.Configurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("Permissions");

            builder.HasKey(p => p.Id);

            builder.OwnsOne(p => p.Name, nameBuilder =>
            {
                nameBuilder.Property(n => n.Value)
                    .HasColumnName("Name")
                    .IsRequired()
                    .HasMaxLength(PermissionName.MaxLength);

                nameBuilder.HasIndex(n => n.Value).IsUnique();
            });

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

            // Gruba göre sorgulamaları hızlandırmak için index
            builder.HasIndex(p => p.Group);
        }
    }
} 