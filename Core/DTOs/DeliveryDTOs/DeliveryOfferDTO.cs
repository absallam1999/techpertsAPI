using Core.Entities;
using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.DeliveryDTOs
{
    public class DeliveryOfferDTO
    {
        public string Id { get; set; }
        public string DeliveryId { get; set; }
        public string ClusterId { get; set; }
        public string DeliveryPersonId { get; set; }
        public DeliveryOfferStatus Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ExpiryTime { get; set; }
        public bool IsActive { get; set; }

        // Optional related info
        public string DeliveryTrackingNumber { get; set; }
        public string CustomerName { get; set; }
        public double? DeliveryLatitude { get; set; }
        public double? DeliveryLongitude { get; set; }
        //public DeliveryOfferDTO(DeliveryOffer deliveryOffer)
        //{
        //    Id = string.Empty;
        //    DeliveryId = string.Empty;
        //    ClusterId = string.Empty;
        //    DeliveryPersonId = string.Empty;
        //    Status = DeliveryOfferStatus.Pending;
        //    CreatedAt = DateTime.UtcNow;
        //    ExpiryTime = null;
        //    IsActive = true;
        //    DeliveryTrackingNumber = string.Empty;
        //    CustomerName = string.Empty;
        //}
    }
}
