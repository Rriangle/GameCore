using GameCore.Application.DTOs;
using GameCore.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameCore.Web.Controllers
{
    /// <summary>
    /// 認證控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        
        /// <summary>
        /// 用戶登入
        /// </summary>
        /// <param name="loginDto">登入資料</param>
        /// <param name="cancellationToken">取消權杖</param>
        /// <returns>登入結果</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _authService.LoginAsync(loginDto.Username, loginDto.Password, cancellationToken);
                if (result.IsSuccess)
                {
                    return Ok(result.Data);
                }
                return BadRequest(result.ErrorMessage);
            }
            catch (NotImplementedException)
            {
                return StatusCode(501, new { Message = "登入功能尚未實作" });
            }
        }
        
        /// <summary>
        /// 用戶註冊
        /// </summary>
        /// <param name="registerDto">註冊資料</param>
        /// <param name="cancellationToken">取消權杖</param>
        /// <returns>註冊結果</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _authService.RegisterAsync(registerDto, cancellationToken);
                if (result.IsSuccess)
                {
                    return Ok(result.Data);
                }
                return BadRequest(result.ErrorMessage);
            }
            catch (NotImplementedException)
            {
                return StatusCode(501, new { Message = "註冊功能尚未實作" });
            }
        }
        
        /// <summary>
        /// 用戶登出
        /// </summary>
        /// <param name="cancellationToken">取消權杖</param>
        /// <returns>登出結果</returns>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout(CancellationToken cancellationToken = default)
        {
            try
            {
                // 這裡應該從Token中取得用戶ID
                var userId = 1; // 暫時寫死
                var result = await _authService.LogoutAsync(userId, cancellationToken);
                if (result.IsSuccess)
                {
                    return Ok(new { Message = "登出成功" });
                }
                return BadRequest(result.ErrorMessage);
            }
            catch (NotImplementedException)
            {
                return StatusCode(501, new { Message = "登出功能尚未實作" });
            }
        }
        
        /// <summary>
        /// 重新整理Token
        /// </summary>
        /// <param name="refreshToken">重新整理Token</param>
        /// <param name="cancellationToken">取消權杖</param>
        /// <returns>重新整理結果</returns>
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _authService.RefreshTokenAsync(refreshToken, cancellationToken);
                if (result.IsSuccess)
                {
                    return Ok(result.Data);
                }
                return BadRequest(result.ErrorMessage);
            }
            catch (NotImplementedException)
            {
                return StatusCode(501, new { Message = "Token重新整理功能尚未實作" });
            }
        }
        
        /// <summary>
        /// 檢查帳號是否存在
        /// </summary>
        /// <param name="account">帳號</param>
        /// <param name="cancellationToken">取消權杖</param>
        /// <returns>檢查結果</returns>
        [HttpGet("check-account/{account}")]
        public async Task<IActionResult> CheckAccount(string account, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _authService.IsAccountExistsAsync(account, cancellationToken);
                if (result.IsSuccess)
                {
                    return Ok(new { Exists = result.Data });
                }
                return BadRequest(result.ErrorMessage);
            }
            catch (NotImplementedException)
            {
                return StatusCode(501, new { Message = "帳號檢查功能尚未實作" });
            }
        }
        
        /// <summary>
        /// 檢查電子郵件是否存在
        /// </summary>
        /// <param name="email">電子郵件</param>
        /// <param name="cancellationToken">取消權杖</param>
        /// <returns>檢查結果</returns>
        [HttpGet("check-email/{email}")]
        public async Task<IActionResult> CheckEmail(string email, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _authService.IsEmailExistsAsync(email, cancellationToken);
                if (result.IsSuccess)
                {
                    return Ok(new { Exists = result.Data });
                }
                return BadRequest(result.ErrorMessage);
            }
            catch (NotImplementedException)
            {
                return StatusCode(501, new { Message = "電子郵件檢查功能尚未實作" });
            }
        }
    }
} 