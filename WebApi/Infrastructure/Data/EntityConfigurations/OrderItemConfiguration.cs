using CoffeeSharp.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Infrastructure.Data.EntityConfigurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> entity)
        {
            entity.HasKey(orderItem => orderItem.Id)
                .HasName("order_items_pkey");

            entity.ToTable("order_items");

            entity.Property(orderItem => orderItem.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");

            entity.Property(orderItem => orderItem.Count)
                .HasColumnName("count");

            entity.Property(orderItem => orderItem.DoneAt)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("done_at");

            entity.Property(orderItem => orderItem.EmployeeId)
                .HasColumnName("employee_id");

            entity.Property(orderItem => orderItem.OrderId)
                .HasColumnName("order_id");

            entity.Property(orderItem => orderItem.Price)
                .HasColumnName("price");

            entity.Property(orderItem => orderItem.ProductId)
                .HasColumnName("product_id");

            entity.Property(orderItem => orderItem.StartedAt)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("started_at");

            entity.HasOne(orderItem => orderItem.Employee)
                .WithMany(employee => employee.OrderItems)
                .HasForeignKey(orderItem => orderItem.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("order_items_employee_id_fkey");

            entity.HasOne(orderItem => orderItem.Order)
                .WithMany(order => order.OrderItems)
                .HasForeignKey(orderItem => orderItem.OrderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("order_items_order_id_fkey");

            entity.HasOne(orderItem => orderItem.Product)
                .WithMany(product => product.OrderItems)
                .HasForeignKey(orderItem => orderItem.ProductId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("order_items_product_id_fkey");
        }
    }
}
