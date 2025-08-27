using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using GameCore.Core.DTOs;
using Microsoft.Extensions.Logging;

namespace GameCore.Core.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(
            INotificationRepository notificationRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ILogger<NotificationService> logger)
        {
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<NotificationDto>> GetNotificationsByUserAsync(int userId, int page = 1, int pageSize = 20)
        {
            try
            {
                var notifications = await _notificationRepository.GetByUserIdAsync(userId, page, pageSize);
                return notifications.Select(n => new NotificationDto
                {
                    NotificationId = n.Id,
                    UserId = n.UserId,
                    Title = n.Title,
                    Message = n.Message,
                    Type = (NotificationType)n.Type,
                    Source = n.Source,
                    IsRead = n.IsRead,
                    CreatedAt = n.CreatedAt,
                    ReadAt = n.ReadAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取用戶通知失敗: {UserId}", userId);
                return Enumerable.Empty<NotificationDto>();
            }
        }

        public async Task<bool> MarkAsReadAsync(int notificationId)
        {
            try
            {
                var notification = await _notificationRepository.GetByIdAsync(notificationId);
                if (notification == null) return false;

                notification.IsRead = true;
                notification.ReadAt = DateTime.UtcNow;

                var result = await _notificationRepository.UpdateAsync(notification);
                return result != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "標記通知為已讀失敗: {NotificationId}", notificationId);
                return false;
            }
        }

        public async Task<NotificationDto> CreateSystemNotificationAsync(int userId, string title, string message, string source, string type)
        {
            try
            {
                var notification = new Notification
                {
                    UserId = userId,
                    Title = title,
                    Message = message,
                    Type = (int)NotificationType.System,
                    Source = source,
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await _notificationRepository.CreateAsync(notification);
                if (result == null) return null;

                return new NotificationDto
                {
                    NotificationId = result.Id,
                    UserId = result.UserId,
                    Title = result.Title,
                    Message = result.Message,
                    Type = (NotificationType)result.Type,
                    Source = result.Source,
                    IsRead = result.IsRead,
                    CreatedAt = result.CreatedAt,
                    ReadAt = result.ReadAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "創建系統通知失敗: {UserId}", userId);
                return null;
            }
        }

        public async Task<NotificationDto> CreatePetInteractionNotificationAsync(int userId, string petName, string interactionType)
        {
            try
            {
                var title = "寵物互動";
                var message = $"您的寵物 {petName} 完成了 {interactionType} 互動！";
                var source = "PetInteraction";

                var notification = new Notification
                {
                    UserId = userId,
                    Title = title,
                    Message = message,
                    Type = (int)NotificationType.PetInteraction,
                    Source = source,
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await _notificationRepository.CreateAsync(notification);
                if (result == null) return null;

                return new NotificationDto
                {
                    NotificationId = result.Id,
                    UserId = result.UserId,
                    Title = result.Title,
                    Message = result.Message,
                    Type = (NotificationType)result.Type,
                    Source = result.Source,
                    IsRead = result.IsRead,
                    CreatedAt = result.CreatedAt,
                    ReadAt = result.ReadAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "創建寵物互動通知失敗: {UserId}", userId);
                return null;
            }
        }

        public async Task<NotificationDto> CreateOrderNotificationAsync(int userId, int orderId, string orderStatus)
        {
            try
            {
                var title = "訂單狀態更新";
                var message = $"您的訂單 #{orderId} 狀態已更新為：{orderStatus}";
                var source = "Order";

                var notification = new Notification
                {
                    UserId = userId,
                    Title = title,
                    Message = message,
                    Type = (int)NotificationType.Order,
                    Source = source,
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await _notificationRepository.CreateAsync(notification);
                if (result == null) return null;

                return new NotificationDto
                {
                    NotificationId = result.Id,
                    UserId = result.UserId,
                    Title = result.Title,
                    Message = result.Message,
                    Type = (NotificationType)result.Type,
                    Source = result.Source,
                    IsRead = result.IsRead,
                    CreatedAt = result.CreatedAt,
                    ReadAt = result.ReadAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "創建訂單通知失敗: {UserId}, {OrderId}", userId, orderId);
                return null;
            }
        }

        public async Task<IEnumerable<string>> GetNotificationSourcesAsync()
        {
            try
            {
                return await _notificationRepository.GetNotificationSourcesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取通知來源失敗");
                return Enumerable.Empty<string>();
            }
        }

        public async Task<IEnumerable<string>> GetNotificationActionsAsync()
        {
            try
            {
                return new List<string>
                {
                    "MarkAsRead",
                    "Delete",
                    "Archive",
                    "Reply",
                    "Forward"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取通知操作失敗");
                return Enumerable.Empty<string>();
            }
        }

        public async Task<bool> DeleteNotificationAsync(int notificationId)
        {
            try
            {
                return await _notificationRepository.DeleteAsync(notificationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "刪除通知失敗: {NotificationId}", notificationId);
                return false;
            }
        }

        public async Task<bool> MarkAllAsReadAsync(int userId)
        {
            try
            {
                return await _notificationRepository.MarkAllAsReadAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "標記所有通知為已讀失敗: {UserId}", userId);
                return false;
            }
        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            try
            {
                return await _notificationRepository.GetUnreadCountAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取未讀通知數量失敗: {UserId}", userId);
                return 0;
            }
        }
    }
} 