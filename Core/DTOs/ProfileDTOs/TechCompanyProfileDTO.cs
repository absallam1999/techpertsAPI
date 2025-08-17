using Core.DTOs.DeliveryDTOs;
using Core.DTOs.MaintenanceDTOs;
using Core.DTOs.PCAssemblyDTOs;
using Core.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.ProfileDTOs
{
    public class TechCompanyProfileDTO
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string? Website { get; set; }
        public string? Description { get; set; }

        // Show lists, not just counts
        public List<ProductCardDTO>? Products { get; set; }
        public List<MaintenanceDetailsDTO>? Maintenances { get; set; }
        public List<DeliveryReadDTO>? Deliveries { get; set; }
        public List<PCAssemblyReadDTO>? PCAssemblies { get; set; }
    }
}
