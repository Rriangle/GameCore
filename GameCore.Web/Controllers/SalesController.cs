using GameCore.Core.DTOs;
using GameCore.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GameCore.Web.Controllers
{
    /// <summary>
    /// 銷售管理 API 控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SalesController : ControllerBase
    {
        private readonly ISalesService _salesService;
        private readonly ILogger<SalesController> _logger;

        public SalesController(
            ISalesService salesService,
            ILogger<SalesController> logger)
        {
            _salesService = salesService;
            _logger = logger;
        }

        /// <summary>
        /// 取得用戶銷售統計
        /// </summary>
        /// <returns>銷售統計資訊</returns>
        [HttpGet("statistics")]
        public async Task<ActionResult<SalesStatisticsResponse>> GetStatistics()
        {
            try
            {
                var userId = GetCurrentUserId();
                var statistics = await _salesService.GetSalesStatisticsAsync(userId);
                return Ok(statistics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得銷售統計時發生錯誤");
                return StatusCode(500, new { error = "取得銷售統計失敗" });
            }
        }

        /// <summary>
        /// 取得銷售排行榜
        /// </summary>
        /// <param name="period">期間類型（daily/weekly/monthly）</param>
        /// <param name="limit">顯示數量</param>
        /// <returns>銷售排行榜</returns>
        [HttpGet("ranking")]
        public async Task<ActionResult<List<SalesRankingItem>>> GetRanking([FromQuery] string period = "monthly", [FromQuery] int limit = 10)
        {
            try
            {
                var ranking = await _salesService.GetSalesRankingAsync(period, limit);
                return Ok(ranking);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得銷售排行榜時發生錯誤，期間：{Period}", period);
                return StatusCode(500, new { error = "取得銷售排行榜失敗" });
            }
        }

        /// <summary>
        /// 取得銷售報表
        /// </summary>
        /// <param name="startDate">開始日期</param>
        /// <param name="endDate">結束日期</param>
        /// <returns>銷售報表</returns>
        [HttpGet("report")]
        public async Task<ActionResult<SalesReportResponse>> GetReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var userId = GetCurrentUserId();
                var report = await _salesService.GetSalesReportAsync(userId, startDate, endDate);
                return Ok(report);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得銷售報表時發生錯誤");
                return StatusCode(500, new { error = "取得銷售報表失敗" });
            }
        }

        /// <summary>
        /// 取得當前用戶ID
        /// </summary>
        /// <returns>用戶ID</returns>
        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out var userId))
            {
                return userId;
            }
            
            throw new InvalidOperationException("無法取得當前用戶ID");
        }
    }
} 