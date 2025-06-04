using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Analytics
{
    public class BranchRevenue
    {
        public long BranchId { get; set; }
        public string BranchName { get; set; } = null!;
        public decimal TotalRevenue { get; set; }
    }
}
