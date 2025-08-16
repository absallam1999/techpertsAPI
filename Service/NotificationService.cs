using Core.DTOs;
using Core.DTOs.NotificationDTOs;
using Core.Enums;
using Core.Factories;
using Core.Interfaces;
using Core.Interfaces.Services;
using Hubs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using TechpertsSolutions.Core.Entities;

namespace Service
{
    public class NotificationService : INotificationService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IHubContext<NotificationsHub> _hubContext;
        private readonly IRepository<Notification> _notificationRepo;

        public NotificationService(
            IRepository<Notification> notificationRepo,
            UserManager<AppUser> userManager,
            IHubContext<NotificationsHub> hubContext)
        {
            _notificationRepo = notificationRepo;
            _userManager = userManager;
            _hubContext = hubContext;
        }


        public async Task<GeneralResponse<NotificationDTO>> SendNotificationAsync(
            string receiverUserId,
            string message,
            NotificationType type,
            string? relatedEntityId = null,
            string? relatedEntityType = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(receiverUserId))
                {
                    return new GeneralResponse<NotificationDTO>
                    {
                        Success = false,
                        Message = "Receiver user ID cannot be null or empty.",
                        Data = null
                    };
                }

                if (string.IsNullOrWhiteSpace(message))
                {
                    return new GeneralResponse<NotificationDTO>
                    {
                        Success = false,
                        Message = "Message cannot be null or empty.",
                        Data = null
                    };
                }

                var notification = new Notification
                {
                    ReceiverUserId = receiverUserId,
                    Message = message,
                    Type = type,
                    IsRead = false,
                    RelatedEntityId = relatedEntityId,
                    RelatedEntityType = relatedEntityType
                };

                await _notificationRepo.AddAsync(notification);
                await _notificationRepo.SaveChangesAsync();

                // Send real-time notification via SignalR
                await SendRealTimeNotificationAsync(receiverUserId, notification);

                var notificationDto = MapToNotificationDTO(notification);
                return new GeneralResponse<NotificationDTO>
                {
                    Success = true,
                    Message = "Notification sent successfully.",
                    Data = notificationDto
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<NotificationDTO>
                {
                    Success = false,
                    Message = $"An error occurred while sending notification: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<GeneralResponse<IEnumerable<NotificationDTO>>> GetUserNotificationsAsync(
            string userId,
            int pageNumber = 1,
            int pageSize = 20)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return new GeneralResponse<IEnumerable<NotificationDTO>>
                    {
                        Success = false,
                        Message = "User ID cannot be null or empty.",
                        Data = null
                    };
                }

                var notifications = await _notificationRepo.FindWithIncludesAsync(
                    n => n.ReceiverUserId == userId,
                    n => n.Receiver);

                var sortedNotifications = notifications
                    .OrderByDescending(n => n.CreatedAt)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var notificationDtos = sortedNotifications.Select(MapToNotificationDTO);

                return new GeneralResponse<IEnumerable<NotificationDTO>>
                {
                    Success = true,
                    Message = "Notifications retrieved successfully.",
                    Data = notificationDtos
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<IEnumerable<NotificationDTO>>
                {
                    Success = false,
                    Message = $"An error occurred while retrieving notifications: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<GeneralResponse<NotificationDTO>> GetNotificationByIdAsync(string notificationId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(notificationId))
                {
                    return new GeneralResponse<NotificationDTO>
                    {
                        Success = false,
                        Message = "Notification ID cannot be null or empty.",
                        Data = null
                    };
                }

                var notification = await _notificationRepo.GetByIdWithIncludesAsync(
                    notificationId,
                    n => n.Receiver);

                if (notification == null)
                {
                    return new GeneralResponse<NotificationDTO>
                    {
                        Success = false,
                        Message = "Notification not found.",
                        Data = null
                    };
                }

                var notificationDto = MapToNotificationDTO(notification);
                return new GeneralResponse<NotificationDTO>
                {
                    Success = true,
                    Message = "Notification retrieved successfully.",
                    Data = notificationDto
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<NotificationDTO>
                {
                    Success = false,
                    Message = $"An error occurred while retrieving notification: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<GeneralResponse<bool>> MarkAsReadAsync(string notificationId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(notificationId))
                {
                    return new GeneralResponse<bool>
                    {
                        Success = false,
                        Message = "Notification ID cannot be null or empty.",
                        Data = false
                    };
                }

                var notification = await _notificationRepo.GetByIdAsync(notificationId);
                if (notification == null)
                {
                    return new GeneralResponse<bool>
                    {
                        Success = false,
                        Message = "Notification not found.",
                        Data = false
                    };
                }

                notification.IsRead = true;
                _notificationRepo.Update(notification);
                await _notificationRepo.SaveChangesAsync();

                return new GeneralResponse<bool>
                {
                    Success = true,
                    Message = "Notification marked as read successfully.",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<bool>
                {
                    Success = false,
                    Message = $"An error occurred while marking notification as read: {ex.Message}",
                    Data = false
                };
            }
        }

        public async Task<GeneralResponse<bool>> MarkAllAsReadAsync(string userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return new GeneralResponse<bool>
                    {
                        Success = false,
                        Message = "User ID cannot be null or empty.",
                        Data = false
                    };
                }

                var notifications = await _notificationRepo.FindAsync(n => n.ReceiverUserId == userId && !n.IsRead);

                foreach (var notification in notifications)
                {
                    notification.IsRead = true;
                    _notificationRepo.Update(notification);
                }

                await _notificationRepo.SaveChangesAsync();

                return new GeneralResponse<bool>
                {
                    Success = true,
                    Message = "All notifications marked as read successfully.",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<bool>
                {
                    Success = false,
                    Message = $"An error occurred while marking all notifications as read: {ex.Message}",
                    Data = false
                };
            }
        }

        public async Task<GeneralResponse<bool>> DeleteNotificationAsync(string notificationId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(notificationId))
                {
                    return new GeneralResponse<bool>
                    {
                        Success = false,
                        Message = "Notification ID cannot be null or empty.",
                        Data = false
                    };
                }

                var notification = await _notificationRepo.GetByIdAsync(notificationId);
                if (notification == null)
                {
                    return new GeneralResponse<bool>
                    {
                        Success = false,
                        Message = "Notification not found.",
                        Data = false
                    };
                }

                _notificationRepo.Remove(notification);
                await _notificationRepo.SaveChangesAsync();

                return new GeneralResponse<bool>
                {
                    Success = true,
                    Message = "Notification deleted successfully.",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<bool>
                {
                    Success = false,
                    Message = $"An error occurred while deleting notification: {ex.Message}",
                    Data = false
                };
            }
        }

        public async Task<GeneralResponse<NotificationCountDTO>> GetNotificationCountAsync(string userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return new GeneralResponse<NotificationCountDTO>
                    {
                        Success = false,
                        Message = "User ID cannot be null or empty.",
                        Data = null
                    };
                }

                var allNotifications = await _notificationRepo.FindAsync(n => n.ReceiverUserId == userId);
                var unreadNotifications = await _notificationRepo.FindAsync(n => n.ReceiverUserId == userId && !n.IsRead);

                var countDto = new NotificationCountDTO
                {
                    TotalCount = allNotifications.Count(),
                    UnreadCount = unreadNotifications.Count()
                };

                return new GeneralResponse<NotificationCountDTO>
                {
                    Success = true,
                    Message = "Notification count retrieved successfully.",
                    Data = countDto
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<NotificationCountDTO>
                {
                    Success = false,
                    Message = $"An error occurred while retrieving notification count: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<GeneralResponse<bool>> SendNotificationToRoleAsync(
            string roleName,
            string message,
            NotificationType type,
            string? relatedEntityId = null,
            string? relatedEntityType = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(roleName))
                {
                    return new GeneralResponse<bool>
                    {
                        Success = false,
                        Message = "Role name cannot be null or empty.",
                        Data = false
                    };
                }

                var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
                var userIds = usersInRole.Select(u => u.Id).ToList();

                return await SendNotificationToMultipleUsersAsync(userIds, message, type, relatedEntityId, relatedEntityType);
            }
            catch (Exception ex)
            {
                return new GeneralResponse<bool>
                {
                    Success = false,
                    Message = $"An error occurred while sending notification to role: {ex.Message}",
                    Data = false
                };
            }
        }

        public async Task<GeneralResponse<bool>> SendNotificationToMultipleUsersAsync(
            List<string> userIds,
            string message,
            NotificationType type,
            string? relatedEntityId = null,
            string? relatedEntityType = null)
        {
            try
            {
                if (userIds == null || !userIds.Any())
                {
                    return new GeneralResponse<bool>
                    {
                        Success = false,
                        Message = "User IDs list cannot be null or empty.",
                        Data = false
                    };
                }

                var notifications = new List<Notification>();
                foreach (var userId in userIds)
                {
                    var notification = new Notification
                    {
                        ReceiverUserId = userId,
                        Message = message,
                        Type = type,
                        IsRead = false,
                        RelatedEntityId = relatedEntityId,
                        RelatedEntityType = relatedEntityType
                    };
                    notifications.Add(notification);
                }

                foreach (var notification in notifications)
                {
                    await _notificationRepo.AddAsync(notification);
                }
                await _notificationRepo.SaveChangesAsync();

                // Send real-time notifications via SignalR
                foreach (var notification in notifications)
                {
                    await SendRealTimeNotificationAsync(notification.ReceiverUserId, notification);
                }

                return new GeneralResponse<bool>
                {
                    Success = true,
                    Message = $"Notifications sent successfully to {notifications.Count} users.",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<bool>
                {
                    Success = false,
                    Message = $"An error occurred while sending notifications to multiple users: {ex.Message}",
                    Data = false
                };
            }
        }

        public async Task<GeneralResponse<NotificationDTO>> SendNotificationFromFactoryAsync(
        string receiverUserId,
        NotificationType type,
        string? relatedEntityId = null,
        string? relatedEntityType = null,
        params object[] args)
        {
            try
            {
                // Use factory to create notification
                var notification = NotificationsFactory.Create(
                    receiverUserId,
                    type,
                    relatedEntityId,
                    relatedEntityType,
                    args
                );

                // Save to DB
                await _notificationRepo.AddAsync(notification);
                await _notificationRepo.SaveChangesAsync();

                // Send via SignalR
                await SendRealTimeNotificationAsync(receiverUserId, notification);

                var notificationDto = MapToNotificationDTO(notification);

                return new GeneralResponse<NotificationDTO>
                {
                    Success = true,
                    Message = "Notification sent successfully.",
                    Data = notificationDto
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<NotificationDTO>
                {
                    Success = false,
                    Message = $"Failed to send notification: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<GeneralResponse<bool>> SendNotificationsToMultipleUsersFromFactoryAsync(
        IEnumerable<string> userIds,
        NotificationType type,
        string? relatedEntityId = null,
        string? relatedEntityType = null,
        params object[] args)
        {
            try
            {
                if (userIds == null || !userIds.Any())
                    return new GeneralResponse<bool> { Success = false, Message = "No users provided", Data = false };

                var notifications = NotificationsFactory.CreateForUsers(userIds, type, relatedEntityId, relatedEntityType, args);

                foreach (var notification in notifications)
                    await _notificationRepo.AddAsync(notification);

                await _notificationRepo.SaveChangesAsync();

                foreach (var notification in notifications)
                    await SendRealTimeNotificationAsync(notification.ReceiverUserId, notification);

                return new GeneralResponse<bool>
                {
                    Success = true,
                    Message = $"Notifications sent successfully to {notifications.Count} users.",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<bool>
                {
                    Success = false,
                    Message = $"Failed to send notifications: {ex.Message}",
                    Data = false
                };
            }
        }


        private async Task SendRealTimeNotificationAsync(string userId, Notification notification)
        {
            try
            {
                var notificationDto = MapToNotificationDTO(notification);

                // Consistent group name
                await _hubContext.Clients.Group($"User_{userId}")
                    .SendAsync("ReceiveNotification", notificationDto);
            }
            catch (Exception ex)
            {
                // TODO: replace with ILogger
                Console.WriteLine($"Error sending real-time notification: {ex.Message}");
            }
        }

        private NotificationDTO MapToNotificationDTO(Notification notification)
        {
            return new NotificationDTO
            {
                Id = notification.Id,
                ReceiverUserId = notification.ReceiverUserId,
                Message = notification.Message,
                Type = notification.Type,
                IsRead = notification.IsRead,
                CreatedAt = notification.CreatedAt,
                RelatedEntityId = notification.RelatedEntityId,
                RelatedEntityType = notification.RelatedEntityType,
                ReceiverName = notification.Receiver?.FullName
            };
        }
    }
}