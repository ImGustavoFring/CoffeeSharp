using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Analytics
{
    public class ProductRevenue
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal TotalRevenue { get; set; }
    }
}
