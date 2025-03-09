using CoffeeSharp.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Infrastructure.Data.EntityConfigurations
{
    public class BalanceHistoryConfiguration : IEntityTypeConfiguration<BalanceHistory>
    {
        public void Configure(EntityTypeBuilder<BalanceHistory> entity)
        {
            entity.HasKey(e => e.Id).HasName("balance_histories_pkey");

            entity.ToTable("balance_histories");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");

            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("created_at");

            entity.Property(e => e.FinishedAt)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("finished_at");

            entity.Property(e => e.BalanceHistoryStatusId)
                .HasColumnName("status_id");

            entity.Property(e => e.Sum)
                .HasColumnName("sum");

            entity.Property(e => e.ClientId)
                .HasColumnName("client_id");

            entity.HasOne(d => d.BalanceHistoryStatus)
                .WithMany(p => p.BalanceHistories)
                .HasForeignKey(d => d.BalanceHistoryStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("balance_histories_status_id_fkey");

            entity.HasOne(d => d.Client)
                .WithMany(p => p.BalanceHistories)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("balance_histories_client_id_fkey");
        }
    }
}
