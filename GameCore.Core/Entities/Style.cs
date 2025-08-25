using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 樣式表
    /// </summary>
    [Table("Styles")]
    public class Style
    {
        /// <summary>
        /// 樣式ID (主鍵，自動遞增)
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int style_id { get; set; }

        /// <summary>
        /// 樣式名稱
        /// </summary>
        [Required]
        [StringLength(100)]
        public string style_name { get; set; } = string.Empty;

        /// <summary>
        /// 效果說明
        /// </summary>
        [StringLength(500)]
        public string? effect_desc { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime created_at { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 設置者ID (外鍵到ManagerRole)
        /// </summary>
        [ForeignKey("ManagerData")]
        public int? manager_id { get; set; }

        /// <summary>
        /// 樣式類型 (avatar/badge/border/background)
        /// </summary>
        [StringLength(50)]
        public string style_type { get; set; } = "avatar";

        /// <summary>
        /// 樣式值 (CSS/JSON)
        /// </summary>
        [StringLength(2000)]
        public string? style_value { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool is_active { get; set; } = true;

        /// <summary>
        /// 排序順序
        /// </summary>
        public int sort_order { get; set; } = 0;

        /// <summary>
        /// 適用角色 (user/moderator/admin/all)
        /// </summary>
        [StringLength(50)]
        public string applicable_role { get; set; } = "user";

        /// <summary>
        /// 獲得條件
        /// </summary>
        [StringLength(500)]
        public string? requirement { get; set; }

        /// <summary>
        /// 是否為稀有樣式
        /// </summary>
        public bool is_rare { get; set; } = false;

        /// <summary>
        /// 是否為限時樣式
        /// </summary>
        public bool is_limited { get; set; } = false;

        /// <summary>
        /// 過期時間
        /// </summary>
        public DateTime? expires_at { get; set; }

        // 導航屬性
        /// <summary>
        /// 設置者
        /// </summary>
        public virtual ManagerData? ManagerData { get; set; }
    }
}