using GameCore.Core.Entities;
using GameCore.Core.Interfaces;

namespace GameCore.Core.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotificationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Notification> CreateNotificationAsync(int sourceId, int actionId, int senderId, string title, string message, int? groupId = null)
        {
            var notification = new Notification
            {
                SourceId = sourceId,
                ActionId = actionId,
                SenderId = senderId,
                NotificationTitle = title,
                NotificationMessage = message,
                GroupId = groupId,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.NotificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();

            return notification;
        }

        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId)
        {
            var notifications = await _unitOfWork.NotificationRepository.GetAllAsync();
            // 這裡需要根據 NotificationRecipient 來獲取用戶的通知
            // 暫時返回所有通知，實際實現需要更複雜的查詢
            return notifications;
        }

        public async Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(int userId)
        {
            var notifications = await _unitOfWork.NotificationRepository.GetAllAsync();
            // 這裡需要根據 NotificationRecipient 來獲取未讀通知
            // 暫時返回所有通知，實際實現需要更複雜的查詢
            return notifications;
        }

        public async Task<bool> MarkNotificationAsReadAsync(int notificationId, int userId)
        {
            // 這裡需要更新 NotificationRecipient 的 IsRead 狀態
            // 暫時返回 true，實際實現需要更複雜的邏輯
            return true;
        }

        public async Task<bool> DeleteNotificationAsync(int notificationId)
        {
            var notification = await _unitOfWork.NotificationRepository.GetByIdAsync(notificationId);
            if (notification == null) return false;

            await _unitOfWork.NotificationRepository.DeleteByIdAsync(notificationId);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            var notifications = await _unitOfWork.NotificationRepository.GetAllAsync();
            // 這裡需要根據 NotificationRecipient 來計算未讀數量
            // 暫時返回 0，實際實現需要更複雜的查詢
            return 0;
        }

        public async Task CreatePetColorChangeNotificationAsync(int userId, string skinColor, string backgroundColor, int pointsCost)
        {
            // 創建寵物換色通知
            var notification = new Notification
            {
                SourceId = 1, // 假設 1 代表寵物系統
                ActionId = 1, // 假設 1 代表換色動作
                SenderId = userId,
                NotificationTitle = "寵物換色成功",
                NotificationMessage = $"您的寵物已成功換色為 {skinColor} 膚色和 {backgroundColor} 背景色，消耗點數 {pointsCost}",
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.NotificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}