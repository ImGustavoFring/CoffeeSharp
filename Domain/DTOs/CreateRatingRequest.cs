using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class CreateRatingRequest
    {
        [Required(ErrorMessage = "Rating name is required.")]
        [StringLength(100, ErrorMessage = "Rating name must not exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Value is required.")]
        [Range(0, long.MaxValue, ErrorMessage = "Value must be a non-negative number.")]
        public long Value { get; set; }
    }
}
