using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 通用收藏實體 - 支援多種類型的收藏功能
    /// </summary>
    [Table("Bookmarks")]
    public class Bookmark
    {
        /// <summary>
        /// 收藏記錄編號 (主鍵)
        /// </summary>
        [Key]
        [Column("bookmark_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookmarkId { get; set; }

        /// <summary>
        /// 使用者編號 (外鍵到 User)
        /// </summary>
        [Required]
        [Column("user_id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// 目標類型 (Post, Product, Game 等)
        /// </summary>
        [Required]
        [Column("target_type")]
        [StringLength(50)]
        public string TargetType { get; set; } = string.Empty;

        /// <summary>
        /// 目標編號 (對應不同類型的主鍵)
        /// </summary>
        [Required]
        [Column("target_id")]
        public int TargetId { get; set; }

        /// <summary>
        /// 收藏時間
        /// </summary>
        [Required]
        [Column("bookmark_time")]
        public DateTime BookmarkTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 收藏標題 (快取用，避免頻繁查詢)
        /// </summary>
        [Column("target_title")]
        [StringLength(200)]
        public string? TargetTitle { get; set; }

        /// <summary>
        /// 收藏備註
        /// </summary>
        [Column("note")]
        [StringLength(500)]
        public string? Note { get; set; }

        /// <summary>
        /// 是否為私人收藏
        /// </summary>
        [Required]
        [Column("is_private")]
        public bool IsPrivate { get; set; } = false;

        /// <summary>
        /// 收藏分類標籤
        /// </summary>
        [Column("tags")]
        [StringLength(200)]
        public string? Tags { get; set; }

        // 導航屬性
        /// <summary>
        /// 收藏者
        /// </summary>
        public virtual User User { get; set; } = null!;
    }
}