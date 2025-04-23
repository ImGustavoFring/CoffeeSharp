using System;
using System.Collections.Generic;
using Domain.Entities;

namespace CoffeeSharp.Domain.Entities;


public class BranchMenu
{
    public long Id { get; set; }

    public long? MenuPresetItemsId { get; set; } = null!;

    public long? BranchId { get; set; } = null!;

    public bool Availability { get; set; }

    public virtual Branch? Branch { get; set; } = null!;

    public virtual MenuPresetItem? MenuPresetItems { get; set; } = null!;
}
