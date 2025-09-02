using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Domain.Entities
{
    /// <summary>
    /// å¸‚å ´è©•åƒ¹å¯¦é?
    /// ?¨æ–¼ç®¡ç??©å®¶å¸‚å ´?„äº¤?“è???
    /// </summary>
    [Table("market_reviews")]
    public class MarketReview
    {
        /// <summary>
        /// è©•åƒ¹IDï¼ˆä¸»?µï?
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// äº¤æ?IDï¼ˆå??µï?
        /// </summary>
        [Column("transaction_id")]
        public int TransactionId { get; set; }

        /// <summary>
        /// è©•åƒ¹?…IDï¼ˆå??µåˆ°Usersï¼?
        /// </summary>
        [Column("reviewer_id")]
        public int ReviewerId { get; set; }

        /// <summary>
        /// è¢«è??¹è€…IDï¼ˆå??µåˆ°Usersï¼?
        /// </summary>
        [Column("reviewee_id")]
        public int RevieweeId { get; set; }

        /// <summary>
        /// è©•å?ï¼?-5ï¼?
        /// </summary>
        [Column("rating")]
        public int Rating { get; set; }

        /// <summary>
        /// è©•åƒ¹?§å®¹
        /// </summary>
        [Column("content")]
        [StringLength(1000)]
        public string? Content { get; set; }

        /// <summary>
        /// å»ºç??‚é?
        /// </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// ?´æ–°?‚é?
        /// </summary>
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // å°Žèˆªå±¬æ€?
        public virtual MarketTransaction Transaction { get; set; } = null!;
        public virtual User Reviewer { get; set; } = null!;
        public virtual User Reviewee { get; set; } = null!;
    }
} 
