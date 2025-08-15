using Core.DTOs;
using Core.DTOs.DeliveryDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechpertsSolutions.Core.Entities;

namespace Core.Interfaces.Services
{
    public interface IDeliveryService
    {
        Task<GeneralResponse<DeliveryReadDTO>> CreateAsync(DeliveryCreateDTO dto);
        Task<GeneralResponse<bool>> AssignDriverToClusterAsync(string clusterId, string driverId);
        Task<GeneralResponse<bool>> AcceptDeliveryAsync(string clusterId, string driverId);
        Task<GeneralResponse<bool>> DeclineDeliveryAsync(string clusterId, string driverId);
        Task<GeneralResponse<bool>> CancelDeliveryAsync(string deliveryId);
        Task<GeneralResponse<bool>> CompleteDeliveryAsync(string deliveryId, string driverId);
        Task<GeneralResponse<DeliveryReadDTO>> GetByIdAsync(string id);
        Task<GeneralResponse<IEnumerable<DeliveryReadDTO>>> GetAllAsync();
        Task<GeneralResponse<bool>> DeleteAsync(string id);
        Task<GeneralResponse<DeliveryTrackingDTO>> GetDeliveryTrackingAsync(string deliveryId);
        Task<IEnumerable<Delivery>> GetDeliveriesWithExpiredOffersAsync();
    }
}
