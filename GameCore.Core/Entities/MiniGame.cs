using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 小冒險遊戲紀錄表
    /// </summary>
    [Table("MiniGame")]
    public class MiniGame
    {
        /// <summary>
        /// 遊戲執行記錄ID (主鍵，自動遞增)
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PlayID { get; set; }

        /// <summary>
        /// 玩家會員ID (外鍵參考Users)
        /// </summary>
        [Required]
        [ForeignKey("User")]
        public int UserID { get; set; }

        /// <summary>
        /// 出戰寵物ID (外鍵參考Pet)
        /// </summary>
        [Required]
        [ForeignKey("Pet")]
        public int PetID { get; set; }

        /// <summary>
        /// 遊戲關卡等級
        /// </summary>
        public int Level { get; set; } = 1;

        /// <summary>
        /// 需面對的怪物數量
        /// </summary>
        public int MonsterCount { get; set; } = 0;

        /// <summary>
        /// 怪物移動速度倍率
        /// </summary>
        [Column(TypeName = "decimal(5,2)")]
        public decimal SpeedMultiplier { get; set; } = 1.00m;

        /// <summary>
        /// 遊戲結果: Win(贏)/Lose(輸)/Abort(中退)
        /// </summary>
        [Required]
        [StringLength(10)]
        public string Result { get; set; } = "Unknown";

        /// <summary>
        /// 寵物本次獲得經驗值
        /// </summary>
        public int ExpGained { get; set; } = 0;

        /// <summary>
        /// 寵物獲得經驗值時間
        /// </summary>
        public DateTime? ExpGainedTime { get; set; }

        /// <summary>
        /// 本次會員點數增減
        /// </summary>
        public int PointsChanged { get; set; } = 0;

        /// <summary>
        /// 本次會員點數變動時間
        /// </summary>
        public DateTime? PointsChangedTime { get; set; }

        /// <summary>
        /// 寵物飢餓值變化量
        /// </summary>
        public int HungerDelta { get; set; } = 0;

        /// <summary>
        /// 寵物心情值變化量
        /// </summary>
        public int MoodDelta { get; set; } = 0;

        /// <summary>
        /// 寵物體力值變化量
        /// </summary>
        public int StaminaDelta { get; set; } = 0;

        /// <summary>
        /// 寵物清潔值變化量
        /// </summary>
        public int CleanlinessDelta { get; set; } = 0;

        /// <summary>
        /// 遊戲開始時間
        /// </summary>
        [Required]
        public DateTime StartTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 遊戲結束時間 (若中退則為null)
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 是否中途放棄 (0=否,1=是)，預設為0(否)
        /// </summary>
        public bool Aborted { get; set; } = false;

        /// <summary>
        /// 遊戲難度
        /// </summary>
        [StringLength(50)]
        public string Difficulty { get; set; } = "Normal";

        /// <summary>
        /// 遊戲分數
        /// </summary>
        public int Score { get; set; } = 0;

        /// <summary>
        /// 擊敗的怪物數量
        /// </summary>
        public int MonstersDefeated { get; set; } = 0;

        /// <summary>
        /// 遊戲時長 (秒)
        /// </summary>
        public int? DurationSeconds { get; set; }

        // 導航屬性
        /// <summary>
        /// 使用者
        /// </summary>
        public virtual User User { get; set; } = null!;

        /// <summary>
        /// 寵物
        /// </summary>
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