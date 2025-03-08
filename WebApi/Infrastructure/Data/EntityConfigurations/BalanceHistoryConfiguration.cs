using CoffeeSharp.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Infrastructure.Data.EntityConfigurations
{
    public class BalanceHistoryConfiguration : IEntityTypeConfiguration<BalanceHistory>
    {
        public void Configure(EntityTypeBuilder<BalanceHistory> entity)
        {
            entity.HasKey(e => e.Id).HasName("balancehistories_pkey");
            entity.ToTable("balancehistories");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.FinishedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("finishedat");
            entity.Property(e => e.BalanceHistoryStatusId)
                .HasColumnName("statusid");
            entity.Property(e => e.Sum)
                .HasColumnName("sum");
            entity.Property(e => e.ClientId)
                .HasColumnName("userid");

            entity.HasOne(d => d.BalanceHistoryStatus)
                .WithMany(p => p.BalanceHistories)
                .HasForeignKey(d => d.BalanceHistoryStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("balancehistories_statusid_fkey");

            entity.HasOne(d => d.Client)
                .WithMany(p => p.BalanceHistories)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("balancehistories_userid_fkey");
        }
    }
}
