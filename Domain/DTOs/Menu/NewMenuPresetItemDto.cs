using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Menu
{
    public class NewMenuPresetItemDto
    {
        [Required]
        public long ProductId { get; set; }
    }

}
