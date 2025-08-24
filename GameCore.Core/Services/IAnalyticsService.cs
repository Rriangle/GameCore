using GameCore.Core.DTOs;

namespace GameCore.Core.Services
{
    /// <summary>
    /// 分析服務介面 - 完整實現遊戲熱度、排行榜、洞察功能
    /// 提供指標來源管理、每日數據收集、熱度指數計算、排行榜快照、洞察貼文等完整分析功能
    /// 嚴格按照規格要求實現每日指標→指數計算→榜單快照的完整流程
    /// </summary>
    public interface IAnalyticsService
    {
        #region 遊戲管理

        /// <summary>
        /// 取得遊戲列表
        /// 支援篩選和排序
        /// </summary>
        /// <param name="activeOnly">是否只顯示啟用的遊戲</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <returns>分頁遊戲列表</returns>
        Task<AnalyticsPagedResult<GameDto>> GetGamesAsync(bool activeOnly = true, int page = 1, int pageSize = 20);

        /// <summary>
        /// 取得遊戲詳細資訊
        /// 包含最新熱度指數和排行榜位置
        /// </summary>
        /// <param name="gameId">遊戲ID</param>
        /// <returns>遊戲詳細資訊</returns>
        Task<GameDto?> GetGameDetailAsync(int gameId);

        /// <summary>
        /// 建立遊戲
        /// 管理員限定功能
        /// </summary>
        /// <param name="createDto">建立遊戲請求</param>
        /// <returns>操作結果和遊戲資訊</returns>
        Task<AnalyticsServiceResult<GameDto>> CreateGameAsync(CreateGameDto createDto);

        /// <summary>
        /// 更新遊戲
        /// 管理員限定功能
        /// </summary>
        /// <param name="gameId">遊戲ID</param>
        /// <param name="updateDto">更新遊戲請求</param>
        /// <returns>操作結果和更新後遊戲資訊</returns>
        Task<AnalyticsServiceResult<GameDto>> UpdateGameAsync(int gameId, UpdateGameDto updateDto);

        #endregion

        #region 指標來源管理

        /// <summary>
        /// 取得指標來源列表
        /// 返回所有可用的數據來源
        /// </summary>
        /// <param name="activeOnly">是否只顯示啟用的來源</param>
        /// <returns>指標來源列表</returns>
        Task<List<MetricSourceDto>> GetMetricSourcesAsync(bool activeOnly = true);

        /// <summary>
        /// 建立指標來源
        /// 管理員限定功能
        /// </summary>
        /// <param name="createDto">建立指標來源請求</param>
        /// <returns>操作結果和來源資訊</returns>
        Task<AnalyticsServiceResult<MetricSourceDto>> CreateMetricSourceAsync(CreateMetricSourceDto createDto);

        /// <summary>
        /// 取得指標列表
        /// 支援按來源篩選
        /// </summary>
        /// <param name="sourceId">來源ID篩選</param>
        /// <param name="activeOnly">是否只顯示啟用的指標</param>
        /// <returns>指標列表</returns>
        Task<List<MetricDto>> GetMetricsAsync(int? sourceId = null, bool activeOnly = true);

        /// <summary>
        /// 建立指標
        /// 管理員限定功能
        /// </summary>
        /// <param name="createDto">建立指標請求</param>
        /// <returns>操作結果和指標資訊</returns>
        Task<AnalyticsServiceResult<MetricDto>> CreateMetricAsync(CreateMetricDto createDto);

        #endregion

        #region 遊戲來源對應管理

        /// <summary>
        /// 取得遊戲來源對應列表
        /// 管理外部來源與內部遊戲的對應關係
        /// </summary>
        /// <param name="gameId">遊戲ID篩選</param>
        /// <param name="sourceId">來源ID篩選</param>
        /// <returns>對應關係列表</returns>
        Task<List<GameSourceMapDto>> GetGameSourceMapsAsync(int? gameId = null, int? sourceId = null);

        /// <summary>
        /// 建立遊戲來源對應
        /// 管理員限定功能
        /// </summary>
        /// <param name="createDto">建立對應請求</param>
        /// <returns>操作結果</returns>
        Task<AnalyticsServiceResult<GameSourceMapDto>> CreateGameSourceMapAsync(CreateGameSourceMapDto createDto);

        #endregion

        #region 每日指標數據管理

        /// <summary>
        /// 取得每日指標數據
        /// 支援時間範圍和指標篩選
        /// </summary>
        /// <param name="gameId">遊戲ID篩選</param>
        /// <param name="metricId">指標ID篩選</param>
        /// <param name="startDate">開始日期</param>
        /// <param name="endDate">結束日期</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <returns>分頁每日指標數據</returns>
        Task<AnalyticsPagedResult<GameMetricDailyDto>> GetGameMetricDailyAsync(
            int? gameId = null, int? metricId = null, DateTime? startDate = null, DateTime? endDate = null,
            int page = 1, int pageSize = 50);

        /// <summary>
        /// 建立每日指標數據
        /// UPSERT 操作，防止重複
        /// </summary>
        /// <param name="createDto">建立數據請求</param>
        /// <returns>操作結果</returns>
        Task<AnalyticsServiceResult<GameMetricDailyDto>> CreateGameMetricDailyAsync(CreateGameMetricDailyDto createDto);

        /// <summary>
        /// 批量建立每日指標數據
        /// 支援大量數據快速導入
        /// </summary>
        /// <param name="batchDto">批量建立請求</param>
        /// <returns>操作結果和處理統計</returns>
        Task<AnalyticsServiceResult> BatchCreateGameMetricDailyAsync(BatchCreateGameMetricDailyDto batchDto);

        /// <summary>
        /// 取得時間序列數據
        /// 用於圖表展示和趨勢分析
        /// </summary>
        /// <param name="queryDto">時間序列查詢請求</param>
        /// <returns>時間序列數據</returns>
        Task<List<TimeSeriesDataPointDto>> GetTimeSeriesDataAsync(TimeSeriesQueryDto queryDto);

        #endregion

        #region 熱度指數計算

        /// <summary>
        /// 計算每日熱度指數
        /// 根據每日指標數據計算加權熱度指數
        /// </summary>
        /// <param name="date">計算日期</param>
        /// <param name="gameIds">遊戲ID列表，null為全部遊戲</param>
        /// <returns>計算結果和處理統計</returns>
        Task<AnalyticsServiceResult> CalculatePopularityIndexAsync(DateTime date, List<int>? gameIds = null);

        /// <summary>
        /// 取得熱度指數數據
        /// 支援時間範圍和遊戲篩選
        /// </summary>
        /// <param name="gameId">遊戲ID篩選</param>
        /// <param name="startDate">開始日期</param>
        /// <param name="endDate">結束日期</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <returns>分頁熱度指數數據</returns>
        Task<AnalyticsPagedResult<PopularityIndexDailyDto>> GetPopularityIndexDailyAsync(
            int? gameId = null, DateTime? startDate = null, DateTime? endDate = null,
            int page = 1, int pageSize = 50);

        /// <summary>
        /// 取得遊戲熱度排行榜
        /// 即時計算當前熱度排名
        /// </summary>
        /// <param name="date">查詢日期</param>
        /// <param name="topN">前N名</param>
        /// <returns>熱度排行榜</returns>
        Task<List<PopularityIndexDailyDto>> GetPopularityRankingAsync(DateTime? date = null, int topN = 10);

        #endregion

        #region 排行榜快照管理

        /// <summary>
        /// 產製排行榜快照
        /// 定期產製 leaderboard_snapshots
        /// </summary>
        /// <param name="period">期間類型 (daily/weekly)</param>
        /// <param name="ts">快照時間</param>
        /// <returns>操作結果和處理統計</returns>
        Task<AnalyticsServiceResult> GenerateLeaderboardSnapshotAsync(string period, DateTime? ts = null);

        /// <summary>
        /// 取得排行榜快照
        /// 前台提供日/週榜查詢
        /// </summary>
        /// <param name="queryDto">排行榜查詢請求</param>
        /// <returns>排行榜快照列表</returns>
        Task<List<LeaderboardSnapshotDto>> GetLeaderboardSnapshotAsync(LeaderboardQueryDto queryDto);

        /// <summary>
        /// 取得排行榜統計
        /// 計算排行榜的統計指標
        /// </summary>
        /// <param name="period">期間類型</param>
        /// <param name="date">統計日期</param>
        /// <returns>排行榜統計資訊</returns>
        Task<LeaderboardStatsDto> GetLeaderboardStatsAsync(string period, DateTime? date = null);

        /// <summary>
        /// 回補排行榜快照
        /// 補充遺漏的歷史快照數據
        /// </summary>
        /// <param name="period">期間類型</param>
        /// <param name="startDate">開始日期</param>
        /// <param name="endDate">結束日期</param>
        /// <returns>操作結果和處理統計</returns>
        Task<AnalyticsServiceResult> BackfillLeaderboardSnapshotsAsync(string period, DateTime startDate, DateTime endDate);

        #endregion

        #region 洞察貼文管理

        /// <summary>
        /// 取得洞察貼文列表
        /// 支援狀態篩選和分頁
        /// </summary>
        /// <param name="gameId">遊戲ID篩選</param>
        /// <param name="status">狀態篩選</param>
        /// <param name="pinnedOnly">是否只顯示置頂</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <returns>分頁洞察貼文列表</returns>
        Task<AnalyticsPagedResult<InsightPostDto>> GetInsightPostsAsync(
            int? gameId = null, string? status = null, bool pinnedOnly = false,
            int page = 1, int pageSize = 20);

        /// <summary>
        /// 取得洞察貼文詳細資訊
        /// 包含指標快照和引用來源
        /// </summary>
        /// <param name="postId">貼文ID</param>
        /// <returns>洞察貼文詳細資訊</returns>
        Task<InsightPostDto?> GetInsightPostDetailAsync(int postId);

        /// <summary>
        /// 建立洞察貼文
        /// 發佈時快照當前指標數據
        /// </summary>
        /// <param name="createDto">建立洞察貼文請求</param>
        /// <returns>操作結果和貼文資訊</returns>
        Task<AnalyticsServiceResult<InsightPostDto>> CreateInsightPostAsync(CreateInsightPostDto createDto);

        /// <summary>
        /// 發佈洞察貼文
        /// 快照 post_metric_snapshot、記錄 post_sources
        /// </summary>
        /// <param name="postId">貼文ID</param>
        /// <returns>操作結果</returns>
        Task<AnalyticsServiceResult> PublishInsightPostAsync(int postId);

        /// <summary>
        /// 更新洞察貼文狀態
        /// 支援草稿、發佈、隱藏等狀態轉換
        /// </summary>
        /// <param name="postId">貼文ID</param>
        /// <param name="status">新狀態</param>
        /// <returns>操作結果</returns>
        Task<AnalyticsServiceResult> UpdateInsightPostStatusAsync(int postId, string status);

        /// <summary>
        /// 設定洞察貼文置頂
        /// 管理員限定功能
        /// </summary>
        /// <param name="postId">貼文ID</param>
        /// <param name="isPinned">是否置頂</param>
        /// <returns>操作結果</returns>
        Task<AnalyticsServiceResult> SetInsightPostPinnedAsync(int postId, bool isPinned);

        #endregion

        #region 數據導入與清洗

        /// <summary>
        /// 導入外部數據
        /// 從配置的API端點獲取數據並清洗入庫
        /// </summary>
        /// <param name="sourceId">來源ID</param>
        /// <param name="date">導入日期</param>
        /// <returns>操作結果和處理統計</returns>
        Task<AnalyticsServiceResult> ImportExternalDataAsync(int sourceId, DateTime date);

        /// <summary>
        /// 批量導入歷史數據
        /// 用於初始化或補充歷史數據
        /// </summary>
        /// <param name="sourceId">來源ID</param>
        /// <param name="startDate">開始日期</param>
        /// <param name="endDate">結束日期</param>
        /// <returns>操作結果和處理統計</returns>
        Task<AnalyticsServiceResult> BatchImportHistoricalDataAsync(int sourceId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// 數據品質檢查
        /// 檢查數據完整性和異常值
        /// </summary>
        /// <param name="date">檢查日期</param>
        /// <returns>數據品質報告</returns>
        Task<AnalyticsServiceResult> DataQualityCheckAsync(DateTime date);

        #endregion

        #region 自動化任務

        /// <summary>
        /// 每日數據處理任務
        /// 自動執行數據導入、指數計算、快照產製
        /// </summary>
        /// <param name="date">處理日期</param>
        /// <returns>任務執行結果</returns>
        Task<AnalyticsServiceResult> DailyDataProcessingTaskAsync(DateTime? date = null);

        /// <summary>
        /// 週期性快照生成任務
        /// 定期產製週榜和月榜快照
        /// </summary>
        /// <param name="period">期間類型</param>
        /// <returns>任務執行結果</returns>
        Task<AnalyticsServiceResult> PeriodicSnapshotGenerationTaskAsync(string period);

        /// <summary>
        /// 數據清理任務
        /// 清理過期的原始數據和臨時文件
        /// </summary>
        /// <param name="retentionDays">保留天數</param>
        /// <returns>清理結果</returns>
        Task<AnalyticsServiceResult> DataCleanupTaskAsync(int retentionDays = 90);

        #endregion

        #region 統計分析

        /// <summary>
        /// 取得分析儀表板統計
        /// 計算綜合統計指標用於儀表板展示
        /// </summary>
        /// <param name="date">統計日期</param>
        /// <returns>儀表板統計資訊</returns>
        Task<AnalyticsDashboardDto> GetAnalyticsDashboardAsync(DateTime? date = null);

        /// <summary>
        /// 取得遊戲表現報告
        /// 分析特定遊戲的各項指標表現
        /// </summary>
        /// <param name="gameId">遊戲ID</param>
        /// <param name="startDate">開始日期</param>
        /// <param name="endDate">結束日期</param>
        /// <returns>遊戲表現報告</returns>
        Task<GamePerformanceReportDto> GetGamePerformanceReportAsync(int gameId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// 取得指標相關性分析
        /// 分析不同指標之間的相關性
        /// </summary>
        /// <param name="metricIds">指標ID列表</param>
        /// <param name="startDate">開始日期</param>
        /// <param name="endDate">結束日期</param>
        /// <returns>相關性分析結果</returns>
        Task<MetricCorrelationAnalysisDto> GetMetricCorrelationAnalysisAsync(List<int> metricIds, DateTime startDate, DateTime endDate);

        /// <summary>
        /// 取得趨勢預測
        /// 基於歷史數據預測未來趨勢
        /// </summary>
        /// <param name="gameId">遊戲ID</param>
        /// <param name="metricId">指標ID</param>
        /// <param name="predictionDays">預測天數</param>
        /// <returns>趨勢預測結果</returns>
        Task<TrendPredictionDto> GetTrendPredictionAsync(int gameId, int metricId, int predictionDays = 7);

        #endregion

        #region 配置管理

        /// <summary>
        /// 取得指標權重配置
        /// 管理熱度指數計算的權重設定
        /// </summary>
        /// <returns>指標權重配置</returns>
        Task<Dictionary<int, decimal>> GetMetricWeightsAsync();

        /// <summary>
        /// 更新指標權重配置
        /// 管理員限定功能
        /// </summary>
        /// <param name="weights">指標權重字典</param>
        /// <returns>操作結果</returns>
        Task<AnalyticsServiceResult> UpdateMetricWeightsAsync(Dictionary<int, decimal> weights);

        /// <summary>
        /// 取得計算參數配置
        /// 管理指數計算和排行榜生成的參數
        /// </summary>
        /// <returns>計算參數配置</returns>
        Task<AnalyticsConfigDto> GetAnalyticsConfigAsync();

        /// <summary>
        /// 更新計算參數配置
        /// 管理員限定功能
        /// </summary>
        /// <param name="config">計算參數配置</param>
        /// <returns>操作結果</returns>
        Task<AnalyticsServiceResult> UpdateAnalyticsConfigAsync(AnalyticsConfigDto config);

        #endregion
    }

    #region 輔助類別

    /// <summary>
    /// 遊戲表現報告 DTO
    /// </summary>
    public class GamePerformanceReportDto
    {
        /// <summary>遊戲ID</summary>
        public int GameId { get; set; }

        /// <summary>遊戲名稱</summary>
        public string? GameName { get; set; }

        /// <summary>報告期間</summary>
        public DateTime StartDate { get; set; }

        /// <summary>報告期間</summary>
        public DateTime EndDate { get; set; }

        /// <summary>平均熱度指數</summary>
        public decimal AveragePopularityIndex { get; set; }

        /// <summary>最高熱度指數</summary>
        public decimal MaxPopularityIndex { get; set; }

        /// <summary>最低熱度指數</summary>
        public decimal MinPopularityIndex { get; set; }

        /// <summary>熱度指數趨勢</summary>
        public string PopularityTrend { get; set; } = string.Empty;

        /// <summary>排名表現</summary>
        public List<LeaderboardSnapshotDto> RankingHistory { get; set; } = new();

        /// <summary>指標表現明細</summary>
        public Dictionary<string, MetricPerformanceDto> MetricPerformances { get; set; } = new();

        /// <summary>關鍵事件</summary>
        public List<KeyEventDto> KeyEvents { get; set; } = new();
    }

    /// <summary>
    /// 指標表現 DTO
    /// </summary>
    public class MetricPerformanceDto
    {
        /// <summary>指標名稱</summary>
        public string? MetricName { get; set; }

        /// <summary>平均值</summary>
        public decimal AverageValue { get; set; }

        /// <summary>增長率</summary>
        public decimal GrowthRate { get; set; }

        /// <summary>趨勢</summary>
        public string Trend { get; set; } = string.Empty;

        /// <summary>數據點</summary>
        public List<TimeSeriesDataPointDto> DataPoints { get; set; } = new();
    }

    /// <summary>
    /// 關鍵事件 DTO
    /// </summary>
    public class KeyEventDto
    {
        /// <summary>事件日期</summary>
        public DateTime Date { get; set; }

        /// <summary>事件類型</summary>
        public string? EventType { get; set; }

        /// <summary>事件描述</summary>
        public string? Description { get; set; }

        /// <summary>影響程度</summary>
        public decimal Impact { get; set; }
    }

    /// <summary>
    /// 指標相關性分析 DTO
    /// </summary>
    public class MetricCorrelationAnalysisDto
    {
        /// <summary>分析期間</summary>
        public DateTime StartDate { get; set; }

        /// <summary>分析期間</summary>
        public DateTime EndDate { get; set; }

        /// <summary>相關性矩陣</summary>
        public Dictionary<string, Dictionary<string, decimal>> CorrelationMatrix { get; set; } = new();

        /// <summary>強相關指標對</summary>
        public List<MetricCorrelationPairDto> StrongCorrelations { get; set; } = new();
    }

    /// <summary>
    /// 指標相關性對 DTO
    /// </summary>
    public class MetricCorrelationPairDto
    {
        /// <summary>指標1名稱</summary>
        public string? Metric1Name { get; set; }

        /// <summary>指標2名稱</summary>
        public string? Metric2Name { get; set; }

        /// <summary>相關係數</summary>
        public decimal CorrelationCoefficient { get; set; }

        /// <summary>相關性強度</summary>
        public string CorrelationStrength { get; set; } = string.Empty;
    }

    /// <summary>
    /// 趨勢預測 DTO
    /// </summary>
    public class TrendPredictionDto
    {
        /// <summary>遊戲ID</summary>
        public int GameId { get; set; }

        /// <summary>指標ID</summary>
        public int MetricId { get; set; }

        /// <summary>預測日期</summary>
        public DateTime PredictionDate { get; set; }

        /// <summary>預測值</summary>
        public List<TimeSeriesDataPointDto> PredictedValues { get; set; } = new();

        /// <summary>置信區間</summary>
        public List<ConfidenceIntervalDto> ConfidenceIntervals { get; set; } = new();

        /// <summary>預測準確度</summary>
        public decimal Accuracy { get; set; }

        /// <summary>使用的模型</summary>
        public string? Model { get; set; }
    }

    /// <summary>
    /// 置信區間 DTO
    /// </summary>
    public class ConfidenceIntervalDto
    {
        /// <summary>時間戳</summary>
        public DateTime Timestamp { get; set; }

        /// <summary>下限</summary>
        public decimal LowerBound { get; set; }

        /// <summary>上限</summary>
        public decimal UpperBound { get; set; }

        /// <summary>置信度</summary>
        public decimal Confidence { get; set; }
    }

    /// <summary>
    /// 分析配置 DTO
    /// </summary>
    public class AnalyticsConfigDto
    {
        /// <summary>熱度指數計算方法</summary>
        public string IndexCalculationMethod { get; set; } = "weighted_average";

        /// <summary>排行榜更新頻率</summary>
        public string LeaderboardUpdateFrequency { get; set; } = "daily";

        /// <summary>數據保留天數</summary>
        public int DataRetentionDays { get; set; } = 365;

        /// <summary>異常值檢測閾值</summary>
        public decimal OutlierDetectionThreshold { get; set; } = 2.0m;

        /// <summary>預測模型參數</summary>
        public Dictionary<string, object> PredictionModelParams { get; set; } = new();

        /// <summary>自動化任務開關</summary>
        public Dictionary<string, bool> AutomationSwitches { get; set; } = new();
    }

    #endregion
}