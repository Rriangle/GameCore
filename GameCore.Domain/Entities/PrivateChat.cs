using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Domain.Entities
{
    /// <summary>
    /// 私�?實�?
    /// ?�於管�??�戶之�??��??��???
    /// </summary>
    [Table("private_chats")]
    public partial class PrivateChat
    {
        /// <summary>
        /// 私�?ID（主?��?
        /// </summary>
        [Key]
        [Column("chat_id")]
        public int ChatId { get; set; }

        /// <summary>
        /// ?�送者ID（�??�到Users�?
        /// </summary>
        [Column("sender_id")]
        public int SenderId { get; set; }

        /// <summary>
        /// ?�戶1 ID（別?��??�於?��?層兼容性�?
        /// </summary>
        [NotMapped]
        public int User1Id => SenderId;

        /// <summary>
        /// ?�收?�ID（�??�到Users�?
        /// </summary>
        [Column("receiver_id")]
        public int ReceiverId { get; set; }

        /// <summary>
        /// ?�戶2 ID（別?��??�於?��?層兼容性�?
        /// </summary>
        [NotMapped]
        public int User2Id => ReceiverId;

        /// <summary>
        /// ?�後�??��???
        /// </summary>
        [Column("last_message_at")]
        public DateTime? LastMessageAt { get; set; }

        /// <summary>
        /// ?�新?��?（別?��??�於?��?層兼容性�?
        /// </summary>
        [NotMapped]
        public DateTime UpdatedAt => LastMessageAt ?? CreatedAt;

        /// <summary>
        /// 建�??��?
        /// </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// ?�送�??��??��?，用?��??�層?�容?��?
        /// </summary>
        [NotMapped]
        public DateTime SentAt => CreatedAt;

        // 導航屬�?
        public virtual User Sender { get; set; } = null!;
        public virtual User Receiver { get; set; } = null!;
        public virtual ICollection<PrivateMessage> Messages { get; set; } = new List<PrivateMessage>();
    }

    /// <summary>
    /// 私人訊息實�?
    /// </summary>
    [Table("private_messages")]
    public partial class PrivateMessage
    {
        /// <summary>
        /// 私人訊息編�? (主鍵)
        /// </summary>
        [Key]
        [Column("private_message_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// 私人?�天編�? (外鍵)
        /// </summary>
        [Required]
        [Column("private_chat_id")]
        [ForeignKey("PrivateChat")]
        public int PrivateChatId { get; set; }

        /// <summary>
        /// ?�天ID（別?��??�於?��?層兼容性�?
        /// </summary>
        [NotMapped]
        public int ChatId => PrivateChatId;

        /// <summary>
        /// ?�送者編??(外鍵)
        /// </summary>
        [Required]
        [Column("sender_id")]
        [ForeignKey("Sender")]
        public int SenderId { get; set; }

        /// <summary>
        /// ?�收?�ID（別?��??�於?��?層兼容性�?
        /// </summary>
        [NotMapped]
        public int ReceiverId => SenderId;

        /// <summary>
        /// 訊息?�容
        /// </summary>
        [Required]
        [Column("content")]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// 訊息類�?
        /// </summary>
        [Required]
        [Column("message_type")]
        [StringLength(20)]
        public string Type { get; set; } = "text";

        /// <summary>
        /// 訊息類�?（別?��??�於?��?層兼容性�?
        /// </summary>
        [NotMapped]
        public string MessageType => Type;

        /// <summary>
        /// ?�否已�?
        /// </summary>
        [Column("is_read")]
        public bool IsRead { get; set; } = false;

        /// <summary>
        /// ?�送�???
        /// </summary>
        [Column("sent_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 已�??��?
        /// </summary>
        [Column("read_at")]
        public DateTime? ReadAt { get; set; }

        // 導航屬�?
        public virtual PrivateChat PrivateChat { get; set; } = null!;
        public virtual User Sender { get; set; } = null!;
    }


} 
