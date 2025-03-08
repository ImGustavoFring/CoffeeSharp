using System;

namespace CoffeeSharp.Domain.Entities;

public class Feedback
{
    public int Id { get; set; }

    public string Content { get; set; } = null!;

    public int RatingId { get; set; }

    public int ClientId { get; set; }

    public int OrderId { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;

    public virtual Rating Rating { get; set; } = null!;
}
