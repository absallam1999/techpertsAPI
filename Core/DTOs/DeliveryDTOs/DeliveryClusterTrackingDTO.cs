using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.DeliveryDTOs
{
    public class DeliveryClusterTrackingDTO
    {
        public string ClusterId { get; set; }
        public string DriverName { get; set; }
        public string Location { get; set; }
        public DateTime LastUpdated { get; set; }
        public DeliveryClusterStatus Status { get; set; }
    }
}