using GameCore.Core.DTOs;
using GameCore.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GameCore.Web.Controllers
{
    /// <summary>
    /// 錢包 API 控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;
        private readonly ILogger<WalletController> _logger;

        public WalletController(
            IWalletService walletService,
            ILogger<WalletController> logger)
        {
            _walletService = walletService;
            _logger = logger;
        }

        /// <summary>
        /// 取得錢包餘額
        /// </summary>
        /// <returns>錢包餘額資訊</returns>
        [HttpGet("balance")]
        public async Task<ActionResult<WalletBalanceResponse>> GetBalance()
        {
            try
            {
                var userId = GetCurrentUserId();
                var balance = await _walletService.GetWalletBalanceAsync(userId);
                return Ok(balance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得錢包餘額時發生錯誤");
                return StatusCode(500, new { error = "取得錢包餘額失敗" });
            }
        }

        /// <summary>
        /// 查詢點數流水記錄
        /// </summary>
        /// <param name="request">查詢條件</param>
        /// <returns>點數流水記錄</returns>
        [HttpGet("ledger")]
        public async Task<ActionResult<PointTransactionQueryResponse>> GetLedger([FromQuery] PointTransactionQueryRequest request)
        {
            try
            {
                var userId = GetCurrentUserId();
                var transactions = await _walletService.GetPointTransactionsAsync(userId, request);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "查詢點數流水記錄時發生錯誤");
                return StatusCode(500, new { error = "查詢點數流水記錄失敗" });
            }
        }

        /// <summary>
        /// 取得銷售錢包資訊
        /// </summary>
        /// <returns>銷售錢包資訊</returns>
        [HttpGet("sales")]
        public async Task<ActionResult<SalesWalletInfo>> GetSalesWallet()
        {
            try
            {
                var userId = GetCurrentUserId();
                var salesWallet = await _walletService.GetSalesWalletInfoAsync(userId);
                return Ok(salesWallet);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得銷售錢包資訊時發生錯誤");
                return StatusCode(500, new { error = "取得銷售錢包資訊失敗" });
            }
        }

        /// <summary>
        /// 申請銷售權限
        /// </summary>
        /// <param name="request">申請資訊</param>
        /// <returns>申請結果</returns>
        [HttpPost("sales/permission")]
        public async Task<ActionResult<SalesPermissionResponse>> ApplySalesPermission([FromBody] SalesPermissionRequest request)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await _walletService.ApplySalesPermissionAsync(userId, request);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "申請銷售權限時發生錯誤");
                return StatusCode(500, new { error = "申請銷售權限失敗" });
            }
        }

        /// <summary>
        /// 取得銷售權限申請狀態
        /// </summary>
        /// <returns>申請狀態</returns>
        [HttpGet("sales/permission/status")]
        public async Task<ActionResult<SalesPermissionResponse?>> GetSalesPermissionStatus()
        {
            try
            {
                var userId = GetCurrentUserId();
                var status = await _walletService.GetSalesPermissionStatusAsync(userId);
                return Ok(status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得銷售權限申請狀態時發生錯誤");
                return StatusCode(500, new { error = "取得銷售權限申請狀態失敗" });
            }
        }

        /// <summary>
        /// 轉移點數到銷售錢包
        /// </summary>
        /// <param name="amount">轉移金額</param>
        /// <returns>是否成功</returns>
        [HttpPost("sales/transfer")]
        public async Task<ActionResult> TransferToSalesWallet([FromBody] TransferRequest request)
        {
            try
            {
                var userId = GetCurrentUserId();
                var success = await _walletService.TransferToSalesWalletAsync(userId, request.Amount);
                
                if (success)
                {
                    return Ok(new { message = "點數轉移成功" });
                }
                else
                {
                    return BadRequest(new { error = "點數轉移失敗" });
                }
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "轉移點數到銷售錢包時發生錯誤");
                return StatusCode(500, new { error = "點數轉移失敗" });
            }
        }

        /// <summary>
        /// 從銷售錢包提領點數
        /// </summary>
        /// <param name="request">提領請求</param>
        /// <returns>是否成功</returns>
        [HttpPost("sales/withdraw")]
        public async Task<ActionResult> WithdrawFromSalesWallet([FromBody] WithdrawRequest request)
        {
            try
            {
                var userId = GetCurrentUserId();
                var success = await _walletService.WithdrawFromSalesWalletAsync(userId, request.Amount);
                
                if (success)
                {
                    return Ok(new { message = "點數提領成功" });
                }
                else
                {
                    return BadRequest(new { error = "點數提領失敗" });
                }
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "從銷售錢包提領點數時發生錯誤");
                return StatusCode(500, new { error = "點數提領失敗" });
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

    /// <summary>
    /// 轉移請求
    /// </summary>
    public class TransferRequest
    {
        /// <summary>
        /// 轉移金額
        /// </summary>
        public int Amount { get; set; }
    }

    /// <summary>
    /// 提領請求
    /// </summary>
    public class WithdrawRequest
    {
        /// <summary>
        /// 提領金額
        /// </summary>
        public int Amount { get; set; }
    }
} 