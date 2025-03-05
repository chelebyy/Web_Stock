using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stock.Domain.Entities;

namespace Stock.Infrastructure.Data.Configurations
{
    public class UserPermissionConfiguration : IEntityTypeConfiguration<UserPermission>
    {
        public void Configure(EntityTypeBuilder<UserPermission> builder)
        {
            builder.HasKey(up => up.Id);

            builder.HasIndex(up => new { up.UserId, up.PermissionId })
                .IsUnique();

            builder.HasOne(up => up.User)
                .WithMany(u => u.UserPermissions)
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(up => up.Permission)
                .WithMany()
                .HasForeignKey(up => up.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(up => up.IsGranted)
                .IsRequired();

            builder.Property(up => up.CreatedAt)
                .IsRequired();

            builder.Property(up => up.CreatedBy)
                .HasMaxLength(50);

            builder.Property(up => up.LastModifiedBy)
                .HasMaxLength(50);
        }
    }
} 