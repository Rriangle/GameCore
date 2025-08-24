using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using GameCore.Core.DTOs;
using GameCore.Core.Enums;
using GameCore.Core.Services;
using Microsoft.Extensions.Logging;

namespace GameCore.Core.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationSourceRepository _notificationSourceRepository;
        private readonly INotificationActionRepository _notificationActionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(
            INotificationRepository notificationRepository,
            INotificationSourceRepository notificationSourceRepository,
            INotificationActionRepository notificationActionRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ILogger<NotificationService> logger)
        {
            _notificationRepository = notificationRepository;
            _notificationSourceRepository = notificationSourceRepository;
            _notificationActionRepository = notificationActionRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(int userId, int page = 1, int pageSize = 20)
        {
            try
            {
                var notifications = await _notificationRepository.GetByUserIdAsync(userId, page, pageSize);
                return notifications.Select(n => new NotificationDto
                {
                    Id = n.Id,
                    UserId = n.UserId,
                    Type = n.Type,
                    Title = n.Title,
                    Message = n.Message,
                    Data = n.Data,
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

        public async Task<bool> MarkAsReadAsync(int notificationId, int userId)
        {
            try
            {
                var notification = await _notificationRepository.GetByIdAsync(notificationId);
                if (notification == null || notification.UserId != userId)
                {
                    return false;
                }

                notification.IsRead = true;
                notification.ReadAt = DateTime.UtcNow;

                _notificationRepository.Update(notification);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("通知標記為已讀: {NotificationId}, 用戶: {UserId}", notificationId, userId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "標記通知為已讀失敗: {NotificationId}", notificationId);
                return false;
            }
        }

        public async Task<bool> MarkAllAsReadAsync(int userId)
        {
            try
            {
                var unreadNotifications = await _notificationRepository.GetUnreadByUserIdAsync(userId);
                foreach (var notification in unreadNotifications)
                {
                    notification.IsRead = true;
                    notification.ReadAt = DateTime.UtcNow;
                    _notificationRepository.Update(notification);
                }

                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("所有通知標記為已讀: 用戶 {UserId}", userId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "標記所有通知為已讀失敗: 用戶 {UserId}", userId);
                return false;
            }
        }

        public async Task<bool> DeleteNotificationAsync(int notificationId, int userId)
        {
            try
            {
                var notification = await _notificationRepository.GetByIdAsync(notificationId);
                if (notification == null || notification.UserId != userId)
                {
                    return false;
                }

                _notificationRepository.Delete(notification);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("通知刪除成功: {NotificationId}, 用戶: {UserId}", notificationId, userId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "刪除通知失敗: {NotificationId}", notificationId);
                return false;
            }
        }

        public async Task<bool> CreateSystemNotificationAsync(int userId, string title, string message, string data = null)
        {
            try
            {
                var notification = new Notification
                {
                    UserId = userId,
                    Type = NotificationType.System,
                    Title = title,
                    Message = message,
                    Data = data,
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                };

                _notificationRepository.Add(notification);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("系統通知創建成功: 用戶 {UserId}, 標題: {Title}", userId, title);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "創建系統通知失敗: 用戶 {UserId}", userId);
                return false;
            }
        }

        public async Task<bool> CreateSignInNotificationAsync(int userId, int points, int experience)
        {
            try
            {
                var notification = new Notification
                {
                    UserId = userId,
                    Type = NotificationType.SignIn,
                    Title = "每日簽到成功",
                    Message = $"恭喜您完成每日簽到！獲得 {points} 點數和 {experience} 經驗值。",
                    Data = $"{{\"points\":{points},\"experience\":{experience}}}",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                };

                _notificationRepository.Add(notification);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("簽到通知創建成功: 用戶 {UserId}, 點數: {Points}, 經驗值: {Experience}", 
                    userId, points, experience);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "創建簽到通知失敗: 用戶 {UserId}", userId);
                return false;
            }
        }

        public async Task<bool> CreatePetNotificationAsync(int userId, string petName, string action, string message)
        {
            try
            {
                var notification = new Notification
                {
                    UserId = userId,
                    Type = NotificationType.Pet,
                    Title = $"寵物 {petName} {action}",
                    Message = message,
                    Data = $"{{\"petName\":\"{petName}\",\"action\":\"{action}\"}}",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                };

                _notificationRepository.Add(notification);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("寵物通知創建成功: 用戶 {UserId}, 寵物: {PetName}, 動作: {Action}", 
                    userId, petName, action);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "創建寵物通知失敗: 用戶 {UserId}", userId);
                return false;
            }
        }

        public async Task<bool> CreateOrderNotificationAsync(int userId, string orderNumber, string status, string message)
        {
            try
            {
                var notification = new Notification
                {
                    UserId = userId,
                    Type = NotificationType.Order,
                    Title = $"訂單 {orderNumber} 狀態更新",
                    Message = message,
                    Data = $"{{\"orderNumber\":\"{orderNumber}\",\"status\":\"{status}\"}}",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                };

                _notificationRepository.Add(notification);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("訂單通知創建成功: 用戶 {UserId}, 訂單號: {OrderNumber}, 狀態: {Status}", 
                    userId, orderNumber, status);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "創建訂單通知失敗: 用戶 {UserId}", userId);
                return false;
            }
        }

        public async Task<bool> CreateMessageNotificationAsync(int userId, string senderName, string message)
        {
            try
            {
                var notification = new Notification
                {
                    UserId = userId,
                    Type = NotificationType.Message,
                    Title = $"來自 {senderName} 的新消息",
                    Message = message.Length > 100 ? message.Substring(0, 100) + "..." : message,
                    Data = $"{{\"senderName\":\"{senderName}\"}}",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                };

                _notificationRepository.Add(notification);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("消息通知創建成功: 用戶 {UserId}, 發送者: {SenderName}", userId, senderName);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "創建消息通知失敗: 用戶 {UserId}", userId);
                return false;
            }
        }

        public async Task<bool> CreateMarketNotificationAsync(int userId, string itemTitle, string action, string message)
        {
            try
            {
                var notification = new Notification
                {
                    UserId = userId,
                    Type = NotificationType.Market,
                    Title = $"市場商品 {action}",
                    Message = message,
                    Data = $"{{\"itemTitle\":\"{itemTitle}\",\"action\":\"{action}\"}}",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                };

                _notificationRepository.Add(notification);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("市場通知創建成功: 用戶 {UserId}, 商品: {ItemTitle}, 動作: {Action}", 
                    userId, itemTitle, action);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "創建市場通知失敗: 用戶 {UserId}", userId);
                return false;
            }
        }

        public async Task<bool> CreateForumNotificationAsync(int userId, string postTitle, string action, string message)
        {
            try
            {
                var notification = new Notification
                {
                    UserId = userId,
                    Type = NotificationType.Forum,
                    Title = $"論壇貼文 {action}",
                    Message = message,
                    Data = $"{{\"postTitle\":\"{postTitle}\",\"action\":\"{action}\"}}",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                };

                _notificationRepository.Add(notification);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("論壇通知創建成功: 用戶 {UserId}, 貼文: {PostTitle}, 動作: {Action}", 
                    userId, postTitle, action);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "創建論壇通知失敗: 用戶 {UserId}", userId);
                return false;
            }
        }

        public async Task<bool> CreateGameNotificationAsync(int userId, string gameName, string result, string message)
        {
            try
            {
                var notification = new Notification
                {
                    UserId = userId,
                    Type = NotificationType.Game,
                    Title = $"{gameName} 遊戲結果",
                    Message = message,
                    Data = $"{{\"gameName\":\"{gameName}\",\"result\":\"{result}\"}}",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                };

                _notificationRepository.Add(notification);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("遊戲通知創建成功: 用戶 {UserId}, 遊戲: {GameName}, 結果: {Result}", 
                    userId, gameName, result);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "創建遊戲通知失敗: 用戶 {UserId}", userId);
                return false;
            }
        }

        public async Task<bool> CreateBulkNotificationAsync(IEnumerable<int> userIds, string title, string message, 
            NotificationType type = NotificationType.System, string data = null)
        {
            try
            {
                var notifications = userIds.Select(userId => new Notification
                {
                    UserId = userId,
                    Type = type,
                    Title = title,
                    Message = message,
                    Data = data,
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow
                });

                foreach (var notification in notifications)
                {
                    _notificationRepository.Add(notification);
                }

                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("批量通知創建成功: {Count} 個用戶, 標題: {Title}", userIds.Count(), title);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "創建批量通知失敗: {Count} 個用戶", userIds.Count());
                return false;
            }
        }

        public async Task<bool> CreateNotificationSourceAsync(int notificationId, string sourceType, int sourceId, string sourceData)
        {
            try
            {
                var notificationSource = new NotificationSource
                {
                    NotificationId = notificationId,
                    SourceType = sourceType,
                    SourceId = sourceId,
                    SourceData = sourceData,
                    CreatedAt = DateTime.UtcNow
                };

                _notificationSourceRepository.Add(notificationSource);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("通知來源創建成功: 通知 {NotificationId}, 來源: {SourceType} {SourceId}", 
                    notificationId, sourceType, sourceId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "創建通知來源失敗: 通知 {NotificationId}", notificationId);
                return false;
            }
        }

        public async Task<bool> CreateNotificationActionAsync(int notificationId, string actionType, string actionData)
        {
            try
            {
                var notificationAction = new NotificationAction
                {
                    NotificationId = notificationId,
                    ActionType = actionType,
                    ActionData = actionData,
                    CreatedAt = DateTime.UtcNow
                };

                _notificationActionRepository.Add(notificationAction);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("通知動作創建成功: 通知 {NotificationId}, 動作: {ActionType}", 
                    notificationId, actionType);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "創建通知動作失敗: 通知 {NotificationId}", notificationId);
                return false;
            }
        }

        public async Task<IEnumerable<NotificationDto>> GetNotificationsByTypeAsync(int userId, NotificationType type, int page = 1, int pageSize = 20)
        {
            try
            {
                var notifications = await _notificationRepository.GetByUserIdAndTypeAsync(userId, type, page, pageSize);
                return notifications.Select(n => new NotificationDto
                {
                    Id = n.Id,
                    UserId = n.UserId,
                    Type = n.Type,
                    Title = n.Title,
                    Message = n.Message,
                    Data = n.Data,
                    IsRead = n.IsRead,
                    CreatedAt = n.CreatedAt,
                    ReadAt = n.ReadAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "按類型獲取通知失敗: 用戶 {UserId}, 類型 {Type}", userId, type);
                return Enumerable.Empty<NotificationDto>();
            }
        }

        public async Task<bool> DeleteOldNotificationsAsync(int userId, int daysOld)
        {
            try
            {
                var cutoffDate = DateTime.UtcNow.AddDays(-daysOld);
                var oldNotifications = await _notificationRepository.GetOldNotificationsAsync(userId, cutoffDate);

                foreach (var notification in oldNotifications)
                {
                    _notificationRepository.Delete(notification);
                }

                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("舊通知清理成功: 用戶 {UserId}, 刪除 {Count} 個通知", 
                    userId, oldNotifications.Count());
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "清理舊通知失敗: 用戶 {UserId}", userId);
                return false;
            }
        }
    }
}