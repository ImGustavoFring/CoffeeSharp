using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Branch.Requests
{
    public class GetBranchesRequest
    {
        [StringLength(100, ErrorMessage = "Search term must be at most 100 characters")]
        public string? Name { get; set; }

        [StringLength(100, ErrorMessage = "Address term must be at most 100 characters")]
        public string? Address { get; set; }

        public int Page { get; set; } = 0;
        public int PageSize { get; set; } = 50;
    }
}
