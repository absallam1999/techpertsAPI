using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.MaintenanceDTOs
{
    public class MaintenanceDecisionDTO
    {
        public string MaintenanceId { get; set; }
        public bool IsAccepted { get; set; }
        public string? RejectionReason { get; set; }
    }
}
