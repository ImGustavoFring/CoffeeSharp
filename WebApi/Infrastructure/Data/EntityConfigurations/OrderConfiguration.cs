﻿using CoffeeSharp.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Infrastructure.Data.EntityConfigurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> entity)
        {
            entity.HasKey(e => e.Id).HasName("orders_pkey");

            entity.ToTable("orders");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");

            entity.Property(e => e.BranchId)
                .HasColumnName("branch_id");

            entity.Property(e => e.ClientId)
                .HasColumnName("client_id");

            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("created_at");

            entity.Property(e => e.DoneAt)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("done_at");

            entity.Property(e => e.FinishedAt)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("finished_at");

            entity.Property(e => e.ExpectedIn)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("expected_in");

            entity.Property(e => e.ClientNote)
                .HasColumnName("client_note");

            entity.HasOne(d => d.Branch)
                .WithMany(p => p.Orders)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("orders_branch_id_fkey");

            entity.HasOne(d => d.Client)
                .WithMany(p => p.Orders)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("orders_client_id_fkey");
        }
    }
}
