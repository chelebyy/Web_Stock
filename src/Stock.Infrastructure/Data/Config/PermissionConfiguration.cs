using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stock.Domain.Entities.Permissions;

namespace Stock.Infrastructure.Data.Config
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Description)
                .HasMaxLength(200);

            builder.Property(x => x.ResourceType)
                .HasMaxLength(50);

            builder.Property(x => x.Group)
                .HasMaxLength(50);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.UpdatedAt);

            builder.HasIndex(x => x.Name)
                .IsUnique();

            builder.HasIndex(x => x.Group);
            builder.HasIndex(x => x.ResourceType);

            builder.HasIndex(x => new { x.Group, x.ResourceType });
        }
    }
} 