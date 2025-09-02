using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Domain.Entities
{
    /// <summary>
    /// ?�戲設�?實�?
    /// </summary>
    [Table("game_settings")]
    public partial class GameSettings
    {
        /// <summary>
        /// 設�?ID（主?��?
        /// </summary>
        [Key]
        [Column("setting_id")]
        public int SettingId { get; set; }

        /// <summary>
        /// 設�??�稱
        /// </summary>
        [Required]
        [Column("setting_name")]
        [StringLength(100)]
        public string SettingName { get; set; } = string.Empty;

        /// <summary>
        /// 設�???
        /// </summary>
        [Column("setting_value")]
        public string SettingValue { get; set; } = string.Empty;

        /// <summary>
        /// 設�??�述
        /// </summary>
        [Column("description")]
        [StringLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// ?�否?�用
        /// </summary>
        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// 建�??��?
        /// </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// ?�新?��?
        /// </summary>
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
} 
