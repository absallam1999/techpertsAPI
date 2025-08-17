using Core.DTOs.DeliveryDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.ProfileDTOs
{
    public class DeliveryPersonProfileDTO
    {
        public string UserId { get; set; }
        public string FullName { get; set; }

        public string? VehicleNumber { get; set; }
        public string? VehicleType { get; set; }
        public string? VehicleImage { get; set; }
        public string? License { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime? LastOnline { get; set; }

        // Show full lists
        public List<DeliveryDTO>? Deliveries { get; set; }
        public List<DeliveryOfferDTO>? Offers { get; set; }
    }
}
