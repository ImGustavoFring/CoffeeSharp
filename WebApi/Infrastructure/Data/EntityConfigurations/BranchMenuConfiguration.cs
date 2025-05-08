using CoffeeSharp.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Infrastructure.Data.EntityConfigurations
{
    public class BranchMenuConfiguration : IEntityTypeConfiguration<BranchMenu>
    {
        public void Configure(EntityTypeBuilder<BranchMenu> entity)
        {
            entity.HasKey(branchMenu => branchMenu.Id)
                .HasName("branch_menus_pkey");

            entity.ToTable("branch_menus");

            entity.Property(branchMenu => branchMenu.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");

            entity.Property(branchMenu => branchMenu.Availability)
                .HasColumnName("availability");

            entity.Property(branchMenu => branchMenu.BranchId)
                .HasColumnName("branch_id");

            entity.Property(branchMenu => branchMenu.MenuPresetItemsId)
                .HasColumnName("menu_preset_item_Id");

            entity.HasOne(branchMenu => branchMenu.Branch)
                .WithMany(branch => branch.BranchMenus)
                .HasForeignKey(branchMenu => branchMenu.BranchId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("branch_menus_branch_id_fkey");

            entity.HasOne(branchMenu => branchMenu.MenuPresetItems)
                .WithMany(menuPresetItems => menuPresetItems.BranchMenus)
                .HasForeignKey(branchMenu => branchMenu.MenuPresetItemsId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("branch_menus_menu_preset_item_id_fkey");

            entity.HasIndex(branchMenu => branchMenu.Availability);
        }
    }
}
