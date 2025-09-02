using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Domain.Entities
{
    /// <summary>
    /// 交�?記�?實�?
    /// </summary>
    [Table("transactions")]
    public class Transaction
    {
        /// <summary>
        /// 交�?編�?
        /// </summary>
        [Key]
        [Column("transaction_id")]
        public int TransactionId { get; set; }

        /// <summary>
        /// 使用?�編??
        /// </summary>
        [Column("user_id")]
        public int UserId { get; set; }

        /// <summary>
        /// 交�??��?
        /// </summary>
        [Column("amount")]
        public decimal Amount { get; set; }

        /// <summary>
        /// 交�?類�?
        /// </summary>
        [Column("transaction_type")]
        [StringLength(50)]
        public string TransactionType { get; set; } = string.Empty;

        /// <summary>
        /// 交�??�??
        /// </summary>
        [Column("status")]
        [StringLength(20)]
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// 交�??�述
        /// </summary>
        [Column("description")]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// ?�考編??
        /// </summary>
        [Column("reference_id")]
        [StringLength(100)]
        public string ReferenceId { get; set; } = string.Empty;

        /// <summary>
        /// 建�??��?
        /// </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// ?�新?��?
        /// </summary>
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// 完�??��?
        /// </summary>
        [Column("completed_at")]
        public DateTime? CompletedAt { get; set; }

        /// <summary>
        /// 使用?��??�屬??
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }
} 
