using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using GameCore.Core.Interfaces;
using GameCore.Core.Entities;
using BCrypt.Net;

namespace GameCore.Web.Controllers
{
    /// <summary>
    /// 帳號管理控制器
    /// 處理使用者註冊、登入、登出等認證相關功能
    /// </summary>
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IUnitOfWork unitOfWork, ILogger<AccountController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <summary>
        /// 顯示登入頁面
        /// </summary>
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// 處理登入請求
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // 嘗試根據帳號或 Email 查找使用者
                User? user = null;
                
                if (model.Account.Contains("@"))
                {
                    user = await _unitOfWork.UserRepository.GetByEmailAsync(model.Account);
                }
                else
                {
                    user = await _unitOfWork.UserRepository.GetByAccountAsync(model.Account);
                }

                // 驗證使用者和密碼
                if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.UserPassword))
                {
                    ModelState.AddModelError(string.Empty, "帳號或密碼不正確");
                    return View(model);
                }

                // 檢查使用者狀態
                var userRights = await _unitOfWork.UserRepository.GetUserRightsAsync(user.UserId);
                if (userRights == null || !userRights.UserStatus)
                {
                    ModelState.AddModelError(string.Empty, "帳號已被停用，請聯繫客服");
                    return View(model);
                }

                // 建立身份聲明
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName ?? ""),
                    new Claim(ClaimTypes.Email, user.UserIntroduce?.Email ?? ""),
                    new Claim("Account", user.UserAccount ?? ""),
                    new Claim("Nickname", user.UserIntroduce?.UserNickName ?? ""),
                    new Claim("ShoppingPermission", userRights.ShoppingPermission.ToString()),
                    new Claim("MessagePermission", userRights.MessagePermission.ToString()),
                    new Claim("SalesAuthority", userRights.SalesAuthority.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe,
                    ExpiresUtc = model.RememberMe ? DateTimeOffset.UtcNow.AddDays(30) : DateTimeOffset.UtcNow.AddMinutes(30)
                };

                // 執行登入
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
                    new ClaimsPrincipal(claimsIdentity), authProperties);

                _logger.LogInformation($"使用者 {user.UserAccount} 登入成功");

                // 重定向到指定頁面或首頁
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "登入時發生錯誤");
                ModelState.AddModelError(string.Empty, "登入時發生錯誤，請稍後再試");
                return View(model);
            }
        }

        /// <summary>
        /// 顯示註冊頁面
        /// </summary>
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// 處理註冊請求
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // 檢查帳號是否已存在
                if (await _unitOfWork.UserRepository.AccountExistsAsync(model.UserAccount))
                {
                    ModelState.AddModelError("UserAccount", "此帳號已被使用");
                    return View(model);
                }

                // 檢查 Email 是否已存在
                if (await _unitOfWork.UserRepository.EmailExistsAsync(model.Email))
                {
                    ModelState.AddModelError("Email", "此 Email 已被註冊");
                    return View(model);
                }

                // 檢查暱稱是否已存在
                if (await _unitOfWork.UserRepository.NicknameExistsAsync(model.UserNickName))
                {
                    ModelState.AddModelError("UserNickName", "此暱稱已被使用");
                    return View(model);
                }

                await _unitOfWork.BeginTransactionAsync();

                try
                {
                    // 建立使用者基本資料
                    var user = new User
                    {
                        UserName = model.UserName,
                        UserAccount = model.UserAccount,
                        UserPassword = BCrypt.Net.BCrypt.HashPassword(model.Password)
                    };

                    await _unitOfWork.UserRepository.AddAsync(user);
                    await _unitOfWork.SaveChangesAsync(); // 需要先儲存才能取得 UserId

                    // 建立使用者介紹資料
                    var userIntroduce = new UserIntroduce
                    {
                        UserId = user.UserId,
                        UserNickName = model.UserNickName,
                        Gender = model.Gender,
                        IdNumber = model.IdNumber,
                        Cellphone = model.Cellphone,
                        Email = model.Email,
                        Address = model.Address,
                        DateOfBirth = model.DateOfBirth,
                        CreateAccount = DateTime.UtcNow
                    };

                    await _unitOfWork.UserRepository.AddAsync(userIntroduce);

                    // 建立使用者權限 (預設值)
                    var userRights = new UserRights
                    {
                        UserId = user.UserId,
                        UserStatus = true,         // 啟用狀態
                        ShoppingPermission = true, // 預設允許購物
                        MessagePermission = true,  // 預設允許留言
                        SalesAuthority = false     // 預設不開放銷售權限
                    };

                    await _unitOfWork.UserRepository.AddAsync(userRights);

                    // 建立使用者錢包 (初始點數為 0)
                    var userWallet = new UserWallet
                    {
                        UserId = user.UserId,
                        UserPoint = 100 // 新用戶贈送 100 點數
                    };

                    await _unitOfWork.UserRepository.AddAsync(userWallet);

                    await _unitOfWork.SaveChangesAsync();
                    await _unitOfWork.CommitTransactionAsync();

                    _logger.LogInformation($"新使用者註冊成功: {model.UserAccount}");

                    // 自動登入
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Email, model.Email),
                        new Claim("Account", user.UserAccount),
                        new Claim("Nickname", model.UserNickName),
                        new Claim("ShoppingPermission", "True"),
                        new Claim("MessagePermission", "True"),
                        new Claim("SalesAuthority", "False")
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
                        new ClaimsPrincipal(claimsIdentity));

                    TempData["SuccessMessage"] = "註冊成功！歡迎加入 GameCore 社群！";
                    return RedirectToAction("Index", "Home");
                }
                catch
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "註冊時發生錯誤");
                ModelState.AddModelError(string.Empty, "註冊時發生錯誤，請稍後再試");
                return View(model);
            }
        }

        /// <summary>
        /// 登出
        /// </summary>
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            
            _logger.LogInformation($"使用者登出");
            
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// 訪問被拒絕頁面
        /// </summary>
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        /// <summary>
        /// Google OAuth 登入回調
        /// </summary>
        [AllowAnonymous]
        public async Task<IActionResult> GoogleCallback()
        {
            try
            {
                var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
                
                if (!result.Succeeded)
                {
                    TempData["ErrorMessage"] = "Google 登入失敗";
                    return RedirectToAction("Login");
                }

                var email = result.Principal?.FindFirst(ClaimTypes.Email)?.Value;
                var name = result.Principal?.FindFirst(ClaimTypes.Name)?.Value;
                
                if (string.IsNullOrEmpty(email))
                {
                    TempData["ErrorMessage"] = "無法取得 Google 帳號資訊";
                    return RedirectToAction("Login");
                }

                // 檢查是否已有此 Email 的帳號
                var existingUser = await _unitOfWork.UserRepository.GetByEmailAsync(email);
                
                if (existingUser != null)
                {
                    // 現有用戶直接登入
                    await SignInExistingUser(existingUser);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // 新用戶需要完成註冊資訊
                    TempData["GoogleEmail"] = email;
                    TempData["GoogleName"] = name;
                    TempData["InfoMessage"] = "請完善您的註冊資訊";
                    return RedirectToAction("CompleteRegistration");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Google OAuth 回調時發生錯誤");
                TempData["ErrorMessage"] = "登入時發生錯誤";
                return RedirectToAction("Login");
            }
        }

        /// <summary>
        /// Facebook OAuth 登入回調
        /// </summary>
        [AllowAnonymous]
        public async Task<IActionResult> FacebookCallback()
        {
            try
            {
                var result = await HttpContext.AuthenticateAsync(FacebookDefaults.AuthenticationScheme);
                
                if (!result.Succeeded)
                {
                    TempData["ErrorMessage"] = "Facebook 登入失敗";
                    return RedirectToAction("Login");
                }

                var email = result.Principal?.FindFirst(ClaimTypes.Email)?.Value;
                var name = result.Principal?.FindFirst(ClaimTypes.Name)?.Value;
                
                if (string.IsNullOrEmpty(email))
                {
                    TempData["ErrorMessage"] = "無法取得 Facebook 帳號資訊";
                    return RedirectToAction("Login");
                }

                // 處理邏輯與 Google 相同
                var existingUser = await _unitOfWork.UserRepository.GetByEmailAsync(email);
                
                if (existingUser != null)
                {
                    await SignInExistingUser(existingUser);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["FacebookEmail"] = email;
                    TempData["FacebookName"] = name;
                    TempData["InfoMessage"] = "請完善您的註冊資訊";
                    return RedirectToAction("CompleteRegistration");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Facebook OAuth 回調時發生錯誤");
                TempData["ErrorMessage"] = "登入時發生錯誤";
                return RedirectToAction("Login");
            }
        }

        /// <summary>
        /// 完成 OAuth 註冊資訊
        /// </summary>
        [AllowAnonymous]
        public IActionResult CompleteRegistration()
        {
            var model = new CompleteRegistrationViewModel
            {
                Email = TempData["GoogleEmail"]?.ToString() ?? TempData["FacebookEmail"]?.ToString() ?? "",
                UserName = TempData["GoogleName"]?.ToString() ?? TempData["FacebookName"]?.ToString() ?? ""
            };

            if (string.IsNullOrEmpty(model.Email))
            {
                return RedirectToAction("Register");
            }

            return View(model);
        }

        /// <summary>
        /// 登入現有用戶
        /// </summary>
        private async Task SignInExistingUser(User user)
        {
            var userRights = await _unitOfWork.UserRepository.GetUserRightsAsync(user.UserId);
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName ?? ""),
                new Claim(ClaimTypes.Email, user.UserIntroduce?.Email ?? ""),
                new Claim("Account", user.UserAccount ?? ""),
                new Claim("Nickname", user.UserIntroduce?.UserNickName ?? ""),
                new Claim("ShoppingPermission", userRights?.ShoppingPermission.ToString() ?? "False"),
                new Claim("MessagePermission", userRights?.MessagePermission.ToString() ?? "False"),
                new Claim("SalesAuthority", userRights?.SalesAuthority.ToString() ?? "False")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
                new ClaimsPrincipal(claimsIdentity));

            _logger.LogInformation($"OAuth 使用者登入成功: {user.UserAccount}");
        }
    }

    /// <summary>
    /// 登入 ViewModel
    /// </summary>
    public class LoginViewModel
    {
        public string Account { get; set; } = "";
        public string Password { get; set; } = "";
        public bool RememberMe { get; set; }
    }

    /// <summary>
    /// 註冊 ViewModel
    /// </summary>
    public class RegisterViewModel
    {
        public string UserName { get; set; } = "";
        public string UserNickName { get; set; } = "";
        public string Email { get; set; } = "";
        public string UserAccount { get; set; } = "";
        public string Gender { get; set; } = "";
        public string IdNumber { get; set; } = "";
        public string Cellphone { get; set; } = "";
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; } = "";
        public string Password { get; set; } = "";
        public string ConfirmPassword { get; set; } = "";
        public bool AgreeToTerms { get; set; }
        public bool AcceptMarketing { get; set; }
    }

    /// <summary>
    /// 完成 OAuth 註冊 ViewModel
    /// </summary>
    public class CompleteRegistrationViewModel
    {
        public string Email { get; set; } = "";
        public string UserName { get; set; } = "";
        public string UserNickName { get; set; } = "";
        public string UserAccount { get; set; } = "";
        public string Gender { get; set; } = "";
        public string IdNumber { get; set; } = "";
        public string Cellphone { get; set; } = "";
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; } = "";
        public bool AgreeToTerms { get; set; }
        public bool AcceptMarketing { get; set; }
    }
}

