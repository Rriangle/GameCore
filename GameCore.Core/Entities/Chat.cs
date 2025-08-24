using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 聊天室表 (對應 ChatRooms)
    /// </summary>
    [Table("ChatRooms")]
    public class ChatRoom
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [StringLength(20)]
        public string Type { get; set; } = "Group"; // Group, Private

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdateTime { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual ICollection<ChatRoomMember> Members { get; set; } = new List<ChatRoomMember>();
        public virtual ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
    }

    /// <summary>
    /// 聊天室成員表 (對應 ChatRoomMembers)
    /// </summary>
    [Table("ChatRoomMembers")]
    public class ChatRoomMember
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ChatRoomId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime JoinTime { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime LastReadTime { get; set; } = DateTime.UtcNow;

        [Required]
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual ChatRoom ChatRoom { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }

    /// <summary>
    /// 聊天訊息表 (對應 ChatMessages)
    /// </summary>
    [Table("ChatMessages")]
    public class ChatMessage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ChatRoomId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Type { get; set; } = "Text"; // Text, Image, File

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual ChatRoom ChatRoom { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }

    /// <summary>
    /// 私聊表 (對應 PrivateChats)
    /// </summary>
    [Table("PrivateChats")]
    public class PrivateChat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int User1Id { get; set; }

        [Required]
        public int User2Id { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdateTime { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual User User1 { get; set; } = null!;
        public virtual User User2 { get; set; } = null!;
        public virtual ICollection<PrivateMessage> Messages { get; set; } = new List<PrivateMessage>();
    }

    /// <summary>
    /// 私聊訊息表 (對應 PrivateMessages)
    /// </summary>
    [Table("PrivateMessages")]
    public class PrivateMessage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int PrivateChatId { get; set; }

        [Required]
        public int SenderId { get; set; }

        [Required]
        public int ReceiverId { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        public bool IsRead { get; set; } = false;

        public DateTime? ReadTime { get; set; }

        [Required]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual PrivateChat PrivateChat { get; set; } = null!;
        public virtual User Sender { get; set; } = null!;
        public virtual User Receiver { get; set; } = null!;
    }
}