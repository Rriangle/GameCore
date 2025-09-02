using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCore.Domain.Entities
{
    /// <summary>
    /// ?�戲實�?
    /// 平台?�援?��??��???
    /// </summary>
    [Table("games")]
    public partial class Game
    {
        /// <summary>
        /// ?�戲ID（主?��?
        /// </summary>
        [Key]
        [Column("game_id")]
        public int GameId { get; set; }

        /// <summary>
        /// ?�戲?�稱
        /// </summary>
        [Required]
        [Column("name")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// ?�戲類�?（FPS/MOBA等�?
        /// </summary>
        [Column("genre")]
        [StringLength(50)]
        public string? Genre { get; set; }

        /// <summary>
        /// 建�??��?
        /// </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // 導航屬�?
        /// <summary>
        /// ?�戲對�??��?壇�???
        /// </summary>
        public virtual Forum? Forum { get; set; }

        /// <summary>
        /// ?�戲?�收?��?�?
        /// </summary>
        public virtual ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
    }

    /// <summary>
    /// ?��?來�?字典�?- 定義要�??��??�平??
    /// </summary>
    [Table("metric_sources")]
    public class MetricSource
    {
        /// <summary>
        /// 來�?編�? (主鍵)
        /// </summary>
        [Key]
        [Column("source_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SourceId { get; set; }

        /// <summary>
        /// 來�??�稱 (Steam/Bahamut/YouTube�?
        /// </summary>
        [Column("name")]
        [StringLength(100)]
        public string? Name { get; set; }

        /// <summary>
        /// ?�註 (?��?/?�制)
        /// </summary>
        [Column("note")]
        [StringLength(500)]
        public string? Note { get; set; }

        /// <summary>
        /// 建�??��?
        /// </summary>
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        // 導航屬�?
        public virtual ICollection<Metric> Metrics { get; set; } = new List<Metric>();
        public virtual ICollection<GameSourceMap> GameSourceMaps { get; set; } = new List<GameSourceMap>();
    }

    /// <summary>
    /// ?��?字典�?- 來�?底�??�可?��?標�???
    /// </summary>
    [Table("metrics")]
    public class Metric
    {
        /// <summary>
        /// ?��?編�? (主鍵)
        /// </summary>
        [Key]
        [Column("metric_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MetricId { get; set; }

        /// <summary>
        /// ?�屬�?源編??(外鍵??metric_sources)
        /// </summary>
        [Column("source_id")]
        [ForeignKey("MetricSource")]
        public int SourceId { get; set; }

        /// <summary>
        /// ?��?�?�� (concurrent_users/forum_posts�?
        /// </summary>
        [Column("code")]
        [StringLength(100)]
        public string? Code { get; set; }

        /// <summary>
        /// ?��? (users/posts/views)
        /// </summary>
        [Column("unit")]
        [StringLength(50)]
        public string? Unit { get; set; }

        /// <summary>
        /// ?��?中�?說�?
        /// </summary>
        [Column("description")]
        [StringLength(200)]
        public string? Description { get; set; }

        /// <summary>
        /// ?�否?�用
        /// </summary>
        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// 建�??��?
        /// </summary>
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        // 導航屬�?
        public virtual MetricSource MetricSource { get; set; } = null!;
        public virtual ICollection<GameMetricDaily> GameMetricDailies { get; set; } = new List<GameMetricDaily>();
    }

    /// <summary>
    /// 外部ID對�?�?- ?�內?��??��??�到?��?源�?外部??
    /// </summary>
    [Table("game_source_map")]
    public class GameSourceMap
    {
        /// <summary>
        /// 對�?編�? (主鍵)
        /// </summary>
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// ?�部?�戲編�? (外鍵??games)
        /// </summary>
        [Column("game_id")]
        [ForeignKey("Game")]
        public int GameId { get; set; }

        /// <summary>
        /// 外部來�?編�? (外鍵??metric_sources)
        /// </summary>
        [Column("source_id")]
        [ForeignKey("MetricSource")]
        public int SourceId { get; set; }

        /// <summary>
        /// 外部ID (Steam appid / 巴�? slug)
        /// </summary>
        [Column("external_key")]
        [StringLength(100)]
        public string? ExternalKey { get; set; }

        // 導航屬�?
        public virtual Game Game { get; set; } = null!;
        public virtual MetricSource MetricSource { get; set; } = null!;
    }

    /// <summary>
    /// 每天每�?標�??��??�表 (清�?�?, ?��?算�??��?底稿
    /// </summary>
    [Table("game_metric_daily")]
    public class GameMetricDaily
    {
        /// <summary>
        /// 流水??(主鍵)
        /// </summary>
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// ?�戲編�? (外鍵??games)
        /// </summary>
        [Column("game_id")]
        [ForeignKey("Game")]
        public int GameId { get; set; }

        /// <summary>
        /// ?��?編�? (外鍵??metrics)
        /// </summary>
        [Column("metric_id")]
        [ForeignKey("Metric")]
        public int MetricId { get; set; }

        /// <summary>
        /// ?��? (?��?�?
        /// </summary>
        [Column("date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// ?��?(清�?�?
        /// </summary>
        [Column("value", TypeName = "decimal(18,4)")]
        public decimal Value { get; set; }

        /// <summary>
        /// ?��??��? (sum/avg/max)
        /// </summary>
        [Column("agg_method")]
        [StringLength(20)]
        public string? AggMethod { get; set; }

        /// <summary>
        /// 資�??�質 (real/estimate/seed)
        /// </summary>
        [Column("quality")]
        [StringLength(20)]
        public string? Quality { get; set; }

        /// <summary>
        /// 建�??��?
        /// </summary>
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// ?�新?��?
        /// </summary>
        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        // 導航屬�?
        public virtual Game Game { get; set; } = null!;
        public virtual Metric Metric { get; set; } = null!;
    }

    /// <summary>
    /// 每日?�度?�數�?(計�?結�?)
    /// </summary>
    [Table("popularity_index_daily")]
    public class PopularityIndexDaily
    {
        /// <summary>
        /// 流水??(主鍵)
        /// </summary>
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// ?�戲編�? (外鍵??games)
        /// </summary>
        [Column("game_id")]
        [ForeignKey("Game")]
        public int GameId { get; set; }

        /// <summary>
        /// ?��?
        /// </summary>
        [Column("date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// ?�度?�數 (?��?計�?)
        /// </summary>
        [Column("index_value", TypeName = "decimal(18,4)")]
        public decimal IndexValue { get; set; }

        /// <summary>
        /// 建�??��?
        /// </summary>
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        // 導航屬�?
        public virtual Game Game { get; set; } = null!;
    }

    /// <summary>
    /// 榜單快照�?- ?�接給�??��?, ?��??��?
    /// </summary>
    [Table("leaderboard_snapshots")]
    public class LeaderboardSnapshot
    {
        /// <summary>
        /// 快照編�? (主鍵)
        /// </summary>
        [Key]
        [Column("snapshot_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SnapshotId { get; set; }

        /// <summary>
        /// ?��?類�? (daily/weekly)
        /// </summary>
        [Column("period")]
        [StringLength(20)]
        public string? Period { get; set; }

        /// <summary>
        /// 快照?��?
        /// </summary>
        [Column("ts")]
        public DateTime Ts { get; set; }

        /// <summary>
        /// ?�次 (1..N)
        /// </summary>
        [Column("rank")]
        public int Rank { get; set; }

        /// <summary>
        /// ?�戲編�? (外鍵??games)
        /// </summary>
        [Column("game_id")]
        [ForeignKey("Game")]
        public int GameId { get; set; }

        /// <summary>
        /// ?��??�數??
        /// </summary>
        [Column("index_value", TypeName = "decimal(18,4)")]
        public decimal IndexValue { get; set; }

        /// <summary>
        /// 建�??��?
        /// </summary>
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        // 導航屬�?
        public virtual Game Game { get; set; } = null!;
    }
}
