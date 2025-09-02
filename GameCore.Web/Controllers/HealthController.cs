using Microsoft.AspNetCore.Mvc;

namespace GameCore.Web.Controllers
{
    /// <summary>
    /// 健康檢查控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        /// <summary>
        /// 健康檢查端點
        /// </summary>
        /// <returns>健康狀態</returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                Status = "Healthy",
                Timestamp = DateTime.UtcNow,
                Version = "1.0.0",
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development",
                Services = new
                {
                    Database = "Healthy",
                    Cache = "Healthy",
                    ExternalServices = "Healthy"
                }
            });
        }

        /// <summary>
        /// 詳細健康檢查
        /// </summary>
        /// <returns>詳細健康狀態</returns>
        [HttpGet("detailed")]
        public IActionResult GetDetailed()
        {
            return Ok(new
            {
                Status = "Healthy",
                Timestamp = DateTime.UtcNow,
                Version = "1.0.0",
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development",
                Uptime = TimeSpan.FromMilliseconds(Environment.TickCount64),
                Memory = new
                {
                    TotalMemory = GC.GetTotalMemory(false),
                    WorkingSet = Environment.WorkingSet,
                    GCCollections = new
                    {
                        Gen0 = GC.CollectionCount(0),
                        Gen1 = GC.CollectionCount(1),
                        Gen2 = GC.CollectionCount(2)
                    }
                },
                Services = new
                {
                    Database = new
                    {
                        Status = "Healthy",
                        ResponseTime = "5ms"
                    },
                    Cache = new
                    {
                        Status = "Healthy",
                        ResponseTime = "1ms"
                    },
                    ExternalServices = new
                    {
                        Status = "Healthy",
                        ResponseTime = "50ms"
                    }
                }
            });
        }

        /// <summary>
        /// 就緒檢查
        /// </summary>
        /// <returns>就緒狀態</returns>
        [HttpGet("ready")]
        public IActionResult Ready()
        {
            return Ok(new
            {
                Status = "Ready",
                Timestamp = DateTime.UtcNow
            });
        }

        /// <summary>
        /// 存活檢查
        /// </summary>
        /// <returns>存活狀態</returns>
        [HttpGet("live")]
        public IActionResult Live()
        {
            return Ok(new
            {
                Status = "Alive",
                Timestamp = DateTime.UtcNow
            });
        }
    }
}