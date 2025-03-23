

using CoffeeSharp.Domain.Entities;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApi.Infrastructure.Data.EntityConfigurations
{
    public class MenuPresetItemConfiguration : IEntityTypeConfiguration<MenuPresetItem>
    {
        public void Configure(EntityTypeBuilder<MenuPresetItem> entity)
        {
            entity.HasKey(e => e.Id).HasName("menu_preset_items_pkey");

            entity.ToTable("menu_preset_items");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");

            entity.HasOne(d => d.MenuPreset)
                 .WithMany(p => p.MenuPresetItems)
                 .HasForeignKey(d => d.MenuPresetId)
                 .OnDelete(DeleteBehavior.Cascade)
                 .HasConstraintName("menu_preset_items_menu_preset_id_fkey");

            entity.HasOne(d => d.Product)
                 .WithMany(p => p.MenuPresetItems)
                 .HasForeignKey(d => d.ProductId)
                 .OnDelete(DeleteBehavior.Cascade)
                 .HasConstraintName("menu_preset_items_product_id_fkey");
        }
    }
}
