using CoffeeSharp.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Infrastructure.Data.EntityConfigurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> entity)
        {
            entity.HasKey(product => product.Id)
                .HasName("products_pkey");

            entity.ToTable("products");

            entity.Property(product => product.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");

            entity.Property(product => product.CategoryId)
                .HasColumnName("category_id");

            entity.Property(product => product.Description)
                .HasColumnName("description");

            entity.Property(product => product.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
                
            entity.Property(product => product.Price)
                .HasColumnName("price");

            entity.HasOne(product => product.Category)
                .WithMany(category => category.Products)
                .HasForeignKey(product => product.CategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("products_category_id_fkey");

            entity.HasIndex(product => product.Name)
                .IsUnique();

            entity.HasIndex(product => product.Price);
        }
    }
}
