using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 論壇版面實體
    /// </summary>
    [Table("Forums")]
    public class Forum
    {
        [Key]
        public int ForumId { get; set; }

        [Required]
        public int GameId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int PostCount { get; set; }

        [Required]
        public int ReplyCount { get; set; }

        [Required]
        public int ViewCount { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public int SortOrder { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        // 導航屬性
        [ForeignKey("GameId")]
        public virtual Game Game { get; set; } = null!;

        public virtual ICollection<ForumPost> Posts { get; set; } = new List<ForumPost>();
        public virtual ICollection<Thread> Threads { get; set; } = new List<Thread>();
        public virtual ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
    }

    /// <summary>
    /// 討論主題實體
    /// </summary>
    [Table("Posts")]
    public class ForumPost
    {
        [Key]
        public int PostId { get; set; }

        [Required]
        public int ForumId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        public PostStatus Status { get; set; }

        [Required]
        public int ViewCount { get; set; }

        [Required]
        public int LikeCount { get; set; }

        [Required]
        public int ReplyCount { get; set; }

        [Required]
        public bool IsPinned { get; set; }

        [Required]
        public bool IsLocked { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        public DateTime? LastReplyAt { get; set; }

        // 導航屬性
        [ForeignKey("ForumId")]
        public virtual Forum Forum { get; set; } = null!;

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        public virtual ICollection<PostReply> Replies { get; set; } = new List<PostReply>();
        public virtual ICollection<PostLike> Likes { get; set; } = new List<PostLike>();
        public virtual ICollection<PostBookmark> Bookmarks { get; set; } = new List<PostBookmark>();
    }

    /// <summary>
    /// 討論回覆實體
    /// </summary>
    [Table("PostReplies")]
    public class PostReply
    {
        [Key]
        public int ReplyId { get; set; }

        [Required]
        public int PostId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        public PostReplyStatus Status { get; set; }

        [Required]
        public int LikeCount { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        // 導航屬性
        [ForeignKey("PostId")]
        public virtual ForumPost Post { get; set; } = null!;

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        public virtual ICollection<PostReplyLike> Likes { get; set; } = new List<PostReplyLike>();
    }

    /// <summary>
    /// 討論讚實體
    /// </summary>
    [Table("PostLikes")]
    public class PostLike
    {
        [Key]
        public int LikeId { get; set; }

        [Required]
        public int PostId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        // 導航屬性
        [ForeignKey("PostId")]
        public virtual ForumPost Post { get; set; } = null!;

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }

    /// <summary>
    /// 討論回覆讚實體
    /// </summary>
    [Table("PostReplyLikes")]
    public class PostReplyLike
    {
        [Key]
        public int LikeId { get; set; }

        [Required]
        public int ReplyId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        // 導航屬性
        [ForeignKey("ReplyId")]
        public virtual PostReply Reply { get; set; } = null!;

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }

    /// <summary>
    /// 討論收藏實體
    /// </summary>
    [Table("PostBookmarks")]
    public class PostBookmark
    {
        [Key]
        public int BookmarkId { get; set; }

        [Required]
        public int PostId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        // 導航屬性
        [ForeignKey("PostId")]
        public virtual ForumPost Post { get; set; } = null!;

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }

    /// <summary>
    /// 版內主題表 (討論串)
    /// </summary>
    [Table("threads")]
    public class Thread
    {
        /// <summary>
        /// 主題編號 (主鍵)
        /// </summary>
        [Key]
        [Column("thread_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ThreadId { get; set; }

        /// <summary>
        /// 所屬版編號 (外鍵到 forums)
        /// </summary>
        [Column("forum_id")]
        [ForeignKey("Forum")]
        public int ForumId { get; set; }

        /// <summary>
        /// 作者編號 (外鍵到 Users)
        /// </summary>
        [Column("author_user_id")]
        [ForeignKey("AuthorUser")]
        public int AuthorUserId { get; set; }

        /// <summary>
        /// 標題
        /// </summary>
        [Column("title")]
        [StringLength(200)]
        public string? Title { get; set; }

        /// <summary>
        /// 狀態 (normal/hidden/archived)
        /// </summary>
        [Column("status")]
        [StringLength(20)]
        public string? Status { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        // 導航屬性
        public virtual Forum Forum { get; set; } = null!;
        public virtual User AuthorUser { get; set; } = null!;
        public virtual ICollection<ThreadPost> ThreadPosts { get; set; } = new List<ThreadPost>();
        public virtual ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
        public virtual ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
    }

    /// <summary>
    /// 主題回覆表 (支援二層結構)
    /// </summary>
    [Table("thread_posts")]
    public class ThreadPost
    {
        /// <summary>
        /// 回覆編號 (主鍵)
        /// </summary>
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// 主題編號 (外鍵到 threads)
        /// </summary>
        [Column("thread_id")]
        [ForeignKey("Thread")]
        public long ThreadId { get; set; }

        /// <summary>
        /// 回覆者編號 (外鍵到 Users)
        /// </summary>
        [Column("author_user_id")]
        [ForeignKey("AuthorUser")]
        public int AuthorUserId { get; set; }

        /// <summary>
        /// 內容 (Markdown)
        /// </summary>
        [Column("content_md")]
        public string? ContentMd { get; set; }

        /// <summary>
        /// 父回覆編號 (支援二層, 外鍵到 thread_posts)
        /// </summary>
        [Column("parent_post_id")]
        [ForeignKey("ParentPost")]
        public long? ParentPostId { get; set; }

        /// <summary>
        /// 狀態 (normal/hidden/deleted)
        /// </summary>
        [Column("status")]
        [StringLength(20)]
        public string? Status { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        // 導航屬性
        public virtual Thread Thread { get; set; } = null!;
        public virtual User AuthorUser { get; set; } = null!;
        public virtual ThreadPost? ParentPost { get; set; }
        public virtual ICollection<ThreadPost> ChildPosts { get; set; } = new List<ThreadPost>();
        public virtual ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
    }

    /// <summary>
    /// 討論狀態枚舉
    /// </summary>
    public enum PostStatus
    {
        Active = 1,
        Hidden = 2,
        Deleted = 3
    }

    /// <summary>
    /// 討論回覆狀態枚舉
    /// </summary>
    public enum PostReplyStatus
    {
        Active = 1,
        Hidden = 2,
        Deleted = 3
    }
}