using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Order.Requests
{
    public class CreateOrderItemRequest
    {
        [Required(ErrorMessage = "ProductId is required.")]
        public long ProductId { get; set; }

        [Required(ErrorMessage = "Count is required.")]
        [Range(1, long.MaxValue, ErrorMessage = "Count must be at least 1.")]
        public long Count { get; set; }
    }
}
