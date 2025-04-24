using CoffeeSharp.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Infrastructure.Data.EntityConfigurations
{
    public class RatingConfiguration : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> entity)
        {
            entity.HasKey(rating => rating.Id)
                .HasName("ratings_pkey");

            entity.ToTable("ratings");

            entity.Property(rating => rating.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");

            entity.Property(rating => rating.Name)
                .HasMaxLength(20)
                .HasColumnName("name");

            entity.Property(rating => rating.Value)
                .HasColumnName("value");

            entity.HasIndex(rating => rating.Name)
                .IsUnique();

            entity.HasIndex(rating => rating.Value)
               .IsUnique();
        }
    }
}
