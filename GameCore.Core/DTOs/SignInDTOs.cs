using System.ComponentModel.DataAnnotations;

namespace GameCore.Core.DTOs
{
    /// <summary>
    /// 簽到結果 DTO
    /// </summary>
    public class SignInResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int PointsGained { get; set; }
        public int ExpGained { get; set; }
        public int CurrentStreak { get; set; }
        public int TotalSignIns { get; set; }
        public DateTime SignInTime { get; set; }
        public bool IsConsecutive { get; set; }
        public bool IsWeeklyBonus { get; set; }
        public bool IsMonthlyBonus { get; set; }
        public Dictionary<string, object> BonusInfo { get; set; } = new();
    }

    /// <summary>
    /// 簽到狀態結果 DTO
    /// </summary>
    public class SignInStatusResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool HasSignedToday { get; set; }
        public DateTime? LastSignInDate { get; set; }
        public int CurrentStreak { get; set; }
        public int TotalSignIns { get; set; }
        public int PointsToday { get; set; }
        public int ExpToday { get; set; }
        public bool CanSignIn { get; set; }
        public DateTime? NextSignInTime { get; set; }
        public SignInRewardsDto TodayRewards { get; set; } = new();
        public SignInRewardsDto TomorrowRewards { get; set; } = new();
    }

    /// <summary>
    /// 簽到記錄 DTO
    /// </summary>
    public class SignInRecordDto
    {
        public int LogId { get; set; }
        public int UserId { get; set; }
        public DateTime SignTime { get; set; }
        public int PointsChanged { get; set; }
        public int ExpGained { get; set; }
        public DateTime PointsChangedTime { get; set; }
        public DateTime ExpGainedTime { get; set; }
        public bool IsWeekend { get; set; }
        public bool IsConsecutiveBonus { get; set; }
        public bool IsMonthlyBonus { get; set; }
        public string SignInType { get; set; } = string.Empty;
    }

    /// <summary>
    /// 簽到統計 DTO
    /// </summary>
    public class SignInStatisticsDto
    {
        public int UserId { get; set; }
        public int TotalSignIns { get; set; }
        public int CurrentStreak { get; set; }
        public int LongestStreak { get; set; }
        public int ThisMonthSignIns { get; set; }
        public int ThisYearSignIns { get; set; }
        public int TotalPointsFromSignIn { get; set; }
        public int TotalExpFromSignIn { get; set; }
        public DateTime? FirstSignInDate { get; set; }
        public DateTime? LastSignInDate { get; set; }
        public double AveragePointsPerSignIn { get; set; }
        public double AverageExpPerSignIn { get; set; }
        public int WeekendSignIns { get; set; }
        public int WeekdaySignIns { get; set; }
        public int ConsecutiveBonusCount { get; set; }
        public int MonthlyBonusCount { get; set; }
    }

    /// <summary>
    /// 簽到日曆 DTO
    /// </summary>
    public class SignInCalendarDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public List<SignInCalendarDayDto> Days { get; set; } = new();
        public int TotalSignIns { get; set; }
        public int PossibleSignIns { get; set; }
        public double SignInRate { get; set; }
        public bool HasMonthlyBonus { get; set; }
        public SignInRewardsDto MonthlyBonusRewards { get; set; } = new();
    }

    /// <summary>
    /// 簽到日曆日期 DTO
    /// </summary>
    public class SignInCalendarDayDto
    {
        public DateTime Date { get; set; }
        public bool HasSignedIn { get; set; }
        public int PointsGained { get; set; }
        public int ExpGained { get; set; }
        public bool IsWeekend { get; set; }
        public bool IsConsecutiveBonus { get; set; }
        public bool IsMonthlyBonus { get; set; }
        public bool IsToday { get; set; }
        public bool CanSignIn { get; set; }
        public SignInRewardsDto Rewards { get; set; } = new();
    }

    /// <summary>
    /// 簽到獎勵 DTO
    /// </summary>
    public class SignInRewardsDto
    {
        public int BasePoints { get; set; }
        public int BaseExp { get; set; }
        public int BonusPoints { get; set; }
        public int BonusExp { get; set; }
        public int TotalPoints { get; set; }
        public int TotalExp { get; set; }
        public bool IsWeekend { get; set; }
        public bool IsConsecutiveBonus { get; set; }
        public bool IsMonthlyBonus { get; set; }
        public List<string> BonusReasons { get; set; } = new();
        public Dictionary<string, object> ExtraRewards { get; set; } = new();
    }

    /// <summary>
    /// 簽到排行榜 DTO
    /// </summary>
    public class SignInLeaderboardDto
    {
        public List<SignInLeaderboardItemDto> TopStreaks { get; set; } = new();
        public List<SignInLeaderboardItemDto> TopMonthly { get; set; } = new();
        public List<SignInLeaderboardItemDto> TopYearly { get; set; } = new();
        public SignInLeaderboardItemDto? CurrentUserRank { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    /// <summary>
    /// 簽到排行榜項目 DTO
    /// </summary>
    public class SignInLeaderboardItemDto
    {
        public int Rank { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string NickName { get; set; } = string.Empty;
        public int Value { get; set; }
        public string Category { get; set; } = string.Empty;
        public DateTime LastSignIn { get; set; }
        public bool IsCurrentUser { get; set; }
    }
}