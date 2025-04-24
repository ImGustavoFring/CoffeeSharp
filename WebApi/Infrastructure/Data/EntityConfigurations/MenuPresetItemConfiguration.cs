

using CoffeeSharp.Domain.Entities;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApi.Infrastructure.Data.EntityConfigurations
{
    public class MenuPresetItemConfiguration : IEntityTypeConfiguration<MenuPresetItems>
    {
        public void Configure(EntityTypeBuilder<MenuPresetItems> entity)
        {
            entity.HasKey(menuPresetItem => menuPresetItem.Id)
                .HasName("menu_preset_items_pkey");

            entity.ToTable("menu_preset_items");

            entity.Property(menuPresetItem => menuPresetItem.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");

            entity.HasOne(menuPresetItem => menuPresetItem.MenuPreset)
                 .WithMany(menuPreset => menuPreset.MenuPresetItems)
                 .HasForeignKey(menuPresetItem => menuPresetItem.MenuPresetId)
                 .OnDelete(DeleteBehavior.Restrict)
                 .HasConstraintName("menu_preset_items_menu_preset_id_fkey");

            entity.HasOne(menuPresetItem => menuPresetItem.Product)
                 .WithMany(product => product.MenuPresetItems)
                 .HasForeignKey(menuPresetItem => menuPresetItem.ProductId)
                 .OnDelete(DeleteBehavior.Restrict)
                 .HasConstraintName("menu_preset_items_product_id_fkey");
        }
    }
}
