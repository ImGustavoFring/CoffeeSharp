using System;
using System.Collections.Generic;

namespace CoffeeSharp.Domain.Context.Entities;


public class Order
{
    public int Id { get; set; }

    public string? UserNote { get; set; }

    public int ClientId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? DoneAt { get; set; }

    public DateTime? FinishedAt { get; set; }

    public int BranchId { get; set; }

    public int? FeedbackId { get; set; } // one to one

    public virtual Branch Branch { get; set; } = null!;

    public virtual Client Client { get; set; } = null!;

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>(); // ??? ef core moment, it should be one to one 

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
