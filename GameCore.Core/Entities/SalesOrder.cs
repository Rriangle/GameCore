using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 銷售訂單表 - 管理用戶的銷售訂單資訊
    /// </summary>
    [Table("SalesOrder")]
    public class SalesOrder
    {
        /// <summary>
        /// 訂單ID - 主鍵，自動遞增
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
        /// 訂單編號 - 唯一的訂單編號
        /// </summary>
        [Required]
        [StringLength(50)]
        public string OrderNumber { get; set; } = string.Empty;

        /// <summary>
        /// 買家ID - 外鍵到 Users 表（買家）
        /// </summary>
        [Required]
        public int BuyerId { get; set; }

        /// <summary>
        /// 訂單狀態 - pending（待處理）、confirmed（已確認）、shipped（已發貨）、delivered（已送達）、completed（已完成）、cancelled（已取消）
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "pending";

        /// <summary>
        /// 訂單總金額 - 訂單的總金額
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 平台手續費 - 平台收取的手續費
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PlatformFee { get; set; } = 0;

        /// <summary>
        /// 賣家收益 - 賣家實際獲得的收益
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SellerEarnings { get; set; }

        /// <summary>
        /// 付款方式 - 買家的付款方式
        /// </summary>
        [Required]
        [StringLength(50)]
        public string PaymentMethod { get; set; } = string.Empty;

        /// <summary>
        /// 付款狀態 - pending（待付款）、paid（已付款）、failed（付款失敗）、refunded（已退款）
        /// </summary>
        [Required]
        [StringLength(20)]
        public string PaymentStatus { get; set; } = "pending";

        /// <summary>
        /// 付款時間 - 記錄付款完成的時間
        /// </summary>
        public DateTime? PaidAt { get; set; }

        /// <summary>
        /// 發貨時間 - 記錄發貨的時間
        /// </summary>
        public DateTime? ShippedAt { get; set; }

        /// <summary>
        /// 送達時間 - 記錄送達的時間
        /// </summary>
        public DateTime? DeliveredAt { get; set; }

        /// <summary>
        /// 完成時間 - 記錄訂單完成的時間
        /// </summary>
        public DateTime? CompletedAt { get; set; }

        /// <summary>
        /// 取消時間 - 記錄訂單取消的時間
        /// </summary>
        public DateTime? CancelledAt { get; set; }

        /// <summary>
        /// 取消原因 - 記錄訂單取消的原因
        /// </summary>
        [StringLength(500)]
        public string? CancellationReason { get; set; }

        /// <summary>
        /// 買家備註 - 買家的訂單備註
        /// </summary>
        [StringLength(1000)]
        public string? BuyerNotes { get; set; }

        /// <summary>
        /// 賣家備註 - 賣家的訂單備註
        /// </summary>
        [StringLength(1000)]
        public string? SellerNotes { get; set; }

        /// <summary>
        /// 創建時間 - 記錄創建此訂單的時間
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 更新時間 - 記錄最後更新此訂單的時間
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

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

        /// <summary>
        /// 買家 - 導航到 Users 表（買家）
        /// </summary>
        [ForeignKey("BuyerId")]
        public virtual User Buyer { get; set; } = null!;
    }
}