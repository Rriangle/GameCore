using GameCore.Core.DTOs;
using GameCore.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GameCore.Web.Controllers
{
    /// <summary>
    /// 論壇控制器
    /// 提供論壇相關的 API 端點和頁面
    /// </summary>
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
        /// </summary>
        /// <returns>論壇首頁視圖</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 論壇版面頁面
        /// </summary>
        /// <param name="id">論壇版ID</param>
        /// <returns>論壇版面視圖</returns>
        public IActionResult Board(int id)
        {
            return View(id);
        }

        /// <summary>
        /// 主題詳情頁面
        /// </summary>
        /// <param name="id">主題ID</param>
        /// <returns>主題詳情視圖</returns>
        public IActionResult Thread(long id)
        {
            return View(id);
        }

        // API 端點
        #region API Endpoints

        /// <summary>
        /// 取得論壇版面列表
        /// </summary>
        /// <param name="gameId">遊戲ID（可選）</param>
        /// <param name="keyword">關鍵字搜尋</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁數量</param>
        /// <returns>論壇版面列表</returns>
        [HttpGet("api/forum")]
        public async Task<ActionResult<PagedResponse<ForumInfo>>> GetForums(
            [FromQuery] int? gameId = null,
            [FromQuery] string? keyword = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            try
            {
                var request = new ForumQueryRequest
                {
                    GameId = gameId,
                    Keyword = keyword,
                    Page = page,
                    PageSize = pageSize
                };

                var result = await _forumService.GetForumsAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得論壇版面列表時發生錯誤");
                return StatusCode(500, new { message = "取得論壇版面列表失敗" });
            }
        }

        /// <summary>
        /// 取得論壇版面詳情
        /// </summary>
        /// <param name="forumId">論壇版ID</param>
        /// <returns>論壇版面詳情</returns>
        [HttpGet("api/forum/{forumId}")]
        public async Task<ActionResult<ForumInfo>> GetForum(int forumId)
        {
            try
            {
                var forum = await _forumService.GetForumByIdAsync(forumId);
                if (forum == null)
                {
                    return NotFound(new { message = "論壇版面不存在" });
                }

                return Ok(forum);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得論壇版面詳情時發生錯誤，ForumId: {ForumId}", forumId);
                return StatusCode(500, new { message = "取得論壇版面詳情失敗" });
            }
        }

        /// <summary>
        /// 取得主題列表
        /// </summary>
        /// <param name="forumId">論壇版ID</param>
        /// <param name="keyword">關鍵字搜尋</param>
        /// <param name="sortBy">排序方式（latest/popular）</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁數量</param>
        /// <returns>主題列表</returns>
        [HttpGet("api/forum/threads")]
        public async Task<ActionResult<PagedResponse<ThreadListItem>>> GetThreads(
            [FromQuery] int forumId,
            [FromQuery] string? keyword = null,
            [FromQuery] string sortBy = "latest",
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            try
            {
                var request = new ThreadQueryRequest
                {
                    ForumId = forumId,
                    Keyword = keyword,
                    SortBy = sortBy,
                    Page = page,
                    PageSize = pageSize
                };

                var result = await _forumService.GetThreadsAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得主題列表時發生錯誤，ForumId: {ForumId}", forumId);
                return StatusCode(500, new { message = "取得主題列表失敗" });
            }
        }

        /// <summary>
        /// 取得主題詳情
        /// </summary>
        /// <param name="threadId">主題ID</param>
        /// <returns>主題詳情</returns>
        [HttpGet("api/forum/threads/{threadId}")]
        public async Task<ActionResult<ThreadDetail>> GetThread(long threadId)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                var thread = await _forumService.GetThreadByIdAsync(threadId, currentUserId);
                if (thread == null)
                {
                    return NotFound(new { message = "主題不存在或已被刪除" });
                }

                return Ok(thread);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得主題詳情時發生錯誤，ThreadId: {ThreadId}", threadId);
                return StatusCode(500, new { message = "取得主題詳情失敗" });
            }
        }

        /// <summary>
        /// 建立新主題
        /// </summary>
        /// <param name="request">建立主題請求</param>
        /// <returns>新建立的主題ID</returns>
        [HttpPost("api/forum/threads")]
        [Authorize]
        public async Task<ActionResult<long>> CreateThread([FromBody] CreateThreadRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "請求資料無效", errors = ModelState });
                }

                var currentUserId = GetCurrentUserId();
                var threadId = await _forumService.CreateThreadAsync(request, currentUserId);

                return CreatedAtAction(nameof(GetThread), new { threadId }, new { threadId });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "建立新主題時發生錯誤");
                return StatusCode(500, new { message = "建立新主題失敗" });
            }
        }

        /// <summary>
        /// 建立新回覆
        /// </summary>
        /// <param name="request">建立回覆請求</param>
        /// <returns>新建立的回覆ID</returns>
        [HttpPost("api/forum/posts")]
        [Authorize]
        public async Task<ActionResult<long>> CreatePost([FromBody] CreatePostRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "請求資料無效", errors = ModelState });
                }

                var currentUserId = GetCurrentUserId();
                var postId = await _forumService.CreatePostAsync(request, currentUserId);

                return Ok(new { postId });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "建立新回覆時發生錯誤");
                return StatusCode(500, new { message = "建立新回覆失敗" });
            }
        }

        /// <summary>
        /// 更新主題
        /// </summary>
        /// <param name="threadId">主題ID</param>
        /// <param name="title">新標題</param>
        /// <returns>是否成功</returns>
        [HttpPut("api/forum/threads/{threadId}")]
        [Authorize]
        public async Task<ActionResult<bool>> UpdateThread(long threadId, [FromBody] string title)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(title))
                {
                    return BadRequest(new { message = "標題不能為空" });
                }

                var currentUserId = GetCurrentUserId();
                var success = await _forumService.UpdateThreadAsync(threadId, title, currentUserId);

                if (!success)
                {
                    return NotFound(new { message = "主題不存在或已被刪除" });
                }

                return Ok(new { success = true });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新主題時發生錯誤，ThreadId: {ThreadId}", threadId);
                return StatusCode(500, new { message = "更新主題失敗" });
            }
        }

        /// <summary>
        /// 更新回覆
        /// </summary>
        /// <param name="postId">回覆ID</param>
        /// <param name="content">新內容</param>
        /// <returns>是否成功</returns>
        [HttpPut("api/forum/posts/{postId}")]
        [Authorize]
        public async Task<ActionResult<bool>> UpdatePost(long postId, [FromBody] string content)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(content))
                {
                    return BadRequest(new { message = "內容不能為空" });
                }

                var currentUserId = GetCurrentUserId();
                var success = await _forumService.UpdatePostAsync(postId, content, currentUserId);

                if (!success)
                {
                    return NotFound(new { message = "回覆不存在或已被刪除" });
                }

                return Ok(new { success = true });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新回覆時發生錯誤，PostId: {PostId}", postId);
                return StatusCode(500, new { message = "更新回覆失敗" });
            }
        }

        /// <summary>
        /// 刪除主題
        /// </summary>
        /// <param name="threadId">主題ID</param>
        /// <returns>是否成功</returns>
        [HttpDelete("api/forum/threads/{threadId}")]
        [Authorize]
        public async Task<ActionResult<bool>> DeleteThread(long threadId)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                var success = await _forumService.DeleteThreadAsync(threadId, currentUserId);

                if (!success)
                {
                    return NotFound(new { message = "主題不存在或已被刪除" });
                }

                return Ok(new { success = true });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "刪除主題時發生錯誤，ThreadId: {ThreadId}", threadId);
                return StatusCode(500, new { message = "刪除主題失敗" });
            }
        }

        /// <summary>
        /// 刪除回覆
        /// </summary>
        /// <param name="postId">回覆ID</param>
        /// <returns>是否成功</returns>
        [HttpDelete("api/forum/posts/{postId}")]
        [Authorize]
        public async Task<ActionResult<bool>> DeletePost(long postId)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                var success = await _forumService.DeletePostAsync(postId, currentUserId);

                if (!success)
                {
                    return NotFound(new { message = "回覆不存在或已被刪除" });
                }

                return Ok(new { success = true });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "刪除回覆時發生錯誤，PostId: {PostId}", postId);
                return StatusCode(500, new { message = "刪除回覆失敗" });
            }
        }

        /// <summary>
        /// 新增反應（讚、表情等）
        /// </summary>
        /// <param name="request">反應請求</param>
        /// <returns>是否成功</returns>
        [HttpPost("api/forum/reactions")]
        [Authorize]
        public async Task<ActionResult<bool>> AddReaction([FromBody] ReactionRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "請求資料無效", errors = ModelState });
                }

                var currentUserId = GetCurrentUserId();
                var success = await _forumService.AddReactionAsync(request, currentUserId);

                return Ok(new { success });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "新增反應時發生錯誤");
                return StatusCode(500, new { message = "新增反應失敗" });
            }
        }

        /// <summary>
        /// 移除反應
        /// </summary>
        /// <param name="request">反應請求</param>
        /// <returns>是否成功</returns>
        [HttpDelete("api/forum/reactions")]
        [Authorize]
        public async Task<ActionResult<bool>> RemoveReaction([FromBody] ReactionRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "請求資料無效", errors = ModelState });
                }

                var currentUserId = GetCurrentUserId();
                var success = await _forumService.RemoveReactionAsync(request, currentUserId);

                return Ok(new { success });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "移除反應時發生錯誤");
                return StatusCode(500, new { message = "移除反應失敗" });
            }
        }

        /// <summary>
        /// 新增收藏
        /// </summary>
        /// <param name="request">收藏請求</param>
        /// <returns>是否成功</returns>
        [HttpPost("api/forum/bookmarks")]
        [Authorize]
        public async Task<ActionResult<bool>> AddBookmark([FromBody] BookmarkRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "請求資料無效", errors = ModelState });
                }

                var currentUserId = GetCurrentUserId();
                var success = await _forumService.AddBookmarkAsync(request, currentUserId);

                return Ok(new { success });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "新增收藏時發生錯誤");
                return StatusCode(500, new { message = "新增收藏失敗" });
            }
        }

        /// <summary>
        /// 移除收藏
        /// </summary>
        /// <param name="request">收藏請求</param>
        /// <returns>是否成功</returns>
        [HttpDelete("api/forum/bookmarks")]
        [Authorize]
        public async Task<ActionResult<bool>> RemoveBookmark([FromBody] BookmarkRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "請求資料無效", errors = ModelState });
                }

                var currentUserId = GetCurrentUserId();
                var success = await _forumService.RemoveBookmarkAsync(request, currentUserId);

                return Ok(new { success });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "移除收藏時發生錯誤");
                return StatusCode(500, new { message = "移除收藏失敗" });
            }
        }

        /// <summary>
        /// 取得用戶的收藏列表
        /// </summary>
        /// <param name="targetType">目標類型</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁數量</param>
        /// <returns>收藏列表</returns>
        [HttpGet("api/forum/bookmarks")]
        [Authorize]
        public async Task<ActionResult<PagedResponse<object>>> GetUserBookmarks(
            [FromQuery] string targetType,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                var result = await _forumService.GetUserBookmarksAsync(currentUserId, targetType, page, pageSize);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得用戶收藏列表時發生錯誤");
                return StatusCode(500, new { message = "取得收藏列表失敗" });
            }
        }

        /// <summary>
        /// 搜尋主題和回覆
        /// </summary>
        /// <param name="keyword">關鍵字</param>
        /// <param name="forumId">論壇版ID（可選）</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁數量</param>
        /// <returns>搜尋結果</returns>
        [HttpGet("api/forum/search")]
        public async Task<ActionResult<PagedResponse<object>>> Search(
            [FromQuery] string keyword,
            [FromQuery] int? forumId = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    return BadRequest(new { message = "關鍵字不能為空" });
                }

                var result = await _forumService.SearchAsync(keyword, forumId, page, pageSize);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "搜尋時發生錯誤，Keyword: {Keyword}", keyword);
                return StatusCode(500, new { message = "搜尋失敗" });
            }
        }

        /// <summary>
        /// 取得熱門主題
        /// </summary>
        /// <param name="forumId">論壇版ID（可選）</param>
        /// <param name="limit">數量限制</param>
        /// <returns>熱門主題列表</returns>
        [HttpGet("api/forum/popular")]
        public async Task<ActionResult<List<ThreadListItem>>> GetPopularThreads(
            [FromQuery] int? forumId = null,
            [FromQuery] int limit = 10)
        {
            try
            {
                var threads = await _forumService.GetPopularThreadsAsync(forumId, limit);
                return Ok(threads);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得熱門主題時發生錯誤");
                return StatusCode(500, new { message = "取得熱門主題失敗" });
            }
        }

        /// <summary>
        /// 取得最新主題
        /// </summary>
        /// <param name="forumId">論壇版ID（可選）</param>
        /// <param name="limit">數量限制</param>
        /// <returns>最新主題列表</returns>
        [HttpGet("api/forum/latest")]
        public async Task<ActionResult<List<ThreadListItem>>> GetLatestThreads(
            [FromQuery] int? forumId = null,
            [FromQuery] int limit = 10)
        {
            try
            {
                var threads = await _forumService.GetLatestThreadsAsync(forumId, limit);
                return Ok(threads);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得最新主題時發生錯誤");
                return StatusCode(500, new { message = "取得最新主題失敗" });
            }
        }

        #endregion

        /// <summary>
        /// 取得當前用戶ID
        /// </summary>
        /// <returns>用戶ID</returns>
        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                throw new UnauthorizedAccessException("無法取得用戶ID");
            }
            return userId;
        }
    }
}