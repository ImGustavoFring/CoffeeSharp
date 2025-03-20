using System;
using System.Collections.Generic;

namespace CoffeeSharp.Domain.Entities;


public class OrderItem
{
    public long Id { get; set; }

    public long OrderId { get; set; }

    public long ProductId { get; set; }

    public long EmployeeId { get; set; }

    public decimal Price { get; set; }

    public long Count { get; set; }

    public DateTime? StartedAt { get; set; } = null;

    public DateTime? DoneAt { get; set; } = null;

    public virtual Employee Employee { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
