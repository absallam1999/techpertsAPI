using TechpertsSolutions.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;

namespace TechpertsSolutions.Core.Entities
{
    public class DeliveryPerson : BaseEntity
    {
        public string? VehicleNumber { get; set; }
        public string? VehicleType { get; set; }
        public string? VehicleImage { get; set; }
        public DeliveryPersonStatus AccountStatus { get; set; } = DeliveryPersonStatus.Pending;
        public bool IsAvailable { get; set; } = false;
        public string UserId { get; set; }
        public AppUser? User { get; set; }
        public string RoleId { get; set; }
        public AppRole? Role { get; set; }

        public List<Delivery>? Deliveries { get; set; } = new List<Delivery>();
    }
}