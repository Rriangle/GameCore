using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 自由市場商品資訊表
    /// </summary>
    [Table("PlayerMarketProductInfo")]
    public class PlayerMarketProductInfo
    {
        /// <summary>
        /// 自由市場商品編號 (主鍵)
        /// </summary>
        [Key]
        [Column("p_product_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PProductId { get; set; }

        /// <summary>
        /// 商品類型
        /// </summary>
        [Column("p_product_type")]
        [StringLength(100)]
        public string? PProductType { get; set; }

        /// <summary>
        /// 商品標題 (噱頭標語)
        /// </summary>
        [Column("p_product_title")]
        [StringLength(200)]
        public string? PProductTitle { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        [Column("p_product_name")]
        [StringLength(200)]
        public string? PProductName { get; set; }

        /// <summary>
        /// 商品描述
        /// </summary>
        [Column("p_product_description")]
        [StringLength(1000)]
        public string? PProductDescription { get; set; }

        /// <summary>
        /// 官方商品編號 (外鍵到 ProductInfo)
        /// </summary>
        [Column("product_id")]
        [ForeignKey("ProductInfo")]
        public int? ProductId { get; set; }

        /// <summary>
        /// 賣家編號 (外鍵到 Users)
        /// </summary>
        [Column("seller_id")]
        [ForeignKey("Seller")]
        public int SellerId { get; set; }

        /// <summary>
        /// 商品狀態
        /// </summary>
        [Column("p_status")]
        [StringLength(50)]
        public string? PStatus { get; set; }

        /// <summary>
        /// 售價
        /// </summary>
        [Column("price", TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        /// <summary>
        /// 商品圖片編號
        /// </summary>
        [Column("p_product_img_id")]
        [StringLength(100)]
        public string? PProductImgId { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 更新時間
        /// </summary>
        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

        // 導航屬性
        public virtual ProductInfo? ProductInfo { get; set; }
        public virtual User Seller { get; set; } = null!;
        public virtual ICollection<PlayerMarketProductImg> ProductImages { get; set; } = new List<PlayerMarketProductImg>();
        public virtual ICollection<PlayerMarketOrderInfo> Orders { get; set; } = new List<PlayerMarketOrderInfo>();
        public virtual ICollection<PlayerMarketRanking> Rankings { get; set; } = new List<PlayerMarketRanking>();
    }

    /// <summary>
    /// 自由市場商品圖片表
    /// </summary>
    [Table("PlayerMarketProductImgs")]
    public class PlayerMarketProductImg
    {
        /// <summary>
        /// 商品圖片編號 (主鍵)
        /// </summary>
        [Key]
        [Column("p_product_img_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PProductImgId { get; set; }

        /// <summary>
        /// 自由市場商品編號 (外鍵到 PlayerMarketProductInfo)
        /// </summary>
        [Column("p_product_id")]
        [ForeignKey("PlayerMarketProductInfo")]
        public int PProductId { get; set; }

        /// <summary>
        /// 商品圖片網址 (二進位資料)
        /// </summary>
        [Column("p_product_img_url")]
        public byte[]? PProductImgUrl { get; set; }

        // 導航屬性
        public virtual PlayerMarketProductInfo PlayerMarketProductInfo { get; set; } = null!;
    }

    /// <summary>
    /// 自由市場訂單表
    /// </summary>
    [Table("PlayerMarketOrderInfo")]
    public class PlayerMarketOrderInfo
    {
        /// <summary>
        /// 自由市場訂單編號 (主鍵)
        /// </summary>
        [Key]
        [Column("p_order_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int POrderId { get; set; }

        /// <summary>
        /// 自由市場商品編號 (外鍵到 PlayerMarketProductInfo)
        /// </summary>
        [Column("p_product_id")]
        [ForeignKey("PlayerMarketProductInfo")]
        public int PProductId { get; set; }

        /// <summary>
        /// 賣家編號 (外鍵到 Users)
        /// </summary>
        [Column("seller_id")]
        [ForeignKey("Seller")]
        public int SellerId { get; set; }

        /// <summary>
        /// 買家編號 (外鍵到 Users)
        /// </summary>
        [Column("buyer_id")]
        [ForeignKey("Buyer")]
        public int BuyerId { get; set; }

        /// <summary>
        /// 訂單日期
        /// </summary>
        [Column("p_order_date")]
        public DateTime? POrderDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 訂單狀態
        /// </summary>
        [Column("p_order_status")]
        [StringLength(50)]
        public string? POrderStatus { get; set; }

        /// <summary>
        /// 付款狀態
        /// </summary>
        [Column("p_payment_status")]
        [StringLength(50)]
        public string? PPaymentStatus { get; set; }

        /// <summary>
        /// 單價
        /// </summary>
        [Column("p_unit_price")]
        public int PUnitPrice { get; set; }

        /// <summary>
        /// 數量
        /// </summary>
        [Column("p_quantity")]
        public int PQuantity { get; set; }

        /// <summary>
        /// 總額
        /// </summary>
        [Column("p_order_total")]
        public int POrderTotal { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        [Column("p_order_created_at")]
        public DateTime? POrderCreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 更新時間
        /// </summary>
        [Column("p_order_updated_at")]
        public DateTime? POrderUpdatedAt { get; set; } = DateTime.UtcNow;

        // 導航屬性
        public virtual PlayerMarketProductInfo PlayerMarketProductInfo { get; set; } = null!;
        public virtual User Seller { get; set; } = null!;
        public virtual User Buyer { get; set; } = null!;
        public virtual ICollection<PlayerMarketOrderTradepage> Tradepages { get; set; } = new List<PlayerMarketOrderTradepage>();
    }

    /// <summary>
    /// 交易中頁面表 - Buyer和Seller都確認在遊戲平台交易完成，才金流
    /// </summary>
    [Table("PlayerMarketOrderTradepage")]
    public class PlayerMarketOrderTradepage
    {
        /// <summary>
        /// 交易頁面編號 (主鍵)
        /// </summary>
        [Key]
        [Column("p_order_tradepage_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int POrderTradepageId { get; set; }

        /// <summary>
        /// 訂單編號 (外鍵到 PlayerMarketOrderInfo)
        /// </summary>
        [Column("p_order_id")]
        [ForeignKey("PlayerMarketOrderInfo")]
        public int POrderId { get; set; }

        /// <summary>
        /// 商品編號 (外鍵到 PlayerMarketProductInfo)
        /// </summary>
        [Column("p_product_id")]
        [ForeignKey("PlayerMarketProductInfo")]
        public int PProductId { get; set; }

        /// <summary>
        /// 平台抽成
        /// </summary>
        [Column("p_order_platform_fee")]
        public int POrderPlatformFee { get; set; }

        /// <summary>
        /// 賣家移交時間 - user1(seller) 賣家移交時間
        /// </summary>
        [Column("seller_transferred_at")]
        public DateTime? SellerTransferredAt { get; set; }

        /// <summary>
        /// 買家接收時間 - user2(buyer) 買家接收時間
        /// </summary>
        [Column("buyer_received_at")]
        public DateTime? BuyerReceivedAt { get; set; }

        /// <summary>
        /// 交易完成時間
        /// </summary>
        [Column("completed_at")]
        public DateTime? CompletedAt { get; set; }

        // 導航屬性
        public virtual PlayerMarketOrderInfo PlayerMarketOrderInfo { get; set; } = null!;
        public virtual PlayerMarketProductInfo PlayerMarketProductInfo { get; set; } = null!;
        public virtual ICollection<PlayerMarketTradeMsg> TradeMessages { get; set; } = new List<PlayerMarketTradeMsg>();
    }

    /// <summary>
    /// 自由市場交易頁面對話表
    /// </summary>
    [Table("PlayerMarketTradeMsg")]
    public class PlayerMarketTradeMsg
    {
        /// <summary>
        /// 交易訊息編號 (主鍵)
        /// </summary>
        [Key]
        [Column("trade_msg_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TradeMsgId { get; set; }

        /// <summary>
        /// 交易頁面編號 (外鍵到 PlayerMarketOrderTradepage)
        /// </summary>
        [Column("p_order_tradepage_id")]
        [ForeignKey("PlayerMarketOrderTradepage")]
        public int POrderTradepageId { get; set; }

        /// <summary>
        /// 誰傳的訊息 (seller/buyer)
        /// </summary>
        [Column("msg_from")]
        [StringLength(20)]
        public string? MsgFrom { get; set; }

        /// <summary>
        /// 訊息內容
        /// </summary>
        [Column("message_text")]
        [StringLength(1000)]
        public string? MessageText { get; set; }

        /// <summary>
        /// 傳訊時間
        /// </summary>
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        // 導航屬性
        public virtual PlayerMarketOrderTradepage PlayerMarketOrderTradepage { get; set; } = null!;
    }

    /// <summary>
    /// 自由市場排行榜表
    /// </summary>
    [Table("PlayerMarketRanking")]
    public class PlayerMarketRanking
    {
        /// <summary>
        /// 排行榜編號 (主鍵)
        /// </summary>
        [Key]
        [Column("p_ranking_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PRankingId { get; set; }

        /// <summary>
        /// 榜單型態
        /// </summary>
        [Column("p_period_type")]
        [StringLength(50)]
        public string? PPeriodType { get; set; }

        /// <summary>
        /// 榜單日期
        /// </summary>
        [Column("p_ranking_date")]
        public DateTime PRankingDate { get; set; }

        /// <summary>
        /// 商品編號 (外鍵到 PlayerMarketProductInfo)
        /// </summary>
        [Column("p_product_id")]
        [ForeignKey("PlayerMarketProductInfo")]
        public int PProductId { get; set; }

        /// <summary>
        /// 排名指標
        /// </summary>
        [Column("p_ranking_metric")]
        [StringLength(50)]
        public string? PRankingMetric { get; set; }

        /// <summary>
        /// 名次
        /// </summary>
        [Column("p_ranking_position")]
        public int PRankingPosition { get; set; }

        /// <summary>
        /// 交易額
        /// </summary>
        [Column("p_trading_amount", TypeName = "decimal(18,2)")]
        public decimal PTradingAmount { get; set; }

        /// <summary>
        /// 交易量
        /// </summary>
        [Column("p_trading_volume")]
        public int PTradingVolume { get; set; }

        /// <summary>
        /// 排行榜更新時間
        /// </summary>
        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

        // 導航屬性
        public virtual PlayerMarketProductInfo PlayerMarketProductInfo { get; set; } = null!;
    }
}