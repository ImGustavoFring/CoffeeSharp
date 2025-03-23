using CoffeeSharp.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Infrastructure.Data.EntityConfigurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> entity)
        {
            entity.HasKey(e => e.Id).HasName("order_items_pkey");

            entity.ToTable("order_items");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");

            entity.Property(e => e.Count)
                .HasColumnName("count");

            entity.Property(e => e.DoneAt)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("done_at");

            entity.Property(e => e.EmployeeId)
                .HasColumnName("employee_id");

            entity.Property(e => e.OrderId)
                .HasColumnName("order_id");

            entity.Property(e => e.Price)
                .HasColumnName("price");

            entity.Property(e => e.ProductId)
                .HasColumnName("product_id");

            entity.Property(e => e.StartedAt)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("started_at");

            entity.HasOne(d => d.Employee)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("order_items_employee_id_fkey");

            entity.HasOne(d => d.Order)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("order_items_order_id_fkey");

            entity.HasOne(d => d.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("order_items_product_id_fkey");
        }
    }
}
