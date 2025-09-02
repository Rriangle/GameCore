using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Domain.Entities
{
    /// <summary>
    /// ?¨æˆ¶ç°½åˆ°çµ±è?å¯¦é?
    /// </summary>
    [Table("user_sign_in_stats")]
    public class UserSignInStats
    {
        /// <summary>
        /// çµ±è?ID (ä¸»éµ)
        /// </summary>
        [Key]
        [Column("stats_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StatsId { get; set; }

        /// <summary>
        /// ?¨æˆ¶ID (å¤–éµ)
        /// </summary>
        [Required]
        [Column("user_id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// ç°½åˆ°?¥æ?
        /// </summary>
        [Required]
        [Column("sign_in_date")]
        [DataType(DataType.Date)]
        public DateTime SignInDate { get; set; }

        /// <summary>
        /// ç°½åˆ°?‚é?
        /// </summary>
        [Required]
        [Column("sign_in_time")]
        public DateTime SignInTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// ???ç°½åˆ°å¤©æ•¸
        /// </summary>
        [Column("consecutive_days")]
        public int ConsecutiveDays { get; set; } = 1;

        /// <summary>
        /// ç¸½ç°½?°å¤©??
        /// </summary>
        [Column("total_days")]
        public int TotalDays { get; set; } = 1;

        /// <summary>
        /// ?²å??„é???
        /// </summary>
        [Column("points_earned")]
        public int PointsEarned { get; set; } = 0;

        /// <summary>
        /// ç°½åˆ°?å‹µé¡å?
        /// </summary>
        [Column("reward_type")]
        [StringLength(50)]
        public string? RewardType { get; set; }

        /// <summary>
        /// ç°½åˆ°?å‹µ?è¿°
        /// </summary>
        [Column("reward_description")]
        [StringLength(200)]
        public string? RewardDescription { get; set; }

        /// <summary>
        /// å»ºç??‚é?
        /// </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // å°èˆªå±¬æ€?
        public virtual User User { get; set; } = null!;
    }
} 
