using GameCore.Core.DTOs;
using GameCore.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GameCore.Web.Controllers
{
    /// <summary>
    /// 管理員銷售權限審核 API 控制器
    /// </summary>
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "admin,moderator")] // 需要管理員或版主權限
    public class AdminSalesController : ControllerBase
    {
        private readonly ISalesService _salesService;
        private readonly IWalletService _walletService;
        private readonly ILogger<AdminSalesController> _logger;

        public AdminSalesController(
            ISalesService salesService,
            IWalletService walletService,
            ILogger<AdminSalesController> logger)
        {
            _salesService = salesService;
            _walletService = walletService;
            _logger = logger;
        }

        /// <summary>
        /// 取得銷售權限申請列表
        /// </summary>
        /// <param name="request">查詢條件</param>
        /// <returns>申請列表</returns>
        [HttpGet("permissions")]
        public async Task<ActionResult<SalesPermissionListResponse>> GetPermissionApplications([FromQuery] SalesPermissionListRequest request)
        {
            try
            {
                var applications = await _salesService.GetSalesPermissionApplicationsAsync(request);
                return Ok(applications);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得銷售權限申請列表時發生錯誤");
                return StatusCode(500, new { error = "取得申請列表失敗" });
            }
        }

        /// <summary>
        /// 審核銷售權限申請
        /// </summary>
        /// <param name="request">審核請求</param>
        /// <returns>審核結果</returns>
        [HttpPost("permissions/review")]
        public async Task<ActionResult<SalesPermissionReviewResponse>> ReviewPermission([FromBody] SalesPermissionReviewRequest request)
        {
            try
            {
                var managerId = GetCurrentManagerId();
                request.ManagerId = managerId;

                var result = await _salesService.ReviewSalesPermissionAsync(request);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "審核銷售權限申請時發生錯誤");
                return StatusCode(500, new { error = "審核申請失敗" });
            }
        }

        /// <summary>
        /// 調整用戶點數（管理員功能）
        /// </summary>
        /// <param name="request">調整請求</param>
        /// <returns>調整結果</returns>
        [HttpPost("wallet/adjust")]
        public async Task<ActionResult<AdminPointAdjustmentResponse>> AdjustUserPoints([FromBody] AdminPointAdjustmentRequest request)
        {
            try
            {
                var managerId = GetCurrentManagerId();
                request.ManagerId = managerId;

                var result = await _walletService.AdjustUserPointsAsync(request);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "調整用戶點數時發生錯誤");
                return StatusCode(500, new { error = "調整點數失敗" });
            }
        }

        /// <summary>
        /// 取得當前管理員ID
        /// </summary>
        /// <returns>管理員ID</returns>
        private int GetCurrentManagerId()
        {
            var managerIdClaim = User.FindFirst("ManagerId")?.Value;
            if (int.TryParse(managerIdClaim, out var managerId))
            {
                return managerId;
            }
            
            // 如果沒有 ManagerId claim，嘗試從 NameIdentifier 取得
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out var userId))
            {
                return userId;
            }
            
            throw new InvalidOperationException("無法取得當前管理員ID");
        }
    }
} 