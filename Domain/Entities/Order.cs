using System;
using System.Collections.Generic;

namespace CoffeeSharp.Domain.Entities;

public class Order
{
    public int Id { get; set; }

    public string? UserNote { get; set; }

    public int ClientId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? DoneAt { get; set; }

    public DateTime? FinishedAt { get; set; }

    public int BranchId { get; set; }

    public virtual Branch Branch { get; set; } = null!;

    public virtual Client Client { get; set; } = null!;

    public virtual Feedback? Feedback { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
