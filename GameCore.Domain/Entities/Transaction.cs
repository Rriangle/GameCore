using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Domain.Entities
{
    /// <summary>
    /// äº¤æ?è¨˜é?å¯¦é?
    /// </summary>
    [Table("transactions")]
    public class Transaction
    {
        /// <summary>
        /// äº¤æ?ç·¨è?
        /// </summary>
        [Key]
        [Column("transaction_id")]
        public int TransactionId { get; set; }

        /// <summary>
        /// ä½¿ç”¨?…ç·¨??
        /// </summary>
        [Column("user_id")]
        public int UserId { get; set; }

        /// <summary>
        /// äº¤æ??‘é?
        /// </summary>
        [Column("amount")]
        public decimal Amount { get; set; }

        /// <summary>
        /// äº¤æ?é¡å?
        /// </summary>
        [Column("transaction_type")]
        [StringLength(50)]
        public string TransactionType { get; set; } = string.Empty;

        /// <summary>
        /// äº¤æ??€??
        /// </summary>
        [Column("status")]
        [StringLength(20)]
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// äº¤æ??è¿°
        /// </summary>
        [Column("description")]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// ?ƒè€ƒç·¨??
        /// </summary>
        [Column("reference_id")]
        [StringLength(100)]
        public string ReferenceId { get; set; } = string.Empty;

        /// <summary>
        /// å»ºç??‚é?
        /// </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// ?´æ–°?‚é?
        /// </summary>
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// å®Œæ??‚é?
        /// </summary>
        [Column("completed_at")]
        public DateTime? CompletedAt { get; set; }

        /// <summary>
        /// ä½¿ç”¨?…å??ªå±¬??
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }
} 
