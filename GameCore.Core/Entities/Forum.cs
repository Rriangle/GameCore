using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 論壇版實體 - 每遊戲一個版
    /// </summary>
    [Table("forums")]
    public class Forum
    {
        /// <summary>
        /// 論壇版編號 (主鍵)
        /// </summary>
        [Key]
        [Column("forum_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ForumId { get; set; }

        /// <summary>
        /// 遊戲編號 (外鍵到 games, 一對一)
        /// </summary>
        [Required]
        [Column("game_id")]
        [ForeignKey("Game")]
        public int GameId { get; set; }

        /// <summary>
        /// 版名
        /// </summary>
        [Column("name")]
        [StringLength(100)]
        public string? Name { get; set; }

        /// <summary>
        /// 版說明
        /// </summary>
        [Column("description")]
        [StringLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        // 導航屬性
        [ForeignKey("GameId")]
        public virtual Game Game { get; set; } = null!;

        public virtual ICollection<Thread> Threads { get; set; } = new List<Thread>();
    }

    /// <summary>
    /// 版內主題實體 (討論串)
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
        /// 所屬版編號 (外鍵)
        /// </summary>
        [Required]
        [Column("forum_id")]
        [ForeignKey("Forum")]
        public int ForumId { get; set; }

        /// <summary>
        /// 作者編號 (外鍵)
        /// </summary>
        [Required]
        [Column("author_user_id")]
        [ForeignKey("Author")]
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
        [StringLength(50)]
        public string Status { get; set; } = "normal";

        /// <summary>
        /// 建立時間
        /// </summary>
        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        [Required]
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        // 導航屬性
        [ForeignKey("ForumId")]
        public virtual Forum Forum { get; set; } = null!;

        [ForeignKey("AuthorUserId")]
        public virtual User Author { get; set; } = null!;

        public virtual ICollection<ThreadPost> Posts { get; set; } = new List<ThreadPost>();
    }

    /// <summary>
    /// 主題回覆實體 (支援二層結構)
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
        /// 主題編號 (外鍵)
        /// </summary>
        [Required]
        [Column("thread_id")]
        [ForeignKey("Thread")]
        public long ThreadId { get; set; }

        /// <summary>
        /// 回覆者編號 (外鍵)
        /// </summary>
        [Required]
        [Column("author_user_id")]
        [ForeignKey("Author")]
        public int AuthorUserId { get; set; }

        /// <summary>
        /// 內容 (Markdown)
        /// </summary>
        [Required]
        [Column("content_md")]
        public string ContentMd { get; set; } = string.Empty;

        /// <summary>
        /// 父回覆編號 (支援二層)
        /// </summary>
        [Column("parent_post_id")]
        [ForeignKey("ParentPost")]
        public long? ParentPostId { get; set; }

        /// <summary>
        /// 狀態 (normal/hidden/deleted)
        /// </summary>
        [Column("status")]
        [StringLength(50)]
        public string Status { get; set; } = "normal";

        /// <summary>
        /// 建立時間
        /// </summary>
        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        [Required]
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        // 導航屬性
        [ForeignKey("ThreadId")]
        public virtual Thread Thread { get; set; } = null!;

        [ForeignKey("AuthorUserId")]
        public virtual User Author { get; set; } = null!;

        [ForeignKey("ParentPostId")]
        public virtual ThreadPost? ParentPost { get; set; }

        public virtual ICollection<ThreadPost> ChildPosts { get; set; } = new List<ThreadPost>();
    }

    /// <summary>
    /// 通用讚表 (多型)
    /// </summary>
    [Table("reactions")]
    public class Reaction
    {
        /// <summary>
        /// 流水號 (主鍵)
        /// </summary>
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// 誰按的 (外鍵)
        /// </summary>
        [Required]
        [Column("user_id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// 目標類型 (post/thread/thread_post)
        /// </summary>
        [Required]
        [Column("target_type")]
        [StringLength(50)]
        public string TargetType { get; set; } = string.Empty;

        /// <summary>
        /// 目標編號 (多型，不設FK)
        /// </summary>
        [Required]
        [Column("target_id")]
        public long TargetId { get; set; }

        /// <summary>
        /// 反應類型 (like/emoji...)
        /// </summary>
        [Required]
        [Column("kind")]
        [StringLength(50)]
        public string Kind { get; set; } = "like";

        /// <summary>
        /// 建立時間
        /// </summary>
        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        // 導航屬性
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }

    /// <summary>
    /// 通用收藏表 (多型)
    /// </summary>
    [Table("bookmarks")]
    public class Bookmark
    {
        /// <summary>
        /// 流水號 (主鍵)
        /// </summary>
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// 收藏者編號 (外鍵)
        /// </summary>
        [Required]
        [Column("user_id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// 目標類型 (post/thread/game/forum)
        /// </summary>
        [Required]
        [Column("target_type")]
        [StringLength(50)]
        public string TargetType { get; set; } = string.Empty;

        /// <summary>
        /// 目標編號 (多型，不設FK)
        /// </summary>
        [Required]
        [Column("target_id")]
        public long TargetId { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        // 導航屬性
        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }
}