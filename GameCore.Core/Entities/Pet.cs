using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    // UserSignInStats class moved to SignIn.cs to avoid duplication

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

    // MiniGame class moved to MiniGame.cs to avoid duplication
}