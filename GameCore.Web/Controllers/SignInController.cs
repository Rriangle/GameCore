using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GameCore.Core.Services;
using System.Security.Claims;

namespace GameCore.Web.Controllers
{
    /// <summary>
    /// 簽到系統控制器
    /// 提供每日簽到、簽到歷史、月度統計等 API 端點
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SignInController : ControllerBase
    {
        private readonly ISignInService _signInService;
        private readonly ILogger<SignInController> _logger;

        public SignInController(ISignInService signInService, ILogger<SignInController> logger)
        {
            _signInService = signInService;
            _logger = logger;
        }

        /// <summary>
        /// 執行每日簽到
        /// POST /api/signin
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> SignIn()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == 0)
                {
                    return Unauthorized(new { message = "請先登入" });
                }

                var result = await _signInService.SignInAsync(userId);

                if (!result.Success)
                {
                    return BadRequest(new { message = result.Message });
                }

                _logger.LogInformation($"使用者 {userId} 簽到成功，獲得 {result.PointsEarned} 點數");

                return Ok(new
                {
                    success = true,
                    message = result.Message,
                    pointsEarned = result.PointsEarned,
                    experienceEarned = result.ExperienceEarned,
                    consecutiveDays = result.ConsecutiveDays,
                    isSevenDayBonus = result.IsSevenDayBonus,
                    isMonthlyPerfectBonus = result.IsMonthlyPerfectBonus,
                    totalUserPoints = result.TotalUserPoints,
                    signInTime = result.SignInRecord?.SignTime
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"簽到時發生錯誤，使用者ID: {GetCurrentUserId()}");
                return StatusCode(500, new { message = "簽到失敗，請稍後再試" });
            }
        }

        /// <summary>
        /// 取得簽到狀態
        /// GET /api/signin/status
        /// </summary>
        [HttpGet("status")]
        public async Task<IActionResult> GetSignInStatus()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == 0)
                {
                    return Unauthorized(new { message = "請先登入" });
                }

                var hasSignedInToday = await _signInService.HasSignedInTodayAsync(userId);
                var consecutiveDays = await _signInService.GetConsecutiveDaysAsync(userId);

                // 取得當月統計
                var now = DateTime.Now;
                var monthlyStats = await _signInService.GetMonthlyStatsAsync(userId, now.Year, now.Month);

                return Ok(new
                {
                    hasSignedInToday = hasSignedInToday,
                    consecutiveDays = consecutiveDays,
                    monthlyStats = new
                    {
                        year = monthlyStats.Year,
                        month = monthlyStats.Month,
                        totalSignInDays = monthlyStats.TotalSignInDays,
                        totalDaysInMonth = monthlyStats.TotalDaysInMonth,
                        isPerfectAttendance = monthlyStats.IsPerfectAttendance,
                        signInDates = monthlyStats.SignInDates,
                        totalPointsEarned = monthlyStats.TotalPointsEarned,
                        totalExperienceEarned = monthlyStats.TotalExperienceEarned
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"取得簽到狀態時發生錯誤，使用者ID: {GetCurrentUserId()}");
                return StatusCode(500, new { message = "取得狀態失敗" });
            }
        }

        /// <summary>
        /// 取得簽到歷史
        /// GET /api/signin/history
        /// </summary>
        [HttpGet("history")]
        public async Task<IActionResult> GetSignInHistory([FromQuery] int page = 1, [FromQuery] int pageSize = 30)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == 0)
                {
                    return Unauthorized(new { message = "請先登入" });
                }

                if (page < 1) page = 1;
                if (pageSize < 1 || pageSize > 100) pageSize = 30;

                var history = await _signInService.GetSignInHistoryAsync(userId, page, pageSize);

                return Ok(new
                {
                    records = history.Records.Select(r => new
                    {
                        logId = r.LogId,
                        signTime = r.SignTime,
                        pointsChanged = r.PointsChanged,
                        expGained = r.ExpGained,
                        pointsChangedTime = r.PointsChangedTime,
                        expGainedTime = r.ExpGainedTime
                    }),
                    pagination = new
                    {
                        totalCount = history.TotalCount,
                        currentPage = history.CurrentPage,
                        pageSize = history.PageSize,
                        totalPages = history.TotalPages,
                        hasNextPage = history.HasNextPage,
                        hasPreviousPage = history.HasPreviousPage
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"取得簽到歷史時發生錯誤，使用者ID: {GetCurrentUserId()}");
                return StatusCode(500, new { message = "取得歷史失敗" });
            }
        }

        /// <summary>
        /// 取得指定月份的簽到統計
        /// GET /api/signin/monthly/{year}/{month}
        /// </summary>
        [HttpGet("monthly/{year:int}/{month:int}")]
        public async Task<IActionResult> GetMonthlyStats(int year, int month)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == 0)
                {
                    return Unauthorized(new { message = "請先登入" });
                }

                if (year < 2020 || year > 2030 || month < 1 || month > 12)
                {
                    return BadRequest(new { message = "無效的年份或月份" });
                }

                var monthlyStats = await _signInService.GetMonthlyStatsAsync(userId, year, month);

                return Ok(monthlyStats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"取得月度簽到統計時發生錯誤，使用者ID: {GetCurrentUserId()}, 年月: {year}-{month}");
                return StatusCode(500, new { message = "取得統計失敗" });
            }
        }

        /// <summary>
        /// 預覽簽到獎勵 (不實際簽到)
        /// GET /api/signin/preview-reward
        /// </summary>
        [HttpGet("preview-reward")]
        public async Task<IActionResult> PreviewSignInReward()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == 0)
                {
                    return Unauthorized(new { message = "請先登入" });
                }

                // 檢查是否已簽到
                var hasSignedInToday = await _signInService.HasSignedInTodayAsync(userId);
                if (hasSignedInToday)
                {
                    return BadRequest(new { message = "今日已經簽到過了" });
                }

                var consecutiveDays = await _signInService.GetConsecutiveDaysAsync(userId);
                consecutiveDays++; // 包含今天

                // 檢查是否為當月最後一天 (全勤獎勵)
                var taipeiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Taipei");
                var taipeiNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, taipeiTimeZone);
                var today = taipeiNow.Date;
                var isLastDayOfMonth = today.AddDays(1).Day == 1;
                var isMonthlyPerfect = false;

                if (isLastDayOfMonth)
                {
                    isMonthlyPerfect = await _signInService.IsMonthlyPerfectAttendanceAsync(userId, today.Year, today.Month);
                }

                var rewardConfig = _signInService.CalculateSignInReward(today, consecutiveDays, isMonthlyPerfect);

                return Ok(new
                {
                    canSignIn = true,
                    consecutiveDays = consecutiveDays,
                    isWeekend = rewardConfig.IsWeekend,
                    basePoints = rewardConfig.BasePoints,
                    baseExperience = rewardConfig.BaseExperience,
                    sevenDayBonus = consecutiveDays == 7 ? new
                    {
                        points = rewardConfig.SevenDayBonusPoints,
                        experience = rewardConfig.SevenDayBonusExperience
                    } : null,
                    monthlyPerfectBonus = isMonthlyPerfect ? new
                    {
                        points = rewardConfig.MonthlyPerfectBonusPoints,
                        experience = rewardConfig.MonthlyPerfectBonusExperience
                    } : null,
                    totalPoints = rewardConfig.TotalPoints,
                    totalExperience = rewardConfig.TotalExperience
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"預覽簽到獎勵時發生錯誤，使用者ID: {GetCurrentUserId()}");
                return StatusCode(500, new { message = "預覽失敗" });
            }
        }

        #region 私有方法

        /// <summary>
        /// 取得當前登入使用者的 ID
        /// </summary>
        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out var userId))
            {
                return userId;
            }
            return 0;
        }

        #endregion
    }
}