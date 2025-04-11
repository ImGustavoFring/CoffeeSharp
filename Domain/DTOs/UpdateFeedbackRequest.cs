using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class UpdateFeedbackRequest
    {
        [Required(ErrorMessage = "Id is required.")]
        public long Id { get; set; }

        [Required(ErrorMessage = "Content is required.")]
        public string Content { get; set; } = string.Empty;

        [Required(ErrorMessage = "RatingId is required.")]
        public long RatingId { get; set; }
    }
}
