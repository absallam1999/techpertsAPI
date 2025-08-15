using Core.DTOs;
using Core.DTOs.DeliveryDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IDeliveryClusterService
    {
        Task<GeneralResponse<DeliveryClusterDTO>> CreateClusterAsync(string deliveryId, DeliveryClusterCreateDTO dto);

        Task<GeneralResponse<DeliveryClusterDTO>> GetByIdAsync(string clusterId);

        Task<GeneralResponse<IEnumerable<DeliveryClusterDTO>>> GetByDeliveryIdAsync(string deliveryId);

        Task<GeneralResponse<DeliveryClusterDTO>> UpdateClusterAsync(string clusterId, DeliveryClusterDTO dto);

        Task<GeneralResponse<DeliveryClusterDTO>> UpdateClusterTrackingAsync(string clusterId, DeliveryClusterTrackingDTO trackingDto);

        Task<GeneralResponse<bool>> DeleteClusterAsync(string clusterId);

        Task<GeneralResponse<DeliveryClusterTrackingDTO>> GetTrackingAsync(string clusterId);

        Task<GeneralResponse<DeliveryClusterDTO>> AssignDriverAsync(string clusterId, string driverId);
        Task<GeneralResponse<int>> BulkAssignDriverAsync(IEnumerable<string> clusterIds, string driverId);
        Task<GeneralResponse<IEnumerable<DeliveryClusterDTO>>> GetUnassignedClustersAsync();
    }
}
