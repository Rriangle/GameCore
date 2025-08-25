using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GameCore.Core.Interfaces;
using GameCore.Core.Models;
using GameCore.Core.Models.SalesDtos;
using System.Security.Claims;

namespace GameCore.Web.Controllers
{
    /// <summary>
    /// 銷售控制器 - 處理用戶銷售權限、商品、訂單等相關功能
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SalesController : ControllerBase
    {
        private readonly ISalesService _salesService;
        private readonly ILogger<SalesController> _logger;

        public SalesController(ISalesService salesService, ILogger<SalesController> logger)
        {
            _salesService = salesService;
            _logger = logger;
        }

        /// <summary>
        /// 申請銷售權限
        /// </summary>
        /// <param name="request">銷售權限申請請求</param>
        /// <returns>申請結果</returns>
        [HttpPost("permission/apply")]
        public async Task<ActionResult<ServiceResult<SalesPermissionDto>>> ApplySalesPermission([FromBody] SalesPermissionRequestDto request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ServiceResult<SalesPermissionDto>.UnauthorizedResult("無效的認證資訊"));
                }

                _logger.LogInformation("申請銷售權限請求: {UserId}", userId);
                
                var result = await _salesService.ApplySalesPermissionAsync(int.Parse(userId), request);
                
                if (result.Success)
                {
                    _logger.LogInformation("申請銷售權限成功: {UserId}", userId);
                    return Ok(result);
                }
                
                _logger.LogWarning("申請銷售權限失敗: {UserId}, 錯誤: {Message}", userId, result.Message);
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "申請銷售權限時發生錯誤");
                return StatusCode(500, ServiceResult<SalesPermissionDto>.FailureResult("申請銷售權限過程中發生內部錯誤"));
            }
        }

        /// <summary>
        /// 獲取銷售權限申請狀態
        /// </summary>
        /// <returns>申請狀態</returns>
        [HttpGet("permission/status")]
        public async Task<ActionResult<ServiceResult<SalesPermissionDto>>> GetApplicationStatus()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ServiceResult<SalesPermissionDto>.UnauthorizedResult("無效的認證資訊"));
                }

                _logger.LogInformation("獲取銷售權限申請狀態請求: {UserId}", userId);
                
                var result = await _salesService.GetApplicationStatusAsync(int.Parse(userId));
                
                if (result.Success)
                {
                    _logger.LogInformation("獲取銷售權限申請狀態成功: {UserId}", userId);
                    return Ok(result);
                }
                
                _logger.LogWarning("獲取銷售權限申請狀態失敗: {UserId}, 錯誤: {Message}", userId, result.Message);
                return NotFound(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取銷售權限申請狀態時發生錯誤");
                return StatusCode(500, ServiceResult<SalesPermissionDto>.FailureResult("獲取銷售權限申請狀態過程中發生內部錯誤"));
            }
        }

        /// <summary>
        /// 獲取銷售錢包
        /// </summary>
        /// <returns>銷售錢包資訊</returns>
        [HttpGet("wallet")]
        public async Task<ActionResult<ServiceResult<SalesWalletDto>>> GetSalesWallet()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ServiceResult<SalesWalletDto>.UnauthorizedResult("無效的認證資訊"));
                }

                _logger.LogInformation("獲取銷售錢包請求: {UserId}", userId);
                
                var result = await _salesService.GetSalesWalletAsync(int.Parse(userId));
                
                if (result.Success)
                {
                    _logger.LogInformation("獲取銷售錢包成功: {UserId}", userId);
                    return Ok(result);
                }
                
                _logger.LogWarning("獲取銷售錢包失敗: {UserId}, 錯誤: {Message}", userId, result.Message);
                return NotFound(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取銷售錢包時發生錯誤");
                return StatusCode(500, ServiceResult<SalesWalletDto>.FailureResult("獲取銷售錢包過程中發生內部錯誤"));
            }
        }

        /// <summary>
        /// 申請提現
        /// </summary>
        /// <param name="request">提現申請請求</param>
        /// <returns>申請結果</returns>
        [HttpPost("withdrawal/request")]
        public async Task<ActionResult<ServiceResult<WithdrawalRequestDto>>> RequestWithdrawal([FromBody] WithdrawalRequestRequestDto request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ServiceResult<WithdrawalRequestDto>.UnauthorizedResult("無效的認證資訊"));
                }

                _logger.LogInformation("申請提現請求: {UserId}, 金額: {Amount}", userId, request.Amount);
                
                var result = await _salesService.RequestWithdrawalAsync(int.Parse(userId), request);
                
                if (result.Success)
                {
                    _logger.LogInformation("申請提現成功: {UserId}, 金額: {Amount}", userId, request.Amount);
                    return Ok(result);
                }
                
                _logger.LogWarning("申請提現失敗: {UserId}, 金額: {Amount}, 錯誤: {Message}", 
                    userId, request.Amount, result.Message);
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "申請提現時發生錯誤");
                return StatusCode(500, ServiceResult<WithdrawalRequestDto>.FailureResult("申請提現過程中發生內部錯誤"));
            }
        }

        /// <summary>
        /// 獲取提現歷史
        /// </summary>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>提現歷史列表</returns>
        [HttpGet("withdrawal/history")]
        public async Task<ActionResult<ServiceResult<WithdrawalHistoryDto>>> GetWithdrawalHistory(
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 20)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ServiceResult<WithdrawalHistoryDto>.UnauthorizedResult("無效的認證資訊"));
                }

                _logger.LogInformation("獲取提現歷史請求: {UserId}, 頁碼: {Page}, 每頁大小: {PageSize}", 
                    userId, page, pageSize);
                
                var result = await _salesService.GetWithdrawalHistoryAsync(int.Parse(userId), page, pageSize);
                
                if (result.Success)
                {
                    _logger.LogInformation("獲取提現歷史成功: {UserId}", userId);
                    return Ok(result);
                }
                
                _logger.LogWarning("獲取提現歷史失敗: {UserId}, 錯誤: {Message}", userId, result.Message);
                return NotFound(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取提現歷史時發生錯誤");
                return StatusCode(500, ServiceResult<WithdrawalHistoryDto>.FailureResult("獲取提現歷史過程中發生內部錯誤"));
            }
        }

        /// <summary>
        /// 獲取銷售統計
        /// </summary>
        /// <returns>銷售統計資訊</returns>
        [HttpGet("statistics")]
        public async Task<ActionResult<ServiceResult<SalesStatisticsDto>>> GetSalesStatistics()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ServiceResult<SalesStatisticsDto>.UnauthorizedResult("無效的認證資訊"));
                }

                _logger.LogInformation("獲取銷售統計請求: {UserId}", userId);
                
                var result = await _salesService.GetSalesStatisticsAsync(int.Parse(userId));
                
                if (result.Success)
                {
                    _logger.LogInformation("獲取銷售統計成功: {UserId}", userId);
                    return Ok(result);
                }
                
                _logger.LogWarning("獲取銷售統計失敗: {UserId}, 錯誤: {Message}", userId, result.Message);
                return NotFound(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取銷售統計時發生錯誤");
                return StatusCode(500, ServiceResult<SalesStatisticsDto>.FailureResult("獲取銷售統計過程中發生內部錯誤"));
            }
        }

        /// <summary>
        /// 獲取銷售排行榜
        /// </summary>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>銷售排行榜</returns>
        [HttpGet("leaderboard")]
        public async Task<ActionResult<ServiceResult<SalesLeaderboardDto>>> GetSalesLeaderboard(
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 20)
        {
            try
            {
                _logger.LogInformation("獲取銷售排行榜請求: 頁碼: {Page}, 每頁大小: {PageSize}", page, pageSize);
                
                var result = await _salesService.GetSalesLeaderboardAsync(page, pageSize);
                
                if (result.Success)
                {
                    _logger.LogInformation("獲取銷售排行榜成功");
                    return Ok(result);
                }
                
                _logger.LogWarning("獲取銷售排行榜失敗: {Message}", result.Message);
                return NotFound(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取銷售排行榜時發生錯誤");
                return StatusCode(500, ServiceResult<SalesLeaderboardDto>.FailureResult("獲取銷售排行榜過程中發生內部錯誤"));
            }
        }

        /// <summary>
        /// 獲取銷售指南
        /// </summary>
        /// <returns>銷售指南資訊</returns>
        [HttpGet("guide")]
        public async Task<ActionResult<ServiceResult<SalesGuideDto>>> GetSalesGuide()
        {
            try
            {
                _logger.LogInformation("獲取銷售指南請求");
                
                var result = await _salesService.GetSalesGuideAsync();
                
                if (result.Success)
                {
                    _logger.LogInformation("獲取銷售指南成功");
                    return Ok(result);
                }
                
                _logger.LogWarning("獲取銷售指南失敗: {Message}", result.Message);
                return NotFound(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取銷售指南時發生錯誤");
                return StatusCode(500, ServiceResult<SalesGuideDto>.FailureResult("獲取銷售指南過程中發生內部錯誤"));
            }
        }

        /// <summary>
        /// 更新銀行帳戶
        /// </summary>
        /// <param name="request">銀行帳戶更新請求</param>
        /// <returns>更新結果</returns>
        [HttpPut("bank-account")]
        public async Task<ActionResult<ServiceResult<BankAccountDto>>> UpdateBankAccount([FromBody] BankAccountUpdateDto request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ServiceResult<BankAccountDto>.UnauthorizedResult("無效的認證資訊"));
                }

                _logger.LogInformation("更新銀行帳戶請求: {UserId}", userId);
                
                var result = await _salesService.UpdateBankAccountAsync(int.Parse(userId), request);
                
                if (result.Success)
                {
                    _logger.LogInformation("更新銀行帳戶成功: {UserId}", userId);
                    return Ok(result);
                }
                
                _logger.LogWarning("更新銀行帳戶失敗: {UserId}, 錯誤: {Message}", userId, result.Message);
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新銀行帳戶時發生錯誤");
                return StatusCode(500, ServiceResult<BankAccountDto>.FailureResult("更新銀行帳戶過程中發生內部錯誤"));
            }
        }

        /// <summary>
        /// 獲取銀行帳戶
        /// </summary>
        /// <returns>銀行帳戶資訊</returns>
        [HttpGet("bank-account")]
        public async Task<ActionResult<ServiceResult<BankAccountDto>>> GetBankAccount()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ServiceResult<BankAccountDto>.UnauthorizedResult("無效的認證資訊"));
                }

                _logger.LogInformation("獲取銀行帳戶請求: {UserId}", userId);
                
                var result = await _salesService.GetBankAccountAsync(int.Parse(userId));
                
                if (result.Success)
                {
                    _logger.LogInformation("獲取銀行帳戶成功: {UserId}", userId);
                    return Ok(result);
                }
                
                _logger.LogWarning("獲取銀行帳戶失敗: {UserId}, 錯誤: {Message}", userId, result.Message);
                return NotFound(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取銀行帳戶時發生錯誤");
                return StatusCode(500, ServiceResult<BankAccountDto>.FailureResult("獲取銀行帳戶過程中發生內部錯誤"));
            }
        }

        /// <summary>
        /// 檢查銷售權限
        /// </summary>
        /// <returns>權限檢查結果</returns>
        [HttpGet("permission/check")]
        public async Task<ActionResult<ServiceResult<bool>>> CheckSalesPermission()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ServiceResult<bool>.UnauthorizedResult("無效的認證資訊"));
                }

                _logger.LogInformation("檢查銷售權限請求: {UserId}", userId);
                
                var result = await _salesService.CheckSalesPermissionAsync(int.Parse(userId));
                
                if (result.Success)
                {
                    _logger.LogInformation("檢查銷售權限成功: {UserId}", userId);
                    return Ok(result);
                }
                
                _logger.LogWarning("檢查銷售權限失敗: {UserId}, 錯誤: {Message}", userId, result.Message);
                return NotFound(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "檢查銷售權限時發生錯誤");
                return StatusCode(500, ServiceResult<bool>.FailureResult("檢查銷售權限過程中發生內部錯誤"));
            }
        }

        /// <summary>
        /// 獲取銷售權限詳情
        /// </summary>
        /// <returns>銷售權限詳情</returns>
        [HttpGet("permission/details")]
        public async Task<ActionResult<ServiceResult<SalesPermissionDetailsDto>>> GetSalesPermissionDetails()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ServiceResult<SalesPermissionDetailsDto>.UnauthorizedResult("無效的認證資訊"));
                }

                _logger.LogInformation("獲取銷售權限詳情請求: {UserId}", userId);
                
                var result = await _salesService.GetSalesPermissionDetailsAsync(int.Parse(userId));
                
                if (result.Success)
                {
                    _logger.LogInformation("獲取銷售權限詳情成功: {UserId}", userId);
                    return Ok(result);
                }
                
                _logger.LogWarning("獲取銷售權限詳情失敗: {UserId}, 錯誤: {Message}", userId, result.Message);
                return NotFound(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取銷售權限詳情時發生錯誤");
                return StatusCode(500, ServiceResult<SalesPermissionDetailsDto>.FailureResult("獲取銷售權限詳情過程中發生內部錯誤"));
            }
        }

        /// <summary>
        /// 獲取銷售商品列表
        /// </summary>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>銷售商品列表</returns>
        [HttpGet("products")]
        public async Task<ActionResult<ServiceResult<List<SalesProductDto>>>> GetSalesProducts(
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 20)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ServiceResult<List<SalesProductDto>>.UnauthorizedResult("無效的認證資訊"));
                }

                _logger.LogInformation("獲取銷售商品列表請求: {UserId}, 頁碼: {Page}, 每頁大小: {PageSize}", 
                    userId, page, pageSize);
                
                var result = await _salesService.GetSalesProductsAsync(int.Parse(userId), page, pageSize);
                
                if (result.Success)
                {
                    _logger.LogInformation("獲取銷售商品列表成功: {UserId}", userId);
                    return Ok(result);
                }
                
                _logger.LogWarning("獲取銷售商品列表失敗: {UserId}, 錯誤: {Message}", userId, result.Message);
                return NotFound(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取銷售商品列表時發生錯誤");
                return StatusCode(500, ServiceResult<List<SalesProductDto>>.FailureResult("獲取銷售商品列表過程中發生內部錯誤"));
            }
        }

        /// <summary>
        /// 獲取銷售訂單列表
        /// </summary>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>銷售訂單列表</returns>
        [HttpGet("orders")]
        public async Task<ActionResult<ServiceResult<List<SalesOrderDto>>>> GetSalesOrders(
            [FromQuery] int page = 1, 
            [FromQuery] int pageSize = 20)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ServiceResult<List<SalesOrderDto>>.UnauthorizedResult("無效的認證資訊"));
                }

                _logger.LogInformation("獲取銷售訂單列表請求: {UserId}, 頁碼: {Page}, 每頁大小: {PageSize}", 
                    userId, page, pageSize);
                
                var result = await _salesService.GetSalesOrdersAsync(int.Parse(userId), page, pageSize);
                
                if (result.Success)
                {
                    _logger.LogInformation("獲取銷售訂單列表成功: {UserId}", userId);
                    return Ok(result);
                }
                
                _logger.LogWarning("獲取銷售訂單列表失敗: {UserId}, 錯誤: {Message}", userId, result.Message);
                return NotFound(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取銷售訂單列表時發生錯誤");
                return StatusCode(500, ServiceResult<List<SalesOrderDto>>.FailureResult("獲取銷售訂單列表過程中發生內部錯誤"));
            }
        }

        /// <summary>
        /// 獲取銷售報告
        /// </summary>
        /// <returns>銷售報告</returns>
        [HttpGet("report")]
        public async Task<ActionResult<ServiceResult<SalesReportDto>>> GetSalesReport()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ServiceResult<SalesReportDto>.UnauthorizedResult("無效的認證資訊"));
                }

                _logger.LogInformation("獲取銷售報告請求: {UserId}", userId);
                
                var result = await _salesService.GetSalesReportAsync(int.Parse(userId));
                
                if (result.Success)
                {
                    _logger.LogInformation("獲取銷售報告成功: {UserId}", userId);
                    return Ok(result);
                }
                
                _logger.LogWarning("獲取銷售報告失敗: {UserId}, 錯誤: {Message}", userId, result.Message);
                return NotFound(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取銷售報告時發生錯誤");
                return StatusCode(500, ServiceResult<SalesReportDto>.FailureResult("獲取銷售報告過程中發生內部錯誤"));
            }
        }

        /// <summary>
        /// 獲取銷售分析
        /// </summary>
        /// <returns>銷售分析資訊</returns>
        [HttpGet("analytics")]
        public async Task<ActionResult<ServiceResult<SalesAnalyticsDto>>> GetSalesAnalytics()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ServiceResult<SalesAnalyticsDto>.UnauthorizedResult("無效的認證資訊"));
                }

                _logger.LogInformation("獲取銷售分析請求: {UserId}", userId);
                
                var result = await _salesService.GetSalesAnalyticsAsync(int.Parse(userId));
                
                if (result.Success)
                {
                    _logger.LogInformation("獲取銷售分析成功: {UserId}", userId);
                    return Ok(result);
                }
                
                _logger.LogWarning("獲取銷售分析失敗: {UserId}, 錯誤: {Message}", userId, result.Message);
                return NotFound(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取銷售分析時發生錯誤");
                return StatusCode(500, ServiceResult<SalesAnalyticsDto>.FailureResult("獲取銷售分析過程中發生內部錯誤"));
            }
        }

        /// <summary>
        /// 獲取銷售目標
        /// </summary>
        /// <returns>銷售目標資訊</returns>
        [HttpGet("target")]
        public async Task<ActionResult<ServiceResult<SalesTargetDto>>> GetSalesTarget()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ServiceResult<SalesTargetDto>.UnauthorizedResult("無效的認證資訊"));
                }

                _logger.LogInformation("獲取銷售目標請求: {UserId}", userId);
                
                var result = await _salesService.GetSalesTargetAsync(int.Parse(userId));
                
                if (result.Success)
                {
                    _logger.LogInformation("獲取銷售目標成功: {UserId}", userId);
                    return Ok(result);
                }
                
                _logger.LogWarning("獲取銷售目標失敗: {UserId}, 錯誤: {Message}", userId, result.Message);
                return NotFound(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取銷售目標時發生錯誤");
                return StatusCode(500, ServiceResult<SalesTargetDto>.FailureResult("獲取銷售目標過程中發生內部錯誤"));
            }
        }

        /// <summary>
        /// 設定銷售目標
        /// </summary>
        /// <param name="request">銷售目標設定請求</param>
        /// <returns>設定結果</returns>
        [HttpPost("target")]
        public async Task<ActionResult<ServiceResult<SalesTargetDto>>> SetSalesTarget([FromBody] SalesTargetSetDto request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ServiceResult<SalesTargetDto>.UnauthorizedResult("無效的認證資訊"));
                }

                _logger.LogInformation("設定銷售目標請求: {UserId}", userId);
                
                var result = await _salesService.SetSalesTargetAsync(int.Parse(userId), request);
                
                if (result.Success)
                {
                    _logger.LogInformation("設定銷售目標成功: {UserId}", userId);
                    return Ok(result);
                }
                
                _logger.LogWarning("設定銷售目標失敗: {UserId}, 錯誤: {Message}", userId, result.Message);
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "設定銷售目標時發生錯誤");
                return StatusCode(500, ServiceResult<SalesTargetDto>.FailureResult("設定銷售目標過程中發生內部錯誤"));
            }
        }

        /// <summary>
        /// 獲取銷售績效
        /// </summary>
        /// <returns>銷售績效資訊</returns>
        [HttpGet("performance")]
        public async Task<ActionResult<ServiceResult<SalesPerformanceDto>>> GetSalesPerformance()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(ServiceResult<SalesPerformanceDto>.UnauthorizedResult("無效的認證資訊"));
                }

                _logger.LogInformation("獲取銷售績效請求: {UserId}", userId);
                
                var result = await _salesService.GetSalesPerformanceAsync(int.Parse(userId));
                
                if (result.Success)
                {
                    _logger.LogInformation("獲取銷售績效成功: {UserId}", userId);
                    return Ok(result);
                }
                
                _logger.LogWarning("獲取銷售績效失敗: {UserId}, 錯誤: {Message}", userId, result.Message);
                return NotFound(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取銷售績效時發生錯誤");
                return StatusCode(500, ServiceResult<SalesPerformanceDto>.FailureResult("獲取銷售績效過程中發生內部錯誤"));
            }
        }
    }
}