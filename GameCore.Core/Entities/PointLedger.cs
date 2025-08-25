using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 點數明細表 - 記錄用戶點數的所有變更明細
    /// </summary>
    [Table("PointLedger")]
    public class PointLedger
    {
        /// <summary>
        /// 明細ID - 主鍵，自動遞增
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
        /// 變更類型 - 例如：signin（簽到）、minigame（小遊戲）、purchase（購買）、adjustment（調整）等
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// 變更金額 - 正數為收入，負數為支出
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        /// <summary>
        /// 變更後餘額 - 記錄此次變更後的點數餘額
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }

        /// <summary>
        /// 變更描述 - 詳細說明此次變更的原因
        /// </summary>
        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 關聯ID - 關聯到其他表的記錄ID（如訂單ID、簽到ID等）
        /// </summary>
        public int? RelatedId { get; set; }

        /// <summary>
        /// 關聯類型 - 關聯表的類型（如 Order、SignIn、MiniGame 等）
        /// </summary>
        [StringLength(50)]
        public string? RelatedType { get; set; }

        /// <summary>
        /// 創建時間 - 記錄創建此明細的時間
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 更新時間 - 記錄最後更新此明細的時間
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// 操作者ID - 記錄執行此操作的管理員ID（如果是管理員操作）
        /// </summary>
        public int? OperatorId { get; set; }

        /// <summary>
        /// 操作者類型 - 記錄操作者類型（如 User、Admin、System 等）
        /// </summary>
        [StringLength(20)]
        public string? OperatorType { get; set; }

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
        /// 操作者 - 導航到操作者表（可能是 User 或 Admin）
        /// </summary>
        [ForeignKey("OperatorId")]
        public virtual User? Operator { get; set; }
    }
}