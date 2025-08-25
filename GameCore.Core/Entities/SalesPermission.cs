using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 銷售權限表 - 管理用戶的銷售權限申請和審核
    /// </summary>
    [Table("SalesPermission")]
    public class SalesPermission
    {
        /// <summary>
        /// 權限ID - 主鍵，自動遞增
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
        /// 申請狀態 - pending（待審核）、approved（已通過）、rejected（已拒絕）、cancelled（已取消）
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "pending";

        /// <summary>
        /// 申請時間 - 記錄提交申請的時間
        /// </summary>
        [Required]
        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 審核時間 - 記錄管理員審核的時間
        /// </summary>
        public DateTime? ApprovedAt { get; set; }

        /// <summary>
        /// 拒絕時間 - 記錄申請被拒絕的時間
        /// </summary>
        public DateTime? RejectedAt { get; set; }

        /// <summary>
        /// 拒絕原因 - 記錄申請被拒絕的原因
        /// </summary>
        [StringLength(500)]
        public string? RejectionReason { get; set; }

        /// <summary>
        /// 營業執照 - 用戶的營業執照號碼或證明文件
        /// </summary>
        [Required]
        [StringLength(100)]
        public string BusinessLicense { get; set; } = string.Empty;

        /// <summary>
        /// 稅號 - 用戶的統一編號或稅籍編號
        /// </summary>
        [Required]
        [StringLength(50)]
        public string TaxId { get; set; } = string.Empty;

        /// <summary>
        /// 銀行帳戶 - 用戶的銀行帳戶資訊
        /// </summary>
        [Required]
        [StringLength(100)]
        public string BankAccount { get; set; } = string.Empty;

        /// <summary>
        /// 聯絡電話 - 用戶的聯絡電話
        /// </summary>
        [Required]
        [StringLength(20)]
        public string ContactPhone { get; set; } = string.Empty;

        /// <summary>
        /// 營業地址 - 用戶的營業地址
        /// </summary>
        [Required]
        [StringLength(200)]
        public string BusinessAddress { get; set; } = string.Empty;

        /// <summary>
        /// 審核者ID - 記錄審核此申請的管理員ID
        /// </summary>
        public int? ReviewerId { get; set; }

        /// <summary>
        /// 審核者類型 - 記錄審核者類型（如 Admin、Manager 等）
        /// </summary>
        [StringLength(20)]
        public string? ReviewerType { get; set; }

        /// <summary>
        /// 審核備註 - 管理員的審核備註
        /// </summary>
        [StringLength(1000)]
        public string? ReviewNotes { get; set; }

        /// <summary>
        /// 更新時間 - 記錄最後更新此權限的時間
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
        /// 審核者 - 導航到審核者表（可能是 Admin 或 Manager）
        /// </summary>
        [ForeignKey("ReviewerId")]
        public virtual User? Reviewer { get; set; }
    }
}