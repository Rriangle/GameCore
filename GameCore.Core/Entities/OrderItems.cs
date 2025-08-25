using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 訂單詳細表
    /// </summary>
    [Table("OrderItems")]
    public class OrderItems
    {
        /// <summary>
        /// 訂單詳細ID (主鍵)
        /// </summary>
        [Key]
        public int item_id { get; set; }

        /// <summary>
        /// 訂單ID (外鍵到OrderInfo)
        /// </summary>
        [Required]
        [ForeignKey("OrderInfo")]
        public int order_id { get; set; }

        /// <summary>
        /// 商品ID (外鍵到ProductInfo)
        /// </summary>
        [Required]
        [ForeignKey("ProductInfo")]
        public int product_id { get; set; }

        /// <summary>
        /// 實際物品編號1.2.3…
        /// </summary>
        [Required]
        public int line_no { get; set; }

        /// <summary>
        /// 單價
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal unit_price { get; set; } = 0;

        /// <summary>
        /// 下單數量
        /// </summary>
        [Required]
        public int quantity { get; set; } = 1;

        /// <summary>
        /// 小計
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal subtotal { get; set; } = 0;

        /// <summary>
        /// 折扣金額
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal discount_amount { get; set; } = 0;

        /// <summary>
        /// 稅金
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal tax_amount { get; set; } = 0;

        /// <summary>
        /// 商品名稱快照
        /// </summary>
        [StringLength(200)]
        public string? product_name_snapshot { get; set; }

        /// <summary>
        /// 商品描述快照
        /// </summary>
        [StringLength(1000)]
        public string? product_description_snapshot { get; set; }

        /// <summary>
        /// 商品圖片URL快照
        /// </summary>
        [StringLength(500)]
        public string? product_image_snapshot { get; set; }

        /// <summary>
        /// 是否已出貨
        /// </summary>
        public bool is_shipped { get; set; } = false;

        /// <summary>
        /// 出貨時間
        /// </summary>
        public DateTime? shipped_at { get; set; }

        /// <summary>
        /// 追蹤號碼
        /// </summary>
        [StringLength(100)]
        public string? tracking_number { get; set; }

        /// <summary>
        /// 運送商
        /// </summary>
        [StringLength(100)]
        public string? shipping_carrier { get; set; }

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
        /// 訂單資訊
        /// </summary>
        public virtual OrderInfo OrderInfo { get; set; } = null!;

        /// <summary>
        /// 商品資訊
        /// </summary>
        public virtual ProductInfo ProductInfo { get; set; } = null!;
    }
}