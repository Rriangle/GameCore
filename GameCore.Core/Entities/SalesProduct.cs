using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 銷售商品表 - 管理用戶的銷售商品資訊
    /// </summary>
    [Table("SalesProduct")]
    public class SalesProduct
    {
        /// <summary>
        /// 商品ID - 主鍵，自動遞增
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// 用戶ID - 外鍵到 Users 表（賣家）
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// 商品名稱 - 商品的名稱
        /// </summary>
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 商品描述 - 商品的詳細描述
        /// </summary>
        [Required]
        [StringLength(2000)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 商品價格 - 商品的售價
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        /// <summary>
        /// 商品庫存 - 商品的庫存數量
        /// </summary>
        [Required]
        public int Stock { get; set; }

        /// <summary>
        /// 商品狀態 - active（上架）、inactive（下架）、sold_out（售完）、deleted（已刪除）
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "active";

        /// <summary>
        /// 商品分類 - 商品的分類
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// 商品標籤 - 商品的標籤，用逗號分隔
        /// </summary>
        [StringLength(500)]
        public string? Tags { get; set; }

        /// <summary>
        /// 商品圖片 - 商品的主要圖片URL
        /// </summary>
        [StringLength(500)]
        public string? MainImage { get; set; }

        /// <summary>
        /// 商品圖片列表 - 商品的所有圖片URL，用逗號分隔
        /// </summary>
        [StringLength(2000)]
        public string? ImageList { get; set; }

        /// <summary>
        /// 商品規格 - 商品的規格資訊（JSON格式）
        /// </summary>
        [StringLength(2000)]
        public string? Specifications { get; set; }

        /// <summary>
        /// 商品重量 - 商品的重量（克）
        /// </summary>
        public decimal? Weight { get; set; }

        /// <summary>
        /// 商品尺寸 - 商品的尺寸（長x寬x高，釐米）
        /// </summary>
        [StringLength(100)]
        public string? Dimensions { get; set; }

        /// <summary>
        /// 運費 - 商品的運費
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal ShippingFee { get; set; } = 0;

        /// <summary>
        /// 是否包郵 - 商品是否包郵
        /// </summary>
        public bool IsFreeShipping { get; set; } = false;

        /// <summary>
        /// 銷售數量 - 商品的累計銷售數量
        /// </summary>
        public int SalesCount { get; set; } = 0;

        /// <summary>
        /// 瀏覽次數 - 商品的累計瀏覽次數
        /// </summary>
        public int ViewCount { get; set; } = 0;

        /// <summary>
        /// 評分 - 商品的平均評分（1-5分）
        /// </summary>
        [Column(TypeName = "decimal(3,2)")]
        public decimal Rating { get; set; } = 0;

        /// <summary>
        /// 評分數量 - 商品的評分數量
        /// </summary>
        public int RatingCount { get; set; } = 0;

        /// <summary>
        /// 創建時間 - 記錄創建此商品的時間
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 更新時間 - 記錄最後更新此商品的時間
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// 上架時間 - 記錄商品上架的時間
        /// </summary>
        public DateTime? ListedAt { get; set; }

        /// <summary>
        /// 下架時間 - 記錄商品下架的時間
        /// </summary>
        public DateTime? UnlistedAt { get; set; }

        /// <summary>
        /// 備註 - 額外的說明資訊
        /// </summary>
        [StringLength(1000)]
        public string? Remarks { get; set; }

        // 導航屬性

        /// <summary>
        /// 賣家 - 導航到 Users 表（賣家）
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User Seller { get; set; } = null!;
    }
}