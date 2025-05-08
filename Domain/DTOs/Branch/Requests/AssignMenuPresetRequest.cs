using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Branch.Requests
{
    public class AssignMenuPresetRequest
    {
        [Required(ErrorMessage = "MenuPresetId is required.")]
        public long MenuPresetId { get; set; }
    }
}
