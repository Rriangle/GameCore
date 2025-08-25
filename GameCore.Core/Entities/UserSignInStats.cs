using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 使用者簽到統計表
    /// </summary>
    [Table("UserSignInStats")]
    public class UserSignInStats
    {
        /// <summary>
        /// 簽到記錄ID (主鍵，自動遞增)
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogID { get; set; }

        /// <summary>
        /// 簽到時間 (預設UTC當下時間)
        /// </summary>
        [Required]
        public DateTime SignTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 會員ID (外鍵參考Users)
        /// </summary>
        [Required]
        [ForeignKey("User")]
        public int UserID { get; set; }

        /// <summary>
        /// 此次簽到會員點數增減數量
        /// </summary>
        public int PointsChanged { get; set; } = 0;

        /// <summary>
        /// 此次簽到寵物獲得經驗值
        /// </summary>
        public int ExpGained { get; set; } = 0;

        /// <summary>
        /// 點數變動時間
        /// </summary>
        public DateTime PointsChangedTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 寵物經驗值獲得時間
        /// </summary>
        public DateTime ExpGainedTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 簽到類型 (平日/假日/連續7天/當月全勤)
        /// </summary>
        [StringLength(50)]
        public string SignType { get; set; } = "平日";

        /// <summary>
        /// 連續簽到天數
        /// </summary>
        public int StreakDays { get; set; } = 0;

        /// <summary>
        /// 是否為補簽
        /// </summary>
        public bool IsBackfill { get; set; } = false;

        /// <summary>
        /// 備註
        /// </summary>
        [StringLength(500)]
        public string? Notes { get; set; }

        // 導航屬性
        /// <summary>
        /// 使用者
        /// </summary>
        public virtual User User { get; set; } = null!;
    }
}