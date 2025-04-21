using System;
using System.Collections.Generic;

namespace CoffeeSharp.Domain.Entities;


public class Branch
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public virtual ICollection<BranchMenu>? BranchMenus { get; set; } = new List<BranchMenu>();

    public virtual ICollection<Employee>? Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Order>? Orders { get; set; } = new List<Order>();
}
