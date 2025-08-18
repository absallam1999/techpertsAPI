using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.MaintenanceDTOs
{
    public class CreateMaintenanceRequest
    {
        public string? WarrantyId { get; set; }
        public string? TechCompanyId { get; set; }
        public string? Issue { get; set; }
        public string? Priority { get; set; }
        public string? Notes { get; set; }
        public ProductCategory? DeviceType { get; set; }
        public List<string>? DeviceImages { get; set; } = new List<string>();
    }
}
