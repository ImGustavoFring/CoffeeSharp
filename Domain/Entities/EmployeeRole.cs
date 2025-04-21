using System;
using System.Collections.Generic;

namespace CoffeeSharp.Domain.Entities;


public class EmployeeRole
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Employee>? Employees { get; set; } = new List<Employee>();
}
