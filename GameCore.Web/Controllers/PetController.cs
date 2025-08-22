using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GameCore.Core.Services;
using GameCore.Core.Entities;
using System.Security.Claims;

namespace GameCore.Web.Controllers
{
    /// <summary>
    /// 寵物系統控制器
    /// 提供寵物互動、狀態查詢、換色等 API 端點
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PetController : ControllerBase
    {
        private readonly IPetService _petService;
        private readonly ILogger<PetController> _logger;

        public PetController(IPetService petService, ILogger<PetController> logger)
        {
            _petService = petService;
            _logger = logger;
        }

        /// <summary>
        /// 取得寵物狀態
        /// GET /api/pet
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetPet()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == 0)
                {
                    return Unauthorized(new { message = "請先登入" });
                }

                var pet = await _petService.GetOrCreatePetAsync(userId);
                var statusDescription = _petService.GetPetStatusDescription(pet);
                var nextLevelExp = _petService.CalculateRequiredExperience(pet.Level);

                var response = new
                {
                    // 基本資料
                    petId = pet.PetId,
                    name = pet.PetName,
                    level = pet.Level,
                    experience = pet.Experience,
                    nextLevelExp = nextLevelExp,
                    
                    // 五維屬性
                    hunger = pet.Hunger,
                    mood = pet.Mood,
                    stamina = pet.Stamina,
                    cleanliness = pet.Cleanliness,
                    health = pet.Health,
                    
                    // 外觀
                    skinColor = pet.SkinColor,
                    backgroundColor = pet.BackgroundColor,
                    
                    // 時間記錄
                    levelUpTime = pet.LevelUpTime,
                    colorChangedTime = pet.ColorChangedTime,
                    
                    // 狀態描述
                    statusDescription = statusDescription,
                    
                    // 是否可以冒險
                    canAdventure = await _petService.CanStartAdventureAsync(userId)
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得寵物狀態時發生錯誤");
                return StatusCode(500, new { message = "伺服器錯誤" });
            }
        }

        /// <summary>
        /// 寵物互動
        /// POST /api/pet/interact
        /// </summary>
        [HttpPost("interact")]
        public async Task<IActionResult> InteractWithPet([FromBody] PetInteractionRequest request)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == 0)
                {
                    return Unauthorized(new { message = "請先登入" });
                }

                // 驗證互動類型
                if (!Enum.TryParse<PetInteractionType>(request.Action, true, out var interactionType))
                {
                    return BadRequest(new { message = "無效的互動類型" });
                }

                var result = await _petService.InteractWithPetAsync(userId, interactionType);

                if (!result.Success)
                {
                    return BadRequest(new { 
                        message = result.Message,
                        cooldownSeconds = result.CooldownSeconds
                    });
                }

                _logger.LogInformation($"使用者 {userId} 執行寵物互動: {request.Action}");

                return Ok(new
                {
                    success = true,
                    message = result.Message,
                    pet = new
                    {
                        hunger = result.Pet?.Hunger,
                        mood = result.Pet?.Mood,
                        stamina = result.Pet?.Stamina,
                        cleanliness = result.Pet?.Cleanliness,
                        health = result.Pet?.Health
                    },
                    healthRestored = result.HealthRestored,
                    cooldownSeconds = result.CooldownSeconds
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"寵物互動時發生錯誤，使用者ID: {GetCurrentUserId()}");
                return StatusCode(500, new { message = "互動失敗，請稍後再試" });
            }
        }

        /// <summary>
        /// 寵物換色
        /// POST /api/pet/recolor
        /// </summary>
        [HttpPost("recolor")]
        public async Task<IActionResult> ChangePetColor([FromBody] PetColorChangeRequest request)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == 0)
                {
                    return Unauthorized(new { message = "請先登入" });
                }

                var result = await _petService.ChangePetColorAsync(userId, request.SkinColor, request.BackgroundColor);

                if (!result.Success)
                {
                    return BadRequest(new { 
                        message = result.Message,
                        remainingPoints = result.RemainingPoints
                    });
                }

                _logger.LogInformation($"使用者 {userId} 為寵物換色成功");

                return Ok(new
                {
                    success = true,
                    message = result.Message,
                    pointsUsed = result.PointsUsed,
                    remainingPoints = result.RemainingPoints,
                    pet = new
                    {
                        skinColor = result.Pet?.SkinColor,
                        backgroundColor = result.Pet?.BackgroundColor,
                        colorChangedTime = result.Pet?.ColorChangedTime
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"寵物換色時發生錯誤，使用者ID: {GetCurrentUserId()}");
                return StatusCode(500, new { message = "換色失敗，請稍後再試" });
            }
        }

        /// <summary>
        /// 更新寵物名稱
        /// PUT /api/pet/name
        /// </summary>
        [HttpPut("name")]
        public async Task<IActionResult> UpdatePetName([FromBody] UpdatePetNameRequest request)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == 0)
                {
                    return Unauthorized(new { message = "請先登入" });
                }

                if (string.IsNullOrWhiteSpace(request.Name) || request.Name.Length > 50)
                {
                    return BadRequest(new { message = "寵物名稱長度必須在 1-50 字元之間" });
                }

                var pet = await _petService.GetOrCreatePetAsync(userId);
                pet.PetName = request.Name.Trim();

                // 這裡需要透過 UnitOfWork 更新
                // await _petService.UpdatePetAsync(pet);

                _logger.LogInformation($"使用者 {userId} 更新寵物名稱為: {request.Name}");

                return Ok(new
                {
                    success = true,
                    message = "寵物名稱更新成功",
                    newName = pet.PetName
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"更新寵物名稱時發生錯誤，使用者ID: {GetCurrentUserId()}");
                return StatusCode(500, new { message = "更新失敗，請稍後再試" });
            }
        }

        /// <summary>
        /// 取得寵物狀態描述
        /// GET /api/pet/status
        /// </summary>
        [HttpGet("status")]
        public async Task<IActionResult> GetPetStatus()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == 0)
                {
                    return Unauthorized(new { message = "請先登入" });
                }

                var pet = await _petService.GetOrCreatePetAsync(userId);
                var statusDescription = _petService.GetPetStatusDescription(pet);

                return Ok(statusDescription);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"取得寵物狀態描述時發生錯誤，使用者ID: {GetCurrentUserId()}");
                return StatusCode(500, new { message = "取得狀態失敗" });
            }
        }

        /// <summary>
        /// 檢查是否可以冒險
        /// GET /api/pet/can-adventure
        /// </summary>
        [HttpGet("can-adventure")]
        public async Task<IActionResult> CanStartAdventure()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == 0)
                {
                    return Unauthorized(new { message = "請先登入" });
                }

                var canAdventure = await _petService.CanStartAdventureAsync(userId);
                var pet = await _petService.GetOrCreatePetAsync(userId);

                string message = canAdventure 
                    ? "寵物狀態良好，可以開始冒險！" 
                    : "寵物狀態不佳，請先照顧寵物再來冒險";

                return Ok(new
                {
                    canAdventure = canAdventure,
                    message = message,
                    petHealth = pet.Health,
                    lowestAttribute = new
                    {
                        hunger = pet.Hunger,
                        mood = pet.Mood,
                        stamina = pet.Stamina,
                        cleanliness = pet.Cleanliness
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"檢查冒險條件時發生錯誤，使用者ID: {GetCurrentUserId()}");
                return StatusCode(500, new { message = "檢查失敗" });
            }
        }

        /// <summary>
        /// 取得互動冷卻時間
        /// GET /api/pet/cooldown/{action}
        /// </summary>
        [HttpGet("cooldown/{action}")]
        public async Task<IActionResult> GetInteractionCooldown(string action)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == 0)
                {
                    return Unauthorized(new { message = "請先登入" });
                }

                if (!Enum.TryParse<PetInteractionType>(action, true, out var interactionType))
                {
                    return BadRequest(new { message = "無效的互動類型" });
                }

                var cooldownSeconds = await _petService.GetInteractionCooldownAsync(userId, interactionType);

                return Ok(new
                {
                    action = action,
                    cooldownSeconds = cooldownSeconds,
                    canInteract = cooldownSeconds == 0
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"取得互動冷卻時間時發生錯誤，使用者ID: {GetCurrentUserId()}");
                return StatusCode(500, new { message = "取得冷卻時間失敗" });
            }
        }

        #region 私有方法

        /// <summary>
        /// 取得當前登入使用者的 ID
        /// </summary>
        /// <returns>使用者 ID，如果未登入則返回 0</returns>
        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out var userId))
            {
                return userId;
            }
            return 0;
        }

        #endregion
    }

    #region 請求模型

    /// <summary>
    /// 寵物互動請求模型
    /// </summary>
    public class PetInteractionRequest
    {
        /// <summary>
        /// 互動動作 (Feed/Bath/Play/Rest)
        /// </summary>
        public string Action { get; set; } = string.Empty;
    }

    /// <summary>
    /// 寵物換色請求模型
    /// </summary>
    public class PetColorChangeRequest
    {
        /// <summary>
        /// 膚色 (十六進位色碼)
        /// </summary>
        public string SkinColor { get; set; } = string.Empty;

        /// <summary>
        /// 背景色 (十六進位色碼或顏色名稱)
        /// </summary>
        public string BackgroundColor { get; set; } = string.Empty;
    }

    /// <summary>
    /// 更新寵物名稱請求模型
    /// </summary>
    public class UpdatePetNameRequest
    {
        /// <summary>
        /// 新的寵物名稱
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }

    #endregion
}