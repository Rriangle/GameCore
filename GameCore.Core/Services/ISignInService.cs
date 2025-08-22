using GameCore.Core.Entities;

namespace GameCore.Core.Services
{
    /// <summary>
    /// 簽到服務介面
    /// 定義每日簽到系統的所有業務邏輯
    /// </summary>
    public interface ISignInService
    {
        /// <summary>
        /// 執行每日簽到
        /// </summary>
        /// <param name="userId">使用者編號</param>
        /// <returns>簽到結果</returns>
        Task<SignInResult> SignInAsync(int userId);

        /// <summary>
        /// 檢查今日是否已簽到
        /// </summary>
        /// <param name="userId">使用者編號</param>
        /// <returns>是否已簽到</returns>
        Task<bool> HasSignedInTodayAsync(int userId);

        /// <summary>
        /// 取得使用者連續簽到天數
        /// </summary>
        /// <param name="userId">使用者編號</param>
        /// <returns>連續簽到天數</returns>
        Task<int> GetConsecutiveDaysAsync(int userId);

        /// <summary>
        /// 取得當月簽到統計
        /// </summary>
        /// <param name="userId">使用者編號</param>
        /// <param name="year">年份</param>
        /// <param name="month">月份</param>
        /// <returns>當月簽到統計</returns>
        Task<MonthlySignInStats> GetMonthlyStatsAsync(int userId, int year, int month);

        /// <summary>
        /// 檢查是否為當月全勤 (用於發放全勤獎勵)
        /// </summary>
        /// <param name="userId">使用者編號</param>
        /// <param name="year">年份</param>
        /// <param name="month">月份</param>
        /// <returns>是否為當月全勤</returns>
        Task<bool> IsMonthlyPerfectAttendanceAsync(int userId, int year, int month);

        /// <summary>
        /// 取得簽到獎勵配置 (平日/假日/連續/全勤)
        /// </summary>
        /// <param name="signInDate">簽到日期</param>
        /// <param name="consecutiveDays">連續天數</param>
        /// <param name="isMonthlyPerfect">是否當月全勤</param>
        /// <returns>獎勵配置</returns>
        SignInRewardConfig CalculateSignInReward(DateTime signInDate, int consecutiveDays, bool isMonthlyPerfect);

        /// <summary>
        /// 取得使用者簽到歷史 (分頁)
        /// </summary>
        /// <param name="userId">使用者編號</param>
        /// <param name="page">頁數</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <returns>簽到歷史</returns>
        Task<SignInHistoryResult> GetSignInHistoryAsync(int userId, int page = 1, int pageSize = 30);
    }

    /// <summary>
    /// 簽到結果
    /// </summary>
    public class SignInResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 結果訊息
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 獲得的點數
        /// </summary>
        public int PointsEarned { get; set; }

        /// <summary>
        /// 獲得的經驗值
        /// </summary>
        public int ExperienceEarned { get; set; }

        /// <summary>
        /// 連續簽到天數
        /// </summary>
        public int ConsecutiveDays { get; set; }

        /// <summary>
        /// 是否觸發連續 7 天獎勵
        /// </summary>
        public bool IsSevenDayBonus { get; set; }

        /// <summary>
        /// 是否觸發當月全勤獎勵
        /// </summary>
        public bool IsMonthlyPerfectBonus { get; set; }

        /// <summary>
        /// 簽到記錄
        /// </summary>
        public UserSignInStats? SignInRecord { get; set; }

        /// <summary>
        /// 更新後的使用者點數
        /// </summary>
        public int TotalUserPoints { get; set; }
    }

    /// <summary>
    /// 簽到獎勵配置
    /// </summary>
    public class SignInRewardConfig
    {
        /// <summary>
        /// 基礎點數獎勵
        /// </summary>
        public int BasePoints { get; set; }

        /// <summary>
        /// 基礎經驗獎勵
        /// </summary>
        public int BaseExperience { get; set; }

        /// <summary>
        /// 連續 7 天額外點數
        /// </summary>
        public int SevenDayBonusPoints { get; set; }

        /// <summary>
        /// 連續 7 天額外經驗
        /// </summary>
        public int SevenDayBonusExperience { get; set; }

        /// <summary>
        /// 當月全勤額外點數
        /// </summary>
        public int MonthlyPerfectBonusPoints { get; set; }

        /// <summary>
        /// 當月全勤額外經驗
        /// </summary>
        public int MonthlyPerfectBonusExperience { get; set; }

        /// <summary>
        /// 是否為假日
        /// </summary>
        public bool IsWeekend { get; set; }

        /// <summary>
        /// 總點數 (包含所有獎勵)
        /// </summary>
        public int TotalPoints => BasePoints + SevenDayBonusPoints + MonthlyPerfectBonusPoints;

        /// <summary>
        /// 總經驗 (包含所有獎勵)
        /// </summary>
        public int TotalExperience => BaseExperience + SevenDayBonusExperience + MonthlyPerfectBonusExperience;
    }

    /// <summary>
    /// 當月簽到統計
    /// </summary>
    public class MonthlySignInStats
    {
        /// <summary>
        /// 年份
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 月份
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        /// 總簽到天數
        /// </summary>
        public int TotalSignInDays { get; set; }

        /// <summary>
        /// 當月總天數
        /// </summary>
        public int TotalDaysInMonth { get; set; }

        /// <summary>
        /// 是否全勤
        /// </summary>
        public bool IsPerfectAttendance { get; set; }

        /// <summary>
        /// 簽到日期列表
        /// </summary>
        public List<DateTime> SignInDates { get; set; } = new List<DateTime>();

        /// <summary>
        /// 當月獲得總點數
        /// </summary>
        public int TotalPointsEarned { get; set; }

        /// <summary>
        /// 當月獲得總經驗
        /// </summary>
        public int TotalExperienceEarned { get; set; }
    }

    /// <summary>
    /// 簽到歷史結果
    /// </summary>
    public class SignInHistoryResult
    {
        /// <summary>
        /// 簽到記錄列表
        /// </summary>
        public List<UserSignInStats> Records { get; set; } = new List<UserSignInStats>();

        /// <summary>
        /// 總記錄數
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 當前頁數
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// 每頁筆數
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 總頁數
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// 是否有下一頁
        /// </summary>
        public bool HasNextPage { get; set; }

        /// <summary>
        /// 是否有上一頁
        /// </summary>
        public bool HasPreviousPage { get; set; }
    }
}