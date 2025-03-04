using System;
using System.Collections.Generic;

namespace CoffeeSharp.Domain.Context.Entities;

public class BalanceHistory
{
    public int Id { get; set; }

    public int UserId { get; set; } // Client ???

    public decimal Sum { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? FinishedAt { get; set; }

    public int BalanceHistoryStatusId { get; set; }

    public virtual BalanceHistoryStatus Status { get; set; } = null!;

    public virtual Client User { get; set; } = null!;
}
