using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace GameCore.Web.Controllers
{
    /// <summary>
    /// MVC認證控制器
    /// 處理認證相關的頁面路由
    /// </summary>
    public class AuthMvcController : Controller
    {
        private readonly ILogger<AuthMvcController> _logger;

        public AuthMvcController(ILogger<AuthMvcController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 登入頁面
        /// </summary>
        /// <returns>登入頁面視圖</returns>
        [HttpGet]
        [Route("login")]
        [Route("Auth/Login")]
        public IActionResult Login()
        {
            // 檢查是否已經登入
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        /// <summary>
        /// 註冊頁面
        /// </summary>
        /// <returns>註冊頁面視圖</returns>
        [HttpGet]
        [Route("register")]
        [Route("Auth/Register")]
        public IActionResult Register()
        {
            // 檢查是否已經登入
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns>重定向到登入頁面</returns>
        [HttpGet]
        [Route("logout")]
        [Route("Auth/Logout")]
        [Authorize]
        public IActionResult Logout()
        {
            // 清除認證 Cookie
            HttpContext.Response.Cookies.Delete(".AspNetCore.Identity.Application");
            
            return RedirectToAction("Login");
        }

        /// <summary>
        /// 存取被拒絕頁面
        /// </summary>
        /// <returns>存取被拒絕頁面視圖</returns>
        [HttpGet]
        [Route("access-denied")]
        [Route("Auth/AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

        /// <summary>
        /// 忘記密碼頁面
        /// </summary>
        /// <returns>忘記密碼頁面視圖</returns>
        [HttpGet]
        [Route("forgot-password")]
        [Route("Auth/ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        /// <summary>
        /// 重設密碼頁面
        /// </summary>
        /// <param name="token">重設密碼Token</param>
        /// <returns>重設密碼頁面視圖</returns>
        [HttpGet]
        [Route("reset-password")]
        [Route("Auth/ResetPassword")]
        public IActionResult ResetPassword(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("ForgotPassword");
            }

            ViewBag.Token = token;
            return View();
        }

        /// <summary>
        /// 個人資料頁面
        /// </summary>
        /// <returns>個人資料頁面視圖</returns>
        [HttpGet]
        [Route("profile")]
        [Route("Auth/Profile")]
        [Authorize]
        public IActionResult Profile()
        {
            return View();
        }

        /// <summary>
        /// 變更密碼頁面
        /// </summary>
        /// <returns>變更密碼頁面視圖</returns>
        [HttpGet]
        [Route("change-password")]
        [Route("Auth/ChangePassword")]
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }
    }
} 