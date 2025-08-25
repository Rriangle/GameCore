using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 銷售目標表 - 管理用戶的銷售目標設定和達成情況
    /// </summary>
    [Table("SalesTarget")]
    public class SalesTarget
    {
        /// <summary>
        /// 目標ID - 主鍵，自動遞增
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
        /// 目標類型 - daily（日目標）、weekly（週目標）、monthly（月目標）、quarterly（季目標）、yearly（年目標）
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// 目標期間 - 目標的開始日期
        /// </summary>
        [Required]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 目標期間 - 目標的結束日期
        /// </summary>
        [Required]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 銷售金額目標 - 目標銷售金額
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalesAmountTarget { get; set; }

        /// <summary>
        /// 銷售數量目標 - 目標銷售數量
        /// </summary>
        [Required]
        public int SalesCountTarget { get; set; }

        /// <summary>
        /// 訂單數量目標 - 目標訂單數量
        /// </summary>
        [Required]
        public int OrderCountTarget { get; set; }

        /// <summary>
        /// 客戶數量目標 - 目標客戶數量
        /// </summary>
        [Required]
        public int CustomerCountTarget { get; set; }

        /// <summary>
        /// 實際銷售金額 - 實際達成的銷售金額
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ActualSalesAmount { get; set; } = 0;

        /// <summary>
        /// 實際銷售數量 - 實際達成的銷售數量
        /// </summary>
        [Required]
        public int ActualSalesCount { get; set; } = 0;

        /// <summary>
        /// 實際訂單數量 - 實際達成的訂單數量
        /// </summary>
        [Required]
        public int ActualOrderCount { get; set; } = 0;

        /// <summary>
        /// 實際客戶數量 - 實際達成的客戶數量
        /// </summary>
        [Required]
        public int ActualCustomerCount { get; set; } = 0;

        /// <summary>
        /// 達成率 - 銷售金額達成率（百分比）
        /// </summary>
        [Column(TypeName = "decimal(5,2)")]
        public decimal CompletionRate { get; set; } = 0;

        /// <summary>
        /// 目標狀態 - active（進行中）、completed（已完成）、failed（未達成）、cancelled（已取消）
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "active";

        /// <summary>
        /// 完成時間 - 記錄目標完成的時間
        /// </summary>
        public DateTime? CompletedAt { get; set; }

        /// <summary>
        /// 獎勵點數 - 達成目標的獎勵點數
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal RewardPoints { get; set; } = 0;

        /// <summary>
        /// 獎勵狀態 - pending（待發放）、issued（已發放）、cancelled（已取消）
        /// </summary>
        [Required]
        [StringLength(20)]
        public string RewardStatus { get; set; } = "pending";

        /// <summary>
        /// 獎勵發放時間 - 記錄獎勵發放的時間
        /// </summary>
        public DateTime? RewardIssuedAt { get; set; }

        /// <summary>
        /// 創建時間 - 記錄創建此目標的時間
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 更新時間 - 記錄最後更新此目標的時間
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
    }
}