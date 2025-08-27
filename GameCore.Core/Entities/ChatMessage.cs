using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 聊天訊息實體
    /// </summary>
    [Table("chat_messages")]
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
        /// 聊天室編號 (外鍵)
        /// </summary>
        [Required]
        [Column("room_id")]
        [ForeignKey("ChatRoom")]
        public int RoomId { get; set; }

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
        /// 訊息類型 (text/image/file/system)
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
        /// 是否已編輯
        /// </summary>
        [Column("is_edited")]
        public bool IsEdited { get; set; } = false;

        /// <summary>
        /// 是否已刪除
        /// </summary>
        [Column("is_deleted")]
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// 發送時間
        /// </summary>
        [Column("sent_at")]
        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 編輯時間
        /// </summary>
        [Column("edited_at")]
        public DateTime? EditedAt { get; set; }

        // 導航屬性
        public virtual ChatRoom ChatRoom { get; set; } = null!;
        public virtual User Sender { get; set; } = null!;
    }


} 