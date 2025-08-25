using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 通用收藏表 (多型)
    /// </summary>
    [Table("bookmarks")]
    public class Bookmark
    {
        /// <summary>
        /// 流水號 (主鍵，自動遞增)
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        /// <summary>
        /// 收藏者ID (外鍵到Users)
        /// </summary>
        [Required]
        [ForeignKey("User")]
        public int user_id { get; set; }

        /// <summary>
        /// 目標類型 (post/thread/game/forum)
        /// </summary>
        [Required]
        [StringLength(50)]
        public string target_type { get; set; } = string.Empty;

        /// <summary>
        /// 目標ID (多型，不設FK)
        /// </summary>
        [Required]
        public long target_id { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime created_at { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 收藏分類
        /// </summary>
        [StringLength(100)]
        public string? category { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        [StringLength(500)]
        public string? notes { get; set; }

        // 導航屬性
        /// <summary>
        /// 使用者
        /// </summary>
        public virtual User User { get; set; } = null!;
    }
}