using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace GameCore.Core.Services
{
    public class SignInService : ISignInService
    {
        private readonly ISignInRepository _signInRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SignInService> _logger;

        public SignInService(
            ISignInRepository signInRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ILogger<SignInService> logger)
        {
            _signInRepository = signInRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<SignInResult> SignInAsync(int userId)
        {
            try
            {
                // 使用 Asia/Taipei 時區
                var taipeiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Taipei");
                var today = TimeZoneInfo.ConvertTime(DateTime.UtcNow, taipeiTimeZone).Date;
                
                // 檢查今天是否已經簽到
                var todaySignIn = await _signInRepository.GetByUserIdAndDateAsync(userId, today);
                if (todaySignIn != null)
                {
                    return new SignInResult
                    {
                        Success = false,
                        Message = "今天已經簽到過了"
                    };
                }

                // 獲取用戶資料
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return new SignInResult
                    {
                        Success = false,
                        Message = "用戶不存在"
                    };
                }

                // 檢查是否為週末
                var isWeekend = IsWeekend(today);
                
                // 計算連續簽到天數
                var consecutiveDays = await CalculateConsecutiveDays(userId);
                
                // 計算獎勵 (按照規格)
                var (points, experience) = CalculateRewards(isWeekend, consecutiveDays, today);
                
                // 創建簽到記錄
                var signInRecord = new SignInRecord
                {
                    UserId = userId,
                    SignInDate = today,
                    Points = points,
                    Experience = experience,
                    IsWeekend = isWeekend,
                    IsPerfect = false, // 稍後檢查
                    CreateTime = DateTime.UtcNow
                };

                _signInRepository.Add(signInRecord);

                // 更新用戶點數和經驗值
                user.Points += points;
                user.Experience += experience;
                _userRepository.Update(user);

                // 檢查是否為月度完美簽到
                var isMonthlyPerfect = await CheckMonthlyPerfectSignIn(userId, today);
                if (isMonthlyPerfect)
                {
                    signInRecord.IsPerfect = true;
                    // 額外獎勵已在 CalculateRewards 中計算
                }

                // 創建簽到統計記錄
                var signInStats = new UserSignInStats
                {
                    UserId = userId,
                    SignTime = DateTime.UtcNow,
                    PointsChanged = points,
                    ExpGained = experience,
                    PointsChangedTime = DateTime.UtcNow,
                    ExpGainedTime = DateTime.UtcNow
                };

                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("用戶簽到成功: {UserId}, 日期: {Date}, 點數: {Points}, 經驗值: {Experience}, 連續天數: {ConsecutiveDays}", 
                    userId, today, points, experience, consecutiveDays + 1);

                return new SignInResult
                {
                    Success = true,
                    Message = "簽到成功！",
                    Points = points,
                    Experience = experience,
                    IsWeekend = isWeekend,
                    ConsecutiveDays = consecutiveDays + 1,
                    IsMonthlyPerfect = isMonthlyPerfect
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "用戶簽到失敗: {UserId}", userId);
                return new SignInResult
                {
                    Success = false,
                    Message = "簽到失敗，請稍後重試"
                };
            }
        }

        public async Task<SignInStatusResult> GetSignInStatusAsync(int userId)
        {
            try
            {
                var taipeiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Taipei");
                var today = TimeZoneInfo.ConvertTime(DateTime.UtcNow, taipeiTimeZone).Date;
                
                // 檢查今天是否已經簽到
                var todaySignIn = await _signInRepository.GetByUserIdAndDateAsync(userId, today);
                var isSignedInToday = todaySignIn != null;

                // 計算連續簽到天數
                var consecutiveDays = await CalculateConsecutiveDays(userId);

                // 檢查是否為週末
                var isWeekend = IsWeekend(today);

                return new SignInStatusResult
                {
                    Success = true,
                    IsSignedInToday = isSignedInToday,
                    IsWeekend = isWeekend,
                    ConsecutiveDays = consecutiveDays,
                    Today = today
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取簽到狀態失敗: {UserId}", userId);
                return new SignInStatusResult
                {
                    Success = false,
                    Message = "獲取狀態失敗"
                };
            }
        }

        public async Task<IEnumerable<SignInRecordDto>> GetSignInHistoryAsync(int userId, int page = 1, int pageSize = 30)
        {
            try
            {
                var records = await _signInRepository.GetByUserIdAsync(userId, page, pageSize);
                return records.Select(r => new SignInRecordDto
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    SignInDate = r.SignInDate,
                    Points = r.Points,
                    Experience = r.Experience,
                    IsWeekend = r.IsWeekend,
                    IsPerfect = r.IsPerfect,
                    CreateTime = r.CreateTime
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取簽到歷史失敗: {UserId}", userId);
                return Enumerable.Empty<SignInRecordDto>();
            }
        }

        public async Task<bool> IsSignedInTodayAsync(int userId)
        {
            try
            {
                var taipeiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Taipei");
                var today = TimeZoneInfo.ConvertTime(DateTime.UtcNow, taipeiTimeZone).Date;
                var todaySignIn = await _signInRepository.GetByUserIdAndDateAsync(userId, today);
                return todaySignIn != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "檢查今日簽到狀態失敗: {UserId}", userId);
                return false;
            }
        }

        public async Task<int> GetConsecutiveDaysAsync(int userId)
        {
            try
            {
                return await CalculateConsecutiveDays(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取連續簽到天數失敗: {UserId}", userId);
                return 0;
            }
        }

        private bool IsWeekend(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }

        private (int points, int experience) CalculateRewards(bool isWeekend, int consecutiveDays, DateTime signInDate)
        {
            int points = 0;
            int experience = 0;

            // 基礎獎勵
            if (isWeekend)
            {
                points = 30;  // 週末: +30 points
                experience = 200;  // 週末: +200 exp
            }
            else
            {
                points = 20;  // 平日: +20 points
                experience = 0;   // 平日: 0 exp
            }

            // 7天連續簽到獎勵
            if (consecutiveDays >= 6) // 今天是第7天
            {
                points += 40;  // +40 points
                experience += 300;  // +300 exp
            }

            // 月度完美簽到獎勵 (每月最後一天)
            var lastDayOfMonth = new DateTime(signInDate.Year, signInDate.Month, DateTime.DaysInMonth(signInDate.Year, signInDate.Month));
            if (signInDate.Date == lastDayOfMonth.Date)
            {
                // 檢查是否整月都有簽到
                var isMonthlyPerfect = CheckMonthlyPerfectSignIn(signInDate).Result;
                if (isMonthlyPerfect)
                {
                    points += 200;  // +200 points
                    experience += 2000;  // +2000 exp
                }
            }

            return (points, experience);
        }

        private async Task<int> CalculateConsecutiveDays(int userId)
        {
            try
            {
                var taipeiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Taipei");
                var today = TimeZoneInfo.ConvertTime(DateTime.UtcNow, taipeiTimeZone).Date;
                var consecutiveDays = 0;
                var currentDate = today.AddDays(-1); // 從昨天開始檢查

                while (true)
                {
                    var signInRecord = await _signInRepository.GetByUserIdAndDateAsync(userId, currentDate);
                    if (signInRecord == null)
                    {
                        break;
                    }

                    consecutiveDays++;
                    currentDate = currentDate.AddDays(-1);
                }

                return consecutiveDays;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "計算連續簽到天數失敗: {UserId}", userId);
                return 0;
            }
        }

        private async Task<bool> CheckMonthlyPerfectSignIn(int userId, DateTime signInDate)
        {
            try
            {
                var year = signInDate.Year;
                var month = signInDate.Month;
                var daysInMonth = DateTime.DaysInMonth(year, month);
                
                var startDate = new DateTime(year, month, 1);
                var endDate = new DateTime(year, month, daysInMonth);
                
                var records = await _signInRepository.GetByUserIdAndDateRangeAsync(userId, startDate, endDate);
                var signInDays = records.Count;

                return signInDays == daysInMonth;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "檢查月度完美簽到失敗: 用戶 {UserId}, 年月 {Year}-{Month}", userId, signInDate.Year, signInDate.Month);
                return false;
            }
        }
    }
}