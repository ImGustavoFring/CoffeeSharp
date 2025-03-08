using CoffeeSharp.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Infrastructure.Data.EntityConfigurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> entity)
        {
            entity.HasKey(e => e.Id).HasName("orderitems_pkey");
            entity.ToTable("orderitems");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Count)
                .HasColumnName("count");
            entity.Property(e => e.DoneAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("doneat");
            entity.Property(e => e.EmployeeId)
                .HasColumnName("employeeid");
            entity.Property(e => e.OrderId)
                .HasColumnName("orderid");
            entity.Property(e => e.Price)
                .HasColumnName("price");
            entity.Property(e => e.ProductId)
                .HasColumnName("productid");
            entity.Property(e => e.StartedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("startedat");

            entity.HasOne(d => d.Employee)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orderitems_employeeid_fkey");

            entity.HasOne(d => d.Order)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orderitems_orderid_fkey");

            entity.HasOne(d => d.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orderitems_productid_fkey");
        }
    }
}
