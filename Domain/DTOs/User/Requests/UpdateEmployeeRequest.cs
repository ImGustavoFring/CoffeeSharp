﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.User.Requests
{
    public class UpdateEmployeeRequest : CreateEmployeeRequest
    {
        [Required(ErrorMessage = "ID is required")]
        public long Id { get; set; }

        public new string? Password { get; set; }
    }
}
