using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Domain.Entities
{
    /// <summary>
    /// 供�??�表
    /// </summary>
    [Table("Supplier")]
    public class Supplier
    {
        /// <summary>
        /// 廠�?編�? (主鍵)
        /// </summary>
        [Key]
        [Column("supplier_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SupplierId { get; set; }

        /// <summary>
        /// 廠�??�稱
        /// </summary>
        [Column("supplier_name")]
        [StringLength(200)]
        public string? SupplierName { get; set; }

        // 導航屬�?
        public virtual ICollection<GameProductDetails> GameProductDetails { get; set; } = new List<GameProductDetails>();
        public virtual ICollection<OtherProductDetails> OtherProductDetails { get; set; } = new List<OtherProductDetails>();
    }

    /// <summary>
    /// ?��?資�?�?
    /// </summary>
    [Table("ProductInfo")]
    public class ProductInfo
    {
        /// <summary>
        /// ?��?編�? (主鍵)
        /// </summary>
        [Key]
        [Column("product_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        /// <summary>
        /// ?��??�稱
        /// </summary>
        [Column("product_name")]
        [StringLength(200)]
        public string? ProductName { get; set; }

        /// <summary>
        /// ?��?類�?
        /// </summary>
        [Column("product_type")]
        [StringLength(100)]
        public string? ProductType { get; set; }

        /// <summary>
        /// ?�價
        /// </summary>
        [Column("price", TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        /// <summary>
        /// 使用�?��
        /// </summary>
        [Column("currency_code")]
        [StringLength(10)]
        public string? CurrencyCode { get; set; }

        /// <summary>
        /// 庫�?
        /// </summary>
        [Column("Shipment_Quantity")]
        public int ShipmentQuantity { get; set; }

        /// <summary>
        /// ?�建??
        /// </summary>
        [Column("product_created_by")]
        [StringLength(100)]
        public string? ProductCreatedBy { get; set; }

        /// <summary>
        /// 建�??��?
        /// </summary>
        [Column("product_created_at")]
        public DateTime? ProductCreatedAt { get; set; }

        /// <summary>
        /// ?�後修?��?
        /// </summary>
        [Column("product_updated_by")]
        [StringLength(100)]
        public string? ProductUpdatedBy { get; set; }

        /// <summary>
        /// ?�新?��?
        /// </summary>
        [Column("product_updated_at")]
        public DateTime? ProductUpdatedAt { get; set; }

        /// <summary>
        /// ?�員編�?
        /// </summary>
        [Column("user_id")]
        public int? UserId { get; set; }

        // 導航屬�?
        public virtual GameProductDetails? GameProductDetails { get; set; }
        public virtual OtherProductDetails? OtherProductDetails { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public virtual ICollection<OfficialStoreRanking> OfficialStoreRankings { get; set; } = new List<OfficialStoreRanking>();
        public virtual ICollection<ProductInfoAuditLog> AuditLogs { get; set; } = new List<ProductInfoAuditLog>();
        public virtual ICollection<PlayerMarketProductInfo> PlayerMarketProducts { get; set; } = new List<PlayerMarketProductInfo>();
    }

    /// <summary>
    /// ?�戲主�??��?資�?�?
    /// </summary>
    [Table("GameProductDetails")]
    public class GameProductDetails
    {
        /// <summary>
        /// ?��?編�? (主鍵, 外鍵??ProductInfo)
        /// </summary>
        [Key]
        [Column("product_id")]
        [ForeignKey("ProductInfo")]
        public int ProductId { get; set; }

        /// <summary>
        /// ?��??�稱
        /// </summary>
        [Column("product_name")]
        [StringLength(200)]
        public string? ProductName { get; set; }

        /// <summary>
        /// ?��??�述
        /// </summary>
        [Column("product_description")]
        [StringLength(1000)]
        public string? ProductDescription { get; set; }

        /// <summary>
        /// 廠�?編�? (外鍵??Supplier)
        /// </summary>
        [Column("supplier_id")]
        [ForeignKey("Supplier")]
        public int? SupplierId { get; set; }

        /// <summary>
        /// ?�戲平台編�?
        /// </summary>
        [Column("platform_id")]
        public int? PlatformId { get; set; }

        /// <summary>
        /// ?�戲編�? (外鍵??games)
        /// </summary>
        [Column("game_id")]
        [ForeignKey("Game")]
        public int? GameId { get; set; }

        /// <summary>
        /// ?�戲?�稱
        /// </summary>
        [Column("game_name")]
        [StringLength(200)]
        public string? GameName { get; set; }

        /// <summary>
        /// 下�????
        /// </summary>
        [Column("download_link")]
        [StringLength(500)]
        public string? DownloadLink { get; set; }

        // 導航屬�?
        public virtual ProductInfo ProductInfo { get; set; } = null!;
        public virtual Supplier? Supplier { get; set; }
        public virtual Game? Game { get; set; }
    }

    /// <summary>
    /// ?��??�主檔�??��?訊表
    /// </summary>
    [Table("OtherProductDetails")]
    public class OtherProductDetails
    {
        /// <summary>
        /// ?��?編�? (主鍵, 外鍵??ProductInfo)
        /// </summary>
        [Key]
        [Column("product_id")]
        [ForeignKey("ProductInfo")]
        public int ProductId { get; set; }

        /// <summary>
        /// ?��??�稱
        /// </summary>
        [Column("product_name")]
        [StringLength(200)]
        public string? ProductName { get; set; }

        /// <summary>
        /// ?��??�述
        /// </summary>
        [Column("product_description")]
        [StringLength(1000)]
        public string? ProductDescription { get; set; }

        /// <summary>
        /// 廠�?編�? (外鍵??Supplier)
        /// </summary>
        [Column("supplier_id")]
        [ForeignKey("Supplier")]
        public int? SupplierId { get; set; }

        /// <summary>
        /// ?�戲平台編�?
        /// </summary>
        [Column("platform_id")]
        public int? PlatformId { get; set; }

        /// <summary>
        /// ?��?序�??��?�?
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
        /// ?��?
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
        /// ?�質
        /// </summary>
        [Column("material")]
        [StringLength(100)]
        public string? Material { get; set; }

        /// <summary>
        /// 庫�??��?
        /// </summary>
        [Column("stock_quantity")]
        [StringLength(50)]
        public string? StockQuantity { get; set; }

        // 導航屬�?
        public virtual ProductInfo ProductInfo { get; set; } = null!;
        public virtual Supplier? Supplier { get; set; }
    }

    /// <summary>
    /// 訂單資�?�?
    /// </summary>
    [Table("OrderInfo")]
    public class OrderInfo
    {
        /// <summary>
        /// 訂單編�? (主鍵)
        /// </summary>
        [Key]
        [Column("order_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        /// <summary>
        /// 下�??�員編�? (外鍵??Users)
        /// </summary>
        [Column("user_id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// 下單?��?
        /// </summary>
        [Column("order_date")]
        public DateTime? OrderDate { get; set; }

        /// <summary>
        /// 訂單?�??(Created/ToShip/Shipped/Completed)
        /// </summary>
        [Column("order_status")]
        [StringLength(50)]
        public string? OrderStatus { get; set; }

        /// <summary>
        /// 付款?�??(Placed/Pending/Paid)
        /// </summary>
        [Column("payment_status")]
        [StringLength(50)]
        public string? PaymentStatus { get; set; }

        /// <summary>
        /// 訂單總�?
        /// </summary>
        [Column("order_total", TypeName = "decimal(18,2)")]
        public decimal OrderTotal { get; set; }

        /// <summary>
        /// 付款?��?
        /// </summary>
        [Column("payment_at")]
        public DateTime? PaymentAt { get; set; }

        /// <summary>
        /// ?�貨?��?
        /// </summary>
        [Column("shipped_at")]
        public DateTime? ShippedAt { get; set; }

        /// <summary>
        /// 完�??��?
        /// </summary>
        [Column("completed_at")]
        public DateTime? CompletedAt { get; set; }

        // 導航屬�?
        public virtual User User { get; set; } = null!;
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }



    /// <summary>
    /// 官方?��??��?榜表
    /// </summary>
    [Table("Official_Store_Ranking")]
    public class OfficialStoreRanking
    {
        /// <summary>
        /// ?��?榜�?水�? (主鍵)
        /// </summary>
        [Key]
        [Column("ranking_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RankingId { get; set; }

        /// <summary>
        /// 榜單?��? (?�、�??�季?�年)
        /// </summary>
        [Column("period_type")]
        [StringLength(50)]
        public string? PeriodType { get; set; }

        /// <summary>
        /// 榜單?��? (計�??��?)
        /// </summary>
        [Column("ranking_date")]
        public DateTime RankingDate { get; set; }

        /// <summary>
        /// ?��?編�? (外鍵??ProductInfo)
        /// </summary>
        [Column("product_ID")]
        [ForeignKey("ProductInfo")]
        public int ProductId { get; set; }

        /// <summary>
        /// ?��??��? (交�?�???
        /// </summary>
        [Column("ranking_metric")]
        [StringLength(50)]
        public string? RankingMetric { get; set; }

        /// <summary>
        /// ?�次
        /// </summary>
        [Column("ranking_position")]
        public byte RankingPosition { get; set; }

        /// <summary>
        /// 交�?�?
        /// </summary>
        [Column("trading_amount", TypeName = "decimal(18,2)")]
        public decimal TradingAmount { get; set; }

        /// <summary>
        /// 交�???
        /// </summary>
        [Column("trading_volume")]
        public int TradingVolume { get; set; }

        /// <summary>
        /// ?��?榜更?��???
        /// </summary>
        [Column("ranking_updated_at")]
        public DateTime? RankingUpdatedAt { get; set; }

        // 導航屬�?
        public virtual ProductInfo ProductInfo { get; set; } = null!;
    }

    /// <summary>
    /// ?��?修改?��?�?
    /// </summary>
    [Table("ProductInfoAuditLog")]
    public class ProductInfoAuditLog
    {
        /// <summary>
        /// ?��?編�? (主鍵)
        /// </summary>
        [Key]
        [Column("log_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LogId { get; set; }

        /// <summary>
        /// ?��?編�? (外鍵??ProductInfo)
        /// </summary>
        [Column("product_id")]
        [ForeignKey("ProductInfo")]
        public int ProductId { get; set; }

        /// <summary>
        /// ?��?類�?
        /// </summary>
        [Column("action_type")]
        [StringLength(50)]
        public string? ActionType { get; set; }

        /// <summary>
        /// 修改欄�??�稱
        /// </summary>
        [Column("field_name")]
        [StringLength(100)]
        public string? FieldName { get; set; }

        /// <summary>
        /// ?��?
        /// </summary>
        [Column("old_value")]
        [StringLength(1000)]
        public string? OldValue { get; set; }

        /// <summary>
        /// ?��?
        /// </summary>
        [Column("new_value")]
        [StringLength(1000)]
        public string? NewValue { get; set; }

        /// <summary>
        /// ?��?人編??(外鍵??ManagerData)
        /// </summary>
        [Column("Manager_Id")]
        [ForeignKey("ManagerData")]
        public int? ManagerId { get; set; }

        /// <summary>
        /// 修改?��?
        /// </summary>
        [Column("changed_at")]
        public DateTime? ChangedAt { get; set; }

        // 導航屬�?
        public virtual ProductInfo ProductInfo { get; set; } = null!;
        public virtual ManagerData? ManagerData { get; set; }
    }

    /// <summary>
    /// ?��??��?實�?
    /// </summary>
    [Table("store_products")]
    public class StoreProduct
    {
        /// <summary>
        /// ?��?編�? (主鍵)
        /// </summary>
        [Key]
        [Column("product_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        /// <summary>
        /// ?��??�稱
        /// </summary>
        [Required]
        [Column("product_name")]
        [StringLength(200)]
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// ?��??�述
        /// </summary>
        [Column("description")]
        public string? Description { get; set; }

        /// <summary>
        /// ?��??�格
        /// </summary>
        [Required]
        [Column("price", TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        /// <summary>
        /// ?��?類別
        /// </summary>
        [Required]
        [Column("category")]
        [StringLength(50)]
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// 庫�??��?
        /// </summary>
        [Column("stock_quantity")]
        public int StockQuantity { get; set; } = 0;

        /// <summary>
        /// ?��??��?URL
        /// </summary>
        [Column("image_url")]
        [StringLength(500)]
        public string? ImageUrl { get; set; }

        /// <summary>
        /// ?�否?�用
        /// </summary>
        [Column("is_active")]
        public bool IsActive { get; set; } = true;

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

        /// <summary>
        /// ?��?ID (?�於?��?層兼容�?
        /// </summary>
        [NotMapped]
        public int Id => ProductId;

        /// <summary>
        /// ?��??�稱 (?�於?��?層兼容�?
        /// </summary>
        [NotMapped]
        public string Name => ProductName;

        // 導航屬�?
        public virtual ICollection<ShoppingCartItem> CartItems { get; set; } = new List<ShoppingCartItem>();
        public virtual ICollection<StoreOrderItem> OrderItems { get; set; } = new List<StoreOrderItem>();
    }

    /// <summary>
    /// 購物車實�?
    /// </summary>
    [Table("shopping_carts")]
    public class ShoppingCart
    {
        /// <summary>
        /// 購物車編??(主鍵)
        /// </summary>
        [Key]
        [Column("cart_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartId { get; set; }

        /// <summary>
        /// 使用?�編??(外鍵)
        /// </summary>
        [Required]
        [Column("user_id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

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
        public virtual User User { get; set; } = null!;
        public virtual ICollection<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();
    }

    /// <summary>
    /// 購物車�??�實�?
    /// </summary>
    [Table("shopping_cart_items")]
    public class ShoppingCartItem
    {
        /// <summary>
        /// ?�目編�? (主鍵)
        /// </summary>
        [Key]
        [Column("item_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemId { get; set; }

        /// <summary>
        /// 購物車編??(外鍵)
        /// </summary>
        [Required]
        [Column("cart_id")]
        [ForeignKey("ShoppingCart")]
        public int CartId { get; set; }

        /// <summary>
        /// ?��?編�? (外鍵)
        /// </summary>
        [Required]
        [Column("product_id")]
        [ForeignKey("StoreProduct")]
        public int ProductId { get; set; }

        /// <summary>
        /// ?��?
        /// </summary>
        [Required]
        [Column("quantity")]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        /// <summary>
        /// ?�價
        /// </summary>
        [Required]
        [Column("unit_price")]
        [Range(0, double.MaxValue)]
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// ?�入?��?
        /// </summary>
        [Column("added_at")]
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

        // 導航屬�?
        public virtual ShoppingCart ShoppingCart { get; set; } = null!;
        public virtual StoreProduct StoreProduct { get; set; } = null!;
    }

    /// <summary>
    /// ?��?訂單實�?
    /// </summary>
    [Table("store_orders")]
    public class StoreOrder
    {
        /// <summary>
        /// 訂單編�? (主鍵)
        /// </summary>
        [Key]
        [Column("order_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        /// <summary>
        /// 訂單?�碼
        /// </summary>
        [Required]
        [Column("order_number")]
        [StringLength(50)]
        public string OrderNumber { get; set; } = string.Empty;

        /// <summary>
        /// 使用?�編??(外鍵)
        /// </summary>
        [Required]
        [Column("user_id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// 訂單?�??
        /// </summary>
        [Required]
        [Column("status")]
        [StringLength(20)]
        public string Status { get; set; } = "pending";

        /// <summary>
        /// 總�?�?
        /// </summary>
        [Required]
        [Column("total_amount")]
        [Range(0, double.MaxValue)]
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// ?�送地?�
        /// </summary>
        [Column("shipping_address")]
        public string? ShippingAddress { get; set; }

        /// <summary>
        /// 付款?��?
        /// </summary>
        [Column("payment_method")]
        [StringLength(50)]
        public string? PaymentMethod { get; set; }

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
        public virtual User User { get; set; } = null!;
        public virtual ICollection<StoreOrderItem> Items { get; set; } = new List<StoreOrderItem>();
    }

    /// <summary>
    /// ?��?訂單?�目實�?
    /// </summary>
    [Table("store_order_items")]
    public class StoreOrderItem
    {
        /// <summary>
        /// ?�目編�? (主鍵)
        /// </summary>
        [Key]
        [Column("item_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemId { get; set; }

        /// <summary>
        /// 訂單編�? (外鍵)
        /// </summary>
        [Required]
        [Column("order_id")]
        [ForeignKey("StoreOrder")]
        public int OrderId { get; set; }

        /// <summary>
        /// ?��?編�? (外鍵)
        /// </summary>
        [Required]
        [Column("product_id")]
        [ForeignKey("StoreProduct")]
        public int ProductId { get; set; }

        /// <summary>
        /// ?��?
        /// </summary>
        [Required]
        [Column("quantity")]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        /// <summary>
        /// ?�價
        /// </summary>
        [Required]
        [Column("unit_price")]
        [Range(0, double.MaxValue)]
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 小�?
        /// </summary>
        [Required]
        [Column("subtotal")]
        [Range(0, double.MaxValue)]
        public decimal Subtotal { get; set; }

        // 導航屬�?
        public virtual StoreOrder StoreOrder { get; set; } = null!;
        public virtual StoreProduct StoreProduct { get; set; } = null!;
    }

    /// <summary>
    /// 商城商品分類
    /// </summary>
    [Table("StoreCategory")]
    public class StoreCategory
    {
        /// <summary>
        /// 分類ID (主鍵)
        /// </summary>
        [Key]
        [Column("category_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }

        /// <summary>
        /// 分類名稱
        /// </summary>
        [Column("name")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 分類描述
        /// </summary>
        [Column("description")]
        [StringLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// 分類圖標
        /// </summary>
        [Column("icon")]
        [StringLength(100)]
        public string? Icon { get; set; }

        /// <summary>
        /// 排序順序
        /// </summary>
        [Column("sort_order")]
        public int SortOrder { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        [Column("is_active")]
        public bool IsActive { get; set; }

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
        public virtual ICollection<StoreProduct> Products { get; set; } = new List<StoreProduct>();
    }

    /// <summary>
    /// 商城購物車項目
    /// </summary>
    [Table("StoreCartItem")]
    public class StoreCartItem
    {
        /// <summary>
        /// 購物車項目ID (主鍵)
        /// </summary>
        [Key]
        [Column("cart_item_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartItemId { get; set; }

        /// <summary>
        /// 用戶ID
        /// </summary>
        [Column("user_id")]
        public int UserId { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>
        [Column("product_id")]
        public int ProductId { get; set; }

        /// <summary>
        /// 數量
        /// </summary>
        [Column("quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// 添加到購物車時間
        /// </summary>
        [Column("added_at")]
        public DateTime AddedAt { get; set; }

        // 導航屬性
        public virtual StoreProduct Product { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }

    /// <summary>
    /// 商城商品評價
    /// </summary>
    [Table("StoreProductReview")]
    public class StoreProductReview
    {
        /// <summary>
        /// 評價ID (主鍵)
        /// </summary>
        [Key]
        [Column("review_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReviewId { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>
        [Column("product_id")]
        public int ProductId { get; set; }

        /// <summary>
        /// 用戶ID
        /// </summary>
        [Column("user_id")]
        public int UserId { get; set; }

        /// <summary>
        /// 評分 (1-5)
        /// </summary>
        [Column("rating")]
        public int Rating { get; set; }

        /// <summary>
        /// 評價內容
        /// </summary>
        [Column("comment")]
        [StringLength(1000)]
        public string? Comment { get; set; }

        /// <summary>
        /// 評價時間
        /// </summary>
        [Column("reviewed_at")]
        public DateTime ReviewedAt { get; set; }

        // 導航屬性
        public virtual StoreProduct Product { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }

    /// <summary>
    /// 商城熱門商品統計
    /// </summary>
    [Table("StorePopularProduct")]
    public class StorePopularProduct
    {
        /// <summary>
        /// 統計ID (主鍵)
        /// </summary>
        [Key]
        [Column("popular_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PopularId { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>
        [Column("product_id")]
        public int ProductId { get; set; }

        /// <summary>
        /// 銷售數量
        /// </summary>
        [Column("sales_count")]
        public int SalesCount { get; set; }

        /// <summary>
        /// 瀏覽次數
        /// </summary>
        [Column("view_count")]
        public int ViewCount { get; set; }

        /// <summary>
        /// 統計日期
        /// </summary>
        [Column("stat_date")]
        public DateTime StatDate { get; set; }

        // 導航屬性
        public virtual StoreProduct Product { get; set; } = null!;
    }
}
