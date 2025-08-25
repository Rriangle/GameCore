namespace GameCore.Core.DTOs
{
    /// <summary>
    /// 聊天室 DTO
    /// </summary>
    public class ChatRoomDto
    {
        public int RoomId { get; set; }
        public string Name { get; set; } = string.Empty;
        public ChatRoomType Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int MemberCount { get; set; }
        public int UnreadCount { get; set; }
        public List<ChatMemberDto> Members { get; set; } = new List<ChatMemberDto>();
    }

    /// <summary>
    /// 聊天室建立 DTO
    /// </summary>
    public class ChatRoomCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public ChatRoomType Type { get; set; }
        public List<int> MemberIds { get; set; } = new List<int>();
    }

    /// <summary>
    /// 聊天室建立結果
    /// </summary>
    public class ChatRoomCreateResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public ChatRoomDto? Room { get; set; }
    }

    /// <summary>
    /// 聊天室更新 DTO
    /// </summary>
    public class ChatRoomUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
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
        public int MessageId { get; set; }
        public int RoomId { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public ChatMessageType MessageType { get; set; }
        public DateTime SentAt { get; set; }
        public DateTime? ReadAt { get; set; }
        public bool IsDeleted { get; set; }
    }

    /// <summary>
    /// 聊天訊息建立 DTO
    /// </summary>
    public class ChatMessageCreateDto
    {
        public int RoomId { get; set; }
        public string Content { get; set; } = string.Empty;
        public ChatMessageType MessageType { get; set; } = ChatMessageType.Text;
    }

    /// <summary>
    /// 聊天訊息建立結果
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
        public DateTime UpdatedAt { get; set; }
        public int UnreadCount { get; set; }
        public PrivateMessageDto? LastMessage { get; set; }
    }

    /// <summary>
    /// 私人聊天建立結果
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
        public ChatMessageType MessageType { get; set; }
        public DateTime SentAt { get; set; }
        public DateTime? ReadAt { get; set; }
    }

    /// <summary>
    /// 私人訊息建立 DTO
    /// </summary>
    public class PrivateMessageCreateDto
    {
        public int ReceiverId { get; set; }
        public string Content { get; set; } = string.Empty;
        public ChatMessageType MessageType { get; set; } = ChatMessageType.Text;
    }

    /// <summary>
    /// 私人訊息建立結果
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
        public ChatMemberRole Role { get; set; }
        public DateTime JoinedAt { get; set; }
        public DateTime? LastSeenAt { get; set; }
    }

    /// <summary>
    /// 聊天室類型
    /// </summary>
    public enum ChatRoomType
    {
        Private = 1,
        Group = 2
    }

    /// <summary>
    /// 聊天訊息類型
    /// </summary>
    public enum ChatMessageType
    {
        Text = 1,
        Image = 2,
        File = 3,
        System = 4
    }

    /// <summary>
    /// 聊天成員角色
    /// </summary>
    public enum ChatMemberRole
    {
        Member = 1,
        Moderator = 2,
        Admin = 3
    }
}