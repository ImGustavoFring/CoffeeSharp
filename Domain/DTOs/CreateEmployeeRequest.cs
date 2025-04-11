using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class CreateEmployeeRequest
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name must not exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "UserName is required")]
        [StringLength(50, ErrorMessage = "UserName must not exceed 50 characters")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "RoleId is required")]
        public long RoleId { get; set; }

        [Required(ErrorMessage = "BranchId is required")]
        public long BranchId { get; set; }
    }
}
