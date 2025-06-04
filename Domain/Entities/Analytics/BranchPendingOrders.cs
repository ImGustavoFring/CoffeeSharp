using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Analytics
{
    public class BranchPendingOrders
    {
        public long BranchId { get; set; }
        public string BranchName { get; set; } = null!;
        public int PendingCount { get; set; }
    }

}
