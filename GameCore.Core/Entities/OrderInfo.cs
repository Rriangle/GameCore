using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 訂單資訊表
    /// </summary>
    [Table("OrderInfo")]
    public class OrderInfo
    {
        /// <summary>
        /// 訂單ID (主鍵)
        /// </summary>
        [Key]
        public int order_id { get; set; }

        /// <summary>
        /// 下訂會員ID (外鍵到Users)
        /// </summary>
        [Required]
        [ForeignKey("User")]
        public int user_id { get; set; }

        /// <summary>
        /// 下單日期
        /// </summary>
        [Required]
        public DateTime order_date { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 訂單狀態 (未付款-為出貨-已出貨-已完成)
        /// </summary>
        [Required]
        [StringLength(50)]
        public string order_status { get; set; } = "Created";

        /// <summary>
        /// 付款狀態 (下單-待付款-已付款)
        /// </summary>
        [Required]
        [StringLength(50)]
        public string payment_status { get; set; } = "Placed";

        /// <summary>
        /// 訂單總額
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal order_total { get; set; } = 0;

        /// <summary>
        /// 付款時間
        /// </summary>
        public DateTime? payment_at { get; set; }

        /// <summary>
        /// 出貨時間
        /// </summary>
        public DateTime? shipped_at { get; set; }

        /// <summary>
        /// 完成時間
        /// </summary>
        public DateTime? completed_at { get; set; }

        /// <summary>
        /// 收貨人姓名
        /// </summary>
        [StringLength(100)]
        public string? receiver_name { get; set; }

        /// <summary>
        /// 收貨人電話
        /// </summary>
        [StringLength(20)]
        public string? receiver_phone { get; set; }

        /// <summary>
        /// 收貨地址
        /// </summary>
        [StringLength(500)]
        public string? shipping_address { get; set; }

        /// <summary>
        /// 運費
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal shipping_fee { get; set; } = 0;

        /// <summary>
        /// 稅金
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal tax_amount { get; set; } = 0;

        /// <summary>
        /// 折扣金額
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal discount_amount { get; set; } = 0;

        /// <summary>
        /// 付款方式
        /// </summary>
        [StringLength(100)]
        public string? payment_method { get; set; }

        /// <summary>
        /// 訂單備註
        /// </summary>
        [StringLength(1000)]
        public string? notes { get; set; }

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
        /// 使用者
        /// </summary>
        public virtual User User { get; set; } = null!;

        /// <summary>
        /// 訂單項目
        /// </summary>
        public virtual ICollection<OrderItems> OrderItems { get; set; } = new List<OrderItems>();
    }
}