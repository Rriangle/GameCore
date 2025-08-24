using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 小遊戲記錄表 (對應 MiniGameRecords)
    /// </summary>
    [Table("MiniGameRecords")]
    public class MiniGameRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string GameType { get; set; } = string.Empty;

        [Required]
        public int Level { get; set; } = 1;

        [Required]
        public int Score { get; set; } = 0;

        [Required]
        public bool IsWin { get; set; } = false;

        [Required]
        public int Experience { get; set; } = 0;

        [Required]
        public int Points { get; set; } = 0;

        [Required]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;

        // Navigation property
        public virtual User User { get; set; } = null!;
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