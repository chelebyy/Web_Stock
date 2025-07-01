using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stock.Domain.Entities;
using Stock.Domain.ValueObjects;

namespace Stock.Infrastructure.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);

            builder.OwnsOne(c => c.Name, nameBuilder =>
            {
                nameBuilder.Property(n => n.Value)
                    .HasColumnName("Name")
                    .IsRequired()
                    .HasMaxLength(CategoryName.MaxLength);

                nameBuilder.HasIndex(n => n.Value).IsUnique();
            });

            builder.Property(c => c.Description)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasMaxLength(500);

            builder.Navigation(c => c.Products)
                   .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(c => c.CreatedAt).IsRequired();
            builder.Property(c => c.UpdatedAt);
        }
    }
} 