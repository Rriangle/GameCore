using GameCore.Core.DTOs;
using GameCore.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GameCore.Web.Controllers
{
    /// <summary>
    /// 虛擬寵物控制器 - 完整實現史萊姆寵物系統API
    /// 提供寵物管理、互動行為、等級系統、換色功能等完整端點
    /// 嚴格按照規格要求實現所有API功能和業務邏輯
    /// </summary>
    [ApiController]
    [Route("api/pet")]
    [Authorize] // 所有寵物功能都需要登入
    public class VirtualPetController : ControllerBase
    {
        private readonly IPetService _petService;
        private readonly ILogger<VirtualPetController> _logger;

        public VirtualPetController(
            IPetService petService,
            ILogger<VirtualPetController> logger)
        {
            _petService = petService;
            _logger = logger;
        }

        #region 寵物基本管理

        /// <summary>
        /// 取得寵物完整資訊
        /// GET /api/pet
        /// </summary>
        /// <returns>寵物完整資訊，包含5維屬性、等級經驗、顏色設定等</returns>
        [HttpGet]
        public async Task<IActionResult> GetPet()
        {
            try
            {
                var userId = GetCurrentUserId();
                _logger.LogInformation($"取得使用者 {userId} 的寵物資訊");

                var pet = await _petService.GetUserPetAsync(userId);

                if (pet == null)
                {
                    return Ok(new
                    {
                        success = true,
                        hasPet = false,
                        message = "您還沒有寵物，是否要建立一隻可愛的史萊姆？"
                    });
                }

                return Ok(new
                {
                    success = true,
                    hasPet = true,
                    data = pet,
                    message = "寵物資訊取得成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得寵物資訊時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得寵物資訊時發生錯誤" });
            }
        }

        /// <summary>
        /// 建立新寵物
        /// POST /api/pet
        /// </summary>
        /// <param name="request">建立寵物請求</param>
        /// <returns>建立的寵物資訊</returns>
        [HttpPost]
        public async Task<IActionResult> CreatePet([FromBody] CreatePetRequest request)
        {
            try
            {
                var userId = GetCurrentUserId();
                _logger.LogInformation($"使用者 {userId} 建立寵物：{request?.PetName ?? "小可愛"}");

                var pet = await _petService.CreatePetAsync(userId, request?.PetName ?? "小可愛");

                return Ok(new
                {
                    success = true,
                    data = pet,
                    message = "恭喜！您的史萊姆寵物建立成功！"
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "建立寵物時發生錯誤");
                return StatusCode(500, new { success = false, message = "建立寵物時發生錯誤" });
            }
        }

        /// <summary>
        /// 更新寵物基本資料
        /// PUT /api/pet/profile
        /// </summary>
        /// <param name="updateRequest">更新請求</param>
        /// <returns>更新結果</returns>
        [HttpPut("profile")]
        public async Task<IActionResult> UpdatePetProfile([FromBody] UpdatePetProfileDto updateRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { success = false, message = "資料格式錯誤", errors = ModelState });
                }

                var userId = GetCurrentUserId();
                _logger.LogInformation($"使用者 {userId} 更新寵物資料");

                var result = await _petService.UpdatePetProfileAsync(userId, updateRequest);

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
                _logger.LogError(ex, "更新寵物資料時發生錯誤");
                return StatusCode(500, new { success = false, message = "更新寵物資料時發生錯誤" });
            }
        }

        #endregion

        #region 寵物互動行為

        /// <summary>
        /// 餵食寵物
        /// POST /api/pet/actions/feed
        /// </summary>
        /// <returns>互動結果</returns>
        [HttpPost("actions/feed")]
        public async Task<IActionResult> FeedPet()
        {
            try
            {
                var userId = GetCurrentUserId();
                _logger.LogInformation($"使用者 {userId} 餵食寵物");

                var result = await _petService.FeedPetAsync(userId);

                if (result.Success)
                {
                    return Ok(new
                    {
                        success = true,
                        data = result,
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
                _logger.LogError(ex, "餵食寵物時發生錯誤");
                return StatusCode(500, new { success = false, message = "餵食寵物時發生錯誤" });
            }
        }

        /// <summary>
        /// 幫寵物洗澡
        /// POST /api/pet/actions/bathe
        /// </summary>
        /// <returns>互動結果</returns>
        [HttpPost("actions/bathe")]
        public async Task<IActionResult> BathePet()
        {
            try
            {
                var userId = GetCurrentUserId();
                _logger.LogInformation($"使用者 {userId} 幫寵物洗澡");

                var result = await _petService.BathePetAsync(userId);

                if (result.Success)
                {
                    return Ok(new
                    {
                        success = true,
                        data = result,
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
                _logger.LogError(ex, "幫寵物洗澡時發生錯誤");
                return StatusCode(500, new { success = false, message = "幫寵物洗澡時發生錯誤" });
            }
        }

        /// <summary>
        /// 與寵物玩耍
        /// POST /api/pet/actions/play
        /// </summary>
        /// <returns>互動結果</returns>
        [HttpPost("actions/play")]
        public async Task<IActionResult> PlayWithPet()
        {
            try
            {
                var userId = GetCurrentUserId();
                _logger.LogInformation($"使用者 {userId} 與寵物玩耍");

                var result = await _petService.PlayWithPetAsync(userId);

                if (result.Success)
                {
                    return Ok(new
                    {
                        success = true,
                        data = result,
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
                _logger.LogError(ex, "與寵物玩耍時發生錯誤");
                return StatusCode(500, new { success = false, message = "與寵物玩耍時發生錯誤" });
            }
        }

        /// <summary>
        /// 讓寵物休息
        /// POST /api/pet/actions/rest
        /// </summary>
        /// <returns>互動結果</returns>
        [HttpPost("actions/rest")]
        public async Task<IActionResult> RestPet()
        {
            try
            {
                var userId = GetCurrentUserId();
                _logger.LogInformation($"使用者 {userId} 讓寵物休息");

                var result = await _petService.RestPetAsync(userId);

                if (result.Success)
                {
                    return Ok(new
                    {
                        success = true,
                        data = result,
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
                _logger.LogError(ex, "讓寵物休息時發生錯誤");
                return StatusCode(500, new { success = false, message = "讓寵物休息時發生錯誤" });
            }
        }

        #endregion

        #region 寵物顏色系統

        /// <summary>
        /// 取得可用顏色選項
        /// GET /api/pet/colors
        /// </summary>
        /// <returns>可用的寵物顏色選項</returns>
        [HttpGet("colors")]
        public async Task<IActionResult> GetAvailableColors()
        {
            try
            {
                _logger.LogInformation("取得可用寵物顏色選項");

                var colors = await _petService.GetAvailableColorsAsync();

                return Ok(new
                {
                    success = true,
                    data = colors,
                    message = "顏色選項取得成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得可用顏色時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得可用顏色時發生錯誤" });
            }
        }

        /// <summary>
        /// 寵物換色
        /// POST /api/pet/recolor
        /// </summary>
        /// <param name="recolorRequest">換色請求</param>
        /// <returns>換色結果</returns>
        [HttpPost("recolor")]
        public async Task<IActionResult> RecolorPet([FromBody] PetRecolorDto recolorRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { success = false, message = "資料格式錯誤", errors = ModelState });
                }

                var userId = GetCurrentUserId();
                _logger.LogInformation($"使用者 {userId} 要求寵物換色");

                var result = await _petService.RecolorPetAsync(userId, recolorRequest);

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
                _logger.LogError(ex, "寵物換色時發生錯誤");
                return StatusCode(500, new { success = false, message = "寵物換色時發生錯誤" });
            }
        }

        /// <summary>
        /// 取得換色歷史
        /// GET /api/pet/colors/history
        /// </summary>
        /// <returns>換色歷史記錄</returns>
        [HttpGet("colors/history")]
        public async Task<IActionResult> GetColorHistory()
        {
            try
            {
                var userId = GetCurrentUserId();
                _logger.LogInformation($"取得使用者 {userId} 的換色歷史");

                var history = await _petService.GetColorHistoryAsync(userId);

                return Ok(new
                {
                    success = true,
                    data = history,
                    message = "換色歷史取得成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得換色歷史時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得換色歷史時發生錯誤" });
            }
        }

        #endregion

        #region 寵物等級與經驗

        /// <summary>
        /// 取得等級統計
        /// GET /api/pet/level/stats
        /// </summary>
        /// <returns>等級統計資訊</returns>
        [HttpGet("level/stats")]
        public async Task<IActionResult> GetLevelStats()
        {
            try
            {
                var userId = GetCurrentUserId();
                _logger.LogInformation($"取得使用者 {userId} 的寵物等級統計");

                var stats = await _petService.GetLevelStatsAsync(userId);

                return Ok(new
                {
                    success = true,
                    data = stats,
                    message = "等級統計取得成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得等級統計時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得等級統計時發生錯誤" });
            }
        }

        /// <summary>
        /// 增加經驗值 (內部API，由其他系統調用)
        /// POST /api/pet/experience
        /// </summary>
        /// <param name="request">經驗增加請求</param>
        /// <returns>經驗增加結果</returns>
        [HttpPost("experience")]
        public async Task<IActionResult> AddExperience([FromBody] AddExperienceRequest request)
        {
            try
            {
                var userId = GetCurrentUserId();
                _logger.LogInformation($"為使用者 {userId} 寵物增加經驗值: {request.Experience}");

                var result = await _petService.AddExperienceAsync(userId, request.Experience, request.Source);

                if (result.Success)
                {
                    return Ok(new
                    {
                        success = true,
                        data = result,
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
                _logger.LogError(ex, "增加寵物經驗時發生錯誤");
                return StatusCode(500, new { success = false, message = "增加寵物經驗時發生錯誤" });
            }
        }

        #endregion

        #region 寵物狀態檢查

        /// <summary>
        /// 檢查冒險準備度
        /// GET /api/pet/adventure/readiness
        /// </summary>
        /// <returns>冒險準備度檢查結果</returns>
        [HttpGet("adventure/readiness")]
        public async Task<IActionResult> CheckAdventureReadiness()
        {
            try
            {
                var userId = GetCurrentUserId();
                _logger.LogInformation($"檢查使用者 {userId} 寵物的冒險準備度");

                var readiness = await _petService.CheckAdventureReadinessAsync(userId);

                return Ok(new
                {
                    success = true,
                    data = readiness,
                    message = readiness.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "檢查冒險準備度時發生錯誤");
                return StatusCode(500, new { success = false, message = "檢查冒險準備度時發生錯誤" });
            }
        }

        #endregion

        #region 寵物統計與排行

        /// <summary>
        /// 取得寵物統計
        /// GET /api/pet/statistics
        /// </summary>
        /// <returns>寵物統計資訊</returns>
        [HttpGet("statistics")]
        public async Task<IActionResult> GetPetStatistics()
        {
            try
            {
                var userId = GetCurrentUserId();
                _logger.LogInformation($"取得使用者 {userId} 的寵物統計");

                var stats = await _petService.GetPetStatisticsAsync(userId);

                return Ok(new
                {
                    success = true,
                    data = stats,
                    message = "寵物統計取得成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得寵物統計時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得寵物統計時發生錯誤" });
            }
        }

        /// <summary>
        /// 取得寵物排行榜
        /// GET /api/pet/rankings?type=level&limit=50
        /// </summary>
        /// <param name="type">排行類型</param>
        /// <param name="limit">限制筆數</param>
        /// <returns>寵物排行榜</returns>
        [HttpGet("rankings")]
        public async Task<IActionResult> GetPetRankings(
            [FromQuery] string type = "level",
            [FromQuery] int limit = 50)
        {
            try
            {
                _logger.LogInformation($"取得寵物排行榜: {type}");

                if (!Enum.TryParse<PetRankingType>(type, true, out var rankingType))
                {
                    return BadRequest(new { success = false, message = "不支援的排行類型" });
                }

                var rankings = await _petService.GetPetRankingsAsync(rankingType, Math.Min(limit, 100));

                return Ok(new
                {
                    success = true,
                    data = rankings,
                    message = "排行榜取得成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得寵物排行榜時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得寵物排行榜時發生錯誤" });
            }
        }

        #endregion

        #region 管理員功能

        /// <summary>
        /// 取得系統設定
        /// GET /api/pet/admin/config
        /// </summary>
        /// <returns>系統設定</returns>
        [HttpGet("admin/config")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetSystemConfig()
        {
            try
            {
                _logger.LogInformation("取得寵物系統設定");

                var config = await _petService.GetSystemConfigAsync();

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
        /// 管理員重置寵物
        /// POST /api/pet/admin/reset/{petId}
        /// </summary>
        /// <param name="petId">寵物ID</param>
        /// <param name="resetRequest">重置請求</param>
        /// <returns>重置結果</returns>
        [HttpPost("admin/reset/{petId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminResetPet(int petId, [FromBody] PetAdminResetDto resetRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { success = false, message = "資料格式錯誤", errors = ModelState });
                }

                var adminId = GetCurrentUserId();
                _logger.LogInformation($"管理員 {adminId} 重置寵物 {petId}");

                var result = await _petService.AdminResetPetAsync(petId, resetRequest);

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
                _logger.LogError(ex, $"管理員重置寵物 {petId} 時發生錯誤");
                return StatusCode(500, new { success = false, message = "重置寵物時發生錯誤" });
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
    /// 建立寵物請求
    /// </summary>
    public class CreatePetRequest
    {
        /// <summary>寵物名稱</summary>
        public string? PetName { get; set; }
    }

    /// <summary>
    /// 增加經驗請求
    /// </summary>
    public class AddExperienceRequest
    {
        /// <summary>增加的經驗值</summary>
        public int Experience { get; set; }

        /// <summary>經驗來源</summary>
        public string Source { get; set; } = string.Empty;
    }

    #endregion
}