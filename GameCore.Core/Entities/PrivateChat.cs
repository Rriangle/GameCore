using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 私人聊天實體
    /// </summary>
    [Table("private_chats")]
    public class PrivateChat
    {
        /// <summary>
        /// 私人聊天編號 (主鍵)
        /// </summary>
        [Key]
        [Column("private_chat_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PrivateChatId { get; set; }

        /// <summary>
        /// 使用者1編號 (外鍵)
        /// </summary>
        [Required]
        [Column("user1_id")]
        [ForeignKey("User1")]
        public int User1Id { get; set; }

        /// <summary>
        /// 使用者2編號 (外鍵)
        /// </summary>
        [Required]
        [Column("user2_id")]
        [ForeignKey("User2")]
        public int User2Id { get; set; }

        /// <summary>
        /// 聊天室編號 (外鍵)
        /// </summary>
        [Required]
        [Column("chat_room_id")]
        [ForeignKey("ChatRoom")]
        public int ChatRoomId { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 更新時間
        /// </summary>
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // 導航屬性
        public virtual User User1 { get; set; } = null!;
        public virtual User User2 { get; set; } = null!;
        public virtual ChatRoom ChatRoom { get; set; } = null!;
    }

    /// <summary>
    /// 私人訊息實體
    /// </summary>
    [Table("private_messages")]
    public class PrivateMessage
    {
        /// <summary>
        /// 私人訊息編號 (主鍵)
        /// </summary>
        [Key]
        [Column("private_message_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PrivateMessageId { get; set; }

        /// <summary>
        /// 私人聊天編號 (外鍵)
        /// </summary>
        [Required]
        [Column("private_chat_id")]
        [ForeignKey("PrivateChat")]
        public int PrivateChatId { get; set; }

        /// <summary>
        /// 發送者編號 (外鍵)
        /// </summary>
        [Required]
        [Column("sender_id")]
        [ForeignKey("Sender")]
        public int SenderId { get; set; }

        /// <summary>
        /// 訊息內容
        /// </summary>
        [Required]
        [Column("content")]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// 訊息類型
        /// </summary>
        [Required]
        [Column("message_type")]
        [StringLength(20)]
        public string MessageType { get; set; } = "text";

        /// <summary>
        /// 是否已讀
        /// </summary>
        [Column("is_read")]
        public bool IsRead { get; set; } = false;

        /// <summary>
        /// 發送時間
        /// </summary>
        [Column("sent_at")]
        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        // 導航屬性
        public virtual PrivateChat PrivateChat { get; set; } = null!;
        public virtual User Sender { get; set; } = null!;
    }


} 