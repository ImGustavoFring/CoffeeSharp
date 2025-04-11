using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class UpdateBranchRequest : CreateBranchRequest
    {
        [Required(ErrorMessage = "ID is required.")]
        public long Id { get; set; }
    }
}
