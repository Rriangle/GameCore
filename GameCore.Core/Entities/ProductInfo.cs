using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 商品資訊表
    /// </summary>
    [Table("ProductInfo")]
    public class ProductInfo
    {
        /// <summary>
        /// 商品ID (主鍵)
        /// </summary>
        [Key]
        public int product_id { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        [Required]
        [StringLength(200)]
        public string product_name { get; set; } = string.Empty;

        /// <summary>
        /// 商品類型
        /// </summary>
        [Required]
        [StringLength(100)]
        public string product_type { get; set; } = string.Empty;

        /// <summary>
        /// 售價
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal price { get; set; } = 0;

        /// <summary>
        /// 使用幣別
        /// </summary>
        [StringLength(10)]
        public string currency_code { get; set; } = "TWD";

        /// <summary>
        /// 庫存
        /// </summary>
        public int Shipment_Quantity { get; set; } = 0;

        /// <summary>
        /// 創建者
        /// </summary>
        [StringLength(100)]
        public string product_created_by { get; set; } = string.Empty;

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime product_created_at { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 最後修改者
        /// </summary>
        [StringLength(100)]
        public string product_updated_by { get; set; } = string.Empty;

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime product_updated_at { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 會員ID
        /// </summary>
        public int? user_id { get; set; }

        /// <summary>
        /// 商品狀態 (Active/Inactive/Discontinued)
        /// </summary>
        [StringLength(50)]
        public string Status { get; set; } = "Active";

        /// <summary>
        /// 商品描述
        /// </summary>
        [StringLength(2000)]
        public string? Description { get; set; }

        /// <summary>
        /// 商品標籤
        /// </summary>
        [StringLength(500)]
        public string? Tags { get; set; }

        /// <summary>
        /// 商品圖片URL
        /// </summary>
        [StringLength(500)]
        public string? ImageUrl { get; set; }

        /// <summary>
        /// 是否為熱門商品
        /// </summary>
        public bool IsHot { get; set; } = false;

        /// <summary>
        /// 是否為推薦商品
        /// </summary>
        public bool IsRecommended { get; set; } = false;

        /// <summary>
        /// 銷售數量
        /// </summary>
        public int SalesCount { get; set; } = 0;

        /// <summary>
        /// 評分
        /// </summary>
        [Column(TypeName = "decimal(3,2)")]
        public decimal Rating { get; set; } = 0;

        /// <summary>
        /// 評分數量
        /// </summary>
        public int RatingCount { get; set; } = 0;

        // 導航屬性
        /// <summary>
        /// 使用者
        /// </summary>
        [ForeignKey("user_id")]
        public virtual User? User { get; set; }

        /// <summary>
        /// 遊戲商品詳情
        /// </summary>
        public virtual GameProductDetails? GameProductDetails { get; set; }

        /// <summary>
        /// 其他商品詳情
        /// </summary>
        public virtual OtherProductDetails? OtherProductDetails { get; set; }

        /// <summary>
        /// 訂單項目
        /// </summary>
        public virtual ICollection<OrderItems> OrderItems { get; set; } = new List<OrderItems>();

        /// <summary>
        /// 自由市場商品
        /// </summary>
        public virtual ICollection<PlayerMarketProductInfo> PlayerMarketProducts { get; set; } = new List<PlayerMarketProductInfo>();

        /// <summary>
        /// 商品審計日誌
        /// </summary>
        public virtual ICollection<ProductInfoAuditLog> AuditLogs { get; set; } = new List<ProductInfoAuditLog>();

        /// <summary>
        /// 官方商城排行榜
        /// </summary>
        public virtual ICollection<OfficialStoreRanking> Rankings { get; set; } = new List<OfficialStoreRanking>();
    }
}