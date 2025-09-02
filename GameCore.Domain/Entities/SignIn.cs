using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Domain.Entities
{
    /// <summary>
    /// 簽到記�?實�?
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

        // 導航屬�?
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }

    /// <summary>
    /// 簽到統�?實�?
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

        // 導航屬�?
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }
}
