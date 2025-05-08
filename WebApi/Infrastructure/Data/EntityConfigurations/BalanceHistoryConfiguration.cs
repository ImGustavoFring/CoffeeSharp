using CoffeeSharp.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Infrastructure.Data.EntityConfigurations
{
    public class BalanceHistoryConfiguration : IEntityTypeConfiguration<BalanceHistory>
    {
        public void Configure(EntityTypeBuilder<BalanceHistory> entity)
        {
            entity.HasKey(balanceHistory => balanceHistory.Id)
                .HasName("balance_histories_pkey");

            entity.ToTable("balance_histories");

            entity.Property(balanceHistory => balanceHistory.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");

            entity.Property(balanceHistory => balanceHistory.CreatedAt)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("created_at");

            entity.Property(balanceHistory => balanceHistory.FinishedAt)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("finished_at");

            entity.Property(balanceHistory => balanceHistory.BalanceHistoryStatusId)
                .HasColumnName("balance_history_status_id");

            entity.Property(balanceHistory => balanceHistory.Sum)
                .HasColumnName("sum");

            entity.Property(balanceHistory => balanceHistory.ClientId)
                .HasColumnName("client_id");

            entity.HasOne(balanceHistory => balanceHistory.BalanceHistoryStatus)
                .WithMany(balanceHistoryStatus => balanceHistoryStatus.BalanceHistories)
                .HasForeignKey(balanceHistory => balanceHistory.BalanceHistoryStatusId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("balance_histories_status_id_fkey");

            entity.HasOne(balanceHistory => balanceHistory.Client)
                .WithMany(client => client.BalanceHistories)
                .HasForeignKey(balanceHistory => balanceHistory.ClientId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("balance_histories_client_id_fkey");
        }
    }
}
