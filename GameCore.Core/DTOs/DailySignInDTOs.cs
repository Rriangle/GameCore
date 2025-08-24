using System.ComponentModel.DataAnnotations;

namespace GameCore.Core.DTOs
{
    #region 簽到狀態 DTOs

    /// <summary>
    /// 簽到狀態 DTO - 顯示使用者當前簽到狀態和統計資訊
    /// </summary>
    public class SignInStatusDto
    {
        /// <summary>使用者ID</summary>
        public int UserId { get; set; }

        /// <summary>今日是否已簽到</summary>
        public bool TodaySigned { get; set; }

        /// <summary>當前連續簽到天數</summary>
        public int CurrentStreak { get; set; }

        /// <summary>Asia/Taipei 時區的當前日期</summary>
        public DateTime TaipeiDate { get; set; }

        /// <summary>Asia/Taipei 時區的當前時間</summary>
        public DateTime TaipeiDateTime { get; set; }

        /// <summary>今日是否為週末</summary>
        public bool IsWeekend { get; set; }

        /// <summary>本月簽到統計</summary>
        public MonthlyAttendanceDto MonthAttendance { get; set; } = new();

        /// <summary>今日潛在獲得獎勵 (如果簽到)</summary>
        public SignInRewards? TodayPotentialRewards { get; set; }

        /// <summary>今日是否可以簽到</summary>
        public bool CanSignToday { get; set; }
    }

    /// <summary>
    /// 簽到獎勵資訊
    /// </summary>
    public class SignInRewards
    {
        /// <summary>可獲得點數</summary>
        public int Points { get; set; }

        /// <summary>可獲得經驗</summary>
        public int Experience { get; set; }

        /// <summary>是否有連續獎勵</summary>
        public bool HasStreakBonus { get; set; }

        /// <summary>是否有月度獎勵</summary>
        public bool HasMonthlyBonus { get; set; }

        /// <summary>獎勵說明</summary>
        public List<string> BonusDescriptions { get; set; } = new();
    }

    #endregion

    #region 簽到結果 DTOs

    /// <summary>
    /// 簽到結果 DTO - 簽到執行後的回應資訊
    /// </summary>
    public class SignInResultDto
    {
        /// <summary>簽到是否成功</summary>
        public bool Success { get; set; }

        /// <summary>結果訊息</summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>獲得的點數</summary>
        public int PointsEarned { get; set; }

        /// <summary>獲得的經驗值</summary>
        public int ExperienceGained { get; set; }

        /// <summary>簽到前連續天數</summary>
        public int StreakBefore { get; set; }

        /// <summary>簽到後連續天數</summary>
        public int StreakAfter { get; set; }

        /// <summary>是否為週末簽到</summary>
        public bool IsWeekend { get; set; }

        /// <summary>是否獲得7天連續獎勵</summary>
        public bool HasSevenDayBonus { get; set; }

        /// <summary>是否獲得月度全勤獎勵</summary>
        public bool HasMonthlyBonus { get; set; }

        /// <summary>獎勵訊息列表</summary>
        public List<string> BonusMessages { get; set; } = new();

        /// <summary>簽到時間 (Asia/Taipei)</summary>
        public DateTime SignInTime { get; set; }
    }

    #endregion

    #region 月度統計 DTOs

    /// <summary>
    /// 月度簽到統計 DTO
    /// </summary>
    public class MonthlyAttendanceDto
    {
        /// <summary>年份</summary>
        public int Year { get; set; }

        /// <summary>月份</summary>
        public int Month { get; set; }

        /// <summary>已簽到的日期列表</summary>
        public List<DateTime> SignedDays { get; set; } = new();

        /// <summary>總簽到天數</summary>
        public int TotalSignedDays { get; set; }

        /// <summary>本月總天數</summary>
        public int TotalDaysInMonth { get; set; }

        /// <summary>出席率 (百分比)</summary>
        public double AttendanceRate { get; set; }

        /// <summary>本月總獲得點數</summary>
        public int TotalPointsEarned { get; set; }

        /// <summary>本月總獲得經驗</summary>
        public int TotalExperienceGained { get; set; }

        /// <summary>是否為全勤月</summary>
        public bool IsPerfectAttendance { get; set; }

        /// <summary>本月內連續簽到天數</summary>
        public int CurrentMonthStreak { get; set; }

        /// <summary>簽到記錄詳細列表</summary>
        public List<SignInRecordDto> Records { get; set; } = new();
    }

    #endregion

    #region 簽到記錄 DTOs

    /// <summary>
    /// 簽到記錄 DTO
    /// </summary>
    public class SignInRecordDto
    {
        /// <summary>記錄ID</summary>
        public int LogId { get; set; }

        /// <summary>簽到時間 (Asia/Taipei)</summary>
        public DateTime SignTime { get; set; }

        /// <summary>獲得點數</summary>
        public int PointsChanged { get; set; }

        /// <summary>獲得經驗</summary>
        public int ExpGained { get; set; }

        /// <summary>是否為週末</summary>
        public bool IsWeekend { get; set; }

        /// <summary>星期幾</summary>
        public string DayOfWeek { get; set; } = string.Empty;

        /// <summary>特殊獎勵說明</summary>
        public List<string> SpecialBonuses { get; set; } = new();
    }

    /// <summary>
    /// 簽到歷史查詢條件 DTO
    /// </summary>
    public class SignInHistoryQueryDto
    {
        /// <summary>查詢開始日期 (Asia/Taipei)</summary>
        public DateTime? FromDate { get; set; }

        /// <summary>查詢結束日期 (Asia/Taipei)</summary>
        public DateTime? ToDate { get; set; }

        /// <summary>頁碼 (預設 1)</summary>
        public int Page { get; set; } = 1;

        /// <summary>每頁筆數 (預設 20)</summary>
        public int PageSize { get; set; } = 20;
    }

    #endregion

    #region 簽到日曆 DTOs

    /// <summary>
    /// 簽到日曆 DTO - 用於前端日曆顯示
    /// </summary>
    public class SignInCalendarDto
    {
        /// <summary>年份</summary>
        public int Year { get; set; }

        /// <summary>月份</summary>
        public int Month { get; set; }

        /// <summary>日曆天數資訊</summary>
        public List<CalendarDayDto> Days { get; set; } = new();

        /// <summary>月度統計摘要</summary>
        public MonthlyAttendanceDto Summary { get; set; } = new();
    }

    /// <summary>
    /// 日曆天數 DTO
    /// </summary>
    public class CalendarDayDto
    {
        /// <summary>日期</summary>
        public DateTime Date { get; set; }

        /// <summary>日期數字</summary>
        public int Day { get; set; }

        /// <summary>是否已簽到</summary>
        public bool IsSignedIn { get; set; }

        /// <summary>是否為今天</summary>
        public bool IsToday { get; set; }

        /// <summary>是否為週末</summary>
        public bool IsWeekend { get; set; }

        /// <summary>是否為本月</summary>
        public bool IsCurrentMonth { get; set; }

        /// <summary>簽到記錄 (如果有)</summary>
        public SignInRecordDto? SignInRecord { get; set; }

        /// <summary>特殊標記 (連續獎勵、月度獎勵等)</summary>
        public List<string> SpecialMarkers { get; set; } = new();
    }

    #endregion

    #region 簽到統計 DTOs

    /// <summary>
    /// 簽到總統計 DTO
    /// </summary>
    public class SignInSummaryDto
    {
        /// <summary>使用者ID</summary>
        public int UserId { get; set; }

        /// <summary>總簽到天數</summary>
        public int TotalSignInDays { get; set; }

        /// <summary>當前最長連續簽到</summary>
        public int CurrentLongestStreak { get; set; }

        /// <summary>歷史最長連續簽到</summary>
        public int HistoryLongestStreak { get; set; }

        /// <summary>總獲得點數</summary>
        public int TotalPointsEarned { get; set; }

        /// <summary>總獲得經驗</summary>
        public int TotalExperienceEarned { get; set; }

        /// <summary>連續7天獎勵次數</summary>
        public int SevenDayBonusCount { get; set; }

        /// <summary>月度全勤次數</summary>
        public int MonthlyPerfectCount { get; set; }

        /// <summary>平均每日獲得點數</summary>
        public double AveragePointsPerDay { get; set; }

        /// <summary>簽到開始日期</summary>
        public DateTime? FirstSignInDate { get; set; }

        /// <summary>最後簽到日期</summary>
        public DateTime? LastSignInDate { get; set; }

        /// <summary>本月簽到統計</summary>
        public MonthlyAttendanceDto CurrentMonth { get; set; } = new();

        /// <summary>過去6個月統計</summary>
        public List<MonthlyAttendanceDto> RecentMonths { get; set; } = new();
    }

    #endregion

    #region 管理員功能 DTOs

    /// <summary>
    /// 管理員簽到調整請求 DTO
    /// </summary>
    public class AdminSignInAdjustmentDto
    {
        /// <summary>目標使用者ID</summary>
        [Required(ErrorMessage = "使用者ID必填")]
        public int UserId { get; set; }

        /// <summary>調整日期 (Asia/Taipei)</summary>
        [Required(ErrorMessage = "調整日期必填")]
        public DateTime AdjustmentDate { get; set; }

        /// <summary>調整類型 (add=補簽, remove=移除)</summary>
        [Required(ErrorMessage = "調整類型必填")]
        public string AdjustmentType { get; set; } = string.Empty;

        /// <summary>調整原因</summary>
        [Required(ErrorMessage = "調整原因必填")]
        [StringLength(200, ErrorMessage = "調整原因不可超過200字")]
        public string Reason { get; set; } = string.Empty;

        /// <summary>是否發送通知給使用者</summary>
        public bool SendNotification { get; set; } = true;
    }

    #endregion
}