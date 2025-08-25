using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 提現申請表 - 管理用戶的提現申請和處理
    /// </summary>
    [Table("WithdrawalRequest")]
    public class WithdrawalRequest
    {
        /// <summary>
        /// 申請ID - 主鍵，自動遞增
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
        /// 提現金額 - 申請提現的金額
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        /// <summary>
        /// 銀行帳戶 - 提現的銀行帳戶資訊
        /// </summary>
        [Required]
        [StringLength(100)]
        public string BankAccount { get; set; } = string.Empty;

        /// <summary>
        /// 申請狀態 - pending（待處理）、approved（已通過）、rejected（已拒絕）、completed（已完成）、cancelled（已取消）
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "pending";

        /// <summary>
        /// 申請時間 - 記錄提交申請的時間
        /// </summary>
        [Required]
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 處理時間 - 記錄處理此申請的時間
        /// </summary>
        public DateTime? ProcessedAt { get; set; }

        /// <summary>
        /// 完成時間 - 記錄提現完成的時間
        /// </summary>
        public DateTime? CompletedAt { get; set; }

        /// <summary>
        /// 處理者ID - 記錄處理此申請的管理員ID
        /// </summary>
        public int? ProcessorId { get; set; }

        /// <summary>
        /// 處理者類型 - 記錄處理者類型（如 Admin、Manager 等）
        /// </summary>
        [StringLength(20)]
        public string? ProcessorType { get; set; }

        /// <summary>
        /// 處理備註 - 管理員的處理備註
        /// </summary>
        [StringLength(1000)]
        public string? ProcessNotes { get; set; }

        /// <summary>
        /// 拒絕原因 - 記錄申請被拒絕的原因
        /// </summary>
        [StringLength(500)]
        public string? RejectionReason { get; set; }

        /// <summary>
        /// 手續費 - 提現的手續費金額
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal ProcessingFee { get; set; } = 0;

        /// <summary>
        /// 實際到帳金額 - 扣除手續費後實際到帳的金額
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal NetAmount { get; set; }

        /// <summary>
        /// 更新時間 - 記錄最後更新此申請的時間
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

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
        /// 處理者 - 導航到處理者表（可能是 Admin 或 Manager）
        /// </summary>
        [ForeignKey("ProcessorId")]
        public virtual User? Processor { get; set; }
    }
}