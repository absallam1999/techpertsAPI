﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.TechCompany
{
    public class TechCompanyCreateDTO
    {
        public string? MapLocation { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string UserId { get; set; }
        public string RoleId { get; set; }
    }
}
