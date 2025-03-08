using System;
using System.Collections.Generic;

namespace CoffeeSharp.Domain.Entities;


public class OrderItem
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int EmployeeId { get; set; }

    public decimal Price { get; set; }

    public int Count { get; set; }

    public DateTime StartedAt { get; set; }

    public DateTime? DoneAt { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
