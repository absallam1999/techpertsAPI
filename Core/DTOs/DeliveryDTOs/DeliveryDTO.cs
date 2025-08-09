using Core.DTOs.OrderDTOs;
using Core.Entities;
using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechpertsSolutions.Core.DTOs.CustomerDTOs;
using TechpertsSolutions.Core.Entities;

namespace Core.DTOs.DeliveryDTOs
{
    public class DeliveryDTO
    {
                public string Id { get; set; } = null!;
                public string? TrackingNumber { get; set; }
                public string? DeliveryAddress { get; set; }
                public string? CustomerPhone { get; set; }
                public string? CustomerName { get; set; }
                public DateTime? EstimatedDeliveryDate { get; set; }
                public DateTime? ActualDeliveryDate { get; set; }

                public int RetryCount { get; set; }
                public DateTime CreatedAt { get; set; }

                public DeliveryStatus DeliveryStatus { get; set; }
                public string? Notes { get; set; }
                public decimal? DeliveryFee { get; set; }

                public string? PickupAddress { get; set; }
                public DateTime? PickupDate { get; set; }

                public string? DeliveryPersonId { get; set; }
                public string? CustomerId { get; set; }
                public string OrderId { get; set; } = null!;

                public double? DeliveryLatitude { get; set; }
                public double? DeliveryLongitude { get; set; }
                public double? Latitude { get; set; }
                public double? Longitude { get; set; }

                public bool IsOnline { get; set; }

                public OrderCreateDTO? Order { get; set; }
                public DeliveryPersonDTO? DeliveryPerson { get; set; }
                public CustomerDTO? Customer { get; set; }
                public List<DeliveryOffer> Offers { get; set; } = new List<DeliveryOffer>();
                public List<TechCompany> TechCompanies { get; set; } = new List<TechCompany>();

    }
}
