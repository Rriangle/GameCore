using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 開通銷售功能表
    /// </summary>
    [Table("MemberSalesProfile")]
    public class MemberSalesProfile
    {
        /// <summary>
        /// 使用者ID (主鍵，外鍵到Users)
        /// </summary>
        [Key]
        [ForeignKey("User")]
        public int User_Id { get; set; }

        /// <summary>
        /// 銀行代號
        /// </summary>
        [Required]
        public int BankCode { get; set; }

        /// <summary>
        /// 銀行帳號
        /// </summary>
        [Required]
        [StringLength(50)]
        public string BankAccountNumber { get; set; } = string.Empty;

        /// <summary>
        /// 帳戶封面照片
        /// </summary>
        public byte[]? AccountCoverPhoto { get; set; }

        /// <summary>
        /// 銀行名稱
        /// </summary>
        [StringLength(100)]
        public string? BankName { get; set; }

        /// <summary>
        /// 帳戶持有人姓名
        /// </summary>
        [StringLength(100)]
        public string? AccountHolderName { get; set; }

        /// <summary>
        /// 申請狀態 (pending/approved/rejected)
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "pending";

        /// <summary>
        /// 申請時間
        /// </summary>
        public DateTime applied_at { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 審核時間
        /// </summary>
        public DateTime? reviewed_at { get; set; }

        /// <summary>
        /// 審核者ID
        /// </summary>
        public int? reviewed_by { get; set; }

        /// <summary>
        /// 審核結果
        /// </summary>
        [StringLength(500)]
        public string? review_notes { get; set; }

        /// <summary>
        /// 拒絕原因
        /// </summary>
        [StringLength(1000)]
        public string? rejection_reason { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool is_active { get; set; } = false;

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
        /// 審核者
        /// </summary>
        [ForeignKey("reviewed_by")]
        public virtual ManagerData? ReviewedBy { get; set; }
    }
}