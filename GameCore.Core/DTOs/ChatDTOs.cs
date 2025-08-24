using System.ComponentModel.DataAnnotations;
using GameCore.Core.Enums;

namespace GameCore.Core.DTOs
{
    /// <summary>
    /// 聊天室 DTO
    /// </summary>
    public class ChatRoomDto
    {
        public int RoomId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int MemberCount { get; set; }
        public DateTime? LastMessageAt { get; set; }
        public string? LastMessageContent { get; set; }
        public string? LastMessageSender { get; set; }
        public int UnreadCount { get; set; }
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// 聊天室創建請求 DTO
    /// </summary>
    public class ChatRoomCreateDto
    {
        [Required(ErrorMessage = "聊天室名稱不能為空")]
        [StringLength(100, ErrorMessage = "聊天室名稱長度不能超過100個字符")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "聊天室類型不能為空")]
        public string Type { get; set; } = string.Empty;

        public List<int> MemberIds { get; set; } = new();
    }

    /// <summary>
    /// 聊天室創建結果 DTO
    /// </summary>
    public class ChatRoomCreateResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public ChatRoomDto? Room { get; set; }
    }

    /// <summary>
    /// 聊天室更新請求 DTO
    /// </summary>
    public class ChatRoomUpdateDto
    {
        [StringLength(100, ErrorMessage = "聊天室名稱長度不能超過100個字符")]
        public string? Name { get; set; }

        public bool? IsActive { get; set; }
    }

    /// <summary>
    /// 聊天室更新結果 DTO
    /// </summary>
    public class ChatRoomUpdateResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public ChatRoomDto? Room { get; set; }
    }

    /// <summary>
    /// 聊天訊息 DTO
    /// </summary>
    public class ChatMessageDto
    {
        public int MessageId { get; set; }
        public int RoomId { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string MessageType { get; set; } = string.Empty;
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
        public bool IsSent { get; set; }
        public DateTime? ReadAt { get; set; }
    }

    /// <summary>
    /// 聊天訊息創建請求 DTO
    /// </summary>
    public class ChatMessageCreateDto
    {
        [Required(ErrorMessage = "訊息內容不能為空")]
        [StringLength(2000, ErrorMessage = "訊息內容長度不能超過2000個字符")]
        public string Content { get; set; } = string.Empty;

        public string MessageType { get; set; } = "Text";
    }

    /// <summary>
    /// 聊天訊息創建結果 DTO
    /// </summary>
    public class ChatMessageCreateResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public ChatMessageDto? ChatMessage { get; set; }
    }

    /// <summary>
    /// 私人聊天 DTO
    /// </summary>
    public class PrivateChatDto
    {
        public int ChatId { get; set; }
        public int User1Id { get; set; }
        public string User1Name { get; set; } = string.Empty;
        public int User2Id { get; set; }
        public string User2Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? LastMessageAt { get; set; }
        public string? LastMessageContent { get; set; }
        public int UnreadCount { get; set; }
        public bool IsBlocked { get; set; }
    }

    /// <summary>
    /// 私人聊天創建結果 DTO
    /// </summary>
    public class PrivateChatCreateResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public PrivateChatDto? Chat { get; set; }
    }

    /// <summary>
    /// 私人訊息 DTO
    /// </summary>
    public class PrivateMessageDto
    {
        public int MessageId { get; set; }
        public int ChatId { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public int ReceiverId { get; set; }
        public string ReceiverName { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string MessageType { get; set; } = string.Empty;
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
        public bool IsSent { get; set; }
        public DateTime? ReadAt { get; set; }
    }

    /// <summary>
    /// 私人訊息創建請求 DTO
    /// </summary>
    public class PrivateMessageCreateDto
    {
        [Required(ErrorMessage = "訊息內容不能為空")]
        [StringLength(2000, ErrorMessage = "訊息內容長度不能超過2000個字符")]
        public string Content { get; set; } = string.Empty;

        public string MessageType { get; set; } = "Text";
    }

    /// <summary>
    /// 私人訊息創建結果 DTO
    /// </summary>
    public class PrivateMessageCreateResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public PrivateMessageDto? PrivateMessage { get; set; }
    }

    /// <summary>
    /// 聊天成員 DTO
    /// </summary>
    public class ChatMemberDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime JoinedAt { get; set; }
        public bool IsOnline { get; set; }
        public DateTime? LastSeenAt { get; set; }
    }

    /// <summary>
    /// 私聊實體（用於接口兼容）
    /// </summary>
    public class PrivateChat
    {
        public int ChatId { get; set; }
        public int User1Id { get; set; }
        public int User2Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastMessageAt { get; set; }
        public string? LastMessageContent { get; set; }
        public bool IsBlocked { get; set; }
    }
}