using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class BranchMenuDto
    {
        public long Id { get; set; }
        public long? MenuPresetItemId { get; set; }
        public long? BranchId { get; set; }
        public bool Availability { get; set; }
    }
}
