using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class CreateBranchMenuRequest
    {
        [Required(ErrorMessage = "MenuPresetItemId is required.")]
        public long MenuPresetItemId { get; set; }

        [Required(ErrorMessage = "BranchId is required.")]
        public long BranchId { get; set; }

        public bool Availability { get; set; }
    }
}
