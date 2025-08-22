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

    /// <summary>
    /// 寵物狀態表
    /// </summary>
    [Table("Pet")]
    public class Pet
    {
        /// <summary>
        /// 寵物編號 (主鍵, 自動遞增)
        /// </summary>
        [Key]
        [Column("PetID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PetId { get; set; }

        /// <summary>
        /// 寵物主人會員編號 (外鍵參考 Users.UserID)
        /// </summary>
        [Required]
        [Column("UserID")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// 寵物名稱 (若未提供則預設為 '小可愛')
        /// </summary>
        [Required]
        [Column("PetName")]
        [StringLength(50)]
        public string PetName { get; set; } = "小可愛";

        /// <summary>
        /// 寵物當前等級
        /// </summary>
        [Required]
        [Column("Level")]
        public int Level { get; set; } = 1;

        /// <summary>
        /// 寵物最後一次升級時間
        /// </summary>
        [Required]
        [Column("LevelUpTime")]
        public DateTime LevelUpTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 寵物累計總經驗值
        /// </summary>
        [Required]
        [Column("Experience")]
        public int Experience { get; set; } = 0;

        /// <summary>
        /// 飢餓值 (0-100)
        /// </summary>
        [Required]
        [Column("Hunger")]
        public int Hunger { get; set; } = 100;

        /// <summary>
        /// 心情值 (0-100)
        /// </summary>
        [Required]
        [Column("Mood")]
        public int Mood { get; set; } = 100;

        /// <summary>
        /// 體力值 (0-100)
        /// </summary>
        [Required]
        [Column("Stamina")]
        public int Stamina { get; set; } = 100;

        /// <summary>
        /// 清潔值 (0-100)
        /// </summary>
        [Required]
        [Column("Cleanliness")]
        public int Cleanliness { get; set; } = 100;

        /// <summary>
        /// 健康度 (0-100)
        /// </summary>
        [Required]
        [Column("Health")]
        public int Health { get; set; } = 100;

        /// <summary>
        /// 膚色十六進位
        /// </summary>
        [Required]
        [Column("SkinColor")]
        [StringLength(50)]
        public string SkinColor { get; set; } = "#ADD8E6";

        /// <summary>
        /// 最後一次膚色更換時間
        /// </summary>
        [Required]
        [Column("ColorChangedTime")]
        public DateTime ColorChangedTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 背景色
        /// </summary>
        [Required]
        [Column("BackgroundColor")]
        [StringLength(50)]
        public string BackgroundColor { get; set; } = "粉藍";

        /// <summary>
        /// 最後一次背景色更換時間
        /// </summary>
        [Required]
        [Column("BackgroundColorChangedTime")]
        public DateTime BackgroundColorChangedTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 最近一次幫寵物換色所花費之會員點數
        /// </summary>
        [Required]
        [Column("PointsChanged")]
        public int PointsChanged { get; set; } = 0;

        /// <summary>
        /// 幫寵物換色所花費之會員點數變動時間
        /// </summary>
        [Required]
        [Column("PointsChangedTime")]
        public DateTime PointsChangedTime { get; set; } = DateTime.UtcNow;

        // 導航屬性
        public virtual User User { get; set; } = null!;
        public virtual ICollection<MiniGame> MiniGames { get; set; } = new List<MiniGame>();
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
        [Column("SpeedMultiplier")]
        [Column(TypeName = "decimal(5,2)")]
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