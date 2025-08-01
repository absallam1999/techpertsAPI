﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Product
{
    public class WarrantyDTO
    {
        public string Id { get; set; } = string.Empty;
        
        [Required]
        [StringLength(200)]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        public DateTime StartDate { get; set; }
        
        [Required]
        public DateTime EndDate { get; set; }
    }
}
