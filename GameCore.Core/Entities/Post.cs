using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 論壇貼文實體
    /// </summary>
    [Table("Posts")]
    public class Post
    {
        /// <summary>
        /// 貼文 ID (主鍵)
        /// </summary>
        [Key]
        [Column("post_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostId { get; set; }

        /// <summary>
        /// 論壇 ID (外鍵到 Forum)
        /// </summary>
        [Column("forum_id")]
        [ForeignKey("Forum")]
        public int ForumId { get; set; }

        /// <summary>
        /// 作者 ID (外鍵到 User)
        /// </summary>
        [Column("author_id")]
        [ForeignKey("Author")]
        public int AuthorId { get; set; }

        /// <summary>
        /// 貼文標題
        /// </summary>
        [Column("title")]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 貼文內容
        /// </summary>
        [Column("content")]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// 貼文類型 (一般、置頂、精華等)
        /// </summary>
        [Column("post_type")]
        [StringLength(50)]
        public string PostType { get; set; } = "Normal";

        /// <summary>
        /// 瀏覽次數
        /// </summary>
        [Column("view_count")]
        public int ViewCount { get; set; } = 0;

        /// <summary>
        /// 回覆次數
        /// </summary>
        [Column("reply_count")]
        public int ReplyCount { get; set; } = 0;

        /// <summary>
        /// 點讚次數
        /// </summary>
        [Column("like_count")]
        public int LikeCount { get; set; } = 0;

        /// <summary>
        /// 是否置頂
        /// </summary>
        [Column("is_pinned")]
        public bool IsPinned { get; set; } = false;

        /// <summary>
        /// 是否精華
        /// </summary>
        [Column("is_featured")]
        public bool IsFeatured { get; set; } = false;

        /// <summary>
        /// 是否鎖定
        /// </summary>
        [Column("is_locked")]
        public bool IsLocked { get; set; } = false;

        /// <summary>
        /// 創建時間
        /// </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 更新時間
        /// </summary>
        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// 最後回覆時間
        /// </summary>
        [Column("last_reply_at")]
        public DateTime? LastReplyAt { get; set; }

        // 導航屬性
        public virtual Forum Forum { get; set; } = null!;
        public virtual User Author { get; set; } = null!;
        public virtual ICollection<Reply> Replies { get; set; } = new List<Reply>();
        public virtual ICollection<PostMetricSnapshot> MetricSnapshots { get; set; } = new List<PostMetricSnapshot>();
    }

    /// <summary>
    /// 貼文指標快照
    /// </summary>
    [Table("PostMetricSnapshots")]
    public class PostMetricSnapshot
    {
        /// <summary>
        /// 快照 ID (主鍵)
        /// </summary>
        [Key]
        [Column("snapshot_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SnapshotId { get; set; }

        /// <summary>
        /// 貼文 ID (外鍵到 Post)
        /// </summary>
        [Column("post_id")]
        [ForeignKey("Post")]
        public int PostId { get; set; }

        /// <summary>
        /// 瀏覽次數
        /// </summary>
        [Column("view_count")]
        public int ViewCount { get; set; }

        /// <summary>
        /// 回覆次數
        /// </summary>
        [Column("reply_count")]
        public int ReplyCount { get; set; }

        /// <summary>
        /// 點讚次數
        /// </summary>
        [Column("like_count")]
        public int LikeCount { get; set; }

        /// <summary>
        /// 快照時間
        /// </summary>
        [Column("snapshot_time")]
        public DateTime SnapshotTime { get; set; } = DateTime.UtcNow;

        // 導航屬性
        public virtual Post Post { get; set; } = null!;
    }
} 