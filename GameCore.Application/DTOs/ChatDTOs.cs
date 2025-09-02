using System.ComponentModel.DataAnnotations;

namespace GameCore.Application.DTOs
{
    /// <summary>
    /// 聊天室回應
    /// </summary>
    public class ChatRoomDto
    {
        /// <summary>
        /// 聊天室 ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 聊天室名稱
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 聊天室類型
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// 創建者 ID
        /// </summary>
        public int CreatorId { get; set; }

        /// <summary>
        /// 創建時間
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 最後活動時間
        /// </summary>
        public DateTime LastActivityAt { get; set; }

        /// <summary>
        /// 成員數量
        /// </summary>
        public int MemberCount { get; set; }

        /// <summary>
        /// 是否公開
        /// </summary>
        public bool IsPublic { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string? Description { get; set; }
    }

    /// <summary>
    /// 聊天訊息回應
    /// </summary>
    public class ChatMessageDto
    {
        /// <summary>
        /// 訊息 ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 聊天室 ID
        /// </summary>
        public int ChatRoomId { get; set; }

        /// <summary>
        /// 發送者 ID
        /// </summary>
        public int SenderId { get; set; }

        /// <summary>
        /// 發送者名稱
        /// </summary>
        public string SenderName { get; set; } = string.Empty;

        /// <summary>
        /// 訊息內容
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// 訊息類型
        /// </summary>
        public string MessageType { get; set; } = "text";

        /// <summary>
        /// 發送時間
        /// </summary>
        public DateTime SentAt { get; set; }

        /// <summary>
        /// 是否已讀
        /// </summary>
        public bool IsRead { get; set; }

        /// <summary>
        /// 附件 URL
        /// </summary>
        public string? AttachmentUrl { get; set; }
    }

    /// <summary>
    /// 私人聊天回應
    /// </summary>
    public class PrivateChatDto
    {
        /// <summary>
        /// 私人聊天 ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用戶 1 ID
        /// </summary>
        public int User1Id { get; set; }

        /// <summary>
        /// 用戶 2 ID
        /// </summary>
        public int User2Id { get; set; }

        /// <summary>
        /// 創建時間
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 最後活動時間
        /// </summary>
        public DateTime LastActivityAt { get; set; }

        /// <summary>
        /// 未讀訊息數量
        /// </summary>
        public int UnreadCount { get; set; }
    }

    /// <summary>
    /// 私人訊息回應
    /// </summary>
    public class PrivateMessageDto
    {
        /// <summary>
        /// 訊息 ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 私人聊天 ID
        /// </summary>
        public int PrivateChatId { get; set; }

        /// <summary>
        /// 發送者 ID
        /// </summary>
        public int SenderId { get; set; }

        /// <summary>
        /// 發送者名稱
        /// </summary>
        public string SenderName { get; set; } = string.Empty;

        /// <summary>
        /// 訊息內容
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// 發送時間
        /// </summary>
        public DateTime SentAt { get; set; }

        /// <summary>
        /// 是否已讀
        /// </summary>
        public bool IsRead { get; set; }
    }

    /// <summary>
    /// 建立聊天室請求
    /// </summary>
    public class CreateChatRoomRequest
    {
        /// <summary>
        /// 聊天室名稱
        /// </summary>
        [Required(ErrorMessage = "聊天室名稱為必填")]
        [StringLength(100, ErrorMessage = "聊天室名稱長度不能超過 100 字元")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 聊天室類型
        /// </summary>
        [Required(ErrorMessage = "聊天室類型為必填")]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// 創建者 ID
        /// </summary>
        [Required(ErrorMessage = "創建者 ID 為必填")]
        public int CreatorId { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(500, ErrorMessage = "描述長度不能超過 500 字元")]
        public string? Description { get; set; }

        /// <summary>
        /// 是否公開
        /// </summary>
        public bool IsPublic { get; set; } = true;
    }

    /// <summary>
    /// 發送訊息請求
    /// </summary>
    public class SendMessageRequest
    {
        /// <summary>
        /// 聊天室 ID
        /// </summary>
        [Required(ErrorMessage = "聊天室 ID 為必填")]
        public int ChatRoomId { get; set; }

        /// <summary>
        /// 發送者 ID
        /// </summary>
        [Required(ErrorMessage = "發送者 ID 為必填")]
        public int SenderId { get; set; }

        /// <summary>
        /// 訊息內容
        /// </summary>
        [Required(ErrorMessage = "訊息內容為必填")]
        [StringLength(1000, ErrorMessage = "訊息內容長度不能超過 1000 字元")]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// 訊息類型
        /// </summary>
        public string MessageType { get; set; } = "text";

        /// <summary>
        /// 附件 URL
        /// </summary>
        public string? AttachmentUrl { get; set; }
    }

    /// <summary>
    /// 加入聊天室請求
    /// </summary>
    public class JoinChatRoomRequest
    {
        /// <summary>
        /// 聊天室 ID
        /// </summary>
        [Required(ErrorMessage = "聊天室 ID 為必填")]
        public int ChatRoomId { get; set; }

        /// <summary>
        /// 用戶 ID
        /// </summary>
        [Required(ErrorMessage = "用戶 ID 為必填")]
        public int UserId { get; set; }
    }
} 