using System;
using System.Collections.Generic;

namespace CoffeeSharp.Domain.Entities;


public class Employee
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int RoleId { get; set; }

    public int BranchId { get; set; }

    public virtual Branch Branch { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual EmployeeRole Role { get; set; } = null!;
}
