using GameCore.Core.DTOs;
using GameCore.Core.Entities;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace GameCore.Core.Services
{
    /// <summary>
    /// 每日簽到服務實作 - 完整實現Asia/Taipei時區簽到系統
    /// 包含連續簽到獎勵、週末加成、當月全勤獎勵等完整功能
    /// 嚴格按照規格要求實現所有簽到邏輯和獎勵計算
    /// </summary>
    public class DailySignInService : IDailySignInService
    {
        private readonly GameCoreDbContext _context;
        private readonly ILogger<DailySignInService> _logger;
        private readonly IWalletService _walletService;
        
        // Asia/Taipei 時區 (UTC+8)
        private static readonly TimeZoneInfo TaipeiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Taipei");

        public DailySignInService(
            GameCoreDbContext context,
            ILogger<DailySignInService> logger,
            IWalletService walletService)
        {
            _context = context;
            _logger = logger;
            _walletService = walletService;
        }

        #region 簽到狀態查詢

        /// <summary>
        /// 取得使用者今日簽到狀態和統計資訊
        /// </summary>
        public async Task<SignInStatusDto> GetSignInStatusAsync(int userId)
        {
            try
            {
                _logger.LogInformation($"取得使用者 {userId} 的簽到狀態");

                var now = DateTime.UtcNow;
                var taipeiNow = TimeZoneInfo.ConvertTimeFromUtc(now, TaipeiTimeZone);
                var todayTaipei = taipeiNow.Date;

                // 檢查今日是否已簽到
                var todaySigned = await HasSignedTodayAsync(userId, todayTaipei);

                // 計算當前連續簽到天數
                var currentStreak = await CalculateCurrentStreakAsync(userId);

                // 取得本月簽到情況
                var monthAttendance = await GetMonthAttendanceAsync(userId, taipeiNow.Year, taipeiNow.Month);

                // 取得今日可獲得的獎勵 (如果還未簽到)
                var todayRewards = todaySigned ? null : await CalculateTodayRewardsAsync(userId, taipeiNow);

                return new SignInStatusDto
                {
                    UserId = userId,
                    TodaySigned = todaySigned,
                    CurrentStreak = currentStreak,
                    TaipeiDate = todayTaipei,
                    TaipeiDateTime = taipeiNow,
                    IsWeekend = IsWeekend(taipeiNow),
                    MonthAttendance = monthAttendance,
                    TodayPotentialRewards = todayRewards,
                    CanSignToday = !todaySigned
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"取得使用者 {userId} 簽到狀態時發生錯誤");
                throw;
            }
        }

        /// <summary>
        /// 檢查使用者今日是否已簽到 (基於Asia/Taipei時區)
        /// </summary>
        private async Task<bool> HasSignedTodayAsync(int userId, DateTime todayTaipei)
        {
            // 將Taipei日期轉換為UTC範圍查詢
            var startUtc = TimeZoneInfo.ConvertTimeToUtc(todayTaipei, TaipeiTimeZone);
            var endUtc = TimeZoneInfo.ConvertTimeToUtc(todayTaipei.AddDays(1), TaipeiTimeZone);

            var todayRecord = await _context.UserSignInStats
                .Where(s => s.UserID == userId && 
                           s.SignTime >= startUtc && 
                           s.SignTime < endUtc)
                .FirstOrDefaultAsync();

            return todayRecord != null;
        }

        /// <summary>
        /// 計算當前連續簽到天數
        /// </summary>
        private async Task<int> CalculateCurrentStreakAsync(int userId)
        {
            var records = await _context.UserSignInStats
                .Where(s => s.UserID == userId)
                .OrderByDescending(s => s.SignTime)
                .Take(100) // 取最近100筆避免效能問題
                .ToListAsync();

            if (!records.Any())
                return 0;

            var streak = 0;
            var now = DateTime.UtcNow;
            var taipeiNow = TimeZoneInfo.ConvertTimeFromUtc(now, TaipeiTimeZone);
            var checkDate = taipeiNow.Date;

            // 從今天開始往前檢查連續簽到
            foreach (var record in records)
            {
                var recordTaipeiDate = TimeZoneInfo.ConvertTimeFromUtc(record.SignTime, TaipeiTimeZone).Date;
                
                if (recordTaipeiDate == checkDate)
                {
                    streak++;
                    checkDate = checkDate.AddDays(-1);
                }
                else if (recordTaipeiDate < checkDate)
                {
                    // 發現斷簽，停止計算
                    break;
                }
            }

            return streak;
        }

        #endregion

        #region 簽到執行

        /// <summary>
        /// 執行每日簽到並計算所有獎勵
        /// </summary>
        public async Task<SignInResultDto> PerformSignInAsync(int userId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _logger.LogInformation($"使用者 {userId} 執行每日簽到");

                var now = DateTime.UtcNow;
                var taipeiNow = TimeZoneInfo.ConvertTimeFromUtc(now, TaipeiTimeZone);
                var todayTaipei = taipeiNow.Date;

                // 檢查今日是否已簽到
                if (await HasSignedTodayAsync(userId, todayTaipei))
                {
                    return new SignInResultDto
                    {
                        Success = false,
                        Message = "今日已經簽到過了"
                    };
                }

                // 計算簽到前的連續天數
                var streakBefore = await CalculateCurrentStreakAsync(userId);

                // 計算基礎獎勵
                var baseRewards = CalculateBaseRewards(taipeiNow);
                var totalPoints = baseRewards.Points;
                var totalExp = baseRewards.Experience;
                var bonusMessages = new List<string>();

                // 檢查連續7天獎勵
                if (streakBefore + 1 == 7) // 今天簽到後達到7天
                {
                    totalPoints += 40;
                    totalExp += 300;
                    bonusMessages.Add("連續簽到7天獎勵：+40點數 +300經驗");
                    _logger.LogInformation($"使用者 {userId} 達成連續7天簽到獎勵");
                }

                // 檢查當月全勤獎勵 (僅在月末最後一天)
                var isMonthlyPerfect = false;
                if (IsLastDayOfMonth(taipeiNow))
                {
                    var monthlyAttendance = await GetMonthAttendanceAsync(userId, taipeiNow.Year, taipeiNow.Month);
                    var daysInMonth = DateTime.DaysInMonth(taipeiNow.Year, taipeiNow.Month);
                    
                    if (monthlyAttendance.SignedDays.Count + 1 == daysInMonth) // +1 因為今天還沒記錄
                    {
                        totalPoints += 200;
                        totalExp += 2000;
                        bonusMessages.Add("當月全勤獎勵：+200點數 +2000經驗");
                        isMonthlyPerfect = true;
                        _logger.LogInformation($"使用者 {userId} 達成當月全勤獎勵");
                    }
                }

                // 建立簽到記錄
                var signInRecord = new UserSignInStats
                {
                    SignTime = now,
                    UserID = userId,
                    PointsChanged = totalPoints,
                    ExpGained = totalExp,
                    PointsChangedTime = now,
                    ExpGainedTime = now
                };

                _context.UserSignInStats.Add(signInRecord);
                await _context.SaveChangesAsync();

                // 更新使用者錢包點數
                var walletResult = await _walletService.EarnPointsAsync(
                    userId, 
                    totalPoints, 
                    $"每日簽到獲得{totalPoints}點數", 
                    $"signin_{signInRecord.LogID}"
                );

                if (!walletResult.Success)
                {
                    _logger.LogWarning($"使用者 {userId} 簽到成功但點數更新失敗: {walletResult.Message}");
                }

                // 更新寵物經驗 (如果有獲得經驗)
                if (totalExp > 0)
                {
                    await UpdatePetExperienceAsync(userId, totalExp);
                }

                await transaction.CommitAsync();

                var streakAfter = streakBefore + 1;

                _logger.LogInformation($"使用者 {userId} 簽到成功：點數+{totalPoints}, 經驗+{totalExp}, 連續{streakAfter}天");

                return new SignInResultDto
                {
                    Success = true,
                    Message = "簽到成功！",
                    PointsEarned = totalPoints,
                    ExperienceGained = totalExp,
                    StreakBefore = streakBefore,
                    StreakAfter = streakAfter,
                    IsWeekend = IsWeekend(taipeiNow),
                    HasSevenDayBonus = bonusMessages.Any(m => m.Contains("連續簽到7天")),
                    HasMonthlyBonus = isMonthlyPerfect,
                    BonusMessages = bonusMessages,
                    SignInTime = taipeiNow
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, $"使用者 {userId} 簽到時發生錯誤");
                throw;
            }
        }

        #endregion

        #region 月度統計

        /// <summary>
        /// 取得指定月份的簽到統計
        /// </summary>
        public async Task<MonthlyAttendanceDto> GetMonthAttendanceAsync(int userId, int year, int month)
        {
            try
            {
                // 計算月份的UTC時間範圍
                var monthStart = new DateTime(year, month, 1);
                var monthEnd = monthStart.AddMonths(1);
                
                var startUtc = TimeZoneInfo.ConvertTimeToUtc(monthStart, TaipeiTimeZone);
                var endUtc = TimeZoneInfo.ConvertTimeToUtc(monthEnd, TaipeiTimeZone);

                var records = await _context.UserSignInStats
                    .Where(s => s.UserID == userId && 
                               s.SignTime >= startUtc && 
                               s.SignTime < endUtc)
                    .OrderBy(s => s.SignTime)
                    .ToListAsync();

                // 轉換為Taipei時區的日期
                var signedDays = records
                    .Select(r => TimeZoneInfo.ConvertTimeFromUtc(r.SignTime, TaipeiTimeZone).Date)
                    .Distinct()
                    .OrderBy(d => d)
                    .ToList();

                var daysInMonth = DateTime.DaysInMonth(year, month);
                var totalPointsEarned = records.Sum(r => r.PointsChanged);
                var totalExpGained = records.Sum(r => r.ExpGained);

                // 計算當前連續簽到天數 (在本月內)
                var currentMonthStreak = CalculateMonthlyStreak(signedDays, year, month);

                return new MonthlyAttendanceDto
                {
                    Year = year,
                    Month = month,
                    SignedDays = signedDays,
                    TotalSignedDays = signedDays.Count,
                    TotalDaysInMonth = daysInMonth,
                    AttendanceRate = (double)signedDays.Count / daysInMonth * 100,
                    TotalPointsEarned = totalPointsEarned,
                    TotalExperienceGained = totalExpGained,
                    IsPerfectAttendance = signedDays.Count == daysInMonth,
                    CurrentMonthStreak = currentMonthStreak,
                    Records = records.Select(r => new SignInRecordDto
                    {
                        LogId = r.LogID,
                        SignTime = TimeZoneInfo.ConvertTimeFromUtc(r.SignTime, TaipeiTimeZone),
                        PointsChanged = r.PointsChanged,
                        ExpGained = r.ExpGained
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"取得使用者 {userId} 月度簽到統計時發生錯誤");
                throw;
            }
        }

        /// <summary>
        /// 取得使用者簽到歷史記錄 (分頁)
        /// </summary>
        public async Task<PagedResult<SignInRecordDto>> GetSignInHistoryAsync(int userId, SignInHistoryQueryDto query)
        {
            try
            {
                var queryable = _context.UserSignInStats
                    .Where(s => s.UserID == userId);

                // 時間範圍篩選 (轉換為UTC)
                if (query.FromDate.HasValue)
                {
                    var fromUtc = TimeZoneInfo.ConvertTimeToUtc(query.FromDate.Value, TaipeiTimeZone);
                    queryable = queryable.Where(s => s.SignTime >= fromUtc);
                }

                if (query.ToDate.HasValue)
                {
                    var toUtc = TimeZoneInfo.ConvertTimeToUtc(query.ToDate.Value.AddDays(1), TaipeiTimeZone);
                    queryable = queryable.Where(s => s.SignTime < toUtc);
                }

                // 總數計算
                var totalCount = await queryable.CountAsync();

                // 排序和分頁
                var records = await queryable
                    .OrderByDescending(s => s.SignTime)
                    .Skip((query.Page - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .ToListAsync();

                var items = records.Select(r => new SignInRecordDto
                {
                    LogId = r.LogID,
                    SignTime = TimeZoneInfo.ConvertTimeFromUtc(r.SignTime, TaipeiTimeZone),
                    PointsChanged = r.PointsChanged,
                    ExpGained = r.ExpGained,
                    IsWeekend = IsWeekend(TimeZoneInfo.ConvertTimeFromUtc(r.SignTime, TaipeiTimeZone)),
                    DayOfWeek = TimeZoneInfo.ConvertTimeFromUtc(r.SignTime, TaipeiTimeZone).DayOfWeek.ToString()
                }).ToList();

                return new PagedResult<SignInRecordDto>
                {
                    Items = items,
                    TotalCount = totalCount,
                    CurrentPage = query.Page,
                    PageSize = query.PageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"取得使用者 {userId} 簽到歷史時發生錯誤");
                throw;
            }
        }

        #endregion

        #region 輔助方法

        /// <summary>
        /// 計算基礎簽到獎勵 (平日/假日)
        /// </summary>
        private static SignInRewards CalculateBaseRewards(DateTime taipeiDateTime)
        {
            if (IsWeekend(taipeiDateTime))
            {
                // 假日 (六、日): +30點數 +200經驗
                return new SignInRewards { Points = 30, Experience = 200 };
            }
            else
            {
                // 平日 (一~五): +20點數 +0經驗
                return new SignInRewards { Points = 20, Experience = 0 };
            }
        }

        /// <summary>
        /// 判斷是否為週末
        /// </summary>
        private static bool IsWeekend(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }

        /// <summary>
        /// 判斷是否為月份最後一天
        /// </summary>
        private static bool IsLastDayOfMonth(DateTime date)
        {
            var lastDay = DateTime.DaysInMonth(date.Year, date.Month);
            return date.Day == lastDay;
        }

        /// <summary>
        /// 計算今日潛在獎勵 (用於前端顯示)
        /// </summary>
        private async Task<SignInRewards> CalculateTodayRewardsAsync(int userId, DateTime taipeiNow)
        {
            var baseRewards = CalculateBaseRewards(taipeiNow);
            var currentStreak = await CalculateCurrentStreakAsync(userId);

            var rewards = new SignInRewards
            {
                Points = baseRewards.Points,
                Experience = baseRewards.Experience
            };

            // 連續7天獎勵
            if (currentStreak + 1 == 7)
            {
                rewards.Points += 40;
                rewards.Experience += 300;
            }

            // 當月全勤獎勵
            if (IsLastDayOfMonth(taipeiNow))
            {
                var monthlyAttendance = await GetMonthAttendanceAsync(userId, taipeiNow.Year, taipeiNow.Month);
                var daysInMonth = DateTime.DaysInMonth(taipeiNow.Year, taipeiNow.Month);
                
                if (monthlyAttendance.SignedDays.Count + 1 == daysInMonth)
                {
                    rewards.Points += 200;
                    rewards.Experience += 2000;
                }
            }

            return rewards;
        }

        /// <summary>
        /// 更新寵物經驗值
        /// </summary>
        private async Task UpdatePetExperienceAsync(int userId, int expGained)
        {
            try
            {
                var pet = await _context.Pets
                    .Where(p => p.UserID == userId)
                    .FirstOrDefaultAsync();

                if (pet != null)
                {
                    pet.Experience += expGained;
                    
                    // 檢查是否升級 (這裡簡化處理，完整升級邏輯在寵物模組實現)
                    await _context.SaveChangesAsync();
                    
                    _logger.LogInformation($"更新寵物 {pet.PetID} 經驗值 +{expGained}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"更新使用者 {userId} 寵物經驗時發生錯誤");
                // 不影響簽到主流程
            }
        }

        /// <summary>
        /// 計算本月內的連續簽到天數
        /// </summary>
        private static int CalculateMonthlyStreak(List<DateTime> signedDays, int year, int month)
        {
            if (!signedDays.Any())
                return 0;

            var now = DateTime.Now;
            var currentDate = new DateTime(year, month, Math.Min(now.Day, DateTime.DaysInMonth(year, month)));
            var streak = 0;

            // 從當前日期往前計算連續天數
            while (currentDate.Month == month)
            {
                if (signedDays.Contains(currentDate))
                {
                    streak++;
                    currentDate = currentDate.AddDays(-1);
                }
                else
                {
                    break;
                }
            }

            return streak;
        }

        #endregion

        #region 內部類別

        /// <summary>
        /// 簽到獎勵結構
        /// </summary>
        private class SignInRewards
        {
            public int Points { get; set; }
            public int Experience { get; set; }
        }

        #endregion
    }
}