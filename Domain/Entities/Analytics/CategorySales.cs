using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Analytics
{
    public class CategorySales
    {
        public long CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public int TotalUnitsSold { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
