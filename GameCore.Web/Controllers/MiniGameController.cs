using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GameCore.Core.Services;
using GameCore.Core.Entities;

namespace GameCore.Web.Controllers
{
    /// <summary>
    /// 小冒險遊戲控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MiniGameController : ControllerBase
    {
        private readonly IMiniGameService _miniGameService;
        private readonly ILogger<MiniGameController> _logger;

        public MiniGameController(
            IMiniGameService miniGameService,
            ILogger<MiniGameController> logger)
        {
            _miniGameService = miniGameService;
            _logger = logger;
        }

        /// <summary>
        /// 開始小冒險遊戲
        /// </summary>
        /// <param name="petId">寵物ID</param>
        /// <param name="gameLevel">遊戲等級</param>
        /// <returns>遊戲結果</returns>
        [HttpPost("start")]
        public async Task<IActionResult> StartGame([FromBody] StartGameRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // 從 JWT 或 Session 取得使用者ID
                var userId = GetCurrentUserId();
                if (userId == null)
                {
                    return Unauthorized("使用者未登入");
                }

                var result = await _miniGameService.StartGameAsync(userId.Value, request.PetId, request.GameLevel);
                
                if (result.IsSuccess)
                {
                    _logger.LogInformation("使用者 {UserId} 開始遊戲，寵物 {PetId}，等級 {GameLevel}", 
                        userId, request.PetId, request.GameLevel);
                    
                    return Ok(new
                    {
                        success = true,
                        message = result.Message,
                        data = result
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = result.Message
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "開始遊戲時發生錯誤");
                return StatusCode(500, new
                {
                    success = false,
                    message = "遊戲執行失敗，請稍後再試"
                });
            }
        }

        /// <summary>
        /// 檢查遊戲資格
        /// </summary>
        /// <param name="petId">寵物ID</param>
        /// <returns>檢查結果</returns>
        [HttpGet("check-eligibility/{petId}")]
        public async Task<IActionResult> CheckEligibility(int petId)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == null)
                {
                    return Unauthorized("使用者未登入");
                }

                var result = await _miniGameService.CheckGameEligibilityAsync(userId.Value, petId);
                
                return Ok(new
                {
                    success = true,
                    data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "檢查遊戲資格時發生錯誤");
                return StatusCode(500, new
                {
                    success = false,
                    message = "檢查失敗，請稍後再試"
                });
            }
        }

        /// <summary>
        /// 取得遊戲設定
        /// </summary>
        /// <param name="gameLevel">遊戲等級</param>
        /// <returns>遊戲設定</returns>
        [HttpGet("settings/{gameLevel}")]
        public async Task<IActionResult> GetGameSettings(int gameLevel)
        {
            try
            {
                var settings = await _miniGameService.GetGameSettingsAsync(gameLevel);
                
                if (settings == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "找不到指定的遊戲等級"
                    });
                }

                return Ok(new
                {
                    success = true,
                    data = settings
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得遊戲設定時發生錯誤");
                return StatusCode(500, new
                {
                    success = false,
                    message = "取得設定失敗，請稍後再試"
                });
            }
        }

        /// <summary>
        /// 取得使用者的遊戲記錄
        /// </summary>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>遊戲記錄列表</returns>
        [HttpGet("records")]
        public async Task<IActionResult> GetUserGameRecords([FromQuery] int page = 1, int pageSize = 20)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == null)
                {
                    return Unauthorized("使用者未登入");
                }

                var records = await _miniGameService.GetUserGameRecordsAsync(userId.Value, page, pageSize);
                
                return Ok(new
                {
                    success = true,
                    data = records,
                    pagination = new
                    {
                        page,
                        pageSize,
                        total = records.Count()
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得遊戲記錄時發生錯誤");
                return StatusCode(500, new
                {
                    success = false,
                    message = "取得記錄失敗，請稍後再試"
                });
            }
        }

        /// <summary>
        /// 取得寵物的遊戲記錄
        /// </summary>
        /// <param name="petId">寵物ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>遊戲記錄列表</returns>
        [HttpGet("pet-records/{petId}")]
        public async Task<IActionResult> GetPetGameRecords(int petId, [FromQuery] int page = 1, int pageSize = 20)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == null)
                {
                    return Unauthorized("使用者未登入");
                }

                var records = await _miniGameService.GetPetGameRecordsAsync(petId, page, pageSize);
                
                return Ok(new
                {
                    success = true,
                    data = records,
                    pagination = new
                    {
                        page,
                        pageSize,
                        total = records.Count()
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得寵物遊戲記錄時發生錯誤");
                return StatusCode(500, new
                {
                    success = false,
                    message = "取得記錄失敗，請稍後再試"
                });
            }
        }

        /// <summary>
        /// 取得當前使用者ID (從 JWT 或 Session)
        /// </summary>
        private int? GetCurrentUserId()
        {
            // 這裡應該從 JWT Token 或 Session 中取得使用者ID
            // 暫時返回固定值，實際實作時需要根據認證機制調整
            if (User.Identity?.IsAuthenticated == true)
            {
                var userIdClaim = User.FindFirst("UserId");
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    return userId;
                }
            }

            // 如果無法從認證取得，可以從 Session 取得
            var sessionUserId = HttpContext.Session.GetString("UserId");
            if (!string.IsNullOrEmpty(sessionUserId) && int.TryParse(sessionUserId, out int sessionId))
            {
                return sessionId;
            }

            return null;
        }
    }

    /// <summary>
    /// 開始遊戲請求模型
    /// </summary>
    public class StartGameRequest
    {
        public int PetId { get; set; }
        public int GameLevel { get; set; }
    }
}