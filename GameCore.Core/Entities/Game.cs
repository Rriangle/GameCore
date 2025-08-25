using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Core.Entities
{
    /// <summary>
    /// 遊戲主檔表
    /// </summary>
    [Table("games")]
    public class Game
    {
        /// <summary>
        /// 遊戲ID (主鍵，自動遞增)
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int game_id { get; set; }

        /// <summary>
        /// 遊戲名稱
        /// </summary>
        [Required]
        [StringLength(200)]
        public string name { get; set; } = string.Empty;

        /// <summary>
        /// 類型 (FPS/MOBA/RPG等)
        /// </summary>
        [Required]
        [StringLength(100)]
        public string genre { get; set; } = string.Empty;

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime created_at { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 遊戲描述
        /// </summary>
        [StringLength(2000)]
        public string? Description { get; set; }

        /// <summary>
        /// 遊戲封面圖片URL
        /// </summary>
        [StringLength(500)]
        public string? CoverImageUrl { get; set; }

        /// <summary>
        /// 遊戲標籤
        /// </summary>
        [StringLength(500)]
        public string? Tags { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// 排序順序
        /// </summary>
        public int SortOrder { get; set; } = 0;

        /// <summary>
        /// 遊戲平台
        /// </summary>
        [StringLength(100)]
        public string? Platform { get; set; }

        /// <summary>
        /// 開發商
        /// </summary>
        [StringLength(200)]
        public string? Developer { get; set; }

        /// <summary>
        /// 發行商
        /// </summary>
        [StringLength(200)]
        public string? Publisher { get; set; }

        /// <summary>
        /// 發行日期
        /// </summary>
        public DateTime? ReleaseDate { get; set; }

        /// <summary>
        /// 遊戲評分
        /// </summary>
        [Column(TypeName = "decimal(3,2)")]
        public decimal Rating { get; set; } = 0;

        /// <summary>
        /// 評分數量
        /// </summary>
        public int RatingCount { get; set; } = 0;

        // 導航屬性
        /// <summary>
        /// 論壇
        /// </summary>
        public virtual Forum? Forum { get; set; }

        /// <summary>
        /// 遊戲來源對應
        /// </summary>
        public virtual ICollection<GameSourceMap> GameSourceMaps { get; set; } = new List<GameSourceMap>();

        /// <summary>
        /// 每日指標
        /// </summary>
        public virtual ICollection<GameMetricDaily> GameMetricDailies { get; set; } = new List<GameMetricDaily>();

        /// <summary>
        /// 熱度指數
        /// </summary>
        public virtual ICollection<PopularityIndexDaily> PopularityIndexDailies { get; set; } = new List<PopularityIndexDaily>();

        /// <summary>
        /// 排行榜快照
        /// </summary>
        public virtual ICollection<LeaderboardSnapshot> LeaderboardSnapshots { get; set; } = new List<LeaderboardSnapshot>();

        /// <summary>
        /// 洞察貼文
        /// </summary>
        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

        /// <summary>
        /// 遊戲商品詳情
        /// </summary>
        public virtual ICollection<GameProductDetails> GameProductDetails { get; set; } = new List<GameProductDetails>();

        /// <summary>
        /// 收藏記錄
        /// </summary>
        public virtual ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
    }

    /// <summary>
    /// 數據來源字典表 - 定義要抓的外部平台
    /// </summary>
    [Table("metric_sources")]
    public class MetricSource
    {
        /// <summary>
        /// 來源編號 (主鍵)
        /// </summary>
        [Key]
        [Column("source_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SourceId { get; set; }

        /// <summary>
        /// 來源名稱 (Steam/Bahamut/YouTube等)
        /// </summary>
        [Column("name")]
        [StringLength(100)]
        public string? Name { get; set; }

        /// <summary>
        /// 備註 (抓法/限制)
        /// </summary>
        [Column("note")]
        [StringLength(500)]
        public string? Note { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        // 導航屬性
        public virtual ICollection<Metric> Metrics { get; set; } = new List<Metric>();
        public virtual ICollection<GameSourceMap> GameSourceMaps { get; set; } = new List<GameSourceMap>();
    }

    /// <summary>
    /// 指標字典表 - 來源底下的可用指標清單
    /// </summary>
    [Table("metrics")]
    public class Metric
    {
        /// <summary>
        /// 指標編號 (主鍵)
        /// </summary>
        [Key]
        [Column("metric_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MetricId { get; set; }

        /// <summary>
        /// 所屬來源編號 (外鍵到 metric_sources)
        /// </summary>
        [Column("source_id")]
        [ForeignKey("MetricSource")]
        public int SourceId { get; set; }

        /// <summary>
        /// 指標代碼 (concurrent_users/forum_posts等)
        /// </summary>
        [Column("code")]
        [StringLength(100)]
        public string? Code { get; set; }

        /// <summary>
        /// 單位 (users/posts/views)
        /// </summary>
        [Column("unit")]
        [StringLength(50)]
        public string? Unit { get; set; }

        /// <summary>
        /// 指標中文說明
        /// </summary>
        [Column("description")]
        [StringLength(200)]
        public string? Description { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// 建立時間
        /// </summary>
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        // 導航屬性
        public virtual MetricSource MetricSource { get; set; } = null!;
        public virtual ICollection<GameMetricDaily> GameMetricDailies { get; set; } = new List<GameMetricDaily>();
    }

    /// <summary>
    /// 外部ID對應表 - 把內部遊戲對應到各來源的外部鍵
    /// </summary>
    [Table("game_source_map")]
    public class GameSourceMap
    {
        /// <summary>
        /// 對應編號 (主鍵)
        /// </summary>
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// 內部遊戲編號 (外鍵到 games)
        /// </summary>
        [Column("game_id")]
        [ForeignKey("Game")]
        public int GameId { get; set; }

        /// <summary>
        /// 外部來源編號 (外鍵到 metric_sources)
        /// </summary>
        [Column("source_id")]
        [ForeignKey("MetricSource")]
        public int SourceId { get; set; }

        /// <summary>
        /// 外部ID (Steam appid / 巴哈 slug)
        /// </summary>
        [Column("external_key")]
        [StringLength(100)]
        public string? ExternalKey { get; set; }

        // 導航屬性
        public virtual Game Game { get; set; } = null!;
        public virtual MetricSource MetricSource { get; set; } = null!;
    }

    /// <summary>
    /// 每天每指標的原始值表 (清洗後), 是計算指數的底稿
    /// </summary>
    [Table("game_metric_daily")]
    public class GameMetricDaily
    {
        /// <summary>
        /// 流水號 (主鍵)
        /// </summary>
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// 遊戲編號 (外鍵到 games)
        /// </summary>
        [Column("game_id")]
        [ForeignKey("Game")]
        public int GameId { get; set; }

        /// <summary>
        /// 指標編號 (外鍵到 metrics)
        /// </summary>
        [Column("metric_id")]
        [ForeignKey("Metric")]
        public int MetricId { get; set; }

        /// <summary>
        /// 日期 (日粒度)
        /// </summary>
        [Column("date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// 數值 (清洗後)
        /// </summary>
        [Column("value")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal Value { get; set; }

        /// <summary>
        /// 聚合方法 (sum/avg/max)
        /// </summary>
        [Column("agg_method")]
        [StringLength(20)]
        public string? AggMethod { get; set; }

        /// <summary>
        /// 資料品質 (real/estimate/seed)
        /// </summary>
        [Column("quality")]
        [StringLength(20)]
        public string? Quality { get; set; }

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
        public virtual Game Game { get; set; } = null!;
        public virtual Metric Metric { get; set; } = null!;
    }

    /// <summary>
    /// 每日熱度指數表 (計算結果)
    /// </summary>
    [Table("popularity_index_daily")]
    public class PopularityIndexDaily
    {
        /// <summary>
        /// 流水號 (主鍵)
        /// </summary>
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// 遊戲編號 (外鍵到 games)
        /// </summary>
        [Column("game_id")]
        [ForeignKey("Game")]
        public int GameId { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        [Column("date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// 熱度指數 (加權計算)
        /// </summary>
        [Column("index_value")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal IndexValue { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        // 導航屬性
        public virtual Game Game { get; set; } = null!;
    }

    /// <summary>
    /// 榜單快照表 - 直接給前台讀, 避免重算
    /// </summary>
    [Table("leaderboard_snapshots")]
    public class LeaderboardSnapshot
    {
        /// <summary>
        /// 快照編號 (主鍵)
        /// </summary>
        [Key]
        [Column("snapshot_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SnapshotId { get; set; }

        /// <summary>
        /// 期間類型 (daily/weekly)
        /// </summary>
        [Column("period")]
        [StringLength(20)]
        public string? Period { get; set; }

        /// <summary>
        /// 快照時間
        /// </summary>
        [Column("ts")]
        public DateTime Ts { get; set; }

        /// <summary>
        /// 名次 (1..N)
        /// </summary>
        [Column("rank")]
        public int Rank { get; set; }

        /// <summary>
        /// 遊戲編號 (外鍵到 games)
        /// </summary>
        [Column("game_id")]
        [ForeignKey("Game")]
        public int GameId { get; set; }

        /// <summary>
        /// 當時指數值
        /// </summary>
        [Column("index_value")]
        [Column(TypeName = "decimal(18,4)")]
        public decimal IndexValue { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        // 導航屬性
        public virtual Game Game { get; set; } = null!;
    }
}