using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Domain.Entities
{
    /// <summary>
    /// ?�用?��?實�?
    /// ?�援?��?主�??�貼?�、�??�、�?壇�?
    /// </summary>
    [Table("bookmarks")]
    public class Bookmark
    {
        /// <summary>
        /// ?��?ID（主?��?
        /// </summary>
        [Key]
        [Column("id")]
        public long Id { get; set; }

        /// <summary>
        /// ?��??�用?�ID（�??��?
        /// </summary>
        [Column("user_id")]
        public int UserId { get; set; }

        /// <summary>
        /// ?��?類�?（post/thread/game/forum�?
        /// </summary>
        [Required]
        [Column("target_type")]
        [StringLength(20)]
        public string TargetType { get; set; } = string.Empty;

        /// <summary>
        /// ?��?ID（�??��?不設FK�?
        /// </summary>
        [Column("target_id")]
        public long TargetId { get; set; }

        /// <summary>
        /// 建�??��?
        /// </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // 導航屬�?
        /// <summary>
        /// ?��???
        /// </summary>
        public virtual User? User { get; set; }
    }
} 
