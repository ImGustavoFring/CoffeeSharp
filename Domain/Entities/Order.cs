using System;
using System.Collections.Generic;

namespace CoffeeSharp.Domain.Entities;

public class Order
{
    public long Id { get; set; }

    public string? ClientNote { get; set; } = null;

    public long ClientId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? DoneAt { get; set; } = null;

    public DateTime? FinishedAt { get; set; } = null;
    public DateTime? ExpectedIn { get; set; } = null;

    public long BranchId { get; set; }

    public virtual Branch? Branch { get; set; } = null!;

    public virtual Client? Client { get; set; } = null!;

    public virtual Feedback? Feedback { get; set; } = null;

    public virtual ICollection<OrderItem>? OrderItems { get; set; } = new List<OrderItem>();
}
