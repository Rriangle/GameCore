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

        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
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