using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GameCore.Core.Interfaces;
using GameCore.Core.Entities;

namespace GameCore.Web.Controllers
{
    /// <summary>
    /// 個人資料控制器
    /// 處理用戶個人資料管理、設定等功能
    /// </summary>
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IUserService _userService;
        private readonly IPetService _petService;
        private readonly ISignInService _signInService;
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(
            IUserService userService,
            IPetService petService,
            ISignInService signInService,
            ILogger<ProfileController> logger)
        {
            _userService = userService;
            _petService = petService;
            _signInService = signInService;
            _logger = logger;
        }

        /// <summary>
        /// 個人資料首頁
        /// 顯示用戶基本資訊、統計數據等
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            try
            {
                var userId = GetCurrentUserId();
                var user = await _userService.GetUserByIdAsync(userId);
                
                if (user == null)
                {
                    return NotFound();
                }

                var viewModel = new ProfileIndexViewModel
                {
                    User = user,
                    Pet = await _petService.GetPetByUserIdAsync(userId),
                    SignInStats = await _signInService.GetUserSignInStatsAsync(userId),
                    GameStats = await _userService.GetUserGameStatsAsync(userId),
                    RecentActivities = await _userService.GetUserRecentActivitiesAsync(userId)
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "載入個人資料頁面時發生錯誤");
                return View("Error");
            }
        }

        /// <summary>
        /// 編輯個人資料頁面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            try
            {
                var userId = GetCurrentUserId();
                var user = await _userService.GetUserByIdAsync(userId);
                
                if (user == null)
                {
                    return NotFound();
                }

                var viewModel = new EditProfileViewModel
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    Email = user.Email,
                    DisplayName = user.DisplayName ?? string.Empty,
                    Bio = string.Empty, // 從 UserIntroduce 取得
                    AvatarUrl = string.Empty,
                    PrivacySettings = new PrivacySettingsViewModel()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "載入編輯個人資料頁面時發生錯誤");
                return View("Error");
            }
        }

        /// <summary>
        /// 處理個人資料更新
        /// </summary>
        /// <param name="model">個人資料編輯模型</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProfileViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var userId = GetCurrentUserId();
                await _userService.UpdateUserProfileAsync(userId, model);

                TempData["SuccessMessage"] = "個人資料更新成功！";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新個人資料時發生錯誤");
                ModelState.AddModelError("", "更新失敗，請稍後再試");
                return View(model);
            }
        }

        /// <summary>
        /// 帳戶設定頁面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Settings()
        {
            try
            {
                var userId = GetCurrentUserId();
                var user = await _userService.GetUserByIdAsync(userId);
                
                if (user == null)
                {
                    return NotFound();
                }

                var viewModel = new AccountSettingsViewModel
                {
                    Email = user.Email,
                    TwoFactorEnabled = false, // 從設定中取得
                    EmailNotifications = true,
                    SmsNotifications = false,
                    PrivacySettings = new PrivacySettingsViewModel(),
                    ConnectedAccounts = await _userService.GetConnectedAccountsAsync(userId)
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "載入帳戶設定頁面時發生錯誤");
                return View("Error");
            }
        }

        /// <summary>
        /// 更新帳戶設定
        /// </summary>
        /// <param name="model">帳戶設定模型</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Settings(AccountSettingsViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var userId = GetCurrentUserId();
                await _userService.UpdateAccountSettingsAsync(userId, model);

                TempData["SuccessMessage"] = "帳戶設定更新成功！";
                return RedirectToAction("Settings");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新帳戶設定時發生錯誤");
                ModelState.AddModelError("", "更新失敗，請稍後再試");
                return View(model);
            }
        }

        /// <summary>
        /// 修改密碼頁面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View(new ChangePasswordViewModel());
        }

        /// <summary>
        /// 處理密碼修改
        /// </summary>
        /// <param name="model">修改密碼模型</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var userId = GetCurrentUserId();
                var result = await _userService.ChangePasswordAsync(userId, model.CurrentPassword, model.NewPassword);

                if (result)
                {
                    TempData["SuccessMessage"] = "密碼修改成功！";
                    return RedirectToAction("Settings");
                }
                else
                {
                    ModelState.AddModelError("CurrentPassword", "目前密碼不正確");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "修改密碼時發生錯誤");
                ModelState.AddModelError("", "修改失敗，請稍後再試");
                return View(model);
            }
        }

        /// <summary>
        /// 我的訂單頁面
        /// </summary>
        /// <param name="status">訂單狀態</param>
        /// <param name="page">頁碼</param>
        /// <returns></returns>
        public async Task<IActionResult> Orders(string? status, int page = 1)
        {
            try
            {
                var userId = GetCurrentUserId();
                var viewModel = new UserOrdersViewModel
                {
                    Orders = await _userService.GetUserOrdersAsync(userId, status, page),
                    CurrentStatus = status,
                    CurrentPage = page
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "載入我的訂單頁面時發生錯誤");
                return View("Error");
            }
        }

        /// <summary>
        /// 我的發文頁面
        /// </summary>
        /// <param name="type">發文類型</param>
        /// <param name="page">頁碼</param>
        /// <returns></returns>
        public async Task<IActionResult> Posts(string type = "threads", int page = 1)
        {
            try
            {
                var userId = GetCurrentUserId();
                var viewModel = new UserPostsViewModel
                {
                    CurrentType = type,
                    CurrentPage = page
                };

                if (type == "threads")
                {
                    viewModel.Threads = await _userService.GetUserThreadsAsync(userId, page);
                }
                else
                {
                    viewModel.Posts = await _userService.GetUserPostsAsync(userId, page);
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "載入我的發文頁面時發生錯誤");
                return View("Error");
            }
        }

        /// <summary>
        /// 查看其他用戶的個人資料
        /// </summary>
        /// <param name="id">用戶ID</param>
        /// <returns></returns>
        public async Task<IActionResult> View(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                var currentUserId = GetCurrentUserId();
                var viewModel = new ViewProfileViewModel
                {
                    User = user,
                    Pet = await _petService.GetPetByUserIdAsync(id),
                    IsOwnProfile = currentUserId == id,
                    GameStats = await _userService.GetUserGameStatsAsync(id),
                    RecentPosts = await _userService.GetUserRecentPostsAsync(id)
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "載入用戶個人資料時發生錯誤，用戶ID: {UserId}", id);
                return View("Error");
            }
        }

        /// <summary>
        /// 獲取當前使用者ID
        /// </summary>
        /// <returns></returns>
        private int GetCurrentUserId()
        {
            // 這裡應該從 Claims 中獲取真實的使用者ID
            // 暫時返回示例ID
            return 1;
        }
    }

    #region ViewModels

    /// <summary>
    /// 個人資料首頁視圖模型
    /// </summary>
    public class ProfileIndexViewModel
    {
        public User User { get; set; } = null!;
        public Pet? Pet { get; set; }
        public UserSignInStats? SignInStats { get; set; }
        public UserGameStats GameStats { get; set; } = new();
        public List<UserActivity> RecentActivities { get; set; } = new();
    }

    /// <summary>
    /// 編輯個人資料視圖模型
    /// </summary>
    public class EditProfileViewModel
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public PrivacySettingsViewModel PrivacySettings { get; set; } = new();
    }

    /// <summary>
    /// 帳戶設定視圖模型
    /// </summary>
    public class AccountSettingsViewModel
    {
        public string Email { get; set; } = string.Empty;
        public bool TwoFactorEnabled { get; set; }
        public bool EmailNotifications { get; set; }
        public bool SmsNotifications { get; set; }
        public PrivacySettingsViewModel PrivacySettings { get; set; } = new();
        public List<ConnectedAccount> ConnectedAccounts { get; set; } = new();
    }

    /// <summary>
    /// 隱私設定視圖模型
    /// </summary>
    public class PrivacySettingsViewModel
    {
        public bool ShowEmail { get; set; }
        public bool ShowRealName { get; set; }
        public bool ShowOnlineStatus { get; set; }
        public bool AllowDirectMessages { get; set; }
        public bool ShowActivityFeed { get; set; }
    }

    /// <summary>
    /// 修改密碼視圖模型
    /// </summary>
    public class ChangePasswordViewModel
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    /// <summary>
    /// 用戶訂單視圖模型
    /// </summary>
    public class UserOrdersViewModel
    {
        public List<OrderInfo> Orders { get; set; } = new();
        public string? CurrentStatus { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
    }

    /// <summary>
    /// 用戶發文視圖模型
    /// </summary>
    public class UserPostsViewModel
    {
        public string CurrentType { get; set; } = "threads";
        public int CurrentPage { get; set; } = 1;
        public List<GameCore.Core.Entities.Thread> Threads { get; set; } = new();
        public List<Post> Posts { get; set; } = new();
    }

    /// <summary>
    /// 查看個人資料視圖模型
    /// </summary>
    public class ViewProfileViewModel
    {
        public User User { get; set; } = null!;
        public Pet? Pet { get; set; }
        public bool IsOwnProfile { get; set; }
        public UserGameStats GameStats { get; set; } = new();
        public List<Post> RecentPosts { get; set; } = new();
    }

    /// <summary>
    /// 用戶遊戲統計
    /// </summary>
    public class UserGameStats
    {
        public int TotalPlayTime { get; set; }
        public int GamesOwned { get; set; }
        public int Achievements { get; set; }
        public string FavoriteGenre { get; set; } = string.Empty;
        public List<GamePlayStats> RecentGames { get; set; } = new();
    }

    /// <summary>
    /// 遊戲遊玩統計
    /// </summary>
    public class GamePlayStats
    {
        public string GameName { get; set; } = string.Empty;
        public int PlayTime { get; set; }
        public DateTime LastPlayed { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }

    /// <summary>
    /// 用戶活動
    /// </summary>
    public class UserActivity
    {
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string Icon { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
    }

    /// <summary>
    /// 連接的帳戶
    /// </summary>
    public class ConnectedAccount
    {
        public string Provider { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public bool IsConnected { get; set; }
        public DateTime? ConnectedAt { get; set; }
    }

    #endregion
}
