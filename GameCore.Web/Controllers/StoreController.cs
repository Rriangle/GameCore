using GameCore.Core.DTOs;
using GameCore.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GameCore.Web.Controllers
{
    /// <summary>
    /// 官方商城控制器 - 完整實現B2C電商功能
    /// 提供商品瀏覽、購物車管理、訂單流程、排行榜查詢等完整電商功能
    /// 嚴格按照規格要求實現商城業務邏輯和狀態機管理
    /// </summary>
    [ApiController]
    [Route("api/store")]
    public class StoreController : ControllerBase
    {
        private readonly IStoreService _storeService;
        private readonly ILogger<StoreController> _logger;

        public StoreController(
            IStoreService storeService,
            ILogger<StoreController> logger)
        {
            _storeService = storeService;
            _logger = logger;
        }

        #region 商品管理 API

        /// <summary>
        /// 取得商品列表
        /// GET /api/store/products
        /// </summary>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <param name="category">商品分類</param>
        /// <param name="inStock">是否有庫存</param>
        /// <param name="sortBy">排序欄位</param>
        /// <param name="sortDirection">排序方向</param>
        /// <returns>商品列表</returns>
        [HttpGet("products")]
        public async Task<IActionResult> GetProducts(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? category = null,
            [FromQuery] bool? inStock = null,
            [FromQuery] string sortBy = "CreatedAt",
            [FromQuery] string sortDirection = "desc")
        {
            try
            {
                _logger.LogInformation("取得商品列表: Page={Page}, PageSize={PageSize}, Category={Category}", 
                    page, pageSize, category);

                var searchDto = new ProductSearchDto
                {
                    ProductType = category,
                    InStock = inStock,
                    Page = page,
                    PageSize = Math.Min(pageSize, 100), // 限制最大頁面大小
                    SortBy = sortBy,
                    SortDirection = sortDirection
                };

                var result = await _storeService.SearchProductsAsync(searchDto);

                return Ok(new
                {
                    success = true,
                    data = result,
                    message = "商品列表取得成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得商品列表時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得商品列表時發生錯誤" });
            }
        }

        /// <summary>
        /// 取得商品詳細資訊
        /// GET /api/store/products/{id}
        /// </summary>
        /// <param name="id">商品ID</param>
        /// <returns>商品詳細資訊</returns>
        [HttpGet("products/{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            try
            {
                _logger.LogInformation("取得商品詳細資訊: ProductId={ProductId}", id);

                var product = await _storeService.GetProductDetailAsync(id);

                if (product == null)
                {
                    return NotFound(new { success = false, message = "商品不存在" });
                }

                return Ok(new
                {
                    success = true,
                    data = product,
                    message = "商品詳細資訊取得成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得商品詳細資訊時發生錯誤: ProductId={ProductId}", id);
                return StatusCode(500, new { success = false, message = "取得商品詳細資訊時發生錯誤" });
            }
        }

        /// <summary>
        /// 搜尋商品
        /// POST /api/store/products/search
        /// </summary>
        /// <param name="searchDto">搜尋條件</param>
        /// <returns>搜尋結果</returns>
        [HttpPost("products/search")]
        public async Task<IActionResult> SearchProducts([FromBody] ProductSearchDto searchDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { success = false, message = "搜尋條件格式錯誤", errors = ModelState });
                }

                _logger.LogInformation("搜尋商品: Keyword={Keyword}, ProductType={ProductType}", 
                    searchDto.Keyword, searchDto.ProductType);

                // 限制最大頁面大小
                searchDto.PageSize = Math.Min(searchDto.PageSize, 100);

                var result = await _storeService.SearchProductsAsync(searchDto);

                return Ok(new
                {
                    success = true,
                    data = result,
                    message = "商品搜尋完成"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "搜尋商品時發生錯誤");
                return StatusCode(500, new { success = false, message = "搜尋商品時發生錯誤" });
            }
        }

        /// <summary>
        /// 取得商品分類
        /// GET /api/store/categories
        /// </summary>
        /// <returns>分類列表</returns>
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                _logger.LogInformation("取得商品分類");

                var categories = await _storeService.GetProductCategoriesAsync();

                return Ok(new
                {
                    success = true,
                    data = categories,
                    message = "商品分類取得成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得商品分類時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得商品分類時發生錯誤" });
            }
        }

        /// <summary>
        /// 取得熱門商品
        /// GET /api/store/products/popular
        /// </summary>
        /// <param name="limit">限制筆數</param>
        /// <returns>熱門商品列表</returns>
        [HttpGet("products/popular")]
        public async Task<IActionResult> GetPopularProducts([FromQuery] int limit = 10)
        {
            try
            {
                _logger.LogInformation("取得熱門商品: Limit={Limit}", limit);

                var products = await _storeService.GetPopularProductsAsync(Math.Min(limit, 50));

                return Ok(new
                {
                    success = true,
                    data = products,
                    message = "熱門商品取得成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得熱門商品時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得熱門商品時發生錯誤" });
            }
        }

        #endregion

        #region 購物車管理 API

        /// <summary>
        /// 取得購物車
        /// GET /api/store/cart
        /// </summary>
        /// <returns>購物車資訊</returns>
        [HttpGet("cart")]
        [Authorize] // 需要登入
        public async Task<IActionResult> GetCart()
        {
            try
            {
                var userId = GetCurrentUserId();
                _logger.LogInformation("取得購物車: UserId={UserId}", userId);

                var cart = await _storeService.GetCartAsync(userId);

                return Ok(new
                {
                    success = true,
                    data = cart,
                    message = "購物車取得成功"
                });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { success = false, message = "使用者未登入" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得購物車時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得購物車時發生錯誤" });
            }
        }

        /// <summary>
        /// 加入購物車
        /// POST /api/store/cart/add
        /// </summary>
        /// <param name="addToCartDto">加入購物車請求</param>
        /// <returns>操作結果</returns>
        [HttpPost("cart/add")]
        [Authorize]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDto addToCartDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { success = false, message = "資料格式錯誤", errors = ModelState });
                }

                var userId = GetCurrentUserId();
                _logger.LogInformation("加入購物車: UserId={UserId}, ProductId={ProductId}, Quantity={Quantity}", 
                    userId, addToCartDto.ProductId, addToCartDto.Quantity);

                var result = await _storeService.AddToCartAsync(userId, addToCartDto);

                if (result.Success)
                {
                    return Ok(new
                    {
                        success = true,
                        data = result.Data,
                        message = result.Message
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = result.Message,
                        errors = result.Errors
                    });
                }
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { success = false, message = "使用者未登入" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "加入購物車時發生錯誤");
                return StatusCode(500, new { success = false, message = "加入購物車時發生錯誤" });
            }
        }

        /// <summary>
        /// 更新購物車項目
        /// PUT /api/store/cart/update
        /// </summary>
        /// <param name="updateCartItemDto">更新購物車項目請求</param>
        /// <returns>操作結果</returns>
        [HttpPut("cart/update")]
        [Authorize]
        public async Task<IActionResult> UpdateCartItem([FromBody] UpdateCartItemDto updateCartItemDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { success = false, message = "資料格式錯誤", errors = ModelState });
                }

                var userId = GetCurrentUserId();
                _logger.LogInformation("更新購物車項目: UserId={UserId}, ProductId={ProductId}, Quantity={Quantity}", 
                    userId, updateCartItemDto.ProductId, updateCartItemDto.Quantity);

                var result = await _storeService.UpdateCartItemAsync(userId, updateCartItemDto);

                if (result.Success)
                {
                    return Ok(new
                    {
                        success = true,
                        data = result.Data,
                        message = result.Message
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = result.Message,
                        errors = result.Errors
                    });
                }
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { success = false, message = "使用者未登入" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新購物車項目時發生錯誤");
                return StatusCode(500, new { success = false, message = "更新購物車項目時發生錯誤" });
            }
        }

        /// <summary>
        /// 移除購物車項目
        /// DELETE /api/store/cart/remove/{productId}
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <returns>操作結果</returns>
        [HttpDelete("cart/remove/{productId}")]
        [Authorize]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            try
            {
                var userId = GetCurrentUserId();
                _logger.LogInformation("移除購物車項目: UserId={UserId}, ProductId={ProductId}", userId, productId);

                var result = await _storeService.RemoveFromCartAsync(userId, productId);

                if (result.Success)
                {
                    return Ok(new
                    {
                        success = true,
                        data = result.Data,
                        message = result.Message
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = result.Message
                    });
                }
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { success = false, message = "使用者未登入" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "移除購物車項目時發生錯誤");
                return StatusCode(500, new { success = false, message = "移除購物車項目時發生錯誤" });
            }
        }

        /// <summary>
        /// 清空購物車
        /// DELETE /api/store/cart/clear
        /// </summary>
        /// <returns>操作結果</returns>
        [HttpDelete("cart/clear")]
        [Authorize]
        public async Task<IActionResult> ClearCart()
        {
            try
            {
                var userId = GetCurrentUserId();
                _logger.LogInformation("清空購物車: UserId={UserId}", userId);

                var result = await _storeService.ClearCartAsync(userId);

                if (result.Success)
                {
                    return Ok(new
                    {
                        success = true,
                        message = result.Message
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = result.Message
                    });
                }
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { success = false, message = "使用者未登入" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "清空購物車時發生錯誤");
                return StatusCode(500, new { success = false, message = "清空購物車時發生錯誤" });
            }
        }

        #endregion

        #region 訂單管理 API

        /// <summary>
        /// 建立訂單
        /// POST /api/store/orders/create
        /// </summary>
        /// <param name="createOrderDto">建立訂單請求</param>
        /// <returns>訂單資訊</returns>
        [HttpPost("orders/create")]
        [Authorize]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { success = false, message = "訂單資料格式錯誤", errors = ModelState });
                }

                var userId = GetCurrentUserId();
                _logger.LogInformation("建立訂單: UserId={UserId}, ItemCount={ItemCount}", 
                    userId, createOrderDto.Items.Count);

                var result = await _storeService.CreateOrderAsync(userId, createOrderDto);

                if (result.Success)
                {
                    return Ok(new
                    {
                        success = true,
                        data = result.Data,
                        message = result.Message
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = result.Message,
                        errors = result.Errors
                    });
                }
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { success = false, message = "使用者未登入" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "建立訂單時發生錯誤");
                return StatusCode(500, new { success = false, message = "建立訂單時發生錯誤" });
            }
        }

        /// <summary>
        /// 從購物車建立訂單
        /// POST /api/store/orders/create-from-cart
        /// </summary>
        /// <returns>訂單資訊</returns>
        [HttpPost("orders/create-from-cart")]
        [Authorize]
        public async Task<IActionResult> CreateOrderFromCart()
        {
            try
            {
                var userId = GetCurrentUserId();
                _logger.LogInformation("從購物車建立訂單: UserId={UserId}", userId);

                var result = await _storeService.CreateOrderFromCartAsync(userId);

                if (result.Success)
                {
                    return Ok(new
                    {
                        success = true,
                        data = result.Data,
                        message = result.Message
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = result.Message,
                        errors = result.Errors
                    });
                }
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { success = false, message = "使用者未登入" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "從購物車建立訂單時發生錯誤");
                return StatusCode(500, new { success = false, message = "從購物車建立訂單時發生錯誤" });
            }
        }

        /// <summary>
        /// 取得訂單詳細資訊
        /// GET /api/store/orders/{id}
        /// </summary>
        /// <param name="id">訂單ID</param>
        /// <returns>訂單詳細資訊</returns>
        [HttpGet("orders/{id}")]
        [Authorize]
        public async Task<IActionResult> GetOrder(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                _logger.LogInformation("取得訂單詳細資訊: UserId={UserId}, OrderId={OrderId}", userId, id);

                var order = await _storeService.GetOrderAsync(userId, id);

                if (order == null)
                {
                    return NotFound(new { success = false, message = "訂單不存在或無權限查看" });
                }

                return Ok(new
                {
                    success = true,
                    data = order,
                    message = "訂單詳細資訊取得成功"
                });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { success = false, message = "使用者未登入" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得訂單詳細資訊時發生錯誤: OrderId={OrderId}", id);
                return StatusCode(500, new { success = false, message = "取得訂單詳細資訊時發生錯誤" });
            }
        }

        /// <summary>
        /// 取得使用者訂單列表
        /// GET /api/store/orders
        /// </summary>
        /// <param name="orderStatus">訂單狀態篩選</param>
        /// <param name="paymentStatus">付款狀態篩選</param>
        /// <param name="fromDate">開始日期</param>
        /// <param name="toDate">結束日期</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <returns>訂單列表</returns>
        [HttpGet("orders")]
        [Authorize]
        public async Task<IActionResult> GetOrders(
            [FromQuery] string? orderStatus = null,
            [FromQuery] string? paymentStatus = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            try
            {
                var userId = GetCurrentUserId();
                _logger.LogInformation("取得使用者訂單列表: UserId={UserId}, Page={Page}", userId, page);

                var queryDto = new OrderQueryDto
                {
                    OrderStatus = orderStatus,
                    PaymentStatus = paymentStatus,
                    FromDate = fromDate,
                    ToDate = toDate,
                    Page = page,
                    PageSize = Math.Min(pageSize, 100)
                };

                var result = await _storeService.GetOrdersAsync(userId, queryDto);

                return Ok(new
                {
                    success = true,
                    data = result,
                    message = "訂單列表取得成功"
                });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { success = false, message = "使用者未登入" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得使用者訂單列表時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得使用者訂單列表時發生錯誤" });
            }
        }

        /// <summary>
        /// 模擬付款 (開發用)
        /// POST /api/store/orders/{id}/pay
        /// </summary>
        /// <param name="id">訂單ID</param>
        /// <returns>付款結果</returns>
        [HttpPost("orders/{id}/pay")]
        [Authorize]
        public async Task<IActionResult> PayOrder(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                _logger.LogInformation("模擬付款: UserId={UserId}, OrderId={OrderId}", userId, id);

                var result = await _storeService.ProcessPaymentAsync(userId, id);

                if (result.Success)
                {
                    return Ok(new
                    {
                        success = true,
                        data = result.Data,
                        message = result.Message
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = result.Message,
                        errors = result.Errors
                    });
                }
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { success = false, message = "使用者未登入" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "模擬付款時發生錯誤: OrderId={OrderId}", id);
                return StatusCode(500, new { success = false, message = "模擬付款時發生錯誤" });
            }
        }

        #endregion

        #region 排行榜 API

        /// <summary>
        /// 取得商城排行榜
        /// GET /api/store/rankings
        /// </summary>
        /// <param name="periodType">榜單型態</param>
        /// <param name="rankingMetric">排名指標</param>
        /// <param name="date">查詢日期</param>
        /// <param name="limit">限制筆數</param>
        /// <returns>排行榜</returns>
        [HttpGet("rankings")]
        public async Task<IActionResult> GetRankings(
            [FromQuery] string periodType = "daily",
            [FromQuery] string rankingMetric = "trading_amount",
            [FromQuery] DateTime? date = null,
            [FromQuery] int limit = 50)
        {
            try
            {
                _logger.LogInformation("取得商城排行榜: PeriodType={PeriodType}, Metric={Metric}", 
                    periodType, rankingMetric);

                var queryDto = new RankingQueryDto
                {
                    PeriodType = periodType,
                    RankingMetric = rankingMetric,
                    Date = date,
                    Limit = Math.Min(limit, 100)
                };

                var rankings = await _storeService.GetRankingsAsync(queryDto);

                return Ok(new
                {
                    success = true,
                    data = rankings,
                    message = "排行榜取得成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得商城排行榜時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得商城排行榜時發生錯誤" });
            }
        }

        #endregion

        #region 統計 API

        /// <summary>
        /// 取得商城統計
        /// GET /api/store/statistics
        /// </summary>
        /// <returns>商城統計資訊</returns>
        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics()
        {
            try
            {
                _logger.LogInformation("取得商城統計");

                var statistics = await _storeService.GetStatisticsAsync();

                return Ok(new
                {
                    success = true,
                    data = statistics,
                    message = "商城統計取得成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得商城統計時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得商城統計時發生錯誤" });
            }
        }

        #endregion

        #region 管理員 API

        /// <summary>
        /// 管理員取得所有訂單 (需要管理員權限)
        /// GET /api/store/admin/orders
        /// </summary>
        [HttpGet("admin/orders")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOrders([FromQuery] OrderQueryDto queryDto)
        {
            try
            {
                var adminId = GetCurrentUserId();
                _logger.LogInformation("管理員取得所有訂單: AdminId={AdminId}", adminId);

                var result = await _storeService.GetAllOrdersAsync(queryDto);

                return Ok(new
                {
                    success = true,
                    data = result,
                    message = "所有訂單取得成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "管理員取得所有訂單時發生錯誤");
                return StatusCode(500, new { success = false, message = "管理員取得所有訂單時發生錯誤" });
            }
        }

        /// <summary>
        /// 管理員更新訂單狀態
        /// PUT /api/store/admin/orders/{id}/status
        /// </summary>
        [HttpPut("admin/orders/{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusDto statusDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { success = false, message = "狀態資料格式錯誤", errors = ModelState });
                }

                var adminId = GetCurrentUserId();
                _logger.LogInformation("管理員更新訂單狀態: AdminId={AdminId}, OrderId={OrderId}, Status={Status}", 
                    adminId, id, statusDto.OrderStatus);

                var result = await _storeService.UpdateOrderStatusAsync(id, statusDto);

                if (result.Success)
                {
                    return Ok(new
                    {
                        success = true,
                        data = result.Data,
                        message = result.Message
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = result.Message,
                        errors = result.Errors
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "管理員更新訂單狀態時發生錯誤: OrderId={OrderId}", id);
                return StatusCode(500, new { success = false, message = "管理員更新訂單狀態時發生錯誤" });
            }
        }

        #endregion

        #region 輔助方法

        /// <summary>
        /// 取得當前登入使用者的 ID
        /// </summary>
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

    #region 管理員 DTO

    /// <summary>
    /// 更新訂單狀態請求 DTO
    /// </summary>
    public class UpdateOrderStatusDto
    {
        /// <summary>訂單狀態</summary>
        [Required]
        public string OrderStatus { get; set; } = string.Empty;

        /// <summary>付款狀態</summary>
        public string? PaymentStatus { get; set; }

        /// <summary>備註</summary>
        public string? Notes { get; set; }
    }

    #endregion
}