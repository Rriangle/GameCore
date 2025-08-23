using Microsoft.AspNetCore.Mvc;
using GameCore.Core.Services;
using GameCore.Core.Entities;
using Microsoft.AspNetCore.Authorization;

namespace GameCore.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ForumController : ControllerBase
    {
        private readonly IForumService _forumService;
        private readonly ILogger<ForumController> _logger;

        public ForumController(IForumService forumService, ILogger<ForumController> logger)
        {
            _forumService = forumService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetForums()
        {
            try
            {
                var forums = await _forumService.GetForumsAsync();
                return Ok(new { Success = true, Data = forums });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting forums");
                return StatusCode(500, new { Success = false, Message = "取得論壇清單失敗" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetForum(int id)
        {
            try
            {
                var forum = await _forumService.GetForumByIdAsync(id);
                if (forum == null)
                {
                    return NotFound(new { Success = false, Message = "論壇不存在" });
                }
                return Ok(new { Success = true, Data = forum });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting forum {ForumId}", id);
                return StatusCode(500, new { Success = false, Message = "取得論壇資訊失敗" });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> CreateForum([FromBody] CreateForumRequest request)
        {
            try
            {
                var forum = await _forumService.CreateForumAsync(
                    request.Name, 
                    request.Description, 
                    request.Category, 
                    request.IsActive);
                return Ok(new { Success = true, Data = forum, Message = "論壇創建成功" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating forum");
                return StatusCode(500, new { Success = false, Message = "創建論壇失敗" });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> UpdateForum(int id, [FromBody] UpdateForumRequest request)
        {
            try
            {
                await _forumService.UpdateForumAsync(id, request.Name, request.Description, request.Category, request.IsActive);
                return Ok(new { Success = true, Message = "論壇更新成功" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating forum {ForumId}", id);
                return StatusCode(500, new { Success = false, Message = "更新論壇失敗" });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteForum(int id)
        {
            try
            {
                await _forumService.DeleteForumAsync(id);
                return Ok(new { Success = true, Message = "論壇刪除成功" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting forum {ForumId}", id);
                return StatusCode(500, new { Success = false, Message = "刪除論壇失敗" });
            }
        }

        [HttpGet("{id}/posts")]
        public async Task<IActionResult> GetForumPosts(int id, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                var posts = await _forumService.GetPostsByForumAsync(id, page, pageSize);
                return Ok(new { Success = true, Data = posts });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting posts for forum {ForumId}", id);
                return StatusCode(500, new { Success = false, Message = "取得貼文清單失敗" });
            }
        }

        [HttpGet("{id}/stats")]
        public async Task<IActionResult> GetForumStats(int id)
        {
            try
            {
                var stats = await _forumService.GetForumStatsAsync(id);
                return Ok(new { Success = true, Data = stats });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting stats for forum {ForumId}", id);
                return StatusCode(500, new { Success = false, Message = "取得論壇統計失敗" });
            }
        }

        [HttpPost("{id}/subscribe")]
        [Authorize]
        public async Task<IActionResult> SubscribeToForum(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _forumService.SubscribeToForumAsync(id, userId);
                return Ok(new { Success = true, Message = "訂閱成功" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error subscribing to forum {ForumId}", id);
                return StatusCode(500, new { Success = false, Message = "訂閱失敗" });
            }
        }

        [HttpDelete("{id}/subscribe")]
        [Authorize]
        public async Task<IActionResult> UnsubscribeFromForum(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _forumService.UnsubscribeFromForumAsync(id, userId);
                return Ok(new { Success = true, Message = "取消訂閱成功" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error unsubscribing from forum {ForumId}", id);
                return StatusCode(500, new { Success = false, Message = "取消訂閱失敗" });
            }
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var categories = await _forumService.GetCategoriesAsync();
                return Ok(new { Success = true, Data = categories });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting forum categories");
                return StatusCode(500, new { Success = false, Message = "取得論壇分類失敗" });
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchForums([FromQuery] string keyword, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                var forums = await _forumService.SearchForumsAsync(keyword, page, pageSize);
                return Ok(new { Success = true, Data = forums });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching forums with keyword {Keyword}", keyword);
                return StatusCode(500, new { Success = false, Message = "搜尋論壇失敗" });
            }
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : 0;
        }
    }

    public class CreateForumRequest
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Category { get; set; } = "";
        public bool IsActive { get; set; } = true;
    }

    public class UpdateForumRequest
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Category { get; set; } = "";
        public bool IsActive { get; set; } = true;
    }
}