using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.ReferenceData.Requests
{
    public class UpdateEmployeeRoleRequest : CreateEmployeeRoleRequest
    {
        [Required(ErrorMessage = "ID is required.")]
        public long Id { get; set; }
    }
}
