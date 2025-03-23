using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeSharp.Domain.Entities;

namespace Domain.Entities;

public class MenuPresetItem
{
    public long Id { get; set; }

    public long ProductId { get; set; }

    public virtual Product Product { get; set; } = null!;

    public long MenuPresetId { get; set; }

    public virtual MenuPreset MenuPreset { get; set; } = null!;

    public virtual ICollection<BranchMenu> BranchMenus { get; set; } = new List<BranchMenu>();


}
