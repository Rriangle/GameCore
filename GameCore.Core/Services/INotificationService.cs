using GameCore.Core.Entities;

namespace GameCore.Core.Services
{
    /// <summary>
    /// 通知服務介面
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// 取得使用者的通知
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>通知列表</returns>
        Task<IEnumerable<Notification>> GetNotificationsByUserAsync(int userId, int page, int pageSize);

        /// <summary>
        /// 取得未讀通知數量
        /// </summary>
        /// <summary>
        /// 取得未讀通知數量
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <returns>未讀通知數量</returns>
        Task<int> GetUnreadCountAsync(int userId);

        /// <summary>
        /// 標記通知為已讀
        /// </summary>
        /// <param name="notificationId">通知ID</param>
        /// <returns>操作結果</returns>
        Task<bool> MarkAsReadAsync(int notificationId);

        /// <summary>
        /// 標記所有通知為已讀
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <returns>操作結果</returns>
        Task<bool> MarkAllAsReadAsync(int userId);

        /// <summary>
        /// 建立系統通知
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="title">標題</param>
        /// <param name="content">內容</param>
        /// <param name="source">來源</param>
        /// <param name="action">動作</param>
        /// <returns>操作結果</returns>
        Task<bool> CreateSystemNotificationAsync(int userId, string title, string content, string source, string action);

        /// <summary>
        /// 建立簽到通知
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="pointsEarned">獲得點數</param>
        /// <param name="experienceEarned">獲得經驗</param>
        /// <returns>操作結果</returns>
        Task<bool> CreateSignInNotificationAsync(int userId, int pointsEarned, int experienceEarned);

        /// <summary>
        /// 建立寵物互動通知
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="petName">寵物名稱</param>
        /// <param name="interactionType">互動類型</param>
        /// <returns>操作結果</returns>
        Task<bool> CreatePetInteractionNotificationAsync(int userId, string petName, string interactionType);

        /// <summary>
        /// 建立訂單通知
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="orderId">訂單ID</param>
        /// <param name="orderStatus">訂單狀態</param>
        /// <returns>操作結果</returns>
        Task<bool> CreateOrderNotificationAsync(int userId, int orderId, string orderStatus);

        /// <summary>
        /// 建立訊息通知
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="senderName">發送者名稱</param>
        /// <param name="messageType">訊息類型</param>
        /// <returns>操作結果</returns>
        Task<bool> CreateMessageNotificationAsync(int userId, string senderName, string messageType);

        /// <summary>
        /// 刪除通知
        /// </summary>
        /// <param name="notificationId">通知ID</param>
        /// <param name="userId">使用者ID</param>
        /// <returns>操作結果</returns>
        Task<bool> DeleteNotificationAsync(int notificationId, int userId);

        /// <summary>
        /// 取得通知來源
        /// </summary>
        /// <returns>通知來源列表</returns>
        Task<IEnumerable<NotificationSource>> GetNotificationSourcesAsync();

        /// <summary>
        /// 取得通知動作
        /// </summary>
        /// <returns>通知動作列表</returns>
        Task<IEnumerable<NotificationAction>> GetNotificationActionsAsync();
    }
}