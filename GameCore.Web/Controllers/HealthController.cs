using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameCore.Infrastructure.Data;

namespace GameCore.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly GameCoreDbContext _context;
        private readonly ILogger<HealthController> _logger;

        public HealthController(GameCoreDbContext context, ILogger<HealthController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var healthStatus = new
                {
                    Status = "Healthy",
                    Timestamp = DateTime.UtcNow,
                    Version = "1.0.0",
                    Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"
                };

                return Ok(healthStatus);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "健康檢查失敗");
                return StatusCode(500, new { Status = "Unhealthy", Error = ex.Message });
            }
        }

        [HttpGet("database")]
        public async Task<IActionResult> CheckDatabase()
        {
            try
            {
                // 檢查資料庫連接
                var canConnect = await _context.Database.CanConnectAsync();
                
                if (!canConnect)
                {
                    return StatusCode(503, new { Status = "Database Unavailable", Message = "無法連接到資料庫" });
                }

                // 檢查基本查詢
                var userCount = await _context.Users.CountAsync();
                var gameCount = await _context.Games.CountAsync();

                var dbStatus = new
                {
                    Status = "Healthy",
                    Database = "Connected",
                    UserCount = userCount,
                    GameCount = gameCount,
                    Timestamp = DateTime.UtcNow
                };

                return Ok(dbStatus);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "資料庫健康檢查失敗");
                return StatusCode(503, new { Status = "Database Error", Error = ex.Message });
            }
        }
    }
}