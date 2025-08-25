using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 遊戲主檔商品資訊表
    /// </summary>
    [Table("GameProductDetails")]
    public class GameProductDetails
    {
        /// <summary>
        /// 商品ID (主鍵，外鍵到ProductInfo)
        /// </summary>
        [Key]
        [ForeignKey("ProductInfo")]
        public int product_id { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        [Required]
        [StringLength(200)]
        public string product_name { get; set; } = string.Empty;

        /// <summary>
        /// 商品描述
        /// </summary>
        [StringLength(2000)]
        public string? product_description { get; set; }

        /// <summary>
        /// 廠商ID (外鍵到Supplier)
        /// </summary>
        [Required]
        [ForeignKey("Supplier")]
        public int supplier_id { get; set; }

        /// <summary>
        /// 遊戲平台ID
        /// </summary>
        public int platform_id { get; set; }

        /// <summary>
        /// 遊戲ID (外鍵到Game)
        /// </summary>
        [ForeignKey("Game")]
        public int? game_id { get; set; }

        /// <summary>
        /// 遊戲名稱
        /// </summary>
        [StringLength(200)]
        public string? game_name { get; set; }

        /// <summary>
        /// 下載連結
        /// </summary>
        [StringLength(500)]
        public string? download_link { get; set; }

        /// <summary>
        /// 遊戲版本
        /// </summary>
        [StringLength(50)]
        public string? game_version { get; set; }

        /// <summary>
        /// 遊戲語言
        /// </summary>
        [StringLength(100)]
        public string? game_language { get; set; }

        /// <summary>
        /// 遊戲地區
        /// </summary>
        [StringLength(100)]
        public string? game_region { get; set; }

        /// <summary>
        /// 遊戲類型
        /// </summary>
        [StringLength(100)]
        public string? game_genre { get; set; }

        /// <summary>
        /// 遊戲年齡分級
        /// </summary>
        [StringLength(20)]
        public string? age_rating { get; set; }

        /// <summary>
        /// 遊戲特色
        /// </summary>
        [StringLength(1000)]
        public string? game_features { get; set; }

        /// <summary>
        /// 系統需求
        /// </summary>
        [StringLength(1000)]
        public string? system_requirements { get; set; }

        /// <summary>
        /// 遊戲截圖
        /// </summary>
        [StringLength(2000)]
        public string? game_screenshots { get; set; }

        /// <summary>
        /// 遊戲影片
        /// </summary>
        [StringLength(2000)]
        public string? game_videos { get; set; }

        /// <summary>
        /// 是否為數位版
        /// </summary>
        public bool is_digital { get; set; } = true;

        /// <summary>
        /// 是否為實體版
        /// </summary>
        public bool is_physical { get; set; } = false;

        /// <summary>
        /// 是否為限定版
        /// </summary>
        public bool is_limited_edition { get; set; } = false;

        /// <summary>
        /// 是否為預購商品
        /// </summary>
        public bool is_pre_order { get; set; } = false;

        /// <summary>
        /// 預購開始日期
        /// </summary>
        public DateTime? pre_order_start_date { get; set; }

        /// <summary>
        /// 預購結束日期
        /// </summary>
        public DateTime? pre_order_end_date { get; set; }

        /// <summary>
        /// 發售日期
        /// </summary>
        public DateTime? release_date { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime created_at { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime updated_at { get; set; } = DateTime.UtcNow;

        // 導航屬性
        /// <summary>
        /// 商品資訊
        /// </summary>
        public virtual ProductInfo ProductInfo { get; set; } = null!;

        /// <summary>
        /// 供應商
        /// </summary>
        public virtual Supplier Supplier { get; set; } = null!;

        /// <summary>
        /// 遊戲
        /// </summary>
        public virtual Game? Game { get; set; }
    }
}