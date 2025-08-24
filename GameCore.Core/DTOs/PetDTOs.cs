using System.ComponentModel.DataAnnotations;

namespace GameCore.Core.DTOs
{
    #region 寵物基本 DTOs

    /// <summary>
    /// 寵物完整資訊 DTO
    /// </summary>
    public class PetDto
    {
        /// <summary>寵物ID</summary>
        public int PetId { get; set; }

        /// <summary>寵物主人ID</summary>
        public int UserId { get; set; }

        /// <summary>寵物名稱</summary>
        public string PetName { get; set; } = string.Empty;

        /// <summary>寵物等級 (1-250)</summary>
        public int Level { get; set; }

        /// <summary>最後升級時間</summary>
        public DateTime LevelUpTime { get; set; }

        /// <summary>總經驗值</summary>
        public int Experience { get; set; }

        /// <summary>下一級所需經驗</summary>
        public int RequiredExperienceForNextLevel { get; set; }

        /// <summary>升級進度百分比</summary>
        public double LevelProgress { get; set; }

        /// <summary>飢餓值 (0-100)</summary>
        public int Hunger { get; set; }

        /// <summary>心情值 (0-100)</summary>
        public int Mood { get; set; }

        /// <summary>體力值 (0-100)</summary>
        public int Stamina { get; set; }

        /// <summary>清潔值 (0-100)</summary>
        public int Cleanliness { get; set; }

        /// <summary>健康度 (0-100)</summary>
        public int Health { get; set; }

        /// <summary>5維屬性總分</summary>
        public int TotalStats => Hunger + Mood + Stamina + Cleanliness + Health;

        /// <summary>膚色 (十六進位)</summary>
        public string SkinColor { get; set; } = "#ADD8E6";

        /// <summary>背景色</summary>
        public string BackgroundColor { get; set; } = "粉藍";

        /// <summary>最後換色時間</summary>
        public DateTime ColorChangedTime { get; set; }

        /// <summary>最後換色花費的點數</summary>
        public int LastColorChangePoints { get; set; }

        /// <summary>最後換色花費時間</summary>
        public DateTime PointsChangedTime { get; set; }

        /// <summary>是否可以進行冒險</summary>
        public bool CanAdventure { get; set; }

        /// <summary>健康狀態描述</summary>
        public string HealthStatus { get; set; } = string.Empty;

        /// <summary>需要關注的屬性</summary>
        public List<string> LowStatsWarnings { get; set; } = new();

        /// <summary>寵物狀態 (Happy, Sad, Tired, Dirty, Sick等)</summary>
        public string PetStatus { get; set; } = string.Empty;
    }

    /// <summary>
    /// 更新寵物資料請求 DTO
    /// </summary>
    public class UpdatePetProfileDto
    {
        /// <summary>新的寵物名稱</summary>
        [Required(ErrorMessage = "寵物名稱不可為空")]
        [StringLength(50, ErrorMessage = "寵物名稱不可超過50字")]
        public string PetName { get; set; } = string.Empty;

        /// <summary>更新原因 (選填)</summary>
        [StringLength(100, ErrorMessage = "更新原因不可超過100字")]
        public string? UpdateReason { get; set; }
    }

    #endregion

    #region 寵物互動 DTOs

    /// <summary>
    /// 寵物互動結果 DTO
    /// </summary>
    public class PetInteractionResultDto
    {
        /// <summary>互動是否成功</summary>
        public bool Success { get; set; }

        /// <summary>互動訊息</summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>互動類型 (feed, bathe, play, rest)</summary>
        public string InteractionType { get; set; } = string.Empty;

        /// <summary>互動前的寵物狀態</summary>
        public PetStatsSnapshot BeforeStats { get; set; } = new();

        /// <summary>互動後的寵物狀態</summary>
        public PetStatsSnapshot AfterStats { get; set; } = new();

        /// <summary>屬性變化</summary>
        public PetStatsChange StatsChange { get; set; } = new();

        /// <summary>是否觸發升級</summary>
        public bool LeveledUp { get; set; }

        /// <summary>升級資訊 (如果有)</summary>
        public PetLevelUpInfo? LevelUpInfo { get; set; }

        /// <summary>是否觸發健康警告</summary>
        public bool HealthWarning { get; set; }

        /// <summary>健康警告訊息</summary>
        public List<string> HealthWarnings { get; set; } = new();

        /// <summary>是否達到完美狀態 (所有屬性100)</summary>
        public bool PerfectCondition { get; set; }

        /// <summary>互動時間</summary>
        public DateTime InteractionTime { get; set; }

        /// <summary>獲得的經驗值 (如果有)</summary>
        public int ExperienceGained { get; set; }
    }

    /// <summary>
    /// 寵物屬性快照
    /// </summary>
    public class PetStatsSnapshot
    {
        public int Hunger { get; set; }
        public int Mood { get; set; }
        public int Stamina { get; set; }
        public int Cleanliness { get; set; }
        public int Health { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }
    }

    /// <summary>
    /// 寵物屬性變化
    /// </summary>
    public class PetStatsChange
    {
        public int HungerChange { get; set; }
        public int MoodChange { get; set; }
        public int StaminaChange { get; set; }
        public int CleanlinessChange { get; set; }
        public int HealthChange { get; set; }
        public int ExperienceChange { get; set; }
    }

    /// <summary>
    /// 寵物升級資訊
    /// </summary>
    public class PetLevelUpInfo
    {
        public int OldLevel { get; set; }
        public int NewLevel { get; set; }
        public int PointsReward { get; set; }
        public string UpgradeMessage { get; set; } = string.Empty;
    }

    #endregion

    #region 寵物顏色系統 DTOs

    /// <summary>
    /// 寵物顏色選項 DTO
    /// </summary>
    public class PetColorOptionDto
    {
        /// <summary>顏色ID</summary>
        public string ColorId { get; set; } = string.Empty;

        /// <summary>顏色名稱</summary>
        public string ColorName { get; set; } = string.Empty;

        /// <summary>膚色值 (十六進位)</summary>
        public string SkinColor { get; set; } = string.Empty;

        /// <summary>背景色值</summary>
        public string BackgroundColor { get; set; } = string.Empty;

        /// <summary>是否為預設顏色</summary>
        public bool IsDefault { get; set; }

        /// <summary>是否為特殊顏色 (需要更高等級)</summary>
        public bool IsSpecial { get; set; }

        /// <summary>解鎖等級需求</summary>
        public int RequiredLevel { get; set; }

        /// <summary>預覽圖片URL</summary>
        public string PreviewImage { get; set; } = string.Empty;
    }

    /// <summary>
    /// 寵物換色請求 DTO
    /// </summary>
    public class PetRecolorDto
    {
        /// <summary>新的膚色 (十六進位)</summary>
        [Required(ErrorMessage = "膚色不可為空")]
        [RegularExpression(@"^#[0-9A-Fa-f]{6}$", ErrorMessage = "膚色格式錯誤")]
        public string SkinColor { get; set; } = string.Empty;

        /// <summary>新的背景色</summary>
        [Required(ErrorMessage = "背景色不可為空")]
        [StringLength(50, ErrorMessage = "背景色名稱不可超過50字")]
        public string BackgroundColor { get; set; } = string.Empty;

        /// <summary>換色原因 (選填)</summary>
        [StringLength(100, ErrorMessage = "換色原因不可超過100字")]
        public string? Reason { get; set; }

        /// <summary>是否確認扣除2000點數</summary>
        public bool ConfirmPayment { get; set; }
    }

    /// <summary>
    /// 寵物換色歷史 DTO
    /// </summary>
    public class PetColorHistoryDto
    {
        /// <summary>換色時間</summary>
        public DateTime ChangeTime { get; set; }

        /// <summary>舊膚色</summary>
        public string OldSkinColor { get; set; } = string.Empty;

        /// <summary>新膚色</summary>
        public string NewSkinColor { get; set; } = string.Empty;

        /// <summary>舊背景色</summary>
        public string OldBackgroundColor { get; set; } = string.Empty;

        /// <summary>新背景色</summary>
        public string NewBackgroundColor { get; set; } = string.Empty;

        /// <summary>花費點數</summary>
        public int PointsCost { get; set; }

        /// <summary>換色原因</summary>
        public string Reason { get; set; } = string.Empty;
    }

    #endregion

    #region 寵物等級與經驗 DTOs

    /// <summary>
    /// 寵物經驗增加結果 DTO
    /// </summary>
    public class PetExperienceResultDto
    {
        /// <summary>是否成功</summary>
        public bool Success { get; set; }

        /// <summary>結果訊息</summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>增加的經驗值</summary>
        public int ExperienceGained { get; set; }

        /// <summary>經驗來源</summary>
        public string Source { get; set; } = string.Empty;

        /// <summary>增加前的經驗值</summary>
        public int OldExperience { get; set; }

        /// <summary>增加後的經驗值</summary>
        public int NewExperience { get; set; }

        /// <summary>增加前的等級</summary>
        public int OldLevel { get; set; }

        /// <summary>增加後的等級</summary>
        public int NewLevel { get; set; }

        /// <summary>是否升級</summary>
        public bool LeveledUp { get; set; }

        /// <summary>升級次數 (可能一次升多級)</summary>
        public int LevelsGained { get; set; }

        /// <summary>升級獎勵點數</summary>
        public int BonusPoints { get; set; }

        /// <summary>升級訊息</summary>
        public List<string> LevelUpMessages { get; set; } = new();
    }

    /// <summary>
    /// 寵物等級統計 DTO
    /// </summary>
    public class PetLevelStatsDto
    {
        /// <summary>當前等級</summary>
        public int CurrentLevel { get; set; }

        /// <summary>當前經驗值</summary>
        public int CurrentExperience { get; set; }

        /// <summary>當前等級所需經驗值</summary>
        public int CurrentLevelRequiredExp { get; set; }

        /// <summary>下一級所需經驗值</summary>
        public int NextLevelRequiredExp { get; set; }

        /// <summary>距離下一級還需要的經驗值</summary>
        public int ExperienceToNextLevel { get; set; }

        /// <summary>當前等級進度百分比</summary>
        public double LevelProgress { get; set; }

        /// <summary>總升級次數</summary>
        public int TotalLevelUps { get; set; }

        /// <summary>總獲得經驗值</summary>
        public int TotalExperienceEarned { get; set; }

        /// <summary>升級獲得的總點數</summary>
        public int TotalBonusPoints { get; set; }

        /// <summary>是否已達最高等級</summary>
        public bool IsMaxLevel { get; set; }

        /// <summary>等級排名 (在所有寵物中)</summary>
        public int LevelRanking { get; set; }

        /// <summary>最後升級時間</summary>
        public DateTime LastLevelUpTime { get; set; }
    }

    #endregion

    #region 每日維護 DTOs

    /// <summary>
    /// 寵物每日衰減結果 DTO
    /// </summary>
    public class PetDailyDecayResultDto
    {
        /// <summary>處理是否成功</summary>
        public bool Success { get; set; }

        /// <summary>處理訊息</summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>處理的寵物數量</summary>
        public int ProcessedPetsCount { get; set; }

        /// <summary>處理時間</summary>
        public DateTime ProcessTime { get; set; }

        /// <summary>衰減詳細資訊</summary>
        public List<PetDecayDetailDto> DecayDetails { get; set; } = new();

        /// <summary>處理錯誤 (如果有)</summary>
        public List<string> Errors { get; set; } = new();
    }

    /// <summary>
    /// 寵物衰減詳細資訊 DTO
    /// </summary>
    public class PetDecayDetailDto
    {
        /// <summary>寵物ID</summary>
        public int PetId { get; set; }

        /// <summary>寵物主人ID</summary>
        public int UserId { get; set; }

        /// <summary>寵物名稱</summary>
        public string PetName { get; set; } = string.Empty;

        /// <summary>衰減前狀態</summary>
        public PetStatsSnapshot BeforeDecay { get; set; } = new();

        /// <summary>衰減後狀態</summary>
        public PetStatsSnapshot AfterDecay { get; set; } = new();

        /// <summary>衰減變化</summary>
        public PetStatsChange DecayChange { get; set; } = new();

        /// <summary>是否觸發健康警告</summary>
        public bool HealthWarning { get; set; }

        /// <summary>健康警告訊息</summary>
        public List<string> HealthWarnings { get; set; } = new();
    }

    /// <summary>
    /// 寵物冒險準備度檢查 DTO
    /// </summary>
    public class PetAdventureReadinessDto
    {
        /// <summary>是否可以冒險</summary>
        public bool CanAdventure { get; set; }

        /// <summary>檢查訊息</summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>當前健康度</summary>
        public int CurrentHealth { get; set; }

        /// <summary>當前5維屬性</summary>
        public PetStatsSnapshot CurrentStats { get; set; } = new();

        /// <summary>阻擋冒險的原因</summary>
        public List<string> BlockingReasons { get; set; } = new();

        /// <summary>建議的改善行動</summary>
        public List<string> SuggestedActions { get; set; } = new();

        /// <summary>預估恢復到可冒險狀態的時間</summary>
        public TimeSpan? EstimatedRecoveryTime { get; set; }
    }

    #endregion

    #region 寵物統計與排行 DTOs

    /// <summary>
    /// 寵物統計 DTO
    /// </summary>
    public class PetStatsDto
    {
        /// <summary>寵物基本資訊</summary>
        public PetDto Pet { get; set; } = new();

        /// <summary>總互動次數</summary>
        public int TotalInteractions { get; set; }

        /// <summary>餵食次數</summary>
        public int FeedCount { get; set; }

        /// <summary>洗澡次數</summary>
        public int BatheCount { get; set; }

        /// <summary>玩耍次數</summary>
        public int PlayCount { get; set; }

        /// <summary>休息次數</summary>
        public int RestCount { get; set; }

        /// <summary>換色次數</summary>
        public int RecolorCount { get; set; }

        /// <summary>總花費點數 (換色)</summary>
        public int TotalPointsSpent { get; set; }

        /// <summary>平均每日互動次數</summary>
        public double AverageInteractionsPerDay { get; set; }

        /// <summary>寵物年齡 (天數)</summary>
        public int PetAge { get; set; }

        /// <summary>最喜歡的互動類型</summary>
        public string FavoriteInteraction { get; set; } = string.Empty;

        /// <summary>健康度歷史最高值</summary>
        public int HighestHealth { get; set; }

        /// <summary>最長連續健康天數</summary>
        public int LongestHealthyStreak { get; set; }

        /// <summary>建立時間</summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>最後互動時間</summary>
        public DateTime? LastInteractionTime { get; set; }
    }

    /// <summary>
    /// 寵物排行榜 DTO
    /// </summary>
    public class PetRankingDto
    {
        /// <summary>排名</summary>
        public int Rank { get; set; }

        /// <summary>寵物ID</summary>
        public int PetId { get; set; }

        /// <summary>寵物名稱</summary>
        public string PetName { get; set; } = string.Empty;

        /// <summary>主人名稱</summary>
        public string OwnerName { get; set; } = string.Empty;

        /// <summary>排行值 (等級、經驗、健康度等)</summary>
        public int RankingValue { get; set; }

        /// <summary>排行類型</summary>
        public PetRankingType RankingType { get; set; }

        /// <summary>寵物等級</summary>
        public int Level { get; set; }

        /// <summary>寵物總經驗</summary>
        public int TotalExperience { get; set; }

        /// <summary>寵物健康度</summary>
        public int Health { get; set; }

        /// <summary>5維屬性總分</summary>
        public int TotalStats { get; set; }

        /// <summary>膚色</summary>
        public string SkinColor { get; set; } = string.Empty;

        /// <summary>背景色</summary>
        public string BackgroundColor { get; set; } = string.Empty;

        /// <summary>寵物建立時間</summary>
        public DateTime CreatedTime { get; set; }
    }

    /// <summary>
    /// 寵物排行類型
    /// </summary>
    public enum PetRankingType
    {
        Level,          // 等級排行
        Experience,     // 經驗排行
        Health,         // 健康度排行
        TotalStats,     // 總屬性排行
        Age            // 年齡排行
    }

    #endregion

    #region 管理員功能 DTOs

    /// <summary>
    /// 管理員寵物重置請求 DTO
    /// </summary>
    public class PetAdminResetDto
    {
        /// <summary>重置類型 (stats, level, all)</summary>
        [Required(ErrorMessage = "重置類型必填")]
        public string ResetType { get; set; } = string.Empty;

        /// <summary>重置原因</summary>
        [Required(ErrorMessage = "重置原因必填")]
        [StringLength(200, ErrorMessage = "重置原因不可超過200字")]
        public string Reason { get; set; } = string.Empty;

        /// <summary>是否發送通知給使用者</summary>
        public bool SendNotification { get; set; } = true;

        /// <summary>自訂屬性值 (如果重置類型是custom)</summary>
        public PetStatsSnapshot? CustomStats { get; set; }
    }

    /// <summary>
    /// 寵物系統設定 DTO
    /// </summary>
    public class PetSystemConfigDto
    {
        /// <summary>換色點數成本</summary>
        public int RecolorCost { get; set; } = 2000;

        /// <summary>每日衰減值設定</summary>
        public PetDailyDecayConfig DailyDecay { get; set; } = new();

        /// <summary>互動增益值設定</summary>
        public PetInteractionConfig InteractionGains { get; set; } = new();

        /// <summary>健康檢查設定</summary>
        public PetHealthConfig HealthConfig { get; set; } = new();

        /// <summary>升級獎勵設定</summary>
        public PetLevelConfig LevelConfig { get; set; } = new();

        /// <summary>是否開啟互動冷卻</summary>
        public bool EnableInteractionCooldown { get; set; } = false;

        /// <summary>互動冷卻時間 (秒)</summary>
        public int InteractionCooldownSeconds { get; set; } = 0;

        /// <summary>最大等級限制</summary>
        public int MaxLevel { get; set; } = 250;

        /// <summary>是否開啟自動每日衰減</summary>
        public bool EnableAutoDailyDecay { get; set; } = true;
    }

    /// <summary>
    /// 每日衰減設定
    /// </summary>
    public class PetDailyDecayConfig
    {
        public int HungerDecay { get; set; } = 20;
        public int MoodDecay { get; set; } = 30;
        public int StaminaDecay { get; set; } = 10;
        public int CleanlinessDecay { get; set; } = 20;
        public int HealthDecay { get; set; } = 20;
    }

    /// <summary>
    /// 互動增益設定
    /// </summary>
    public class PetInteractionConfig
    {
        public int FeedGain { get; set; } = 10;
        public int BatheGain { get; set; } = 10;
        public int PlayGain { get; set; } = 10;
        public int RestGain { get; set; } = 10;
        public int InteractionExperience { get; set; } = 5; // 每次互動獲得的經驗
    }

    /// <summary>
    /// 健康檢查設定
    /// </summary>
    public class PetHealthConfig
    {
        public int LowStatThreshold { get; set; } = 30; // 低屬性閾值
        public int HealthPenalty { get; set; } = 20;    // 健康度懲罰
        public int MinAdventureHealth { get; set; } = 1; // 最低冒險健康度
    }

    /// <summary>
    /// 升級獎勵設定
    /// </summary>
    public class PetLevelConfig
    {
        public int BaseUpgradeReward { get; set; } = 0; // 基礎升級獎勵點數
        public bool EnableUpgradeRewards { get; set; } = false; // 是否啟用升級獎勵
    }

    #endregion

    #region 通用 DTOs

    /// <summary>
    /// 服務結果泛型 DTO
    /// </summary>
    /// <typeparam name="T">資料類型</typeparam>
    public class ServiceResult<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string> Errors { get; set; } = new();
    }

    /// <summary>
    /// 服務結果 DTO (無資料)
    /// </summary>
    public class ServiceResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new();
    }

    #endregion
}