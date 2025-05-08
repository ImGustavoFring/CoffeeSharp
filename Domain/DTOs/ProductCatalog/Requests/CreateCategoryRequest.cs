using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.ProductCatalog.Requests
{
    public class CreateCategoryRequest
    {
        [Required(ErrorMessage = "Category name is required")]
        [StringLength(100, ErrorMessage = "Category name must not exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        public long? ParentId { get; set; }
    }
}
