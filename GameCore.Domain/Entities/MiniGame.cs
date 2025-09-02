using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Domain.Entities
{
    /// <summary>
    /// 小�??��??��??�實�?
    /// </summary>
    [Table("MiniGameRecords")]
    public partial class MiniGameRecord
    {
        [Key]
        public int GameRecordId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int PetId { get; set; }

        [Required]
        public int PlayID { get; set; }

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

        // 導航屬�?
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        [ForeignKey("PetId")]
        public virtual Pet Pet { get; set; } = null!;
    }

    /// <summary>
    /// 小�??��??�設定實�?
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
