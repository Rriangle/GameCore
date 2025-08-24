using System.ComponentModel.DataAnnotations;

namespace GameCore.Core.DTOs
{
    #region 通知系統 DTOs

    /// <summary>
    /// 通知 DTO
    /// </summary>
    public class NotificationDto
    {
        /// <summary>通知ID</summary>
        public int NotificationId { get; set; }

        /// <summary>來源類型ID</summary>
        public int SourceId { get; set; }

        /// <summary>來源類型名稱</summary>
        public string? SourceName { get; set; }

        /// <summary>行為類型ID</summary>
        public int ActionId { get; set; }

        /// <summary>行為類型名稱</summary>
        public string? ActionName { get; set; }

        /// <summary>發送者ID</summary>
        public int SenderId { get; set; }

        /// <summary>發送者名稱</summary>
        public string? SenderName { get; set; }

        /// <summary>發送者暱稱</summary>
        public string? SenderNickname { get; set; }

        /// <summary>管理員發送者ID</summary>
        public int? SenderManagerId { get; set; }

        /// <summary>管理員發送者名稱</summary>
        public string? SenderManagerName { get; set; }

        /// <summary>通知標題</summary>
        public string? NotificationTitle { get; set; }

        /// <summary>通知內容</summary>
        public string? NotificationMessage { get; set; }

        /// <summary>建立時間</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>群組ID (群組相關通知)</summary>
        public int? GroupId { get; set; }

        /// <summary>群組名稱</summary>
        public string? GroupName { get; set; }

        /// <summary>是否已讀</summary>
        public bool IsRead { get; set; }

        /// <summary>已讀時間</summary>
        public DateTime? ReadAt { get; set; }

        /// <summary>收件人ID</summary>
        public int RecipientId { get; set; }

        /// <summary>通知類型顯示</summary>
        public string NotificationTypeDisplay => $"{SourceName} - {ActionName}";

        /// <summary>時間顯示 (相對時間)</summary>
        public string TimeDisplay
        {
            get
            {
                var timeSpan = DateTime.UtcNow - CreatedAt;
                if (timeSpan.TotalMinutes < 1) return "剛剛";
                if (timeSpan.TotalMinutes < 60) return $"{(int)timeSpan.TotalMinutes}分鐘前";
                if (timeSpan.TotalHours < 24) return $"{(int)timeSpan.TotalHours}小時前";
                if (timeSpan.TotalDays < 7) return $"{(int)timeSpan.TotalDays}天前";
                return CreatedAt.ToString("MM/dd HH:mm");
            }
        }
    }

    /// <summary>
    /// 建立通知請求 DTO
    /// </summary>
    public class CreateNotificationDto
    {
        /// <summary>來源類型ID</summary>
        [Required(ErrorMessage = "來源類型為必填")]
        public int SourceId { get; set; }

        /// <summary>行為類型ID</summary>
        [Required(ErrorMessage = "行為類型為必填")]
        public int ActionId { get; set; }

        /// <summary>發送者ID</summary>
        public int? SenderId { get; set; }

        /// <summary>管理員發送者ID</summary>
        public int? SenderManagerId { get; set; }

        /// <summary>通知標題</summary>
        [StringLength(200, ErrorMessage = "通知標題長度不能超過200字元")]
        public string? NotificationTitle { get; set; }

        /// <summary>通知內容</summary>
        [Required(ErrorMessage = "通知內容為必填")]
        [StringLength(1000, ErrorMessage = "通知內容長度不能超過1000字元")]
        public string NotificationMessage { get; set; } = string.Empty;

        /// <summary>群組ID (群組相關通知)</summary>
        public int? GroupId { get; set; }

        /// <summary>收件人ID列表</summary>
        [Required(ErrorMessage = "收件人列表為必填")]
        public List<int> RecipientIds { get; set; } = new();
    }

    /// <summary>
    /// 通知來源 DTO
    /// </summary>
    public class NotificationSourceDto
    {
        /// <summary>來源ID</summary>
        public int SourceId { get; set; }

        /// <summary>來源名稱</summary>
        public string? SourceName { get; set; }
    }

    /// <summary>
    /// 通知行為 DTO
    /// </summary>
    public class NotificationActionDto
    {
        /// <summary>行為ID</summary>
        public int ActionId { get; set; }

        /// <summary>行為名稱</summary>
        public string? ActionName { get; set; }
    }

    /// <summary>
    /// 標記已讀請求 DTO
    /// </summary>
    public class MarkNotificationReadDto
    {
        /// <summary>通知ID列表</summary>
        [Required(ErrorMessage = "通知ID列表為必填")]
        public List<int> NotificationIds { get; set; } = new();
    }

    /// <summary>
    /// 通知統計 DTO
    /// </summary>
    public class NotificationStatsDto
    {
        /// <summary>總通知數</summary>
        public int TotalCount { get; set; }

        /// <summary>未讀通知數</summary>
        public int UnreadCount { get; set; }

        /// <summary>今日通知數</summary>
        public int TodayCount { get; set; }

        /// <summary>本週通知數</summary>
        public int WeekCount { get; set; }

        /// <summary>按來源統計</summary>
        public List<NotificationSourceStatsDto> SourceStats { get; set; } = new();

        /// <summary>按行為統計</summary>
        public List<NotificationActionStatsDto> ActionStats { get; set; } = new();
    }

    /// <summary>
    /// 通知來源統計 DTO
    /// </summary>
    public class NotificationSourceStatsDto
    {
        /// <summary>來源名稱</summary>
        public string? SourceName { get; set; }

        /// <summary>通知數量</summary>
        public int Count { get; set; }

        /// <summary>未讀數量</summary>
        public int UnreadCount { get; set; }
    }

    /// <summary>
    /// 通知行為統計 DTO
    /// </summary>
    public class NotificationActionStatsDto
    {
        /// <summary>行為名稱</summary>
        public string? ActionName { get; set; }

        /// <summary>通知數量</summary>
        public int Count { get; set; }

        /// <summary>未讀數量</summary>
        public int UnreadCount { get; set; }
    }

    #endregion

    #region 聊天系統 DTOs

    /// <summary>
    /// 聊天訊息 DTO
    /// </summary>
    public class ChatMessageDto
    {
        /// <summary>訊息ID</summary>
        public int MessageId { get; set; }

        /// <summary>管理員ID (客服)</summary>
        public int? ManagerId { get; set; }

        /// <summary>管理員名稱</summary>
        public string? ManagerName { get; set; }

        /// <summary>發送者ID</summary>
        public int SenderId { get; set; }

        /// <summary>發送者名稱</summary>
        public string? SenderName { get; set; }

        /// <summary>發送者暱稱</summary>
        public string? SenderNickname { get; set; }

        /// <summary>接收者ID</summary>
        public int? ReceiverId { get; set; }

        /// <summary>接收者名稱</summary>
        public string? ReceiverName { get; set; }

        /// <summary>接收者暱稱</summary>
        public string? ReceiverNickname { get; set; }

        /// <summary>訊息內容</summary>
        public string ChatContent { get; set; } = string.Empty;

        /// <summary>發送時間</summary>
        public DateTime SentAt { get; set; }

        /// <summary>是否已讀</summary>
        public bool IsRead { get; set; }

        /// <summary>是否寄送成功</summary>
        public bool IsSent { get; set; }

        /// <summary>訊息類型</summary>
        public string MessageType => ManagerId.HasValue ? "客服" : "私訊";

        /// <summary>時間顯示</summary>
        public string TimeDisplay
        {
            get
            {
                var timeSpan = DateTime.UtcNow - SentAt;
                if (timeSpan.TotalMinutes < 1) return "剛剛";
                if (timeSpan.TotalMinutes < 60) return $"{(int)timeSpan.TotalMinutes}分鐘前";
                if (timeSpan.TotalHours < 24) return $"{(int)timeSpan.TotalHours}小時前";
                return SentAt.ToString("MM/dd HH:mm");
            }
        }
    }

    /// <summary>
    /// 發送聊天訊息請求 DTO
    /// </summary>
    public class SendChatMessageDto
    {
        /// <summary>接收者ID (私訊時)</summary>
        public int? ReceiverId { get; set; }

        /// <summary>管理員ID (客服時)</summary>
        public int? ManagerId { get; set; }

        /// <summary>訊息內容</summary>
        [Required(ErrorMessage = "訊息內容為必填")]
        [StringLength(1000, ErrorMessage = "訊息內容長度不能超過1000字元")]
        public string ChatContent { get; set; } = string.Empty;
    }

    /// <summary>
    /// 聊天對話 DTO
    /// </summary>
    public class ChatConversationDto
    {
        /// <summary>對話對象ID</summary>
        public int PartnerId { get; set; }

        /// <summary>對話對象名稱</summary>
        public string? PartnerName { get; set; }

        /// <summary>對話對象暱稱</summary>
        public string? PartnerNickname { get; set; }

        /// <summary>對話對象頭像</summary>
        public string? PartnerAvatar { get; set; }

        /// <summary>最後訊息</summary>
        public ChatMessageDto? LastMessage { get; set; }

        /// <summary>未讀訊息數</summary>
        public int UnreadCount { get; set; }

        /// <summary>最後活動時間</summary>
        public DateTime? LastActivityAt { get; set; }

        /// <summary>對話類型</summary>
        public string ConversationType { get; set; } = "private";
    }

    /// <summary>
    /// 標記聊天已讀請求 DTO
    /// </summary>
    public class MarkChatReadDto
    {
        /// <summary>對話對象ID</summary>
        [Required(ErrorMessage = "對話對象ID為必填")]
        public int PartnerId { get; set; }

        /// <summary>最後已讀訊息ID</summary>
        public int? LastReadMessageId { get; set; }
    }

    #endregion

    #region 群組系統 DTOs

    /// <summary>
    /// 群組 DTO
    /// </summary>
    public class GroupDto
    {
        /// <summary>群組ID</summary>
        public int GroupId { get; set; }

        /// <summary>群組名稱</summary>
        public string? GroupName { get; set; }

        /// <summary>建立者ID</summary>
        public int? CreatedBy { get; set; }

        /// <summary>建立者名稱</summary>
        public string? CreatedByName { get; set; }

        /// <summary>建立者暱稱</summary>
        public string? CreatedByNickname { get; set; }

        /// <summary>建立時間</summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>成員數量</summary>
        public int MemberCount { get; set; }

        /// <summary>是否為群組管理員</summary>
        public bool IsAdmin { get; set; }

        /// <summary>是否為群組成員</summary>
        public bool IsMember { get; set; }

        /// <summary>最後活動時間</summary>
        public DateTime? LastActivityAt { get; set; }

        /// <summary>最後訊息</summary>
        public GroupChatDto? LastMessage { get; set; }

        /// <summary>未讀訊息數</summary>
        public int UnreadCount { get; set; }

        /// <summary>群組成員列表</summary>
        public List<GroupMemberDto> Members { get; set; } = new();
    }

    /// <summary>
    /// 群組成員 DTO
    /// </summary>
    public class GroupMemberDto
    {
        /// <summary>群組ID</summary>
        public int GroupId { get; set; }

        /// <summary>使用者ID</summary>
        public int UserId { get; set; }

        /// <summary>使用者名稱</summary>
        public string? UserName { get; set; }

        /// <summary>使用者暱稱</summary>
        public string? UserNickname { get; set; }

        /// <summary>使用者頭像</summary>
        public string? UserAvatar { get; set; }

        /// <summary>加入時間</summary>
        public DateTime? JoinedAt { get; set; }

        /// <summary>是否為管理員</summary>
        public bool IsAdmin { get; set; }

        /// <summary>最後活動時間</summary>
        public DateTime? LastActivityAt { get; set; }

        /// <summary>角色顯示</summary>
        public string RoleDisplay => IsAdmin ? "管理員" : "成員";
    }

    /// <summary>
    /// 建立群組請求 DTO
    /// </summary>
    public class CreateGroupDto
    {
        /// <summary>群組名稱</summary>
        [Required(ErrorMessage = "群組名稱為必填")]
        [StringLength(100, ErrorMessage = "群組名稱長度不能超過100字元")]
        public string GroupName { get; set; } = string.Empty;

        /// <summary>初始成員ID列表</summary>
        public List<int> InitialMemberIds { get; set; } = new();
    }

    /// <summary>
    /// 更新群組請求 DTO
    /// </summary>
    public class UpdateGroupDto
    {
        /// <summary>群組名稱</summary>
        [StringLength(100, ErrorMessage = "群組名稱長度不能超過100字元")]
        public string? GroupName { get; set; }
    }

    /// <summary>
    /// 加入群組請求 DTO
    /// </summary>
    public class JoinGroupDto
    {
        /// <summary>群組ID</summary>
        [Required(ErrorMessage = "群組ID為必填")]
        public int GroupId { get; set; }
    }

    /// <summary>
    /// 邀請加入群組請求 DTO
    /// </summary>
    public class InviteToGroupDto
    {
        /// <summary>群組ID</summary>
        [Required(ErrorMessage = "群組ID為必填")]
        public int GroupId { get; set; }

        /// <summary>被邀請者ID列表</summary>
        [Required(ErrorMessage = "被邀請者列表為必填")]
        public List<int> UserIds { get; set; } = new();
    }

    /// <summary>
    /// 移除群組成員請求 DTO
    /// </summary>
    public class RemoveFromGroupDto
    {
        /// <summary>群組ID</summary>
        [Required(ErrorMessage = "群組ID為必填")]
        public int GroupId { get; set; }

        /// <summary>被移除者ID</summary>
        [Required(ErrorMessage = "被移除者ID為必填")]
        public int UserId { get; set; }
    }

    /// <summary>
    /// 設定群組管理員請求 DTO
    /// </summary>
    public class SetGroupAdminDto
    {
        /// <summary>群組ID</summary>
        [Required(ErrorMessage = "群組ID為必填")]
        public int GroupId { get; set; }

        /// <summary>使用者ID</summary>
        [Required(ErrorMessage = "使用者ID為必填")]
        public int UserId { get; set; }

        /// <summary>是否設為管理員</summary>
        [Required]
        public bool IsAdmin { get; set; }
    }

    #endregion

    #region 群組聊天 DTOs

    /// <summary>
    /// 群組聊天 DTO
    /// </summary>
    public class GroupChatDto
    {
        /// <summary>群組聊天ID</summary>
        public int GroupChatId { get; set; }

        /// <summary>群組ID</summary>
        public int? GroupId { get; set; }

        /// <summary>群組名稱</summary>
        public string? GroupName { get; set; }

        /// <summary>發送者ID</summary>
        public int? SenderId { get; set; }

        /// <summary>發送者名稱</summary>
        public string? SenderName { get; set; }

        /// <summary>發送者暱稱</summary>
        public string? SenderNickname { get; set; }

        /// <summary>發送者頭像</summary>
        public string? SenderAvatar { get; set; }

        /// <summary>訊息內容</summary>
        public string? GroupChatContent { get; set; }

        /// <summary>發送時間</summary>
        public DateTime? SentAt { get; set; }

        /// <summary>是否寄送成功</summary>
        public bool IsSent { get; set; }

        /// <summary>是否為發送者</summary>
        public bool IsSender { get; set; }

        /// <summary>時間顯示</summary>
        public string TimeDisplay
        {
            get
            {
                if (!SentAt.HasValue) return "";
                var timeSpan = DateTime.UtcNow - SentAt.Value;
                if (timeSpan.TotalMinutes < 1) return "剛剛";
                if (timeSpan.TotalMinutes < 60) return $"{(int)timeSpan.TotalMinutes}分鐘前";
                if (timeSpan.TotalHours < 24) return $"{(int)timeSpan.TotalHours}小時前";
                return SentAt.Value.ToString("MM/dd HH:mm");
            }
        }
    }

    /// <summary>
    /// 發送群組聊天請求 DTO
    /// </summary>
    public class SendGroupChatDto
    {
        /// <summary>群組ID</summary>
        [Required(ErrorMessage = "群組ID為必填")]
        public int GroupId { get; set; }

        /// <summary>訊息內容</summary>
        [Required(ErrorMessage = "訊息內容為必填")]
        [StringLength(1000, ErrorMessage = "訊息內容長度不能超過1000字元")]
        public string GroupChatContent { get; set; } = string.Empty;
    }

    #endregion

    #region 封鎖系統 DTOs

    /// <summary>
    /// 群組封鎖 DTO
    /// </summary>
    public class GroupBlockDto
    {
        /// <summary>封鎖ID</summary>
        public int BlockId { get; set; }

        /// <summary>群組ID</summary>
        public int GroupId { get; set; }

        /// <summary>群組名稱</summary>
        public string? GroupName { get; set; }

        /// <summary>被封鎖者ID</summary>
        public int UserId { get; set; }

        /// <summary>被封鎖者名稱</summary>
        public string? UserName { get; set; }

        /// <summary>被封鎖者暱稱</summary>
        public string? UserNickname { get; set; }

        /// <summary>封鎖者ID</summary>
        public int BlockedBy { get; set; }

        /// <summary>封鎖者名稱</summary>
        public string? BlockedByName { get; set; }

        /// <summary>封鎖者暱稱</summary>
        public string? BlockedByNickname { get; set; }

        /// <summary>建立時間</summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>封鎖原因</summary>
        public string? BlockReason { get; set; }
    }

    /// <summary>
    /// 封鎖使用者請求 DTO
    /// </summary>
    public class BlockUserDto
    {
        /// <summary>群組ID</summary>
        [Required(ErrorMessage = "群組ID為必填")]
        public int GroupId { get; set; }

        /// <summary>被封鎖者ID</summary>
        [Required(ErrorMessage = "被封鎖者ID為必填")]
        public int UserId { get; set; }

        /// <summary>封鎖原因</summary>
        [StringLength(200, ErrorMessage = "封鎖原因長度不能超過200字元")]
        public string? BlockReason { get; set; }
    }

    /// <summary>
    /// 解除封鎖請求 DTO
    /// </summary>
    public class UnblockUserDto
    {
        /// <summary>群組ID</summary>
        [Required(ErrorMessage = "群組ID為必填")]
        public int GroupId { get; set; }

        /// <summary>被解除封鎖者ID</summary>
        [Required(ErrorMessage = "被解除封鎖者ID為必填")]
        public int UserId { get; set; }
    }

    #endregion

    #region 分頁結果

    /// <summary>
    /// 社交分頁結果 DTO
    /// </summary>
    /// <typeparam name="T">資料類型</typeparam>
    public class SocialPagedResult<T>
    {
        /// <summary>當前頁碼</summary>
        public int Page { get; set; }

        /// <summary>每頁筆數</summary>
        public int PageSize { get; set; }

        /// <summary>總筆數</summary>
        public int TotalCount { get; set; }

        /// <summary>總頁數</summary>
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        /// <summary>是否有上一頁</summary>
        public bool HasPreviousPage => Page > 1;

        /// <summary>是否有下一頁</summary>
        public bool HasNextPage => Page < TotalPages;

        /// <summary>資料列表</summary>
        public List<T> Data { get; set; } = new();

        /// <summary>是否為空結果</summary>
        public bool IsEmpty => !Data.Any();
    }

    #endregion

    #region 服務結果

    /// <summary>
    /// 社交服務執行結果
    /// </summary>
    public class SocialServiceResult
    {
        /// <summary>是否成功</summary>
        public bool Success { get; set; }

        /// <summary>訊息</summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>錯誤清單</summary>
        public List<string> Errors { get; set; } = new();

        /// <summary>建立成功結果</summary>
        public static SocialServiceResult CreateSuccess(string message = "操作成功")
        {
            return new SocialServiceResult { Success = true, Message = message };
        }

        /// <summary>建立失敗結果</summary>
        public static SocialServiceResult CreateFailure(string message, List<string>? errors = null)
        {
            return new SocialServiceResult 
            { 
                Success = false, 
                Message = message, 
                Errors = errors ?? new List<string>() 
            };
        }
    }

    /// <summary>
    /// 帶資料的社交服務執行結果
    /// </summary>
    /// <typeparam name="T">資料類型</typeparam>
    public class SocialServiceResult<T> : SocialServiceResult
    {
        /// <summary>結果資料</summary>
        public T? Data { get; set; }

        /// <summary>建立成功結果</summary>
        public static SocialServiceResult<T> CreateSuccess(T data, string message = "操作成功")
        {
            return new SocialServiceResult<T> 
            { 
                Success = true, 
                Message = message, 
                Data = data 
            };
        }

        /// <summary>建立失敗結果</summary>
        public static new SocialServiceResult<T> CreateFailure(string message, List<string>? errors = null)
        {
            return new SocialServiceResult<T> 
            { 
                Success = false, 
                Message = message, 
                Errors = errors ?? new List<string>() 
            };
        }
    }

    #endregion
}