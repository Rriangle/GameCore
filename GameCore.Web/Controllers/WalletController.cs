using GameCore.Core.DTOs;
using GameCore.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GameCore.Web.Controllers
{
    /// <summary>
    /// 錢包控制器 - 處理會員點數、交易流水和銷售錢包相關功能
    /// 提供完整的錢包管理 API，包含點數查詢、收支明細、銷售功能申請等
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // 所有錢包相關操作都需要登入
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;
        private readonly ILogger<WalletController> _logger;

        public WalletController(IWalletService walletService, ILogger<WalletController> logger)
        {
            _walletService = walletService;
            _logger = logger;
        }

        #region 基本錢包功能 (Basic Wallet Functions)

        /// <summary>
        /// 取得當前使用者的錢包資訊
        /// GET /api/wallet
        /// </summary>
        /// <returns>錢包資訊，包含點數餘額、優惠券、銷售錢包等</returns>
        [HttpGet]
        public async Task<IActionResult> GetWallet()
        {
            try
            {
                var userId = GetCurrentUserId();
                _logger.LogInformation($"取得使用者 {userId} 的錢包資訊");

                var wallet = await _walletService.GetWalletAsync(userId);
                
                return Ok(new
                {
                    success = true,
                    data = wallet,
                    message = "錢包資訊取得成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得錢包資訊時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得錢包資訊時發生錯誤" });
            }
        }

        /// <summary>
        /// 取得當前使用者的點數餘額 (簡化版API)
        /// GET /api/wallet/balance
        /// </summary>
        /// <returns>僅包含點數餘額的回應</returns>
        [HttpGet("balance")]
        public async Task<IActionResult> GetBalance()
        {
            try
            {
                var userId = GetCurrentUserId();
                var wallet = await _walletService.GetWalletAsync(userId);
                
                return Ok(new
                {
                    balance = wallet.CurrentPoints,
                    success = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得點數餘額時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得點數餘額時發生錯誤" });
            }
        }

        /// <summary>
        /// 檢查是否有足夠點數進行指定消費
        /// GET /api/wallet/check-sufficient?points=100
        /// </summary>
        /// <param name="points">需要檢查的點數</param>
        /// <returns>是否有足夠點數</returns>
        [HttpGet("check-sufficient")]
        public async Task<IActionResult> CheckSufficientPoints([FromQuery] int points)
        {
            try
            {
                if (points <= 0)
                {
                    return BadRequest(new { success = false, message = "檢查點數必須大於 0" });
                }

                var userId = GetCurrentUserId();
                var hasSufficient = await _walletService.HasSufficientPointsAsync(userId, points);
                
                return Ok(new
                {
                    sufficient = hasSufficient,
                    requiredPoints = points,
                    success = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "檢查點數餘額時發生錯誤");
                return StatusCode(500, new { success = false, message = "檢查點數餘額時發生錯誤" });
            }
        }

        #endregion

        #region 收支明細查詢 (Transaction History)

        /// <summary>
        /// 取得使用者點數收支明細
        /// GET /api/wallet/ledger?from=2024-01-01&to=2024-01-31&type=signin&page=1&pageSize=20&sortBy=date_desc
        /// </summary>
        /// <param name="from">查詢開始時間 (可選)</param>
        /// <param name="to">查詢結束時間 (可選)</param>
        /// <param name="type">交易類型篩選 (可選): signin, minigame, pet_color, adjustment</param>
        /// <param name="page">頁碼 (預設 1)</param>
        /// <param name="pageSize">每頁筆數 (預設 20)</param>
        /// <param name="sortBy">排序方式 (預設 date_desc)</param>
        /// <returns>分頁的收支明細清單</returns>
        [HttpGet("ledger")]
        public async Task<IActionResult> GetLedgerHistory(
            [FromQuery] DateTime? from = null,
            [FromQuery] DateTime? to = null,
            [FromQuery] string? type = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string sortBy = "date_desc")
        {
            try
            {
                var userId = GetCurrentUserId();
                _logger.LogInformation($"查詢使用者 {userId} 的收支明細，類型: {type}，頁碼: {page}");

                var request = new LedgerQueryDto
                {
                    FromDate = from,
                    ToDate = to,
                    TransactionType = type,
                    Page = page,
                    PageSize = Math.Min(pageSize, 100), // 限制最大每頁筆數
                    SortBy = sortBy
                };

                var result = await _walletService.GetLedgerHistoryAsync(userId, request);
                
                return Ok(new
                {
                    success = true,
                    data = result.Items,
                    pagination = new
                    {
                        currentPage = result.CurrentPage,
                        pageSize = result.PageSize,
                        totalCount = result.TotalCount,
                        totalPages = result.TotalPages,
                        hasNextPage = result.HasNextPage,
                        hasPreviousPage = result.HasPreviousPage
                    },
                    filters = new
                    {
                        fromDate = from,
                        toDate = to,
                        transactionType = type,
                        sortBy = sortBy
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "查詢收支明細時發生錯誤");
                return StatusCode(500, new { success = false, message = "查詢收支明細時發生錯誤" });
            }
        }

        /// <summary>
        /// 取得使用者點數統計摘要
        /// GET /api/wallet/statistics
        /// </summary>
        /// <returns>包含今日、本週、本月的收入支出統計</returns>
        [HttpGet("statistics")]
        public async Task<IActionResult> GetPointsStatistics()
        {
            try
            {
                var userId = GetCurrentUserId();
                _logger.LogInformation($"取得使用者 {userId} 的點數統計");

                var statistics = await _walletService.GetPointsStatisticsAsync(userId);
                
                return Ok(new
                {
                    success = true,
                    data = statistics,
                    message = "點數統計取得成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得點數統計時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得點數統計時發生錯誤" });
            }
        }

        #endregion

        #region 銷售功能管理 (Sales Management)

        /// <summary>
        /// 申請開通銷售功能
        /// POST /api/wallet/sales/apply
        /// </summary>
        /// <param name="salesProfileDto">銷售檔案申請資料</param>
        /// <returns>申請結果</returns>
        [HttpPost("sales/apply")]
        public async Task<IActionResult> ApplySalesProfile([FromBody] CreateSalesProfileDto salesProfileDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { success = false, message = "申請資料格式錯誤", errors = ModelState });
                }

                var userId = GetCurrentUserId();
                _logger.LogInformation($"使用者 {userId} 申請開通銷售功能");

                var result = await _walletService.ApplySalesProfileAsync(userId, salesProfileDto);
                
                if (result.Success)
                {
                    return Ok(new
                    {
                        success = true,
                        data = result.Data,
                        message = result.Message
                    });
                }

                return BadRequest(new
                {
                    success = false,
                    message = result.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "申請銷售功能時發生錯誤");
                return StatusCode(500, new { success = false, message = "申請銷售功能時發生錯誤" });
            }
        }

        /// <summary>
        /// 取得當前使用者的銷售檔案資訊
        /// GET /api/wallet/sales/profile
        /// </summary>
        /// <returns>銷售檔案資訊</returns>
        [HttpGet("sales/profile")]
        public async Task<IActionResult> GetSalesProfile()
        {
            try
            {
                var userId = GetCurrentUserId();
                var profile = await _walletService.GetSalesProfileAsync(userId);
                
                if (profile == null)
                {
                    return NotFound(new { success = false, message = "尚未申請銷售功能" });
                }

                return Ok(new
                {
                    success = true,
                    data = profile,
                    message = "銷售檔案取得成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得銷售檔案時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得銷售檔案時發生錯誤" });
            }
        }

        /// <summary>
        /// 取得當前使用者的銷售錢包資訊
        /// GET /api/wallet/sales/wallet
        /// </summary>
        /// <returns>銷售錢包資訊</returns>
        [HttpGet("sales/wallet")]
        public async Task<IActionResult> GetSalesWallet()
        {
            try
            {
                var userId = GetCurrentUserId();
                var salesWallet = await _walletService.GetSalesWalletAsync(userId);
                
                if (salesWallet == null)
                {
                    return NotFound(new { success = false, message = "銷售錢包不存在" });
                }

                return Ok(new
                {
                    success = true,
                    data = salesWallet,
                    message = "銷售錢包資訊取得成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得銷售錢包時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得銷售錢包時發生錯誤" });
            }
        }

        #endregion

        #region 管理員功能 (Admin Functions)

        /// <summary>
        /// 管理員調整使用者點數
        /// POST /api/wallet/admin/adjust
        /// </summary>
        /// <param name="request">點數調整請求</param>
        /// <returns>調整結果</returns>
        [HttpPost("admin/adjust")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdjustUserPoints([FromBody] AdjustPointsRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { success = false, message = "調整資料格式錯誤", errors = ModelState });
                }

                var adminId = GetCurrentUserId();
                _logger.LogInformation($"管理員 {adminId} 調整使用者 {request.UserId} 點數: {request.PointsDelta}");

                var result = await _walletService.AdjustPointsAsync(request.UserId, request.PointsDelta, request.Reason, adminId);
                
                return Ok(new
                {
                    success = true,
                    data = result,
                    message = $"成功調整使用者點數 {request.PointsDelta} 點"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "管理員調整點數時發生錯誤");
                return StatusCode(500, new { success = false, message = "調整點數時發生錯誤" });
            }
        }

        #endregion

        #region 交易處理 (Transaction Processing)

        /// <summary>
        /// 執行點數消費 (內部API，供其他服務呼叫)
        /// POST /api/wallet/spend
        /// </summary>
        /// <param name="request">消費請求</param>
        /// <returns>消費結果</returns>
        [HttpPost("spend")]
        public async Task<IActionResult> SpendPoints([FromBody] SpendPointsRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { success = false, message = "消費資料格式錯誤", errors = ModelState });
                }

                var userId = GetCurrentUserId();
                _logger.LogInformation($"使用者 {userId} 消費 {request.Points} 點數: {request.Purpose}");

                var result = await _walletService.SpendPointsAsync(userId, request.Points, request.Purpose, request.ReferenceId);
                
                if (result.Success)
                {
                    return Ok(new
                    {
                        success = true,
                        data = result.Data,
                        message = result.Message
                    });
                }

                return BadRequest(new
                {
                    success = false,
                    message = result.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "消費點數時發生錯誤");
                return StatusCode(500, new { success = false, message = "消費點數時發生錯誤" });
            }
        }

        /// <summary>
        /// 執行點數獲得 (內部API，供其他服務呼叫)
        /// POST /api/wallet/earn
        /// </summary>
        /// <param name="request">獲得請求</param>
        /// <returns>獲得結果</returns>
        [HttpPost("earn")]
        public async Task<IActionResult> EarnPoints([FromBody] EarnPointsRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { success = false, message = "獲得資料格式錯誤", errors = ModelState });
                }

                var userId = GetCurrentUserId();
                _logger.LogInformation($"使用者 {userId} 獲得 {request.Points} 點數: {request.Source}");

                var result = await _walletService.EarnPointsAsync(userId, request.Points, request.Source, request.ReferenceId);
                
                if (result.Success)
                {
                    return Ok(new
                    {
                        success = true,
                        data = result.Data,
                        message = result.Message
                    });
                }

                return BadRequest(new
                {
                    success = false,
                    message = result.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲得點數時發生錯誤");
                return StatusCode(500, new { success = false, message = "獲得點數時發生錯誤" });
            }
        }

        #endregion

        #region 輔助方法 (Helper Methods)

        /// <summary>
        /// 取得當前登入使用者的 ID
        /// </summary>
        /// <returns>使用者 ID</returns>
        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new UnauthorizedAccessException("無法取得使用者身份資訊");
            }
            return userId;
        }

        #endregion
    }

    #region 請求 DTOs (Request DTOs)

    /// <summary>
    /// 管理員調整點數請求 DTO
    /// </summary>
    public class AdjustPointsRequestDto
    {
        /// <summary>目標使用者 ID</summary>
        [Required(ErrorMessage = "使用者 ID 必填")]
        public int UserId { get; set; }

        /// <summary>點數變化量 (正數=增加, 負數=扣除)</summary>
        [Required(ErrorMessage = "點數變化量必填")]
        public int PointsDelta { get; set; }

        /// <summary>調整原因</summary>
        [Required(ErrorMessage = "調整原因必填")]
        [StringLength(200, ErrorMessage = "調整原因不可超過 200 字")]
        public string Reason { get; set; } = string.Empty;
    }

    /// <summary>
    /// 點數消費請求 DTO
    /// </summary>
    public class SpendPointsRequestDto
    {
        /// <summary>消費點數</summary>
        [Required(ErrorMessage = "消費點數必填")]
        [Range(1, int.MaxValue, ErrorMessage = "消費點數必須大於 0")]
        public int Points { get; set; }

        /// <summary>消費目的</summary>
        [Required(ErrorMessage = "消費目的必填")]
        [StringLength(100, ErrorMessage = "消費目的不可超過 100 字")]
        public string Purpose { get; set; } = string.Empty;

        /// <summary>關聯 ID (可選)</summary>
        public string? ReferenceId { get; set; }
    }

    /// <summary>
    /// 點數獲得請求 DTO
    /// </summary>
    public class EarnPointsRequestDto
    {
        /// <summary>獲得點數</summary>
        [Required(ErrorMessage = "獲得點數必填")]
        [Range(1, int.MaxValue, ErrorMessage = "獲得點數必須大於 0")]
        public int Points { get; set; }

        /// <summary>來源說明</summary>
        [Required(ErrorMessage = "來源說明必填")]
        [StringLength(100, ErrorMessage = "來源說明不可超過 100 字")]
        public string Source { get; set; } = string.Empty;

        /// <summary>關聯 ID (可選)</summary>
        public string? ReferenceId { get; set; }
    }

    #endregion
}