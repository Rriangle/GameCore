using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 論壇表 (對應 Forums)
    /// </summary>
    [Table("Forums")]
    public class Forum
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [StringLength(50)]
        public string Category { get; set; } = string.Empty;

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdateTime { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
    }

    /// <summary>
    /// 貼文表 (對應 Posts)
    /// </summary>
    [Table("Posts")]
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ForumId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Tags { get; set; }

        [Required]
        public int ViewCount { get; set; } = 0;

        [Required]
        public int LikeCount { get; set; } = 0;

        [Required]
        public int ReplyCount { get; set; } = 0;

        [Required]
        public bool IsSticky { get; set; } = false;

        [Required]
        public bool IsLocked { get; set; } = false;

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdateTime { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Forum Forum { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual ICollection<PostReply> Replies { get; set; } = new List<PostReply>();
        public virtual ICollection<PostLike> Likes { get; set; } = new List<PostLike>();
        public virtual ICollection<PostBookmark> Bookmarks { get; set; } = new List<PostBookmark>();
    }

    /// <summary>
    /// 貼文回覆表 (對應 PostReplies)
    /// </summary>
    [Table("PostReplies")]
    public class PostReply
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int PostId { get; set; }

        [Required]
        public int UserId { get; set; }

        public int? ParentReplyId { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        public int LikeCount { get; set; } = 0;

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdateTime { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Post Post { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual PostReply? ParentReply { get; set; }
        public virtual ICollection<PostReply> ChildReplies { get; set; } = new List<PostReply>();
    }

    /// <summary>
    /// 貼文按讚表 (對應 PostLikes)
    /// </summary>
    [Table("PostLikes")]
    public class PostLike
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int PostId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Post Post { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }

    /// <summary>
    /// 貼文收藏表 (對應 PostBookmarks)
    /// </summary>
    [Table("PostBookmarks")]
    public class PostBookmark
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int PostId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual Post Post { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}