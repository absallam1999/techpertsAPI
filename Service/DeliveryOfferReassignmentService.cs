using Core.DTOs.DeliveryDTOs;
using Core.Entities;
using Core.Enums;
using Core.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class DeliveryReassignmentService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<DeliveryReassignmentService> _logger;
        private const int MaxRetries = 3;

        public DeliveryReassignmentService(IServiceScopeFactory scopeFactory, ILogger<DeliveryReassignmentService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();

                    var deliveryService = scope.ServiceProvider.GetRequiredService<IDeliveryService>();
                    var deliveryPersonService = scope.ServiceProvider.GetRequiredService<IDeliveryPersonService>();
                    var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

                    // Get deliveries that are still pending assignment
                    var pendingDeliveries = await deliveryService.GetByStatusAsync(DeliveryStatus.Pending.ToString());

                    if (pendingDeliveries.Success && pendingDeliveries.Data != null)
                    {
                        foreach (var delivery in pendingDeliveries.Data)
                        {
                            // Get the active offer for this delivery
                            var activeOffer = delivery.Offers.FirstOrDefault(o => o.IsActive);

                            // Check if there is an expired offer
                            if (activeOffer != null && activeOffer.ExpiryTime.HasValue &&
                                activeOffer.ExpiryTime <= DateTime.UtcNow)
                            {
                                _logger.LogInformation($"Offer expired for delivery {delivery.Id}");

                                // Mark current offer as inactive
                                activeOffer.IsActive = false;

                                if (delivery.RetryCount >= MaxRetries)
                                {
                                    await notificationService.SendNotificationToRoleAsync(
                                        "Admin",
                                        $"Delivery #{delivery.TrackingNumber} could not be assigned after {MaxRetries} attempts.",
                                        NotificationType.SystemAlert,
                                        delivery.Id,
                                        "Delivery"
                                    );
                                    continue;
                                }

                                // Find next available driver
                                var availableDrivers = await deliveryPersonService.GetAvailableDeliveryPersonsAsync();
                                if (!availableDrivers.Success || availableDrivers.Data == null || !availableDrivers.Data.Any())
                                {
                                    await notificationService.SendNotificationToRoleAsync(
                                        "Admin",
                                        $"No available drivers for delivery #{delivery.TrackingNumber}.",
                                        NotificationType.SystemAlert,
                                        delivery.Id,
                                        "Delivery"
                                    );
                                    continue;
                                }

                                // Choose next driver (nearest or first)
                                var nextDriver = availableDrivers.Data.FirstOrDefault();

                                // Assign delivery
                                await deliveryService.AssignDeliveryToPersonAsync(delivery.Id, nextDriver.Id);

                                // Increment retry count
                                delivery.RetryCount++;

                                // Notify driver
                                await notificationService.NotifyDeliveryPersonAsync(
                                    nextDriver.Id,
                                    $"A delivery #{delivery.TrackingNumber} is available for pickup."
                                );
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in delivery reassignment service");
                }

                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }
    }
}
