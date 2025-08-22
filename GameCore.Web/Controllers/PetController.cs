using GameCore.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameCore.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PetController : ControllerBase
    {
        private readonly IPetService _petService;
        private readonly ILogger<PetController> _logger;

        public PetController(IPetService petService, ILogger<PetController> logger)
        {
            _petService = petService;
            _logger = logger;
        }

        [HttpGet("status")]
        public async Task<IActionResult> GetPetStatus()
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
                var status = await _petService.GetPetStatusAsync(userId);
                
                if (status.Success)
                {
                    return Ok(status.Pet);
                }
                
                return NotFound(new { message = status.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting pet status for user {UserId}", User.FindFirst("UserId")?.Value);
                return StatusCode(500, new { message = "伺服器錯誤，請稍後再試" });
            }
        }

        [HttpPost("feed")]
        public async Task<IActionResult> FeedPet()
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
                var result = await _petService.FeedPetAsync(userId);
                
                if (result.Success)
                {
                    return Ok(new { 
                        message = result.Message, 
                        pet = result.Pet,
                        pointsGained = result.PointsGained,
                        experienceGained = result.ExperienceGained
                    });
                }
                
                return BadRequest(new { message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while feeding pet for user {UserId}", User.FindFirst("UserId")?.Value);
                return StatusCode(500, new { message = "伺服器錯誤，請稍後再試" });
            }
        }

        [HttpPost("clean")]
        public async Task<IActionResult> CleanPet()
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
                var result = await _petService.CleanPetAsync(userId);
                
                if (result.Success)
                {
                    return Ok(new { 
                        message = result.Message, 
                        pet = result.Pet,
                        pointsGained = result.PointsGained,
                        experienceGained = result.ExperienceGained
                    });
                }
                
                return BadRequest(new { message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while cleaning pet for user {UserId}", User.FindFirst("UserId")?.Value);
                return StatusCode(500, new { message = "伺服器錯誤，請稍後再試" });
            }
        }

        [HttpPost("play")]
        public async Task<IActionResult> PlayWithPet()
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
                var result = await _petService.PlayWithPetAsync(userId);
                
                if (result.Success)
                {
                    return Ok(new { 
                        message = result.Message, 
                        pet = result.Pet,
                        pointsGained = result.PointsGained,
                        experienceGained = result.ExperienceGained
                    });
                }
                
                return BadRequest(new { message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while playing with pet for user {UserId}", User.FindFirst("UserId")?.Value);
                return StatusCode(500, new { message = "伺服器錯誤，請稍後再試" });
            }
        }

        [HttpPost("rest")]
        public async Task<IActionResult> RestPet()
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
                var result = await _petService.RestPetAsync(userId);
                
                if (result.Success)
                {
                    return Ok(new { 
                        message = result.Message, 
                        pet = result.Pet
                    });
                }
                
                return BadRequest(new { message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while resting pet for user {UserId}", User.FindFirst("UserId")?.Value);
                return StatusCode(500, new { message = "伺服器錯誤，請稍後再試" });
            }
        }

        [HttpPost("change-color")]
        public async Task<IActionResult> ChangePetColor([FromBody] PetColorChangeDto colorChangeDto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
                var result = await _petService.ChangePetColorAsync(userId, colorChangeDto.Color);
                
                if (result.Success)
                {
                    return Ok(new { 
                        message = result.Message, 
                        pet = result.Pet
                    });
                }
                
                return BadRequest(new { message = result.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while changing pet color for user {UserId}", User.FindFirst("UserId")?.Value);
                return StatusCode(500, new { message = "伺服器錯誤，請稍後再試" });
            }
        }

        [HttpGet("leaderboard")]
        public async Task<IActionResult> GetPetLeaderboard([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                // This would need implementation in PetService
                // For now, return a placeholder response with mock data
                var leaderboard = new
                {
                    pets = new[]
                    {
                        new { name = "小龍", level = 25, owner = "玩家1", species = "龍", color = "紅色" },
                        new { name = "小貓", level = 23, owner = "玩家2", species = "貓", color = "橘色" },
                        new { name = "小狗", level = 22, owner = "玩家3", species = "狗", color = "棕色" },
                        new { name = "小鳥", level = 20, owner = "玩家4", species = "鳥", color = "藍色" },
                        new { name = "小魚", level = 18, owner = "玩家5", species = "魚", color = "金色" }
                    },
                    currentPage = page,
                    totalPages = 5,
                    totalCount = 100
                };
                
                return Ok(leaderboard);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting pet leaderboard");
                return StatusCode(500, new { message = "伺服器錯誤，請稍後再試" });
            }
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetPetHistory([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
                
                // This would need implementation in PetService
                // For now, return a placeholder response
                var history = new
                {
                    activities = new[]
                    {
                        new { action = "餵食", timestamp = DateTime.UtcNow.AddHours(-1), points = 10, experience = 5 },
                        new { action = "清理", timestamp = DateTime.UtcNow.AddHours(-3), points = 8, experience = 4 },
                        new { action = "遊戲", timestamp = DateTime.UtcNow.AddHours(-5), points = 12, experience = 6 },
                        new { action = "休息", timestamp = DateTime.UtcNow.AddHours(-7), points = 0, experience = 0 }
                    },
                    currentPage = page,
                    totalPages = 3,
                    totalCount = 50
                };
                
                return Ok(history);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting pet history for user {UserId}", User.FindFirst("UserId")?.Value);
                return StatusCode(500, new { message = "伺服器錯誤，請稍後再試" });
            }
        }

        [HttpGet("available-colors")]
        public async Task<IActionResult> GetAvailableColors()
        {
            try
            {
                var colors = new[]
                {
                    new { name = "紅色", value = "red", cost = 0, unlocked = true },
                    new { name = "藍色", value = "blue", cost = 100, unlocked = false },
                    new { name = "綠色", value = "green", cost = 150, unlocked = false },
                    new { name = "黃色", value = "yellow", cost = 200, unlocked = false },
                    new { name = "紫色", value = "purple", cost = 300, unlocked = false },
                    new { name = "橘色", value = "orange", cost = 250, unlocked = false },
                    new { name = "粉色", value = "pink", cost = 350, unlocked = false },
                    new { name = "黑色", value = "black", cost = 500, unlocked = false },
                    new { name = "白色", value = "white", cost = 400, unlocked = false },
                    new { name = "金色", value = "gold", cost = 1000, unlocked = false }
                };
                
                return Ok(colors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting available colors");
                return StatusCode(500, new { message = "伺服器錯誤，請稍後再試" });
            }
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetPetStats()
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
                
                // This would need implementation in PetService
                // For now, return a placeholder response
                var stats = new
                {
                    totalFeeds = 45,
                    totalCleans = 32,
                    totalPlays = 28,
                    totalRests = 15,
                    daysAlive = 30,
                    totalExperience = 2500,
                    totalLevels = 5,
                    healthAverage = 85,
                    happinessAverage = 90,
                    energyAverage = 75
                };
                
                return Ok(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting pet stats for user {UserId}", User.FindFirst("UserId")?.Value);
                return StatusCode(500, new { message = "伺服器錯誤，請稍後再試" });
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePet([FromBody] PetCreateDto createDto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
                
                // This would need implementation in PetService
                // For now, return a placeholder response
                return Ok(new { message = "寵物創建功能開發中" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating pet for user {UserId}", User.FindFirst("UserId")?.Value);
                return StatusCode(500, new { message = "伺服器錯誤，請稍後再試" });
            }
        }

        [HttpGet("needs")]
        public async Task<IActionResult> GetPetNeeds()
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
                
                // This would simulate getting pet needs/requirements
                var needs = new
                {
                    hunger = new { level = 65, urgent = false, nextFeedIn = TimeSpan.FromHours(2) },
                    cleanliness = new { level = 40, urgent = true, nextCleanIn = TimeSpan.FromMinutes(30) },
                    happiness = new { level = 80, urgent = false, nextPlayIn = TimeSpan.FromHours(1) },
                    energy = new { level = 30, urgent = true, nextRestIn = TimeSpan.FromMinutes(15) },
                    recommendations = new[]
                    {
                        "你的寵物需要清理！",
                        "你的寵物需要休息！",
                        "2小時後記得餵食"
                    }
                };
                
                return Ok(needs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting pet needs for user {UserId}", User.FindFirst("UserId")?.Value);
                return StatusCode(500, new { message = "伺服器錯誤，請稍後再試" });
            }
        }
    }

    // DTOs for API requests
    public class PetColorChangeDto
    {
        public string Color { get; set; } = string.Empty;
    }

    public class PetCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Species { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
    }
}