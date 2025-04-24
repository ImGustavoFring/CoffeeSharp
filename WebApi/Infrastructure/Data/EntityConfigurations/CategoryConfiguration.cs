using CoffeeSharp.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Infrastructure.Data.EntityConfigurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> entity)
        {
            entity.HasKey(category => category.Id)
                .HasName("categories_pkey");

            entity.ToTable("categories");

            entity.Property(category => category.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");

            entity.Property(category => category.Name)
                .HasMaxLength(30)
                .HasColumnName("name");

            entity.Property(category => category.ParentCategoryId)
                .HasColumnName("parent_category_id");

            entity.HasOne(category => category.ParentCategory)
                .WithMany(parentCategory => parentCategory.ChildСategories)
                .HasForeignKey(category => category.ParentCategoryId)
                .HasConstraintName("categories_parent_id_fkey");

            entity.HasIndex(category => category.Name)
                .IsUnique();
        }
    }
}
