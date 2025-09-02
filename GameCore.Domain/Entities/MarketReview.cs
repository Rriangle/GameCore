using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Domain.Entities
{
    /// <summary>
    /// 市場評價實�?
    /// ?�於管�??�家市場?�交?��???
    /// </summary>
    [Table("market_reviews")]
    public class MarketReview
    {
        /// <summary>
        /// 評價ID（主?��?
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// 交�?ID（�??��?
        /// </summary>
        [Column("transaction_id")]
        public int TransactionId { get; set; }

        /// <summary>
        /// 評價?�ID（�??�到Users�?
        /// </summary>
        [Column("reviewer_id")]
        public int ReviewerId { get; set; }

        /// <summary>
        /// 被�??�者ID（�??�到Users�?
        /// </summary>
        [Column("reviewee_id")]
        public int RevieweeId { get; set; }

        /// <summary>
        /// 評�?�?-5�?
        /// </summary>
        [Column("rating")]
        public int Rating { get; set; }

        /// <summary>
        /// 評價?�容
        /// </summary>
        [Column("content")]
        [StringLength(1000)]
        public string? Content { get; set; }

        /// <summary>
        /// 建�??��?
        /// </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// ?�新?��?
        /// </summary>
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // 導航屬�?
        public virtual MarketTransaction Transaction { get; set; } = null!;
        public virtual User Reviewer { get; set; } = null!;
        public virtual User Reviewee { get; set; } = null!;
    }
} 
