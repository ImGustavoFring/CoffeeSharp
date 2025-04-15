using System;

namespace CoffeeSharp.Domain.Entities;

public class Feedback
{
    public long Id { get; set; }

    public string Content { get; set; } = null!;

    public long RatingId { get; set; }

    public long OrderId { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Rating? Rating { get; set; } = null!;
}
