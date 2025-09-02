using System;
using System.Collections.Generic;

namespace GameCore.Core.DTOs
{
    /// <summary>
    /// 聊天室 DTO
    /// </summary>
    public class ChatRoomDto
    {
        public int RoomId { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public string RoomType { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<int> MemberIds { get; set; } = new List<int>();
        public int UnreadCount { get; set; }
        public DateTime? LastMessageTime { get; set; }
    }

    /// <summary>
    /// 創建聊天室 DTO
    /// </summary>
    public class ChatRoomCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int CreatedBy { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public string RoomType { get; set; } = string.Empty;
        public List<int> MemberIds { get; set; } = new List<int>();
    }

    /// <summary>
    /// 聊天室創建結果
    /// </summary>
    public class ChatRoomCreateResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? ChatRoomId { get; set; }
        public int? RoomId { get; set; }
        public ChatRoomDto? Room { get; set; }
    }

    /// <summary>
    /// 更新聊天室 DTO
    /// </summary>
    public class ChatRoomUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string RoomName { get; set; } = string.Empty;
    }

    /// <summary>
    /// 聊天室更新結果
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
        public int Id { get; set; }
        public int ChatRoomId { get; set; }
        public int MessageId { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string MessageType { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public DateTime SentAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
    }

    /// <summary>
    /// 創建聊天訊息 DTO
    /// </summary>
    public class ChatMessageCreateDto
    {
        public int ChatRoomId { get; set; }
        public int SenderId { get; set; }
        public string Content { get; set; } = string.Empty;
        public string MessageType { get; set; } = "Text";
        public string Type { get; set; } = "text";
    }

    /// <summary>
    /// 聊天訊息創建結果
    /// </summary>
    public class ChatMessageCreateResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? MessageId { get; set; }
        public ChatMessageDto? ChatMessage { get; set; }
    }

    /// <summary>
    /// 私人聊天 DTO
    /// </summary>
    public class PrivateChatDto
    {
        public int Id { get; set; }
        public int ChatId { get; set; }
        public int User1Id { get; set; }
        public int User2Id { get; set; }
        public string User1Name { get; set; } = string.Empty;
        public string User2Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? LastMessageAt { get; set; }
        public DateTime? LastMessageTime { get; set; }
        public int UnreadCount { get; set; }
    }

    /// <summary>
    /// 私人聊天創建結果
    /// </summary>
    public class PrivateChatCreateResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? PrivateChatId { get; set; }
        public int? ChatId { get; set; }
        public PrivateChatDto? Chat { get; set; }
    }

    /// <summary>
    /// 私人訊息 DTO
    /// </summary>
    public class PrivateMessageDto
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public int PrivateChatId { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string MessageType { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public DateTime SentAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ReadAt { get; set; }
        public bool IsRead { get; set; }
    }

    /// <summary>
    /// 創建私人訊息 DTO
    /// </summary>
    public class PrivateMessageCreateDto
    {
        public int PrivateChatId { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Content { get; set; } = string.Empty;
        public string MessageType { get; set; } = "Text";
        public string Type { get; set; } = "text";
    }

    /// <summary>
    /// 私人訊息創建結果
    /// </summary>
    public class PrivateMessageCreateResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? MessageId { get; set; }
        public PrivateMessageDto? PrivateMessage { get; set; }
    }


} 
