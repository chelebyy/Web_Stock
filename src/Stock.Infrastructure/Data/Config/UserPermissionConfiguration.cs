using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stock.Domain.Entities;

namespace Stock.Infrastructure.Data.Config
{
    public class UserPermissionConfiguration : IEntityTypeConfiguration<UserPermission>
    {
        public void Configure(EntityTypeBuilder<UserPermission> builder)
        {
            builder.ToTable("UserPermissions");
            
            // Composite primary key
            builder.HasKey(up => new { up.UserId, up.PermissionId });
            
            // Configure UserId to be first part of key
            builder.Property(up => up.UserId)
                .HasColumnOrder(0)
                .IsRequired();
                
            // Configure PermissionId to be second part of key
            builder.Property(up => up.PermissionId)
                .HasColumnOrder(1)
                .IsRequired();
                
            // Configure IsGranted
            builder.Property(up => up.IsGranted)
                .IsRequired();
                
            // Configure CreatedAt
            builder.Property(up => up.CreatedAt)
                .IsRequired();
                
            // User relationship
            builder.HasOne(up => up.User)
                .WithMany(u => u.UserPermissions)
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);
                
            // Permission relationship
            builder.HasOne(up => up.Permission)
                .WithMany(p => p.UserPermissions)
                .HasForeignKey(up => up.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 