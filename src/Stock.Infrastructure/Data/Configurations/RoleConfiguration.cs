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

            builder.OwnsOne(r => r.Name, nameBuilder =>
            {
                nameBuilder.Property(n => n.Value)
                    .HasColumnName("Name")
                    .IsRequired()
                    .HasMaxLength(50);
                
                nameBuilder.HasIndex(n => n.Value)
                    .IsUnique()
                    .HasDatabaseName("IX_Roles_Name");
            });

            builder.Property(r => r.Description)
                .HasMaxLength(200);

            builder.Property(r => r.CreatedAt)
                .IsRequired();

            builder.Property(r => r.IsDeleted)
                .HasDefaultValue(false);
        }
    }
}