using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 小冒險遊戲紀錄實體 (對應資料庫 MiniGame 表)
    /// </summary>
    [Table("MiniGame")]
    public class MiniGame
    {
        /// <summary>
        /// 遊戲執行記錄 ID，自動遞增
        /// </summary>
        [Key]
        [Column("PlayID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PlayId { get; set; }

        /// <summary>
        /// 玩家會員 ID，外鍵參考 Users.UserID
        /// </summary>
        [Required]
        [Column("UserID")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// 出戰寵物 ID，外鍵參考 Pet.PetID
        /// </summary>
        [Required]
        [Column("PetID")]
        [ForeignKey("Pet")]
        public int PetId { get; set; }

        /// <summary>
        /// 遊戲關卡等級
        /// </summary>
        [Required]
        [Column("Level")]
        public int Level { get; set; } = 0;

        /// <summary>
        /// 需面對的怪物數量
        /// </summary>
        [Required]
        [Column("MonsterCount")]
        public int MonsterCount { get; set; } = 0;

        /// <summary>
        /// 怪物移動速度倍率
        /// </summary>
        [Required]
        [Column("SpeedMultiplier", TypeName = "decimal(5,2)")]
        public decimal SpeedMultiplier { get; set; } = 1.00m;

        /// <summary>
        /// 遊戲結果: Win(贏)/Lose(輸)/Abort(中退)
        /// </summary>
        [Required]
        [Column("Result")]
        [StringLength(10)]
        public string Result { get; set; } = "Unknown";

        /// <summary>
        /// 寵物本次獲得經驗值
        /// </summary>
        [Required]
        [Column("ExpGained")]
        public int ExpGained { get; set; } = 0;

        /// <summary>
        /// 寵物獲得經驗值時間
        /// </summary>
        [Column("ExpGainedTime")]
        public DateTime? ExpGainedTime { get; set; }

        /// <summary>
        /// 本次會員點數增減
        /// </summary>
        [Required]
        [Column("PointsChanged")]
        public int PointsChanged { get; set; } = 0;

        /// <summary>
        /// 本次會員點數變動時間
        /// </summary>
        [Column("PointsChangedTime")]
        public DateTime? PointsChangedTime { get; set; }

        /// <summary>
        /// 寵物飢餓值變化量
        /// </summary>
        [Required]
        [Column("HungerDelta")]
        public int HungerDelta { get; set; } = 0;

        /// <summary>
        /// 寵物心情值變化量
        /// </summary>
        [Required]
        [Column("MoodDelta")]
        public int MoodDelta { get; set; } = 0;

        /// <summary>
        /// 寵物體力值變化量
        /// </summary>
        [Required]
        [Column("StaminaDelta")]
        public int StaminaDelta { get; set; } = 0;

        /// <summary>
        /// 寵物清潔值變化量
        /// </summary>
        [Required]
        [Column("CleanlinessDelta")]
        public int CleanlinessDelta { get; set; } = 0;

        /// <summary>
        /// 遊戲開始時間
        /// </summary>
        [Required]
        [Column("StartTime")]
        public DateTime StartTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 遊戲結束時間，若中退則為null
        /// </summary>
        [Column("EndTime")]
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 是否中途放棄 (0=否,1=是)，預設為0(否)
        /// </summary>
        [Required]
        [Column("Aborted")]
        public bool Aborted { get; set; } = false;

        // 導航屬性
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        [ForeignKey("PetId")]
        public virtual Pet Pet { get; set; } = null!;
    }
    /// <summary>
    /// 小冒險遊戲記錄實體
    /// </summary>
    [Table("MiniGameRecords")]
    public class MiniGameRecord
    {
        [Key]
        public int GameRecordId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int PetId { get; set; }

        [Required]
        public int GameLevel { get; set; }

        [Required]
        public bool IsSuccess { get; set; }

        [Required]
        public int PointsEarned { get; set; }

        [Required]
        public int ExperienceEarned { get; set; }

        [Required]
        public int HealthChange { get; set; }

        [Required]
        public int HungerChange { get; set; }

        [Required]
        public int CleanlinessChange { get; set; }

        [Required]
        public int HappinessChange { get; set; }

        [Required]
        public int EnergyChange { get; set; }

        [Required]
        public DateTime GameDate { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        // 導航屬性
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        [ForeignKey("PetId")]
        public virtual Pet Pet { get; set; } = null!;
    }

    /// <summary>
    /// 小冒險遊戲設定實體
    /// </summary>
    [Table("MiniGameSettings")]
    public class MiniGameSettings
    {
        [Key]
        public int SettingId { get; set; }

        [Required]
        public int GameLevel { get; set; }

        [Required]
        public string LevelName { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int Difficulty { get; set; }

        [Required]
        public int MinHealthRequired { get; set; }

        [Required]
        public int MinHungerRequired { get; set; }

        [Required]
        public int MinCleanlinessRequired { get; set; }

        [Required]
        public int MinHappinessRequired { get; set; }

        [Required]
        public int MinEnergyRequired { get; set; }

        [Required]
        public int SuccessRate { get; set; }

        [Required]
        public int BasePointsReward { get; set; }

        [Required]
        public int BaseExperienceReward { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }
    }
}