using Core.DTOs.DeliveryDTOs;
using Core.DTOs.DeliveryPersonDTOs;
using Core.Enums;
using TechpertsSolutions.Core.Entities;

namespace Service.Utilities
{
    public static class DeliveryMapper
    {
        public static DeliveryDTO MapToDeliveryDTO(Delivery delivery)
        {
            if (delivery == null) return null;

            return new DeliveryDTO
            {
                Id = delivery.Id,
                TrackingNumber = delivery.TrackingNumber,
                DeliveryAddress = delivery.DeliveryAddress,
                CustomerPhone = delivery.CustomerPhone,
                CustomerName = delivery.CustomerName,
                EstimatedDeliveryDate = delivery.EstimatedDeliveryDate,
                ActualDeliveryDate = delivery.ActualDeliveryDate,
                DeliveryStatus = delivery.Status,
                Notes = delivery.Notes,
                DeliveryFee = delivery.DeliveryFee,
                DeliveryPersonId = delivery.DeliveryPersonId,
                CustomerId = delivery.CustomerId
            };
        }

        public static Delivery MapToDelivery(DeliveryCreateDTO dto)
        {
            if (dto == null) return null;

            return new Delivery
            {
                Id = Guid.NewGuid().ToString(),
                TrackingNumber = dto.TrackingNumber,
                DeliveryAddress = dto.DeliveryAddress,
                CustomerPhone = dto.CustomerPhone,
                CustomerName = dto.CustomerName,
                EstimatedDeliveryDate = dto.EstimatedDeliveryDate,
                Status = DeliveryStatus.Assigned,
                Notes = dto.Notes,
                DeliveryFee = dto.DeliveryFee,
                DeliveryPersonId = dto.DeliveryPersonId,
                CustomerId = dto.CustomerId
            };
        }

        public static void UpdateDelivery(Delivery delivery, DeliveryUpdateDTO dto)
        {
            if (delivery == null || dto == null) return;

            if (!string.IsNullOrWhiteSpace(dto.TrackingNumber))
                delivery.TrackingNumber = dto.TrackingNumber;

            if (!string.IsNullOrWhiteSpace(dto.DeliveryAddress))
                delivery.DeliveryAddress = dto.DeliveryAddress;

            if (!string.IsNullOrWhiteSpace(dto.CustomerPhone))
                delivery.CustomerPhone = dto.CustomerPhone;

            if (!string.IsNullOrWhiteSpace(dto.CustomerName))
                delivery.CustomerName = dto.CustomerName;

            if (dto.EstimatedDeliveryDate.HasValue)
                delivery.EstimatedDeliveryDate = dto.EstimatedDeliveryDate;

            if (dto.ActualDeliveryDate.HasValue)
                delivery.ActualDeliveryDate = dto.ActualDeliveryDate;

            if (!string.IsNullOrWhiteSpace(dto.DeliveryStatus))
            {
                if (Enum.TryParse<DeliveryStatus>(dto.DeliveryStatus, out var status))
                {
                    delivery.Status = status;
                }
            }

            if (!string.IsNullOrWhiteSpace(dto.Notes))
                delivery.Notes = dto.Notes;

            if (dto.DeliveryFee.HasValue)
                delivery.DeliveryFee = dto.DeliveryFee;

            if (!string.IsNullOrWhiteSpace(dto.DeliveryPersonId))
                delivery.DeliveryPersonId = dto.DeliveryPersonId;
        }

        public static DeliveryDetailsDTO MapToDeliveryDetailsDTO(Delivery delivery)
        {
            if (delivery == null) return null;

            return new DeliveryDetailsDTO
            {
                Id = delivery.Id,
                TrackingNumber = delivery.TrackingNumber,
                DeliveryAddress = delivery.DeliveryAddress,
                CustomerPhone = delivery.CustomerPhone,
                CustomerName = delivery.CustomerName,
                EstimatedDeliveryDate = delivery.EstimatedDeliveryDate,
                ActualDeliveryDate = delivery.ActualDeliveryDate,
                DeliveryStatus = delivery.Status.ToString(),
                Notes = delivery.Notes,
                DeliveryFee = delivery.DeliveryFee,
                DeliveryPerson = delivery.DeliveryPerson != null ? new DeliveryPersonDTO
                {
                    Id = delivery.DeliveryPerson.Id,
                    UserFullName = delivery.DeliveryPerson.User?.FullName,
                    VehicleNumber = delivery.DeliveryPerson.VehicleNumber,
                    VehicleType = delivery.DeliveryPerson.VehicleType,
                    PhoneNumber = delivery.DeliveryPerson.User.PhoneNumber,
                    City = delivery.DeliveryPerson.User.City,
                    Country = delivery.DeliveryPerson.User.Country,
                    IsAvailable = delivery.DeliveryPerson.IsAvailable
                } : null,
                TechCompanies = delivery.TechCompanies?.Select(tc => new DeliveryTechCompanyDTO
                {
                    Id = tc.Id,
                    City = tc.User.City,
                    Country = tc.User.Country,
                    UserFullName = tc.User?.FullName
                }).ToList()
            };
        }

        public static IEnumerable<DeliveryDTO> MapToDeliveryDTOList(IEnumerable<Delivery> deliveries)
        {
            if (deliveries == null) return new List<DeliveryDTO>();
            return deliveries.Select(MapToDeliveryDTO).Where(dto => dto != null);
        }
    }
} 
