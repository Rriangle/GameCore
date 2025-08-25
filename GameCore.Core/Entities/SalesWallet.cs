using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 銷售錢包表 - 管理用戶的銷售收益和餘額
    /// </summary>
    [Table("SalesWallet")]
    public class SalesWallet
    {
        /// <summary>
        /// 錢包ID - 主鍵，自動遞增
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
        /// 總收益 - 用戶的總銷售收益
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalEarnings { get; set; } = 0;

        /// <summary>
        /// 可用餘額 - 用戶可提現的餘額
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal AvailableBalance { get; set; } = 0;

        /// <summary>
        /// 待處理餘額 - 用戶待處理的餘額（如提現申請中）
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PendingBalance { get; set; } = 0;

        /// <summary>
        /// 凍結餘額 - 用戶被凍結的餘額
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal FrozenBalance { get; set; } = 0;

        /// <summary>
        /// 最後更新時間 - 記錄最後更新此錢包的時間
        /// </summary>
        [Required]
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 創建時間 - 記錄創建此錢包的時間
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

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
    }
}