using System;
using System.Collections.Generic;

namespace CoffeeSharp.Domain.Entities;

public class BalanceHistory
{
    public int Id { get; set; }

    public int ClientId { get; set; }

    public decimal Sum { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? FinishedAt { get; set; }

    public int BalanceHistoryStatusId { get; set; }

    public virtual BalanceHistoryStatus BalanceHistoryStatus { get; set; } = null!;

    public virtual Client Client { get; set; } = null!;
}
