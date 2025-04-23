using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class MenuPreset
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; } = null;

    public virtual ICollection<MenuPresetItem>? MenuPresetItems { get; set; } = new List<MenuPresetItem>();

}

