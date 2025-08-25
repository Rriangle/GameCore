using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 版內主題表 (討論串)
    /// </summary>
    [Table("threads")]
    public class Thread
    {
        /// <summary>
        /// 主題ID (主鍵，自動遞增)
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long thread_id { get; set; }

        /// <summary>
        /// 所屬版ID (外鍵到forums)
        /// </summary>
        [Required]
        [ForeignKey("Forum")]
        public int forum_id { get; set; }

        /// <summary>
        /// 作者ID (外鍵到Users)
        /// </summary>
        [Required]
        [ForeignKey("AuthorUser")]
        public int author_user_id { get; set; }

        /// <summary>
        /// 標題
        /// </summary>
        [Required]
        [StringLength(500)]
        public string title { get; set; } = string.Empty;

        /// <summary>
        /// 狀態 (normal/hidden/archived)
        /// </summary>
        [Required]
        [StringLength(50)]
        public string status { get; set; } = "normal";

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime created_at { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime updated_at { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 內容 (Markdown格式)
        /// </summary>
        [StringLength(10000)]
        public string? content_md { get; set; }

        /// <summary>
        /// 瀏覽次數
        /// </summary>
        public int view_count { get; set; } = 0;

        /// <summary>
        /// 回覆次數
        /// </summary>
        public int reply_count { get; set; } = 0;

        /// <summary>
        /// 是否置頂
        /// </summary>
        public bool is_pinned { get; set; } = false;

        /// <summary>
        /// 是否精華
        /// </summary>
        public bool is_essence { get; set; } = false;

        /// <summary>
        /// 最後回覆時間
        /// </summary>
        public DateTime? last_reply_at { get; set; }

        /// <summary>
        /// 最後回覆者ID
        /// </summary>
        public int? last_replier_id { get; set; }

        /// <summary>
        /// 標籤
        /// </summary>
        [StringLength(500)]
        public string? tags { get; set; }

        // 導航屬性
        /// <summary>
        /// 所屬論壇
        /// </summary>
        public virtual Forum Forum { get; set; } = null!;

        /// <summary>
        /// 作者
        /// </summary>
        public virtual User AuthorUser { get; set; } = null!;

        /// <summary>
        /// 最後回覆者
        /// </summary>
        [ForeignKey("last_replier_id")]
        public virtual User? LastReplier { get; set; }

        /// <summary>
        /// 回覆列表
        /// </summary>
        public virtual ICollection<ThreadPost> ThreadPosts { get; set; } = new List<ThreadPost>();

        /// <summary>
        /// 反應記錄
        /// </summary>
        public virtual ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();

        /// <summary>
        /// 收藏記錄
        /// </summary>
        public virtual ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
    }
}