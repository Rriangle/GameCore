using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GameCore.Core.Interfaces;
using GameCore.Core.Models;
using GameCore.Core.Models.WalletDtos;
using System.Security.Claims;

namespace GameCore.Web.Controllers
{
    /// <summary>
    /// 錢包控制器 - 處理用戶錢包、點數、優惠券等相關功能
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;
        private readonly ILogger<WalletController> _logger;

        public WalletController(IWalletService walletService, ILogger<WalletController> logger)
        {
            _walletService = walletService;
            _logger = logger;
        }

        /// <summary>
        /// 獲取錢包餘額
        /// </summary>
        /// <returns>錢包餘額資訊</returns>
        [HttpGet("balance")]
        public async Task<ActionResult<ServiceResult<WalletBalanceDto>>> GetBalance()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ServiceResult<WalletBalanceDto>.UnauthorizedResult("無效的認證資訊"));
                }

                _logger.LogInformation("獲取錢包餘額請求: {UserId}", userId);
                
                var result = await _walletService.GetBalanceAsync(int.Parse(userId));
                
                if (result.Success)
                {
                    _logger.LogInformation("獲取錢包餘額成功: {UserId}", userId);
                    return Ok(result);
                }
                
                _logger.LogWarning("獲取錢包餘額失敗: {UserId}, 錯誤: {Message}", userId, result.Message);
                return NotFound(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取錢包餘額時發生錯誤");
                return StatusCode(500, ServiceResult<WalletBalanceDto>.FailureResult("獲取錢包餘額過程中發生內部錯誤"));
            }
        }

        /// <summary>
        /// 獲取點數明細
        /// </summary>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <param name="type">點數類型篩選</param>
        /// <returns>點數明細列表</returns>
        [HttpGet("ledger")]
        public async Task<ActionResult<ServiceResult<PointLedgerDto>>> GetLedger(
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 20, 
            [FromQuery] string? type = null)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ServiceResult<PointLedgerDto>.UnauthorizedResult("無效的認證資訊"));
                }

                _logger.LogInformation("獲取點數明細請求: {UserId}, 頁碼: {Page}, 每頁大小: {PageSize}, 類型: {Type}", 
                    userId, page, pageSize, type);
                
                var result = await _walletService.GetLedgerAsync(int.Parse(userId), page, pageSize, type);
                
                if (result.Success)
                {
                    _logger.LogInformation("獲取點數明細成功: {UserId}", userId);
                    return Ok(result);
                }
                
                _logger.LogWarning("獲取點數明細失敗: {UserId}, 錯誤: {Message}", userId, result.Message);
                return NotFound(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取點數明細時發生錯誤");
                return StatusCode(500, ServiceResult<PointLedgerDto>.FailureResult("獲取點數明細過程中發生內部錯誤"));
            }
        }

        /// <summary>
        /// 獲取點數統計
        /// </summary>
        /// <returns>點數統計資訊</returns>
        [HttpGet("statistics")]
        public async Task<ActionResult<ServiceResult<PointStatisticsDto>>> GetStatistics()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ServiceResult<PointStatisticsDto>.UnauthorizedResult("無效的認證資訊"));
                }

                _logger.LogInformation("獲取點數統計請求: {UserId}", userId);
                
                var result = await _walletService.GetStatisticsAsync(int.Parse(userId));
                
                if (result.Success)
                {
                    _logger.LogInformation("獲取點數統計成功: {UserId}", userId);
                    return Ok(result);
                }
                
                _logger.LogWarning("獲取點數統計失敗: {UserId}, 錯誤: {Message}", userId, result.Message);
                return NotFound(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取點數統計時發生錯誤");
                return StatusCode(500, ServiceResult<PointStatisticsDto>.FailureResult("獲取點數統計過程中發生內部錯誤"));
            }
        }

        /// <summary>
        /// 獲取優惠券列表
        /// </summary>
        /// <param name="status">優惠券狀態篩選</param>
        /// <returns>優惠券列表</returns>
        [HttpGet("coupons")]
        public async Task<ActionResult<ServiceResult<List<CouponDto>>>> GetCoupons([FromQuery] string? status = null)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ServiceResult<List<CouponDto>>.UnauthorizedResult("無效的認證資訊"));
                }

                _logger.LogInformation("獲取優惠券列表請求: {UserId}, 狀態: {Status}", userId, status);
                
                var result = await _walletService.GetCouponsAsync(int.Parse(userId), status);
                
                if (result.Success)
                {
                    _logger.LogInformation("獲取優惠券列表成功: {UserId}", userId);
                    return Ok(result);
                }
                
                _logger.LogWarning("獲取優惠券列表失敗: {UserId}, 錯誤: {Message}", userId, result.Message);
                return NotFound(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取優惠券列表時發生錯誤");
                return StatusCode(500, ServiceResult<List<CouponDto>>.FailureResult("獲取優惠券列表過程中發生內部錯誤"));
            }
        }

        /// <summary>
        /// 使用優惠券
        /// </summary>
        /// <param name="couponCode">優惠券代碼</param>
        /// <param name="orderAmount">訂單金額</param>
        /// <returns>使用結果</returns>
        [HttpPost("use-coupon")]
        public async Task<ActionResult<ServiceResult<CouponUsageResultDto>>> UseCoupon(
            [FromBody] string couponCode, 
            [FromQuery] decimal orderAmount)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ServiceResult<CouponUsageResultDto>.UnauthorizedResult("無效的認證資訊"));
                }

                _logger.LogInformation("使用優惠券請求: {UserId}, 優惠券代碼: {CouponCode}, 訂單金額: {OrderAmount}", 
                    userId, couponCode, orderAmount);
                
                var result = await _walletService.UseCouponAsync(int.Parse(userId), couponCode, orderAmount);
                
                if (result.Success)
                {
                    _logger.LogInformation("使用優惠券成功: {UserId}, 優惠券代碼: {CouponCode}", userId, couponCode);
                    return Ok(result);
                }
                
                _logger.LogWarning("使用優惠券失敗: {UserId}, 優惠券代碼: {CouponCode}, 錯誤: {Message}", 
                    userId, couponCode, result.Message);
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "使用優惠券時發生錯誤");
                return StatusCode(500, ServiceResult<CouponUsageResultDto>.FailureResult("使用優惠券過程中發生內部錯誤"));
            }
        }

        /// <summary>
        /// 獲取點數排行榜
        /// </summary>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>點數排行榜</returns>
        [HttpGet("leaderboard")]
        public async Task<ActionResult<ServiceResult<PointLeaderboardDto>>> GetLeaderboard(
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 20)
        {
            try
            {
                _logger.LogInformation("獲取點數排行榜請求: 頁碼: {Page}, 每頁大小: {PageSize}", page, pageSize);
                
                var result = await _walletService.GetLeaderboardAsync(page, pageSize);
                
                if (result.Success)
                {
                    _logger.LogInformation("獲取點數排行榜成功");
                    return Ok(result);
                }
                
                _logger.LogWarning("獲取點數排行榜失敗: {Message}", result.Message);
                return NotFound(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取點數排行榜時發生錯誤");
                return StatusCode(500, ServiceResult<PointLeaderboardDto>.FailureResult("獲取點數排行榜過程中發生內部錯誤"));
            }
        }

        /// <summary>
        /// 獲取點數獲取方式
        /// </summary>
        /// <returns>點數獲取方式列表</returns>
        [HttpGet("earning-methods")]
        public async Task<ActionResult<ServiceResult<List<PointEarningMethodDto>>>> GetEarningMethods()
        {
            try
            {
                _logger.LogInformation("獲取點數獲取方式請求");
                
                var result = await _walletService.GetEarningMethodsAsync();
                
                if (result.Success)
                {
                    _logger.LogInformation("獲取點數獲取方式成功");
                    return Ok(result);
                }
                
                _logger.LogWarning("獲取點數獲取方式失敗: {Message}", result.Message);
                return NotFound(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取點數獲取方式時發生錯誤");
                return StatusCode(500, ServiceResult<List<PointEarningMethodDto>>.FailureResult("獲取點數獲取方式過程中發生內部錯誤"));
            }
        }

        /// <summary>
        /// 獲取點數消費歷史
        /// </summary>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>點數消費歷史</returns>
        [HttpGet("spending-history")]
        public async Task<ActionResult<ServiceResult<PointSpendingHistoryDto>>> GetSpendingHistory(
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 20)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ServiceResult<PointSpendingHistoryDto>.UnauthorizedResult("無效的認證資訊"));
                }

                _logger.LogInformation("獲取點數消費歷史請求: {UserId}, 頁碼: {Page}, 每頁大小: {PageSize}", 
                    userId, page, pageSize);
                
                var result = await _walletService.GetSpendingHistoryAsync(int.Parse(userId), page, pageSize);
                
                if (result.Success)
                {
                    _logger.LogInformation("獲取點數消費歷史成功: {UserId}", userId);
                    return Ok(result);
                }
                
                _logger.LogWarning("獲取點數消費歷史失敗: {UserId}, 錯誤: {Message}", userId, result.Message);
                return NotFound(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取點數消費歷史時發生錯誤");
                return StatusCode(500, ServiceResult<PointSpendingHistoryDto>.FailureResult("獲取點數消費歷史過程中發生內部錯誤"));
            }
        }

        /// <summary>
        /// 獲取點數預測
        /// </summary>
        /// <returns>點數預測資訊</returns>
        [HttpGet("forecast")]
        public async Task<ActionResult<ServiceResult<PointForecastDto>>> GetPointsForecast()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ServiceResult<PointForecastDto>.UnauthorizedResult("無效的認證資訊"));
                }

                _logger.LogInformation("獲取點數預測請求: {UserId}", userId);
                
                var result = await _walletService.GetPointsForecastAsync(int.Parse(userId));
                
                if (result.Success)
                {
                    _logger.LogInformation("獲取點數預測成功: {UserId}", userId);
                    return Ok(result);
                }
                
                _logger.LogWarning("獲取點數預測失敗: {UserId}, 錯誤: {Message}", userId, result.Message);
                return NotFound(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取點數預測時發生錯誤");
                return StatusCode(500, ServiceResult<PointForecastDto>.FailureResult("獲取點數預測過程中發生內部錯誤"));
            }
        }

        /// <summary>
        /// 獲取點數歷史記錄
        /// </summary>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>點數歷史記錄</returns>
        [HttpGet("history")]
        public async Task<ActionResult<ServiceResult<PointHistoryDto>>> GetPointsHistory(
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 20)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ServiceResult<PointHistoryDto>.UnauthorizedResult("無效的認證資訊"));
                }

                _logger.LogInformation("獲取點數歷史記錄請求: {UserId}, 頁碼: {Page}, 每頁大小: {PageSize}", 
                    userId, page, pageSize);
                
                var result = await _walletService.GetPointsHistoryAsync(int.Parse(userId), page, pageSize);
                
                if (result.Success)
                {
                    _logger.LogInformation("獲取點數歷史記錄成功: {UserId}", userId);
                    return Ok(result);
                }
                
                _logger.LogWarning("獲取點數歷史記錄失敗: {UserId}, 錯誤: {Message}", userId, result.Message);
                return NotFound(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取點數歷史記錄時發生錯誤");
                return StatusCode(500, ServiceResult<PointHistoryDto>.FailureResult("獲取點數歷史記錄過程中發生內部錯誤"));
            }
        }
    }
}