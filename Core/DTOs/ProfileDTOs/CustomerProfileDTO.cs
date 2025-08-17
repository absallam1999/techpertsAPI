using Core.DTOs.CartDTOs;
using Core.DTOs.DeliveryDTOs;
using Core.DTOs.MaintenanceDTOs;
using Core.DTOs.OrderDTOs;
using Core.DTOs.PCAssemblyDTOs;
using Core.DTOs.WishListDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.ProfileDTOs
{
    public class CustomerProfileDTO
    {
        public string UserId { get; set; }
        public string FullName { get; set; }

        // Show actual entities instead of counts
        public CartReadDTO Cart { get; set; }
        public WishListReadDTO? WishList { get; set; }
        public List<PCAssemblyReadDTO>? PCAssemblies { get; set; }
        public List<OrderReadDTO>? Orders { get; set; }
        public List<DeliveryReadDTO>? Deliveries { get; set; }
        public List<MaintenanceDetailsDTO>? Maintenances { get; set; }
    }
}
