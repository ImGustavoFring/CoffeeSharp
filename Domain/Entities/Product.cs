using System;
using System.Collections.Generic;
using Domain.Entities;

namespace CoffeeSharp.Domain.Entities;


public class Product
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public long? CategoryId { get; set; } = null!;

    public virtual ICollection<MenuPresetItem>? MenuPresetItems { get; set; } = new List<MenuPresetItem>();

    public virtual Category? Category { get; set; } = null!;

    public virtual ICollection<OrderItem>? OrderItems { get; set; } = new List<OrderItem>();
}
