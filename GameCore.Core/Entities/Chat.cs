using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 聊天室實體
    /// </summary>
    [Table("ChatRooms")]
    public class ChatRoom
    {
        [Key]
        public int RoomId { get; set; }

        [Required]
        public string RoomName { get; set; } = string.Empty;

        [Required]
        public ChatRoomType RoomType { get; set; }

        [Required]
        public int CreatorId { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        // 導航屬性
        [ForeignKey("CreatorId")]
        public virtual User Creator { get; set; } = null!;

        public virtual ICollection<ChatRoomMember> Members { get; set; } = new List<ChatRoomMember>();
        public virtual ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
    }

    /// <summary>
    /// 聊天室成員實體
    /// </summary>
    [Table("ChatRoomMembers")]
    public class ChatRoomMember
    {
        [Key]
        public int MemberId { get; set; }

        [Required]
        public int RoomId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public ChatMemberRole Role { get; set; }

        [Required]
        public DateTime JoinedAt { get; set; }

        public DateTime? LastReadAt { get; set; }

        [Required]
        public bool IsActive { get; set; }

        // 導航屬性
        [ForeignKey("RoomId")]
        public virtual ChatRoom Room { get; set; } = null!;

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }

    /// <summary>
    /// 聊天訊息實體
    /// </summary>
    [Table("ChatMessages")]
    public class ChatMessage
    {
        [Key]
        public int MessageId { get; set; }

        [Required]
        public int RoomId { get; set; }

        [Required]
        public int SenderId { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        public ChatMessageType MessageType { get; set; }

        [Required]
        public DateTime SentAt { get; set; }

        public DateTime? ReadAt { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        // 導航屬性
        [ForeignKey("RoomId")]
        public virtual ChatRoom Room { get; set; } = null!;

        [ForeignKey("SenderId")]
        public virtual User Sender { get; set; } = null!;
    }

    /// <summary>
    /// 私人聊天實體
    /// </summary>
    [Table("PrivateChats")]
    public class PrivateChat
    {
        [Key]
        public int ChatId { get; set; }

        [Required]
        public int User1Id { get; set; }

        [Required]
        public int User2Id { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        // 導航屬性
        [ForeignKey("User1Id")]
        public virtual User User1 { get; set; } = null!;

        [ForeignKey("User2Id")]
        public virtual User User2 { get; set; } = null!;

        public virtual ICollection<PrivateMessage> Messages { get; set; } = new List<PrivateMessage>();
    }

    /// <summary>
    /// 私人訊息實體
    /// </summary>
    [Table("PrivateMessages")]
    public class PrivateMessage
    {
        [Key]
        public int MessageId { get; set; }

        [Required]
        public int ChatId { get; set; }

        [Required]
        public int SenderId { get; set; }

        [Required]
        public int ReceiverId { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        public ChatMessageType MessageType { get; set; }

        [Required]
        public DateTime SentAt { get; set; }

        public DateTime? ReadAt { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        // 導航屬性
        [ForeignKey("ChatId")]
        public virtual PrivateChat Chat { get; set; } = null!;

        [ForeignKey("SenderId")]
        public virtual User Sender { get; set; } = null!;

        [ForeignKey("ReceiverId")]
        public virtual User Receiver { get; set; } = null!;
    }

    /// <summary>
    /// 聊天室類型列舉
    /// </summary>
    public enum ChatRoomType
    {
        Private = 1,       // 私人聊天
        Group = 2,         // 群組聊天
        Support = 3        // 客服聊天
    }

    /// <summary>
    /// 聊天室成員角色列舉
    /// </summary>
    public enum ChatMemberRole
    {
        Member = 1,        // 一般成員
        Moderator = 2,     // 管理員
        Admin = 3          // 超級管理員
    }

    /// <summary>
    /// 聊天訊息類型列舉
    /// </summary>
    public enum ChatMessageType
    {
        Text = 1,          // 文字訊息
        Image = 2,         // 圖片訊息
        File = 3,          // 檔案訊息
        System = 4         // 系統訊息
    }
}