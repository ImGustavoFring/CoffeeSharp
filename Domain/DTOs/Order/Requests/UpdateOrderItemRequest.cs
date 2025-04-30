using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Order.Requests
{
    public class UpdateOrderItemRequest
    {
        public long Id { get; set; }
        public long Count { get; set; }
    }
}
