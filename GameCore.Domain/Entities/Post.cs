using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Domain.Entities
{
    /// <summary>
    /// 論�?貼�?實�?
    /// </summary>
    [Table("Posts")]
    public partial class Post
    {
        /// <summary>
        /// 貼�? ID (主鍵)
        /// </summary>
        [Key]
        [Column("post_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostId { get; set; }

        /// <summary>
        /// 論�? ID (外鍵??Forum)
        /// </summary>
        [Column("forum_id")]
        [ForeignKey("Forum")]
        public int ForumId { get; set; }

        /// <summary>
        /// 作�?ID (外鍵??User)
        /// </summary>
        [Column("author_id")]
        [ForeignKey("Author")]
        public int AuthorId { get; set; }

        /// <summary>
        /// 貼�?標�?
        /// </summary>
        [Column("title")]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 貼�??�容
        /// </summary>
        [Column("content")]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// 貼�?類�? (一?�、置?�、精?��?)
        /// </summary>
        [Column("post_type")]
        [StringLength(50)]
        public string PostType { get; set; } = "Normal";

        /// <summary>
        /// ?�覽次數
        /// </summary>
        [Column("view_count")]
        public int ViewCount { get; set; } = 0;

        /// <summary>
        /// ?��?次數
        /// </summary>
        [Column("reply_count")]
        public int ReplyCount { get; set; } = 0;

        /// <summary>
        /// 點�?次數
        /// </summary>
        [Column("like_count")]
        public int LikeCount { get; set; } = 0;

        /// <summary>
        /// ?�否置�?
        /// </summary>
        [Column("is_pinned")]
        public bool IsPinned { get; set; } = false;

        /// <summary>
        /// ?�否精華
        /// </summary>
        [Column("is_featured")]
        public bool IsFeatured { get; set; } = false;

        /// <summary>
        /// ?�否?��?
        /// </summary>
        [Column("is_locked")]
        public bool IsLocked { get; set; } = false;

        /// <summary>
        /// ?�建?��?
        /// </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// ?�新?��?
        /// </summary>
        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// ?�後�?覆�???
        /// </summary>
        [Column("last_reply_at")]
        public DateTime? LastReplyAt { get; set; }

        // 導航屬�?
        public virtual Forum Forum { get; set; } = null!;
        public virtual User Author { get; set; } = null!;
        public virtual ICollection<ThreadPost> Replies { get; set; } = new List<ThreadPost>();
        public virtual ICollection<PostMetricSnapshot> MetricSnapshots { get; set; } = new List<PostMetricSnapshot>();
    }

    /// <summary>
    /// 貼�??��?快照
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
        /// 貼�? ID (外鍵??Post)
        /// </summary>
        [Column("post_id")]
        [ForeignKey("Post")]
        public int PostId { get; set; }

        /// <summary>
        /// ?�覽次數
        /// </summary>
        [Column("view_count")]
        public int ViewCount { get; set; }

        /// <summary>
        /// ?��?次數
        /// </summary>
        [Column("reply_count")]
        public int ReplyCount { get; set; }

        /// <summary>
        /// 點�?次數
        /// </summary>
        [Column("like_count")]
        public int LikeCount { get; set; }

        /// <summary>
        /// 快照?��?
        /// </summary>
        [Column("snapshot_time")]
        public DateTime SnapshotTime { get; set; } = DateTime.UtcNow;

        // 導航屬�?
        public virtual Post Post { get; set; } = null!;
    }
} 
