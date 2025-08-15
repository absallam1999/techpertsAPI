using Core.DTOs;
using Core.DTOs.DeliveryDTOs;
using Core.DTOs.OrderDTOs;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Core.Interfaces.Services;
using Core.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Service.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechpertsSolutions.Core.Entities;
using TechpertsSolutions.Repository.Data;

namespace Service
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> _orderRepo;
        private readonly IRepository<Delivery> _deliveryRepo;
        private readonly IRepository<DeliveryPerson> _deliveryPersonRepo;
        private readonly IRepository<DeliveryOffer> _deliveryOfferRepo;
        private readonly IRepository<OrderHistory> _orderHistoryRepo;
        private readonly TechpertsContext _dbContext;
        private readonly IDeliveryService _deliveryService;
        private readonly INotificationService _notificationService;
        private readonly ILogger<OrderService> _logger;

        public OrderService(
            IRepository<Order> orderRepo,
            IRepository<OrderHistory> orderHistoryRepo,
            IRepository<Delivery> deliveryRepo,
            IRepository<DeliveryPerson> deliveryPersonRepo,
            IRepository<DeliveryOffer> deliveryOfferRepo,
            INotificationService notificationService,
            IDeliveryService deliveryService,
            ILogger<OrderService> logger,
            TechpertsContext dbContext
            )
        {
            _orderRepo = orderRepo;
            _orderHistoryRepo = orderHistoryRepo;
            _deliveryRepo = deliveryRepo;
            _deliveryPersonRepo = deliveryPersonRepo;
            _deliveryOfferRepo = deliveryOfferRepo;
            _deliveryService = deliveryService;
            _notificationService = notificationService;
            _dbContext = dbContext;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        //public async Task<GeneralResponse<OrderReadDTO>> CreateOrderAsync(OrderCreateDTO dto)
        //{

        //    if (dto == null)
        //    {
        //        return new GeneralResponse<OrderReadDTO>
        //        {
        //            Success = false,
        //            Message = "Order data cannot be null.",
        //            Data = null
        //        };
        //    }

        //    if (string.IsNullOrWhiteSpace(dto.CustomerId))
        //    {
        //        return new GeneralResponse<OrderReadDTO>
        //        {
        //            Success = false,
        //            Message = "Customer ID is required.",
        //            Data = null
        //        };
        //    }

        //    if (!Guid.TryParse(dto.CustomerId, out _))
        //    {
        //        return new GeneralResponse<OrderReadDTO>
        //        {
        //            Success = false,
        //            Message = "Invalid Customer ID format. Expected GUID format.",
        //            Data = null
        //        };
        //    }

        //    if (dto.OrderItems == null || !dto.OrderItems.Any())
        //    {
        //        return new GeneralResponse<OrderReadDTO>
        //        {
        //            Success = false,
        //            Message = "Order must contain at least one item.",
        //            Data = null
        //        };
        //    }

        //    try
        //    {
        //        var order = OrderMapper.ToEntity(dto);

        //        // Calculate total amount
        //        order.TotalAmount = order.OrderItems.Sum(i => i.ItemTotal);

        //        // Get or create order history for this customer
        //        var orderHistory = await GetOrCreateOrderHistoryAsync(dto.CustomerId);
        //        order.OrderHistoryId = orderHistory.Id;

        //        await _orderRepo.AddAsync(order);
        //        await _orderRepo.SaveChangesAsync();

        //        // Send notification to admin about new order
        //        await _notificationService.SendNotificationToRoleAsync(
        //            "Admin",
        //            $"New order #{order.Id} has been created by customer {order.CustomerId}",
        //            NotificationType.OrderCreated,
        //            order.Id,
        //            "Order"
        //        );

        //        // Get the created order with all includes to return proper data
        //        var createdOrder = await _orderRepo.GetFirstOrDefaultAsync(
        //            o => o.Id == order.Id,
        //            includeProperties: "OrderItems,OrderItems.Product,Customer,Customer.User,OrderHistory");

        //        return new GeneralResponse<OrderReadDTO>
        //        {
        //            Success = true,
        //            Message = "Order created successfully.",
        //            Data = OrderMapper.ToReadDTO(createdOrder)
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new GeneralResponse<OrderReadDTO>
        //        {
        //            Success = false,
        //            Message = "An unexpected error occurred while creating the order.",
        //            Data = null
        //        };
        //    }
        //}










        //public async Task<GeneralResponse<OrderReadDTO>> CreateOrderAsync(OrderCreateDTO dto)
        //{
        //    if (dto == null)
        //    {
        //        return new GeneralResponse<OrderReadDTO>
        //        {
        //            Success = false,
        //            Message = "Order data cannot be null.",
        //            Data = null
        //        };
        //    }

        //    if (string.IsNullOrWhiteSpace(dto.CustomerId))
        //    {
        //        return new GeneralResponse<OrderReadDTO>
        //        {
        //            Success = false,
        //            Message = "Customer ID is required.",
        //            Data = null
        //        };
        //    }

        //    if (!Guid.TryParse(dto.CustomerId, out _))
        //    {
        //        return new GeneralResponse<OrderReadDTO>
        //        {
        //            Success = false,
        //            Message = "Invalid Customer ID format. Expected GUID format.",
        //            Data = null
        //        };
        //    }

        //    if (dto.OrderItems == null || !dto.OrderItems.Any())
        //    {
        //        return new GeneralResponse<OrderReadDTO>
        //        {
        //            Success = false,
        //            Message = "Order must contain at least one item.",
        //            Data = null
        //        };
        //    }

        //    //try
        //    //{
        //    //    var order = OrderMapper.ToEntity(dto);
        //    //    order.TotalAmount = order.OrderItems.Sum(i => i.ItemTotal);

        //    //    var orderHistory = await GetOrCreateOrderHistoryAsync(dto.CustomerId);
        //    //    order.OrderHistoryId = orderHistory.Id;

        //    //    await _orderRepo.AddAsync(order);
        //    //    await _orderRepo.SaveChangesAsync();

        //    //    var delivery = new Delivery
        //    //    {
        //    //        TrackingNumber = Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper(),
        //    //        CustomerId = dto.CustomerId,
        //    //        OrderId = order.Id,
        //    //    };
        //    //    await _deliveryRepo.AddAsync(delivery);
        //    //    await _deliveryRepo.SaveChangesAsync();

        //    //    await _notificationService.SendNotificationToRoleAsync(
        //    //        "Admin",
        //    //        $"New order #{order.Id} has been created by customer {order.CustomerId}",
        //    //        NotificationType.OrderCreated,
        //    //        order.Id,
        //    //        "Order"
        //    //    );

        //    //    await _notificationService.SendNotificationToRoleAsync(
        //    //        "Delivery",
        //    //        $"New delivery #{delivery.TrackingNumber} is available for assignment.",
        //    //        NotificationType.DeliveryAssigned,
        //    //        delivery.Id,
        //    //        "Delivery"
        //    //    );

        //    //    var createdOrder = await _orderRepo.GetFirstOrDefaultAsync(
        //    //        o => o.Id == order.Id,
        //    //        includeProperties: "OrderItems,OrderItems.Product,Customer,Customer.User,OrderHistory");

        //    //    return new GeneralResponse<OrderReadDTO>
        //    //    {
        //    //        Success = true,
        //    //        Message = "Order has been created successfully and sent to Delivery.",
        //    //        Data = OrderMapper.ToReadDTO(createdOrder)
        //    //    };
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    return new GeneralResponse<OrderReadDTO>
        //    //    {
        //    //        Success = false,
        //    //        Message = $"An unexpected error occurred while creating the order. {ex}",
        //    //        Data = null
        //    //    };
        //    //}
        //    try
        //    {
        //        Delivery existingDelivery = null;
        //        if (!string.IsNullOrEmpty(dto.DeliveryId))
        //        {
        //            existingDelivery = await _deliveryRepo.GetByIdAsync(dto.DeliveryId);
        //            if (existingDelivery == null)
        //                throw new Exception($"Delivery with id {dto.DeliveryId} not found.");
        //        }

        //        var order = OrderMapper.ToEntity(dto);
        //        order.TotalAmount = order.OrderItems.Sum(i => i.ItemTotal);

        //        var orderHistory = await GetOrCreateOrderHistoryAsync(dto.CustomerId);
        //        order.OrderHistoryId = orderHistory.Id;

        //        await _orderRepo.AddAsync(order);
        //        await _orderRepo.SaveChangesAsync();

        //        Delivery delivery;
        //        if (existingDelivery != null)
        //        {
        //            existingDelivery.OrderId = order.Id;
        //            delivery = existingDelivery;
        //        }
        //        else
        //        {
        //            delivery = new Delivery
        //            {
        //                TrackingNumber = Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper(),
        //                CustomerId = dto.CustomerId,
        //                OrderId = order.Id,
        //                DeliveryLatitude = existingDelivery.Latitude,
        //                DeliveryLongitude = existingDelivery.Longitude
        //            };
        //            await _deliveryRepo.AddAsync(delivery);
        //        }

        //        await _deliveryRepo.SaveChangesAsync();

        //        await _notificationService.SendNotificationToRoleAsync(
        //            "Admin",
        //            $"New order #{order.Id} has been created by customer {order.CustomerId}",
        //            NotificationType.OrderCreated,
        //            order.Id,
        //            "Order"
        //        );

        //        await _notificationService.SendNotificationToRoleAsync(
        //            "Delivery",
        //            $"New delivery #{delivery.TrackingNumber} is available for assignment.",
        //            NotificationType.DeliveryAssigned,
        //            delivery.Id,
        //            "Delivery"
        //        );

        //        await _notificationService.SendNotificationToRoleAsync(
        //            "TechCompany",
        //            $"New order #{order.Id} has been created by customer {order.CustomerId}",
        //            NotificationType.OrderCreated,
        //            order.Id,
        //            "Order"
        //        );

        //        if (delivery.DeliveryLatitude.HasValue && delivery.DeliveryLongitude.HasValue)
        //        {

        //            var availableDriversResponse = await _deliveryService.GetAvailableDeliveriesAsync();

        //            if (availableDriversResponse.Success && availableDriversResponse.Data != null)
        //            {
        //                var availableDrivers = availableDriversResponse.Data
        //                    .Where(d => d.Latitude.HasValue && d.Longitude.HasValue && d.IsOnline)
        //                    .Select(d => new Delivery
        //                    {
        //                        Id = d.Id,
        //                        Latitude = d.Latitude,
        //                        Longitude = d.Longitude,
        //                        IsOnline = d.IsOnline,
        //                        DeliveryPerson = new DeliveryPerson
        //                        {
        //                            Id = d.DeliveryPersonId,
        //                            UserId = d.DeliveryPerson.User.Id
        //                        }
        //                    })
        //                    .ToList();

        //                await _deliveryService.AssignDeliveryToNearestAsync(delivery.Id, availableDrivers, 3, 3);
        //            }
        //        }

        //        var createdOrder = await _orderRepo.GetFirstOrDefaultAsync(
        //            o => o.Id == order.Id,
        //            includeProperties: "OrderItems,OrderItems.Product,Customer,Customer.User,OrderHistory");

        //        return new GeneralResponse<OrderReadDTO>
        //        {
        //            Success = true,
        //            Message = "Order has been created successfully and sent to Delivery.",
        //            Data = OrderMapper.ToReadDTO(createdOrder)
        //        };

        //    }
        //    catch (Exception ex)
        //    {
        //        return new GeneralResponse<OrderReadDTO>
        //        {
        //            Success = false,
        //            Message = $"An unexpected error occurred while creating the order. {ex}",
        //            Data = null
        //        };
        //    }
        //}











        //public async Task<GeneralResponse<OrderReadDTO>> CreateOrderAsync(OrderCreateDTO dto)
        //{
        //    if (dto == null)
        //        return new GeneralResponse<OrderReadDTO>
        //        {
        //            Success = false,
        //            Message = "Order data cannot be null.",
        //            Data = null
        //        };

        //    if (string.IsNullOrWhiteSpace(dto.CustomerId))
        //        return new GeneralResponse<OrderReadDTO>
        //        {
        //            Success = false,
        //            Message = "Customer ID is required.",
        //            Data = null
        //        };

        //    if (!Guid.TryParse(dto.CustomerId, out _))
        //        return new GeneralResponse<OrderReadDTO>
        //        {
        //            Success = false,
        //            Message = "Invalid Customer ID format. Expected GUID format.",
        //            Data = null
        //        };

        //    if (dto.OrderItems == null || !dto.OrderItems.Any())
        //        return new GeneralResponse<OrderReadDTO>
        //        {
        //            Success = false,
        //            Message = "Order must contain at least one item.",
        //            Data = null
        //        };

        //    try
        //    {
        //        Delivery delivery = null;
        //        if (!string.IsNullOrEmpty(dto.DeliveryId))
        //        {
        //            delivery = await _deliveryRepo.GetByIdAsync(dto.DeliveryId);
        //            if (delivery == null)
        //                throw new Exception($"Delivery with id {dto.DeliveryId} not found.");
        //        }

        //        var order = OrderMapper.ToEntity(dto);
        //        order.TotalAmount = order.OrderItems.Sum(i => i.ItemTotal);

        //        var orderHistory = await GetOrCreateOrderHistoryAsync(dto.CustomerId);
        //        order.OrderHistoryId = orderHistory.Id;

        //        await _orderRepo.AddAsync(order);
        //        await _orderRepo.SaveChangesAsync();

        //        if (delivery != null)
        //        {
        //            delivery.OrderId = order.Id;
        //        }
        //        else
        //        {
        //            delivery = new Delivery
        //            {
        //                TrackingNumber = Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper(),
        //                CustomerId = dto.CustomerId,
        //                OrderId = order.Id,
        //                DeliveryLatitude = dto.DeliveryLatitude,
        //                DeliveryLongitude = dto.DeliveryLongitude,
        //                Status = DeliveryStatus.Pending,
        //                RetryCount = 0,
        //                CreatedAt = DateTime.UtcNow
        //            };
        //            await _deliveryRepo.AddAsync(delivery);
        //        }

        //        await _deliveryRepo.SaveChangesAsync();

        //        await _notificationService.SendNotificationToRoleAsync(
        //            "Admin",
        //            $"New order #{order.Id} has been created by customer {order.CustomerId}",
        //            NotificationType.OrderCreated,
        //            order.Id,
        //            "Order"
        //        );

        //        await _notificationService.SendNotificationToRoleAsync(
        //            "Delivery",
        //            $"New delivery #{delivery.TrackingNumber} is available for assignment.",
        //            NotificationType.DeliveryAssigned,
        //            delivery.Id,
        //            "Delivery"
        //        );

        //        await _notificationService.SendNotificationToRoleAsync(
        //            "TechCompany",
        //            $"New order #{order.Id} has been created by customer {order.CustomerId}",
        //            NotificationType.OrderCreated,
        //            order.Id,
        //            "Order"
        //        );

        //        if (delivery.DeliveryLatitude.HasValue && delivery.DeliveryLongitude.HasValue)
        //        {
        //            var availableDriversResponse = await _deliveryService.GetAvailableDeliveriesAsync();

        //            if (availableDriversResponse.Success && availableDriversResponse.Data != null)
        //            {
        //                var availableDrivers = availableDriversResponse.Data
        //                    .Where(d => d.Latitude.HasValue && d.Longitude.HasValue && d.IsOnline)
        //                    .Select(d => new Delivery
        //                    {
        //                        Id = d.Id,
        //                        Latitude = d.Latitude,
        //                        Longitude = d.Longitude,
        //                        IsOnline = d.IsOnline,
        //                        DeliveryPerson = new DeliveryPerson
        //                        {
        //                            Id = d.DeliveryPersonId,
        //                            UserId = d.DeliveryPerson.User.Id
        //                        }
        //                    })
        //                    .ToList();

        //                await _deliveryService.AssignDeliveryToNearestAsync(delivery.Id, availableDrivers, 3, 3);
        //            }
        //        }

        //        var createdOrder = await _orderRepo.GetFirstOrDefaultAsync(
        //            o => o.Id == order.Id,
        //            includeProperties: "OrderItems,OrderItems.Product,Customer,Customer.User,OrderHistory");

        //        return new GeneralResponse<OrderReadDTO>
        //        {
        //            Success = true,
        //            Message = "Order has been created successfully and sent to Delivery.",
        //            Data = OrderMapper.ToReadDTO(createdOrder)
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new GeneralResponse<OrderReadDTO>
        //        {
        //            Success = false,
        //            Message = $"An unexpected error occurred while creating the order. {ex.Message}",
        //            Data = null
        //        };
        //    }
        //}

        //public async Task<GeneralResponse<OrderReadDTO>> CreateOrderAsync(OrderCreateDTO dto)
        //{
        //    if (dto == null)
        //        return new GeneralResponse<OrderReadDTO>
        //        {
        //            Success = false,
        //            Message = "Order data cannot be null.",
        //            Data = null
        //        };

        //    if (string.IsNullOrWhiteSpace(dto.CustomerId))
        //        return new GeneralResponse<OrderReadDTO>
        //        {
        //            Success = false,
        //            Message = "Customer ID is required.",
        //            Data = null
        //        };

        //    if (!Guid.TryParse(dto.CustomerId, out _))
        //        return new GeneralResponse<OrderReadDTO>
        //        {
        //            Success = false,
        //            Message = "Invalid Customer ID format. Expected GUID format.",
        //            Data = null
        //        };

        //    if (dto.OrderItems == null || !dto.OrderItems.Any())
        //        return new GeneralResponse<OrderReadDTO>
        //        {
        //            Success = false,
        //            Message = "Order must contain at least one item.",
        //            Data = null
        //        };

        //    try
        //    {
        //        // Begin a transaction (adjust if your repository supports it, otherwise use DbContext transaction)
        //        using (var transaction = await _orderRepo.BeginTransactionAsync())
        //        {
        //            // 1. Create the Order entity from DTO
        //            var order = OrderMapper.ToEntity(dto);
        //            order.TotalAmount = order.OrderItems.Sum(i => i.ItemTotal);

        //            // Get or create OrderHistory for the customer
        //            var orderHistory = await GetOrCreateOrderHistoryAsync(dto.CustomerId);
        //            order.OrderHistoryId = orderHistory.Id;

        //            // Save Order
        //            await _orderRepo.AddAsync(order);
        //            await _orderRepo.SaveChangesAsync();

        //            // 2. Create Delivery linked to newly created Order
        //            var delivery = new Delivery
        //            {
        //                TrackingNumber = Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper(),
        //                CustomerId = dto.CustomerId,
        //                OrderId = order.Id,
        //                DeliveryLatitude = dto.DeliveryLatitude,
        //                DeliveryLongitude = dto.DeliveryLongitude,
        //                Status = DeliveryStatus.Pending,
        //                RetryCount = 0,
        //                CreatedAt = DateTime.UtcNow
        //            };

        //            await _deliveryRepo.AddAsync(delivery);
        //            await _deliveryRepo.SaveChangesAsync();

        //            // 3. Assign delivery to nearest available drivers if location exists
        //            if (delivery.DeliveryLatitude.HasValue && delivery.DeliveryLongitude.HasValue)
        //            {
        //                var availableDriversResponse = await _deliveryService.GetAvailableDeliveriesAsync();

        //                if (availableDriversResponse.Success && availableDriversResponse.Data != null)
        //                {
        //                    var availableDrivers = availableDriversResponse.Data
        //                        .Where(d => d.Latitude.HasValue && d.Longitude.HasValue && d.IsOnline)
        //                        .Select(d => new Delivery
        //                        {
        //                            Id = d.Id,
        //                            Latitude = d.Latitude,
        //                            Longitude = d.Longitude,
        //                            IsOnline = d.IsOnline,
        //                            DeliveryPerson = new DeliveryPerson
        //                            {
        //                                Id = d.DeliveryPersonId,
        //                                UserId = d.DeliveryPerson.User.Id
        //                            }
        //                        })
        //                        .ToList();

        //                    await _deliveryService.AssignDeliveryToNearestAsync(delivery.Id, availableDrivers, 3, 3);
        //                }
        //            }

        //            // 4. Send notifications about new order and delivery
        //            await _notificationService.SendNotificationToRoleAsync(
        //                "Admin",
        //                $"New order #{order.Id} has been created by customer {order.CustomerId}",
        //                NotificationType.OrderCreated,
        //                order.Id,
        //                "Order"
        //            );

        //            await _notificationService.SendNotificationToRoleAsync(
        //                "Delivery",
        //                $"New delivery #{delivery.TrackingNumber} is available for assignment.",
        //                NotificationType.DeliveryAssigned,
        //                delivery.Id,
        //                "Delivery"
        //            );

        //            await _notificationService.SendNotificationToRoleAsync(
        //                "TechCompany",
        //                $"New order #{order.Id} has been created by customer {order.CustomerId}",
        //                NotificationType.OrderCreated,
        //                order.Id,
        //                "Order"
        //            );

        //            // 5. Commit the transaction only after everything is successful
        //            await transaction.CommitAsync();

        //            // 6. Reload order with related data for returning
        //            var createdOrder = await _orderRepo.GetFirstOrDefaultAsync(
        //                o => o.Id == order.Id,
        //                includeProperties: "OrderItems,OrderItems.Product,Customer,Customer.User,OrderHistory,Delivery,Delivery.DeliveryPerson,Delivery.DeliveryPerson.User"
        //            );

        //            return new GeneralResponse<OrderReadDTO>
        //            {
        //                Success = true,
        //                Message = "Order has been created successfully and sent to Delivery.",
        //                Data = OrderMapper.ToReadDTO(createdOrder)
        //            };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return new GeneralResponse<OrderReadDTO>
        //        {
        //            Success = false,
        //            Message = $"An unexpected error occurred while creating the order. {ex.Message}",
        //            Data = null
        //        };
        //    }
        //}



        //public async Task<GeneralResponse<OrderReadDTO>> CreateOrderAsync(OrderCreateDTO dto)
        //{
        //    if (dto == null)
        //        return new GeneralResponse<OrderReadDTO>
        //        {
        //            Success = false,
        //            Message = "Order data cannot be null.",
        //            Data = null
        //        };

        //    if (string.IsNullOrWhiteSpace(dto.CustomerId))
        //        return new GeneralResponse<OrderReadDTO>
        //        {
        //            Success = false,
        //            Message = "Customer ID is required.",
        //            Data = null
        //        };

        //    if (!Guid.TryParse(dto.CustomerId, out _))
        //        return new GeneralResponse<OrderReadDTO>
        //        {
        //            Success = false,
        //            Message = "Invalid Customer ID format. Expected GUID format.",
        //            Data = null
        //        };

        //    if (dto.OrderItems == null || !dto.OrderItems.Any())
        //        return new GeneralResponse<OrderReadDTO>
        //        {
        //            Success = false,
        //            Message = "Order must contain at least one item.",
        //            Data = null
        //        };

        //    try
        //    {
        //        //await using var transaction = await _dbContext.Database.BeginTransactionAsync();

        //        var order = OrderMapper.ToEntity(dto);
        //        order.TotalAmount = order.OrderItems.Sum(i => i.ItemTotal);

        //        var orderHistory = await GetOrCreateOrderHistoryAsync(dto.CustomerId);
        //        order.OrderHistoryId = orderHistory.Id;

        //        await _orderRepo.AddAsync(order);
        //        await _orderRepo.SaveChangesAsync();

        //        //var delivery = await _deliveryService.CreateDeliveryForOrderAsync(order, dto.DeliveryLatitude, dto.DeliveryLongitude, dto.CustomerId);

        //        //var delivery = new Delivery
        //        //{
        //        //    TrackingNumber = Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper(),
        //        //    CustomerId = dto.CustomerId,
        //        //    OrderId = order.Id,
        //        //    DeliveryLatitude = dto.DeliveryLatitude,
        //        //    DeliveryLongitude = dto.DeliveryLongitude,
        //        //    Status = DeliveryStatus.Pending,
        //        //    RetryCount = 0,
        //        //    CreatedAt = DateTime.UtcNow
        //        //};

        //        //await _deliveryRepo.AddAsync(delivery);
        //        //await _deliveryRepo.SaveChangesAsync();

        //        //if (delivery.DeliveryLatitude.HasValue && delivery.DeliveryLongitude.HasValue)
        //        //{
        //        //    var availableDriversResponse = await _deliveryService.GetAvailableDeliveriesAsync();

        //        //    if (availableDriversResponse.Success && availableDriversResponse.Data != null)
        //        //    {
        //        //        var availableDrivers = availableDriversResponse.Data
        //        //            .Where(d => d.Latitude.HasValue && d.Longitude.HasValue && d.IsOnline)
        //        //            .Select(d => new Delivery
        //        //            {
        //        //                Id = d.Id,
        //        //                Latitude = d.Latitude,
        //        //                Longitude = d.Longitude,
        //        //                IsOnline = d.IsOnline,
        //        //                DeliveryPerson = new DeliveryPerson
        //        //                {
        //        //                    Id = d.DeliveryPersonId,
        //        //                    UserId = d.DeliveryPerson.User.Id
        //        //                }
        //        //            })
        //        //            .ToList();

        //        //        await _deliveryService.AssignDeliveryToNearestAsync(delivery.Id, availableDrivers, 3, 3);
        //        //    }
        //        //}

        //        //await _notificationService.SendNotificationToRoleAsync(
        //        //    "Admin",
        //        //    $"New order #{order.Id} has been created by customer {order.CustomerId}",
        //        //    NotificationType.OrderCreated,
        //        //    order.Id,
        //        //    "Order"
        //        //);

        //        //await _notificationService.SendNotificationToRoleAsync(
        //        //    "Delivery",
        //        //    $"New delivery #{delivery.TrackingNumber} is available for assignment.",
        //        //    NotificationType.DeliveryAssigned,
        //        //    delivery.Id,
        //        //    "Delivery"
        //        //);

        //        //await _notificationService.SendNotificationToRoleAsync(
        //        //    "TechCompany",
        //        //    $"New order #{order.Id} has been created by customer {order.CustomerId}",
        //        //    NotificationType.OrderCreated,
        //        //    order.Id,
        //        //    "Order"
        //        //);

        //        //await transaction.CommitAsync();

        //        var createdOrder = await _orderRepo.GetFirstOrDefaultAsync(
        //            o => o.Id == order.Id,
        //            includeProperties: "OrderItems,OrderItems.Product,Customer,Customer.User,OrderHistory,Delivery,Delivery.DeliveryPerson,Delivery.DeliveryPerson.User"
        //        );

        //        return new GeneralResponse<OrderReadDTO>
        //        {
        //            Success = true,
        //            Message = "Order has been created successfully and sent to Delivery.",
        //            Data = OrderMapper.ToReadDTO(createdOrder)
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new GeneralResponse<OrderReadDTO>
        //        {
        //            Success = false,
        //            Message = $"An unexpected error occurred while creating the order. {ex.Message}",
        //            Data = null
        //        };
        //    }
        //}

        public async Task<GeneralResponse<OrderReadDTO>> CreateOrderAsync(OrderCreateDTO dto)
        {
            if (dto == null)
                return new GeneralResponse<OrderReadDTO>
                {
                    Success = false,
                    Message = "Order data cannot be null.",
                    Data = null
                };

            if (string.IsNullOrWhiteSpace(dto.CustomerId))
                return new GeneralResponse<OrderReadDTO>
                {
                    Success = false,
                    Message = "Customer ID is required.",
                    Data = null
                };

            if (!Guid.TryParse(dto.CustomerId, out _))
                return new GeneralResponse<OrderReadDTO>
                {
                    Success = false,
                    Message = "Invalid Customer ID format. Expected GUID format.",
                    Data = null
                };

            if (dto.OrderItems == null || !dto.OrderItems.Any())
                return new GeneralResponse<OrderReadDTO>
                {
                    Success = false,
                    Message = "Order must contain at least one item.",
                    Data = null
                };

            try
            {
                var order = OrderMapper.ToEntity(dto);
                order.TotalAmount = order.OrderItems.Sum(i => i.ItemTotal);

                var orderHistory = await GetOrCreateOrderHistoryAsync(dto.CustomerId);
                order.OrderHistoryId = orderHistory.Id;

                await _orderRepo.AddAsync(order);
                await _orderRepo.SaveChangesAsync();

                var deliveryDto = new DeliveryCreateDTO
                {
                    OrderId = order.Id,
                    CustomerLatitude = (double)dto.DeliveryLatitude,
                    CustomerLongitude = (double)dto.DeliveryLongitude
                };

                var deliveryResponse = await _deliveryService.CreateAsync(deliveryDto);

                if (!deliveryResponse.Success)
                {
                    _logger.LogWarning("CreateOrderAsync: Delivery creation failed for order {OrderId}: {Message}",
                        order.Id, deliveryResponse.Message);
                }
                else
                {
                    _logger.LogInformation("CreateOrderAsync: Delivery created successfully for order {OrderId}.", order.Id);
                }

                var createdOrder = await _orderRepo.GetFirstOrDefaultAsync(
                    o => o.Id == order.Id,
                    query => query
                        .Include(o => o.OrderItems)
                            .ThenInclude(oi => oi.Product)
                                .ThenInclude(p => p.TechCompany)
                        .Include(o => o.Customer)
                            .ThenInclude(c => c.User)
                        .Include(o => o.OrderHistory)
                        .Include(o => o.Deliveries)
                            .ThenInclude(d => d.DeliveryPerson)
                                .ThenInclude(dp => dp.User)
                );

                return new GeneralResponse<OrderReadDTO>
                {
                    Success = true,
                    Message = "Order created successfully and delivery offers sent to nearby drivers.",
                    Data = OrderMapper.ToReadDTO(createdOrder)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateOrderAsync: Failed to create order.");
                return new GeneralResponse<OrderReadDTO>
                {
                    Success = false,
                    Message = $"An unexpected error occurred while creating the order. {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<GeneralResponse<OrderReadDTO>> GetOrderByIdAsync(string id)
        {
            
            if (string.IsNullOrWhiteSpace(id))
            {
                return new GeneralResponse<OrderReadDTO>
                {
                    Success = false,
                    Message = "Order ID cannot be null or empty.",
                    Data = null
                };
            }

            if (!Guid.TryParse(id, out _))
            {
                return new GeneralResponse<OrderReadDTO>
                {
                    Success = false,
                    Message = "Invalid Order ID format. Expected GUID format.",
                    Data = null
                };
            }

            try
            {
                // Comprehensive includes for detailed order view
                var order = await _orderRepo.GetByIdWithIncludesAsync(id,
                    o => o.OrderItems,
                    o => o.Customer,
                    o => o.OrderHistory,
                    o => o.Deliveries,
                    o => o.ServiceUsage);

                if (order == null)
                {
                    return new GeneralResponse<OrderReadDTO>
                    {
                        Success = false,
                        Message = $"Order with ID '{id}' not found.",
                        Data = null
                    };
                }

                // Get order items with their products using string includes for nested properties
                var orderWithItems = await _orderRepo.FindWithStringIncludesAsync(
                    o => o.Id == id,
                    includeProperties: "OrderItems,OrderItems.Product,OrderItems.Product.Category,OrderItems.Product.SubCategory,OrderItems.Product.TechCompany,Customer,Customer.User,OrderHistory,Delivery,ServiceUsage");

                var orderEntity = orderWithItems.FirstOrDefault();
                if (orderEntity == null)
                {
                    return new GeneralResponse<OrderReadDTO>
                    {
                        Success = false,
                        Message = $"Order with ID '{id}' not found.",
                        Data = null
                    };
                }

                return new GeneralResponse<OrderReadDTO>
                {
                    Success = true,
                    Message = "Order retrieved successfully.",
                    Data = OrderMapper.ToReadDTO(orderEntity)
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<OrderReadDTO>
                {
                    Success = false,
                    Message = "An unexpected error occurred while retrieving the order.",
                    Data = null
                };
            }
        }

        public async Task<GeneralResponse<IEnumerable<OrderReadDTO>>> GetAllOrdersAsync()
        {
            try
            {
                // Optimized includes for order listing with all necessary related data
                var allOrders = await _orderRepo.FindWithStringIncludesAsync(
                    o => true, // This will match all orders
                    includeProperties: "OrderItems,OrderItems.Product,OrderItems.Product.Category,OrderItems.Product.SubCategory,OrderItems.Product.TechCompany,Customer,Customer.User,OrderHistory,Delivery,ServiceUsage");
                
                var orderDtos = allOrders.Where(o => o != null).Select(OrderMapper.ToReadDTO).Where(dto => dto != null);
                
                return new GeneralResponse<IEnumerable<OrderReadDTO>>
                {
                    Success = true,
                    Message = "Orders retrieved successfully.",
                    Data = orderDtos
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<IEnumerable<OrderReadDTO>>
                {
                    Success = false,
                    Message = "An unexpected error occurred while retrieving orders.",
                    Data = null
                };
            }
        }

        public async Task<GeneralResponse<IEnumerable<OrderReadDTO>>> GetOrdersByCustomerIdAsync(string customerId)
        {
            
            if (string.IsNullOrWhiteSpace(customerId))
            {
                return new GeneralResponse<IEnumerable<OrderReadDTO>>
                {
                    Success = false,
                    Message = "Customer ID cannot be null or empty.",
                    Data = null
                };
            }

            if (!Guid.TryParse(customerId, out _))
            {
                return new GeneralResponse<IEnumerable<OrderReadDTO>>
                {
                    Success = false,
                    Message = "Invalid Customer ID format. Expected GUID format.",
                    Data = null
                };
            }

            try
            {
                // Optimized includes for customer orders with all necessary related data
                var orders = await _orderRepo.FindWithStringIncludesAsync(
                    o => o.CustomerId == customerId, 
                    includeProperties: "OrderItems,OrderItems.Product,OrderItems.Product.Category,OrderItems.Product.SubCategory,OrderItems.Product.TechCompany,Customer,Customer.User,OrderHistory,Delivery,ServiceUsage");
                
                var orderDtos = orders.Where(o => o != null).Select(OrderMapper.ToReadDTO).Where(dto => dto != null);
                
                return new GeneralResponse<IEnumerable<OrderReadDTO>>
                {
                    Success = true,
                    Message = "Customer orders retrieved successfully.",
                    Data = orderDtos
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<IEnumerable<OrderReadDTO>>
                {
                    Success = false,
                    Message = "An unexpected error occurred while retrieving customer orders.",
                    Data = null
                };
            }
        }

        private async Task<OrderHistory> GetOrCreateOrderHistoryAsync(string customerId)
        {
            var existingHistory = await _orderHistoryRepo.GetFirstOrDefaultAsync(
                oh => oh.Orders.Any(o => o.CustomerId == customerId),
                includeProperties: "Orders");

            if (existingHistory != null)
            {
                return existingHistory;
            }

            var newHistory = new OrderHistory
            {
                Id = Guid.NewGuid().ToString(),
                Orders = new List<Order>()
            };

            await _orderHistoryRepo.AddAsync(newHistory);
            await _orderHistoryRepo.SaveChangesAsync();

            return newHistory;
        }

        public async Task<GeneralResponse<IEnumerable<OrderHistoryReadDTO>>> GetOrderHistoryByCustomerIdAsync(string customerId)
        {
            
            if (string.IsNullOrWhiteSpace(customerId))
            {
                return new GeneralResponse<IEnumerable<OrderHistoryReadDTO>>
                {
                    Success = false,
                    Message = "Customer ID cannot be null or empty.",
                    Data = null
                };
            }

            if (!Guid.TryParse(customerId, out _))
            {
                return new GeneralResponse<IEnumerable<OrderHistoryReadDTO>>
                {
                    Success = false,
                    Message = "Invalid Customer ID format. Expected GUID format.",
                    Data = null
                };
            }

            try
            {
                var orderHistories = await _orderHistoryRepo.FindWithStringIncludesAsync(
                    oh => oh.Orders.Any(o => o.CustomerId == customerId),
                    includeProperties: "Orders,Orders.OrderItems,Orders.OrderItems.Product,Orders.Customer,Orders.Customer.User");

                var orderHistoryDtos = orderHistories
                    .Where(oh => oh != null)
                    .Select(OrderHistoryMapper.MapToOrderHistoryReadDTO)
                    .Where(dto => dto != null);

                return new GeneralResponse<IEnumerable<OrderHistoryReadDTO>>
                {
                    Success = true,
                    Message = "Customer order history retrieved successfully.",
                    Data = orderHistoryDtos
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<IEnumerable<OrderHistoryReadDTO>>
                {
                    Success = false,
                    Message = "An unexpected error occurred while retrieving customer order history.",
                    Data = null
                };
            }
        }

        public async Task<GeneralResponse<bool>> UpdateOrderStatusAsync(string orderId, OrderStatus newStatus)
        {
            
            if (string.IsNullOrWhiteSpace(orderId))
            {
                return new GeneralResponse<bool>
                {
                    Success = false,
                    Message = "Order ID cannot be null or empty.",
                    Data = false
                };
            }

            if (!Guid.TryParse(orderId, out _))
            {
                return new GeneralResponse<bool>
                {
                    Success = false,
                    Message = "Invalid Order ID format. Expected GUID format.",
                    Data = false
                };
            }

            try
            {
                var order = await _orderRepo.GetByIdAsync(orderId);
                if (order == null)
                {
                    return new GeneralResponse<bool>
                    {
                        Success = false,
                        Message = "Order not found.",
                        Data = false
                    };
                }

                order.Status = newStatus;
                _orderRepo.Update(order);
                await _orderRepo.SaveChangesAsync();

                await _notificationService.SendNotificationAsync(
                    order.CustomerId,
                    $"Your order #{order.Id} status has been updated to '{newStatus.GetStringValue()}'",
                    NotificationType.OrderStatusChanged,
                    order.Id,
                    "Order"
                );

                return new GeneralResponse<bool>
                {
                    Success = true,
                    Message = $"Order status updated successfully to '{newStatus.GetStringValue()}'.",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<bool>
                {
                    Success = false,
                    Message = $"An unexpected error occurred while updating order status. {ex}",
                    Data = false
                };
            }
        }

        public async Task<GeneralResponse<IEnumerable<OrderReadDTO>>> GetOrdersByStatusAsync(OrderStatus status)
        {
            try
            {
                var orders = await _orderRepo.FindWithStringIncludesAsync(
                    o => o.Status == status,
                    includeProperties: "OrderItems,OrderItems.Product,Customer,Customer.User,OrderHistory");

                var orderDtos = orders
                    .Where(o => o != null)
                    .Select(OrderMapper.ToReadDTO)
                    .Where(dto => dto != null);

                return new GeneralResponse<IEnumerable<OrderReadDTO>>
                {
                    Success = true,
                    Message = $"Orders with status '{status.GetStringValue()}' retrieved successfully.",
                    Data = orderDtos
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<IEnumerable<OrderReadDTO>>
                {
                    Success = false,
                    Message = "An unexpected error occurred while retrieving orders by status.",
                    Data = null
                };
            }
        }
    }
}
