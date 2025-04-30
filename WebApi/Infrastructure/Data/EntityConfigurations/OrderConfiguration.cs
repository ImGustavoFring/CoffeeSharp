using CoffeeSharp.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Infrastructure.Data.EntityConfigurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> entity)
        {
            entity.HasKey(order => order.Id)
                .HasName("orders_pkey");

            entity.ToTable("orders");

            entity.Property(order => order.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");

            entity.Property(order => order.BranchId)
                .HasColumnName("branch_id");

            entity.Property(order => order.ClientId)
                .HasColumnName("client_id");

            entity.Property(order => order.CreatedAt)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("created_at");

            entity.Property(order => order.DoneAt)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("done_at");

            entity.Property(order => order.FinishedAt)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("finished_at");

            entity.Property(order => order.ExpectedIn)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("expected_in");

            entity.Property(order => order.ClientNote)
                .HasColumnName("client_note");

            entity.HasOne(order => order.Branch)
                .WithMany(branch => branch.Orders)
                .HasForeignKey(order => order.BranchId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("orders_branch_id_fkey");

            entity.HasOne(order => order.Client)
                .WithMany(client => client.Orders)
                .HasForeignKey(order => order.ClientId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("orders_client_id_fkey");
        }
    }
}
