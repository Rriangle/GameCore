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
        /// 廠商ID (主鍵)
        /// </summary>
        [Key]
        public int supplier_id { get; set; }

        /// <summary>
        /// 廠商名字
        /// </summary>
        [Required]
        [StringLength(200)]
        public string supplier_name { get; set; } = string.Empty;

        /// <summary>
        /// 廠商代碼
        /// </summary>
        [StringLength(50)]
        public string? supplier_code { get; set; }

        /// <summary>
        /// 廠商類型 (manufacturer/distributor/reseller)
        /// </summary>
        [StringLength(50)]
        public string supplier_type { get; set; } = "manufacturer";

        /// <summary>
        /// 聯絡人姓名
        /// </summary>
        [StringLength(100)]
        public string? contact_person { get; set; }

        /// <summary>
        /// 聯絡電話
        /// </summary>
        [StringLength(20)]
        public string? contact_phone { get; set; }

        /// <summary>
        /// 聯絡電子郵件
        /// </summary>
        [StringLength(100)]
        [EmailAddress]
        public string? contact_email { get; set; }

        /// <summary>
        /// 廠商地址
        /// </summary>
        [StringLength(500)]
        public string? address { get; set; }

        /// <summary>
        /// 廠商網站
        /// </summary>
        [StringLength(200)]
        public string? website { get; set; }

        /// <summary>
        /// 廠商描述
        /// </summary>
        [StringLength(2000)]
        public string? description { get; set; }

        /// <summary>
        /// 合作開始日期
        /// </summary>
        public DateTime? partnership_start_date { get; set; }

        /// <summary>
        /// 合作結束日期
        /// </summary>
        public DateTime? partnership_end_date { get; set; }

        /// <summary>
        /// 合作狀態 (active/inactive/terminated)
        /// </summary>
        [StringLength(50)]
        public string partnership_status { get; set; } = "active";

        /// <summary>
        /// 付款條件
        /// </summary>
        [StringLength(200)]
        public string? payment_terms { get; set; }

        /// <summary>
        /// 信用額度
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal credit_limit { get; set; } = 0;

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool is_active { get; set; } = true;

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
        public virtual ICollection<ProductInfo> ProductInfos { get; set; } = new List<ProductInfo>();

        /// <summary>
        /// 遊戲商品詳情
        /// </summary>
        public virtual ICollection<GameProductDetails> GameProductDetails { get; set; } = new List<GameProductDetails>();

        /// <summary>
        /// 其他商品詳情
        /// </summary>
        public virtual ICollection<OtherProductDetails> OtherProductDetails { get; set; } = new List<OtherProductDetails>();
    }
}