﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.TechManager
{
    public class TechManagerCreateDTO
    {
        public string? Specialization { get; set; }
        public string UserId { get; set; }
        public string RoleId { get; set; }
    }
}
