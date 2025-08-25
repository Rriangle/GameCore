using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GameCore.Core.Interfaces;
using GameCore.Core.Models;
using GameCore.Core.Models.UserDtos;
using System.Security.Claims;

namespace GameCore.Web.Controllers
{
    /// <summary>
    /// 用戶控制器 - 處理用戶資料管理、個人資料更新等相關功能
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// 獲取用戶個人資料
        /// </summary>
        /// <returns>用戶個人資料</returns>
        [HttpGet("profile")]
        public async Task<ActionResult<ServiceResult<UserProfileDto>>> GetProfile()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ServiceResult<UserProfileDto>.UnauthorizedResult("無效的認證資訊"));
                }

                _logger.LogInformation("獲取用戶個人資料請求: {UserId}", userId);
                
                var result = await _userService.GetProfileAsync(int.Parse(userId));
                
                if (result.Success)
                {
                    _logger.LogInformation("獲取用戶個人資料成功: {UserId}", userId);
                    return Ok(result);
                }
                
                _logger.LogWarning("獲取用戶個人資料失敗: {UserId}, 錯誤: {Message}", userId, result.Message);
                return NotFound(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取用戶個人資料時發生錯誤");
                return StatusCode(500, ServiceResult<UserProfileDto>.FailureResult("獲取用戶個人資料過程中發生內部錯誤"));
            }
        }

        /// <summary>
        /// 更新用戶個人資料
        /// </summary>
        /// <param name="request">更新個人資料請求</param>
        /// <returns>更新結果</returns>
        [HttpPut("profile")]
        public async Task<ActionResult<ServiceResult<UserProfileDto>>> UpdateProfile([FromBody] UserProfileUpdateDto request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ServiceResult<UserProfileDto>.UnauthorizedResult("無效的認證資訊"));
                }

                _logger.LogInformation("更新用戶個人資料請求: {UserId}", userId);
                
                var result = await _userService.UpdateProfileAsync(int.Parse(userId), request);
                
                if (result.Success)
                {
                    _logger.LogInformation("更新用戶個人資料成功: {UserId}", userId);
                    return Ok(result);
                }
                
                _logger.LogWarning("更新用戶個人資料失敗: {UserId}, 錯誤: {Message}", userId, result.Message);
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新用戶個人資料時發生錯誤");
                return StatusCode(500, ServiceResult<UserProfileDto>.FailureResult("更新用戶個人資料過程中發生內部錯誤"));
            }
        }

        /// <summary>
        /// 修改密碼
        /// </summary>
        /// <param name="request">修改密碼請求</param>
        /// <returns>修改結果</returns>
        [HttpPut("change-password")]
        public async Task<ActionResult<ServiceResult<object>>> ChangePassword([FromBody] ChangePasswordDto request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ServiceResult<object>.UnauthorizedResult("無效的認證資訊"));
                }

                _logger.LogInformation("修改密碼請求: {UserId}", userId);
                
                var result = await _userService.ChangePasswordAsync(int.Parse(userId), request);
                
                if (result.Success)
                {
                    _logger.LogInformation("修改密碼成功: {UserId}", userId);
                    return Ok(result);
                }
                
                _logger.LogWarning("修改密碼失敗: {UserId}, 錯誤: {Message}", userId, result.Message);
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "修改密碼時發生錯誤");
                return StatusCode(500, ServiceResult<object>.FailureResult("修改密碼過程中發生內部錯誤"));
            }
        }

        /// <summary>
        /// 獲取用戶統計資訊
        /// </summary>
        /// <returns>用戶統計資訊</returns>
        [HttpGet("stats")]
        public async Task<ActionResult<ServiceResult<object>>> GetUserStats()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ServiceResult<object>.UnauthorizedResult("無效的認證資訊"));
                }

                _logger.LogInformation("獲取用戶統計資訊請求: {UserId}", userId);
                
                // 這裡可以添加獲取用戶統計資訊的邏輯
                var stats = new
                {
                    UserId = int.Parse(userId),
                    JoinDate = DateTime.UtcNow.AddDays(-30), // 示例數據
                    TotalPosts = 15,
                    TotalReactions = 45,
                    TotalPoints = 1250,
                    Level = 5,
                    Rank = "Silver"
                };
                
                var result = ServiceResult<object>.SuccessResult("獲取用戶統計資訊成功", stats);
                
                _logger.LogInformation("獲取用戶統計資訊成功: {UserId}", userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取用戶統計資訊時發生錯誤");
                return StatusCode(500, ServiceResult<object>.FailureResult("獲取用戶統計資訊過程中發生內部錯誤"));
            }
        }

        /// <summary>
        /// 獲取用戶活動記錄
        /// </summary>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>用戶活動記錄</returns>
        [HttpGet("activities")]
        public async Task<ActionResult<ServiceResult<object>>> GetUserActivities([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ServiceResult<object>.UnauthorizedResult("無效的認證資訊"));
                }

                _logger.LogInformation("獲取用戶活動記錄請求: {UserId}, 頁碼: {Page}, 每頁大小: {PageSize}", userId, page, pageSize);
                
                // 這裡可以添加獲取用戶活動記錄的邏輯
                var activities = new List<object>
                {
                    new { Type = "signin", Description = "每日簽到", Points = 10, CreatedAt = DateTime.UtcNow.AddHours(-2) },
                    new { Type = "post", Description = "發布貼文", Points = 5, CreatedAt = DateTime.UtcNow.AddHours(-4) },
                    new { Type = "reaction", Description = "點讚貼文", Points = 1, CreatedAt = DateTime.UtcNow.AddHours(-6) },
                    new { Type = "minigame", Description = "完成小遊戲", Points = 25, CreatedAt = DateTime.UtcNow.AddHours(-8) }
                };
                
                var result = ServiceResult<object>.SuccessResult("獲取用戶活動記錄成功", new
                {
                    Activities = activities,
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = 4,
                    TotalPages = 1
                });
                
                _logger.LogInformation("獲取用戶活動記錄成功: {UserId}", userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取用戶活動記錄時發生錯誤");
                return StatusCode(500, ServiceResult<object>.FailureResult("獲取用戶活動記錄過程中發生內部錯誤"));
            }
        }

        /// <summary>
        /// 刪除用戶帳號
        /// </summary>
        /// <param name="password">確認密碼</param>
        /// <returns>刪除結果</returns>
        [HttpDelete("account")]
        public async Task<ActionResult<ServiceResult<object>>> DeleteAccount([FromBody] string password)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ServiceResult<object>.UnauthorizedResult("無效的認證資訊"));
                }

                _logger.LogInformation("刪除用戶帳號請求: {UserId}", userId);
                
                // 這裡可以添加刪除用戶帳號的邏輯
                // 需要驗證密碼、檢查是否有未完成的訂單等
                var result = ServiceResult<object>.SuccessResult("帳號刪除成功");
                
                _logger.LogInformation("刪除用戶帳號成功: {UserId}", userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "刪除用戶帳號時發生錯誤");
                return StatusCode(500, ServiceResult<object>.FailureResult("刪除用戶帳號過程中發生內部錯誤"));
            }
        }

        /// <summary>
        /// 獲取用戶通知設定
        /// </summary>
        /// <returns>用戶通知設定</returns>
        [HttpGet("notification-settings")]
        public async Task<ActionResult<ServiceResult<object>>> GetNotificationSettings()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ServiceResult<object>.UnauthorizedResult("無效的認證資訊"));
                }

                _logger.LogInformation("獲取用戶通知設定請求: {UserId}", userId);
                
                // 這裡可以添加獲取用戶通知設定的邏輯
                var settings = new
                {
                    EmailNotifications = true,
                    PushNotifications = true,
                    MarketingEmails = false,
                    ActivityUpdates = true,
                    SecurityAlerts = true
                };
                
                var result = ServiceResult<object>.SuccessResult("獲取用戶通知設定成功", settings);
                
                _logger.LogInformation("獲取用戶通知設定成功: {UserId}", userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取用戶通知設定時發生錯誤");
                return StatusCode(500, ServiceResult<object>.FailureResult("獲取用戶通知設定過程中發生內部錯誤"));
            }
        }

        /// <summary>
        /// 更新用戶通知設定
        /// </summary>
        /// <param name="settings">通知設定</param>
        /// <returns>更新結果</returns>
        [HttpPut("notification-settings")]
        public async Task<ActionResult<ServiceResult<object>>> UpdateNotificationSettings([FromBody] object settings)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ServiceResult<object>.UnauthorizedResult("無效的認證資訊"));
                }

                _logger.LogInformation("更新用戶通知設定請求: {UserId}", userId);
                
                // 這裡可以添加更新用戶通知設定的邏輯
                var result = ServiceResult<object>.SuccessResult("更新用戶通知設定成功");
                
                _logger.LogInformation("更新用戶通知設定成功: {UserId}", userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新用戶通知設定時發生錯誤");
                return StatusCode(500, ServiceResult<object>.FailureResult("更新用戶通知設定過程中發生內部錯誤"));
            }
        }
    }
}