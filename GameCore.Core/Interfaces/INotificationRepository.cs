using GameCore.Core.Entities;

namespace GameCore.Core.Interfaces
{
    /// <summary>
    /// 通知 Repository 介面
    /// </summary>
    public interface INotificationRepository : IRepository<Notification>
    {
        /// <summary>
        /// 取得未讀通知數量
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns></returns>
        Task<int> GetUnreadCountAsync(int userId);

        /// <summary>
        /// 取得用戶通知
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁數量</param>
        /// <returns></returns>
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId, int page, int pageSize);

        /// <summary>
        /// 標記通知為已讀
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="notificationId">通知ID</param>
        /// <returns></returns>
        Task MarkAsReadAsync(int userId, int notificationId);

        /// <summary>
        /// 標記所有通知為已讀
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns></returns>
        Task MarkAllAsReadAsync(int userId);

        /// <summary>
        /// 創建寵物換色通知
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="oldColor">舊顏色</param>
        /// <param name="newColor">新顏色</param>
        /// <param name="cost">花費</param>
        /// <returns></returns>
        Task CreatePetColorChangeNotificationAsync(int userId, string oldColor, string newColor, int cost);

        /// <summary>
        /// 創建簽到通知
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="points">獲得點數</param>
        /// <param name="experience">獲得經驗</param>
        /// <param name="streak">連續簽到天數</param>
        /// <returns></returns>
        Task CreateSignInNotificationAsync(int userId, int points, int experience, int streak);
    }
}
