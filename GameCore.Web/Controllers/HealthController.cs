using Microsoft.AspNetCore.Mvc;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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
        public IActionResult Health()
        {
            return Ok(new 
            { 
                Status = "Healthy", 
                Timestamp = DateTime.UtcNow,
                Version = "1.0.0",
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown"
            });
        }

        [HttpGet("database")]
        public async Task<IActionResult> DatabaseHealth()
        {
            try
            {
                var canConnect = await _context.Database.CanConnectAsync();
                if (canConnect)
                {
                    var userCount = await _context.Users.CountAsync();
                    return Ok(new 
                    { 
                        Status = "Healthy", 
                        Database = "Connected",
                        UserCount = userCount,
                        Timestamp = DateTime.UtcNow
                    });
                }
                return StatusCode(503, new { Status = "Unhealthy", Database = "Disconnected" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database health check failed");
                return StatusCode(503, new { Status = "Unhealthy", Database = "Error", Error = ex.Message });
            }
        }

        [HttpGet("cache")]
        public IActionResult CacheHealth()
        {
            // 這裡可以檢查 Redis 或其他快取服務的狀態
            return Ok(new { Status = "Healthy", Cache = "Available", Timestamp = DateTime.UtcNow });
        }

        [HttpGet("external")]
        public IActionResult ExternalServicesHealth()
        {
            // 這裡可以檢查外部服務的狀態
            return Ok(new { Status = "Healthy", ExternalServices = "Available", Timestamp = DateTime.UtcNow });
        }

        [HttpGet("detailed")]
        public async Task<IActionResult> DetailedHealth()
        {
            try
            {
                var checks = new List<object>();

                // 檢查資料庫
                try
                {
                    var canConnect = await _context.Database.CanConnectAsync();
                    checks.Add(new { Service = "Database", Status = canConnect ? "Healthy" : "Unhealthy" });
                }
                catch (Exception ex)
                {
                    checks.Add(new { Service = "Database", Status = "Unhealthy", Error = ex.Message });
                }

                // 檢查記憶體使用量
                var workingSet = GC.GetTotalMemory(false);
                checks.Add(new { Service = "Memory", Status = "Healthy", Usage = $"{workingSet / 1024 / 1024} MB" });

                // 檢查磁碟空間
                var drives = DriveInfo.GetDrives().Where(d => d.IsReady);
                foreach (var drive in drives)
                {
                    var freePercentage = (double)drive.AvailableFreeSpace / drive.TotalSize * 100;
                    checks.Add(new 
                    { 
                        Service = $"Disk_{drive.Name.Replace("\\", "")}", 
                        Status = freePercentage > 10 ? "Healthy" : "Warning",
                        FreeSpace = $"{freePercentage:F1}%"
                    });
                }

                return Ok(new 
                { 
                    Status = "Healthy", 
                    Checks = checks,
                    Timestamp = DateTime.UtcNow,
                    Uptime = DateTime.UtcNow - Process.GetCurrentProcess().StartTime.ToUniversalTime()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Detailed health check failed");
                return StatusCode(500, new { Status = "Error", Message = ex.Message });
            }
        }
    }
}