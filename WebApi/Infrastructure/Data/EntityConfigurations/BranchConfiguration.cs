using CoffeeSharp.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Infrastructure.Data.EntityConfigurations
{
    public class BranchConfiguration : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> entity)
        {
            entity.HasKey(branch => branch.Id)
                .HasName("branches_pkey");

            entity.ToTable("branches");

            entity.Property(branch => branch.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");

            entity.Property(branch => branch.Address)
                .HasColumnName("address");

            entity.Property(branch => branch.Name)
                .HasMaxLength(100)
                .HasColumnName("name");

            entity.HasIndex(branch => branch.Name)
                  .IsUnique();

            entity.HasIndex(branch => branch.Address)
                  .IsUnique();
        }
    }
}
