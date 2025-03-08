using CoffeeSharp.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Infrastructure.Data.EntityConfigurations
{
    public class BranchMenuConfiguration : IEntityTypeConfiguration<BranchMenu>
    {
        public void Configure(EntityTypeBuilder<BranchMenu> entity)
        {
            entity.HasKey(e => e.Id).HasName("branchmenus_pkey");
            entity.ToTable("branchmenus");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Availability)
                .HasColumnName("availability");
            entity.Property(e => e.BranchId)
                .HasColumnName("branchid");
            entity.Property(e => e.ProductId)
                .HasColumnName("productid");

            entity.HasOne(d => d.Branch)
                .WithMany(p => p.BranchMenus)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("branchmenus_branchid_fkey");

            entity.HasOne(d => d.Product)
                .WithMany(p => p.BranchMenus)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("branchmenus_productid_fkey");
        }
    }
}
