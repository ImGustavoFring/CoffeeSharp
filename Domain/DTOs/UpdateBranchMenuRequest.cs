using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DTOs.Branch.Requests;

namespace Domain.DTOs
{
    public class UpdateBranchMenuRequest : CreateBranchMenuRequest
    {
        [Required(ErrorMessage = "ID is required.")]
        public long Id { get; set; }
    }
}
