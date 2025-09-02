using GameCore.Core.DTOs;
using GameCore.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GameCore.Web.Controllers
{
    /// <summary>
    /// 錢包 MVC 控制器
    /// </summary>
    [Authorize]
    public class WalletMvcController : Controller
    {
        private readonly IWalletService _walletService;
        private readonly ISalesService _salesService;
        private readonly ILogger<WalletMvcController> _logger;

        public WalletMvcController(
            IWalletService walletService,
            ISalesService salesService,
            ILogger<WalletMvcController> logger)
        {
            _walletService = walletService;
            _salesService = salesService;
            _logger = logger;
        }

        /// <summary>
        /// 錢包首頁
        /// </summary>
        /// <returns>錢包頁面</returns>
        [HttpGet]
        [Route("wallet")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var userId = GetCurrentUserId();
                var walletBalance = await _walletService.GetWalletBalanceAsync(userId);
                var salesWallet = await _walletService.GetSalesWalletInfoAsync(userId);
                var salesStats = await _salesService.GetSalesStatisticsAsync(userId);

                var viewModel = new WalletIndexViewModel
                {
                    WalletBalance = walletBalance,
                    SalesWallet = salesWallet,
                    SalesStatistics = salesStats
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "載入錢包頁面時發生錯誤");
                TempData["Error"] = "載入錢包資訊失敗";
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// 點數流水記錄頁面
        /// </summary>
        /// <returns>流水記錄頁面</returns>
        [HttpGet]
        [Route("wallet/ledger")]
        public async Task<IActionResult> Ledger([FromQuery] PointTransactionQueryRequest request)
        {
            try
            {
                var userId = GetCurrentUserId();
                var transactions = await _walletService.GetPointTransactionsAsync(userId, request);

                var viewModel = new LedgerViewModel
                {
                    Transactions = transactions,
                    QueryRequest = request
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "載入點數流水記錄頁面時發生錯誤");
                TempData["Error"] = "載入流水記錄失敗";
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// 銷售權限申請頁面
        /// </summary>
        /// <returns>申請頁面</returns>
        [HttpGet]
        [Route("wallet/sales/permission")]
        public async Task<IActionResult> SalesPermission()
        {
            try
            {
                var userId = GetCurrentUserId();
                var currentStatus = await _walletService.GetSalesPermissionStatusAsync(userId);
                var hasAuthority = await _walletService.HasSalesAuthorityAsync(userId);

                var viewModel = new SalesPermissionViewModel
                {
                    CurrentStatus = currentStatus,
                    HasAuthority = hasAuthority
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "載入銷售權限申請頁面時發生錯誤");
                TempData["Error"] = "載入申請資訊失敗";
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// 提交銷售權限申請
        /// </summary>
        /// <param name="request">申請資訊</param>
        /// <returns>申請結果</returns>
        [HttpPost]
        [Route("wallet/sales/permission")]
        public async Task<IActionResult> SubmitSalesPermission([FromForm] SalesPermissionRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["Error"] = "請檢查輸入資料";
                    return RedirectToAction("SalesPermission");
                }

                var userId = GetCurrentUserId();
                var result = await _walletService.ApplySalesPermissionAsync(userId, request);

                TempData["Success"] = "銷售權限申請已提交，請等待審核結果";
                return RedirectToAction("SalesPermission");
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("SalesPermission");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "提交銷售權限申請時發生錯誤");
                TempData["Error"] = "提交申請失敗";
                return RedirectToAction("SalesPermission");
            }
        }

        /// <summary>
        /// 銷售錢包管理頁面
        /// </summary>
        /// <returns>銷售錢包頁面</returns>
        [HttpGet]
        [Route("wallet/sales")]
        public async Task<IActionResult> SalesWallet()
        {
            try
            {
                var userId = GetCurrentUserId();
                var salesWallet = await _walletService.GetSalesWalletInfoAsync(userId);
                var salesStats = await _salesService.GetSalesStatisticsAsync(userId);

                var viewModel = new SalesWalletViewModel
                {
                    SalesWallet = salesWallet,
                    SalesStatistics = salesStats
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "載入銷售錢包頁面時發生錯誤");
                TempData["Error"] = "載入銷售錢包資訊失敗";
                return RedirectToAction("Index");
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
    /// 錢包首頁視圖模型
    /// </summary>
    public class WalletIndexViewModel
    {
        public WalletBalanceResponse WalletBalance { get; set; } = new();
        public SalesWalletInfo SalesWallet { get; set; } = new();
        public SalesStatisticsResponse SalesStatistics { get; set; } = new();
    }

    /// <summary>
    /// 流水記錄視圖模型
    /// </summary>
    public class LedgerViewModel
    {
        public PointTransactionQueryResponse Transactions { get; set; } = new();
        public PointTransactionQueryRequest QueryRequest { get; set; } = new();
    }

    /// <summary>
    /// 銷售權限申請視圖模型
    /// </summary>
    public class SalesPermissionViewModel
    {
        public SalesPermissionResponse? CurrentStatus { get; set; }
        public bool HasAuthority { get; set; }
    }

    /// <summary>
    /// 銷售錢包視圖模型
    /// </summary>
    public class SalesWalletViewModel
    {
        public SalesWalletInfo SalesWallet { get; set; } = new();
        public SalesStatisticsResponse SalesStatistics { get; set; } = new();
    }
} 