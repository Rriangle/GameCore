using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Domain.Entities
{
    /// <summary>
    /// ?�戶簽到統�?實�?
    /// </summary>
    [Table("user_sign_in_stats")]
    public class UserSignInStats
    {
        /// <summary>
        /// 統�?ID (主鍵)
        /// </summary>
        [Key]
        [Column("stats_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StatsId { get; set; }

        /// <summary>
        /// ?�戶ID (外鍵)
        /// </summary>
        [Required]
        [Column("user_id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// 簽到?��?
        /// </summary>
        [Required]
        [Column("sign_in_date")]
        [DataType(DataType.Date)]
        public DateTime SignInDate { get; set; }

        /// <summary>
        /// 簽到?��?
        /// </summary>
        [Required]
        [Column("sign_in_time")]
        public DateTime SignInTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// ???簽到天數
        /// </summary>
        [Column("consecutive_days")]
        public int ConsecutiveDays { get; set; } = 1;

        /// <summary>
        /// 總簽?�天??
        /// </summary>
        [Column("total_days")]
        public int TotalDays { get; set; } = 1;

        /// <summary>
        /// ?��??��???
        /// </summary>
        [Column("points_earned")]
        public int PointsEarned { get; set; } = 0;

        /// <summary>
        /// 簽到?�勵類�?
        /// </summary>
        [Column("reward_type")]
        [StringLength(50)]
        public string? RewardType { get; set; }

        /// <summary>
        /// 簽到?�勵?�述
        /// </summary>
        [Column("reward_description")]
        [StringLength(200)]
        public string? RewardDescription { get; set; }

        /// <summary>
        /// 建�??��?
        /// </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // 導航屬�?
        public virtual User User { get; set; } = null!;
    }
} 
