using System;
using System.Collections.Generic;
using CoffeeSharp.Domain.Entities;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using WebApi.Infrastructure.Data.EntityConfigurations;

namespace CoffeeSharp.WebApi.Infrastructure.Data;

public partial class CoffeeSharpDbContext : DbContext
{
    public CoffeeSharpDbContext()
    {
    }

    public CoffeeSharpDbContext(DbContextOptions<CoffeeSharpDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BalanceHistory> BalanceHistories { get; set; }
    public virtual DbSet<BalanceHistoryStatus> BalanceHistoryStatuses { get; set; }
    public virtual DbSet<Branch> Branches { get; set; }
    public virtual DbSet<BranchMenu> BranchMenus { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Client> Clients { get; set; }
    public virtual DbSet<Employee> Employees { get; set; }
    public virtual DbSet<EmployeeRole> EmployeeRoles { get; set; }
    public virtual DbSet<Feedback> Feedbacks { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderItem> OrderItems { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<Rating> Ratings { get; set; }
    public virtual DbSet<Admin> Admins { get; set; }
    public virtual DbSet<MenuPreset> MenuPresets { get; set; }
    public virtual DbSet<MenuPresetItem> MenuPresetItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BalanceHistoryConfiguration());
        modelBuilder.ApplyConfiguration(new BalanceHistoryStatusConfiguration());
        modelBuilder.ApplyConfiguration(new BranchConfiguration());
        modelBuilder.ApplyConfiguration(new BranchMenuConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new ClientConfiguration());
        modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
        modelBuilder.ApplyConfiguration(new EmployeeRoleConfiguration());
        modelBuilder.ApplyConfiguration(new FeedbackConfiguration());
        modelBuilder.ApplyConfiguration(new OrderConfiguration());
        modelBuilder.ApplyConfiguration(new OrderItemConfiguration());
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new RatingConfiguration());
        modelBuilder.ApplyConfiguration(new AdminConfiguration());
        modelBuilder.ApplyConfiguration(new MenuPresetConfiguration());
        modelBuilder.ApplyConfiguration(new MenuPresetItemConfiguration());

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
