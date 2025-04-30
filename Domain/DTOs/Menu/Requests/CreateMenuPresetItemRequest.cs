using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Menu.Requests
{
    public class CreateMenuPresetItemRequest
    {
        [Required(ErrorMessage = "ProductId is required.")]
        public long ProductId { get; set; }

        [Required(ErrorMessage = "MenuPresetId is required.")]
        public long MenuPresetId { get; set; }
    }
}
