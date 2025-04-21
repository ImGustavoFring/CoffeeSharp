using CoffeeSharp.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Infrastructure.Data.EntityConfigurations
{
    public class RatingConfiguration : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> entity)
        {
            entity.HasKey(e => e.Id).HasName("ratings_pkey");

            entity.ToTable("ratings");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");

            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .HasColumnName("name");

            entity.Property(e => e.Value)
                .HasColumnName("value");

            entity.HasIndex(e => e.Name)
                .IsUnique();

            entity.HasIndex(e => e.Value)
               .IsUnique();
        }
    }
}
