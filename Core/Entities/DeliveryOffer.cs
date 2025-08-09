using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TechpertsSolutions.Core.Entities;

namespace Core.Entities
{
    public class DeliveryOffer : BaseEntity
    {
        public string DeliveryId { get; set; }
        public string DeliveryPersonId { get; set; }
        public DeliveryOfferStatus Status { get; set; } = DeliveryOfferStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? RespondedAt { get; set; }
        public DateTime? ExpiryTime {get; set;}
        public bool IsActive { get; set; } = true; // Useful for quick filtering

        // Navigation props
        public Delivery Delivery { get; set; }
        public DeliveryPerson DeliveryPerson { get; set; }
    }
}
