using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GameCore.Core.Services;
using GameCore.Core.DTOs;
using System.Security.Claims;

namespace GameCore.Web.Controllers
{
    /// <summary>
    /// 官方商城控制器
    /// 提供商品瀏覽、購物車、訂單等功能
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class StoreController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<StoreController> _logger;

        public StoreController(
            IProductService productService,
            ILogger<StoreController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        /// <summary>
        /// 取得商品列表
        /// </summary>
        [HttpGet("products")]
        public async Task<IActionResult> GetProducts(
            [FromQuery] int? categoryId = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            try
            {
                var products = await _productService.GetProductsAsync(categoryId, page, pageSize);
                return Ok(new
                {
                    success = true,
                    data = products,
                    message = "取得商品列表成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得商品列表失敗");
                return StatusCode(500, new
                {
                    success = false,
                    message = "取得商品列表失敗"
                });
            }
        }

        /// <summary>
        /// 取得商品詳情
        /// </summary>
        [HttpGet("products/{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "商品不存在"
                    });
                }

                return Ok(new
                {
                    success = true,
                    data = product,
                    message = "取得商品詳情成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得商品詳情失敗: ProductId={ProductId}", id);
                return StatusCode(500, new
                {
                    success = false,
                    message = "取得商品詳情失敗"
                });
            }
        }

        /// <summary>
        /// 搜尋商品
        /// </summary>
        [HttpGet("products/search")]
        public async Task<IActionResult> SearchProducts(
            [FromQuery] string? keyword = null,
            [FromQuery] int? categoryId = null,
            [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            try
            {
                var products = await _productService.SearchProductsAsync(
                    keyword, categoryId, minPrice, maxPrice, page, pageSize);

                return Ok(new
                {
                    success = true,
                    data = products,
                    message = "搜尋商品成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "搜尋商品失敗");
                return StatusCode(500, new
                {
                    success = false,
                    message = "搜尋商品失敗"
                });
            }
        }

        /// <summary>
        /// 取得熱門商品
        /// </summary>
        [HttpGet("products/popular")]
        public async Task<IActionResult> GetPopularProducts([FromQuery] int limit = 10)
        {
            try
            {
                var products = await _productService.GetPopularProductsAsync(limit);
                return Ok(new
                {
                    success = true,
                    data = products,
                    message = "取得熱門商品成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得熱門商品失敗");
                return StatusCode(500, new
                {
                    success = false,
                    message = "取得熱門商品失敗"
                });
            }
        }

        /// <summary>
        /// 取得推薦商品
        /// </summary>
        [HttpGet("products/recommended")]
        [Authorize]
        public async Task<IActionResult> GetRecommendedProducts([FromQuery] int limit = 10)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var products = await _productService.GetRecommendedProductsAsync(userId, limit);
                return Ok(new
                {
                    success = true,
                    data = products,
                    message = "取得推薦商品成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得推薦商品失敗");
                return StatusCode(500, new
                {
                    success = false,
                    message = "取得推薦商品失敗"
                });
            }
        }

        /// <summary>
        /// 檢查商品庫存
        /// </summary>
        [HttpGet("products/{id}/stock")]
        public async Task<IActionResult> CheckStock(int id, [FromQuery] int quantity = 1)
        {
            try
            {
                var hasStock = await _productService.CheckStockAsync(id, quantity);
                return Ok(new
                {
                    success = true,
                    data = new { hasStock, quantity },
                    message = hasStock ? "庫存充足" : "庫存不足"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "檢查庫存失敗: ProductId={ProductId}", id);
                return StatusCode(500, new
                {
                    success = false,
                    message = "檢查庫存失敗"
                });
            }
        }

        /// <summary>
        /// 取得商品統計
        /// </summary>
        [HttpGet("products/{id}/statistics")]
        public async Task<IActionResult> GetProductStatistics(int id)
        {
            try
            {
                var statistics = await _productService.GetProductStatisticsAsync(id);
                return Ok(new
                {
                    success = true,
                    data = statistics,
                    message = "取得商品統計成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得商品統計失敗: ProductId={ProductId}", id);
                return StatusCode(500, new
                {
                    success = false,
                    message = "取得商品統計失敗"
                });
            }
        }
    }
}