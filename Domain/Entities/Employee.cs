using System;
using System.Collections.Generic;

namespace CoffeeSharp.Domain.Entities;


public class Employee
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public long? RoleId { get; set; } = null!;

    public long? BranchId { get; set; } = null!;

    public virtual Branch? Branch { get; set; } = null!;

    public virtual ICollection<OrderItem>? OrderItems { get; set; } = new List<OrderItem>();

    public virtual EmployeeRole? Role { get; set; } = null!;
}
