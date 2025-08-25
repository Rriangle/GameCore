using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 通用讚表 (多型)
    /// </summary>
    [Table("reactions")]
    public class Reaction
    {
        /// <summary>
        /// 流水號 (主鍵，自動遞增)
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        /// <summary>
        /// 誰按的 (外鍵到Users)
        /// </summary>
        [Required]
        [ForeignKey("User")]
        public int user_id { get; set; }

        /// <summary>
        /// 目標類型 (post/thread/thread_post)
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
        /// 反應類型 (like/emoji等)
        /// </summary>
        [Required]
        [StringLength(50)]
        public string kind { get; set; } = "like";

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime created_at { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 反應值 (用於不同類型的反應)
        /// </summary>
        [StringLength(100)]
        public string? reaction_value { get; set; }

        // 導航屬性
        /// <summary>
        /// 使用者
        /// </summary>
        public virtual User User { get; set; } = null!;
    }
}