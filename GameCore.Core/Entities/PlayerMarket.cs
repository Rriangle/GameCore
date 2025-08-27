using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 玩家市場項目實體
    /// </summary>
    [Table("player_market_items")]
    public class PlayerMarketItem
    {
        /// <summary>
        /// 項目編號 (主鍵)
        /// </summary>
        [Key]
        [Column("item_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemId { get; set; }

        /// <summary>
        /// 賣家編號 (外鍵)
        /// </summary>
        [Required]
        [Column("seller_id")]
        [ForeignKey("Seller")]
        public int SellerId { get; set; }

        /// <summary>
        /// 項目名稱
        /// </summary>
        [Required]
        [Column("item_name")]
        [StringLength(200)]
        public string ItemName { get; set; } = string.Empty;

        /// <summary>
        /// 項目描述
        /// </summary>
        [Column("description")]
        public string? Description { get; set; }

        /// <summary>
        /// 價格
        /// </summary>
        [Required]
        [Column("price")]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        /// <summary>
        /// 類別
        /// </summary>
        [Required]
        [Column("category")]
        [StringLength(50)]
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// 狀態
        /// </summary>
        [Required]
        [Column("status")]
        [StringLength(20)]
        public string Status { get; set; } = "active";

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
        public virtual User Seller { get; set; } = null!;
        public virtual ICollection<MarketTransaction> Transactions { get; set; } = new List<MarketTransaction>();
        public virtual ICollection<MarketReview> Reviews { get; set; } = new List<MarketReview>();
    }

    /// <summary>
    /// 市場交易實體
    /// </summary>
    [Table("market_transactions")]
    public class MarketTransaction
    {
        /// <summary>
        /// 交易編號 (主鍵)
        /// </summary>
        [Key]
        [Column("transaction_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionId { get; set; }

        /// <summary>
        /// 買家編號 (外鍵)
        /// </summary>
        [Required]
        [Column("buyer_id")]
        [ForeignKey("Buyer")]
        public int BuyerId { get; set; }

        /// <summary>
        /// 賣家編號 (外鍵)
        /// </summary>
        [Required]
        [Column("seller_id")]
        [ForeignKey("Seller")]
        public int SellerId { get; set; }

        /// <summary>
        /// 項目編號 (外鍵)
        /// </summary>
        [Required]
        [Column("item_id")]
        [ForeignKey("PlayerMarketItem")]
        public int ItemId { get; set; }

        /// <summary>
        /// 交易金額
        /// </summary>
        [Required]
        [Column("amount")]
        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }

        /// <summary>
        /// 交易狀態
        /// </summary>
        [Required]
        [Column("status")]
        [StringLength(20)]
        public string Status { get; set; } = "pending";

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
        public virtual User Buyer { get; set; } = null!;
        public virtual User Seller { get; set; } = null!;
        public virtual PlayerMarketItem PlayerMarketItem { get; set; } = null!;
    }

    /// <summary>
    /// 市場評論實體
    /// </summary>
    [Table("market_reviews")]
    public class MarketReview
    {
        /// <summary>
        /// 評論編號 (主鍵)
        /// </summary>
        [Key]
        [Column("review_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReviewId { get; set; }

        /// <summary>
        /// 項目編號 (外鍵)
        /// </summary>
        [Required]
        [Column("item_id")]
        [ForeignKey("PlayerMarketItem")]
        public int ItemId { get; set; }

        /// <summary>
        /// 評論者編號 (外鍵)
        /// </summary>
        [Required]
        [Column("reviewer_id")]
        [ForeignKey("Reviewer")]
        public int ReviewerId { get; set; }

        /// <summary>
        /// 評分
        /// </summary>
        [Required]
        [Column("rating")]
        [Range(1, 5)]
        public int Rating { get; set; }

        /// <summary>
        /// 評論內容
        /// </summary>
        [Column("comment")]
        public string? Comment { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // 導航屬性
        public virtual PlayerMarketItem PlayerMarketItem { get; set; } = null!;
        public virtual User Reviewer { get; set; } = null!;
    }

    /// <summary>
    /// 市場項目狀態列舉
    /// </summary>
    public enum MarketItemStatus
    {
        Active,
        Sold,
        Cancelled,
        Expired
    }

    /// <summary>
    /// 交易狀態列舉
    /// </summary>
    public enum TransactionStatus
    {
        Pending,
        Confirmed,
        Completed,
        Cancelled,
        Refunded
    }
}