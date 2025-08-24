using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 簽到記錄表 (對應 SignInRecords)
    /// </summary>
    [Table("SignInRecords")]
    public class SignInRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime SignInDate { get; set; }

        [Required]
        public int Points { get; set; } = 0;

        [Required]
        public int Experience { get; set; } = 0;

        [Required]
        public bool IsWeekend { get; set; } = false;

        [Required]
        public bool IsPerfect { get; set; } = false;

        [Required]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;

        // Navigation property
        public virtual User User { get; set; } = null!;
    }

    /// <summary>
    /// 簽到統計表 (對應 UserSignInStats)
    /// </summary>
    [Table("UserSignInStats")]
    public class UserSignInStats
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogId { get; set; }

        [Required]
        public DateTime SignTime { get; set; } = DateTime.UtcNow;

        [Required]
        public int UserId { get; set; }

        [Required]
        public int PointsChanged { get; set; } = 0;

        [Required]
        public int ExpGained { get; set; } = 0;

        [Required]
        public DateTime PointsChangedTime { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime ExpGainedTime { get; set; } = DateTime.UtcNow;

        // Navigation property
        public virtual User User { get; set; } = null!;
    }
}