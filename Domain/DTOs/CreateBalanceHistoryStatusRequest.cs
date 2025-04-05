using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class CreateBalanceHistoryStatusRequest
    {
        [Required(ErrorMessage = "Status name is required.")]
        [StringLength(100, ErrorMessage = "Status name must not exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;
    }
}
