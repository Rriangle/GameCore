using System.ComponentModel.DataAnnotations;

namespace GameCore.Application.DTOs
{
    /// <summary>
    /// 通知回應
    /// </summary>
    public class NotificationResponse
    {
        /// <summary>
        /// 通知 ID
        /// </summary>
        public int Id { get; set; }

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
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// 是否已讀
        /// </summary>
        public bool IsRead { get; set; }

        /// <summary>
        /// 優先級
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// 相關連結
        /// </summary>
        public string? RelatedUrl { get; set; }

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
    /// 通知 DTO (舊版本相容性)
    /// </summary>
    public class NotificationDto : NotificationResponse
    {
    }

    /// <summary>
    /// 發送通知請求
    /// </summary>
    public class SendNotificationRequest
    {
        /// <summary>
        /// 用戶 ID
        /// </summary>
        [Required(ErrorMessage = "用戶 ID 為必填")]
        public int UserId { get; set; }

        /// <summary>
        /// 標題
        /// </summary>
        [Required(ErrorMessage = "標題為必填")]
        [StringLength(200, ErrorMessage = "標題長度不能超過 200 字元")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 內容
        /// </summary>
        [Required(ErrorMessage = "內容為必填")]
        [StringLength(1000, ErrorMessage = "內容長度不能超過 1000 字元")]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// 通知類型
        /// </summary>
        [Required(ErrorMessage = "通知類型為必填")]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// 優先級
        /// </summary>
        [Range(1, 5, ErrorMessage = "優先級必須在 1-5 之間")]
        public int Priority { get; set; } = 3;

        /// <summary>
        /// 相關連結
        /// </summary>
        [Url(ErrorMessage = "相關連結格式不正確")]
        public string? RelatedUrl { get; set; }
    }

    /// <summary>
    /// 批量發送通知請求
    /// </summary>
    public class SendBulkNotificationRequest
    {
        /// <summary>
        /// 用戶 ID 列表
        /// </summary>
        [Required(ErrorMessage = "用戶 ID 列表為必填")]
        public List<int> UserIds { get; set; } = new List<int>();

        /// <summary>
        /// 標題
        /// </summary>
        [Required(ErrorMessage = "標題為必填")]
        [StringLength(200, ErrorMessage = "標題長度不能超過 200 字元")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 內容
        /// </summary>
        [Required(ErrorMessage = "內容為必填")]
        [StringLength(1000, ErrorMessage = "內容長度不能超過 1000 字元")]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// 通知類型
        /// </summary>
        [Required(ErrorMessage = "通知類型為必填")]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// 優先級
        /// </summary>
        [Range(1, 5, ErrorMessage = "優先級必須在 1-5 之間")]
        public int Priority { get; set; } = 3;

        /// <summary>
        /// 相關連結
        /// </summary>
        [Url(ErrorMessage = "相關連結格式不正確")]
        public string? RelatedUrl { get; set; }
    }

    /// <summary>
    /// 標記通知為已讀請求
    /// </summary>
    public class MarkNotificationAsReadRequest
    {
        /// <summary>
        /// 通知 ID
        /// </summary>
        [Required(ErrorMessage = "通知 ID 為必填")]
        public int NotificationId { get; set; }

        /// <summary>
        /// 用戶 ID
        /// </summary>
        [Required(ErrorMessage = "用戶 ID 為必填")]
        public int UserId { get; set; }
    }

    /// <summary>
    /// 刪除通知請求
    /// </summary>
    public class DeleteNotificationRequest
    {
        /// <summary>
        /// 通知 ID
        /// </summary>
        [Required(ErrorMessage = "通知 ID 為必填")]
        public int NotificationId { get; set; }

        /// <summary>
        /// 用戶 ID
        /// </summary>
        [Required(ErrorMessage = "用戶 ID 為必填")]
        public int UserId { get; set; }
    }

    /// <summary>
    /// 通知統計回應
    /// </summary>
    public class NotificationStatisticsResponse
    {
        /// <summary>
        /// 總通知數量
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 未讀通知數量
        /// </summary>
        public int UnreadCount { get; set; }

        /// <summary>
        /// 已讀通知數量
        /// </summary>
        public int ReadCount { get; set; }

        /// <summary>
        /// 高優先級通知數量
        /// </summary>
        public int HighPriorityCount { get; set; }
    }
} 