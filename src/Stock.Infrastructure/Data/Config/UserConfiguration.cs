using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stock.Domain.Entities;

namespace Stock.Infrastructure.Data.Config
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.PasswordHash)
                .IsRequired();

            builder.Property(u => u.Email)
                .HasMaxLength(100);

            builder.Property(u => u.Sicil)
                .HasMaxLength(20);

            builder.HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // İndeksler
            builder.HasIndex(u => u.Username).IsUnique();
            builder.HasIndex(u => u.Email);
            builder.HasIndex(u => u.Sicil);
            builder.HasIndex(u => u.IsDeleted);
            builder.HasIndex(u => u.RoleId);
        }
    }
} 