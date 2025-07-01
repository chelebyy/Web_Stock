using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stock.Domain.Entities;

namespace Stock.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            // Value Object'lerin konfigürasyonu
            builder.OwnsOne(u => u.FullName, fullNameBuilder =>
            {
                fullNameBuilder.Property(fn => fn.Adi)
                    .HasColumnName("Adi")
                    .IsRequired()
                    .HasMaxLength(100);

                fullNameBuilder.Property(fn => fn.Soyadi)
                    .HasColumnName("Soyadi")
                    .IsRequired()
                    .HasMaxLength(100);
            });

            builder.OwnsOne(u => u.Sicil, sicilBuilder =>
            {
                sicilBuilder.Property(s => s.Value)
                    .HasColumnName("Sicil")
                    .IsRequired()
                    .HasMaxLength(50);
                
                // Index'i OwnsOne yapılandırması içinde tanımlama
                sicilBuilder.HasIndex(s => s.Value)
                    .IsUnique()
                    .HasDatabaseName("IX_Users_Sicil");
            });

            builder.Property(u => u.PasswordHash)
                .IsRequired();

            builder.Property(u => u.IsAdmin)
                .HasDefaultValue(false);

            builder.Property(u => u.CreatedAt)
                .IsRequired();

            builder.Property(u => u.IsDeleted)
                .HasDefaultValue(false);

            // Relationships
            builder.HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
} 