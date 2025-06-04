using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Analytics
{
    public class AverageOrderValueItem
    {
        public DateTime Date { get; set; }
        public decimal AverageValue { get; set; }
    }
}
