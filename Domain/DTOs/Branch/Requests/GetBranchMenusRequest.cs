using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Branch.Requests
{
    public class GetBranchMenusRequest
    {
        public long? BranchId { get; set; }

        public long? MenuPresetItemsId { get; set; }

        public bool? Availability { get; set; }
        public int Page { get; set; } = 0;
        public int PageSize { get; set; } = 50;
    }
}
