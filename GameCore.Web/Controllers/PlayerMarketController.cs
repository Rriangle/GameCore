using Microsoft.AspNetCore.Mvc;
using GameCore.Core.Services;
using GameCore.Core.Entities;
using Microsoft.AspNetCore.Authorization;

namespace GameCore.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerMarketController : ControllerBase
    {
        private readonly IPlayerMarketService _playerMarketService;
        private readonly ILogger<PlayerMarketController> _logger;

        public PlayerMarketController(IPlayerMarketService playerMarketService, ILogger<PlayerMarketController> logger)
        {
            _playerMarketService = playerMarketService;
            _logger = logger;
        }

        [HttpGet("listings")]
        public async Task<IActionResult> GetListings([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                var listings = await _playerMarketService.GetActiveListingsAsync(page, pageSize);
                return Ok(new { Success = true, Data = listings });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting market listings");
                return StatusCode(500, new { Success = false, Message = "取得商品清單失敗" });
            }
        }

        [HttpGet("listings/search")]
        public async Task<IActionResult> SearchListings([FromQuery] string keyword, [FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                var listings = await _playerMarketService.SearchListingsAsync(keyword, minPrice, maxPrice, page, pageSize);
                return Ok(new { Success = true, Data = listings });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching market listings");
                return StatusCode(500, new { Success = false, Message = "搜尋商品失敗" });
            }
        }

        [HttpGet("listings/{id}")]
        public async Task<IActionResult> GetListing(int id)
        {
            try
            {
                var listing = await _playerMarketService.GetTransactionAsync(id);
                if (listing == null)
                {
                    return NotFound(new { Success = false, Message = "商品不存在" });
                }
                return Ok(new { Success = true, Data = listing });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting listing {ListingId}", id);
                return StatusCode(500, new { Success = false, Message = "取得商品資訊失敗" });
            }
        }

        [HttpPost("listings")]
        [Authorize]
        public async Task<IActionResult> CreateListing([FromBody] CreateListingRequest request)
        {
            try
            {
                var userId = GetCurrentUserId();
                var listing = await _playerMarketService.CreateListingAsync(
                    userId, 
                    request.ItemName, 
                    request.Description, 
                    request.Price, 
                    request.Quantity);
                return Ok(new { Success = true, Data = listing, Message = "商品上架成功" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating listing");
                return StatusCode(500, new { Success = false, Message = "商品上架失敗" });
            }
        }

        [HttpPut("listings/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateListing(int id, [FromBody] UpdateListingRequest request)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _playerMarketService.UpdateListingAsync(id, userId, request.Price, request.Description);
                return Ok(new { Success = true, Message = "商品資訊已更新" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating listing {ListingId}", id);
                return StatusCode(500, new { Success = false, Message = "更新商品失敗" });
            }
        }

        [HttpDelete("listings/{id}")]
        [Authorize]
        public async Task<IActionResult> RemoveListing(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _playerMarketService.RemoveListingAsync(id, userId);
                return Ok(new { Success = true, Message = "商品已下架" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing listing {ListingId}", id);
                return StatusCode(500, new { Success = false, Message = "下架商品失敗" });
            }
        }

        [HttpPost("listings/{id}/purchase")]
        [Authorize]
        public async Task<IActionResult> PurchaseListing(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var transaction = await _playerMarketService.PurchaseItemAsync(id, userId);
                return Ok(new { Success = true, Data = transaction, Message = "購買成功" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error purchasing listing {ListingId}", id);
                return StatusCode(500, new { Success = false, Message = "購買失敗" });
            }
        }

        [HttpGet("my-listings")]
        [Authorize]
        public async Task<IActionResult> GetMyListings([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                var userId = GetCurrentUserId();
                var listings = await _playerMarketService.GetUserListingsAsync(userId, page, pageSize);
                return Ok(new { Success = true, Data = listings });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user listings");
                return StatusCode(500, new { Success = false, Message = "取得我的商品失敗" });
            }
        }

        [HttpGet("my-purchases")]
        [Authorize]
        public async Task<IActionResult> GetMyPurchases([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                var userId = GetCurrentUserId();
                var purchases = await _playerMarketService.GetUserPurchasesAsync(userId, page, pageSize);
                return Ok(new { Success = true, Data = purchases });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user purchases");
                return StatusCode(500, new { Success = false, Message = "取得購買記錄失敗" });
            }
        }

        [HttpPost("transactions/{transactionId}/review")]
        [Authorize]
        public async Task<IActionResult> CreateReview(int transactionId, [FromBody] CreateReviewRequest request)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _playerMarketService.CreateReviewAsync(
                    transactionId, 
                    userId, 
                    request.Rating, 
                    request.Comment);
                return Ok(new { Success = true, Message = "評價已提交" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating review for transaction {TransactionId}", transactionId);
                return StatusCode(500, new { Success = false, Message = "評價提交失敗" });
            }
        }

        [HttpGet("users/{userId}/reviews")]
        public async Task<IActionResult> GetUserReviews(int userId)
        {
            try
            {
                var reviews = await _playerMarketService.GetUserReviewsAsync(userId);
                return Ok(new { Success = true, Data = reviews });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user reviews for user {UserId}", userId);
                return StatusCode(500, new { Success = false, Message = "取得用戶評價失敗" });
            }
        }

        [HttpGet("users/{userId}/rating")]
        public async Task<IActionResult> GetUserRating(int userId)
        {
            try
            {
                var rating = await _playerMarketService.GetUserRatingAsync(userId);
                return Ok(new { Success = true, Data = new { Rating = rating } });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user rating for user {UserId}", userId);
                return StatusCode(500, new { Success = false, Message = "取得用戶評分失敗" });
            }
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetMarketStats([FromQuery] int days = 30)
        {
            try
            {
                var stats = await _playerMarketService.GetMarketStatsAsync(days);
                return Ok(new { Success = true, Data = stats });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting market stats");
                return StatusCode(500, new { Success = false, Message = "取得市場統計失敗" });
            }
        }

        [HttpGet("popular")]
        public async Task<IActionResult> GetPopularItems([FromQuery] int days = 30, [FromQuery] int take = 10)
        {
            try
            {
                var items = await _playerMarketService.GetPopularItemsAsync(days, take);
                return Ok(new { Success = true, Data = items });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting popular items");
                return StatusCode(500, new { Success = false, Message = "取得熱門商品失敗" });
            }
        }

        [HttpGet("recent")]
        public async Task<IActionResult> GetRecentTransactions([FromQuery] int days = 7)
        {
            try
            {
                var transactions = await _playerMarketService.GetRecentTransactionsAsync(days);
                return Ok(new { Success = true, Data = transactions });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting recent transactions");
                return StatusCode(500, new { Success = false, Message = "取得最近交易失敗" });
            }
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : 0;
        }
    }

    public class CreateListingRequest
    {
        public string ItemName { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateListingRequest
    {
        public decimal Price { get; set; }
        public string Description { get; set; } = "";
    }

    public class CreateReviewRequest
    {
        public int Rating { get; set; }
        public string Comment { get; set; } = "";
    }
}