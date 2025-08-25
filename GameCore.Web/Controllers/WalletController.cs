using GameCore.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameCore.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WalletController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<WalletController> _logger;

        public WalletController(IUnitOfWork unitOfWork, ILogger<WalletController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpGet("balance")]
        public async Task<IActionResult> GetBalance()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId <= 0)
                {
                    return Unauthorized(new { code = "unauthorized", message = "未登入" });
                }

                var wallet = await _unitOfWork.UserRepository.GetWalletAsync(userId);
                var balance = wallet?.UserPoint ?? 0;
                return Ok(new { balance });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetBalance failed for user {UserId}", GetCurrentUserId());
                return StatusCode(500, new { code = "server_error", message = "伺服器錯誤" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetWallet()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId <= 0)
                {
                    return Unauthorized(new { code = "unauthorized", message = "未登入" });
                }

                var wallet = await _unitOfWork.UserRepository.GetWalletAsync(userId);
                if (wallet == null)
                {
                    return NotFound(new { code = "not_found", message = "找不到錢包資料" });
                }

                return Ok(new
                {
                    userId = wallet.UserId,
                    balance = wallet.UserPoint,
                    coupon = wallet.CouponNumber
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetWallet failed for user {UserId}", GetCurrentUserId());
                return StatusCode(500, new { code = "server_error", message = "伺服器錯誤" });
            }
        }

        private int GetCurrentUserId()
        {
            var val = User.FindFirst("UserId")?.Value;
            return int.TryParse(val, out var id) ? id : 0;
        }
    }
}

