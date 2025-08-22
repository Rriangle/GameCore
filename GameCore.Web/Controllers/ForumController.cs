using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GameCore.Core.Interfaces;
using GameCore.Core.Entities;

namespace GameCore.Web.Controllers
{
    /// <summary>
    /// 論壇控制器
    /// 處理論壇瀏覽、發文、回覆等功能
    /// </summary>
    [Authorize]
    public class ForumController : Controller
    {
        private readonly IForumService _forumService;
        private readonly ILogger<ForumController> _logger;

        public ForumController(IForumService forumService, ILogger<ForumController> logger)
        {
            _forumService = forumService;
            _logger = logger;
        }

        /// <summary>
        /// 論壇首頁
        /// 顯示所有版塊和熱門討論
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            try
            {
                var viewModel = new ForumIndexViewModel
                {
                    Forums = await _forumService.GetAllForumsAsync(),
                    HotThreads = await _forumService.GetHotThreadsAsync(),
                    RecentPosts = await _forumService.GetRecentPostsAsync(),
                    OnlineUsers = await _forumService.GetOnlineUsersAsync()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "載入論壇首頁時發生錯誤");
                return View("Error");
            }
        }

        /// <summary>
        /// 版塊詳情頁面
        /// 顯示特定版塊的主題列表
        /// </summary>
        /// <param name="id">版塊ID</param>
        /// <param name="sort">排序方式</param>
        /// <param name="page">頁碼</param>
        /// <returns></returns>
        public async Task<IActionResult> Forum(int id, string sort = "latest", int page = 1)
        {
            try
            {
                var forum = await _forumService.GetForumByIdAsync(id);
                if (forum == null)
                {
                    return NotFound();
                }

                var viewModel = new ForumDetailViewModel
                {
                    Forum = forum,
                    Threads = await _forumService.GetThreadsAsync(id, sort, page),
                    CurrentSort = sort,
                    CurrentPage = page,
                    PinnedThreads = await _forumService.GetPinnedThreadsAsync(id)
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "載入版塊詳情時發生錯誤，版塊ID: {ForumId}", id);
                return View("Error");
            }
        }

        /// <summary>
        /// 主題詳情頁面
        /// 顯示主題內容和回覆
        /// </summary>
        /// <param name="id">主題ID</param>
        /// <param name="page">頁碼</param>
        /// <returns></returns>
        public async Task<IActionResult> Thread(int id, int page = 1)
        {
            try
            {
                var thread = await _forumService.GetThreadByIdAsync(id);
                if (thread == null)
                {
                    return NotFound();
                }

                // 增加瀏覽量
                await _forumService.IncrementThreadViewsAsync(id);

                var viewModel = new ThreadDetailViewModel
                {
                    Thread = thread,
                    Posts = await _forumService.GetThreadPostsAsync(id, page),
                    CurrentPage = page,
                    TotalPages = await _forumService.GetThreadPostPagesAsync(id),
                    CanReply = await _forumService.CanUserReplyAsync(GetCurrentUserId(), id)
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "載入主題詳情時發生錯誤，主題ID: {ThreadId}", id);
                return View("Error");
            }
        }

        /// <summary>
        /// 新增主題頁面
        /// </summary>
        /// <param name="forumId">版塊ID</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> NewThread(int forumId)
        {
            try
            {
                var forum = await _forumService.GetForumByIdAsync(forumId);
                if (forum == null)
                {
                    return NotFound();
                }

                var viewModel = new NewThreadViewModel
                {
                    ForumId = forumId,
                    ForumName = forum.ForumName
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "載入新增主題頁面時發生錯誤，版塊ID: {ForumId}", forumId);
                return View("Error");
            }
        }

        /// <summary>
        /// 處理新增主題
        /// </summary>
        /// <param name="model">新主題資料</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewThread(NewThreadViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var userId = GetCurrentUserId();
                var threadId = await _forumService.CreateThreadAsync(userId, model);

                return RedirectToAction("Thread", new { id = threadId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "新增主題時發生錯誤");
                ModelState.AddModelError("", "發布失敗，請稍後再試");
                return View(model);
            }
        }

        /// <summary>
        /// 處理回覆主題
        /// </summary>
        /// <param name="threadId">主題ID</param>
        /// <param name="content">回覆內容</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reply(int threadId, string content)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(content))
                {
                    return Json(new { success = false, message = "回覆內容不能為空" });
                }

                var userId = GetCurrentUserId();
                await _forumService.CreateReplyAsync(userId, threadId, content);

                return Json(new { success = true, message = "回覆成功" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "回覆主題時發生錯誤，主題ID: {ThreadId}", threadId);
                return Json(new { success = false, message = "回覆失敗，請稍後再試" });
            }
        }

        /// <summary>
        /// 搜尋功能
        /// </summary>
        /// <param name="q">搜尋關鍵字</param>
        /// <param name="type">搜尋類型</param>
        /// <param name="page">頁碼</param>
        /// <returns></returns>
        public async Task<IActionResult> Search(string q, string type = "all", int page = 1)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(q))
                {
                    return View(new SearchResultViewModel());
                }

                var viewModel = new SearchResultViewModel
                {
                    Query = q,
                    SearchType = type,
                    CurrentPage = page,
                    Results = await _forumService.SearchAsync(q, type, page)
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "搜尋時發生錯誤，關鍵字: {Query}", q);
                return View("Error");
            }
        }

        /// <summary>
        /// 我的發文列表
        /// </summary>
        /// <param name="type">發文類型</param>
        /// <param name="page">頁碼</param>
        /// <returns></returns>
        public async Task<IActionResult> MyPosts(string type = "threads", int page = 1)
        {
            try
            {
                var userId = GetCurrentUserId();
                var viewModel = new MyPostsViewModel
                {
                    CurrentType = type,
                    CurrentPage = page
                };

                if (type == "threads")
                {
                    viewModel.MyThreads = await _forumService.GetUserThreadsAsync(userId, page);
                }
                else
                {
                    viewModel.MyReplies = await _forumService.GetUserRepliesAsync(userId, page);
                }

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "載入我的發文時發生錯誤");
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
    /// 論壇首頁視圖模型
    /// </summary>
    public class ForumIndexViewModel
    {
        public List<Forum> Forums { get; set; } = new();
        public List<GameCore.Core.Entities.Thread> HotThreads { get; set; } = new();
        public List<Post> RecentPosts { get; set; } = new();
        public List<User> OnlineUsers { get; set; } = new();
    }

    /// <summary>
    /// 版塊詳情視圖模型
    /// </summary>
    public class ForumDetailViewModel
    {
        public Forum Forum { get; set; } = null!;
        public List<GameCore.Core.Entities.Thread> Threads { get; set; } = new();
        public List<GameCore.Core.Entities.Thread> PinnedThreads { get; set; } = new();
        public string CurrentSort { get; set; } = "latest";
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
    }

    /// <summary>
    /// 主題詳情視圖模型
    /// </summary>
    public class ThreadDetailViewModel
    {
        public GameCore.Core.Entities.Thread Thread { get; set; } = null!;
        public List<ThreadPost> Posts { get; set; } = new();
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public bool CanReply { get; set; } = true;
    }

    /// <summary>
    /// 新增主題視圖模型
    /// </summary>
    public class NewThreadViewModel
    {
        public int ForumId { get; set; }
        public string ForumName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public bool IsSticky { get; set; }
        public bool IsLocked { get; set; }
        public List<string> Tags { get; set; } = new();
    }

    /// <summary>
    /// 搜尋結果視圖模型
    /// </summary>
    public class SearchResultViewModel
    {
        public string Query { get; set; } = string.Empty;
        public string SearchType { get; set; } = "all";
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public List<SearchResult> Results { get; set; } = new();
    }

    /// <summary>
    /// 我的發文視圖模型
    /// </summary>
    public class MyPostsViewModel
    {
        public string CurrentType { get; set; } = "threads";
        public int CurrentPage { get; set; } = 1;
        public List<GameCore.Core.Entities.Thread> MyThreads { get; set; } = new();
        public List<ThreadPost> MyReplies { get; set; } = new();
    }

    /// <summary>
    /// 搜尋結果項目
    /// </summary>
    public class SearchResult
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty; // "thread" or "post"
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string ForumName { get; set; } = string.Empty;
    }

    #endregion
}
