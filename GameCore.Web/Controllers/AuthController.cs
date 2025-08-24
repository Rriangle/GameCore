using GameCore.Core.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GameCore.Web.Controllers
{
    /// <summary>
    /// 身份驗證控制器
    /// 處理登入頁面顯示、OAuth 回調、以及身份驗證相關的 View 路由
    /// </summary>
    [Route("Auth")]
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _configuration;

        public AuthController(
            IUserService userService, 
            ILogger<AuthController> logger,
            IConfiguration configuration)
        {
            _userService = userService;
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// 顯示登入頁面
        /// </summary>
        /// <param name="returnUrl">登入成功後的回傳 URL</param>
        /// <returns>登入頁面 View</returns>
        [HttpGet("Login")]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            // 如果用戶已經登入，直接跳轉
            if (User.Identity?.IsAuthenticated == true)
            {
                return Redirect(returnUrl ?? "/");
            }

            ViewBag.ReturnUrl = returnUrl;
            ViewBag.Title = "登入 - GameCore";
            
            return View();
        }

        /// <summary>
        /// 顯示註冊頁面
        /// </summary>
        /// <returns>註冊頁面 View</returns>
        [HttpGet("Register")]
        [AllowAnonymous]
        public IActionResult Register()
        {
            // 如果用戶已經登入，直接跳轉到首頁
            if (User.Identity?.IsAuthenticated == true)
            {
                return Redirect("/");
            }

            ViewBag.Title = "註冊 - GameCore";
            
            return View();
        }

        /// <summary>
        /// 顯示忘記密碼頁面
        /// </summary>
        /// <returns>忘記密碼頁面 View</returns>
        [HttpGet("ForgotPassword")]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            ViewBag.Title = "忘記密碼 - GameCore";
            
            return View();
        }

        /// <summary>
        /// 顯示重設密碼頁面
        /// </summary>
        /// <param name="token">重設密碼令牌</param>
        /// <returns>重設密碼頁面 View</returns>
        [HttpGet("ResetPassword")]
        [AllowAnonymous]
        public IActionResult ResetPassword(string? token = null)
        {
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction(nameof(ForgotPassword));
            }

            ViewBag.Token = token;
            ViewBag.Title = "重設密碼 - GameCore";
            
            return View();
        }

        /// <summary>
        /// 處理登出
        /// </summary>
        /// <returns>跳轉到首頁</returns>
        [HttpPost("Logout")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            try
            {
                // 清除 Cookie 中的驗證資訊
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                
                // 清除所有 Cookie
                Response.Cookies.Delete("AuthToken");
                
                _logger.LogInformation("User logged out successfully");
                
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during logout");
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// 顯示存取被拒頁面
        /// </summary>
        /// <returns>存取被拒頁面 View</returns>
        [HttpGet("AccessDenied")]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            ViewBag.Title = "存取被拒 - GameCore";
            
            return View();
        }

        #region OAuth 相關端點

        /// <summary>
        /// 發起 Google OAuth 登入
        /// </summary>
        /// <param name="returnUrl">登入成功後的回傳 URL</param>
        /// <returns>跳轉到 Google OAuth</returns>
        [HttpGet("OAuth/Google")]
        [AllowAnonymous]
        public IActionResult GoogleLogin(string? returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(GoogleCallback), "Auth", new { returnUrl });
            var properties = new AuthenticationProperties 
            { 
                RedirectUri = redirectUrl,
                Items =
                {
                    { "returnUrl", returnUrl ?? "/" }
                }
            };
            
            return Challenge(properties, "Google");
        }

        /// <summary>
        /// Google OAuth 回調處理
        /// </summary>
        /// <param name="returnUrl">登入成功後的回傳 URL</param>
        /// <returns>跳轉到指定頁面或首頁</returns>
        [HttpGet("OAuth/Google/Callback")]
        [AllowAnonymous]
        public async Task<IActionResult> GoogleCallback(string? returnUrl = null)
        {
            try
            {
                var result = await HttpContext.AuthenticateAsync("Google");
                
                if (!result.Succeeded)
                {
                    TempData["Error"] = "Google 登入失敗，請稍後再試";
                    return RedirectToAction(nameof(Login));
                }

                var claims = result.Principal?.Claims;
                var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                var googleId = claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(googleId))
                {
                    TempData["Error"] = "無法從 Google 獲取必要資訊";
                    return RedirectToAction(nameof(Login));
                }

                // 嘗試使用 OAuth 資訊登入或註冊
                var loginResult = await _userService.OAuthLoginAsync("Google", googleId, email, name);
                
                if (loginResult.Success)
                {
                    // 建立本地身份驗證 Cookie
                    await CreateAuthenticationCookie(loginResult.User);
                    
                    _logger.LogInformation("User logged in successfully via Google OAuth: {Email}", email);
                    
                    return Redirect(returnUrl ?? "/");
                }
                else
                {
                    TempData["Error"] = loginResult.Message ?? "Google 登入處理失敗";
                    return RedirectToAction(nameof(Login));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during Google OAuth callback");
                TempData["Error"] = "Google 登入過程中發生錯誤";
                return RedirectToAction(nameof(Login));
            }
        }

        /// <summary>
        /// 發起 Facebook OAuth 登入
        /// </summary>
        /// <param name="returnUrl">登入成功後的回傳 URL</param>
        /// <returns>跳轉到 Facebook OAuth</returns>
        [HttpGet("OAuth/Facebook")]
        [AllowAnonymous]
        public IActionResult FacebookLogin(string? returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(FacebookCallback), "Auth", new { returnUrl });
            var properties = new AuthenticationProperties 
            { 
                RedirectUri = redirectUrl,
                Items =
                {
                    { "returnUrl", returnUrl ?? "/" }
                }
            };
            
            return Challenge(properties, "Facebook");
        }

        /// <summary>
        /// Facebook OAuth 回調處理
        /// </summary>
        /// <param name="returnUrl">登入成功後的回傳 URL</param>
        /// <returns>跳轉到指定頁面或首頁</returns>
        [HttpGet("OAuth/Facebook/Callback")]
        [AllowAnonymous]
        public async Task<IActionResult> FacebookCallback(string? returnUrl = null)
        {
            try
            {
                var result = await HttpContext.AuthenticateAsync("Facebook");
                
                if (!result.Succeeded)
                {
                    TempData["Error"] = "Facebook 登入失敗，請稍後再試";
                    return RedirectToAction(nameof(Login));
                }

                var claims = result.Principal?.Claims;
                var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                var facebookId = claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(facebookId))
                {
                    TempData["Error"] = "無法從 Facebook 獲取必要資訊";
                    return RedirectToAction(nameof(Login));
                }

                // 嘗試使用 OAuth 資訊登入或註冊
                var loginResult = await _userService.OAuthLoginAsync("Facebook", facebookId, email, name);
                
                if (loginResult.Success)
                {
                    // 建立本地身份驗證 Cookie
                    await CreateAuthenticationCookie(loginResult.User);
                    
                    _logger.LogInformation("User logged in successfully via Facebook OAuth: {Email}", email ?? "No Email");
                    
                    return Redirect(returnUrl ?? "/");
                }
                else
                {
                    TempData["Error"] = loginResult.Message ?? "Facebook 登入處理失敗";
                    return RedirectToAction(nameof(Login));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during Facebook OAuth callback");
                TempData["Error"] = "Facebook 登入過程中發生錯誤";
                return RedirectToAction(nameof(Login));
            }
        }

        /// <summary>
        /// 發起 Discord OAuth 登入 (模擬實作，實際需要配置 Discord OAuth)
        /// </summary>
        /// <param name="returnUrl">登入成功後的回傳 URL</param>
        /// <returns>跳轉到 Discord OAuth 或顯示未實作訊息</returns>
        [HttpGet("OAuth/Discord")]
        [AllowAnonymous]
        public IActionResult DiscordLogin(string? returnUrl = null)
        {
            // Discord OAuth 需要額外配置，這裡先返回未實作訊息
            TempData["Warning"] = "Discord 登入功能開發中，請使用其他方式登入";
            return RedirectToAction(nameof(Login), new { returnUrl });
        }

        #endregion

        #region API 端點供 JavaScript 使用

        /// <summary>
        /// 檢查使用者名稱或帳號唯一性
        /// </summary>
        /// <param name="request">檢查請求</param>
        /// <returns>唯一性檢查結果</returns>
        [HttpPost("api/check-unique")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckUnique([FromBody] CheckUniqueRequest request)
        {
            try
            {
                bool isUnique = false;
                
                switch (request.Field?.ToLower())
                {
                    case "username":
                        isUnique = !await _userService.ExistsByUsernameAsync(request.Value);
                        break;
                    case "useraccount":
                        isUnique = !await _userService.ExistsByAccountAsync(request.Value);
                        break;
                    case "email":
                        isUnique = !await _userService.ExistsByEmailAsync(request.Value);
                        break;
                    case "nickname":
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

        /// <summary>
        /// 發送忘記密碼郵件
        /// </summary>
        /// <param name="request">忘記密碼請求</param>
        /// <returns>處理結果</returns>
        [HttpPost("api/forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            try
            {
                var result = await _userService.SendPasswordResetEmailAsync(request.Email);
                
                // 為了安全考量，不管郵箱是否存在都返回成功訊息
                return Ok(new { message = "如果該郵箱存在於我們的系統中，您將收到密碼重設指示" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing forgot password request");
                return Ok(new { message = "如果該郵箱存在於我們的系統中，您將收到密碼重設指示" });
            }
        }

        /// <summary>
        /// 重設密碼
        /// </summary>
        /// <param name="request">重設密碼請求</param>
        /// <returns>處理結果</returns>
        [HttpPost("api/reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            try
            {
                var result = await _userService.ResetPasswordAsync(request.Token, request.NewPassword);
                
                if (result.Success)
                {
                    return Ok(new { message = "密碼重設成功，請使用新密碼登入" });
                }
                
                return BadRequest(new { message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while resetting password");
                return StatusCode(500, new { message = "密碼重設過程中發生錯誤" });
            }
        }

        #endregion

        #region 私有輔助方法

        /// <summary>
        /// 建立身份驗證 Cookie
        /// </summary>
        /// <param name="user">使用者資訊</param>
        private async Task CreateAuthenticationCookie(dynamic user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.User_ID.ToString()),
                new Claim("UserId", user.User_ID.ToString()),
                new Claim(ClaimTypes.Name, user.User_name ?? ""),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim("NickName", user.User_NickName ?? ""),
                new Claim("LoginProvider", "Local")
            };

            // 添加角色和權限 Claims
            if (user.IsAdmin == true)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }
            
            if (user.SalesAuthority == true)
            {
                claims.Add(new Claim("SalesAuthority", "true"));
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                claimsPrincipal,
                authProperties);
        }

        #endregion
    }

    #region 請求 DTO 類別

    /// <summary>
    /// 唯一性檢查請求
    /// </summary>
    public class CheckUniqueRequest
    {
        /// <summary>
        /// 要檢查的欄位名稱
        /// </summary>
        public string Field { get; set; } = string.Empty;
        
        /// <summary>
        /// 要檢查的值
        /// </summary>
        public string Value { get; set; } = string.Empty;
    }

    /// <summary>
    /// 忘記密碼請求
    /// </summary>
    public class ForgotPasswordRequest
    {
        /// <summary>
        /// 電子郵件地址
        /// </summary>
        public string Email { get; set; } = string.Empty;
    }

    /// <summary>
    /// 重設密碼請求
    /// </summary>
    public class ResetPasswordRequest
    {
        /// <summary>
        /// 重設密碼令牌
        /// </summary>
        public string Token { get; set; } = string.Empty;
        
        /// <summary>
        /// 新密碼
        /// </summary>
        public string NewPassword { get; set; } = string.Empty;
    }

    #endregion
}