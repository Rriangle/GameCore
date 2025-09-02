using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Domain.Entities
{
    /// <summary>
    /// ?�家市場?�目實�?
    /// </summary>
    [Table("player_market_items")]
    public class PlayerMarketItem
    {
        /// <summary>
        /// ?�目編�? (主鍵)
        /// </summary>
        [Key]
        [Column("item_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemId { get; set; }

        /// <summary>
        /// �?��編�? (外鍵)
        /// </summary>
        [Required]
        [Column("seller_id")]
        [ForeignKey("Seller")]
        public int SellerId { get; set; }

        /// <summary>
        /// ?�目?�稱
        /// </summary>
        [Required]
        [Column("item_name")]
        [StringLength(200)]
        public string ItemName { get; set; } = string.Empty;

        /// <summary>
        /// ?�目?�述
        /// </summary>
        [Column("description")]
        public string? Description { get; set; }

        /// <summary>
        /// ?�格
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
        /// ?�??
        /// </summary>
        [Required]
        [Column("status")]
        [StringLength(20)]
        public string Status { get; set; } = "active";

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
        public virtual User Seller { get; set; } = null!;
        public virtual ICollection<MarketTransaction> Transactions { get; set; } = new List<MarketTransaction>();
        public virtual ICollection<MarketReview> Reviews { get; set; } = new List<MarketReview>();
    }

    /// <summary>
    /// 市場?�目?�?��???
    /// </summary>
    public enum MarketItemStatus
    {
        Active,
        Sold,
        Cancelled,
        Expired
    }

    /// <summary>
    /// 交�??�?��???
    /// </summary>
    public enum TransactionStatus
    {
        Pending,
        Confirmed,
        Completed,
        Cancelled,
        Refunded
    }

    /// <summary>
    /// 玩家市場商品圖片
    /// </summary>
    [Table("PlayerMarketProductImg")]
    public class PlayerMarketProductImg
    {
        /// <summary>
        /// 圖片ID (主鍵)
        /// </summary>
        [Key]
        [Column("img_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ImgId { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>
        [Column("product_id")]
        public int ProductId { get; set; }

        /// <summary>
        /// 圖片URL
        /// </summary>
        [Column("img_url")]
        [StringLength(500)]
        public string ImgUrl { get; set; } = string.Empty;

        /// <summary>
        /// 圖片描述
        /// </summary>
        [Column("img_description")]
        [StringLength(200)]
        public string? ImgDescription { get; set; }

        /// <summary>
        /// 是否為主圖
        /// </summary>
        [Column("is_main")]
        public bool IsMain { get; set; }

        /// <summary>
        /// 排序順序
        /// </summary>
        [Column("sort_order")]
        public int SortOrder { get; set; }

        /// <summary>
        /// 創建時間
        /// </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        // 導航屬性
        public virtual PlayerMarketProductInfo Product { get; set; } = null!;
    }

    /// <summary>
    /// 玩家市場訂單交易頁面
    /// </summary>
    [Table("PlayerMarketOrderTradepage")]
    public class PlayerMarketOrderTradepage
    {
        /// <summary>
        /// 交易頁面ID (主鍵)
        /// </summary>
        [Key]
        [Column("tradepage_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TradepageId { get; set; }

        /// <summary>
        /// 訂單ID
        /// </summary>
        [Column("order_id")]
        public int OrderId { get; set; }

        /// <summary>
        /// 買家ID
        /// </summary>
        [Column("buyer_id")]
        public int BuyerId { get; set; }

        /// <summary>
        /// 賣家ID
        /// </summary>
        [Column("seller_id")]
        public int SellerId { get; set; }

        /// <summary>
        /// 交易狀態
        /// </summary>
        [Column("trade_status")]
        [StringLength(50)]
        public string TradeStatus { get; set; } = string.Empty;

        /// <summary>
        /// 創建時間
        /// </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        // 導航屬性
        public virtual PlayerMarketOrderInfo Order { get; set; } = null!;
        public virtual User Buyer { get; set; } = null!;
        public virtual User Seller { get; set; } = null!;
    }

    /// <summary>
    /// 玩家市場交易消息
    /// </summary>
    [Table("PlayerMarketTradeMsg")]
    public class PlayerMarketTradeMsg
    {
        /// <summary>
        /// 消息ID (主鍵)
        /// </summary>
        [Key]
        [Column("msg_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MsgId { get; set; }

        /// <summary>
        /// 交易頁面ID
        /// </summary>
        [Column("tradepage_id")]
        public int TradepageId { get; set; }

        /// <summary>
        /// 發送者ID
        /// </summary>
        [Column("sender_id")]
        public int SenderId { get; set; }

        /// <summary>
        /// 消息內容
        /// </summary>
        [Column("message")]
        [StringLength(1000)]
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 消息類型
        /// </summary>
        [Column("msg_type")]
        [StringLength(50)]
        public string MsgType { get; set; } = string.Empty;

        /// <summary>
        /// 發送時間
        /// </summary>
        [Column("sent_at")]
        public DateTime SentAt { get; set; }

        /// <summary>
        /// 是否已讀
        /// </summary>
        [Column("is_read")]
        public bool IsRead { get; set; }

        // 導航屬性
        public virtual PlayerMarketOrderTradepage Tradepage { get; set; } = null!;
        public virtual User Sender { get; set; } = null!;
    }

    /// <summary>
    /// 玩家市場排行榜
    /// </summary>
    [Table("PlayerMarketRanking")]
    public class PlayerMarketRanking
    {
        /// <summary>
        /// 排行榜ID (主鍵)
        /// </summary>
        [Key]
        [Column("ranking_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RankingId { get; set; }

        /// <summary>
        /// 用戶ID
        /// </summary>
        [Column("user_id")]
        public int UserId { get; set; }

        /// <summary>
        /// 排行榜類型
        /// </summary>
        [Column("ranking_type")]
        [StringLength(50)]
        public string RankingType { get; set; } = string.Empty;

        /// <summary>
        /// 排名
        /// </summary>
        [Column("rank")]
        public int Rank { get; set; }

        /// <summary>
        /// 分數
        /// </summary>
        [Column("score")]
        public decimal Score { get; set; }

        /// <summary>
        /// 統計週期
        /// </summary>
        [Column("period")]
        [StringLength(20)]
        public string Period { get; set; } = string.Empty;

        /// <summary>
        /// 統計日期
        /// </summary>
        [Column("stat_date")]
        public DateTime StatDate { get; set; }

        /// <summary>
        /// 創建時間
        /// </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        // 導航屬性
        public virtual User User { get; set; } = null!;
    }
}
