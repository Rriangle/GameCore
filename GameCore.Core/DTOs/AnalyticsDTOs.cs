using System.ComponentModel.DataAnnotations;

namespace GameCore.Core.DTOs
{
    #region 遊戲管理 DTOs

    /// <summary>
    /// 遊戲 DTO
    /// </summary>
    public class GameDto
    {
        /// <summary>遊戲ID</summary>
        public int GameId { get; set; }

        /// <summary>遊戲名稱</summary>
        public string? GameName { get; set; }

        /// <summary>遊戲描述</summary>
        public string? GameDescription { get; set; }

        /// <summary>是否啟用</summary>
        public bool IsActive { get; set; }

        /// <summary>建立時間</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>最新熱度指數</summary>
        public decimal? LatestPopularityIndex { get; set; }

        /// <summary>排行榜位置</summary>
        public int? LeaderboardRank { get; set; }

        /// <summary>今日指標數量</summary>
        public int TodayMetricsCount { get; set; }

        /// <summary>外部來源對應</summary>
        public List<GameSourceMapDto> SourceMappings { get; set; } = new();
    }

    /// <summary>
    /// 建立遊戲請求 DTO
    /// </summary>
    public class CreateGameDto
    {
        /// <summary>遊戲名稱</summary>
        [Required(ErrorMessage = "遊戲名稱為必填")]
        [StringLength(100, ErrorMessage = "遊戲名稱長度不能超過100字元")]
        public string GameName { get; set; } = string.Empty;

        /// <summary>遊戲描述</summary>
        [StringLength(500, ErrorMessage = "遊戲描述長度不能超過500字元")]
        public string? GameDescription { get; set; }

        /// <summary>是否啟用</summary>
        public bool IsActive { get; set; } = true;
    }

    /// <summary>
    /// 更新遊戲請求 DTO
    /// </summary>
    public class UpdateGameDto
    {
        /// <summary>遊戲名稱</summary>
        [StringLength(100, ErrorMessage = "遊戲名稱長度不能超過100字元")]
        public string? GameName { get; set; }

        /// <summary>遊戲描述</summary>
        [StringLength(500, ErrorMessage = "遊戲描述長度不能超過500字元")]
        public string? GameDescription { get; set; }

        /// <summary>是否啟用</summary>
        public bool? IsActive { get; set; }
    }

    #endregion

    #region 指標來源管理 DTOs

    /// <summary>
    /// 指標來源 DTO
    /// </summary>
    public class MetricSourceDto
    {
        /// <summary>來源ID</summary>
        public int SourceId { get; set; }

        /// <summary>來源名稱</summary>
        public string? SourceName { get; set; }

        /// <summary>API端點</summary>
        public string? ApiEndpoint { get; set; }

        /// <summary>是否啟用</summary>
        public bool IsActive { get; set; }

        /// <summary>建立時間</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>相關指標數量</summary>
        public int MetricsCount { get; set; }

        /// <summary>對應遊戲數量</summary>
        public int GameMappingsCount { get; set; }
    }

    /// <summary>
    /// 建立指標來源請求 DTO
    /// </summary>
    public class CreateMetricSourceDto
    {
        /// <summary>來源名稱</summary>
        [Required(ErrorMessage = "來源名稱為必填")]
        [StringLength(100, ErrorMessage = "來源名稱長度不能超過100字元")]
        public string SourceName { get; set; } = string.Empty;

        /// <summary>API端點</summary>
        [StringLength(500, ErrorMessage = "API端點長度不能超過500字元")]
        public string? ApiEndpoint { get; set; }

        /// <summary>是否啟用</summary>
        public bool IsActive { get; set; } = true;
    }

    #endregion

    #region 指標管理 DTOs

    /// <summary>
    /// 指標 DTO
    /// </summary>
    public class MetricDto
    {
        /// <summary>指標ID</summary>
        public int MetricId { get; set; }

        /// <summary>來源ID</summary>
        public int SourceId { get; set; }

        /// <summary>來源名稱</summary>
        public string? SourceName { get; set; }

        /// <summary>指標代碼</summary>
        public string? Code { get; set; }

        /// <summary>單位</summary>
        public string? Unit { get; set; }

        /// <summary>指標說明</summary>
        public string? Description { get; set; }

        /// <summary>是否啟用</summary>
        public bool IsActive { get; set; }

        /// <summary>建立時間</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>權重</summary>
        public decimal Weight { get; set; } = 1.0m;

        /// <summary>今日數據點數量</summary>
        public int TodayDataPointsCount { get; set; }
    }

    /// <summary>
    /// 建立指標請求 DTO
    /// </summary>
    public class CreateMetricDto
    {
        /// <summary>來源ID</summary>
        [Required(ErrorMessage = "來源ID為必填")]
        public int SourceId { get; set; }

        /// <summary>指標代碼</summary>
        [Required(ErrorMessage = "指標代碼為必填")]
        [StringLength(50, ErrorMessage = "指標代碼長度不能超過50字元")]
        public string Code { get; set; } = string.Empty;

        /// <summary>單位</summary>
        [StringLength(20, ErrorMessage = "單位長度不能超過20字元")]
        public string? Unit { get; set; }

        /// <summary>指標說明</summary>
        [StringLength(200, ErrorMessage = "指標說明長度不能超過200字元")]
        public string? Description { get; set; }

        /// <summary>是否啟用</summary>
        public bool IsActive { get; set; } = true;

        /// <summary>權重</summary>
        [Range(0.1, 10.0, ErrorMessage = "權重必須在0.1到10.0之間")]
        public decimal Weight { get; set; } = 1.0m;
    }

    #endregion

    #region 遊戲來源對應 DTOs

    /// <summary>
    /// 遊戲來源對應 DTO
    /// </summary>
    public class GameSourceMapDto
    {
        /// <summary>對應ID</summary>
        public int MapId { get; set; }

        /// <summary>遊戲ID</summary>
        public int GameId { get; set; }

        /// <summary>遊戲名稱</summary>
        public string? GameName { get; set; }

        /// <summary>來源ID</summary>
        public int SourceId { get; set; }

        /// <summary>來源名稱</summary>
        public string? SourceName { get; set; }

        /// <summary>外部ID</summary>
        public string? ExternalId { get; set; }

        /// <summary>是否啟用</summary>
        public bool IsActive { get; set; }

        /// <summary>建立時間</summary>
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// 建立遊戲來源對應請求 DTO
    /// </summary>
    public class CreateGameSourceMapDto
    {
        /// <summary>遊戲ID</summary>
        [Required(ErrorMessage = "遊戲ID為必填")]
        public int GameId { get; set; }

        /// <summary>來源ID</summary>
        [Required(ErrorMessage = "來源ID為必填")]
        public int SourceId { get; set; }

        /// <summary>外部ID</summary>
        [Required(ErrorMessage = "外部ID為必填")]
        [StringLength(100, ErrorMessage = "外部ID長度不能超過100字元")]
        public string ExternalId { get; set; } = string.Empty;

        /// <summary>是否啟用</summary>
        public bool IsActive { get; set; } = true;
    }

    #endregion

    #region 每日指標數據 DTOs

    /// <summary>
    /// 每日指標數據 DTO
    /// </summary>
    public class GameMetricDailyDto
    {
        /// <summary>數據ID</summary>
        public int Id { get; set; }

        /// <summary>遊戲ID</summary>
        public int GameId { get; set; }

        /// <summary>遊戲名稱</summary>
        public string? GameName { get; set; }

        /// <summary>指標ID</summary>
        public int MetricId { get; set; }

        /// <summary>指標代碼</summary>
        public string? MetricCode { get; set; }

        /// <summary>指標說明</summary>
        public string? MetricDescription { get; set; }

        /// <summary>日期</summary>
        public DateTime Date { get; set; }

        /// <summary>數值</summary>
        public decimal Value { get; set; }

        /// <summary>聚合方法</summary>
        public string? AggMethod { get; set; }

        /// <summary>建立時間</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>更新時間</summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>與前日比較變化</summary>
        public decimal? DayOverDayChange { get; set; }

        /// <summary>與前日比較變化百分比</summary>
        public decimal? DayOverDayChangePercent { get; set; }
    }

    /// <summary>
    /// 建立每日指標數據請求 DTO
    /// </summary>
    public class CreateGameMetricDailyDto
    {
        /// <summary>遊戲ID</summary>
        [Required(ErrorMessage = "遊戲ID為必填")]
        public int GameId { get; set; }

        /// <summary>指標ID</summary>
        [Required(ErrorMessage = "指標ID為必填")]
        public int MetricId { get; set; }

        /// <summary>日期</summary>
        [Required(ErrorMessage = "日期為必填")]
        public DateTime Date { get; set; }

        /// <summary>數值</summary>
        [Required(ErrorMessage = "數值為必填")]
        [Range(0, double.MaxValue, ErrorMessage = "數值必須為非負數")]
        public decimal Value { get; set; }

        /// <summary>聚合方法</summary>
        [StringLength(20, ErrorMessage = "聚合方法長度不能超過20字元")]
        public string? AggMethod { get; set; } = "sum";
    }

    /// <summary>
    /// 批量建立每日指標數據請求 DTO
    /// </summary>
    public class BatchCreateGameMetricDailyDto
    {
        /// <summary>數據列表</summary>
        [Required(ErrorMessage = "數據列表為必填")]
        public List<CreateGameMetricDailyDto> Metrics { get; set; } = new();

        /// <summary>是否覆蓋已存在的數據</summary>
        public bool OverwriteExisting { get; set; } = false;
    }

    #endregion

    #region 熱度指數 DTOs

    /// <summary>
    /// 熱度指數 DTO
    /// </summary>
    public class PopularityIndexDailyDto
    {
        /// <summary>指數ID</summary>
        public long Id { get; set; }

        /// <summary>遊戲ID</summary>
        public int GameId { get; set; }

        /// <summary>遊戲名稱</summary>
        public string? GameName { get; set; }

        /// <summary>日期</summary>
        public DateTime Date { get; set; }

        /// <summary>熱度指數</summary>
        public decimal IndexValue { get; set; }

        /// <summary>建立時間</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>排行榜位置</summary>
        public int? Rank { get; set; }

        /// <summary>與前日比較變化</summary>
        public decimal? DayOverDayChange { get; set; }

        /// <summary>與前日比較變化百分比</summary>
        public decimal? DayOverDayChangePercent { get; set; }

        /// <summary>指數詳細計算資訊</summary>
        public PopularityIndexDetailsDto? Details { get; set; }
    }

    /// <summary>
    /// 熱度指數詳細計算資訊 DTO
    /// </summary>
    public class PopularityIndexDetailsDto
    {
        /// <summary>各指標貢獻值</summary>
        public Dictionary<string, decimal> MetricContributions { get; set; } = new();

        /// <summary>各指標權重</summary>
        public Dictionary<string, decimal> MetricWeights { get; set; } = new();

        /// <summary>各指標原始值</summary>
        public Dictionary<string, decimal> MetricRawValues { get; set; } = new();

        /// <summary>總權重</summary>
        public decimal TotalWeight { get; set; }

        /// <summary>加權總分</summary>
        public decimal WeightedScore { get; set; }

        /// <summary>正規化後分數</summary>
        public decimal NormalizedScore { get; set; }
    }

    #endregion

    #region 排行榜快照 DTOs

    /// <summary>
    /// 排行榜快照 DTO
    /// </summary>
    public class LeaderboardSnapshotDto
    {
        /// <summary>快照ID</summary>
        public long SnapshotId { get; set; }

        /// <summary>期間類型</summary>
        public string? Period { get; set; }

        /// <summary>快照時間</summary>
        public DateTime Ts { get; set; }

        /// <summary>遊戲ID</summary>
        public int GameId { get; set; }

        /// <summary>遊戲名稱</summary>
        public string? GameName { get; set; }

        /// <summary>排名</summary>
        public int Rank { get; set; }

        /// <summary>分數</summary>
        public decimal Score { get; set; }

        /// <summary>前一名次</summary>
        public int? PreviousRank { get; set; }

        /// <summary>名次變化</summary>
        public int? RankChange { get; set; }

        /// <summary>前一分數</summary>
        public decimal? PreviousScore { get; set; }

        /// <summary>分數變化</summary>
        public decimal? ScoreChange { get; set; }

        /// <summary>分數變化百分比</summary>
        public decimal? ScoreChangePercent { get; set; }

        /// <summary>排名變化趨勢</summary>
        public string RankTrend
        {
            get
            {
                if (RankChange == null) return "new";
                if (RankChange > 0) return "up";
                if (RankChange < 0) return "down";
                return "stable";
            }
        }
    }

    /// <summary>
    /// 排行榜查詢請求 DTO
    /// </summary>
    public class LeaderboardQueryDto
    {
        /// <summary>期間類型</summary>
        [Required(ErrorMessage = "期間類型為必填")]
        public string Period { get; set; } = "daily";

        /// <summary>查詢日期</summary>
        public DateTime? Date { get; set; }

        /// <summary>前幾名</summary>
        [Range(1, 100, ErrorMessage = "排名範圍必須在1到100之間")]
        public int TopN { get; set; } = 10;

        /// <summary>遊戲ID篩選</summary>
        public int? GameId { get; set; }

        /// <summary>是否包含變化資訊</summary>
        public bool IncludeChanges { get; set; } = true;
    }

    /// <summary>
    /// 排行榜統計 DTO
    /// </summary>
    public class LeaderboardStatsDto
    {
        /// <summary>期間類型</summary>
        public string? Period { get; set; }

        /// <summary>統計日期</summary>
        public DateTime Date { get; set; }

        /// <summary>總遊戲數</summary>
        public int TotalGames { get; set; }

        /// <summary>平均分數</summary>
        public decimal AverageScore { get; set; }

        /// <summary>最高分數</summary>
        public decimal MaxScore { get; set; }

        /// <summary>最低分數</summary>
        public decimal MinScore { get; set; }

        /// <summary>分數標準差</summary>
        public decimal ScoreStandardDeviation { get; set; }

        /// <summary>上升遊戲數</summary>
        public int GamesUp { get; set; }

        /// <summary>下降遊戲數</summary>
        public int GamesDown { get; set; }

        /// <summary>保持不變遊戲數</summary>
        public int GamesStable { get; set; }

        /// <summary>新進入榜單遊戲數</summary>
        public int NewGames { get; set; }
    }

    #endregion

    #region 洞察貼文 DTOs

    /// <summary>
    /// 洞察貼文 DTO
    /// </summary>
    public class InsightPostDto
    {
        /// <summary>貼文ID</summary>
        public int PostId { get; set; }

        /// <summary>遊戲ID</summary>
        public int? GameId { get; set; }

        /// <summary>遊戲名稱</summary>
        public string? GameName { get; set; }

        /// <summary>標題</summary>
        public string? Title { get; set; }

        /// <summary>內容</summary>
        public string? Content { get; set; }

        /// <summary>狀態</summary>
        public string? Status { get; set; }

        /// <summary>是否置頂</summary>
        public bool IsPinned { get; set; }

        /// <summary>建立時間</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>發佈時間</summary>
        public DateTime? PublishedAt { get; set; }

        /// <summary>瀏覽次數</summary>
        public int ViewCount { get; set; }

        /// <summary>點讚數</summary>
        public int LikeCount { get; set; }

        /// <summary>評論數</summary>
        public int CommentCount { get; set; }

        /// <summary>指標快照</summary>
        public PostMetricSnapshotDto? MetricSnapshot { get; set; }

        /// <summary>引用來源</summary>
        public List<PostSourceDto> Sources { get; set; } = new();
    }

    /// <summary>
    /// 建立洞察貼文請求 DTO
    /// </summary>
    public class CreateInsightPostDto
    {
        /// <summary>遊戲ID</summary>
        public int? GameId { get; set; }

        /// <summary>標題</summary>
        [Required(ErrorMessage = "標題為必填")]
        [StringLength(200, ErrorMessage = "標題長度不能超過200字元")]
        public string Title { get; set; } = string.Empty;

        /// <summary>內容</summary>
        [Required(ErrorMessage = "內容為必填")]
        public string Content { get; set; } = string.Empty;

        /// <summary>是否置頂</summary>
        public bool IsPinned { get; set; } = false;

        /// <summary>立即發佈</summary>
        public bool PublishNow { get; set; } = false;

        /// <summary>引用來源</summary>
        public List<string> SourceUrls { get; set; } = new();
    }

    #endregion

    #region 貼文指標快照 DTOs

    /// <summary>
    /// 貼文指標快照 DTO
    /// </summary>
    public class PostMetricSnapshotDto
    {
        /// <summary>快照ID</summary>
        public int SnapshotId { get; set; }

        /// <summary>貼文ID</summary>
        public int PostId { get; set; }

        /// <summary>遊戲ID</summary>
        public int? GameId { get; set; }

        /// <summary>快照日期</summary>
        public DateTime Date { get; set; }

        /// <summary>當日指數</summary>
        public decimal IndexValue { get; set; }

        /// <summary>詳細資訊 (JSON)</summary>
        public string? DetailsJson { get; set; }

        /// <summary>建立時間</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>解析後的詳細資訊</summary>
        public Dictionary<string, object>? ParsedDetails { get; set; }
    }

    #endregion

    #region 貼文來源 DTOs

    /// <summary>
    /// 貼文來源 DTO
    /// </summary>
    public class PostSourceDto
    {
        /// <summary>來源ID</summary>
        public int SourceId { get; set; }

        /// <summary>貼文ID</summary>
        public int PostId { get; set; }

        /// <summary>來源URL</summary>
        public string? SourceUrl { get; set; }

        /// <summary>來源標題</summary>
        public string? SourceTitle { get; set; }

        /// <summary>來源類型</summary>
        public string? SourceType { get; set; }

        /// <summary>建立時間</summary>
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// 建立貼文來源請求 DTO
    /// </summary>
    public class CreatePostSourceDto
    {
        /// <summary>貼文ID</summary>
        [Required(ErrorMessage = "貼文ID為必填")]
        public int PostId { get; set; }

        /// <summary>來源URL</summary>
        [Required(ErrorMessage = "來源URL為必填")]
        [Url(ErrorMessage = "來源URL格式不正確")]
        public string SourceUrl { get; set; } = string.Empty;

        /// <summary>來源標題</summary>
        [StringLength(200, ErrorMessage = "來源標題長度不能超過200字元")]
        public string? SourceTitle { get; set; }

        /// <summary>來源類型</summary>
        [StringLength(50, ErrorMessage = "來源類型長度不能超過50字元")]
        public string? SourceType { get; set; } = "external";
    }

    #endregion

    #region 分頁和查詢結果 DTOs

    /// <summary>
    /// 分析分頁結果 DTO
    /// </summary>
    /// <typeparam name="T">資料類型</typeparam>
    public class AnalyticsPagedResult<T>
    {
        /// <summary>當前頁碼</summary>
        public int Page { get; set; }

        /// <summary>每頁筆數</summary>
        public int PageSize { get; set; }

        /// <summary>總筆數</summary>
        public int TotalCount { get; set; }

        /// <summary>總頁數</summary>
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        /// <summary>是否有上一頁</summary>
        public bool HasPreviousPage => Page > 1;

        /// <summary>是否有下一頁</summary>
        public bool HasNextPage => Page < TotalPages;

        /// <summary>資料列表</summary>
        public List<T> Data { get; set; } = new();

        /// <summary>統計摘要</summary>
        public Dictionary<string, object>? Summary { get; set; }

        /// <summary>是否為空結果</summary>
        public bool IsEmpty => !Data.Any();
    }

    /// <summary>
    /// 時間序列數據點 DTO
    /// </summary>
    public class TimeSeriesDataPointDto
    {
        /// <summary>時間戳</summary>
        public DateTime Timestamp { get; set; }

        /// <summary>數值</summary>
        public decimal Value { get; set; }

        /// <summary>標籤</summary>
        public string? Label { get; set; }

        /// <summary>額外屬性</summary>
        public Dictionary<string, object>? Properties { get; set; }
    }

    /// <summary>
    /// 時間序列查詢請求 DTO
    /// </summary>
    public class TimeSeriesQueryDto
    {
        /// <summary>開始日期</summary>
        [Required(ErrorMessage = "開始日期為必填")]
        public DateTime StartDate { get; set; }

        /// <summary>結束日期</summary>
        [Required(ErrorMessage = "結束日期為必填")]
        public DateTime EndDate { get; set; }

        /// <summary>遊戲ID篩選</summary>
        public List<int>? GameIds { get; set; }

        /// <summary>指標ID篩選</summary>
        public List<int>? MetricIds { get; set; }

        /// <summary>聚合粒度</summary>
        public string Granularity { get; set; } = "daily";

        /// <summary>聚合方法</summary>
        public string AggregationMethod { get; set; } = "sum";

        /// <summary>是否填補缺失數據</summary>
        public bool FillMissingData { get; set; } = true;
    }

    #endregion

    #region 服務結果 DTOs

    /// <summary>
    /// 分析服務執行結果
    /// </summary>
    public class AnalyticsServiceResult
    {
        /// <summary>是否成功</summary>
        public bool Success { get; set; }

        /// <summary>訊息</summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>錯誤清單</summary>
        public List<string> Errors { get; set; } = new();

        /// <summary>處理記錄數</summary>
        public int ProcessedCount { get; set; }

        /// <summary>建立成功結果</summary>
        public static AnalyticsServiceResult CreateSuccess(string message = "操作成功", int processedCount = 0)
        {
            return new AnalyticsServiceResult 
            { 
                Success = true, 
                Message = message, 
                ProcessedCount = processedCount 
            };
        }

        /// <summary>建立失敗結果</summary>
        public static AnalyticsServiceResult CreateFailure(string message, List<string>? errors = null)
        {
            return new AnalyticsServiceResult 
            { 
                Success = false, 
                Message = message, 
                Errors = errors ?? new List<string>() 
            };
        }
    }

    /// <summary>
    /// 帶資料的分析服務執行結果
    /// </summary>
    /// <typeparam name="T">資料類型</typeparam>
    public class AnalyticsServiceResult<T> : AnalyticsServiceResult
    {
        /// <summary>結果資料</summary>
        public T? Data { get; set; }

        /// <summary>建立成功結果</summary>
        public static AnalyticsServiceResult<T> CreateSuccess(T data, string message = "操作成功", int processedCount = 0)
        {
            return new AnalyticsServiceResult<T> 
            { 
                Success = true, 
                Message = message, 
                Data = data,
                ProcessedCount = processedCount
            };
        }

        /// <summary>建立失敗結果</summary>
        public static new AnalyticsServiceResult<T> CreateFailure(string message, List<string>? errors = null)
        {
            return new AnalyticsServiceResult<T> 
            { 
                Success = false, 
                Message = message, 
                Errors = errors ?? new List<string>() 
            };
        }
    }

    #endregion

    #region 儀表板統計 DTOs

    /// <summary>
    /// 分析儀表板統計 DTO
    /// </summary>
    public class AnalyticsDashboardDto
    {
        /// <summary>總遊戲數</summary>
        public int TotalGames { get; set; }

        /// <summary>活躍遊戲數</summary>
        public int ActiveGames { get; set; }

        /// <summary>總指標數</summary>
        public int TotalMetrics { get; set; }

        /// <summary>今日數據點數</summary>
        public int TodayDataPoints { get; set; }

        /// <summary>平均熱度指數</summary>
        public decimal AveragePopularityIndex { get; set; }

        /// <summary>最高熱度指數</summary>
        public decimal MaxPopularityIndex { get; set; }

        /// <summary>今日排行榜快照數</summary>
        public int TodaySnapshotCount { get; set; }

        /// <summary>洞察貼文數</summary>
        public int InsightPostsCount { get; set; }

        /// <summary>熱門遊戲排行</summary>
        public List<LeaderboardSnapshotDto> TopGames { get; set; } = new();

        /// <summary>最新洞察貼文</summary>
        public List<InsightPostDto> LatestInsights { get; set; } = new();

        /// <summary>每日指標趨勢</summary>
        public List<TimeSeriesDataPointDto> DailyTrends { get; set; } = new();

        /// <summary>指標來源分布</summary>
        public Dictionary<string, int> SourceDistribution { get; set; } = new();
    }

    #endregion
}