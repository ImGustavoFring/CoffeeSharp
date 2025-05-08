using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Order.Requests
{
    public class CreateFeedbackRequest
    {
        [Required(ErrorMessage = "Content is required.")]
        public string Content { get; set; } = string.Empty;

        [Required(ErrorMessage = "RatingId is required.")]
        public long RatingId { get; set; }

        [Required(ErrorMessage = "OrderId is required.")]
        public long OrderId { get; set; }
    }
}
