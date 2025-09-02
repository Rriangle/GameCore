using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Domain.Entities
{
    /// <summary>
    /// ?�知來�?�?
    /// </summary>
    [Table("Notification_Sources")]
    public partial class NotificationSource
    {
        /// <summary>
        /// 來�?類�?編�? (主鍵)
        /// </summary>
        [Key]
        [Column("source_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SourceId { get; set; }

        /// <summary>
        /// 來�??�稱
        /// </summary>
        [Column("source_name")]
        [StringLength(100)]
        public string? SourceName { get; set; }

        // 導航屬�?
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }

    /// <summary>
    /// ?�知行為�?
    /// </summary>
    [Table("Notification_Actions")]
    public partial class NotificationAction
    {
        /// <summary>
        /// 行為類?編? (主鍵)
        /// </summary>
        [Key]
        [Column("action_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ActionId { get; set; }

        /// <summary>
        /// 行為?�稱
        /// </summary>
        [Column("action_name")]
        [StringLength(100)]
        public string? ActionName { get; set; }

        // 導航屬�?
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }

    /// <summary>
    /// ?�知主表
    /// </summary>
    [Table("Notifications")]
    public partial class Notification
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public int Type { get; set; }
        public string Source { get; set; } = string.Empty;
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReadAt { get; set; }

        // Navigation properties
        public User User { get; set; } = null!;
    }

    /// <summary>
    /// ?�知?�收?�表
    /// </summary>
    [Table("Notification_Recipients")]
    public class NotificationRecipient
    {
        /// <summary>
        /// ?�收紀?�編??(主鍵)
        /// </summary>
        [Key]
        [Column("recipient_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RecipientId { get; set; }

        /// <summary>
        /// ?�知編�? (外鍵??Notifications)
        /// </summary>
        [Required]
        [Column("notification_id")]
        [ForeignKey("Notification")]
        public int NotificationId { get; set; }

        /// <summary>
        /// 使用?�編??(外鍵??Users)
        /// </summary>
        [Required]
        [Column("user_id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// ?�否已�?
        /// </summary>
        [Required]
        [Column("is_read")]
        public bool IsRead { get; set; } = false;

        /// <summary>
        /// 已�??��?
        /// </summary>
        [Column("read_at")]
        public DateTime? ReadAt { get; set; }

        // 導航屬�?
        public virtual Notification Notification { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }



    /// <summary>
    /// 群�?�?
    /// </summary>
    [Table("Groups")]
    public class Group
    {
        /// <summary>
        /// 群�?編�? (主鍵)
        /// </summary>
        [Key]
        [Column("group_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GroupId { get; set; }

        /// <summary>
        /// 群�??�稱
        /// </summary>
        [Column("group_name")]
        [StringLength(100)]
        public string? GroupName { get; set; }

        /// <summary>
        /// 建�??�編??(外鍵??Users)
        /// </summary>
        [Column("created_by")]
        [ForeignKey("CreatedByUser")]
        public int? CreatedBy { get; set; }

        /// <summary>
        /// 建�??��?
        /// </summary>
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        // 導航屬�?
        public virtual User? CreatedByUser { get; set; }
        public virtual ICollection<GroupMember> Members { get; set; } = new List<GroupMember>();
        public virtual ICollection<GroupChat> GroupChats { get; set; } = new List<GroupChat>();
        public virtual ICollection<GroupBlock> GroupBlocks { get; set; } = new List<GroupBlock>();
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }

    /// <summary>
    /// 群�??�員�?
    /// </summary>
    [Table("Group_Member")]
    public class GroupMember
    {
        /// <summary>
        /// 群�?編�? (複�?主鍵, 外鍵??Groups)
        /// </summary>
        [Column("group_id")]
        [ForeignKey("Group")]
        public int GroupId { get; set; }

        /// <summary>
        /// 使用?�編??(複�?主鍵, 外鍵??Users)
        /// </summary>
        [Column("user_id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// ?�入?��?
        /// </summary>
        [Column("joined_at")]
        public DateTime? JoinedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// ?�否?�管?�員
        /// </summary>
        [Required]
        [Column("is_admin")]
        public bool IsAdmin { get; set; } = false;

        // 導航屬�?
        public virtual Group Group { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }

    /// <summary>
    /// 群�?專用?�天�?
    /// </summary>
    [Table("Group_Chat")]
    public class GroupChat
    {
        /// <summary>
        /// 群�??�天編�? (主鍵)
        /// </summary>
        [Key]
        [Column("group_chat_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GroupChatId { get; set; }

        /// <summary>
        /// 群�?編�? (外鍵??Groups)
        /// </summary>
        [Column("group_id")]
        [ForeignKey("Group")]
        public int? GroupId { get; set; }

        /// <summary>
        /// ?�送者編??(外鍵??Users)
        /// </summary>
        [Column("sender_id")]
        [ForeignKey("SenderUser")]
        public int? SenderId { get; set; }

        /// <summary>
        /// 訊息?�容
        /// </summary>
        [Column("group_chat_content")]
        [StringLength(1000)]
        public string? GroupChatContent { get; set; }

        /// <summary>
        /// ?�送�???
        /// </summary>
        [Column("sent_at")]
        public DateTime? SentAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// ?�否寄�?
        /// </summary>
        [Required]
        [Column("is_sent")]
        public bool IsSent { get; set; } = true;

        // 導航屬�?
        public virtual Group? Group { get; set; }
        public virtual User? SenderUser { get; set; }
    }

    /// <summary>
    /// 封�?�?(群�?專用)
    /// </summary>
    [Table("Group_Block")]
    public class GroupBlock
    {
        /// <summary>
        /// 封�?編�? (主鍵)
        /// </summary>
        [Key]
        [Column("block_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BlockId { get; set; }

        /// <summary>
        /// 群�?編�? (外鍵??Groups)
        /// </summary>
        [Column("group_id")]
        [ForeignKey("Group")]
        public int GroupId { get; set; }

        /// <summary>
        /// 被�??�者編??(外鍵??Users)
        /// </summary>
        [Column("user_id")]
        [ForeignKey("BlockedUser")]
        public int UserId { get; set; }

        /// <summary>
        /// 封�??�編??(外鍵??Users)
        /// </summary>
        [Column("blocked_by")]
        [ForeignKey("BlockedByUser")]
        public int BlockedBy { get; set; }

        /// <summary>
        /// 建�??��?
        /// </summary>
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        // 導航屬�?
        public virtual Group Group { get; set; } = null!;
        public virtual User BlockedUser { get; set; } = null!;
        public virtual User BlockedByUser { get; set; } = null!;
    }
}
