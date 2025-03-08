using System;
using System.Collections.Generic;

namespace CoffeeSharp.Domain.Entities;


public class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int CategoryId { get; set; }

    public virtual ICollection<BranchMenu> BranchMenus { get; set; } = new List<BranchMenu>();

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
