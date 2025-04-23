using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApi.Infrastructure.Data.EntityConfigurations
{
    public class MenuPresetConfiguration : IEntityTypeConfiguration<MenuPreset>
    {
        public void Configure(EntityTypeBuilder<MenuPreset> entity)
        {
            entity.HasKey(e => e.Id).HasName("menu_presets_pkey");

            entity.ToTable("menu_presets");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");

            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");

            entity.Property(e => e.Description)
               .HasColumnName("description");

            entity.HasIndex(e => e.Name)
                .IsUnique();
        }
    }
}
