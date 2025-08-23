using GameCore.Core.Entities;

namespace GameCore.Core.Interfaces
{
    /// <summary>
    /// 通知倉庫介面
    /// </summary>
    public interface INotificationRepository : IRepository<Notification>
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
        /// 建立通知
        /// </summary>
        /// <param name="notification">通知</param>
        /// <returns>操作結果</returns>
        Task<bool> CreateNotificationAsync(Notification notification);

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
    }

    /// <summary>
    /// 通知來源倉庫介面
    /// </summary>
    public interface INotificationSourceRepository : IRepository<NotificationSource>
    {
        /// <summary>
        /// 取得所有通知來源
        /// </summary>
        /// <returns>通知來源列表</returns>
        Task<IEnumerable<NotificationSource>> GetAllSourcesAsync();

        /// <summary>
        /// 取得通知來源
        /// </summary>
        /// <param name="sourceName">來源名稱</param>
        /// <returns>通知來源</returns>
        Task<NotificationSource?> GetSourceByNameAsync(string sourceName);
    }

    /// <summary>
    /// 通知動作倉庫介面
    /// </summary>
    public interface INotificationActionRepository : IRepository<NotificationAction>
    {
        /// <summary>
        /// 取得所有通知動作
        /// </summary>
        /// <returns>通知動作列表</returns>
        Task<IEnumerable<NotificationAction>> GetAllActionsAsync();

        /// <summary>
        /// 取得通知動作
        /// </summary>
        /// <param name="actionName">動作名稱</param>
        /// <returns>通知動作</returns>
        Task<NotificationAction?> GetActionByNameAsync(string actionName);
    }
}