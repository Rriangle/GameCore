using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using GameCore.Core.DTOs;
using GameCore.Core.Enums;
using GameCore.Core.Services;
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
                var today = DateTime.UtcNow.Date;
                
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

                // 檢查是否為假日
                var isHoliday = IsHoliday(today);
                
                // 計算獎勵
                var (points, experience) = CalculateRewards(today, isHoliday);
                
                // 創建簽到記錄
                var signInRecord = new SignInRecord
                {
                    UserId = userId,
                    SignInDate = today,
                    Points = points,
                    Experience = experience,
                    IsHoliday = isHoliday,
                    CreatedAt = DateTime.UtcNow
                };

                _signInRepository.Add(signInRecord);

                // 更新用戶點數和經驗值
                user.Points += points;
                user.Experience += experience;
                _userRepository.Update(user);

                // 更新或創建簽到統計
                await UpdateSignInStatistics(userId, today);

                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("用戶簽到成功: {UserId}, 日期: {Date}, 點數: {Points}, 經驗值: {Experience}", 
                    userId, today, points, experience);

                return new SignInResult
                {
                    Success = true,
                    Message = "簽到成功！",
                    Points = points,
                    Experience = experience,
                    IsHoliday = isHoliday
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
                var today = DateTime.UtcNow.Date;
                
                // 檢查今天是否已經簽到
                var todaySignIn = await _signInRepository.GetByUserIdAndDateAsync(userId, today);
                var isSignedInToday = todaySignIn != null;

                // 獲取簽到統計
                var statistics = await _signInRepository.GetStatisticsByUserIdAsync(userId);
                
                // 計算連續簽到天數
                var consecutiveDays = await CalculateConsecutiveDays(userId);

                // 檢查是否為假日
                var isHoliday = IsHoliday(today);

                return new SignInStatusResult
                {
                    Success = true,
                    IsSignedInToday = isSignedInToday,
                    IsHoliday = isHoliday,
                    ConsecutiveDays = consecutiveDays,
                    MonthlyPerfectDays = statistics?.MonthlyPerfectDays ?? 0,
                    TotalSignInDays = statistics?.TotalSignInDays ?? 0,
                    TotalPoints = statistics?.TotalPoints ?? 0,
                    TotalExperience = statistics?.TotalExperience ?? 0
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
                    IsHoliday = r.IsHoliday,
                    CreatedAt = r.CreatedAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取簽到歷史失敗: {UserId}", userId);
                return Enumerable.Empty<SignInRecordDto>();
            }
        }

        public async Task<SignInStatisticsDto> GetSignInStatisticsAsync(int userId)
        {
            try
            {
                var statistics = await _signInRepository.GetStatisticsByUserIdAsync(userId);
                if (statistics == null)
                {
                    return new SignInStatisticsDto
                    {
                        Success = false,
                        Message = "統計資料不存在"
                    };
                }

                var consecutiveDays = await CalculateConsecutiveDays(userId);

                return new SignInStatisticsDto
                {
                    Success = true,
                    TotalSignInDays = statistics.TotalSignInDays,
                    ConsecutiveDays = consecutiveDays,
                    MonthlyPerfectDays = statistics.MonthlyPerfectDays,
                    TotalPoints = statistics.TotalPoints,
                    TotalExperience = statistics.TotalExperience,
                    LastSignInDate = statistics.LastSignInDate,
                    CreatedAt = statistics.CreatedAt,
                    UpdatedAt = statistics.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取簽到統計失敗: {UserId}", userId);
                return new SignInStatisticsDto
                {
                    Success = false,
                    Message = "獲取統計失敗"
                };
            }
        }

        public async Task<IEnumerable<SignInCalendarDto>> GetMonthlySignInCalendarAsync(int userId, int year, int month)
        {
            try
            {
                var startDate = new DateTime(year, month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);
                
                var records = await _signInRepository.GetByUserIdAndDateRangeAsync(userId, startDate, endDate);
                var calendar = new List<SignInCalendarDto>();

                for (var date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    var record = records.FirstOrDefault(r => r.SignInDate.Date == date.Date);
                    var isHoliday = IsHoliday(date);

                    calendar.Add(new SignInCalendarDto
                    {
                        Date = date,
                        IsSignedIn = record != null,
                        IsHoliday = isHoliday,
                        Points = record?.Points ?? 0,
                        Experience = record?.Experience ?? 0
                    });
                }

                return calendar;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取月度簽到日曆失敗: 用戶 {UserId}, 年月 {Year}-{Month}", userId, year, month);
                return Enumerable.Empty<SignInCalendarDto>();
            }
        }

        public async Task<bool> IsSignedInTodayAsync(int userId)
        {
            try
            {
                var today = DateTime.UtcNow.Date;
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

        public async Task<bool> IsMonthlyPerfectAsync(int userId, int year, int month)
        {
            try
            {
                var startDate = new DateTime(year, month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);
                var daysInMonth = DateTime.DaysInMonth(year, month);
                
                var records = await _signInRepository.GetByUserIdAndDateRangeAsync(userId, startDate, endDate);
                var signInDays = records.Count;

                return signInDays == daysInMonth;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "檢查月度完美簽到失敗: 用戶 {UserId}, 年月 {Year}-{Month}", userId, year, month);
                return false;
            }
        }

        private bool IsHoliday(DateTime date)
        {
            // 簡單的假日判斷邏輯
            // 週末
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                return true;
            }

            // 國定假日（台灣）
            var holidays = new[]
            {
                new DateTime(date.Year, 1, 1),   // 元旦
                new DateTime(date.Year, 2, 28),  // 和平紀念日
                new DateTime(date.Year, 4, 4),   // 兒童節
                new DateTime(date.Year, 4, 5),   // 清明節
                new DateTime(date.Year, 5, 1),   // 勞動節
                new DateTime(date.Year, 6, 3),   // 端午節
                new DateTime(date.Year, 9, 29),  // 中秋節
                new DateTime(date.Year, 10, 10), // 國慶日
                new DateTime(date.Year, 12, 25)  // 聖誕節
            };

            return holidays.Any(h => h.Date == date.Date);
        }

        private (int points, int experience) CalculateRewards(DateTime date, bool isHoliday)
        {
            var basePoints = 10;
            var baseExperience = 5;

            // 假日獎勵
            if (isHoliday)
            {
                basePoints = (int)(basePoints * 1.5);
                baseExperience = (int)(baseExperience * 1.5);
            }

            // 特殊日期獎勵
            if (date.Day == 1) // 每月第一天
            {
                basePoints += 20;
                baseExperience += 10;
            }

            if (date.DayOfWeek == DayOfWeek.Monday) // 每週第一天
            {
                basePoints += 10;
                baseExperience += 5;
            }

            return (basePoints, baseExperience);
        }

        private async Task<int> CalculateConsecutiveDays(int userId)
        {
            try
            {
                var today = DateTime.UtcNow.Date;
                var consecutiveDays = 0;
                var currentDate = today;

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

        private async Task UpdateSignInStatistics(int userId, DateTime signInDate)
        {
            try
            {
                var statistics = await _signInRepository.GetStatisticsByUserIdAsync(userId);
                if (statistics == null)
                {
                    // 創建新的統計記錄
                    statistics = new SignInStatistics
                    {
                        UserId = userId,
                        TotalSignInDays = 1,
                        MonthlyPerfectDays = 0,
                        TotalPoints = 0,
                        TotalExperience = 0,
                        LastSignInDate = signInDate,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    _signInRepository.AddStatistics(statistics);
                }
                else
                {
                    // 更新現有統計
                    statistics.TotalSignInDays++;
                    statistics.LastSignInDate = signInDate;
                    statistics.UpdatedAt = DateTime.UtcNow;
                    _signInRepository.UpdateStatistics(statistics);
                }

                // 檢查月度完美簽到
                var year = signInDate.Year;
                var month = signInDate.Month;
                if (await IsMonthlyPerfectAsync(userId, year, month))
                {
                    statistics.MonthlyPerfectDays++;
                    _signInRepository.UpdateStatistics(statistics);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新簽到統計失敗: 用戶 {UserId}", userId);
            }
        }
    }
}