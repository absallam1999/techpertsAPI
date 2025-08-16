using Core.DTOs.DeliveryPersonDTOs;
using Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IDeliveryPersonService
    {
        Task<GeneralResponse<DeliveryPersonReadDTO>> GetByIdAsync(string id);
        Task<GeneralResponse<IEnumerable<DeliveryPersonReadDTO>>> GetAllAsync();
        Task<GeneralResponse<DeliveryPersonReadDTO>> UpdateAsync(string id, DeliveryPersonUpdateDTO dto);
        Task<GeneralResponse<IEnumerable<DeliveryPersonReadDTO>>> GetAvailableDeliveryPersonsAsync();
        Task<GeneralResponse<bool>> AcceptOfferAsync(string offerId, string driverId);
        Task<GeneralResponse<bool>> DeclineOfferAsync(string offerId, string driverId);
        Task<GeneralResponse<bool>> CancelOfferAsync(string offerId, string driverId);
    }
} 
