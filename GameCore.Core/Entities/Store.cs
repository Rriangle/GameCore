using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 供應商表
    /// </summary>
    [Table("Supplier")]
    public class Supplier
    {
        /// <summary>
        /// 廠商編號 (主鍵)
        /// </summary>
        [Key]
        [Column("supplier_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SupplierId { get; set; }

        /// <summary>
        /// 廠商名稱
        /// </summary>
        [Column("supplier_name")]
        [StringLength(200)]
        public string? SupplierName { get; set; }

        // 導航屬性
        public virtual ICollection<GameProductDetails> GameProductDetails { get; set; } = new List<GameProductDetails>();
        public virtual ICollection<OtherProductDetails> OtherProductDetails { get; set; } = new List<OtherProductDetails>();
    }

    /// <summary>
    /// 商品資訊表
    /// </summary>
    [Table("ProductInfo")]
    public class ProductInfo
    {
        /// <summary>
        /// 商品編號 (主鍵)
        /// </summary>
        [Key]
        [Column("product_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        [Column("product_name")]
        [StringLength(200)]
        public string? ProductName { get; set; }

        /// <summary>
        /// 商品類型
        /// </summary>
        [Column("product_type")]
        [StringLength(100)]
        public string? ProductType { get; set; }

        /// <summary>
        /// 售價
        /// </summary>
        [Column("price", TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        /// <summary>
        /// 使用幣別
        /// </summary>
        [Column("currency_code")]
        [StringLength(10)]
        public string? CurrencyCode { get; set; }

        /// <summary>
        /// 庫存
        /// </summary>
        [Column("Shipment_Quantity")]
        public int ShipmentQuantity { get; set; }

        /// <summary>
        /// 創建者
        /// </summary>
        [Column("product_created_by")]
        [StringLength(100)]
        public string? ProductCreatedBy { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        [Column("product_created_at")]
        public DateTime? ProductCreatedAt { get; set; }

        /// <summary>
        /// 最後修改者
        /// </summary>
        [Column("product_updated_by")]
        [StringLength(100)]
        public string? ProductUpdatedBy { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        [Column("product_updated_at")]
        public DateTime? ProductUpdatedAt { get; set; }

        /// <summary>
        /// 會員編號
        /// </summary>
        [Column("user_id")]
        public int? UserId { get; set; }

        // 導航屬性
        public virtual GameProductDetails? GameProductDetails { get; set; }
        public virtual OtherProductDetails? OtherProductDetails { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public virtual ICollection<OfficialStoreRanking> OfficialStoreRankings { get; set; } = new List<OfficialStoreRanking>();
        public virtual ICollection<ProductInfoAuditLog> AuditLogs { get; set; } = new List<ProductInfoAuditLog>();
        public virtual ICollection<PlayerMarketProductInfo> PlayerMarketProducts { get; set; } = new List<PlayerMarketProductInfo>();
    }

    /// <summary>
    /// 遊戲主檔商品資訊表
    /// </summary>
    [Table("GameProductDetails")]
    public class GameProductDetails
    {
        /// <summary>
        /// 商品編號 (主鍵, 外鍵到 ProductInfo)
        /// </summary>
        [Key]
        [Column("product_id")]
        [ForeignKey("ProductInfo")]
        public int ProductId { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        [Column("product_name")]
        [StringLength(200)]
        public string? ProductName { get; set; }

        /// <summary>
        /// 商品描述
        /// </summary>
        [Column("product_description")]
        [StringLength(1000)]
        public string? ProductDescription { get; set; }

        /// <summary>
        /// 廠商編號 (外鍵到 Supplier)
        /// </summary>
        [Column("supplier_id")]
        [ForeignKey("Supplier")]
        public int? SupplierId { get; set; }

        /// <summary>
        /// 遊戲平台編號
        /// </summary>
        [Column("platform_id")]
        public int? PlatformId { get; set; }

        /// <summary>
        /// 遊戲編號 (外鍵到 games)
        /// </summary>
        [Column("game_id")]
        [ForeignKey("Game")]
        public int? GameId { get; set; }

        /// <summary>
        /// 遊戲名稱
        /// </summary>
        [Column("game_name")]
        [StringLength(200)]
        public string? GameName { get; set; }

        /// <summary>
        /// 下載連結
        /// </summary>
        [Column("download_link")]
        [StringLength(500)]
        public string? DownloadLink { get; set; }

        // 導航屬性
        public virtual ProductInfo ProductInfo { get; set; } = null!;
        public virtual Supplier? Supplier { get; set; }
        public virtual Game? Game { get; set; }
    }

    /// <summary>
    /// 非遊戲主檔商品資訊表
    /// </summary>
    [Table("OtherProductDetails")]
    public class OtherProductDetails
    {
        /// <summary>
        /// 商品編號 (主鍵, 外鍵到 ProductInfo)
        /// </summary>
        [Key]
        [Column("product_id")]
        [ForeignKey("ProductInfo")]
        public int ProductId { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        [Column("product_name")]
        [StringLength(200)]
        public string? ProductName { get; set; }

        /// <summary>
        /// 商品描述
        /// </summary>
        [Column("product_description")]
        [StringLength(1000)]
        public string? ProductDescription { get; set; }

        /// <summary>
        /// 廠商編號 (外鍵到 Supplier)
        /// </summary>
        [Column("supplier_id")]
        [ForeignKey("Supplier")]
        public int? SupplierId { get; set; }

        /// <summary>
        /// 遊戲平台編號
        /// </summary>
        [Column("platform_id")]
        public int? PlatformId { get; set; }

        /// <summary>
        /// 數位序號兌換碼
        /// </summary>
        [Column("digital_code")]
        [StringLength(100)]
        public string? DigitalCode { get; set; }

        /// <summary>
        /// 尺寸
        /// </summary>
        [Column("size")]
        [StringLength(50)]
        public string? Size { get; set; }

        /// <summary>
        /// 顏色
        /// </summary>
        [Column("color")]
        [StringLength(50)]
        public string? Color { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        [Column("weight")]
        [StringLength(50)]
        public string? Weight { get; set; }

        /// <summary>
        /// 尺寸
        /// </summary>
        [Column("dimensions")]
        [StringLength(100)]
        public string? Dimensions { get; set; }

        /// <summary>
        /// 材質
        /// </summary>
        [Column("material")]
        [StringLength(100)]
        public string? Material { get; set; }

        /// <summary>
        /// 庫存數量
        /// </summary>
        [Column("stock_quantity")]
        [StringLength(50)]
        public string? StockQuantity { get; set; }

        // 導航屬性
        public virtual ProductInfo ProductInfo { get; set; } = null!;
        public virtual Supplier? Supplier { get; set; }
    }

    /// <summary>
    /// 訂單資訊表
    /// </summary>
    [Table("OrderInfo")]
    public class OrderInfo
    {
        /// <summary>
        /// 訂單編號 (主鍵)
        /// </summary>
        [Key]
        [Column("order_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        /// <summary>
        /// 下訂會員編號 (外鍵到 Users)
        /// </summary>
        [Column("user_id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// 下單日期
        /// </summary>
        [Column("order_date")]
        public DateTime? OrderDate { get; set; }

        /// <summary>
        /// 訂單狀態 (Created/ToShip/Shipped/Completed)
        /// </summary>
        [Column("order_status")]
        [StringLength(50)]
        public string? OrderStatus { get; set; }

        /// <summary>
        /// 付款狀態 (Placed/Pending/Paid)
        /// </summary>
        [Column("payment_status")]
        [StringLength(50)]
        public string? PaymentStatus { get; set; }

        /// <summary>
        /// 訂單總額
        /// </summary>
        [Column("order_total", TypeName = "decimal(18,2)")]
        public decimal OrderTotal { get; set; }

        /// <summary>
        /// 付款時間
        /// </summary>
        [Column("payment_at")]
        public DateTime? PaymentAt { get; set; }

        /// <summary>
        /// 出貨時間
        /// </summary>
        [Column("shipped_at")]
        public DateTime? ShippedAt { get; set; }

        /// <summary>
        /// 完成時間
        /// </summary>
        [Column("completed_at")]
        public DateTime? CompletedAt { get; set; }

        // 導航屬性
        public virtual User User { get; set; } = null!;
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }



    /// <summary>
    /// 官方商城排行榜表
    /// </summary>
    [Table("Official_Store_Ranking")]
    public class OfficialStoreRanking
    {
        /// <summary>
        /// 排行榜流水號 (主鍵)
        /// </summary>
        [Key]
        [Column("ranking_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RankingId { get; set; }

        /// <summary>
        /// 榜單型態 (日、月、季、年)
        /// </summary>
        [Column("period_type")]
        [StringLength(50)]
        public string? PeriodType { get; set; }

        /// <summary>
        /// 榜單日期 (計算日期)
        /// </summary>
        [Column("ranking_date")]
        public DateTime RankingDate { get; set; }

        /// <summary>
        /// 商品編號 (外鍵到 ProductInfo)
        /// </summary>
        [Column("product_ID")]
        [ForeignKey("ProductInfo")]
        public int ProductId { get; set; }

        /// <summary>
        /// 排名指標 (交易額/量)
        /// </summary>
        [Column("ranking_metric")]
        [StringLength(50)]
        public string? RankingMetric { get; set; }

        /// <summary>
        /// 名次
        /// </summary>
        [Column("ranking_position")]
        public byte RankingPosition { get; set; }

        /// <summary>
        /// 交易額
        /// </summary>
        [Column("trading_amount", TypeName = "decimal(18,2)")]
        public decimal TradingAmount { get; set; }

        /// <summary>
        /// 交易量
        /// </summary>
        [Column("trading_volume")]
        public int TradingVolume { get; set; }

        /// <summary>
        /// 排行榜更新時間
        /// </summary>
        [Column("ranking_updated_at")]
        public DateTime? RankingUpdatedAt { get; set; }

        // 導航屬性
        public virtual ProductInfo ProductInfo { get; set; } = null!;
    }

    /// <summary>
    /// 商品修改日誌表
    /// </summary>
    [Table("ProductInfoAuditLog")]
    public class ProductInfoAuditLog
    {
        /// <summary>
        /// 日誌編號 (主鍵)
        /// </summary>
        [Key]
        [Column("log_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LogId { get; set; }

        /// <summary>
        /// 商品編號 (外鍵到 ProductInfo)
        /// </summary>
        [Column("product_id")]
        [ForeignKey("ProductInfo")]
        public int ProductId { get; set; }

        /// <summary>
        /// 動作類型
        /// </summary>
        [Column("action_type")]
        [StringLength(50)]
        public string? ActionType { get; set; }

        /// <summary>
        /// 修改欄位名稱
        /// </summary>
        [Column("field_name")]
        [StringLength(100)]
        public string? FieldName { get; set; }

        /// <summary>
        /// 舊值
        /// </summary>
        [Column("old_value")]
        [StringLength(1000)]
        public string? OldValue { get; set; }

        /// <summary>
        /// 新值
        /// </summary>
        [Column("new_value")]
        [StringLength(1000)]
        public string? NewValue { get; set; }

        /// <summary>
        /// 操作人編號 (外鍵到 ManagerData)
        /// </summary>
        [Column("Manager_Id")]
        [ForeignKey("ManagerData")]
        public int? ManagerId { get; set; }

        /// <summary>
        /// 修改時間
        /// </summary>
        [Column("changed_at")]
        public DateTime? ChangedAt { get; set; }

        // 導航屬性
        public virtual ProductInfo ProductInfo { get; set; } = null!;
        public virtual ManagerData? ManagerData { get; set; }
    }

    /// <summary>
    /// 商城產品實體
    /// </summary>
    [Table("store_products")]
    public class StoreProduct
    {
        /// <summary>
        /// 產品編號 (主鍵)
        /// </summary>
        [Key]
        [Column("product_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        /// <summary>
        /// 產品名稱
        /// </summary>
        [Required]
        [Column("product_name")]
        [StringLength(200)]
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// 產品描述
        /// </summary>
        [Column("description")]
        public string? Description { get; set; }

        /// <summary>
        /// 產品價格
        /// </summary>
        [Required]
        [Column("price")]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        /// <summary>
        /// 產品類別
        /// </summary>
        [Required]
        [Column("category")]
        [StringLength(50)]
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// 庫存數量
        /// </summary>
        [Column("stock_quantity")]
        public int StockQuantity { get; set; } = 0;

        /// <summary>
        /// 是否啟用
        /// </summary>
        [Column("is_active")]
        public bool IsActive { get; set; } = true;

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
        public virtual ICollection<ShoppingCartItem> CartItems { get; set; } = new List<ShoppingCartItem>();
        public virtual ICollection<StoreOrderItem> OrderItems { get; set; } = new List<StoreOrderItem>();
    }

    /// <summary>
    /// 購物車實體
    /// </summary>
    [Table("shopping_carts")]
    public class ShoppingCart
    {
        /// <summary>
        /// 購物車編號 (主鍵)
        /// </summary>
        [Key]
        [Column("cart_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartId { get; set; }

        /// <summary>
        /// 使用者編號 (外鍵)
        /// </summary>
        [Required]
        [Column("user_id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

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
        public virtual User User { get; set; } = null!;
        public virtual ICollection<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();
    }

    /// <summary>
    /// 購物車項目實體
    /// </summary>
    [Table("shopping_cart_items")]
    public class ShoppingCartItem
    {
        /// <summary>
        /// 項目編號 (主鍵)
        /// </summary>
        [Key]
        [Column("item_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemId { get; set; }

        /// <summary>
        /// 購物車編號 (外鍵)
        /// </summary>
        [Required]
        [Column("cart_id")]
        [ForeignKey("ShoppingCart")]
        public int CartId { get; set; }

        /// <summary>
        /// 產品編號 (外鍵)
        /// </summary>
        [Required]
        [Column("product_id")]
        [ForeignKey("StoreProduct")]
        public int ProductId { get; set; }

        /// <summary>
        /// 數量
        /// </summary>
        [Required]
        [Column("quantity")]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        /// <summary>
        /// 單價
        /// </summary>
        [Required]
        [Column("unit_price")]
        [Range(0, double.MaxValue)]
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 加入時間
        /// </summary>
        [Column("added_at")]
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

        // 導航屬性
        public virtual ShoppingCart ShoppingCart { get; set; } = null!;
        public virtual StoreProduct StoreProduct { get; set; } = null!;
    }

    /// <summary>
    /// 商城訂單實體
    /// </summary>
    [Table("store_orders")]
    public class StoreOrder
    {
        /// <summary>
        /// 訂單編號 (主鍵)
        /// </summary>
        [Key]
        [Column("order_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        /// <summary>
        /// 訂單號碼
        /// </summary>
        [Required]
        [Column("order_number")]
        [StringLength(50)]
        public string OrderNumber { get; set; } = string.Empty;

        /// <summary>
        /// 使用者編號 (外鍵)
        /// </summary>
        [Required]
        [Column("user_id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// 訂單狀態
        /// </summary>
        [Required]
        [Column("status")]
        [StringLength(20)]
        public string Status { get; set; } = "pending";

        /// <summary>
        /// 總金額
        /// </summary>
        [Required]
        [Column("total_amount")]
        [Range(0, double.MaxValue)]
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 運送地址
        /// </summary>
        [Column("shipping_address")]
        public string? ShippingAddress { get; set; }

        /// <summary>
        /// 付款方式
        /// </summary>
        [Column("payment_method")]
        [StringLength(50)]
        public string? PaymentMethod { get; set; }

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
        public virtual User User { get; set; } = null!;
        public virtual ICollection<StoreOrderItem> Items { get; set; } = new List<StoreOrderItem>();
    }

    /// <summary>
    /// 商城訂單項目實體
    /// </summary>
    [Table("store_order_items")]
    public class StoreOrderItem
    {
        /// <summary>
        /// 項目編號 (主鍵)
        /// </summary>
        [Key]
        [Column("item_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemId { get; set; }

        /// <summary>
        /// 訂單編號 (外鍵)
        /// </summary>
        [Required]
        [Column("order_id")]
        [ForeignKey("StoreOrder")]
        public int OrderId { get; set; }

        /// <summary>
        /// 產品編號 (外鍵)
        /// </summary>
        [Required]
        [Column("product_id")]
        [ForeignKey("StoreProduct")]
        public int ProductId { get; set; }

        /// <summary>
        /// 數量
        /// </summary>
        [Required]
        [Column("quantity")]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        /// <summary>
        /// 單價
        /// </summary>
        [Required]
        [Column("unit_price")]
        [Range(0, double.MaxValue)]
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 小計
        /// </summary>
        [Required]
        [Column("subtotal")]
        [Range(0, double.MaxValue)]
        public decimal Subtotal { get; set; }

        // 導航屬性
        public virtual StoreOrder StoreOrder { get; set; } = null!;
        public virtual StoreProduct StoreProduct { get; set; } = null!;
    }
}