using Microsoft.AspNetCore.Mvc;
using GameCore.Core.Services;
using GameCore.Core.Entities;
using Microsoft.AspNetCore.Authorization;

namespace GameCore.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IForumService _forumService;
        private readonly ILogger<PostController> _logger;

        public PostController(IForumService forumService, ILogger<PostController> logger)
        {
            _forumService = forumService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts([FromQuery] int forumId = 0, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                var posts = forumId > 0 
                    ? await _forumService.GetPostsByForumAsync(forumId, page, pageSize)
                    : await _forumService.GetAllPostsAsync(page, pageSize);
                return Ok(new { Success = true, Data = posts });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting posts");
                return StatusCode(500, new { Success = false, Message = "取得貼文清單失敗" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            try
            {
                var post = await _forumService.GetPostByIdAsync(id);
                if (post == null)
                {
                    return NotFound(new { Success = false, Message = "貼文不存在" });
                }
                return Ok(new { Success = true, Data = post });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting post {PostId}", id);
                return StatusCode(500, new { Success = false, Message = "取得貼文資訊失敗" });
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest request)
        {
            try
            {
                var userId = GetCurrentUserId();
                var postCreate = new PostCreateDto
                {
                    ForumId = request.ForumId,
                    Title = request.Title,
                    Content = request.Content
                };
                var post = await _forumService.CreatePostAsync(userId, postCreate);
                return Ok(new { Success = true, Data = post, Message = "貼文創建成功" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating post");
                return StatusCode(500, new { Success = false, Message = "創建貼文失敗" });
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdatePost(int id, [FromBody] UpdatePostRequest request)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _forumService.UpdatePostAsync(id, userId, request.Title, request.Content, request.Tags);
                return Ok(new { Success = true, Message = "貼文更新成功" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating post {PostId}", id);
                return StatusCode(500, new { Success = false, Message = "更新貼文失敗" });
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePost(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _forumService.DeletePostAsync(id, userId);
                return Ok(new { Success = true, Message = "貼文刪除成功" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting post {PostId}", id);
                return StatusCode(500, new { Success = false, Message = "刪除貼文失敗" });
            }
        }

        [HttpPost("{id}/like")]
        [Authorize]
        public async Task<IActionResult> LikePost(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _forumService.LikePostAsync(userId, id);
                return Ok(new { Success = true, Message = "貼文按讚成功" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error liking post {PostId}", id);
                return StatusCode(500, new { Success = false, Message = "按讚失敗" });
            }
        }

        [HttpDelete("{id}/like")]
        [Authorize]
        public async Task<IActionResult> UnlikePost(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _forumService.UnlikePostAsync(userId, id);
                return Ok(new { Success = true, Message = "取消按讚成功" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error unliking post {PostId}", id);
                return StatusCode(500, new { Success = false, Message = "取消按讚失敗" });
            }
        }

        [HttpPost("{id}/bookmark")]
        [Authorize]
        public async Task<IActionResult> BookmarkPost(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _forumService.BookmarkPostAsync(userId, id);
                return Ok(new { Success = true, Message = "貼文收藏成功" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error bookmarking post {PostId}", id);
                return StatusCode(500, new { Success = false, Message = "收藏失敗" });
            }
        }

        [HttpDelete("{id}/bookmark")]
        [Authorize]
        public async Task<IActionResult> RemoveBookmark(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _forumService.RemoveBookmarkAsync(id, userId);
                return Ok(new { Success = true, Message = "取消收藏成功" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing bookmark for post {PostId}", id);
                return StatusCode(500, new { Success = false, Message = "取消收藏失敗" });
            }
        }

        [HttpGet("{id}/replies")]
        public async Task<IActionResult> GetPostReplies(int id, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                var replies = await _forumService.GetPostRepliesAsync(id, page, pageSize);
                return Ok(new { Success = true, Data = replies });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting replies for post {PostId}", id);
                return StatusCode(500, new { Success = false, Message = "取得回覆清單失敗" });
            }
        }

        [HttpPost("{id}/replies")]
        [Authorize]
        public async Task<IActionResult> CreateReply(int id, [FromBody] CreateReplyRequest request)
        {
            try
            {
                var userId = GetCurrentUserId();
                var reply = await _forumService.CreateReplyAsync(
                    id, 
                    userId, 
                    request.Content, 
                    request.ParentReplyId);
                return Ok(new { Success = true, Data = reply, Message = "回覆創建成功" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating reply for post {PostId}", id);
                return StatusCode(500, new { Success = false, Message = "創建回覆失敗" });
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchPosts([FromQuery] string keyword, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                var posts = await _forumService.SearchPostsAsync(keyword, page, pageSize);
                return Ok(new { Success = true, Data = posts });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching posts with keyword {Keyword}", keyword);
                return StatusCode(500, new { Success = false, Message = "搜尋貼文失敗" });
            }
        }

        [HttpGet("trending")]
        public async Task<IActionResult> GetTrendingPosts([FromQuery] int take = 10)
        {
            try
            {
                var posts = await _forumService.GetTrendingPostsAsync(take);
                return Ok(new { Success = true, Data = posts });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting trending posts");
                return StatusCode(500, new { Success = false, Message = "取得熱門貼文失敗" });
            }
        }

        [HttpGet("my-posts")]
        [Authorize]
        public async Task<IActionResult> GetMyPosts([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                var userId = GetCurrentUserId();
                var posts = await _forumService.GetUserPostsAsync(userId, page, pageSize);
                return Ok(new { Success = true, Data = posts });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user posts");
                return StatusCode(500, new { Success = false, Message = "取得我的貼文失敗" });
            }
        }

        [HttpGet("my-bookmarks")]
        [Authorize]
        public async Task<IActionResult> GetMyBookmarks([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                var userId = GetCurrentUserId();
                var posts = await _forumService.GetUserBookmarksAsync(userId, page, pageSize);
                return Ok(new { Success = true, Data = posts });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user bookmarks");
                return StatusCode(500, new { Success = false, Message = "取得我的收藏失敗" });
            }
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : 0;
        }
    }

    public class CreatePostRequest
    {
        public int ForumId { get; set; }
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public string[] Tags { get; set; } = Array.Empty<string>();
    }

    public class UpdatePostRequest
    {
        public string Title { get; set; } = "";
        public string Content { get; set; } = "";
        public string[] Tags { get; set; } = Array.Empty<string>();
    }

    public class CreateReplyRequest
    {
        public string Content { get; set; } = "";
        public int? ParentReplyId { get; set; }
    }
}