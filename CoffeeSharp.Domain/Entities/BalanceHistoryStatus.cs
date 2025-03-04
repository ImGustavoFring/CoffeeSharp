using System;
using System.Collections.Generic;

namespace CoffeeSharp.Domain.Context.Entities;


public class BalanceHistoryStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<BalanceHistory> BalanceHistories { get; set; } = new List<BalanceHistory>();
}
