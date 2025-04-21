using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class OrderItemDto
    {
        public long Id { get; set; }
        public long? OrderId { get; set; }
        public long? ProductId { get; set; }
        public long? EmployeeId { get; set; }
        public decimal Price { get; set; }
        public long Count { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? DoneAt { get; set; }
    }
}
