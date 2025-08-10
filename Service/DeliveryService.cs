using Core.DTOs;
using Core.DTOs.DeliveryDTOs;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Service.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechpertsSolutions.Core.Entities;

namespace Service
{
    public class DeliveryService : IDeliveryService
    {
        private readonly IRepository<Delivery> _deliveryrepo;
        private readonly IRepository<DeliveryPerson> _deliveryPersonRepo;
        private readonly IRepository<DeliveryOffer> _deliveryOfferRepo;
        private readonly INotificationService _notificationService;

        public DeliveryService(IRepository<Delivery> deliveryRepo, IRepository<DeliveryPerson> deliveryPersonRepo, IRepository<DeliveryOffer> deliveryOfferRepo, INotificationService notificationService)
        {
            _deliveryrepo = deliveryRepo;
            _deliveryOfferRepo = deliveryOfferRepo;
            _deliveryPersonRepo = deliveryPersonRepo;
            _notificationService = notificationService;
        }

        public async Task<GeneralResponse<IEnumerable<DeliveryDTO>>> GetAllAsync()
        {
            try
            {
                // Optimized includes for delivery listing with essential related data
                var deliveries = await _deliveryrepo.GetAllWithIncludesAsync(
                    d => d.Customer,
                    d => d.Customer.User,
                    d => d.DeliveryPerson,
                    d => d.DeliveryPerson.User);

                var deliveryDtos = deliveries.Select(DeliveryMapper.MapToDeliveryDTO).ToList();

                return new GeneralResponse<IEnumerable<DeliveryDTO>>
                {
                    Success = true,
                    Message = "Deliveries retrieved successfully.",
                    Data = deliveryDtos
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<IEnumerable<DeliveryDTO>>
                {
                    Success = false,
                    Message = "An unexpected error occurred while retrieving deliveries.",
                    Data = null
                };
            }
        }

        public async Task<GeneralResponse<DeliveryDTO>> GetByIdAsync(string id)
        {
            
            if (string.IsNullOrWhiteSpace(id))
            {
                return new GeneralResponse<DeliveryDTO>
                {
                    Success = false,
                    Message = "Delivery ID cannot be null or empty.",
                    Data = null
                };
            }

            if (!Guid.TryParse(id, out _))
            {
                return new GeneralResponse<DeliveryDTO>
                {
                    Success = false,
                    Message = "Invalid Delivery ID format. Expected GUID format.",
                    Data = null
                };
            }

            try
            {
                // Comprehensive includes for detailed delivery view
                var delivery = await _deliveryrepo.GetByIdWithIncludesAsync(id,
                    d => d.Customer,
                    d => d.Customer.User,
                    d => d.DeliveryPerson,
                    d => d.DeliveryPerson.User);

                if (delivery == null)
                {
                    return new GeneralResponse<DeliveryDTO>
                    {
                        Success = false,
                        Message = $"Delivery with ID '{id}' not found.",
                        Data = null
                    };
                }

                return new GeneralResponse<DeliveryDTO>
                {
                    Success = true,
                    Message = "Delivery retrieved successfully.",
                    Data = DeliveryMapper.MapToDeliveryDTO(delivery)
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<DeliveryDTO>
                {
                    Success = false,
                    Message = "An unexpected error occurred while retrieving the delivery.",
                    Data = null
                };
            }
        }

        public async Task<GeneralResponse<DeliveryDTO>> AddAsync(DeliveryCreateDTO dto)
        {
            try
            {
                var entity = DeliveryMapper.MapToDelivery(dto);

                await _deliveryrepo.AddAsync(entity);
                await _deliveryrepo.SaveChangesAsync();

                return new GeneralResponse<DeliveryDTO>
                {
                    Success = true,
                    Message = "Delivery created successfully.",
                    Data = DeliveryMapper.MapToDeliveryDTO(entity)
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<DeliveryDTO>
                {
                    Success = false,
                    Message = "An unexpected error occurred while creating the delivery.",
                    Data = null
                };
            }
        }

        public async Task<GeneralResponse<DeliveryDTO>> UpdateAsync(string id, DeliveryUpdateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return new GeneralResponse<DeliveryDTO>
                {
                    Success = false,
                    Message = "Delivery ID cannot be null or empty.",
                    Data = null
                };
            }

            try
            {
                var entity = await _deliveryrepo.GetByIdAsync(id);
                if (entity == null)
                {
                    return new GeneralResponse<DeliveryDTO>
                    {
                        Success = false,
                        Message = $"Delivery with ID '{id}' not found.",
                        Data = null
                    };
                }

                DeliveryMapper.UpdateDelivery(entity, dto);
                _deliveryrepo.Update(entity);
                await _deliveryrepo.SaveChangesAsync();

                return new GeneralResponse<DeliveryDTO>
                {
                    Success = true,
                    Message = "Delivery updated successfully.",
                    Data = DeliveryMapper.MapToDeliveryDTO(entity)
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<DeliveryDTO>
                {
                    Success = false,
                    Message = "An unexpected error occurred while updating the delivery.",
                    Data = null
                };
            }
        }

        public async Task<GeneralResponse<bool>> DeleteAsync(string id)
        {
            
            if (string.IsNullOrWhiteSpace(id))
            {
                return new GeneralResponse<bool>
                {
                    Success = false,
                    Message = "Delivery ID cannot be null or empty.",
                    Data = false
                };
            }

            if (!Guid.TryParse(id, out _))
            {
                return new GeneralResponse<bool>
                {
                    Success = false,
                    Message = "Invalid Delivery ID format. Expected GUID format.",
                    Data = false
                };
            }

            try
            {
                var entity = await _deliveryrepo.GetByIdAsync(id);
                if (entity == null)
                {
                    return new GeneralResponse<bool>
                    {
                        Success = false,
                        Message = $"Delivery with ID '{id}' not found.",
                        Data = false
                    };
                }

                _deliveryrepo.Remove(entity);
                await _deliveryrepo.SaveChangesAsync();
                
                return new GeneralResponse<bool>
                {
                    Success = true,
                    Message = "Delivery deleted successfully.",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<bool>
                {
                    Success = false,
                    Message = "An unexpected error occurred while deleting the delivery.",
                    Data = false
                };
            }
        }

        public async Task<GeneralResponse<DeliveryDetailsDTO>> GetDetailsByIdAsync(string id)
        {
            
            if (string.IsNullOrWhiteSpace(id))
            {
                return new GeneralResponse<DeliveryDetailsDTO>
                {
                    Success = false,
                    Message = "Delivery ID cannot be null or empty.",
                    Data = null
                };
            }

            if (!Guid.TryParse(id, out _))
            {
                return new GeneralResponse<DeliveryDetailsDTO>
                {
                    Success = false,
                    Message = "Invalid Delivery ID format. Expected GUID format.",
                    Data = null
                };
            }

            try
            {
                var delivery = await _deliveryrepo.GetByIdWithIncludesAsync(id,
                    d => d.DeliveryPerson,
                    d => d.Customer,
                    d => d.TechCompanies
                );

                if (delivery == null)
                {
                    return new GeneralResponse<DeliveryDetailsDTO>
                    {
                        Success = false,
                        Message = $"Delivery with ID '{id}' not found.",
                        Data = null
                    };
                }

                return new GeneralResponse<DeliveryDetailsDTO>
                {
                    Success = true,
                    Message = "Delivery details retrieved successfully.",
                    Data = DeliveryMapper.MapToDeliveryDetailsDTO(delivery)
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<DeliveryDetailsDTO>
                {
                    Success = false,
                    Message = "An unexpected error occurred while retrieving delivery details.",
                    Data = null
                };
            }
        }

        public async Task<GeneralResponse<IEnumerable<DeliveryDTO>>> GetByDeliveryPersonIdAsync(string deliveryPersonId)
        {
            if (string.IsNullOrWhiteSpace(deliveryPersonId))
            {
                return new GeneralResponse<IEnumerable<DeliveryDTO>>
                {
                    Success = false,
                    Message = "Delivery Person ID cannot be null or empty.",
                    Data = null
                };
            }

            try
            {
                var deliveries = await _deliveryrepo.FindWithIncludesAsync(
                    d => d.DeliveryPersonId == deliveryPersonId,
                    d => d.DeliveryPerson,
                    d => d.Customer
                );

                return new GeneralResponse<IEnumerable<DeliveryDTO>>
                {
                    Success = true,
                    Message = "Deliveries for delivery person retrieved successfully.",
                    Data = DeliveryMapper.MapToDeliveryDTOList(deliveries)
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<IEnumerable<DeliveryDTO>>
                {
                    Success = false,
                    Message = "An unexpected error occurred while retrieving deliveries for delivery person.",
                    Data = null
                };
            }
        }

        public async Task<GeneralResponse<IEnumerable<DeliveryDTO>>> GetByStatusAsync(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
            {
                return new GeneralResponse<IEnumerable<DeliveryDTO>>
                {
                    Success = false,
                    Message = "Status cannot be null or empty.",
                    Data = null
                };
            }

            try
            {
                // Parse the string status to enum
                if (Enum.TryParse<DeliveryStatus>(status, out var deliveryStatus))
                {
                    var deliveries = await _deliveryrepo.FindWithIncludesAsync(
                        d => d.Status == deliveryStatus,
                        d => d.DeliveryPerson,
                        d => d.Customer
                    );

                    return new GeneralResponse<IEnumerable<DeliveryDTO>>
                    {
                        Success = true,
                        Message = $"Deliveries with status '{status}' retrieved successfully.",
                        Data = DeliveryMapper.MapToDeliveryDTOList(deliveries)
                    };
                }
                else
                {
                    return new GeneralResponse<IEnumerable<DeliveryDTO>>
                    {
                        Success = false,
                        Message = $"Invalid status: {status}",
                        Data = null
                    };
                }
            }
            catch (Exception ex)
            {
                return new GeneralResponse<IEnumerable<DeliveryDTO>>
                {
                    Success = false,
                    Message = "An unexpected error occurred while retrieving deliveries by status.",
                    Data = null
                };
            }
        }

        public async Task<GeneralResponse<DeliveryDTO>> AssignDeliveryToPersonAsync(string deliveryId, string deliveryPersonId)
        {
            if (string.IsNullOrWhiteSpace(deliveryId) || string.IsNullOrWhiteSpace(deliveryPersonId))
            {
                return new GeneralResponse<DeliveryDTO>
                {
                    Success = false,
                    Message = "Delivery ID and Delivery Person ID cannot be null or empty.",
                    Data = null
                };
            }

            try
            {
                var delivery = await _deliveryrepo.GetByIdAsync(deliveryId);
                if (delivery == null)
                {
                    return new GeneralResponse<DeliveryDTO>
                    {
                        Success = false,
                        Message = "Delivery not found.",
                        Data = null
                    };
                }

                if (!string.IsNullOrEmpty(delivery.DeliveryPersonId) && delivery.DeliveryPersonId != deliveryPersonId)
                {
                    return new GeneralResponse<DeliveryDTO>
                    {
                        Success = false,
                        Message = "This delivery is already assigned to another person.",
                        Data = null
                    };
                }

                if (delivery.Status != DeliveryStatus.Pending)
                {
                    return new GeneralResponse<DeliveryDTO>
                    {
                        Success = false,
                        Message = $"Cannot assign delivery in status '{delivery.Status}'.",
                        Data = null
                    };
                }

                var deliveryPerson = await _deliveryPersonRepo.GetByIdAsync(deliveryPersonId);
                if (deliveryPerson == null || !deliveryPerson.IsAvailable || !delivery.IsOnline)
                {
                    return new GeneralResponse<DeliveryDTO>
                    {
                        Success = false,
                        Message = "Delivery person is not available for assignment.",
                        Data = null
                    };
                }

                delivery.DeliveryPersonId = deliveryPersonId;
                delivery.Status = DeliveryStatus.Assigned;

                _deliveryrepo.Update(delivery);
                await _deliveryrepo.SaveChangesAsync();

                return new GeneralResponse<DeliveryDTO>
                {
                    Success = true,
                    Message = "Delivery assigned to delivery person successfully.",
                    Data = DeliveryMapper.MapToDeliveryDTO(delivery)
                };
            }
            catch (Exception)
            {
                return new GeneralResponse<DeliveryDTO>
                {
                    Success = false,
                    Message = "An unexpected error occurred while assigning delivery.",
                    Data = null
                };
            }
        }

        public async Task<GeneralResponse<DeliveryDTO>> UpdateDeliveryStatusAsync(string deliveryId, string newStatus)
        {
            if (string.IsNullOrWhiteSpace(deliveryId) || string.IsNullOrWhiteSpace(newStatus))
            {
                return new GeneralResponse<DeliveryDTO>
                {
                    Success = false,
                    Message = "Delivery ID and new status cannot be null or empty.",
                    Data = null
                };
            }

            try
            {
                var delivery = await _deliveryrepo.GetByIdAsync(deliveryId);
                if (delivery == null)
                {
                    return new GeneralResponse<DeliveryDTO>
                    {
                        Success = false,
                        Message = "Delivery not found.",
                        Data = null
                    };
                }

                if (Enum.TryParse<DeliveryStatus>(newStatus, out var status))
                {
                    delivery.Status = status;
                }
                else
                {
                    return new GeneralResponse<DeliveryDTO>
                    {
                        Success = false,
                        Message = $"Invalid status: {newStatus}",
                        Data = null
                    };
                }
                _deliveryrepo.Update(delivery);
                await _deliveryrepo.SaveChangesAsync();

                return new GeneralResponse<DeliveryDTO>
                {
                    Success = true,
                    Message = $"Delivery status updated to '{newStatus}' successfully.",
                    Data = DeliveryMapper.MapToDeliveryDTO(delivery)
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<DeliveryDTO>
                {
                    Success = false,
                    Message = "An unexpected error occurred while updating delivery status.",
                    Data = null
                };
            }
        }

        public async Task<GeneralResponse<DeliveryDTO>> CompleteDeliveryAsync(string deliveryId, string deliveryPersonId)
        {
            if (string.IsNullOrWhiteSpace(deliveryId) || string.IsNullOrWhiteSpace(deliveryPersonId))
            {
                return new GeneralResponse<DeliveryDTO>
                {
                    Success = false,
                    Message = "Delivery ID and Delivery Person ID cannot be null or empty.",
                    Data = null
                };
            }

            try
            {
                var delivery = await _deliveryrepo.GetByIdAsync(deliveryId);
                if (delivery == null)
                {
                    return new GeneralResponse<DeliveryDTO>
                    {
                        Success = false,
                        Message = "Delivery not found.",
                        Data = null
                    };
                }

                if (delivery.DeliveryPersonId != deliveryPersonId)
                {
                    return new GeneralResponse<DeliveryDTO>
                    {
                        Success = false,
                        Message = "This delivery is not assigned to you.",
                        Data = null
                    };
                }

                delivery.Status = DeliveryStatus.Delivered;
                delivery.ActualDeliveryDate = DateTime.UtcNow;
                _deliveryrepo.Update(delivery);
                await _deliveryrepo.SaveChangesAsync();

                return new GeneralResponse<DeliveryDTO>
                {
                    Success = true,
                    Message = "Delivery completed successfully.",
                    Data = DeliveryMapper.MapToDeliveryDTO(delivery)
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<DeliveryDTO>
                {
                    Success = false,
                    Message = "An unexpected error occurred while completing delivery.",
                    Data = null
                };
            }
        }

        public async Task<GeneralResponse<IEnumerable<DeliveryDTO>>> GetAvailableDeliveriesAsync()
        {
            try
            {
                var deliveries = await _deliveryrepo.FindWithIncludesAsync(
                d => d.IsOnline && d.DeliveryPersonId != null && d.DeliveryPerson.IsAvailable
                );

                return new GeneralResponse<IEnumerable<DeliveryDTO>>
                {
                    Success = true,
                    Message = "Available deliveries retrieved successfully.",
                    Data = DeliveryMapper.MapToDeliveryDTOList(deliveries)
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<IEnumerable<DeliveryDTO>>
                {
                    Success = false,
                    Message = "An unexpected error occurred while retrieving available deliveries.",
                    Data = null
                };
            }
        }

        //public async Task<GeneralResponse<DeliveryDTO>> AcceptDeliveryAsync(string deliveryId, string deliveryPersonId)
        //{
        //    if (string.IsNullOrWhiteSpace(deliveryId) || string.IsNullOrWhiteSpace(deliveryPersonId))
        //    {
        //        return new GeneralResponse<DeliveryDTO>
        //        {
        //            Success = false,
        //            Message = "Delivery ID and Delivery Person ID cannot be null or empty.",
        //            Data = null
        //        };
        //    }

        //    try
        //    {
        //        var delivery = await _deliveryrepo.GetByIdAsync(deliveryId);
        //        if (delivery == null)
        //        {
        //            return new GeneralResponse<DeliveryDTO>
        //            {
        //                Success = false,
        //                Message = "Delivery not found.",
        //                Data = null
        //            };
        //        }

        //        if (delivery.Status != DeliveryStatus.Assigned || delivery.DeliveryPersonId != null)
        //        {
        //            return new GeneralResponse<DeliveryDTO>
        //            {
        //                Success = false,
        //                Message = "This delivery is not available for acceptance.",
        //                Data = null
        //            };
        //        }

        //        delivery.DeliveryPersonId = deliveryPersonId;
        //        delivery.Status = DeliveryStatus.PickedUp;
        //        _deliveryrepo.Update(delivery);
        //        await _deliveryrepo.SaveChangesAsync();

        //        return new GeneralResponse<DeliveryDTO>
        //        {
        //            Success = true,
        //            Message = "Delivery accepted successfully.",
        //            Data = DeliveryMapper.MapToDeliveryDTO(delivery)
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new GeneralResponse<DeliveryDTO>
        //        {
        //            Success = false,
        //            Message = "An unexpected error occurred while accepting delivery.",
        //            Data = null
        //        };
        //    }
        //}

        //public async Task<GeneralResponse<DeliveryDTO>> AssignDeliveryToNearestAsync(string deliveryId, List<Delivery> availableDrivers)
        //{
        //    // Get delivery entity
        //    var delivery = await _deliveryrepo.GetByIdAsync(deliveryId);
        //    if (delivery == null)
        //    {
        //        return new GeneralResponse<DeliveryDTO>
        //        {
        //            Success = false,
        //            Message = $"Delivery with ID {deliveryId} not found.",
        //            Data = null
        //        };
        //    }

        //    // Ensure delivery has coordinates
        //    if (!delivery.DeliveryLatitude.HasValue || !delivery.DeliveryLongitude.HasValue)
        //    {
        //        return new GeneralResponse<DeliveryDTO>
        //        {
        //            Success = false,
        //            Message = "Delivery coordinates are missing.",
        //            Data = null
        //        };
        //    }

        //    Delivery nearestDriver = null;
        //    double shortestDistance = double.MaxValue;

        //    // Loop through available drivers
        //    foreach (var driver in availableDrivers)
        //    {
        //        if (!driver.Latitude.HasValue || !driver.Longitude.HasValue)
        //            continue;

        //        var distance = CalculateHaversineDistance(
        //            delivery.DeliveryLatitude.Value,
        //            delivery.DeliveryLongitude.Value,
        //            driver.Latitude.Value,
        //            driver.Longitude.Value
        //        );

        //        if (distance < shortestDistance)
        //        {
        //            shortestDistance = distance;
        //            nearestDriver = driver;
        //        }
        //    }

        //    if (nearestDriver == null)
        //    {
        //        return new GeneralResponse<DeliveryDTO>
        //        {
        //            Success = false,
        //            Message = "No available drivers found near the delivery location.",
        //            Data = null
        //        };
        //    }

        //    // Assign the nearest driver
        //    delivery.DeliveryPersonId = nearestDriver.DeliveryPersonId;
        //    _deliveryrepo.Update(delivery);
        //    await _deliveryrepo.SaveChangesAsync();

        //    // Optionally send a notification to the driver
        //    await _notificationService.SendNotificationAsync(
        //        nearestDriver.DeliveryPerson.UserId,
        //        $"You have been assigned a new delivery: {delivery.TrackingNumber}",
        //        NotificationType.DeliveryAssigned,
        //        delivery.Id,
        //        "Delivery"
        //    );

        //    return new GeneralResponse<DeliveryDTO>
        //    {
        //        Success = true,
        //        Message = $"Delivery assigned to nearest driver {nearestDriver.DeliveryPersonId} ({shortestDistance:F2} km away).",
        //        Data = DeliveryMapper.MapToDeliveryDTO(delivery)
        //    };
        //}


        public async Task<GeneralResponse<DeliveryDTO>> AcceptDeliveryAsync(string deliveryId, string deliveryPersonId)
        {
            if (string.IsNullOrWhiteSpace(deliveryId) || string.IsNullOrWhiteSpace(deliveryPersonId))
                return new GeneralResponse<DeliveryDTO> { Success = false, Message = "Delivery ID and Delivery Person ID are required.", Data = null };

            try
            {
                // Load delivery and its offers (assumes you have a GetByIdWithIncludesAsync that can include Offers)
                var delivery = await _deliveryrepo.GetByIdWithIncludesAsync(
                    deliveryId,
                    d => d.Offers,            // include offers to update statuses
                    d => d.DeliveryPerson     // include current assigned person if any
                );

                if (delivery == null)
                    return new GeneralResponse<DeliveryDTO> { Success = false, Message = "Delivery not found.", Data = null };

                // Ensure there is a pending offer for this delivery/person
                var offer = await _deliveryOfferRepo.GetFirstOrDefaultAsync(
                    o => o.DeliveryId == deliveryId && o.DeliveryPersonId == deliveryPersonId && o.Status == DeliveryOfferStatus.Pending);

                if (offer == null)
                    return new GeneralResponse<DeliveryDTO> { Success = false, Message = "No active offer found for this delivery and delivery person.", Data = null };

                // Ensure delivery is still unassigned (first-come wins)
                if (!string.IsNullOrWhiteSpace(delivery.DeliveryPersonId))
                {
                    return new GeneralResponse<DeliveryDTO> { Success = false, Message = "Delivery has already been accepted by another driver.", Data = null };
                }

                // Mark the offer accepted
                offer.Status = DeliveryOfferStatus.Accepted;
                offer.RespondedAt = DateTime.UtcNow;
                _deliveryOfferRepo.Update(offer);

                // Mark all other pending offers for this delivery as Declined/Expired
                var otherOffers = await _deliveryOfferRepo.FindWithIncludesAsync(
                    o => o.DeliveryId == deliveryId && o.Id != offer.Id && o.Status == DeliveryOfferStatus.Pending);

                foreach (var o in otherOffers)
                {
                    o.Status = DeliveryOfferStatus.Declined;
                    o.RespondedAt = DateTime.UtcNow;
                    _deliveryOfferRepo.Update(o);
                }

                await _deliveryOfferRepo.SaveChangesAsync();

                // Assign delivery to this delivery person
                delivery.DeliveryPersonId = deliveryPersonId;
                delivery.Status = DeliveryStatus.Assigned; // accepted => assigned; driver later sets PickedUp
                _deliveryrepo.Update(delivery);
                await _deliveryrepo.SaveChangesAsync();

                // Notifications: admin + customer + driver confirmation
                await _notificationService.SendNotificationToRoleAsync(
                    "Admin",
                    $"Delivery #{delivery.TrackingNumber ?? delivery.Id} accepted by driver {deliveryPersonId}.",
                    NotificationType.DeliveryOfferAccepted,
                    delivery.Id,
                    "Delivery");

                // notify customer (if you have customer's user id)
                if (!string.IsNullOrWhiteSpace(delivery.CustomerId))
                {
                    await _notificationService.SendNotificationAsync(
                        delivery.CustomerId,
                        $"Your delivery #{delivery.TrackingNumber ?? delivery.Id} was accepted and is being prepared for pickup.",
                        NotificationType.DeliveryAssigned,
                        delivery.Id,
                        "Delivery");
                }

                return new GeneralResponse<DeliveryDTO>
                {
                    Success = true,
                    Message = "Delivery accepted successfully.",
                    Data = DeliveryMapper.MapToDeliveryDTO(delivery)
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<DeliveryDTO>
                {
                    Success = false,
                    Message = $"An unexpected error occurred while accepting delivery: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<GeneralResponse<DeliveryDTO>> AssignDeliveryToNearestAsync(
            string deliveryId,
            List<Delivery> availableDrivers,
            int takeNearest = 3,                // configurable number of initial offers
            int offerExpiryMinutes = 3)         // configurable expiry for offers
        {
            if (string.IsNullOrWhiteSpace(deliveryId))
                return new GeneralResponse<DeliveryDTO> { Success = false, Message = "Delivery ID is required.", Data = null };

            if (availableDrivers == null || !availableDrivers.Any())
                return new GeneralResponse<DeliveryDTO> { Success = false, Message = "No available drivers provided.", Data = null };

            try
            {
                var delivery = await _deliveryrepo.GetByIdAsync(deliveryId);
                if (delivery == null)
                    return new GeneralResponse<DeliveryDTO> { Success = false, Message = "Delivery not found.", Data = null };

                if (!delivery.DeliveryLatitude.HasValue || !delivery.DeliveryLongitude.HasValue)
                    return new GeneralResponse<DeliveryDTO> { Success = false, Message = "Delivery coordinates are missing.", Data = null };

                // filter valid drivers: have coordinates, online and have DeliveryPerson info
                var validDrivers = availableDrivers
                    .Where(d => d.Latitude.HasValue && d.Longitude.HasValue && d.IsOnline && d.DeliveryPerson != null)
                    .Select(d => new
                    {
                        DriverDelivery = d,
                        Distance = CalculateHaversineDistance(
                            delivery.DeliveryLatitude.Value,
                            delivery.DeliveryLongitude.Value,
                            d.Latitude.Value,
                            d.Longitude.Value)
                    })
                    .OrderBy(x => x.Distance)
                    .Take(takeNearest)
                    .ToList();

                if (!validDrivers.Any())
                    return new GeneralResponse<DeliveryDTO> { Success = false, Message = "No nearby online drivers found.", Data = null };

                var now = DateTime.UtcNow;
                var expiry = now.AddMinutes(offerExpiryMinutes);
                var createdOffers = new List<DeliveryOffer>();

                foreach (var entry in validDrivers)
                {
                    var driver = entry.DriverDelivery;

                    // avoid duplicate offers for same delivery+person (check existing pending offers)
                    var existingOffer = await _deliveryOfferRepo.GetFirstOrDefaultAsync(
                        o => o.DeliveryId == delivery.Id && o.DeliveryPersonId == driver.DeliveryPerson.Id && o.Status == DeliveryOfferStatus.Pending);

                    if (existingOffer != null)
                        continue;

                    var offer = new DeliveryOffer
                    {
                        Id = Guid.NewGuid().ToString(),
                        DeliveryId = delivery.Id,
                        DeliveryPersonId = driver.DeliveryPerson.Id,
                        Status = DeliveryOfferStatus.Pending,
                        CreatedAt = now,
                        ExpiryTime = expiry
                    };

                    await _deliveryOfferRepo.AddAsync(offer);
                    createdOffers.Add(offer);

                    // send persistent + realtime notification (SendNotificationAsync stores + SignalR)
                    var notifyUserId = driver.DeliveryPerson.UserId;
                    if (!string.IsNullOrWhiteSpace(notifyUserId))
                    {
                        await _notificationService.SendNotificationAsync(
                            notifyUserId,
                            $"New delivery offer: #{delivery.TrackingNumber ?? delivery.Id} — {entry.Distance:F2} km from you",
                            NotificationType.DeliveryOfferCreated,
                            delivery.Id,
                            "Delivery");
                    }
                }

                if (createdOffers.Any())
                    await _deliveryOfferRepo.SaveChangesAsync();

                return new GeneralResponse<DeliveryDTO>
                {
                    Success = true,
                    Message = $"Delivery offers created and notifications sent to {createdOffers.Count} drivers.",
                    Data = DeliveryMapper.MapToDeliveryDTO(delivery)
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<DeliveryDTO>
                {
                    Success = false,
                    Message = $"An unexpected error occurred while creating offers: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<Delivery> CreateDeliveryForOrderAsync(Order order, double? latitude, double? longitude, string customerId)
        {
            var delivery = new Delivery
            {
                TrackingNumber = Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper(),
                CustomerId = customerId,
                OrderId = order.Id,
                DeliveryLatitude = latitude,
                DeliveryLongitude = longitude,
                Status = DeliveryStatus.Pending,
                RetryCount = 0,
                CreatedAt = DateTime.UtcNow
            };

            await _deliveryrepo.AddAsync(delivery);
            await _deliveryrepo.SaveChangesAsync();


            if (delivery.DeliveryLatitude.HasValue && delivery.DeliveryLongitude.HasValue)
            {
                var availableDriversResponse = await GetAvailableDeliveriesAsync();

                if (availableDriversResponse.Success && availableDriversResponse.Data != null)
                {
                    var availableDrivers = availableDriversResponse.Data
                        .Where(d => d.Latitude.HasValue && d.Longitude.HasValue && d.IsOnline)
                        .Select(d => new Delivery
                        {
                            Id = d.Id,
                            Latitude = d.Latitude,
                            Longitude = d.Longitude,
                            IsOnline = d.IsOnline,
                            DeliveryPerson = new DeliveryPerson
                            {
                                Id = d.DeliveryPersonId,
                                UserId = d.DeliveryPerson.User.Id
                            }
                        })
                        .ToList();

                    await AssignDeliveryToNearestAsync(delivery.Id, availableDrivers, 3, 3);
                }
            }

            await _notificationService.SendNotificationToRoleAsync(
                "Admin",
                $"New order #{order.Id} has been created by customer {order.CustomerId}",
                NotificationType.OrderCreated,
                order.Id,
                "Order"
            );

            await _notificationService.SendNotificationToRoleAsync(
                "Delivery",
                $"New delivery #{delivery.TrackingNumber} is available for assignment.",
                NotificationType.DeliveryAssigned,
                delivery.Id,
                "Delivery"
            );

            await _notificationService.SendNotificationToRoleAsync(
                "TechCompany",
                $"New order #{order.Id} has been created by customer {order.CustomerId}",
                NotificationType.OrderCreated,
                order.Id,
                "Order"
            );

            return delivery;
        }

        private double CalculateHaversineDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Earth's radius in km
            var dLat = DegreesToRadians(lat2 - lat1);
            var dLon = DegreesToRadians(lon2 - lon1);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        private double DegreesToRadians(double deg) => deg * (Math.PI / 180);

    }
}