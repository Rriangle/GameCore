using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 優惠券表 - 管理用戶的優惠券資訊
    /// </summary>
    [Table("Coupon")]
    public class Coupon
    {
        /// <summary>
        /// 優惠券ID - 主鍵，自動遞增
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// 用戶ID - 外鍵到 Users 表
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// 優惠券代碼 - 唯一標識符
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// 優惠券類型 - 例如：percentage（百分比折扣）、fixed（固定金額折扣）
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// 優惠券面值 - 百分比類型為折扣百分比，固定類型為折扣金額
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Value { get; set; }

        /// <summary>
        /// 最低使用金額 - 使用此優惠券的最低訂單金額
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal MinAmount { get; set; }

        /// <summary>
        /// 最大折扣金額 - 限制最大折扣金額（主要用於百分比類型）
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MaxDiscount { get; set; }

        /// <summary>
        /// 優惠券狀態 - active（有效）、used（已使用）、expired（已過期）、cancelled（已取消）
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "active";

        /// <summary>
        /// 過期時間 - 優惠券的有效期限
        /// </summary>
        public DateTime? ExpiredAt { get; set; }

        /// <summary>
        /// 使用時間 - 記錄優惠券被使用的時間
        /// </summary>
        public DateTime? UsedAt { get; set; }

        /// <summary>
        /// 使用訂單ID - 記錄使用此優惠券的訂單ID
        /// </summary>
        public int? UsedOrderId { get; set; }

        /// <summary>
        /// 創建時間 - 記錄創建此優惠券的時間
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 更新時間 - 記錄最後更新此優惠券的時間
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// 發放原因 - 記錄發放此優惠券的原因（如活動、獎勵等）
        /// </summary>
        [StringLength(200)]
        public string? IssueReason { get; set; }

        /// <summary>
        /// 發放者ID - 記錄發放此優惠券的管理員ID
        /// </summary>
        public int? IssuerId { get; set; }

        /// <summary>
        /// 發放者類型 - 記錄發放者類型（如 Admin、System 等）
        /// </summary>
        [StringLength(20)]
        public string? IssuerType { get; set; }

        /// <summary>
        /// 備註 - 額外的說明資訊
        /// </summary>
        [StringLength(1000)]
        public string? Remarks { get; set; }

        // 導航屬性

        /// <summary>
        /// 用戶 - 導航到 Users 表
        /// </summary>
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        /// <summary>
        /// 發放者 - 導航到發放者表（可能是 Admin 或 System）
        /// </summary>
        [ForeignKey("IssuerId")]
        public virtual User? Issuer { get; set; }
    }
}