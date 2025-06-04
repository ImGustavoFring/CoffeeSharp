using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Analytics
{
    public class ClientSpending
    {
        public long ClientId { get; set; }
        public string ClientName { get; set; } = null!;
        public decimal TotalSpent { get; set; }
    }
}
