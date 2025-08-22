using GameCore.Core.Entities;

namespace GameCore.Core.Interfaces
{
    /// <summary>
    /// 通知服務介面
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// 取得使用者的未讀通知
        /// </summary>
        Task<List<Notification>> GetUnreadNotificationsAsync(int userId);

        /// <summary>
        /// 建立通知
        /// </summary>
        Task<Notification> CreateNotificationAsync(int sourceId, int actionId, int senderId, string title, string message, List<int> recipientIds);

        /// <summary>
        /// 標記通知為已讀
        /// </summary>
        Task MarkAsReadAsync(int notificationId, int userId);

        /// <summary>
        /// 發送系統通知給所有使用者
        /// </summary>
        Task SendSystemNotificationAsync(string title, string message);
    }
}

