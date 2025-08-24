using System.ComponentModel.DataAnnotations;

namespace GameCore.Core.DTOs
{
    #region 遊戲管理 DTOs

    /// <summary>
    /// 遊戲資格檢查結果
    /// </summary>
    public class MiniGameEligibilityDto
    {
        /// <summary>是否可以開始遊戲</summary>
        public bool CanPlay { get; set; }

        /// <summary>檢查結果訊息</summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>今日已玩次數</summary>
        public int TodayPlayCount { get; set; }

        /// <summary>每日最大次數</summary>
        public int DailyLimit { get; set; }

        /// <summary>剩餘次數</summary>
        public int RemainingPlays => Math.Max(0, DailyLimit - TodayPlayCount);

        /// <summary>寵物是否健康可玩</summary>
        public bool PetHealthy { get; set; }

        /// <summary>阻止遊戲的原因列表</summary>
        public List<string> BlockingReasons { get; set; } = new();

        /// <summary>建議操作列表</summary>
        public List<string> SuggestedActions { get; set; } = new();

        /// <summary>下次可玩時間 (如果今日次數已滿)</summary>
        public DateTime? NextPlayTime { get; set; }
    }

    /// <summary>
    /// 遊戲會話資訊
    /// </summary>
    public class MiniGameSessionDto
    {
        /// <summary>遊戲ID</summary>
        public int GameId { get; set; }

        /// <summary>使用者ID</summary>
        public int UserId { get; set; }

        /// <summary>關卡等級</summary>
        public int Level { get; set; }

        /// <summary>怪物數量</summary>
        public int MonsterCount { get; set; }

        /// <summary>速度倍率</summary>
        public decimal SpeedMultiplier { get; set; }

        /// <summary>預期獎勵 (勝利時)</summary>
        public GameRewardDto ExpectedReward { get; set; } = new();

        /// <summary>遊戲開始時間</summary>
        public DateTime StartTime { get; set; }

        /// <summary>會話狀態</summary>
        public string Status { get; set; } = "進行中";

        /// <summary>遊戲提示</summary>
        public string GameTips { get; set; } = string.Empty;
    }

    /// <summary>
    /// 完成遊戲請求
    /// </summary>
    public class FinishGameDto
    {
        /// <summary>遊戲結果 (true=勝利, false=失敗)</summary>
        [Required]
        public bool IsVictory { get; set; }

        /// <summary>實際遊戲時長 (秒)</summary>
        [Range(1, 3600)]
        public int DurationSeconds { get; set; }

        /// <summary>擊敗的怪物數量</summary>
        [Range(0, int.MaxValue)]
        public int MonstersDefeated { get; set; }

        /// <summary>最終分數</summary>
        [Range(0, int.MaxValue)]
        public int FinalScore { get; set; }

        /// <summary>遊戲過程備註</summary>
        public string? GameNotes { get; set; }
    }

    /// <summary>
    /// 遊戲結算結果
    /// </summary>
    public class MiniGameResultDto
    {
        /// <summary>遊戲ID</summary>
        public int GameId { get; set; }

        /// <summary>遊戲結果</summary>
        public bool IsVictory { get; set; }

        /// <summary>結果描述</summary>
        public string ResultMessage { get; set; } = string.Empty;

        /// <summary>獲得的獎勵</summary>
        public GameRewardDto Reward { get; set; } = new();

        /// <summary>寵物屬性變化</summary>
        public PetStatsChangeDto PetStatsChange { get; set; } = new();

        /// <summary>寵物更新結果</summary>
        public PetStatsUpdateResultDto PetUpdateResult { get; set; } = new();

        /// <summary>下一關等級</summary>
        public int NextLevel { get; set; }

        /// <summary>是否可以繼續遊戲</summary>
        public bool CanContinue { get; set; }

        /// <summary>遊戲時長</summary>
        public TimeSpan Duration { get; set; }

        /// <summary>完成時間</summary>
        public DateTime FinishTime { get; set; }

        /// <summary>今日剩餘次數</summary>
        public int RemainingPlaysToday { get; set; }
    }

    #endregion

    #region 遊戲記錄與統計 DTOs

    /// <summary>
    /// 遊戲記錄查詢請求
    /// </summary>
    public class GetGameRecordsDto
    {
        /// <summary>開始日期</summary>
        public DateTime? FromDate { get; set; }

        /// <summary>結束日期</summary>
        public DateTime? ToDate { get; set; }

        /// <summary>遊戲結果篩選 (null=全部, true=勝利, false=失敗)</summary>
        public bool? IsVictory { get; set; }

        /// <summary>關卡等級篩選</summary>
        public int? Level { get; set; }

        /// <summary>頁碼</summary>
        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;

        /// <summary>每頁筆數</summary>
        [Range(1, 100)]
        public int PageSize { get; set; } = 20;

        /// <summary>排序欄位</summary>
        public string SortBy { get; set; } = "StartTime";

        /// <summary>排序方向 (asc/desc)</summary>
        public string SortDirection { get; set; } = "desc";
    }

    /// <summary>
    /// 遊戲記錄
    /// </summary>
    public class MiniGameRecordDto
    {
        /// <summary>遊戲ID</summary>
        public int GameId { get; set; }

        /// <summary>關卡等級</summary>
        public int Level { get; set; }

        /// <summary>怪物數量</summary>
        public int MonsterCount { get; set; }

        /// <summary>速度倍率</summary>
        public decimal SpeedMultiplier { get; set; }

        /// <summary>遊戲結果</summary>
        public bool? IsVictory { get; set; }

        /// <summary>結果文字</summary>
        public string ResultText => IsVictory switch
        {
            true => "勝利",
            false => "失敗", 
            null => "中斷"
        };

        /// <summary>擊敗怪物數</summary>
        public int MonstersDefeated { get; set; }

        /// <summary>最終分數</summary>
        public int FinalScore { get; set; }

        /// <summary>獲得經驗</summary>
        public int ExpGained { get; set; }

        /// <summary>獲得點數</summary>
        public int PointsGained { get; set; }

        /// <summary>寵物屬性變化</summary>
        public PetStatsChangeDto StatsChange { get; set; } = new();

        /// <summary>開始時間</summary>
        public DateTime StartTime { get; set; }

        /// <summary>結束時間</summary>
        public DateTime? EndTime { get; set; }

        /// <summary>遊戲時長</summary>
        public TimeSpan? Duration => EndTime?.Subtract(StartTime);

        /// <summary>是否中斷</summary>
        public bool IsAborted { get; set; }

        /// <summary>中斷原因</summary>
        public string? AbortReason { get; set; }
    }

    /// <summary>
    /// 遊戲統計資訊
    /// </summary>
    public class MiniGameStatsDto
    {
        /// <summary>總遊戲次數</summary>
        public int TotalGames { get; set; }

        /// <summary>總勝利次數</summary>
        public int TotalVictories { get; set; }

        /// <summary>總失敗次數</summary>
        public int TotalDefeats { get; set; }

        /// <summary>總中斷次數</summary>
        public int TotalAborts { get; set; }

        /// <summary>勝率</summary>
        public double WinRate => TotalGames > 0 ? (double)TotalVictories / TotalGames * 100 : 0;

        /// <summary>最高關卡</summary>
        public int HighestLevel { get; set; }

        /// <summary>目前關卡</summary>
        public int CurrentLevel { get; set; }

        /// <summary>總遊戲時間</summary>
        public TimeSpan TotalPlayTime { get; set; }

        /// <summary>平均遊戲時間</summary>
        public TimeSpan AveragePlayTime => TotalGames > 0 ? TimeSpan.FromTicks(TotalPlayTime.Ticks / TotalGames) : TimeSpan.Zero;

        /// <summary>總獲得經驗</summary>
        public int TotalExperienceEarned { get; set; }

        /// <summary>總獲得點數</summary>
        public int TotalPointsEarned { get; set; }

        /// <summary>最佳分數</summary>
        public int BestScore { get; set; }

        /// <summary>平均分數</summary>
        public double AverageScore { get; set; }

        /// <summary>連勝記錄</summary>
        public int LongestWinStreak { get; set; }

        /// <summary>目前連勝</summary>
        public int CurrentWinStreak { get; set; }

        /// <summary>首次遊戲時間</summary>
        public DateTime? FirstGameTime { get; set; }

        /// <summary>最後遊戲時間</summary>
        public DateTime? LastGameTime { get; set; }

        /// <summary>各關卡統計</summary>
        public List<LevelStatsDto> LevelStats { get; set; } = new();
    }

    /// <summary>
    /// 關卡統計
    /// </summary>
    public class LevelStatsDto
    {
        /// <summary>關卡等級</summary>
        public int Level { get; set; }

        /// <summary>嘗試次數</summary>
        public int Attempts { get; set; }

        /// <summary>勝利次數</summary>
        public int Victories { get; set; }

        /// <summary>通過率</summary>
        public double PassRate => Attempts > 0 ? (double)Victories / Attempts * 100 : 0;

        /// <summary>最佳分數</summary>
        public int BestScore { get; set; }

        /// <summary>最快完成時間</summary>
        public TimeSpan? BestTime { get; set; }
    }

    /// <summary>
    /// 當日遊戲狀態
    /// </summary>
    public class DailyGameStatusDto
    {
        /// <summary>查詢日期</summary>
        public DateTime Date { get; set; }

        /// <summary>今日已玩次數</summary>
        public int TodayPlayCount { get; set; }

        /// <summary>每日限制次數</summary>
        public int DailyLimit { get; set; }

        /// <summary>剩餘次數</summary>
        public int RemainingPlays => Math.Max(0, DailyLimit - TodayPlayCount);

        /// <summary>是否可以遊戲</summary>
        public bool CanPlay { get; set; }

        /// <summary>今日遊戲記錄</summary>
        public List<MiniGameRecordDto> TodayGames { get; set; } = new();

        /// <summary>今日獲得總經驗</summary>
        public int TodayTotalExp { get; set; }

        /// <summary>今日獲得總點數</summary>
        public int TodayTotalPoints { get; set; }

        /// <summary>今日勝利次數</summary>
        public int TodayVictories { get; set; }

        /// <summary>下次重置時間</summary>
        public DateTime NextResetTime { get; set; }
    }

    #endregion

    #region 關卡設定與獎勵 DTOs

    /// <summary>
    /// 關卡設定
    /// </summary>
    public class GameLevelConfigDto
    {
        /// <summary>關卡等級</summary>
        public int Level { get; set; }

        /// <summary>怪物數量</summary>
        public int MonsterCount { get; set; }

        /// <summary>速度倍率</summary>
        public decimal SpeedMultiplier { get; set; }

        /// <summary>速度描述</summary>
        public string SpeedDescription { get; set; } = string.Empty;

        /// <summary>勝利獎勵</summary>
        public GameRewardDto VictoryReward { get; set; } = new();

        /// <summary>失敗獎勵</summary>
        public GameRewardDto DefeatReward { get; set; } = new();

        /// <summary>關卡描述</summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>關卡提示</summary>
        public string Tips { get; set; } = string.Empty;

        /// <summary>是否已解鎖</summary>
        public bool IsUnlocked { get; set; }

        /// <summary>解鎖條件</summary>
        public string UnlockCondition { get; set; } = string.Empty;
    }

    /// <summary>
    /// 遊戲獎勵
    /// </summary>
    public class GameRewardDto
    {
        /// <summary>經驗值</summary>
        public int Experience { get; set; }

        /// <summary>點數</summary>
        public int Points { get; set; }

        /// <summary>獎勵描述</summary>
        public string Description => $"+{Experience} 經驗, +{Points} 點數";

        /// <summary>額外獎勵說明</summary>
        public string? BonusDescription { get; set; }

        /// <summary>是否有額外獎勵</summary>
        public bool HasBonus => !string.IsNullOrEmpty(BonusDescription);
    }

    /// <summary>
    /// 寵物屬性變化
    /// </summary>
    public class PetStatsChangeDto
    {
        /// <summary>飢餓值變化</summary>
        public int HungerChange { get; set; }

        /// <summary>心情值變化</summary>
        public int MoodChange { get; set; }

        /// <summary>體力值變化</summary>
        public int StaminaChange { get; set; }

        /// <summary>清潔值變化</summary>
        public int CleanlinessChange { get; set; }

        /// <summary>健康度變化</summary>
        public int HealthChange { get; set; }

        /// <summary>變化摘要</summary>
        public string Summary
        {
            get
            {
                var changes = new List<string>();
                if (HungerChange != 0) changes.Add($"飢餓 {(HungerChange > 0 ? "+" : "")}{HungerChange}");
                if (MoodChange != 0) changes.Add($"心情 {(MoodChange > 0 ? "+" : "")}{MoodChange}");
                if (StaminaChange != 0) changes.Add($"體力 {(StaminaChange > 0 ? "+" : "")}{StaminaChange}");
                if (CleanlinessChange != 0) changes.Add($"清潔 {(CleanlinessChange > 0 ? "+" : "")}{CleanlinessChange}");
                if (HealthChange != 0) changes.Add($"健康 {(HealthChange > 0 ? "+" : "")}{HealthChange}");
                return string.Join(", ", changes);
            }
        }

        /// <summary>是否有負面影響</summary>
        public bool HasNegativeEffects => HungerChange < 0 || MoodChange < 0 || StaminaChange < 0 || CleanlinessChange < 0 || HealthChange < 0;

        /// <summary>是否有正面影響</summary>
        public bool HasPositiveEffects => HungerChange > 0 || MoodChange > 0 || StaminaChange > 0 || CleanlinessChange > 0 || HealthChange > 0;
    }

    /// <summary>
    /// 寵物屬性更新結果
    /// </summary>
    public class PetStatsUpdateResultDto
    {
        /// <summary>更新前屬性</summary>
        public PetStatsSnapshot BeforeStats { get; set; } = new();

        /// <summary>更新後屬性</summary>
        public PetStatsSnapshot AfterStats { get; set; } = new();

        /// <summary>是否升級</summary>
        public bool LeveledUp { get; set; }

        /// <summary>升級資訊</summary>
        public PetLevelUpInfo? LevelUpInfo { get; set; }

        /// <summary>健康警告</summary>
        public List<string> HealthWarnings { get; set; } = new();

        /// <summary>是否觸發健康檢查</summary>
        public bool HealthCheckTriggered { get; set; }

        /// <summary>是否可以繼續冒險</summary>
        public bool CanContinueAdventure { get; set; }

        /// <summary>建議操作</summary>
        public List<string> SuggestedActions { get; set; } = new();
    }

    #endregion

    #region 每日重置與維護 DTOs

    /// <summary>
    /// 每日重置結果
    /// </summary>
    public class DailyResetResultDto
    {
        /// <summary>重置日期</summary>
        public DateTime ResetDate { get; set; }

        /// <summary>重置成功</summary>
        public bool Success { get; set; }

        /// <summary>重置訊息</summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>影響的使用者數</summary>
        public int AffectedUsers { get; set; }

        /// <summary>重置的遊戲次數總計</summary>
        public int TotalGamesReset { get; set; }

        /// <summary>寵物衰減處理數</summary>
        public int PetsProcessed { get; set; }

        /// <summary>重置開始時間</summary>
        public DateTime StartTime { get; set; }

        /// <summary>重置結束時間</summary>
        public DateTime EndTime { get; set; }

        /// <summary>處理時間</summary>
        public TimeSpan ProcessingTime => EndTime - StartTime;

        /// <summary>錯誤訊息</summary>
        public List<string> Errors { get; set; } = new();

        /// <summary>警告訊息</summary>
        public List<string> Warnings { get; set; } = new();
    }

    /// <summary>
    /// 每日重置統計
    /// </summary>
    public class DailyResetStatsDto
    {
        /// <summary>重置日期</summary>
        public DateTime Date { get; set; }

        /// <summary>重置狀態</summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>處理時間</summary>
        public TimeSpan ProcessingTime { get; set; }

        /// <summary>影響使用者數</summary>
        public int AffectedUsers { get; set; }

        /// <summary>處理的寵物數</summary>
        public int PetsProcessed { get; set; }

        /// <summary>重置執行時間</summary>
        public DateTime ExecutionTime { get; set; }

        /// <summary>是否有錯誤</summary>
        public bool HasErrors { get; set; }

        /// <summary>錯誤數量</summary>
        public int ErrorCount { get; set; }
    }

    #endregion

    #region 管理員功能 DTOs

    /// <summary>
    /// 遊戲系統設定
    /// </summary>
    public class MiniGameSystemConfigDto
    {
        /// <summary>每日遊戲次數限制</summary>
        [Range(1, 10)]
        public int DailyPlayLimit { get; set; } = 3;

        /// <summary>是否啟用每日重置</summary>
        public bool EnableDailyReset { get; set; } = true;

        /// <summary>關卡設定</summary>
        public List<GameLevelConfigDto> LevelConfigs { get; set; } = new();

        /// <summary>寵物屬性影響設定</summary>
        public PetStatsEffectConfigDto StatsEffectConfig { get; set; } = new();

        /// <summary>系統維護模式</summary>
        public bool MaintenanceMode { get; set; } = false;

        /// <summary>維護訊息</summary>
        public string? MaintenanceMessage { get; set; }

        /// <summary>最後更新時間</summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>更新者</summary>
        public string? UpdatedBy { get; set; }
    }

    /// <summary>
    /// 寵物屬性影響設定
    /// </summary>
    public class PetStatsEffectConfigDto
    {
        /// <summary>勝利時屬性變化</summary>
        public PetStatsChangeDto VictoryEffects { get; set; } = new()
        {
            HungerChange = -20,
            MoodChange = 30,
            StaminaChange = -20,
            CleanlinessChange = -20
        };

        /// <summary>失敗時屬性變化</summary>
        public PetStatsChangeDto DefeatEffects { get; set; } = new()
        {
            HungerChange = -20,
            MoodChange = -30,
            StaminaChange = -20,
            CleanlinessChange = -20
        };

        /// <summary>是否啟用健康檢查</summary>
        public bool EnableHealthCheck { get; set; } = true;

        /// <summary>最低健康度要求</summary>
        public int MinHealthRequired { get; set; } = 1;
    }

    /// <summary>
    /// 管理員遊戲記錄查詢
    /// </summary>
    public class AdminGameRecordsQueryDto : GetGameRecordsDto
    {
        /// <summary>使用者ID篩選</summary>
        public int? UserId { get; set; }

        /// <summary>使用者名稱篩選</summary>
        public string? Username { get; set; }

        /// <summary>是否包含中斷記錄</summary>
        public bool IncludeAborted { get; set; } = true;

        /// <summary>最小分數篩選</summary>
        public int? MinScore { get; set; }

        /// <summary>最大分數篩選</summary>
        public int? MaxScore { get; set; }
    }

    /// <summary>
    /// 管理員遊戲記錄
    /// </summary>
    public class AdminMiniGameRecordDto : MiniGameRecordDto
    {
        /// <summary>使用者名稱</summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>使用者電子郵件</summary>
        public string UserEmail { get; set; } = string.Empty;

        /// <summary>IP位址</summary>
        public string? IpAddress { get; set; }

        /// <summary>使用者代理</summary>
        public string? UserAgent { get; set; }

        /// <summary>是否可疑記錄</summary>
        public bool IsSuspicious { get; set; }

        /// <summary>可疑原因</summary>
        public List<string> SuspiciousReasons { get; set; } = new();
    }

    #endregion

    #region 排行榜與統計 DTOs

    /// <summary>
    /// 遊戲排行榜
    /// </summary>
    public class MiniGameRankingDto
    {
        /// <summary>排名</summary>
        public int Rank { get; set; }

        /// <summary>使用者ID</summary>
        public int UserId { get; set; }

        /// <summary>使用者名稱</summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>排行值</summary>
        public long RankingValue { get; set; }

        /// <summary>排行描述</summary>
        public string RankingDescription { get; set; } = string.Empty;

        /// <summary>排行類型</summary>
        public GameRankingType RankingType { get; set; }

        /// <summary>最後更新時間</summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>額外資訊</summary>
        public Dictionary<string, object> ExtraInfo { get; set; } = new();
    }

    /// <summary>
    /// 遊戲整體統計
    /// </summary>
    public class MiniGameOverallStatsDto
    {
        /// <summary>總使用者數</summary>
        public int TotalPlayers { get; set; }

        /// <summary>活躍使用者數 (7天內)</summary>
        public int ActivePlayers7Days { get; set; }

        /// <summary>總遊戲次數</summary>
        public long TotalGames { get; set; }

        /// <summary>總勝利次數</summary>
        public long TotalVictories { get; set; }

        /// <summary>整體勝率</summary>
        public double OverallWinRate => TotalGames > 0 ? (double)TotalVictories / TotalGames * 100 : 0;

        /// <summary>總遊戲時間</summary>
        public TimeSpan TotalPlayTime { get; set; }

        /// <summary>平均遊戲時間</summary>
        public TimeSpan AverageGameTime => TotalGames > 0 ? TimeSpan.FromTicks(TotalPlayTime.Ticks / TotalGames) : TimeSpan.Zero;

        /// <summary>今日遊戲次數</summary>
        public int TodayGames { get; set; }

        /// <summary>本週遊戲次數</summary>
        public int WeekGames { get; set; }

        /// <summary>本月遊戲次數</summary>
        public int MonthGames { get; set; }

        /// <summary>各關卡通過率</summary>
        public List<LevelPassRateDto> LevelPassRates { get; set; } = new();

        /// <summary>每日遊戲趨勢 (近30天)</summary>
        public List<DailyGameTrendDto> DailyTrends { get; set; } = new();
    }

    /// <summary>
    /// 關卡通過率
    /// </summary>
    public class LevelPassRateDto
    {
        /// <summary>關卡等級</summary>
        public int Level { get; set; }

        /// <summary>總嘗試次數</summary>
        public int TotalAttempts { get; set; }

        /// <summary>成功次數</summary>
        public int SuccessCount { get; set; }

        /// <summary>通過率</summary>
        public double PassRate => TotalAttempts > 0 ? (double)SuccessCount / TotalAttempts * 100 : 0;

        /// <summary>平均分數</summary>
        public double AverageScore { get; set; }

        /// <summary>最高分數</summary>
        public int HighestScore { get; set; }

        /// <summary>平均完成時間</summary>
        public TimeSpan AverageCompletionTime { get; set; }

        /// <summary>最快完成時間</summary>
        public TimeSpan FastestCompletionTime { get; set; }
    }

    /// <summary>
    /// 每日遊戲趨勢
    /// </summary>
    public class DailyGameTrendDto
    {
        /// <summary>日期</summary>
        public DateTime Date { get; set; }

        /// <summary>遊戲次數</summary>
        public int GameCount { get; set; }

        /// <summary>獨特玩家數</summary>
        public int UniquePlayerCount { get; set; }

        /// <summary>勝利次數</summary>
        public int VictoryCount { get; set; }

        /// <summary>當日勝率</summary>
        public double WinRate => GameCount > 0 ? (double)VictoryCount / GameCount * 100 : 0;

        /// <summary>平均遊戲時長</summary>
        public TimeSpan AverageGameDuration { get; set; }
    }

    #endregion

    #region 分頁結果

    /// <summary>
    /// 分頁結果
    /// </summary>
    /// <typeparam name="T">資料類型</typeparam>
    public class PagedResult<T>
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

        /// <summary>是否為空結果</summary>
        public bool IsEmpty => !Data.Any();
    }

    #endregion
}