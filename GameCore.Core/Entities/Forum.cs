using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 論壇版主檔表
    /// </summary>
    [Table("forums")]
    public class Forum
    {
        /// <summary>
        /// 論壇版ID (主鍵，自動遞增)
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int forum_id { get; set; }

        /// <summary>
        /// 遊戲ID (外鍵到games，一對一)
        /// </summary>
        [ForeignKey("Game")]
        public int? game_id { get; set; }

        /// <summary>
        /// 版名
        /// </summary>
        [Required]
        [StringLength(100)]
        public string name { get; set; } = string.Empty;

        /// <summary>
        /// 版說明
        /// </summary>
        [StringLength(500)]
        public string? description { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime created_at { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 版主ID
        /// </summary>
        public int? ModeratorId { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// 排序順序
        /// </summary>
        public int SortOrder { get; set; } = 0;

        /// <summary>
        /// 今日新貼數量
        /// </summary>
        public int TodayPosts { get; set; } = 0;

        /// <summary>
        /// 活躍用戶數量
        /// </summary>
        public int ActiveUsers { get; set; } = 0;

        /// <summary>
        /// 總主題數量
        /// </summary>
        public int TotalThreads { get; set; } = 0;

        /// <summary>
        /// 總回覆數量
        /// </summary>
        public int TotalPosts { get; set; } = 0;

        // 導航屬性
        /// <summary>
        /// 遊戲
        /// </summary>
        public virtual Game? Game { get; set; }

        /// <summary>
        /// 版主
        /// </summary>
        [ForeignKey("ModeratorId")]
        public virtual User? Moderator { get; set; }

        /// <summary>
        /// 主題列表
        /// </summary>
        public virtual ICollection<Thread> Threads { get; set; } = new List<Thread>();

        /// <summary>
        /// 收藏記錄
        /// </summary>
        public virtual ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
    }

    /// <summary>
    /// 討論主題實體
    /// </summary>
    [Table("Posts")]
    public class Post
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

        public int? ParentReplyId { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        public PostStatus Status { get; set; }

        [Required]
        public int LikeCount { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        // 導航屬性
        [ForeignKey("PostId")]
        public virtual Post Post { get; set; } = null!;

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;

        [ForeignKey("ParentReplyId")]
        public virtual PostReply? ParentReply { get; set; }

        public virtual ICollection<PostReply> ChildReplies { get; set; } = new List<PostReply>();
        public virtual ICollection<PostReplyLike> Likes { get; set; } = new List<PostReplyLike>();
    }

    /// <summary>
    /// 貼文按讚實體
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
        public virtual Post Post { get; set; } = null!;

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }

    /// <summary>
    /// 回覆按讚實體
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
    /// 貼文收藏實體
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
        public virtual Post Post { get; set; } = null!;

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }

    /// <summary>
    /// 貼文狀態列舉
    /// </summary>
    public enum PostStatus
    {
        Normal = 1,        // 正常
        Hidden = 2,        // 隱藏
        Archived = 3,      // 封存
        Deleted = 4        // 刪除
    }
}