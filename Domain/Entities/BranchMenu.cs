using System;
using System.Collections.Generic;

namespace CoffeeSharp.Domain.Entities;


public class BranchMenu
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int BranchId { get; set; }

    public bool Availability { get; set; }

    public virtual Branch Branch { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
