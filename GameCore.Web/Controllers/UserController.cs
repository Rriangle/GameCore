using GameCore.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameCore.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto registerDto)
        {
            try
            {
                var result = await _userService.RegisterAsync(registerDto);
                
                if (result.Success)
                {
                    return Ok(new { message = "註冊成功", user = result.User });
                }
                
                return BadRequest(new { message = result.Message, errors = result.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during user registration");
                return StatusCode(500, new { message = "伺服器錯誤，請稍後再試" });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
        {
            try
            {
                var result = await _userService.LoginAsync(loginDto);
                
                if (result.Success)
                {
                    // Set authentication cookie
                    Response.Cookies.Append("AuthToken", result.Token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTime.UtcNow.AddDays(7)
                    });
                    
                    return Ok(new { message = "登入成功", user = result.User });
                }
                
                return Unauthorized(new { message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during user login");
                return StatusCode(500, new { message = "伺服器錯誤，請稍後再試" });
            }
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
                await _userService.LogoutAsync(userId);
                
                Response.Cookies.Delete("AuthToken");
                
                return Ok(new { message = "登出成功" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during user logout");
                return StatusCode(500, new { message = "伺服器錯誤，請稍後再試" });
            }
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
                var result = await _userService.GetProfileAsync(userId);
                
                if (result.Success)
                {
                    return Ok(result.User);
                }
                
                return NotFound(new { message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting user profile");
                return StatusCode(500, new { message = "伺服器錯誤，請稍後再試" });
            }
        }

        [HttpPut("profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UserUpdateDto updateDto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
                var result = await _userService.UpdateProfileAsync(userId, updateDto);
                
                if (result.Success)
                {
                    return Ok(new { message = "資料更新成功", user = result.User });
                }
                
                return BadRequest(new { message = result.Message, errors = result.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating user profile");
                return StatusCode(500, new { message = "伺服器錯誤，請稍後再試" });
            }
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] PasswordChangeDto passwordDto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
                var result = await _userService.ChangePasswordAsync(userId, passwordDto);
                
                if (result.Success)
                {
                    return Ok(new { message = "密碼變更成功" });
                }
                
                return BadRequest(new { message = result.Message, errors = result.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while changing password");
                return StatusCode(500, new { message = "伺服器錯誤，請稍後再試" });
            }
        }

        [HttpGet("check-username/{username}")]
        public async Task<IActionResult> CheckUsername(string username)
        {
            try
            {
                var exists = await _userService.ExistsByUsernameAsync(username);
                return Ok(new { exists, available = !exists });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking username availability");
                return StatusCode(500, new { message = "伺服器錯誤，請稍後再試" });
            }
        }

        [HttpGet("check-email/{email}")]
        public async Task<IActionResult> CheckEmail(string email)
        {
            try
            {
                var exists = await _userService.ExistsByEmailAsync(email);
                return Ok(new { exists, available = !exists });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking email availability");
                return StatusCode(500, new { message = "伺服器錯誤，請稍後再試" });
            }
        }

        [HttpPost("check-unique")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckUnique([FromBody] CheckUniqueRequest request)
        {
            try
            {
                bool isUnique = false;
                
                switch (request.Field?.ToLower())
                {
                    case "username":
                    case "user_name":
                        isUnique = !await _userService.ExistsByUsernameAsync(request.Value);
                        break;
                    case "useraccount":
                    case "user_account":
                        isUnique = !await _userService.ExistsByAccountAsync(request.Value);
                        break;
                    case "email":
                        isUnique = !await _userService.ExistsByEmailAsync(request.Value);
                        break;
                    case "nickname":
                    case "user_nickname":
                        isUnique = !await _userService.ExistsByNicknameAsync(request.Value);
                        break;
                    default:
                        return BadRequest(new { message = "不支援的檢查欄位" });
                }
                
                return Ok(new { isUnique });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking uniqueness for {Field}: {Value}", 
                    request.Field, request.Value);
                return StatusCode(500, new { message = "檢查過程中發生錯誤" });
            }
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
                var result = await _userService.GetProfileAsync(userId);
                
                if (result.Success)
                {
                    return Ok(new { success = true, user = result.User });
                }
                
                return NotFound(new { success = false, message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting current user info");
                return StatusCode(500, new { success = false, message = "伺服器錯誤，請稍後再試" });
            }
        }

        [HttpGet("leaderboard")]
        public async Task<IActionResult> GetLeaderboard([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                // This would need implementation in UserService
                // For now, return a placeholder response
                return Ok(new { message = "排行榜功能開發中" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting leaderboard");
                return StatusCode(500, new { message = "伺服器錯誤，請稍後再試" });
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchUsers([FromQuery] string searchTerm, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                // This would need implementation in UserService
                // For now, return a placeholder response
                return Ok(new { message = "搜尋功能開發中" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching users");
                return StatusCode(500, new { message = "伺服器錯誤，請稍後再試" });
            }
        }

        [HttpPost("follow/{targetUserId}")]
        [Authorize]
        public async Task<IActionResult> FollowUser(int targetUserId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
                
                if (userId == targetUserId)
                {
                    return BadRequest(new { message = "無法追蹤自己" });
                }
                
                // This would need implementation in UserService
                // For now, return a placeholder response
                return Ok(new { message = "追蹤功能開發中" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while following user");
                return StatusCode(500, new { message = "伺服器錯誤，請稍後再試" });
            }
        }

        [HttpDelete("follow/{targetUserId}")]
        [Authorize]
        public async Task<IActionResult> UnfollowUser(int targetUserId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
                
                // This would need implementation in UserService
                // For now, return a placeholder response
                return Ok(new { message = "取消追蹤功能開發中" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while unfollowing user");
                return StatusCode(500, new { message = "伺服器錯誤，請稍後再試" });
            }
        }

        [HttpGet("stats")]
        [Authorize]
        public async Task<IActionResult> GetUserStats()
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
                
                // This would need implementation in UserService
                // For now, return a placeholder response
                var stats = new
                {
                    totalUsers = 1000,
                    activeUsers = 250,
                    newUsersToday = 15,
                    userLevel = 5,
                    totalPoints = 2500,
                    rank = 42
                };
                
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting user stats");
                return StatusCode(500, new { message = "伺服器錯誤，請稍後再試" });
            }
        }
    }
}