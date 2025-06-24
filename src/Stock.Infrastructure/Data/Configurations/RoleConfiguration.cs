using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stock.Domain.Entities;

namespace Stock.Infrastructure.Data.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(50)
                .UsePropertyAccessMode(PropertyAccessMode.Property);

            builder.Property(r => r.Description)
                .HasMaxLength(200)
                .UsePropertyAccessMode(PropertyAccessMode.Property);

            builder.Property(r => r.CreatedAt)
                .IsRequired();

            builder.Property(r => r.IsDeleted)
                .HasDefaultValue(false);
        }
    }
} 