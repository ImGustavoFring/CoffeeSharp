using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class CreateCategoryRequest
    {
        public string Name { get; set; } = string.Empty;
        public long? ParentId { get; set; }
    }
}
