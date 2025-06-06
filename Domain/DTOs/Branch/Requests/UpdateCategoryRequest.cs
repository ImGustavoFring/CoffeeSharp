﻿using Domain.DTOs.ProductCatalog.Requests;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Branch.Requests
{
    public class UpdateCategoryRequest : CreateCategoryRequest
    {
        [Required(ErrorMessage = "ID is required")]
        public long Id { get; set; }
    }
}
