using System;
using System.Collections.Generic;

namespace CoffeeSharp.Domain.Entities;


public class Category
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public long? ParentCategoryId { get; set; } = null;

    public virtual ICollection<Category>? ChildСategories { get; set; } = new List<Category>();

    public virtual Category? ParentCategory { get; set; } = null!;

    public virtual ICollection<Product>? Products { get; set; } = new List<Product>();
}
