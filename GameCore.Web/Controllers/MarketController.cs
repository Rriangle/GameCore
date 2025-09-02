using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GameCore.Core.Services;
using GameCore.Core.DTOs;
using System.Security.Claims;

namespace GameCore.Web.Controllers
{
    /// <summary>
    /// 玩家市場控制器
    /// 提供商品上架、交易、評價等功能
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MarketController : ControllerBase
    {
        private readonly IPlayerMarketService _marketService;
        private readonly ILogger<MarketController> _logger;

        public MarketController(
            IPlayerMarketService marketService,
            ILogger<MarketController> logger)
        {
            _marketService = marketService;
            _logger = logger;
        }

        /// <summary>
        /// 取得市場商品列表
        /// </summary>
        [HttpGet("products")]
        [AllowAnonymous]
        public async Task<IActionResult> GetMarketProducts(
            [FromQuery] string? keyword = null,
            [FromQuery] int? categoryId = null,
            [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            try
            {
                var products = await _marketService.GetMarketProductsAsync(
                    keyword, categoryId, minPrice, maxPrice, page, pageSize);

                return Ok(new
                {
                    success = true,
                    data = products,
                    message = "取得市場商品列表成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得市場商品列表失敗");
                return StatusCode(500, new
                {
                    success = false,
                    message = "取得市場商品列表失敗"
                });
            }
        }

        /// <summary>
        /// 取得市場商品詳情
        /// </summary>
        [HttpGet("products/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetMarketProduct(int id)
        {
            try
            {
                var product = await _marketService.GetMarketProductByIdAsync(id);
                if (product == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "市場商品不存在"
                    });
                }

                return Ok(new
                {
                    success = true,
                    data = product,
                    message = "取得市場商品詳情成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得市場商品詳情失敗: ProductId={ProductId}", id);
                return StatusCode(500, new
                {
                    success = false,
                    message = "取得市場商品詳情失敗"
                });
            }
        }

        /// <summary>
        /// 上架商品
        /// </summary>
        [HttpPost("products")]
        public async Task<IActionResult> CreateMarketProduct([FromBody] CreateMarketProductRequest request)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var product = await _marketService.CreateMarketProductAsync(userId, request);

                return Ok(new
                {
                    success = true,
                    data = product,
                    message = "商品上架成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "商品上架失敗");
                return StatusCode(500, new
                {
                    success = false,
                    message = "商品上架失敗"
                });
            }
        }

        /// <summary>
        /// 更新商品
        /// </summary>
        [HttpPut("products/{id}")]
        public async Task<IActionResult> UpdateMarketProduct(int id, [FromBody] UpdateMarketProductRequest request)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var success = await _marketService.UpdateMarketProductAsync(userId, id, request);

                if (!success)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "更新商品失敗"
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "更新商品成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新商品失敗: ProductId={ProductId}", id);
                return StatusCode(500, new
                {
                    success = false,
                    message = "更新商品失敗"
                });
            }
        }

        /// <summary>
        /// 下架商品
        /// </summary>
        [HttpDelete("products/{id}")]
        public async Task<IActionResult> DeleteMarketProduct(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var success = await _marketService.DeleteMarketProductAsync(userId, id);

                if (!success)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "下架商品失敗"
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "下架商品成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "下架商品失敗: ProductId={ProductId}", id);
                return StatusCode(500, new
                {
                    success = false,
                    message = "下架商品失敗"
                });
            }
        }

        /// <summary>
        /// 購買商品
        /// </summary>
        [HttpPost("products/{id}/purchase")]
        public async Task<IActionResult> PurchaseProduct(int id, [FromBody] PurchaseRequest request)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var result = await _marketService.PurchaseProductAsync(userId, id, request);

                if (!result.Success)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = result.Message
                    });
                }

                return Ok(new
                {
                    success = true,
                    data = result,
                    message = "購買成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "購買商品失敗: ProductId={ProductId}", id);
                return StatusCode(500, new
                {
                    success = false,
                    message = "購買商品失敗"
                });
            }
        }

        /// <summary>
        /// 取得我的商品
        /// </summary>
        [HttpGet("my-products")]
        public async Task<IActionResult> GetMyProducts()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var products = await _marketService.GetUserMarketItemsAsync(userId);

                return Ok(new
                {
                    success = true,
                    data = products,
                    message = "取得我的商品成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得我的商品失敗");
                return StatusCode(500, new
                {
                    success = false,
                    message = "取得我的商品失敗"
                });
            }
        }

        /// <summary>
        /// 取得交易記錄
        /// </summary>
        [HttpGet("transactions")]
        public async Task<IActionResult> GetTransactions()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var transactions = await _marketService.GetUserTransactionsAsync(userId);

                return Ok(new
                {
                    success = true,
                    data = transactions,
                    message = "取得交易記錄成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得交易記錄失敗");
                return StatusCode(500, new
                {
                    success = false,
                    message = "取得交易記錄失敗"
                });
            }
        }
    }

    /// <summary>
    /// 創建市場商品請求
    /// </summary>
    public class CreateMarketProductRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ProductType { get; set; } = string.Empty;
        public int? ProductId { get; set; }
    }

    /// <summary>
    /// 更新市場商品請求
    /// </summary>
    public class UpdateMarketProductRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    /// <summary>
    /// 購買請求
    /// </summary>
    public class PurchaseRequest
    {
        public int Quantity { get; set; } = 1;
        public string PaymentMethod { get; set; } = string.Empty;
    }
} 