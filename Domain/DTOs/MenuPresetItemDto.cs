using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class MenuPresetItemDto
    {
        public long Id { get; set; }
        public long? ProductId { get; set; }
        public long? MenuPresetId { get; set; }
    }
}
