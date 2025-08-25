using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 禁言選項表 (字典)
    /// </summary>
    [Table("Mutes")]
    public class Mute
    {
        /// <summary>
        /// 禁言選項ID (主鍵，自動遞增)
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int mute_id { get; set; }

        /// <summary>
        /// 禁言名稱
        /// </summary>
        [Required]
        [StringLength(100)]
        public string mute_name { get; set; } = string.Empty;

        /// <summary>
        /// 禁言時長 (分鐘)
        /// </summary>
        public int duration_minutes { get; set; } = 0;

        /// <summary>
        /// 禁言原因
        /// </summary>
        [StringLength(500)]
        public string? reason { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime created_at { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool is_active { get; set; } = true;

        /// <summary>
        /// 設置者ID (外鍵到ManagerRole)
        /// </summary>
        [ForeignKey("ManagerData")]
        public int? manager_id { get; set; }

        /// <summary>
        /// 禁言類型 (temporary/permanent/warning)
        /// </summary>
        [StringLength(50)]
        public string mute_type { get; set; } = "temporary";

        /// <summary>
        /// 適用範圍 (user/thread/forum/global)
        /// </summary>
        [StringLength(50)]
        public string scope { get; set; } = "user";

        /// <summary>
        /// 備註
        /// </summary>
        [StringLength(1000)]
        public string? notes { get; set; }

        // 導航屬性
        /// <summary>
        /// 設置者
        /// </summary>
        public virtual ManagerData? ManagerData { get; set; }
    }
}