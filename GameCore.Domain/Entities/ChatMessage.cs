using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Domain.Entities
{
    /// <summary>
    /// ?�天訊息實�?
    /// </summary>
    [Table("chat_messages")]
    public partial class ChatMessage
    {
        /// <summary>
        /// 訊息編�? (主鍵)
        /// </summary>
        [Key]
        [Column("message_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// ?�天室編??(外鍵)
        /// </summary>
        [Required]
        [Column("room_id")]
        [ForeignKey("ChatRoom")]
        public int ChatRoomId { get; set; }

        /// <summary>
        /// ?�天室ID（別?��??�於?��?層兼容性�?
        /// </summary>
        [NotMapped]
        public int RoomId => ChatRoomId;

        /// <summary>
        /// ?�送者編??(外鍵)
        /// </summary>
        [Required]
        [Column("sender_id")]
        [ForeignKey("Sender")]
        public int SenderId { get; set; }

        /// <summary>
        /// 訊息?�容
        /// </summary>
        [Required]
        [Column("content")]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// 訊息類�? (text/image/file/system)
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
        /// ?�否已編�?
        /// </summary>
        [Column("is_edited")]
        public bool IsEdited { get; set; } = false;

        /// <summary>
        /// ?�否已刪??
        /// </summary>
        [Column("is_deleted")]
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// ?�送�???
        /// </summary>
        [Column("sent_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// ?�送�??��??��?，用?��??�層?�容?��?
        /// </summary>
        [NotMapped]
        public DateTime SentAt => CreatedAt;

        /// <summary>
        /// 編輯?��?
        /// </summary>
        [Column("edited_at")]
        public DateTime? EditedAt { get; set; }

        // 導航屬�?
        public virtual ChatRoom ChatRoom { get; set; } = null!;
        public virtual User Sender { get; set; } = null!;
    }


} 
