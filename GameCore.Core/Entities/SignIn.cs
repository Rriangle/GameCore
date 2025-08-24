using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 簽到記錄實體 (對應資料庫 UserSignInStats 表)
    /// </summary>
    [Table("UserSignInStats")]
    public class UserSignInStats
    {
        /// <summary>
        /// 簽到記錄 ID，自動遞增
        /// </summary>
        [Key]
        [Column("LogID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogId { get; set; }

        /// <summary>
        /// 簽到時間（預設 UTC 當下時間）
        /// </summary>
        [Required]
        [Column("SignTime")]
        public DateTime SignTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 會員 ID，外鍵參考 Users.UserID
        /// </summary>
        [Required]
        [Column("UserID")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// 此次簽到會員點數增減數量
        /// </summary>
        [Required]
        [Column("PointsChanged")]
        public int PointsChanged { get; set; } = 0;

        /// <summary>
        /// 此次簽到寵物獲得經驗值
        /// </summary>
        [Required]
        [Column("ExpGained")]
        public int ExpGained { get; set; } = 0;

        /// <summary>
        /// 點數變動時間
        /// </summary>
        [Required]
        [Column("PointsChangedTime")]
        public DateTime PointsChangedTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 寵物經驗值獲得時間
        /// </summary>
        [Required]
        [Column("ExpGainedTime")]
        public DateTime ExpGainedTime { get; set; } = DateTime.UtcNow;

        // 導航屬性
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }
    /// <summary>
    /// 簽到記錄實體
    /// </summary>
    [Table("SignInRecords")]
    public class SignInRecord
    {
        [Key]
        public int SignInId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime SignInDate { get; set; }

        [Required]
        public int PointsEarned { get; set; }

        [Required]
        public int ExperienceEarned { get; set; }

        [Required]
        public bool IsHoliday { get; set; }

        [Required]
        public bool IsConsecutive { get; set; }

        [Required]
        public int ConsecutiveDays { get; set; }

        [Required]
        public bool IsMonthlyPerfect { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        // 導航屬性
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }

    /// <summary>
    /// 簽到統計實體
    /// </summary>
    [Table("SignInStatistics")]
    public class SignInStatistics
    {
        [Key]
        public int StatisticsId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public int Month { get; set; }

        [Required]
        public int TotalSignInDays { get; set; }

        [Required]
        public int ConsecutiveDays { get; set; }

        [Required]
        public int MaxConsecutiveDays { get; set; }

        [Required]
        public int TotalPointsEarned { get; set; }

        [Required]
        public int TotalExperienceEarned { get; set; }

        [Required]
        public bool IsMonthlyPerfect { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        // 導航屬性
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }
}