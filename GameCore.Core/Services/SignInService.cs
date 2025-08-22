using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace GameCore.Core.Services
{
    /// <summary>
    /// 簽到服務實作
    /// 包含每日簽到的完整業務邏輯，按照規格實現獎勵機制
    /// </summary>
    public class SignInService : ISignInService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPetService _petService;
        private readonly ILogger<SignInService> _logger;

        // 簽到獎勵常數 (按照規格設定)
        private const int WEEKDAY_POINTS = 20;        // 平日點數
        private const int WEEKEND_POINTS = 30;        // 假日點數
        private const int WEEKEND_EXPERIENCE = 200;   // 假日經驗
        private const int SEVEN_DAY_BONUS_POINTS = 40;     // 連續 7 天額外點數
        private const int SEVEN_DAY_BONUS_EXPERIENCE = 300; // 連續 7 天額外經驗
        private const int MONTHLY_PERFECT_POINTS = 200;     // 當月全勤額外點數
        private const int MONTHLY_PERFECT_EXPERIENCE = 2000; // 當月全勤額外經驗

        public SignInService(IUnitOfWork unitOfWork, IPetService petService, ILogger<SignInService> logger)
        {
            _unitOfWork = unitOfWork;
            _petService = petService;
            _logger = logger;
        }

        /// <summary>
        /// 執行每日簽到 (以 Asia/Taipei 時區為基準)
        /// </summary>
        public async Task<SignInResult> SignInAsync(int userId)
        {
            try
            {
                // 轉換為台北時區進行日期判斷
                var taipeiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Taipei");
                var taipeiNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, taipeiTimeZone);
                var signInDate = taipeiNow.Date;

                // 檢查今日是否已簽到
                if (await HasSignedInTodayAsync(userId))
                {
                    return new SignInResult
                    {
                        Success = false,
                        Message = "今日已經簽到過了，明天再來吧！"
                    };
                }

                // 取得連續簽到天數
                var consecutiveDays = await GetConsecutiveDaysAsync(userId);
                consecutiveDays++; // 包含今天

                // 檢查是否為當月全勤 (在月底簽到時檢查)
                var isLastDayOfMonth = signInDate.AddDays(1).Day == 1;
                var isMonthlyPerfect = false;
                if (isLastDayOfMonth)
                {
                    isMonthlyPerfect = await IsMonthlyPerfectAttendanceAsync(userId, signInDate.Year, signInDate.Month);
                }

                // 計算簽到獎勵
                var rewardConfig = CalculateSignInReward(signInDate, consecutiveDays, isMonthlyPerfect);

                // 建立簽到記錄
                var signInRecord = new UserSignInStats
                {
                    UserId = userId,
                    SignTime = DateTime.UtcNow,
                    PointsChanged = rewardConfig.TotalPoints,
                    ExpGained = rewardConfig.TotalExperience,
                    PointsChangedTime = DateTime.UtcNow,
                    ExpGainedTime = DateTime.UtcNow
                };

                await _unitOfWork.SignInRepository.AddAsync(signInRecord);

                // 更新使用者點數
                await _unitOfWork.UserRepository.AddPointsAsync(userId, rewardConfig.TotalPoints, "每日簽到");

                // 為寵物增加經驗值
                if (rewardConfig.TotalExperience > 0)
                {
                    var pet = await _petService.GetOrCreatePetAsync(userId);
                    await _petService.AddExperienceAsync(pet, rewardConfig.TotalExperience);
                }

                // 取得更新後的使用者點數
                var wallet = await _unitOfWork.UserRepository.GetWalletAsync(userId);

                await _unitOfWork.SaveChangesAsync();

                // 建立結果訊息
                var messageBuilder = new List<string>();
                messageBuilder.Add($"簽到成功！獲得 {rewardConfig.BasePoints} 點數");

                if (rewardConfig.BaseExperience > 0)
                {
                    messageBuilder.Add($"寵物獲得 {rewardConfig.BaseExperience} 經驗值");
                }

                if (consecutiveDays == 7)
                {
                    messageBuilder.Add($"連續簽到 7 天！額外獲得 {rewardConfig.SevenDayBonusPoints} 點數和 {rewardConfig.SevenDayBonusExperience} 經驗值");
                }

                if (isMonthlyPerfect)
                {
                    messageBuilder.Add($"當月全勤達成！額外獲得 {rewardConfig.MonthlyPerfectBonusPoints} 點數和 {rewardConfig.MonthlyPerfectBonusExperience} 經驗值");
                }

                _logger.LogInformation($"使用者 {userId} 簽到成功，連續 {consecutiveDays} 天，獲得 {rewardConfig.TotalPoints} 點數，{rewardConfig.TotalExperience} 經驗");

                return new SignInResult
                {
                    Success = true,
                    Message = string.Join("，", messageBuilder),
                    PointsEarned = rewardConfig.TotalPoints,
                    ExperienceEarned = rewardConfig.TotalExperience,
                    ConsecutiveDays = consecutiveDays,
                    IsSevenDayBonus = consecutiveDays == 7,
                    IsMonthlyPerfectBonus = isMonthlyPerfect,
                    SignInRecord = signInRecord,
                    TotalUserPoints = wallet?.UserPoint ?? 0
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"簽到時發生錯誤，使用者ID: {userId}");
                return new SignInResult
                {
                    Success = false,
                    Message = "簽到失敗，請稍後再試"
                };
            }
        }

        /// <summary>
        /// 檢查今日是否已簽到 (以台北時區為準)
        /// </summary>
        public async Task<bool> HasSignedInTodayAsync(int userId)
        {
            var taipeiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Taipei");
            var taipeiNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, taipeiTimeZone);
            var today = taipeiNow.Date;

            var todaySignIn = await _unitOfWork.SignInRepository.GetSignInByDateAsync(userId, today);
            return todaySignIn != null;
        }

        /// <summary>
        /// 取得使用者連續簽到天數
        /// </summary>
        public async Task<int> GetConsecutiveDaysAsync(int userId)
        {
            var recentSignIns = await _unitOfWork.SignInRepository.GetRecentSignInsAsync(userId, 30);
            
            if (!recentSignIns.Any()) return 0;

            var taipeiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Taipei");
            var taipeiNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, taipeiTimeZone);
            var today = taipeiNow.Date;

            int consecutiveDays = 0;
            var checkDate = today.AddDays(-1); // 從昨天開始檢查

            // 向前檢查連續簽到天數
            while (true)
            {
                var signInOnDate = recentSignIns.Any(s => 
                {
                    var signInTaipeiTime = TimeZoneInfo.ConvertTimeFromUtc(s.SignTime, taipeiTimeZone);
                    return signInTaipeiTime.Date == checkDate;
                });

                if (signInOnDate)
                {
                    consecutiveDays++;
                    checkDate = checkDate.AddDays(-1);
                }
                else
                {
                    break;
                }
            }

            return consecutiveDays;
        }

        /// <summary>
        /// 取得當月簽到統計
        /// </summary>
        public async Task<MonthlySignInStats> GetMonthlyStatsAsync(int userId, int year, int month)
        {
            var monthStart = new DateTime(year, month, 1);
            var monthEnd = monthStart.AddMonths(1).AddDays(-1);

            var monthlySignIns = await _unitOfWork.SignInRepository.GetSignInsByDateRangeAsync(userId, monthStart, monthEnd);

            var taipeiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Taipei");
            var signInDates = monthlySignIns.Select(s =>
            {
                var taipeiTime = TimeZoneInfo.ConvertTimeFromUtc(s.SignTime, taipeiTimeZone);
                return taipeiTime.Date;
            }).Distinct().OrderBy(d => d).ToList();

            var totalDaysInMonth = DateTime.DaysInMonth(year, month);
            var isPerfectAttendance = signInDates.Count == totalDaysInMonth;

            var totalPoints = monthlySignIns.Sum(s => s.PointsChanged);
            var totalExperience = monthlySignIns.Sum(s => s.ExpGained);

            return new MonthlySignInStats
            {
                Year = year,
                Month = month,
                TotalSignInDays = signInDates.Count,
                TotalDaysInMonth = totalDaysInMonth,
                IsPerfectAttendance = isPerfectAttendance,
                SignInDates = signInDates,
                TotalPointsEarned = totalPoints,
                TotalExperienceEarned = totalExperience
            };
        }

        /// <summary>
        /// 檢查是否為當月全勤
        /// </summary>
        public async Task<bool> IsMonthlyPerfectAttendanceAsync(int userId, int year, int month)
        {
            var monthStats = await GetMonthlyStatsAsync(userId, year, month);
            return monthStats.IsPerfectAttendance;
        }

        /// <summary>
        /// 計算簽到獎勵 (按照規格設定)
        /// </summary>
        public SignInRewardConfig CalculateSignInReward(DateTime signInDate, int consecutiveDays, bool isMonthlyPerfect)
        {
            var isWeekend = signInDate.DayOfWeek == DayOfWeek.Saturday || signInDate.DayOfWeek == DayOfWeek.Sunday;

            var config = new SignInRewardConfig
            {
                IsWeekend = isWeekend
            };

            // 基礎獎勵 (平日/假日)
            if (isWeekend)
            {
                config.BasePoints = WEEKEND_POINTS;           // 假日 +30 點
                config.BaseExperience = WEEKEND_EXPERIENCE;   // 假日 +200 經驗
            }
            else
            {
                config.BasePoints = WEEKDAY_POINTS;           // 平日 +20 點
                config.BaseExperience = 0;                    // 平日 +0 經驗
            }

            // 連續 7 天獎勵
            if (consecutiveDays == 7)
            {
                config.SevenDayBonusPoints = SEVEN_DAY_BONUS_POINTS;         // +40 點
                config.SevenDayBonusExperience = SEVEN_DAY_BONUS_EXPERIENCE; // +300 經驗
            }

            // 當月全勤獎勵 (在當月最後一日簽到時發放)
            if (isMonthlyPerfect)
            {
                config.MonthlyPerfectBonusPoints = MONTHLY_PERFECT_POINTS;         // +200 點
                config.MonthlyPerfectBonusExperience = MONTHLY_PERFECT_EXPERIENCE; // +2000 經驗
            }

            return config;
        }

        /// <summary>
        /// 取得使用者簽到歷史 (分頁)
        /// </summary>
        public async Task<SignInHistoryResult> GetSignInHistoryAsync(int userId, int page = 1, int pageSize = 30)
        {
            try
            {
                var totalCount = await _unitOfWork.SignInRepository.GetSignInCountAsync(userId);
                var records = await _unitOfWork.SignInRepository.GetSignInHistoryAsync(userId, page, pageSize);

                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

                return new SignInHistoryResult
                {
                    Records = records.ToList(),
                    TotalCount = totalCount,
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalPages = totalPages,
                    HasNextPage = page < totalPages,
                    HasPreviousPage = page > 1
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"取得簽到歷史時發生錯誤，使用者ID: {userId}");
                return new SignInHistoryResult();
            }
        }
    }
}