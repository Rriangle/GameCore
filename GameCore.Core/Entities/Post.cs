using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 統一貼文表 - 洞察與未來UGC都走這
    /// </summary>
    [Table("posts")]
    public class Post
    {
        /// <summary>
        /// 文章編號 (主鍵)
        /// </summary>
        [Key]
        [Column("post_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostId { get; set; }

        /// <summary>
        /// 類型 (insight/user)
        /// </summary>
        [Column("type")]
        [StringLength(50)]
        public string? Type { get; set; }

        /// <summary>
        /// 關聯遊戲編號 (可為NULL, 外鍵到 games)
        /// </summary>
        [Column("game_id")]
        [ForeignKey("Game")]
        public int? GameId { get; set; }

        /// <summary>
        /// 標題
        /// </summary>
        [Column("title")]
        [StringLength(200)]
        public string? Title { get; set; }

        /// <summary>
        /// 三行摘要 (卡片用)
        /// </summary>
        [Column("tldr")]
        [StringLength(500)]
        public string? Tldr { get; set; }

        /// <summary>
        /// 內文 (Markdown)
        /// </summary>
        [Column("body_md")]
        public string? BodyMd { get; set; }

        /// <summary>
        /// 可見性 (public/hidden)
        /// </summary>
        [Column("visibility")]
        public bool Visibility { get; set; } = true;

        /// <summary>
        /// 狀態 (draft/published/hidden)
        /// </summary>
        [Column("status")]
        [StringLength(20)]
        public string? Status { get; set; }

        /// <summary>
        /// 是否置頂
        /// </summary>
        [Column("pinned")]
        public bool Pinned { get; set; } = false;

        /// <summary>
        /// 作者編號 (外鍵到 Users)
        /// </summary>
        [Column("created_by")]
        [ForeignKey("CreatedByUser")]
        public int? CreatedBy { get; set; }

        /// <summary>
        /// 發佈時間
        /// </summary>
        [Column("published_at")]
        public DateTime? PublishedAt { get; set; }

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
        public virtual Game? Game { get; set; }
        public virtual User? CreatedByUser { get; set; }
        public virtual PostMetricSnapshot? PostMetricSnapshot { get; set; }
        public virtual ICollection<PostSource> PostSources { get; set; } = new List<PostSource>();
        public virtual ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
        public virtual ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
    }

    /// <summary>
    /// 洞察發佈時的數據快照表 (固定展示)
    /// </summary>
    [Table("post_metric_snapshot")]
    public class PostMetricSnapshot
    {
        /// <summary>
        /// 文章編號 (主鍵, 外鍵到 posts)
        /// </summary>
        [Key]
        [Column("post_id")]
        [ForeignKey("Post")]
        public long PostId { get; set; }

        /// <summary>
        /// 當時的遊戲編號 (外鍵到 games)
        /// </summary>
        [Column("game_id")]
        [ForeignKey("Game")]
        public int? GameId { get; set; }

        /// <summary>
        /// 拍照日期
        /// </summary>
        [Column("date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// 當日指數
        /// </summary>
        [Column("index_value", TypeName = "decimal(18,4)")]
        public decimal IndexValue { get; set; }

        /// <summary>
        /// 當日各指標值/Δ%/權重 (JSON)
        /// </summary>
        [Column("details_json")]
        public string? DetailsJson { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        // 導航屬性
        public virtual Post Post { get; set; } = null!;
        public virtual Game? Game { get; set; }
    }

    /// <summary>
    /// 洞察引用來源清單表
    /// </summary>
    [Table("post_sources")]
    public class PostSource
    {
        /// <summary>
        /// 流水號 (主鍵)
        /// </summary>
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// 文章編號 (外鍵到 posts)
        /// </summary>
        [Column("post_id")]
        [ForeignKey("Post")]
        public long PostId { get; set; }

        /// <summary>
        /// 顯示名稱
        /// </summary>
        [Column("source_name")]
        [StringLength(100)]
        public string? SourceName { get; set; }

        /// <summary>
        /// 外部連結
        /// </summary>
        [Column("url")]
        [StringLength(500)]
        public string? Url { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        // 導航屬性
        public virtual Post Post { get; set; } = null!;
    }

    /// <summary>
    /// 論壇版主檔表 - 每遊戲一個版
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
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        // 導航屬性
        public virtual Game Game { get; set; } = null!;
        public virtual ICollection<Thread> Threads { get; set; } = new List<Thread>();
        public virtual ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
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
        /// 誰按的 (外鍵到 Users)
        /// </summary>
        [Column("user_id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// 目標類型 (post/thread/thread_post)
        /// </summary>
        [Column("target_type")]
        [StringLength(50)]
        public string? TargetType { get; set; }

        /// <summary>
        /// 目標編號 (多型, 不設FK)
        /// </summary>
        [Column("target_id")]
        public long TargetId { get; set; }

        /// <summary>
        /// 反應類型 (like/emoji等)
        /// </summary>
        [Column("kind")]
        [StringLength(50)]
        public string? Kind { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        // 導航屬性
        public virtual User User { get; set; } = null!;
        public virtual Post? Post { get; set; }
        public virtual Thread? Thread { get; set; }
        public virtual ThreadPost? ThreadPost { get; set; }
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
        /// 收藏者編號 (外鍵到 Users)
        /// </summary>
        [Column("user_id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        /// <summary>
        /// 目標類型 ('post' | 'thread' | 'game' | 'forum')
        /// </summary>
        [Column("target_type")]
        [StringLength(50)]
        public string? TargetType { get; set; }

        /// <summary>
        /// 目標編號 (多型, 不設FK)
        /// </summary>
        [Column("target_id")]
        public long TargetId { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        // 導航屬性
        public virtual User User { get; set; } = null!;
        public virtual Post? Post { get; set; }
        public virtual Thread? Thread { get; set; }
        public virtual Game? Game { get; set; }
        public virtual Forum? Forum { get; set; }
    }
}