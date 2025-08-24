using GameCore.Core.DTOs;
using GameCore.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GameCore.Web.Controllers
{
    /// <summary>
    /// 小遊戲控制器 - 完整實現出發冒險系統API
    /// 提供遊戲管理、記錄查詢、統計分析等完整端點
    /// 嚴格按照規格要求實現每日次數限制和寵物整合
    /// </summary>
    [ApiController]
    [Route("api/minigame")]
    [Authorize] // 所有小遊戲功能都需要登入
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

        #region 遊戲基本管理

        /// <summary>
        /// 檢查遊戲資格
        /// GET /api/minigame/eligibility
        /// </summary>
        /// <returns>是否可以開始遊戲及相關訊息</returns>
        [HttpGet("eligibility")]
        public async Task<IActionResult> CheckEligibility()
        {
            try
            {
                var userId = GetCurrentUserId();
                _logger.LogInformation($"檢查使用者 {userId} 的遊戲資格");

                var result = await _miniGameService.CheckGameEligibilityAsync(userId);

                return Ok(new
                {
                    success = true,
                    data = result,
                    message = result.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "檢查遊戲資格時發生錯誤");
                return StatusCode(500, new { success = false, message = "檢查遊戲資格時發生錯誤" });
            }
        }

        /// <summary>
        /// 開始新遊戲
        /// POST /api/minigame/start
        /// </summary>
        /// <returns>遊戲會話資訊</returns>
        [HttpPost("start")]
        public async Task<IActionResult> StartGame()
        {
            try
            {
                var userId = GetCurrentUserId();
                _logger.LogInformation($"使用者 {userId} 開始新遊戲");

                var result = await _miniGameService.StartGameAsync(userId);

                if (result.Success)
                {
                    return Ok(new
                    {
                        success = true,
                        data = result.Data,
                        message = result.Message
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = result.Message,
                        errors = result.Errors
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "開始遊戲時發生錯誤");
                return StatusCode(500, new { success = false, message = "開始遊戲時發生錯誤" });
            }
        }

        /// <summary>
        /// 完成遊戲
        /// POST /api/minigame/finish
        /// </summary>
        /// <param name="gameId">遊戲ID</param>
        /// <param name="finishRequest">完成遊戲請求</param>
        /// <returns>遊戲結算結果</returns>
        [HttpPost("finish/{gameId}")]
        public async Task<IActionResult> FinishGame(int gameId, [FromBody] FinishGameDto finishRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { success = false, message = "資料格式錯誤", errors = ModelState });
                }

                var userId = GetCurrentUserId();
                _logger.LogInformation($"使用者 {userId} 完成遊戲 {gameId}");

                var result = await _miniGameService.FinishGameAsync(userId, gameId, finishRequest);

                if (result.Success)
                {
                    return Ok(new
                    {
                        success = true,
                        data = result.Data,
                        message = result.Message
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = result.Message,
                        errors = result.Errors
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"完成遊戲 {gameId} 時發生錯誤");
                return StatusCode(500, new { success = false, message = "完成遊戲時發生錯誤" });
            }
        }

        /// <summary>
        /// 中斷遊戲
        /// POST /api/minigame/abort/{gameId}
        /// </summary>
        /// <param name="gameId">遊戲ID</param>
        /// <param name="reason">中斷原因</param>
        /// <returns>中斷結果</returns>
        [HttpPost("abort/{gameId}")]
        public async Task<IActionResult> AbortGame(int gameId, [FromBody] AbortGameRequest? request = null)
        {
            try
            {
                var userId = GetCurrentUserId();
                var reason = request?.Reason ?? "使用者中斷";
                _logger.LogInformation($"使用者 {userId} 中斷遊戲 {gameId}：{reason}");

                var result = await _miniGameService.AbortGameAsync(userId, gameId, reason);

                if (result.Success)
                {
                    return Ok(new
                    {
                        success = true,
                        message = result.Message
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
                _logger.LogError(ex, $"中斷遊戲 {gameId} 時發生錯誤");
                return StatusCode(500, new { success = false, message = "中斷遊戲時發生錯誤" });
            }
        }

        #endregion

        #region 遊戲記錄與統計

        /// <summary>
        /// 取得遊戲記錄
        /// GET /api/minigame/records
        /// </summary>
        /// <param name="from">開始日期</param>
        /// <param name="to">結束日期</param>
        /// <param name="result">遊戲結果篩選</param>
        /// <param name="level">關卡篩選</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <returns>遊戲記錄列表</returns>
        [HttpGet("records")]
        public async Task<IActionResult> GetRecords(
            [FromQuery] DateTime? from = null,
            [FromQuery] DateTime? to = null,
            [FromQuery] bool? result = null,
            [FromQuery] int? level = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            try
            {
                var userId = GetCurrentUserId();
                _logger.LogInformation($"取得使用者 {userId} 的遊戲記錄");

                var request = new GetGameRecordsDto
                {
                    FromDate = from,
                    ToDate = to,
                    IsVictory = result,
                    Level = level,
                    Page = page,
                    PageSize = Math.Min(pageSize, 100) // 限制最大頁面大小
                };

                var records = await _miniGameService.GetGameRecordsAsync(userId, request);

                return Ok(new
                {
                    success = true,
                    data = records,
                    message = "遊戲記錄取得成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得遊戲記錄時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得遊戲記錄時發生錯誤" });
            }
        }

        /// <summary>
        /// 取得遊戲統計
        /// GET /api/minigame/statistics
        /// </summary>
        /// <returns>遊戲統計資訊</returns>
        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics()
        {
            try
            {
                var userId = GetCurrentUserId();
                _logger.LogInformation($"取得使用者 {userId} 的遊戲統計");

                var stats = await _miniGameService.GetGameStatisticsAsync(userId);

                return Ok(new
                {
                    success = true,
                    data = stats,
                    message = "遊戲統計取得成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得遊戲統計時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得遊戲統計時發生錯誤" });
            }
        }

        /// <summary>
        /// 取得當日遊戲狀態
        /// GET /api/minigame/daily-status
        /// </summary>
        /// <returns>當日遊戲狀態</returns>
        [HttpGet("daily-status")]
        public async Task<IActionResult> GetDailyStatus()
        {
            try
            {
                var userId = GetCurrentUserId();
                _logger.LogInformation($"取得使用者 {userId} 的當日遊戲狀態");

                var status = await _miniGameService.GetDailyGameStatusAsync(userId);

                return Ok(new
                {
                    success = true,
                    data = status,
                    message = "當日遊戲狀態取得成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得當日遊戲狀態時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得當日遊戲狀態時發生錯誤" });
            }
        }

        #endregion

        #region 關卡與設定

        /// <summary>
        /// 取得關卡設定
        /// GET /api/minigame/levels
        /// </summary>
        /// <returns>關卡設定列表</returns>
        [HttpGet("levels")]
        public async Task<IActionResult> GetLevels()
        {
            try
            {
                _logger.LogInformation("取得關卡設定");

                var levels = await _miniGameService.GetLevelConfigsAsync();

                return Ok(new
                {
                    success = true,
                    data = levels,
                    message = "關卡設定取得成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得關卡設定時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得關卡設定時發生錯誤" });
            }
        }

        /// <summary>
        /// 取得使用者當前關卡
        /// GET /api/minigame/current-level
        /// </summary>
        /// <returns>當前可挑戰關卡</returns>
        [HttpGet("current-level")]
        public async Task<IActionResult> GetCurrentLevel()
        {
            try
            {
                var userId = GetCurrentUserId();
                _logger.LogInformation($"取得使用者 {userId} 的當前關卡");

                var currentLevel = await _miniGameService.GetUserCurrentLevelAsync(userId);

                return Ok(new
                {
                    success = true,
                    data = new { currentLevel },
                    message = "當前關卡取得成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得當前關卡時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得當前關卡時發生錯誤" });
            }
        }

        #endregion

        #region 排行榜

        /// <summary>
        /// 取得遊戲排行榜
        /// GET /api/minigame/rankings?type=level&limit=50
        /// </summary>
        /// <param name="type">排行類型</param>
        /// <param name="limit">限制筆數</param>
        /// <returns>遊戲排行榜</returns>
        [HttpGet("rankings")]
        public async Task<IActionResult> GetRankings(
            [FromQuery] string type = "level",
            [FromQuery] int limit = 50)
        {
            try
            {
                _logger.LogInformation($"取得遊戲排行榜: {type}");

                if (!Enum.TryParse<GameRankingType>(type, true, out var rankingType))
                {
                    return BadRequest(new { success = false, message = "不支援的排行類型" });
                }

                var rankings = await _miniGameService.GetGameRankingsAsync(rankingType, Math.Min(limit, 100));

                return Ok(new
                {
                    success = true,
                    data = rankings,
                    message = "排行榜取得成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得遊戲排行榜時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得遊戲排行榜時發生錯誤" });
            }
        }

        #endregion

        #region 管理員功能

        /// <summary>
        /// 取得系統設定
        /// GET /api/minigame/admin/config
        /// </summary>
        /// <returns>系統設定</returns>
        [HttpGet("admin/config")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetSystemConfig()
        {
            try
            {
                _logger.LogInformation("取得小遊戲系統設定");

                var config = await _miniGameService.GetSystemConfigAsync();

                return Ok(new
                {
                    success = true,
                    data = config,
                    message = "系統設定取得成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得系統設定時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得系統設定時發生錯誤" });
            }
        }

        /// <summary>
        /// 管理員查詢所有遊戲記錄
        /// GET /api/minigame/admin/records
        /// </summary>
        /// <param name="userId">使用者ID篩選</param>
        /// <param name="username">使用者名稱篩選</param>
        /// <param name="from">開始日期</param>
        /// <param name="to">結束日期</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <returns>所有遊戲記錄</returns>
        [HttpGet("admin/records")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllRecords(
            [FromQuery] int? userId = null,
            [FromQuery] string? username = null,
            [FromQuery] DateTime? from = null,
            [FromQuery] DateTime? to = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            try
            {
                var adminId = GetCurrentUserId();
                _logger.LogInformation($"管理員 {adminId} 查詢所有遊戲記錄");

                var request = new AdminGameRecordsQueryDto
                {
                    UserId = userId,
                    Username = username,
                    FromDate = from,
                    ToDate = to,
                    Page = page,
                    PageSize = Math.Min(pageSize, 100)
                };

                var records = await _miniGameService.GetAllGameRecordsAsync(request);

                return Ok(new
                {
                    success = true,
                    data = records,
                    message = "管理員遊戲記錄取得成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "管理員查詢遊戲記錄時發生錯誤");
                return StatusCode(500, new { success = false, message = "管理員查詢遊戲記錄時發生錯誤" });
            }
        }

        #endregion

        #region 輔助方法

        /// <summary>
        /// 取得當前登入使用者的 ID
        /// </summary>
        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException("無法取得使用者身份資訊");
            }
            return userId;
        }

        #endregion
    }

    #region 請求 DTOs

    /// <summary>
    /// 中斷遊戲請求
    /// </summary>
    public class AbortGameRequest
    {
        /// <summary>中斷原因</summary>
        public string? Reason { get; set; }
    }

    #endregion
}