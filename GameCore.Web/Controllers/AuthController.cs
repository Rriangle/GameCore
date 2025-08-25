using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GameCore.Core.Interfaces;
using GameCore.Core.Models;
using GameCore.Core.Models.AuthDtos;
using System.Security.Claims;

namespace GameCore.Web.Controllers
{
    /// <summary>
    /// 認證控制器 - 處理用戶登入、註冊、第三方登入等認證相關功能
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUserService userService, ILogger<AuthController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// 用戶註冊
        /// </summary>
        /// <param name="request">註冊請求</param>
        /// <returns>註冊結果</returns>
        [HttpPost("register")]
        public async Task<ActionResult<ServiceResult<UserRegistrationResultDto>>> Register([FromBody] UserRegistrationDto request)
        {
            try
            {
                _logger.LogInformation("用戶註冊請求: {Email}", request.Email);
                
                var result = await _userService.RegisterAsync(request);
                
                if (result.Success)
                {
                    _logger.LogInformation("用戶註冊成功: {Email}", request.Email);
                    return Ok(result);
                }
                
                _logger.LogWarning("用戶註冊失敗: {Email}, 錯誤: {Message}", request.Email, result.Message);
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "用戶註冊時發生錯誤: {Email}", request.Email);
                return StatusCode(500, ServiceResult<UserRegistrationResultDto>.FailureResult("註冊過程中發生內部錯誤"));
            }
        }

        /// <summary>
        /// 用戶登入
        /// </summary>
        /// <param name="request">登入請求</param>
        /// <returns>登入結果</returns>
        [HttpPost("login")]
        public async Task<ActionResult<ServiceResult<UserLoginResultDto>>> Login([FromBody] UserLoginDto request)
        {
            try
            {
                _logger.LogInformation("用戶登入請求: {Email}", request.Email);
                
                var result = await _userService.LoginAsync(request);
                
                if (result.Success)
                {
                    _logger.LogInformation("用戶登入成功: {Email}", request.Email);
                    return Ok(result);
                }
                
                _logger.LogWarning("用戶登入失敗: {Email}, 錯誤: {Message}", request.Email, result.Message);
                return Unauthorized(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "用戶登入時發生錯誤: {Email}", request.Email);
                return StatusCode(500, ServiceResult<UserLoginResultDto>.FailureResult("登入過程中發生內部錯誤"));
            }
        }

        /// <summary>
        /// 第三方登入
        /// </summary>
        /// <param name="request">第三方登入請求</param>
        /// <returns>登入結果</returns>
        [HttpPost("third-party-login")]
        public async Task<ActionResult<ServiceResult<UserLoginResultDto>>> ThirdPartyLogin([FromBody] ThirdPartyLoginDto request)
        {
            try
            {
                _logger.LogInformation("第三方登入請求: {Provider} - {ProviderId}", request.Provider, request.ProviderId);
                
                var result = await _userService.ThirdPartyLoginAsync(request);
                
                if (result.Success)
                {
                    _logger.LogInformation("第三方登入成功: {Provider} - {ProviderId}", request.Provider, request.ProviderId);
                    return Ok(result);
                }
                
                _logger.LogWarning("第三方登入失敗: {Provider} - {ProviderId}, 錯誤: {Message}", request.Provider, request.ProviderId, result.Message);
                return Unauthorized(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "第三方登入時發生錯誤: {Provider} - {ProviderId}", request.Provider, request.ProviderId);
                return StatusCode(500, ServiceResult<UserLoginResultDto>.FailureResult("第三方登入過程中發生內部錯誤"));
            }
        }

        /// <summary>
        /// 忘記密碼
        /// </summary>
        /// <param name="request">忘記密碼請求</param>
        /// <returns>處理結果</returns>
        [HttpPost("forgot-password")]
        public async Task<ActionResult<ServiceResult<object>>> ForgotPassword([FromBody] ForgotPasswordRequestDto request)
        {
            try
            {
                _logger.LogInformation("忘記密碼請求: {Email}", request.Email);
                
                var result = await _userService.ForgotPasswordAsync(request.Email);
                
                if (result.Success)
                {
                    _logger.LogInformation("忘記密碼處理成功: {Email}", request.Email);
                    return Ok(result);
                }
                
                _logger.LogWarning("忘記密碼處理失敗: {Email}, 錯誤: {Message}", request.Email, result.Message);
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "忘記密碼處理時發生錯誤: {Email}", request.Email);
                return StatusCode(500, ServiceResult<object>.FailureResult("忘記密碼處理過程中發生內部錯誤"));
            }
        }

        /// <summary>
        /// 重設密碼
        /// </summary>
        /// <param name="request">重設密碼請求</param>
        /// <returns>處理結果</returns>
        [HttpPost("reset-password")]
        public async Task<ActionResult<ServiceResult<object>>> ResetPassword([FromBody] ResetPasswordRequestDto request)
        {
            try
            {
                _logger.LogInformation("重設密碼請求: {Email}", request.Email);
                
                var result = await _userService.ResetPasswordAsync(request);
                
                if (result.Success)
                {
                    _logger.LogInformation("重設密碼成功: {Email}", request.Email);
                    return Ok(result);
                }
                
                _logger.LogWarning("重設密碼失敗: {Email}, 錯誤: {Message}", request.Email, result.Message);
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "重設密碼時發生錯誤: {Email}", request.Email);
                return StatusCode(500, ServiceResult<object>.FailureResult("重設密碼過程中發生內部錯誤"));
            }
        }

        /// <summary>
        /// 驗證 Token
        /// </summary>
        /// <returns>驗證結果</returns>
        [HttpPost("validate-token")]
        [Authorize]
        public async Task<ActionResult<ServiceResult<TokenValidationResultDto>>> ValidateToken()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ServiceResult<TokenValidationResultDto>.UnauthorizedResult("無效的認證資訊"));
                }

                _logger.LogInformation("Token 驗證請求: {UserId}", userId);
                
                var result = await _userService.ValidateTokenAsync(userId);
                
                if (result.Success)
                {
                    _logger.LogInformation("Token 驗證成功: {UserId}", userId);
                    return Ok(result);
                }
                
                _logger.LogWarning("Token 驗證失敗: {UserId}, 錯誤: {Message}", userId, result.Message);
                return Unauthorized(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Token 驗證時發生錯誤");
                return StatusCode(500, ServiceResult<TokenValidationResultDto>.FailureResult("Token 驗證過程中發生內部錯誤"));
            }
        }

        /// <summary>
        /// 重新整理 Token
        /// </summary>
        /// <param name="request">重新整理 Token 請求</param>
        /// <returns>重新整理結果</returns>
        [HttpPost("refresh-token")]
        public async Task<ActionResult<ServiceResult<TokenRefreshResultDto>>> RefreshToken([FromBody] TokenRefreshRequestDto request)
        {
            try
            {
                _logger.LogInformation("重新整理 Token 請求");
                
                var result = await _userService.RefreshTokenAsync(request.RefreshToken);
                
                if (result.Success)
                {
                    _logger.LogInformation("重新整理 Token 成功");
                    return Ok(result);
                }
                
                _logger.LogWarning("重新整理 Token 失敗: {Message}", result.Message);
                return Unauthorized(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "重新整理 Token 時發生錯誤");
                return StatusCode(500, ServiceResult<TokenRefreshResultDto>.FailureResult("重新整理 Token 過程中發生內部錯誤"));
            }
        }

        /// <summary>
        /// 用戶登出
        /// </summary>
        /// <returns>登出結果</returns>
        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult<ServiceResult<object>>> Logout()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ServiceResult<object>.UnauthorizedResult("無效的認證資訊"));
                }

                _logger.LogInformation("用戶登出請求: {UserId}", userId);
                
                // 這裡可以添加登出邏輯，例如將 Token 加入黑名單
                var result = ServiceResult<object>.SuccessResult("登出成功");
                
                _logger.LogInformation("用戶登出成功: {UserId}", userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "用戶登出時發生錯誤");
                return StatusCode(500, ServiceResult<object>.FailureResult("登出過程中發生內部錯誤"));
            }
        }

        /// <summary>
        /// 獲取當前用戶資訊
        /// </summary>
        /// <returns>用戶資訊</returns>
        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<ServiceResult<UserProfileDto>>> GetCurrentUser()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ServiceResult<UserProfileDto>.UnauthorizedResult("無效的認證資訊"));
                }

                _logger.LogInformation("獲取當前用戶資訊請求: {UserId}", userId);
                
                var result = await _userService.GetProfileAsync(int.Parse(userId));
                
                if (result.Success)
                {
                    _logger.LogInformation("獲取當前用戶資訊成功: {UserId}", userId);
                    return Ok(result);
                }
                
                _logger.LogWarning("獲取當前用戶資訊失敗: {UserId}, 錯誤: {Message}", userId, result.Message);
                return NotFound(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取當前用戶資訊時發生錯誤");
                return StatusCode(500, ServiceResult<UserProfileDto>.FailureResult("獲取用戶資訊過程中發生內部錯誤"));
            }
        }
    }
}