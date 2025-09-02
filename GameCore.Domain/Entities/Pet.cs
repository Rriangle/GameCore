using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Domain.Entities
{
    /// <summary>
    /// 寵物實�?
    /// </summary>
    [Table("pets")]
    public partial class Pet
    {
        /// <summary>
        /// 寵物ID (主鍵)
        /// </summary>
        [Key]
        [Column("pet_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// ?�戶ID (外鍵)
        /// </summary>
        [Required]
        [Column("user_id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// 寵物?�稱
        /// </summary>
        [Required]
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 寵物類�?
        /// </summary>
        [Required]
        [Column("type")]
        [StringLength(50)]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// 寵物顏色
        /// </summary>
        [Column("color")]
        [StringLength(50)]
        public string Color { get; set; } = string.Empty;

        /// <summary>
        /// 寵物?��?
        /// </summary>
        [Column("pattern")]
        [StringLength(50)]
        public string Pattern { get; set; } = string.Empty;

        /// <summary>
        /// 寵物等�?
        /// </summary>
        [Column("level")]
        public int Level { get; set; } = 1;

        /// <summary>
        /// 寵物經�???
        /// </summary>
        [Column("experience")]
        public int Experience { get; set; } = 0;

        /// <summary>
        /// 寵物?�康�?
        /// </summary>
        [Column("health")]
        public int Health { get; set; } = 100;

        /// <summary>
        /// 寵物?�大健康度
        /// </summary>
        [Column("max_health")]
        public int MaxHealth { get; set; } = 100;

        /// <summary>
        /// 寵物精�?
        /// </summary>
        [Column("energy")]
        public int Energy { get; set; } = 100;

        /// <summary>
        /// 寵物?�大精??
        /// </summary>
        [Column("max_energy")]
        public int MaxEnergy { get; set; } = 100;

        /// <summary>
        /// 寵物快�?�?
        /// </summary>
        [Column("happiness")]
        public int Happiness { get; set; } = 50;

        /// <summary>
        /// 寵物飢�?�?
        /// </summary>
        [Column("hunger")]
        public int Hunger { get; set; } = 50;

        /// <summary>
        /// 寵物心�?
        /// </summary>
        [Column("mood")]
        public int Mood { get; set; } = 50;

        /// <summary>
        /// 寵物體�?
        /// </summary>
        [Column("stamina")]
        public int Stamina { get; set; } = 50;

        /// <summary>
        /// 寵物清�?�?
        /// </summary>
        [Column("cleanliness")]
        public int Cleanliness { get; set; } = 50;

        /// <summary>
        /// 寵物?�??
        /// </summary>
        [Column("status")]
        [StringLength(50)]
        public string Status { get; set; } = "Healthy";

        /// <summary>
        /// 寵物?�色
        /// </summary>
        [Column("skin_color")]
        [StringLength(50)]
        public string SkinColor { get; set; } = string.Empty;

        /// <summary>
        /// 寵物?�景??
        /// </summary>
        [Column("background_color")]
        [StringLength(50)]
        public string BackgroundColor { get; set; } = string.Empty;

        /// <summary>
        /// 建�??��?
        /// </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// ?�後�??��???
        /// </summary>
        [Column("last_interaction_at")]
        public DateTime? LastInteractionAt { get; set; }

        /// <summary>
        /// ?��??��?
        /// </summary>
        [Column("level_up_time")]
        public DateTime LevelUpTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// ?�色?��?
        /// </summary>
        [Column("color_changed_time")]
        public DateTime ColorChangedTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// ?�景?��??��???
        /// </summary>
        [Column("background_color_changed_time")]
        public DateTime BackgroundColorChangedTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 點數變更
        /// </summary>
        [Column("points_changed")]
        public int PointsChanged { get; set; } = 0;

        /// <summary>
        /// 點數變更?��?
        /// </summary>
        [Column("points_changed_time")]
        public DateTime PointsChangedTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 寵物ID (?�於?��?層兼容�?
        /// </summary>
        [NotMapped]
        public int PetId => Id;

        /// <summary>
        /// 寵物?�稱 (?�於?��?層兼容�?
        /// </summary>
        [NotMapped]
        public string PetName => Name;

        // 導航屬�?
        public virtual User User { get; set; } = null!;
        public virtual ICollection<MiniGameRecord> MiniGameRecords { get; set; } = new List<MiniGameRecord>();
    }

    /// <summary>
    /// 寵物互�?類�?
    /// </summary>
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
    /// 寵物互�?結�?
    /// </summary>
    public class PetInteractionResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int ExperienceGained { get; set; }
        public int PointsGained { get; set; }
        public bool LevelUp { get; set; }
        public Pet Pet { get; set; } = null!;
    }

    /// <summary>
    /// 寵物?�色結�?
    /// </summary>
    public class PetColorChangeResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int PointsCost { get; set; }
        public Pet Pet { get; set; } = null!;
    }

    /// <summary>
    /// 小�??��??��??�表
    /// </summary>
    [Table("MiniGame")]
    public class MiniGame
    {
        /// <summary>
        /// ?�戲?��?記�?編�? (主鍵, ?��??��?)
        /// </summary>
        [Key]
        [Column("PlayID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PlayId { get; set; }

        /// <summary>
        /// ?�家?�員編�? (外鍵?��?Users.UserID)
        /// </summary>
        [Required]
        [Column("UserID")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// ?�戰寵物編�? (外鍵?��?Pet.PetID)
        /// </summary>
        [Required]
        [Column("PetID")]
        [ForeignKey("Pet")]
        public int PetId { get; set; }

        /// <summary>
        /// ?�戲?�卡等�?
        /// </summary>
        [Required]
        [Column("Level")]
        public int Level { get; set; } = 1;

        /// <summary>
        /// ?�?��??�怪物?��?
        /// </summary>
        [Required]
        [Column("MonsterCount")]
        public int MonsterCount { get; set; } = 0;

        /// <summary>
        /// ?�物移�??�度?��?
        /// </summary>
        [Required]
        [Column("SpeedMultiplier", TypeName = "decimal(5,2)")]
        public decimal SpeedMultiplier { get; set; } = 1.00m;

        /// <summary>
        /// ?�戲結�?: Win(�?/Lose(�?/Abort(中退)
        /// </summary>
        [Required]
        [Column("Result")]
        [StringLength(10)]
        public string Result { get; set; } = "Unknown";

        /// <summary>
        /// 寵物?�次?��?經�???
        /// </summary>
        [Required]
        [Column("ExpGained")]
        public int ExpGained { get; set; } = 0;

        /// <summary>
        /// 寵物?��?經�??��???
        /// </summary>
        [Column("ExpGainedTime")]
        public DateTime? ExpGainedTime { get; set; }

        /// <summary>
        /// ?�次?�員點數增�?
        /// </summary>
        [Required]
        [Column("PointsChanged")]
        public int PointsChanged { get; set; } = 0;

        /// <summary>
        /// ?�次?�員點數變�??��?
        /// </summary>
        [Column("PointsChangedTime")]
        public DateTime? PointsChangedTime { get; set; }

        /// <summary>
        /// 寵物飢�??��??��?
        /// </summary>
        [Required]
        [Column("HungerDelta")]
        public int HungerDelta { get; set; } = 0;

        /// <summary>
        /// 寵物心�??��??��?
        /// </summary>
        [Required]
        [Column("MoodDelta")]
        public int MoodDelta { get; set; } = 0;

        /// <summary>
        /// 寵物體�??��??��?
        /// </summary>
        [Required]
        [Column("StaminaDelta")]
        public int StaminaDelta { get; set; } = 0;

        /// <summary>
        /// 寵物清�??��??��?
        /// </summary>
        [Required]
        [Column("CleanlinessDelta")]
        public int CleanlinessDelta { get; set; } = 0;

        /// <summary>
        /// ?�戲?��??��?
        /// </summary>
        [Required]
        [Column("StartTime")]
        public DateTime StartTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// ?�戲結�??��? (?�中?�?�為null)
        /// </summary>
        [Column("EndTime")]
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// ?�否中途放�?(0=??1=??, ?�設??(??
        /// </summary>
        [Required]
        [Column("Aborted")]
        public bool Aborted { get; set; } = false;

        // 導航屬�?
        public virtual User User { get; set; } = null!;
        public virtual Pet Pet { get; set; } = null!;
    }
}
