using GameCore.Core.Entities;

namespace GameCore.Core.Interfaces
{
    /// <summary>
    /// 通知服務介面
    /// </summary>
    public interface INotificationService
    {
        Task<Notification> CreateNotificationAsync(int sourceId, int actionId, int senderId, string title, string message, int? groupId = null);
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId);
        Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(int userId);
        Task<bool> MarkNotificationAsReadAsync(int notificationId, int userId);
        Task<bool> DeleteNotificationAsync(int notificationId);
        Task<int> GetUnreadCountAsync(int userId);
        Task CreatePetColorChangeNotificationAsync(int userId, string skinColor, string backgroundColor, int pointsCost);
    }
}