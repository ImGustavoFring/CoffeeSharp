using System;
using System.Collections.Generic;

namespace CoffeeSharp.Domain.Entities;


public class BalanceHistoryStatus
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<BalanceHistory> BalanceHistories { get; set; } = new List<BalanceHistory>();
}
