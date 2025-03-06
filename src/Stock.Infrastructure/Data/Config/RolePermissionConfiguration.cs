using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stock.Domain.Entities;

namespace Stock.Infrastructure.Data.Config
{
    public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.RoleId)
                .IsRequired();

            builder.Property(x => x.PermissionId)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.HasOne(x => x.Role)
                .WithMany(x => x.RolePermissions)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Permission)
                .WithMany(x => x.RolePermissions)
                .HasForeignKey(x => x.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => new { x.RoleId, x.PermissionId })
                .IsUnique();
        }
    }
} 