using CoffeeSharp.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Infrastructure.Data.EntityConfigurations
{
    public class BalanceHistoryStatusConfiguration : IEntityTypeConfiguration<BalanceHistoryStatus>
    {
        public void Configure(EntityTypeBuilder<BalanceHistoryStatus> entity)
        {
            entity.HasKey(e => e.Id).HasName("balancehistorystatuses_pkey");
            entity.ToTable("balancehistorystatuses");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        }
    }
}
