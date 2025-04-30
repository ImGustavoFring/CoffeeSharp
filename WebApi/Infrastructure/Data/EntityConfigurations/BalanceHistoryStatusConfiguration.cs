using CoffeeSharp.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Infrastructure.Data.EntityConfigurations
{
    public class BalanceHistoryStatusConfiguration : IEntityTypeConfiguration<BalanceHistoryStatus>
    {
        public void Configure(EntityTypeBuilder<BalanceHistoryStatus> entity)
        {
            entity.HasKey(balanceHistoryStatus => balanceHistoryStatus.Id)
                .HasName("balance_history_statuses_pkey");

            entity.ToTable("balance_history_statuses");

            entity.Property(balanceHistoryStatus => balanceHistoryStatus.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");

            entity.Property(balanceHistoryStatus => balanceHistoryStatus.Name)
                .HasMaxLength(20)
                .HasColumnName("name");

            entity.HasIndex(balanceHistoryStatus => balanceHistoryStatus.Name)
                  .IsUnique();
        }
    }
}
