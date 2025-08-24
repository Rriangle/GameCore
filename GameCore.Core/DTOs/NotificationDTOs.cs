using System.ComponentModel.DataAnnotations;

namespace GameCore.Core.DTOs
{
    /// <summary>
    /// 通知 DTO
    /// </summary>
    public class NotificationDto
    {
        public int NotificationId { get; set; }
        public string SourceName { get; set; } = string.Empty;
        public string ActionName { get; set; } = string.Empty;
        public int SenderId { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public int? SenderManagerId { get; set; }
        public string? SenderManagerName { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int? GroupId { get; set; }
        public string? GroupName { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadAt { get; set; }
        public string NotificationType { get; set; } = string.Empty;
        public string Priority { get; set; } = "Normal";
        public Dictionary<string, object> Data { get; set; } = new();
    }

    /// <summary>
    /// 通知創建請求 DTO
    /// </summary>
    public class NotificationCreateDto
    {
        [Required(ErrorMessage = "通知標題不能為空")]
        [StringLength(200, ErrorMessage = "通知標題長度不能超過200個字符")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "通知內容不能為空")]
        [StringLength(1000, ErrorMessage = "通知內容長度不能超過1000個字符")]
        public string Message { get; set; } = string.Empty;

        [Required(ErrorMessage = "通知類型不能為空")]
        public string NotificationType { get; set; } = string.Empty;

        public string Priority { get; set; } = "Normal";

        public List<int> RecipientIds { get; set; } = new();

        public Dictionary<string, object> Data { get; set; } = new();
    }

    /// <summary>
    /// 通知創建結果 DTO
    /// </summary>
    public class NotificationCreateResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<NotificationDto> Notifications { get; set; } = new();
    }

    /// <summary>
    /// 通知來源 DTO
    /// </summary>
    public class NotificationSourceDto
    {
        public int SourceId { get; set; }
        public string SourceName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// 通知動作 DTO
    /// </summary>
    public class NotificationActionDto
    {
        public int ActionId { get; set; }
        public string ActionName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// 通知統計 DTO
    /// </summary>
    public class NotificationStatsDto
    {
        public int TotalCount { get; set; }
        public int UnreadCount { get; set; }
        public int ReadCount { get; set; }
        public int TodayCount { get; set; }
        public int WeekCount { get; set; }
        public Dictionary<string, int> TypeCounts { get; set; } = new();
    }

    /// <summary>
    /// 通知設定 DTO
    /// </summary>
    public class NotificationSettingDto
    {
        public int UserId { get; set; }
        public bool EmailEnabled { get; set; }
        public bool PushEnabled { get; set; }
        public bool SmsEnabled { get; set; }
        public Dictionary<string, bool> TypeSettings { get; set; } = new();
    }

    /// <summary>
    /// 批量通知請求 DTO
    /// </summary>
    public class BulkNotificationDto
    {
        [Required(ErrorMessage = "通知標題不能為空")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "通知內容不能為空")]
        public string Message { get; set; } = string.Empty;

        [Required(ErrorMessage = "通知類型不能為空")]
        public string NotificationType { get; set; } = string.Empty;

        public string Priority { get; set; } = "Normal";

        [Required(ErrorMessage = "收件人不能為空")]
        [MinLength(1, ErrorMessage = "至少需要一個收件人")]
        public List<int> RecipientIds { get; set; } = new();

        public Dictionary<string, object> Data { get; set; } = new();
        
        public DateTime? ScheduledAt { get; set; }
    }
}