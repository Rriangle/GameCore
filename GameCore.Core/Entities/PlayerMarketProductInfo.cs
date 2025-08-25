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
        /// 自由市場商品ID (主鍵)
        /// </summary>
        [Key]
        public int p_product_id { get; set; }

        /// <summary>
        /// 自由市場商品類型
        /// </summary>
        [Required]
        [StringLength(100)]
        public string p_product_type { get; set; } = string.Empty;

        /// <summary>
        /// 商品標題 (噱頭標語)
        /// </summary>
        [StringLength(200)]
        public string? p_product_title { get; set; }

        /// <summary>
        /// 自由市場商品名稱
        /// </summary>
        [Required]
        [StringLength(200)]
        public string p_product_name { get; set; } = string.Empty;

        /// <summary>
        /// 自由市場商品描述
        /// </summary>
        [StringLength(2000)]
        public string? p_product_description { get; set; }

        /// <summary>
        /// 商品ID (外鍵到ProductInfo)
        /// </summary>
        [ForeignKey("ProductInfo")]
        public int? product_id { get; set; }

        /// <summary>
        /// 賣家ID (外鍵到Users)
        /// </summary>
        [Required]
        [ForeignKey("Seller")]
        public int seller_id { get; set; }

        /// <summary>
        /// 自由市場商品狀態
        /// </summary>
        [Required]
        [StringLength(50)]
        public string p_status { get; set; } = "Draft";

        /// <summary>
        /// 售價
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal price { get; set; } = 0;

        /// <summary>
        /// 自由市場商品圖片ID
        /// </summary>
        [StringLength(100)]
        public string? p_product_img_id { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime created_at { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime updated_at { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 商品條件 (全新/二手/翻新)
        /// </summary>
        [StringLength(50)]
        public string? condition { get; set; }

        /// <summary>
        /// 商品品牌
        /// </summary>
        [StringLength(100)]
        public string? brand { get; set; }

        /// <summary>
        /// 商品型號
        /// </summary>
        [StringLength(100)]
        public string? model { get; set; }

        /// <summary>
        /// 商品顏色
        /// </summary>
        [StringLength(100)]
        public string? color { get; set; }

        /// <summary>
        /// 商品尺寸
        /// </summary>
        [StringLength(100)]
        public string? size { get; set; }

        /// <summary>
        /// 商品重量
        /// </summary>
        [StringLength(100)]
        public string? weight { get; set; }

        /// <summary>
        /// 商品材質
        /// </summary>
        [StringLength(100)]
        public string? material { get; set; }

        /// <summary>
        /// 庫存數量
        /// </summary>
        public int stock_quantity { get; set; } = 1;

        /// <summary>
        /// 是否可議價
        /// </summary>
        public bool is_negotiable { get; set; } = false;

        /// <summary>
        /// 是否可面交
        /// </summary>
        public bool is_face_to_face { get; set; } = true;

        /// <summary>
        /// 是否可郵寄
        /// </summary>
        public bool is_shipping { get; set; } = true;

        /// <summary>
        /// 運費
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal shipping_fee { get; set; } = 0;

        /// <summary>
        /// 商品標籤
        /// </summary>
        [StringLength(500)]
        public string? tags { get; set; }

        /// <summary>
        /// 瀏覽次數
        /// </summary>
        public int view_count { get; set; } = 0;

        /// <summary>
        /// 收藏次數
        /// </summary>
        public int favorite_count { get; set; } = 0;

        /// <summary>
        /// 是否為熱門商品
        /// </summary>
        public bool is_hot { get; set; } = false;

        /// <summary>
        /// 是否為推薦商品
        /// </summary>
        public bool is_recommended { get; set; } = false;

        // 導航屬性
        /// <summary>
        /// 商品資訊
        /// </summary>
        public virtual ProductInfo? ProductInfo { get; set; }

        /// <summary>
        /// 賣家
        /// </summary>
        public virtual User Seller { get; set; } = null!;

        /// <summary>
        /// 商品圖片
        /// </summary>
        public virtual ICollection<PlayerMarketProductImg> ProductImgs { get; set; } = new List<PlayerMarketProductImg>();

        /// <summary>
        /// 訂單
        /// </summary>
        public virtual ICollection<PlayerMarketOrderInfo> Orders { get; set; } = new List<PlayerMarketOrderInfo>();

        /// <summary>
        /// 排行榜
        /// </summary>
        public virtual ICollection<PlayerMarketRanking> Rankings { get; set; } = new List<PlayerMarketRanking>();
    }
}