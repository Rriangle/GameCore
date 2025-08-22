using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 通知來源表
    /// </summary>
    [Table("Notification_Sources")]
    public class NotificationSource
    {
        /// <summary>
        /// 來源類型編號 (主鍵)
        /// </summary>
        [Key]
        [Column("source_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SourceId { get; set; }

        /// <summary>
        /// 來源名稱
        /// </summary>
        [Column("source_name")]
        [StringLength(100)]
        public string? SourceName { get; set; }

        // 導航屬性
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }

    /// <summary>
    /// 通知行為表
    /// </summary>
    [Table("Notification_Actions")]
    public class NotificationAction
    {
        /// <summary>
        /// 行為類型編號 (主鍵)
        /// </summary>
        [Key]
        [Column("action_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ActionId { get; set; }

        /// <summary>
        /// 行為名稱
        /// </summary>
        [Column("action_name")]
        [StringLength(100)]
        public string? ActionName { get; set; }

        // 導航屬性
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }

    /// <summary>
    /// 通知主表
    /// </summary>
    [Table("Notifications")]
    public class Notification
    {
        /// <summary>
        /// 通知編號 (主鍵)
        /// </summary>
        [Key]
        [Column("notification_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotificationId { get; set; }

        /// <summary>
        /// 來源類型編號 (外鍵到 Notification_Sources)
        /// </summary>
        [Required]
        [Column("source_id")]
        [ForeignKey("NotificationSource")]
        public int SourceId { get; set; }

        /// <summary>
        /// 行為類型編號 (外鍵到 Notification_Actions)
        /// </summary>
        [Required]
        [Column("action_id")]
        [ForeignKey("NotificationAction")]
        public int ActionId { get; set; }

        /// <summary>
        /// 發送者編號 (外鍵到 Users)
        /// </summary>
        [Required]
        [Column("sender_id")]
        [ForeignKey("SenderUser")]
        public int SenderId { get; set; }

        /// <summary>
        /// 發送者編號 (管理員, 外鍵到 ManagerData)
        /// </summary>
        [Column("sender_manager_id")]
        [ForeignKey("SenderManager")]
        public int? SenderManagerId { get; set; }

        /// <summary>
        /// 通知標題
        /// </summary>
        [Column("notification_title")]
        [StringLength(200)]
        public string? NotificationTitle { get; set; }

        /// <summary>
        /// 通知內容
        /// </summary>
        [Column("notification_message")]
        [StringLength(1000)]
        public string? NotificationMessage { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 群組編號 (若為群組相關, 外鍵到 Groups)
        /// </summary>
        [Column("group_id")]
        [ForeignKey("Group")]
        public int? GroupId { get; set; }

        // 導航屬性
        public virtual NotificationSource NotificationSource { get; set; } = null!;
        public virtual NotificationAction NotificationAction { get; set; } = null!;
        public virtual User SenderUser { get; set; } = null!;
        public virtual ManagerData? SenderManager { get; set; }
        public virtual Group? Group { get; set; }
        public virtual ICollection<NotificationRecipient> Recipients { get; set; } = new List<NotificationRecipient>();
    }

    /// <summary>
    /// 通知接收者表
    /// </summary>
    [Table("Notification_Recipients")]
    public class NotificationRecipient
    {
        /// <summary>
        /// 接收紀錄編號 (主鍵)
        /// </summary>
        [Key]
        [Column("recipient_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RecipientId { get; set; }

        /// <summary>
        /// 通知編號 (外鍵到 Notifications)
        /// </summary>
        [Required]
        [Column("notification_id")]
        [ForeignKey("Notification")]
        public int NotificationId { get; set; }

        /// <summary>
        /// 使用者編號 (外鍵到 Users)
        /// </summary>
        [Required]
        [Column("user_id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// 是否已讀
        /// </summary>
        [Required]
        [Column("is_read")]
        public bool IsRead { get; set; } = false;

        /// <summary>
        /// 已讀時間
        /// </summary>
        [Column("read_at")]
        public DateTime? ReadAt { get; set; }

        // 導航屬性
        public virtual Notification Notification { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }

    /// <summary>
    /// 聊天訊息表
    /// </summary>
    [Table("Chat_Message")]
    public class ChatMessage
    {
        /// <summary>
        /// 訊息編號 (主鍵)
        /// </summary>
        [Key]
        [Column("message_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MessageId { get; set; }

        /// <summary>
        /// 管理員編號 (客服, 外鍵到 ManagerData)
        /// </summary>
        [Column("manager_id")]
        [ForeignKey("ManagerData")]
        public int? ManagerId { get; set; }

        /// <summary>
        /// 發送者編號 (外鍵到 Users)
        /// </summary>
        [Required]
        [Column("sender_id")]
        [ForeignKey("SenderUser")]
        public int SenderId { get; set; }

        /// <summary>
        /// 接收者編號 (外鍵到 Users)
        /// </summary>
        [Column("receiver_id")]
        [ForeignKey("ReceiverUser")]
        public int? ReceiverId { get; set; }

        /// <summary>
        /// 訊息內容
        /// </summary>
        [Required]
        [Column("chat_content")]
        [StringLength(1000)]
        public string ChatContent { get; set; } = string.Empty;

        /// <summary>
        /// 發送時間
        /// </summary>
        [Required]
        [Column("sent_at")]
        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 是否已讀
        /// </summary>
        [Required]
        [Column("is_read")]
        public bool IsRead { get; set; } = false;

        /// <summary>
        /// 是否寄送
        /// </summary>
        [Required]
        [Column("is_sent")]
        public bool IsSent { get; set; } = true;

        // 導航屬性
        public virtual ManagerData? ManagerData { get; set; }
        public virtual User SenderUser { get; set; } = null!;
        public virtual User? ReceiverUser { get; set; }
    }

    /// <summary>
    /// 群組表
    /// </summary>
    [Table("Groups")]
    public class Group
    {
        /// <summary>
        /// 群組編號 (主鍵)
        /// </summary>
        [Key]
        [Column("group_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GroupId { get; set; }

        /// <summary>
        /// 群組名稱
        /// </summary>
        [Column("group_name")]
        [StringLength(100)]
        public string? GroupName { get; set; }

        /// <summary>
        /// 建立者編號 (外鍵到 Users)
        /// </summary>
        [Column("created_by")]
        [ForeignKey("CreatedByUser")]
        public int? CreatedBy { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        // 導航屬性
        public virtual User? CreatedByUser { get; set; }
        public virtual ICollection<GroupMember> Members { get; set; } = new List<GroupMember>();
        public virtual ICollection<GroupChat> GroupChats { get; set; } = new List<GroupChat>();
        public virtual ICollection<GroupBlock> GroupBlocks { get; set; } = new List<GroupBlock>();
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }

    /// <summary>
    /// 群組成員表
    /// </summary>
    [Table("Group_Member")]
    public class GroupMember
    {
        /// <summary>
        /// 群組編號 (複合主鍵, 外鍵到 Groups)
        /// </summary>
        [Column("group_id")]
        [ForeignKey("Group")]
        public int GroupId { get; set; }

        /// <summary>
        /// 使用者編號 (複合主鍵, 外鍵到 Users)
        /// </summary>
        [Column("user_id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// 加入時間
        /// </summary>
        [Column("joined_at")]
        public DateTime? JoinedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 是否為管理員
        /// </summary>
        [Required]
        [Column("is_admin")]
        public bool IsAdmin { get; set; } = false;

        // 導航屬性
        public virtual Group Group { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }

    /// <summary>
    /// 群組專用聊天表
    /// </summary>
    [Table("Group_Chat")]
    public class GroupChat
    {
        /// <summary>
        /// 群組聊天編號 (主鍵)
        /// </summary>
        [Key]
        [Column("group_chat_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GroupChatId { get; set; }

        /// <summary>
        /// 群組編號 (外鍵到 Groups)
        /// </summary>
        [Column("group_id")]
        [ForeignKey("Group")]
        public int? GroupId { get; set; }

        /// <summary>
        /// 發送者編號 (外鍵到 Users)
        /// </summary>
        [Column("sender_id")]
        [ForeignKey("SenderUser")]
        public int? SenderId { get; set; }

        /// <summary>
        /// 訊息內容
        /// </summary>
        [Column("group_chat_content")]
        [StringLength(1000)]
        public string? GroupChatContent { get; set; }

        /// <summary>
        /// 發送時間
        /// </summary>
        [Column("sent_at")]
        public DateTime? SentAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 是否寄送
        /// </summary>
        [Required]
        [Column("is_sent")]
        public bool IsSent { get; set; } = true;

        // 導航屬性
        public virtual Group? Group { get; set; }
        public virtual User? SenderUser { get; set; }
    }

    /// <summary>
    /// 封鎖表 (群組專用)
    /// </summary>
    [Table("Group_Block")]
    public class GroupBlock
    {
        /// <summary>
        /// 封鎖編號 (主鍵)
        /// </summary>
        [Key]
        [Column("block_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BlockId { get; set; }

        /// <summary>
        /// 群組編號 (外鍵到 Groups)
        /// </summary>
        [Column("group_id")]
        [ForeignKey("Group")]
        public int GroupId { get; set; }

        /// <summary>
        /// 被封鎖者編號 (外鍵到 Users)
        /// </summary>
        [Column("user_id")]
        [ForeignKey("BlockedUser")]
        public int UserId { get; set; }

        /// <summary>
        /// 封鎖者編號 (外鍵到 Users)
        /// </summary>
        [Column("blocked_by")]
        [ForeignKey("BlockedByUser")]
        public int BlockedBy { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        // 導航屬性
        public virtual Group Group { get; set; } = null!;
        public virtual User BlockedUser { get; set; } = null!;
        public virtual User BlockedByUser { get; set; } = null!;
    }
}