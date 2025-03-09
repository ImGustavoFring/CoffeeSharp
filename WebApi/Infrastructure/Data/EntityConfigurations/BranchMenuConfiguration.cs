using CoffeeSharp.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Infrastructure.Data.EntityConfigurations
{
    public class BranchMenuConfiguration : IEntityTypeConfiguration<BranchMenu>
    {
        public void Configure(EntityTypeBuilder<BranchMenu> entity)
        {
            entity.HasKey(e => e.Id).HasName("branch_menus_pkey");

            entity.ToTable("branch_menus");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");

            entity.Property(e => e.Availability)
                .HasColumnName("availability");

            entity.Property(e => e.BranchId)
                .HasColumnName("branch_id");

            entity.Property(e => e.ProductId)
                .HasColumnName("product_id");

            entity.HasOne(d => d.Branch)
                .WithMany(p => p.BranchMenus)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("branch_menus_branch_id_fkey");

            entity.HasOne(d => d.Product)
                .WithMany(p => p.BranchMenus)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("branch_menus_product_id_fkey");
        }
    }
}
