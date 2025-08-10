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
        Task<GeneralResponse<IEnumerable<DeliveryDTO>>> GetAllAsync();
        Task<GeneralResponse<DeliveryDTO>> GetByIdAsync(string id);
        Task<GeneralResponse<DeliveryDetailsDTO>> GetDetailsByIdAsync(string id); 
        Task<GeneralResponse<DeliveryDTO>> AddAsync(DeliveryCreateDTO dto);                              
        Task<GeneralResponse<bool>> DeleteAsync(string id);
        Task<GeneralResponse<DeliveryDTO>> UpdateAsync(string id, DeliveryUpdateDTO dto);
        Task<GeneralResponse<IEnumerable<DeliveryDTO>>> GetByDeliveryPersonIdAsync(string deliveryPersonId);
        Task<GeneralResponse<IEnumerable<DeliveryDTO>>> GetByStatusAsync(string status);
        Task<GeneralResponse<DeliveryDTO>> AssignDeliveryToPersonAsync(string deliveryId, string deliveryPersonId);
        Task<GeneralResponse<DeliveryDTO>> UpdateDeliveryStatusAsync(string deliveryId, string newStatus);
        Task<GeneralResponse<DeliveryDTO>> CompleteDeliveryAsync(string deliveryId, string deliveryPersonId);
        Task<GeneralResponse<IEnumerable<DeliveryDTO>>> GetAvailableDeliveriesAsync();
        Task<GeneralResponse<DeliveryDTO>> AcceptDeliveryAsync(string deliveryId, string deliveryPersonId);
        Task<GeneralResponse<DeliveryDTO>> AssignDeliveryToNearestAsync(string deliveryId, List<Delivery> availableDeliveryPersons, int takeNearest , int offerExpiryMinutes);
        Task<Delivery> CreateDeliveryForOrderAsync(Order order, double? latitude, double? longitude, string customerId);
    }
}
