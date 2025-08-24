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
}