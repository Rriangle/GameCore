using GameCore.Core.DTOs;
using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameCore.Core.Services
{
    /// <summary>
    /// 通知服務實現類
    /// 提供通知相關的業務邏輯處理
    /// </summary>
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(
            INotificationRepository notificationRepository,
            IUserRepository userRepository,
            ILogger<NotificationService> logger)
        {
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        /// <summary>
        /// 創建通知
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="title">通知標題</param>
        /// <param name="message">通知內容</param>
        /// <param name="type">通知類型</param>
        /// <returns>創建結果</returns>
        public async Task<bool> CreateNotificationAsync(int userId, string title, string message, string type = "general")
        {
            try
            {
                _logger.LogInformation("創建通知，用戶ID: {UserId}, 標題: {Title}, 類型: {Type}", userId, title, type);
                
                // 檢查用戶是否存在
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning("用戶不存在，無法創建通知，用戶ID: {UserId}", userId);
                    return false;
                }
                
                var result = await _notificationRepository.CreateNotificationAsync(userId, title, message, type);
                
                if (result)
                {
                    _logger.LogInformation("成功創建通知，用戶ID: {UserId}, 標題: {Title}", userId, title);
                }
                else
                {
                    _logger.LogWarning("創建通知失敗，用戶ID: {UserId}, 標題: {Title}", userId, title);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "創建通知時發生錯誤，用戶ID: {UserId}, 標題: {Title}", userId, title);
                throw;
            }
        }

        /// <summary>
        /// 獲取用戶通知列表
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <param name="includeRead">是否包含已讀通知</param>
        /// <returns>通知列表</returns>
        public async Task<PagedResult<Notification>> GetUserNotificationsAsync(int userId, int page = 1, int pageSize = 20, bool includeRead = true)
        {
            try
            {
                _logger.LogInformation("獲取用戶通知列表，用戶ID: {UserId}, 頁碼: {Page}, 每頁大小: {PageSize}, 包含已讀: {IncludeRead}", 
                    userId, page, pageSize, includeRead);
                
                var notifications = await _notificationRepository.GetUserNotificationsAsync(userId, page, pageSize, includeRead);
                
                _logger.LogInformation("成功獲取用戶通知列表，用戶ID: {UserId}, 共 {TotalCount} 條通知", 
                    userId, notifications.TotalCount);
                return notifications;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取用戶通知列表時發生錯誤，用戶ID: {UserId}", userId);
                throw;
            }
        }

        /// <summary>
        /// 標記通知為已讀
        /// </summary>
        /// <param name="notificationId">通知ID</param>
        /// <param name="userId">用戶ID</param>
        /// <returns>更新結果</returns>
        public async Task<bool> MarkAsReadAsync(int notificationId, int userId)
        {
            try
            {
                _logger.LogInformation("標記通知為已讀，通知ID: {NotificationId}, 用戶ID: {UserId}", notificationId, userId);
                
                var result = await _notificationRepository.MarkAsReadAsync(notificationId, userId);
                
                if (result)
                {
                    _logger.LogInformation("成功標記通知為已讀，通知ID: {NotificationId}, 用戶ID: {UserId}", notificationId, userId);
                }
                else
                {
                    _logger.LogWarning("標記通知為已讀失敗，通知ID: {NotificationId}, 用戶ID: {UserId}", notificationId, userId);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "標記通知為已讀時發生錯誤，通知ID: {NotificationId}, 用戶ID: {UserId}", notificationId, userId);
                throw;
            }
        }

        /// <summary>
        /// 標記所有通知為已讀
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns>更新結果</returns>
        public async Task<bool> MarkAllAsReadAsync(int userId)
        {
            try
            {
                _logger.LogInformation("標記所有通知為已讀，用戶ID: {UserId}", userId);
                
                var result = await _notificationRepository.MarkAllAsReadAsync(userId);
                
                if (result)
                {
                    _logger.LogInformation("成功標記所有通知為已讀，用戶ID: {UserId}", userId);
                }
                else
                {
                    _logger.LogWarning("標記所有通知為已讀失敗，用戶ID: {UserId}", userId);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "標記所有通知為已讀時發生錯誤，用戶ID: {UserId}", userId);
                throw;
            }
        }

        /// <summary>
        /// 刪除通知
        /// </summary>
        /// <param name="notificationId">通知ID</param>
        /// <param name="userId">用戶ID</param>
        /// <returns>刪除結果</returns>
        public async Task<bool> DeleteNotificationAsync(int notificationId, int userId)
        {
            try
            {
                _logger.LogInformation("刪除通知，通知ID: {NotificationId}, 用戶ID: {UserId}", notificationId, userId);
                
                var result = await _notificationRepository.DeleteNotificationAsync(notificationId, userId);
                
                if (result)
                {
                    _logger.LogInformation("成功刪除通知，通知ID: {NotificationId}, 用戶ID: {UserId}", notificationId, userId);
                }
                else
                {
                    _logger.LogWarning("刪除通知失敗，通知ID: {NotificationId}, 用戶ID: {UserId}", notificationId, userId);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "刪除通知時發生錯誤，通知ID: {NotificationId}, 用戶ID: {UserId}", notificationId, userId);
                throw;
            }
        }

        /// <summary>
        /// 獲取未讀通知數量
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns>未讀通知數量</returns>
        public async Task<int> GetUnreadCountAsync(int userId)
        {
            try
            {
                _logger.LogInformation("獲取未讀通知數量，用戶ID: {UserId}", userId);
                
                var count = await _notificationRepository.GetUnreadCountAsync(userId);
                
                _logger.LogInformation("成功獲取未讀通知數量，用戶ID: {UserId}, 數量: {Count}", userId, count);
                return count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取未讀通知數量時發生錯誤，用戶ID: {UserId}", userId);
                throw;
            }
        }

        /// <summary>
        /// 創建寵物顏色變化通知
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="petName">寵物名稱</param>
        /// <param name="oldColor">舊顏色</param>
        /// <param name="newColor">新顏色</param>
        /// <returns>創建結果</returns>
        public async Task<bool> CreatePetColorChangeNotificationAsync(int userId, string petName, string oldColor, string newColor)
        {
            try
            {
                _logger.LogInformation("創建寵物顏色變化通知，用戶ID: {UserId}, 寵物名稱: {PetName}, 顏色變化: {OldColor} -> {NewColor}", 
                    userId, petName, oldColor, newColor);
                
                var result = await _notificationRepository.CreatePetColorChangeNotificationAsync(userId, petName, oldColor, newColor);
                
                if (result)
                {
                    _logger.LogInformation("成功創建寵物顏色變化通知，用戶ID: {UserId}, 寵物名稱: {PetName}", userId, petName);
                }
                else
                {
                    _logger.LogWarning("創建寵物顏色變化通知失敗，用戶ID: {UserId}, 寵物名稱: {PetName}", userId, petName);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "創建寵物顏色變化通知時發生錯誤，用戶ID: {UserId}, 寵物名稱: {PetName}", userId, petName);
                throw;
            }
        }

        /// <summary>
        /// 創建系統公告
        /// </summary>
        /// <param name="title">公告標題</param>
        /// <param name="message">公告內容</param>
        /// <param name="type">公告類型</param>
        /// <returns>創建結果</returns>
        public async Task<bool> CreateSystemAnnouncementAsync(string title, string message, string type = "announcement")
        {
            try
            {
                _logger.LogInformation("創建系統公告，標題: {Title}, 類型: {Type}", title, type);
                
                // 獲取所有用戶
                var users = await _userRepository.GetAllAsync();
                var successCount = 0;
                
                foreach (var user in users)
                {
                    try
                    {
                        var result = await _notificationRepository.CreateNotificationAsync(user.UserId, title, message, type);
                        if (result)
                        {
                            successCount++;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "為用戶 {UserId} 創建系統公告時發生錯誤", user.UserId);
                    }
                }
                
                _logger.LogInformation("成功創建系統公告，標題: {Title}, 成功發送給 {SuccessCount}/{TotalCount} 個用戶", 
                    title, successCount, users.Count);
                
                return successCount > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "創建系統公告時發生錯誤，標題: {Title}", title);
                throw;
            }
        }

        /// <summary>
        /// 獲取通知統計資料
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns>通知統計資料</returns>
        public async Task<NotificationStats> GetNotificationStatsAsync(int userId)
        {
            try
            {
                _logger.LogInformation("獲取通知統計資料，用戶ID: {UserId}", userId);
                
                var unreadCount = await _notificationRepository.GetUnreadCountAsync(userId);
                var totalCount = await _notificationRepository.GetTotalCountAsync(userId);
                
                var stats = new NotificationStats
                {
                    UserId = userId,
                    UnreadCount = unreadCount,
                    TotalCount = totalCount,
                    ReadCount = totalCount - unreadCount
                };
                
                _logger.LogInformation("成功獲取通知統計資料，用戶ID: {UserId}, 未讀: {UnreadCount}, 總計: {TotalCount}", 
                    userId, unreadCount, totalCount);
                return stats;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取通知統計資料時發生錯誤，用戶ID: {UserId}", userId);
                throw;
            }
        }

        /// <summary>
        /// 清理過期通知
        /// </summary>
        /// <param name="daysToKeep">保留天數</param>
        /// <returns>清理結果</returns>
        public async Task<bool> CleanupExpiredNotificationsAsync(int daysToKeep = 30)
        {
            try
            {
                _logger.LogInformation("清理過期通知，保留天數: {DaysToKeep}", daysToKeep);
                
                var result = await _notificationRepository.CleanupExpiredNotificationsAsync(daysToKeep);
                
                if (result)
                {
                    _logger.LogInformation("成功清理過期通知，保留天數: {DaysToKeep}", daysToKeep);
                }
                else
                {
                    _logger.LogWarning("清理過期通知失敗，保留天數: {DaysToKeep}", daysToKeep);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "清理過期通知時發生錯誤，保留天數: {DaysToKeep}", daysToKeep);
                throw;
            }
        }
    }

    /// <summary>
    /// 通知統計資料 DTO
    /// </summary>
    public class NotificationStats
    {
        public int UserId { get; set; }
        public int UnreadCount { get; set; }
        public int ReadCount { get; set; }
        public int TotalCount { get; set; }
    }
}
