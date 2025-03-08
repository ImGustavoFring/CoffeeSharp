using System;
using System.Collections.Generic;

namespace CoffeeSharp.Domain.Entities;


public class Rating
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Value { get; set; }

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
}
