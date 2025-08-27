using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 玩家市場商品資訊實體
    /// </summary>
    [Table("player_market_product_infos")]
    public class PlayerMarketProductInfo
    {
        /// <summary>
        /// 商品編號 (主鍵)
        /// </summary>
        [Key]
        [Column("product_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        /// <summary>
        /// 賣家編號 (外鍵)
        /// </summary>
        [Required]
        [Column("seller_id")]
        [ForeignKey("Seller")]
        public int SellerId { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        [Required]
        [Column("product_name")]
        [StringLength(100)]
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// 商品描述
        /// </summary>
        [Column("description")]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 商品價格
        /// </summary>
        [Required]
        [Column("price", TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        /// <summary>
        /// 商品類別
        /// </summary>
        [Required]
        [Column("category")]
        [StringLength(50)]
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// 商品狀態
        /// </summary>
        [Required]
        [Column("status")]
        [StringLength(20)]
        public string Status { get; set; } = "Active";

        /// <summary>
        /// 庫存數量
        /// </summary>
        [Column("stock_quantity")]
        public int StockQuantity { get; set; } = 1;

        /// <summary>
        /// 商品圖片URL
        /// </summary>
        [Column("image_url")]
        [StringLength(255)]
        public string? ImageUrl { get; set; }

        /// <summary>
        /// 商品標籤
        /// </summary>
        [Column("tags")]
        [StringLength(200)]
        public string? Tags { get; set; }

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

        /// <summary>
        /// 是否為精選商品
        /// </summary>
        [Column("is_featured")]
        public bool IsFeatured { get; set; } = false;

        /// <summary>
        /// 瀏覽次數
        /// </summary>
        [Column("view_count")]
        public int ViewCount { get; set; } = 0;

        /// <summary>
        /// 銷售次數
        /// </summary>
        [Column("sold_count")]
        public int SoldCount { get; set; } = 0;

        /// <summary>
        /// 評分
        /// </summary>
        [Column("rating", TypeName = "decimal(3,2)")]
        public decimal Rating { get; set; } = 0.00m;

        /// <summary>
        /// 評分數量
        /// </summary>
        [Column("rating_count")]
        public int RatingCount { get; set; } = 0;

        // 導航屬性
        public virtual User Seller { get; set; } = null!;
        public virtual ICollection<MarketTransaction> Transactions { get; set; } = new List<MarketTransaction>();
        public virtual ICollection<MarketReview> Reviews { get; set; } = new List<MarketReview>();
    }
} 