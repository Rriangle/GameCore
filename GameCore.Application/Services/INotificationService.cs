using GameCore.Application.Common;
using GameCore.Application.DTOs;

namespace GameCore.Application.Services
{
    /// <summary>
    /// 通知服務介面
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// 取得用戶通知列表
        /// </summary>
        /// <param name="userId">用戶 ID</param>
        /// <param name="pageNumber">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>通知列表</returns>
        Task<Result<PagedResult<NotificationResponse>>> GetUserNotificationsAsync(int userId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);

        /// <summary>
        /// 發送通知
        /// </summary>
        /// <param name="request">發送通知請求</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>發送結果</returns>
        Task<Result<NotificationResponse>> SendNotificationAsync(SendNotificationRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// 標記通知為已讀
        /// </summary>
        /// <param name="notificationId">通知 ID</param>
        /// <param name="userId">用戶 ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>標記結果</returns>
        Task<OperationResult> MarkAsReadAsync(int notificationId, int userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// 標記所有通知為已讀
        /// </summary>
        /// <param name="userId">用戶 ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>標記結果</returns>
        Task<OperationResult> MarkAllAsReadAsync(int userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// 刪除通知
        /// </summary>
        /// <param name="notificationId">通知 ID</param>
        /// <param name="userId">用戶 ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>刪除結果</returns>
        Task<OperationResult> DeleteNotificationAsync(int notificationId, int userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// 取得未讀通知數量
        /// </summary>
        /// <param name="userId">用戶 ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>未讀通知數量</returns>
        Task<Result<int>> GetUnreadCountAsync(int userId, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// 通知回應
    /// </summary>
    public class NotificationResponse
    {
        /// <summary>
        /// 通知 ID
        /// </summary>
        public int NotificationId { get; set; }

        /// <summary>
        /// 用戶 ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 標題
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 內容
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// 通知類型
        /// </summary>
        public string NotificationType { get; set; } = string.Empty;

        /// <summary>
        /// 優先級
        /// </summary>
        public string Priority { get; set; } = string.Empty;

        /// <summary>
        /// 是否已讀
        /// </summary>
        public bool IsRead { get; set; }

        /// <summary>
        /// 相關資料
        /// </summary>
        public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 讀取時間
        /// </summary>
        public DateTime? ReadAt { get; set; }
    }

    /// <summary>
    /// 發送通知請求
    /// </summary>
    public class SendNotificationRequest
    {
        /// <summary>
        /// 用戶 ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 標題
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 內容
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// 通知類型
        /// </summary>
        public string NotificationType { get; set; } = string.Empty;

        /// <summary>
        /// 優先級
        /// </summary>
        public string Priority { get; set; } = "Normal";

        /// <summary>
        /// 相關資料
        /// </summary>
        public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();
    }
} 