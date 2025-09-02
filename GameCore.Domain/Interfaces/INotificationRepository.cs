using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// 通知資料存取介面
    /// </summary>
    public interface INotificationRepository
    {
        /// <summary>
        /// 根據ID取得通知
        /// </summary>
        Task<Notification?> GetByIdAsync(int id);

        /// <summary>
        /// 根據用戶ID取得通知列表
        /// </summary>
        Task<IEnumerable<Notification>> GetByUserIdAsync(int userId);

        /// <summary>
        /// 新增通知
        /// </summary>
        Task<Notification> AddAsync(Notification notification);

        /// <summary>
        /// 更新通知
        /// </summary>
        Task<Notification> UpdateAsync(Notification notification);

        /// <summary>
        /// 刪除通知
        /// </summary>
        Task DeleteAsync(int id);

        /// <summary>
        /// 創建寵物換色通知
        /// </summary>
        Task CreatePetColorChangeNotificationAsync(int userId, string oldColor, string newColor);

        /// <summary>
        /// 根據用戶和動作取得通知
        /// </summary>
        Task<Notification?> GetByUserAndActionAsync(int userId, string action);

        /// <summary>
        /// 標記所有通知為已讀
        /// </summary>
        Task MarkAllAsReadAsync(int userId);

        /// <summary>
        /// 取得未讀通知數量
        /// </summary>
        Task<int> GetUnreadCountAsync(int userId);
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
