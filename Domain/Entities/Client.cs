using System;
using System.Collections.Generic;

namespace CoffeeSharp.Domain.Entities;


public class Client
{
    public long Id { get; set; }
    public string TelegramId { get; set; } = null!;
    public string Name { get; set; } = null!;

    public decimal Balance { get; set; }

    public virtual ICollection<BalanceHistory> BalanceHistories { get; set; } = new List<BalanceHistory>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
