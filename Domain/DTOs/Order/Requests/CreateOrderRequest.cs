using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Order.Requests
{
    public class CreateOrderRequest
    {
        [Required(ErrorMessage = "ClientId is required.")]
        public long ClientId { get; set; }

        public string? ClientNote { get; set; }

        [Required(ErrorMessage = "ExpectedIn is required.")]
        public DateTime ExpectedIn { get; set; }

        [Required(ErrorMessage = "BranchId is required.")]
        public long BranchId { get; set; }

        [Required(ErrorMessage = "At least one order item is required.")]
        [MinLength(1, ErrorMessage = "At least one order item must be provided.")]
        public List<NewOrderItemDto> Items { get; set; } = new List<NewOrderItemDto>();
    }
}
