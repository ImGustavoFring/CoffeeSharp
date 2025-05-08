using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.User.Requests
{
    public record UpdateAdminRequest(
        [Required(ErrorMessage = "ID is required")]
        long Id,

        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
        string UserName,

        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters")]
        string? Password
    );
}
