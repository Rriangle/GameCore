using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GameCore.Core.Enums;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 玩家市場商品實體
    /// </summary>
    [Table("player_market_items")]
    public class PlayerMarketItem
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("seller_id")]
        public int SellerId { get; set; }

        [Required]
        [Column("title", TypeName = "varchar(200)")]
        public string Title { get; set; } = string.Empty;

        [Column("description", TypeName = "text")]
        public string Description { get; set; } = string.Empty;

        [Column("price", TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Column("category", TypeName = "varchar(50)")]
        public string Category { get; set; } = string.Empty;

        [Column("condition", TypeName = "varchar(20)")]
        public string Condition { get; set; } = string.Empty;

        [Column("image_url", TypeName = "varchar(500)")]
        public string? ImageUrl { get; set; }

        [Column("status")]
        public MarketItemStatus Status { get; set; } = MarketItemStatus.Active;

        [Column("view_count")]
        public int ViewCount { get; set; } = 0;

        [Column("favorite_count")]
        public int FavoriteCount { get; set; } = 0;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Column("expires_at")]
        public DateTime? ExpiresAt { get; set; }

        // 導航屬性
        [ForeignKey("SellerId")]
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
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("market_item_id")]
        public int MarketItemId { get; set; }

        [Column("buyer_id")]
        public int BuyerId { get; set; }

        [Column("seller_id")]
        public int SellerId { get; set; }

        [Column("transaction_amount", TypeName = "decimal(18,2)")]
        public decimal TransactionAmount { get; set; }

        [Column("status")]
        public TransactionStatus Status { get; set; } = TransactionStatus.Pending;

        [Column("transaction_date")]
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        [Column("completed_at")]
        public DateTime? CompletedAt { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // 導航屬性
        [ForeignKey("MarketItemId")]
        public virtual PlayerMarketItem MarketItem { get; set; } = null!;
        [ForeignKey("BuyerId")]
        public virtual User Buyer { get; set; } = null!;
        [ForeignKey("SellerId")]
        public virtual User Seller { get; set; } = null!;
    }

    /// <summary>
    /// 市場評價實體
    /// </summary>
    [Table("market_reviews")]
    public class MarketReview
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("market_item_id")]
        public int MarketItemId { get; set; }

        [Column("reviewer_id")]
        public int ReviewerId { get; set; }

        [Column("rating")]
        public int Rating { get; set; } // 1-5 星

        [Column("comment", TypeName = "text")]
        public string? Comment { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // 導航屬性
        [ForeignKey("MarketItemId")]
        public virtual PlayerMarketItem MarketItem { get; set; } = null!;
        [ForeignKey("ReviewerId")]
        public virtual User Reviewer { get; set; } = null!;
    }
}