using GameCore.Core.DTOs;
using GameCore.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GameCore.Web.Controllers
{
    /// <summary>
    /// 自由市場控制器 - 完整實現C2C交易功能
    /// 提供商品上架、交易頁面、即時訊息、排行榜查詢等完整C2C功能
    /// 嚴格按照規格要求實現自由市場業務邏輯和狀態機管理
    /// </summary>
    [ApiController]
    [Route("api/market")]
    public class PlayerMarketController : ControllerBase
    {
        private readonly IPlayerMarketService _playerMarketService;
        private readonly ILogger<PlayerMarketController> _logger;

        public PlayerMarketController(
            IPlayerMarketService playerMarketService,
            ILogger<PlayerMarketController> logger)
        {
            _playerMarketService = playerMarketService;
            _logger = logger;
        }

        #region 商品管理 API

        /// <summary>
        /// 取得市場商品列表
        /// GET /api/market/products
        /// </summary>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <param name="category">商品分類</param>
        /// <param name="status">商品狀態</param>
        /// <param name="sortBy">排序欄位</param>
        /// <param name="sortDirection">排序方向</param>
        /// <returns>商品列表</returns>
        [HttpGet("products")]
        public async Task<IActionResult> GetProducts(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? category = null,
            [FromQuery] string? status = "active",
            [FromQuery] string sortBy = "CreatedAt",
            [FromQuery] string sortDirection = "desc")
        {
            try
            {
                _logger.LogInformation("取得市場商品列表: Page={Page}, PageSize={PageSize}, Category={Category}", 
                    page, pageSize, category);

                var searchDto = new PlayerMarketSearchDto
                {
                    PProductType = category,
                    PStatus = status,
                    Page = page,
                    PageSize = Math.Min(pageSize, 100), // 限制最大頁面大小
                    SortBy = sortBy,
                    SortDirection = sortDirection
                };

                var result = await _playerMarketService.SearchProductsAsync(searchDto);

                return Ok(new
                {
                    success = true,
                    data = result,
                    message = "市場商品列表取得成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得市場商品列表時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得市場商品列表時發生錯誤" });
            }
        }

        /// <summary>
        /// 取得商品詳細資訊
        /// GET /api/market/products/{id}
        /// </summary>
        /// <param name="id">商品ID</param>
        /// <returns>商品詳細資訊</returns>
        [HttpGet("products/{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            try
            {
                _logger.LogInformation("取得市場商品詳細資訊: ProductId={ProductId}", id);

                var product = await _playerMarketService.GetProductDetailAsync(id);

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
                _logger.LogError(ex, "取得市場商品詳細資訊時發生錯誤: ProductId={ProductId}", id);
                return StatusCode(500, new { success = false, message = "取得商品詳細資訊時發生錯誤" });
            }
        }

        /// <summary>
        /// 搜尋商品
        /// POST /api/market/products/search
        /// </summary>
        /// <param name="searchDto">搜尋條件</param>
        /// <returns>搜尋結果</returns>
        [HttpPost("products/search")]
        public async Task<IActionResult> SearchProducts([FromBody] PlayerMarketSearchDto searchDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { success = false, message = "搜尋條件格式錯誤", errors = ModelState });
                }

                _logger.LogInformation("搜尋市場商品: Keyword={Keyword}, ProductType={ProductType}", 
                    searchDto.Keyword, searchDto.PProductType);

                // 限制最大頁面大小
                searchDto.PageSize = Math.Min(searchDto.PageSize, 100);

                var result = await _playerMarketService.SearchProductsAsync(searchDto);

                return Ok(new
                {
                    success = true,
                    data = result,
                    message = "商品搜尋完成"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "搜尋市場商品時發生錯誤");
                return StatusCode(500, new { success = false, message = "搜尋商品時發生錯誤" });
            }
        }

        /// <summary>
        /// 上架商品 (需要銷售權限)
        /// POST /api/market/products
        /// </summary>
        /// <param name="createDto">建立商品請求</param>
        /// <returns>上架結果</returns>
        [HttpPost("products")]
        [Authorize] // 需要登入
        public async Task<IActionResult> CreateProduct([FromBody] CreatePlayerMarketProductDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { success = false, message = "商品資料格式錯誤", errors = ModelState });
                }

                var userId = GetCurrentUserId();
                _logger.LogInformation("使用者 {UserId} 上架商品: {ProductName}", userId, createDto.PProductName);

                var result = await _playerMarketService.CreateProductAsync(userId, createDto);

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
                _logger.LogError(ex, "上架商品時發生錯誤");
                return StatusCode(500, new { success = false, message = "上架商品時發生錯誤" });
            }
        }

        /// <summary>
        /// 更新商品資訊 (僅限商品擁有者)
        /// PUT /api/market/products/{id}
        /// </summary>
        /// <param name="id">商品ID</param>
        /// <param name="updateDto">更新商品請求</param>
        /// <returns>更新結果</returns>
        [HttpPut("products/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdatePlayerMarketProductDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { success = false, message = "更新資料格式錯誤", errors = ModelState });
                }

                var userId = GetCurrentUserId();
                _logger.LogInformation("使用者 {UserId} 更新商品 {ProductId}", userId, id);

                var result = await _playerMarketService.UpdateProductAsync(userId, id, updateDto);

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
                _logger.LogError(ex, "更新商品時發生錯誤: ProductId={ProductId}", id);
                return StatusCode(500, new { success = false, message = "更新商品時發生錯誤" });
            }
        }

        /// <summary>
        /// 上傳商品圖片
        /// POST /api/market/products/{id}/images
        /// </summary>
        /// <param name="id">商品ID</param>
        /// <param name="images">圖片資料 (Base64)</param>
        /// <returns>上傳結果</returns>
        [HttpPost("products/{id}/images")]
        [Authorize]
        public async Task<IActionResult> UploadProductImages(int id, [FromBody] List<string> images)
        {
            try
            {
                if (images == null || !images.Any())
                {
                    return BadRequest(new { success = false, message = "請選擇要上傳的圖片" });
                }

                var userId = GetCurrentUserId();
                _logger.LogInformation("使用者 {UserId} 上傳商品 {ProductId} 圖片", userId, id);

                var result = await _playerMarketService.UploadProductImagesAsync(userId, id, images);

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
                _logger.LogError(ex, "上傳商品圖片時發生錯誤: ProductId={ProductId}", id);
                return StatusCode(500, new { success = false, message = "上傳圖片時發生錯誤" });
            }
        }

        /// <summary>
        /// 取得使用者的商品列表
        /// GET /api/market/my-products
        /// </summary>
        /// <param name="status">商品狀態篩選</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <returns>使用者商品列表</returns>
        [HttpGet("my-products")]
        [Authorize]
        public async Task<IActionResult> GetMyProducts(
            [FromQuery] string? status = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            try
            {
                var userId = GetCurrentUserId();
                _logger.LogInformation("取得使用者 {UserId} 的商品列表", userId);

                var searchDto = new PlayerMarketSearchDto
                {
                    SellerId = userId,
                    PStatus = status,
                    Page = page,
                    PageSize = Math.Min(pageSize, 100)
                };

                var result = await _playerMarketService.SearchProductsAsync(searchDto);

                return Ok(new
                {
                    success = true,
                    data = result,
                    message = "我的商品列表取得成功"
                });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { success = false, message = "使用者未登入" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得使用者商品列表時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得商品列表時發生錯誤" });
            }
        }

        #endregion

        #region 訂單管理 API

        /// <summary>
        /// 下單購買
        /// POST /api/market/orders
        /// </summary>
        /// <param name="createOrderDto">建立訂單請求</param>
        /// <returns>訂單資訊</returns>
        [HttpPost("orders")]
        [Authorize]
        public async Task<IActionResult> CreateOrder([FromBody] CreatePlayerMarketOrderDto createOrderDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { success = false, message = "訂單資料格式錯誤", errors = ModelState });
                }

                var userId = GetCurrentUserId();
                _logger.LogInformation("使用者 {UserId} 建立市場訂單: ProductId={ProductId}", 
                    userId, createOrderDto.PProductId);

                var result = await _playerMarketService.CreateOrderAsync(userId, createOrderDto);

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
                _logger.LogError(ex, "建立市場訂單時發生錯誤");
                return StatusCode(500, new { success = false, message = "建立訂單時發生錯誤" });
            }
        }

        /// <summary>
        /// 取得訂單詳細資訊
        /// GET /api/market/orders/{id}
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
                _logger.LogInformation("取得市場訂單詳細資訊: UserId={UserId}, OrderId={OrderId}", userId, id);

                var order = await _playerMarketService.GetOrderAsync(userId, id);

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
                _logger.LogError(ex, "取得市場訂單詳細資訊時發生錯誤: OrderId={OrderId}", id);
                return StatusCode(500, new { success = false, message = "取得訂單詳細資訊時發生錯誤" });
            }
        }

        /// <summary>
        /// 取得使用者訂單列表 (買家和賣家)
        /// GET /api/market/orders
        /// </summary>
        /// <param name="role">角色篩選 (buyer/seller)</param>
        /// <param name="status">訂單狀態篩選</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <returns>訂單列表</returns>
        [HttpGet("orders")]
        [Authorize]
        public async Task<IActionResult> GetOrders(
            [FromQuery] string? role = null,
            [FromQuery] string? status = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            try
            {
                var userId = GetCurrentUserId();
                _logger.LogInformation("取得使用者 {UserId} 的市場訂單列表: Role={Role}", userId, role);

                var result = await _playerMarketService.GetUserOrdersAsync(userId, role, status, page, Math.Min(pageSize, 100));

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
                _logger.LogError(ex, "取得使用者市場訂單列表時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得訂單列表時發生錯誤" });
            }
        }

        #endregion

        #region 交易頁面 API

        /// <summary>
        /// 建立交易頁面
        /// POST /api/market/tradepages
        /// </summary>
        /// <param name="orderId">訂單ID</param>
        /// <returns>交易頁面資訊</returns>
        [HttpPost("tradepages")]
        [Authorize]
        public async Task<IActionResult> CreateTradepage([FromBody] int orderId)
        {
            try
            {
                var userId = GetCurrentUserId();
                _logger.LogInformation("使用者 {UserId} 建立交易頁面: OrderId={OrderId}", userId, orderId);

                var result = await _playerMarketService.CreateTradepageAsync(userId, orderId);

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
                _logger.LogError(ex, "建立交易頁面時發生錯誤");
                return StatusCode(500, new { success = false, message = "建立交易頁面時發生錯誤" });
            }
        }

        /// <summary>
        /// 取得交易頁面詳細資訊
        /// GET /api/market/tradepages/{id}
        /// </summary>
        /// <param name="id">交易頁面ID</param>
        /// <returns>交易頁面詳細資訊</returns>
        [HttpGet("tradepages/{id}")]
        [Authorize]
        public async Task<IActionResult> GetTradepage(int id)
        {
            try
            {
                var userId = GetCurrentUserId();
                _logger.LogInformation("取得交易頁面詳細資訊: UserId={UserId}, TradepageId={TradepageId}", userId, id);

                var tradepage = await _playerMarketService.GetTradepageAsync(userId, id);

                if (tradepage == null)
                {
                    return NotFound(new { success = false, message = "交易頁面不存在或無權限查看" });
                }

                return Ok(new
                {
                    success = true,
                    data = tradepage,
                    message = "交易頁面詳細資訊取得成功"
                });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { success = false, message = "使用者未登入" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得交易頁面詳細資訊時發生錯誤: TradepageId={TradepageId}", id);
                return StatusCode(500, new { success = false, message = "取得交易頁面詳細資訊時發生錯誤" });
            }
        }

        /// <summary>
        /// 發送交易訊息
        /// POST /api/market/tradepages/{id}/messages
        /// </summary>
        /// <param name="id">交易頁面ID</param>
        /// <param name="messageDto">訊息內容</param>
        /// <returns>發送結果</returns>
        [HttpPost("tradepages/{id}/messages")]
        [Authorize]
        public async Task<IActionResult> SendTradeMessage(int id, [FromBody] SendTradeMessageDto messageDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { success = false, message = "訊息格式錯誤", errors = ModelState });
                }

                var userId = GetCurrentUserId();
                _logger.LogInformation("使用者 {UserId} 發送交易訊息: TradepageId={TradepageId}", userId, id);

                var result = await _playerMarketService.SendTradeMessageAsync(userId, id, messageDto);

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
                _logger.LogError(ex, "發送交易訊息時發生錯誤: TradepageId={TradepageId}", id);
                return StatusCode(500, new { success = false, message = "發送訊息時發生錯誤" });
            }
        }

        /// <summary>
        /// 賣家確認移交
        /// POST /api/market/tradepages/{id}/seller-transferred
        /// </summary>
        /// <param name="id">交易頁面ID</param>
        /// <param name="confirmDto">確認資訊</param>
        /// <returns>確認結果</returns>
        [HttpPost("tradepages/{id}/seller-transferred")]
        [Authorize]
        public async Task<IActionResult> ConfirmSellerTransfer(int id, [FromBody] ConfirmTransferDto confirmDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                _logger.LogInformation("賣家 {UserId} 確認移交: TradepageId={TradepageId}", userId, id);

                var result = await _playerMarketService.ConfirmSellerTransferAsync(userId, id, confirmDto);

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
                _logger.LogError(ex, "賣家確認移交時發生錯誤: TradepageId={TradepageId}", id);
                return StatusCode(500, new { success = false, message = "確認移交時發生錯誤" });
            }
        }

        /// <summary>
        /// 買家確認接收
        /// POST /api/market/tradepages/{id}/buyer-received
        /// </summary>
        /// <param name="id">交易頁面ID</param>
        /// <param name="confirmDto">確認資訊</param>
        /// <returns>確認結果</returns>
        [HttpPost("tradepages/{id}/buyer-received")]
        [Authorize]
        public async Task<IActionResult> ConfirmBuyerReceived(int id, [FromBody] ConfirmTransferDto confirmDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                _logger.LogInformation("買家 {UserId} 確認接收: TradepageId={TradepageId}", userId, id);

                var result = await _playerMarketService.ConfirmBuyerReceivedAsync(userId, id, confirmDto);

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
                _logger.LogError(ex, "買家確認接收時發生錯誤: TradepageId={TradepageId}", id);
                return StatusCode(500, new { success = false, message = "確認接收時發生錯誤" });
            }
        }

        #endregion

        #region 排行榜 API

        /// <summary>
        /// 取得自由市場排行榜
        /// GET /api/market/rankings
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
                _logger.LogInformation("取得自由市場排行榜: PeriodType={PeriodType}, Metric={Metric}", 
                    periodType, rankingMetric);

                var queryDto = new PlayerMarketRankingQueryDto
                {
                    PPeriodType = periodType,
                    PRankingMetric = rankingMetric,
                    Date = date,
                    Limit = Math.Min(limit, 100)
                };

                var rankings = await _playerMarketService.GetRankingsAsync(queryDto);

                return Ok(new
                {
                    success = true,
                    data = rankings,
                    message = "排行榜取得成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得自由市場排行榜時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得排行榜時發生錯誤" });
            }
        }

        #endregion

        #region 統計 API

        /// <summary>
        /// 取得自由市場統計
        /// GET /api/market/statistics
        /// </summary>
        /// <returns>市場統計資訊</returns>
        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics()
        {
            try
            {
                _logger.LogInformation("取得自由市場統計");

                var statistics = await _playerMarketService.GetStatisticsAsync();

                return Ok(new
                {
                    success = true,
                    data = statistics,
                    message = "市場統計取得成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得自由市場統計時發生錯誤");
                return StatusCode(500, new { success = false, message = "取得市場統計時發生錯誤" });
            }
        }

        #endregion

        #region 管理員 API

        /// <summary>
        /// 管理員取得所有商品 (需要管理員權限)
        /// GET /api/market/admin/products
        /// </summary>
        [HttpGet("admin/products")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllProducts([FromQuery] PlayerMarketSearchDto searchDto)
        {
            try
            {
                var adminId = GetCurrentUserId();
                _logger.LogInformation("管理員 {AdminId} 取得所有市場商品", adminId);

                var result = await _playerMarketService.SearchProductsAsync(searchDto);

                return Ok(new
                {
                    success = true,
                    data = result,
                    message = "所有商品取得成功"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "管理員取得所有商品時發生錯誤");
                return StatusCode(500, new { success = false, message = "管理員取得所有商品時發生錯誤" });
            }
        }

        /// <summary>
        /// 管理員更新商品狀態
        /// PUT /api/market/admin/products/{id}/status
        /// </summary>
        [HttpPut("admin/products/{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProductStatus(int id, [FromBody] UpdateProductStatusDto statusDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { success = false, message = "狀態資料格式錯誤", errors = ModelState });
                }

                var adminId = GetCurrentUserId();
                _logger.LogInformation("管理員 {AdminId} 更新商品狀態: ProductId={ProductId}, Status={Status}", 
                    adminId, id, statusDto.PStatus);

                var result = await _playerMarketService.UpdateProductStatusAsync(id, statusDto);

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
                _logger.LogError(ex, "管理員更新商品狀態時發生錯誤: ProductId={ProductId}", id);
                return StatusCode(500, new { success = false, message = "管理員更新商品狀態時發生錯誤" });
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
    /// 更新商品狀態請求 DTO
    /// </summary>
    public class UpdateProductStatusDto
    {
        /// <summary>商品狀態</summary>
        [Required]
        public string PStatus { get; set; } = string.Empty;

        /// <summary>狀態變更原因</summary>
        public string? Reason { get; set; }

        /// <summary>備註</summary>
        public string? Notes { get; set; }
    }

    #endregion
}