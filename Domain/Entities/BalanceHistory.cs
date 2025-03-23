using System;
using System.Collections.Generic;

namespace CoffeeSharp.Domain.Entities;

public class BalanceHistory
{
    public long Id { get; set; }

    public long ClientId { get; set; }

    public decimal Sum { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? FinishedAt { get; set; } = null;

    public long BalanceHistoryStatusId { get; set; }

    public virtual BalanceHistoryStatus BalanceHistoryStatus { get; set; } = null!;

    public virtual Client Client { get; set; } = null!;
}
