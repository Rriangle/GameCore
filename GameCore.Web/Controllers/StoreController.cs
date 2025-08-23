using Microsoft.AspNetCore.Mvc;
using GameCore.Core.Services;
using GameCore.Core.Entities;
using Microsoft.AspNetCore.Authorization;

namespace GameCore.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoreController : ControllerBase
    {
        private readonly IStoreService _storeService;
        private readonly ILogger<StoreController> _logger;

        public StoreController(IStoreService storeService, ILogger<StoreController> logger)
        {
            _storeService = storeService;
            _logger = logger;
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetProducts([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string category = "")
        {
            try
            {
                var products = await _storeService.GetProductsAsync(page, pageSize, category);
                return Ok(new { Success = true, Data = products });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting products");
                return StatusCode(500, new { Success = false, Message = "取得商品清單失敗" });
            }
        }

        [HttpGet("products/{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            try
            {
                var product = await _storeService.GetProductByIdAsync(id);
                if (product == null)
                {
                    return NotFound(new { Success = false, Message = "商品不存在" });
                }
                return Ok(new { Success = true, Data = product });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting product {ProductId}", id);
                return StatusCode(500, new { Success = false, Message = "取得商品資訊失敗" });
            }
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var categories = await _storeService.GetCategoriesAsync();
                return Ok(new { Success = true, Data = categories });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting categories");
                return StatusCode(500, new { Success = false, Message = "取得分類清單失敗" });
            }
        }

        [HttpPost("products/{productId}/purchase")]
        [Authorize]
        public async Task<IActionResult> PurchaseProduct(int productId, [FromBody] PurchaseRequest request)
        {
            try
            {
                var userId = GetCurrentUserId();
                var order = await _storeService.CreateOrderAsync(userId, productId, request.Quantity);
                return Ok(new { Success = true, Data = order, Message = "購買成功" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error purchasing product {ProductId}", productId);
                return StatusCode(500, new { Success = false, Message = "購買失敗" });
            }
        }

        [HttpGet("cart")]
        [Authorize]
        public async Task<IActionResult> GetCart()
        {
            try
            {
                var userId = GetCurrentUserId();
                var cartItems = await _storeService.GetCartItemsAsync(userId);
                return Ok(new { Success = true, Data = cartItems });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cart for user {UserId}", GetCurrentUserId());
                return StatusCode(500, new { Success = false, Message = "取得購物車失敗" });
            }
        }

        [HttpPost("cart/add")]
        [Authorize]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _storeService.AddToCartAsync(userId, request.ProductId, request.Quantity);
                return Ok(new { Success = true, Message = "已加入購物車" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding to cart");
                return StatusCode(500, new { Success = false, Message = "加入購物車失敗" });
            }
        }

        [HttpPut("cart/{itemId}")]
        [Authorize]
        public async Task<IActionResult> UpdateCartItem(int itemId, [FromBody] UpdateCartRequest request)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _storeService.UpdateCartItemAsync(userId, itemId, request.Quantity);
                return Ok(new { Success = true, Message = "購物車已更新" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating cart item {ItemId}", itemId);
                return StatusCode(500, new { Success = false, Message = "更新購物車失敗" });
            }
        }

        [HttpDelete("cart/{itemId}")]
        [Authorize]
        public async Task<IActionResult> RemoveFromCart(int itemId)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _storeService.RemoveFromCartAsync(userId, itemId);
                return Ok(new { Success = true, Message = "已移除商品" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing cart item {ItemId}", itemId);
                return StatusCode(500, new { Success = false, Message = "移除商品失敗" });
            }
        }

        [HttpPost("cart/checkout")]
        [Authorize]
        public async Task<IActionResult> Checkout([FromBody] CheckoutRequest request)
        {
            try
            {
                var userId = GetCurrentUserId();
                var order = await _storeService.CheckoutAsync(userId, request.DeliveryAddress, request.PaymentMethod);
                return Ok(new { Success = true, Data = order, Message = "結帳成功" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during checkout for user {UserId}", GetCurrentUserId());
                return StatusCode(500, new { Success = false, Message = "結帳失敗" });
            }
        }

        [HttpGet("orders")]
        [Authorize]
        public async Task<IActionResult> GetOrders([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                var userId = GetCurrentUserId();
                var orders = await _storeService.GetUserOrdersAsync(userId, page, pageSize);
                return Ok(new { Success = true, Data = orders });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting orders for user {UserId}", GetCurrentUserId());
                return StatusCode(500, new { Success = false, Message = "取得訂單清單失敗" });
            }
        }

        [HttpGet("orders/{orderId}")]
        [Authorize]
        public async Task<IActionResult> GetOrder(int orderId)
        {
            try
            {
                var userId = GetCurrentUserId();
                var order = await _storeService.GetOrderByIdAsync(orderId);
                if (order == null || order.UserId != userId)
                {
                    return NotFound(new { Success = false, Message = "訂單不存在" });
                }
                return Ok(new { Success = true, Data = order });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting order {OrderId}", orderId);
                return StatusCode(500, new { Success = false, Message = "取得訂單資訊失敗" });
            }
        }

        [HttpPost("orders/{orderId}/cancel")]
        [Authorize]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _storeService.CancelOrderAsync(orderId, userId);
                return Ok(new { Success = true, Message = "訂單已取消" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling order {OrderId}", orderId);
                return StatusCode(500, new { Success = false, Message = "取消訂單失敗" });
            }
        }

        [HttpGet("popular")]
        public async Task<IActionResult> GetPopularProducts([FromQuery] int take = 10)
        {
            try
            {
                var products = await _storeService.GetPopularProductsAsync(take);
                return Ok(new { Success = true, Data = products });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting popular products");
                return StatusCode(500, new { Success = false, Message = "取得熱門商品失敗" });
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts([FromQuery] string keyword, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                var products = await _storeService.SearchProductsAsync(keyword, page, pageSize);
                return Ok(new { Success = true, Data = products });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching products with keyword {Keyword}", keyword);
                return StatusCode(500, new { Success = false, Message = "搜尋商品失敗" });
            }
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : 0;
        }
    }

    public class PurchaseRequest
    {
        public int Quantity { get; set; }
    }

    public class AddToCartRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateCartRequest
    {
        public int Quantity { get; set; }
    }

    public class CheckoutRequest
    {
        public string DeliveryAddress { get; set; } = "";
        public string PaymentMethod { get; set; } = "";
    }
}