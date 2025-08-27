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
        /// 簽到記錄編號 (主鍵, 自動遞增)
        /// </summary>
        [Key]
        [Column("LogID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogId { get; set; }

        /// <summary>
        /// 簽到時間 (預設 UTC 當下時間)
        /// </summary>
        [Required]
        [Column("SignTime")]
        public DateTime SignTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 會員編號 (外鍵參考 Users.UserID)
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
        public virtual User User { get; set; } = null!;
    }

    public class Pet
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Pattern { get; set; } = string.Empty;
        public int Level { get; set; } = 1;
        public int Experience { get; set; } = 0;
        public int Health { get; set; } = 100;
        public int MaxHealth { get; set; } = 100;
        public int Energy { get; set; } = 100;
        public int MaxEnergy { get; set; } = 100;
        public int Happiness { get; set; } = 50;
        public string Status { get; set; } = "Healthy";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastInteractionAt { get; set; }

        // Navigation properties
        public User User { get; set; } = null!;
    }

    public enum PetInteractionType
    {
        Feed,
        Play,
        Groom,
        Train,
        Sleep,
        Exercise
    }

    /// <summary>
    /// 小冒險遊戲紀錄表
    /// </summary>
    [Table("MiniGame")]
    public class MiniGame
    {
        /// <summary>
        /// 遊戲執行記錄編號 (主鍵, 自動遞增)
        /// </summary>
        [Key]
        [Column("PlayID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PlayId { get; set; }

        /// <summary>
        /// 玩家會員編號 (外鍵參考 Users.UserID)
        /// </summary>
        [Required]
        [Column("UserID")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// 出戰寵物編號 (外鍵參考 Pet.PetID)
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
        public int Level { get; set; } = 1;

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
        /// 遊戲結束時間 (若中退則為null)
        /// </summary>
        [Column("EndTime")]
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 是否中途放棄 (0=否,1=是), 預設為0(否)
        /// </summary>
        [Required]
        [Column("Aborted")]
        public bool Aborted { get; set; } = false;

        // 導航屬性
        public virtual User User { get; set; } = null!;
        public virtual Pet Pet { get; set; } = null!;
    }
}