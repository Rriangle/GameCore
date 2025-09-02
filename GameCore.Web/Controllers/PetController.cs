using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GameCore.Core.Services;
using GameCore.Core.DTOs;
using System.Security.Claims;

namespace GameCore.Web.Controllers
{
    /// <summary>
    /// 寵物控制器
    /// 提供寵物養成、互動、冒險等功能
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PetController : ControllerBase
    {
        private readonly IPetService _petService;
        private readonly ILogger<PetController> _logger;

        public PetController(
            IPetService petService,
            ILogger<PetController> logger)
        {
            _petService = petService;
            _logger = logger;
        }

        /// <summary>
        /// 取得我的寵物
        /// </summary>
        [HttpGet("me")]
        public async Task<IActionResult> GetMyPet()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var pet = await _petService.GetOrCreatePetAsync(userId);

                return Ok(new
                {
                    success = true,
                    data = pet,
                    message = "取得寵物資訊成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得寵物資訊失敗");
                return StatusCode(500, new
                {
                    success = false,
                    message = "取得寵物資訊失敗"
                });
            }
        }

        /// <summary>
        /// 與寵物互動
        /// </summary>
        [HttpPost("interact")]
        public async Task<IActionResult> InteractWithPet([FromBody] PetInteractionRequest request)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var success = await _petService.InteractWithPetAsync(userId, request.InteractionType);

                if (!success)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "互動失敗"
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "互動成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "寵物互動失敗");
                return StatusCode(500, new
                {
                    success = false,
                    message = "寵物互動失敗"
                });
            }
        }

        /// <summary>
        /// 變更寵物顏色
        /// </summary>
        [HttpPost("recolor")]
        public async Task<IActionResult> ChangePetColor([FromBody] PetColorChangeRequest request)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var result = await _petService.ChangePetColorAsync(userId, request.SkinColor, request.BackgroundColor);

                if (!result.Success)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = result.Message
                    });
                }

                return Ok(new
                {
                    success = true,
                    data = result,
                    message = "變更顏色成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "變更寵物顏色失敗");
                return StatusCode(500, new
                {
                    success = false,
                    message = "變更寵物顏色失敗"
                });
            }
        }

        /// <summary>
        /// 開始冒險
        /// </summary>
        [HttpPost("adventure/start")]
        public async Task<IActionResult> StartAdventure()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var canStart = await _petService.CanStartAdventureAsync(userId);

                if (!canStart)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "無法開始冒險"
                    });
                }

                // 這裡應該創建一個冒險記錄
                return Ok(new
                {
                    success = true,
                    message = "冒險開始"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "開始冒險失敗");
                return StatusCode(500, new
                {
                    success = false,
                    message = "開始冒險失敗"
                });
            }
        }

        /// <summary>
        /// 結束冒險
        /// </summary>
        [HttpPost("adventure/finish")]
        public async Task<IActionResult> FinishAdventure([FromBody] AdventureFinishRequest request)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                
                // 這裡應該更新冒險記錄並給予獎勵
                return Ok(new
                {
                    success = true,
                    data = new
                    {
                        result = request.Result,
                        expGained = request.Result == "Win" ? 100 : 20,
                        pointsGained = request.Result == "Win" ? 10 : 2
                    },
                    message = "冒險結束"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "結束冒險失敗");
                return StatusCode(500, new
                {
                    success = false,
                    message = "結束冒險失敗"
                });
            }
        }

        /// <summary>
        /// 取得寵物狀態描述
        /// </summary>
        [HttpGet("status")]
        public async Task<IActionResult> GetPetStatus()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var pet = await _petService.GetOrCreatePetAsync(userId);
                var status = _petService.GetPetStatusDescription(pet);

                return Ok(new
                {
                    success = true,
                    data = status,
                    message = "取得寵物狀態成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得寵物狀態失敗");
                return StatusCode(500, new
                {
                    success = false,
                    message = "取得寵物狀態失敗"
                });
            }
        }

        /// <summary>
        /// 取得冒險記錄
        /// </summary>
        [HttpGet("adventure/records")]
        public async Task<IActionResult> GetAdventureRecords()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                
                // 這裡應該從資料庫取得冒險記錄
                var records = new List<object>();

                return Ok(new
                {
                    success = true,
                    data = records,
                    message = "取得冒險記錄成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得冒險記錄失敗");
                return StatusCode(500, new
                {
                    success = false,
                    message = "取得冒險記錄失敗"
                });
            }
        }
    }

    /// <summary>
    /// 寵物互動請求
    /// </summary>
    public class PetInteractionRequest
    {
        public string InteractionType { get; set; } = string.Empty;
    }

    /// <summary>
    /// 寵物顏色變更請求
    /// </summary>
    public class PetColorChangeRequest
    {
        public string SkinColor { get; set; } = string.Empty;
        public string BackgroundColor { get; set; } = string.Empty;
    }

    /// <summary>
    /// 冒險結束請求
    /// </summary>
    public class AdventureFinishRequest
    {
        public string Result { get; set; } = string.Empty; // Win, Lose, Abort
        public int Duration { get; set; } = 0; // 冒險持續時間（秒）
    }
}