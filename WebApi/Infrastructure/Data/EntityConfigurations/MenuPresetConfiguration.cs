using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApi.Infrastructure.Data.EntityConfigurations
{
    public class MenuPresetConfiguration : IEntityTypeConfiguration<MenuPreset>
    {
        public void Configure(EntityTypeBuilder<MenuPreset> entity)
        {
            entity.HasKey(menuPreset => menuPreset.Id)
                .HasName("menu_presets_pkey");

            entity.ToTable("menu_presets");

            entity.Property(menuPreset => menuPreset.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");

            entity.Property(menuPreset => menuPreset.Name)
                .HasMaxLength(30)
                .HasColumnName("name");

            entity.Property(menuPreset => menuPreset.Description)
               .HasColumnName("description");

            entity.HasIndex(menuPreset => menuPreset.Name)
                .IsUnique();
        }
    }
}
