using GameCore.Core.DTOs;
using GameCore.Core.Services;
using GameCore.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;
using Xunit;

namespace GameCore.Tests.Controllers
{
    /// <summary>
    /// 官方商城控制器測試類別
    /// 測試所有商城相關API的功能和邊界條件
    /// 驗證商品管理、購物車操作、訂單流程、排行榜查詢等完整電商功能
    /// </summary>
    public class StoreControllerTests
    {
        private readonly Mock<IStoreService> _mockStoreService;
        private readonly Mock<ILogger<StoreController>> _mockLogger;
        private readonly StoreController _controller;

        public StoreControllerTests()
        {
            _mockStoreService = new Mock<IStoreService>();
            _mockLogger = new Mock<ILogger<StoreController>>();
            _controller = new StoreController(_mockStoreService.Object, _mockLogger.Object);

            // 設定模擬的使用者身份
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "123"),
                new Claim(ClaimTypes.Name, "testuser")
            };
            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = principal
                }
            };
        }

        #region 商品管理測試

        [Fact]
        public async Task GetProducts_ShouldReturnProductList_WhenValidRequest()
        {
            // Arrange
            var expectedProducts = new PagedResult<ProductListDto>
            {
                Page = 1,
                PageSize = 20,
                TotalCount = 100,
                Data = new List<ProductListDto>
                {
                    new() { ProductId = 1, ProductName = "Test Game 1", ProductType = "遊戲", Price = 1590.00m, CurrencyCode = "TWD", StockQuantity = 50 },
                    new() { ProductId = 2, ProductName = "Test Game 2", ProductType = "遊戲", Price = 990.00m, CurrencyCode = "TWD", StockQuantity = 30 }
                }
            };

            _mockStoreService
                .Setup(s => s.SearchProductsAsync(It.IsAny<ProductSearchDto>()))
                .ReturnsAsync(expectedProducts);

            // Act
            var result = await _controller.GetProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            Assert.NotNull(response);
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var dataProperty = responseType.GetProperty("data");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.True((bool)successProperty.GetValue(response));
            Assert.NotNull(dataProperty.GetValue(response));
            Assert.Equal("商品列表取得成功", messageProperty.GetValue(response));
        }

        [Fact]
        public async Task GetProduct_ShouldReturnProductDetail_WhenProductExists()
        {
            // Arrange
            var expectedProduct = new ProductDetailDto
            {
                ProductId = 1,
                ProductName = "Test Game",
                ProductType = "遊戲",
                Price = 1590.00m,
                CurrencyCode = "TWD",
                StockQuantity = 50,
                ProductDescription = "測試遊戲描述",
                Supplier = new SupplierDto { SupplierId = 1, SupplierName = "Test Supplier" }
            };

            _mockStoreService
                .Setup(s => s.GetProductDetailAsync(1))
                .ReturnsAsync(expectedProduct);

            // Act
            var result = await _controller.GetProduct(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var dataProperty = responseType.GetProperty("data");
            
            Assert.True((bool)successProperty.GetValue(response));
            
            var data = dataProperty.GetValue(response) as ProductDetailDto;
            Assert.NotNull(data);
            Assert.Equal("Test Game", data.ProductName);
            Assert.Equal(1590.00m, data.Price);
        }

        [Fact]
        public async Task GetProduct_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            _mockStoreService
                .Setup(s => s.GetProductDetailAsync(999))
                .ReturnsAsync((ProductDetailDto?)null);

            // Act
            var result = await _controller.GetProduct(999);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = notFoundResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.False((bool)successProperty.GetValue(response));
            Assert.Equal("商品不存在", messageProperty.GetValue(response));
        }

        [Fact]
        public async Task SearchProducts_ShouldReturnSearchResults_WhenValidSearchDto()
        {
            // Arrange
            var searchDto = new ProductSearchDto
            {
                Keyword = "Test",
                ProductType = "遊戲",
                MinPrice = 500,
                MaxPrice = 2000,
                Page = 1,
                PageSize = 10
            };

            var expectedResults = new PagedResult<ProductListDto>
            {
                Page = 1,
                PageSize = 10,
                TotalCount = 25,
                Data = new List<ProductListDto>
                {
                    new() { ProductId = 1, ProductName = "Test Game", ProductType = "遊戲", Price = 1590.00m }
                }
            };

            _mockStoreService
                .Setup(s => s.SearchProductsAsync(It.Is<ProductSearchDto>(dto => 
                    dto.Keyword == "Test" && 
                    dto.ProductType == "遊戲" && 
                    dto.MinPrice == 500)))
                .ReturnsAsync(expectedResults);

            // Act
            var result = await _controller.SearchProducts(searchDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as PagedResult<ProductListDto>;
            Assert.NotNull(data);
            Assert.Equal(25, data.TotalCount);
            Assert.Single(data.Data);
        }

        [Fact]
        public async Task GetCategories_ShouldReturnCategoryList()
        {
            // Arrange
            var expectedCategories = new List<string> { "遊戲", "周邊", "點數卡", "收藏品" };

            _mockStoreService
                .Setup(s => s.GetProductCategoriesAsync())
                .ReturnsAsync(expectedCategories);

            // Act
            var result = await _controller.GetCategories();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as List<string>;
            Assert.NotNull(data);
            Assert.Equal(4, data.Count);
            Assert.Contains("遊戲", data);
        }

        [Fact]
        public async Task GetPopularProducts_ShouldReturnPopularProducts()
        {
            // Arrange
            var expectedProducts = new List<ProductListDto>
            {
                new() { ProductId = 1, ProductName = "熱門遊戲1", Price = 1590.00m },
                new() { ProductId = 2, ProductName = "熱門遊戲2", Price = 990.00m }
            };

            _mockStoreService
                .Setup(s => s.GetPopularProductsAsync(10))
                .ReturnsAsync(expectedProducts);

            // Act
            var result = await _controller.GetPopularProducts(10);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as List<ProductListDto>;
            Assert.NotNull(data);
            Assert.Equal(2, data.Count);
        }

        #endregion

        #region 購物車管理測試

        [Fact]
        public async Task GetCart_ShouldReturnCart_WhenUserLoggedIn()
        {
            // Arrange
            var expectedCart = new ShoppingCartDto
            {
                UserId = 123,
                Items = new List<CartItemDto>
                {
                    new() { ProductId = 1, ProductName = "Test Game", UnitPrice = 1590.00m, Quantity = 1, CurrencyCode = "TWD", StockQuantity = 50 }
                }
            };

            _mockStoreService
                .Setup(s => s.GetCartAsync(123))
                .ReturnsAsync(expectedCart);

            // Act
            var result = await _controller.GetCart();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as ShoppingCartDto;
            Assert.NotNull(data);
            Assert.Equal(123, data.UserId);
            Assert.Single(data.Items);
            Assert.Equal(1590.00m, data.Total);
        }

        [Fact]
        public async Task AddToCart_ShouldReturnSuccess_WhenValidRequest()
        {
            // Arrange
            var addToCartDto = new AddToCartDto { ProductId = 1, Quantity = 2 };
            var expectedResult = ServiceResult<ShoppingCartDto>.CreateSuccess(
                new ShoppingCartDto { UserId = 123 }, 
                "商品已加入購物車");

            _mockStoreService
                .Setup(s => s.AddToCartAsync(123, addToCartDto))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.AddToCart(addToCartDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.True((bool)successProperty.GetValue(response));
            Assert.Equal("商品已加入購物車", messageProperty.GetValue(response));
        }

        [Fact]
        public async Task AddToCart_ShouldReturnBadRequest_WhenServiceFails()
        {
            // Arrange
            var addToCartDto = new AddToCartDto { ProductId = 1, Quantity = 2 };
            var expectedResult = ServiceResult<ShoppingCartDto>.CreateFailure(
                "庫存不足", 
                new List<string> { "商品庫存只剩1個" });

            _mockStoreService
                .Setup(s => s.AddToCartAsync(123, addToCartDto))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.AddToCart(addToCartDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = badRequestResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.False((bool)successProperty.GetValue(response));
            Assert.Equal("庫存不足", messageProperty.GetValue(response));
        }

        [Fact]
        public async Task UpdateCartItem_ShouldReturnSuccess_WhenValidRequest()
        {
            // Arrange
            var updateDto = new UpdateCartItemDto { ProductId = 1, Quantity = 3 };
            var expectedResult = ServiceResult<ShoppingCartDto>.CreateSuccess(
                new ShoppingCartDto { UserId = 123 }, 
                "購物車項目已更新");

            _mockStoreService
                .Setup(s => s.UpdateCartItemAsync(123, updateDto))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.UpdateCartItem(updateDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            _mockStoreService.Verify(s => s.UpdateCartItemAsync(123, updateDto), Times.Once);
        }

        [Fact]
        public async Task RemoveFromCart_ShouldReturnSuccess_WhenValidRequest()
        {
            // Arrange
            var expectedResult = ServiceResult<ShoppingCartDto>.CreateSuccess(
                new ShoppingCartDto { UserId = 123 }, 
                "商品已從購物車移除");

            _mockStoreService
                .Setup(s => s.RemoveFromCartAsync(123, 1))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.RemoveFromCart(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            _mockStoreService.Verify(s => s.RemoveFromCartAsync(123, 1), Times.Once);
        }

        [Fact]
        public async Task ClearCart_ShouldReturnSuccess_WhenValidRequest()
        {
            // Arrange
            var expectedResult = ServiceResult.CreateSuccess("購物車已清空");

            _mockStoreService
                .Setup(s => s.ClearCartAsync(123))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.ClearCart();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            _mockStoreService.Verify(s => s.ClearCartAsync(123), Times.Once);
        }

        #endregion

        #region 訂單管理測試

        [Fact]
        public async Task CreateOrder_ShouldReturnSuccess_WhenValidRequest()
        {
            // Arrange
            var createOrderDto = new CreateOrderDto
            {
                Items = new List<CreateOrderItemDto>
                {
                    new() { ProductId = 1, Quantity = 1 },
                    new() { ProductId = 2, Quantity = 2 }
                }
            };

            var expectedOrder = new OrderDto
            {
                OrderId = 100,
                UserId = 123,
                OrderStatus = "Created",
                PaymentStatus = "Pending",
                OrderTotal = 3180.00m
            };

            var expectedResult = ServiceResult<OrderDto>.CreateSuccess(expectedOrder, "訂單建立成功");

            _mockStoreService
                .Setup(s => s.CreateOrderAsync(123, createOrderDto))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.CreateOrder(createOrderDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as OrderDto;
            Assert.NotNull(data);
            Assert.Equal(100, data.OrderId);
            Assert.Equal(3180.00m, data.OrderTotal);
        }

        [Fact]
        public async Task CreateOrder_ShouldReturnBadRequest_WhenInvalidModel()
        {
            // Arrange
            var createOrderDto = new CreateOrderDto { Items = new List<CreateOrderItemDto>() }; // 空項目列表

            _controller.ModelState.AddModelError("Items", "訂單必須包含至少一個項目");

            // Act
            var result = await _controller.CreateOrder(createOrderDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = badRequestResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.False((bool)successProperty.GetValue(response));
            Assert.Equal("訂單資料格式錯誤", messageProperty.GetValue(response));
        }

        [Fact]
        public async Task CreateOrderFromCart_ShouldReturnSuccess_WhenCartHasItems()
        {
            // Arrange
            var expectedOrder = new OrderDto
            {
                OrderId = 101,
                UserId = 123,
                OrderStatus = "Created",
                PaymentStatus = "Pending",
                OrderTotal = 1590.00m
            };

            var expectedResult = ServiceResult<OrderDto>.CreateSuccess(expectedOrder, "從購物車建立訂單成功");

            _mockStoreService
                .Setup(s => s.CreateOrderFromCartAsync(123))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.CreateOrderFromCart();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            _mockStoreService.Verify(s => s.CreateOrderFromCartAsync(123), Times.Once);
        }

        [Fact]
        public async Task GetOrder_ShouldReturnOrder_WhenOrderExists()
        {
            // Arrange
            var expectedOrder = new OrderDto
            {
                OrderId = 100,
                UserId = 123,
                OrderStatus = "Completed",
                PaymentStatus = "Paid",
                OrderTotal = 1590.00m,
                Items = new List<OrderItemDto>
                {
                    new() { ItemId = 1, ProductId = 1, ProductName = "Test Game", Quantity = 1, UnitPrice = 1590.00m, Subtotal = 1590.00m }
                }
            };

            _mockStoreService
                .Setup(s => s.GetOrderAsync(123, 100))
                .ReturnsAsync(expectedOrder);

            // Act
            var result = await _controller.GetOrder(100);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as OrderDto;
            Assert.NotNull(data);
            Assert.Equal(100, data.OrderId);
            Assert.Single(data.Items);
        }

        [Fact]
        public async Task GetOrder_ShouldReturnNotFound_WhenOrderDoesNotExist()
        {
            // Arrange
            _mockStoreService
                .Setup(s => s.GetOrderAsync(123, 999))
                .ReturnsAsync((OrderDto?)null);

            // Act
            var result = await _controller.GetOrder(999);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = notFoundResult.Value;
            
            var responseType = response.GetType();
            var messageProperty = responseType.GetProperty("message");
            
            Assert.Equal("訂單不存在或無權限查看", messageProperty.GetValue(response));
        }

        [Fact]
        public async Task GetOrders_ShouldReturnOrderList_WhenValidRequest()
        {
            // Arrange
            var expectedOrders = new PagedResult<OrderDto>
            {
                Page = 1,
                PageSize = 20,
                TotalCount = 5,
                Data = new List<OrderDto>
                {
                    new() { OrderId = 100, UserId = 123, OrderStatus = "Completed", OrderTotal = 1590.00m },
                    new() { OrderId = 101, UserId = 123, OrderStatus = "Shipped", OrderTotal = 990.00m }
                }
            };

            _mockStoreService
                .Setup(s => s.GetOrdersAsync(123, It.IsAny<OrderQueryDto>()))
                .ReturnsAsync(expectedOrders);

            // Act
            var result = await _controller.GetOrders();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as PagedResult<OrderDto>;
            Assert.NotNull(data);
            Assert.Equal(5, data.TotalCount);
            Assert.Equal(2, data.Data.Count);
        }

        [Fact]
        public async Task PayOrder_ShouldReturnSuccess_WhenPaymentSuccessful()
        {
            // Arrange
            var expectedOrder = new OrderDto
            {
                OrderId = 100,
                UserId = 123,
                OrderStatus = "ToShip",
                PaymentStatus = "Paid",
                OrderTotal = 1590.00m,
                PaymentAt = DateTime.UtcNow
            };

            var expectedResult = ServiceResult<OrderDto>.CreateSuccess(expectedOrder, "付款成功");

            _mockStoreService
                .Setup(s => s.ProcessPaymentAsync(123, 100))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.PayOrder(100);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as OrderDto;
            Assert.NotNull(data);
            Assert.Equal("ToShip", data.OrderStatus);
            Assert.Equal("Paid", data.PaymentStatus);
        }

        #endregion

        #region 排行榜測試

        [Fact]
        public async Task GetRankings_ShouldReturnRankings_WhenValidRequest()
        {
            // Arrange
            var expectedRankings = new List<StoreRankingDto>
            {
                new() { RankingId = 1, PeriodType = "daily", RankingMetric = "trading_amount", RankingPosition = 1, ProductName = "熱門遊戲1", TradingAmount = 50000.00m },
                new() { RankingId = 2, PeriodType = "daily", RankingMetric = "trading_amount", RankingPosition = 2, ProductName = "熱門遊戲2", TradingAmount = 35000.00m }
            };

            _mockStoreService
                .Setup(s => s.GetRankingsAsync(It.Is<RankingQueryDto>(q => 
                    q.PeriodType == "daily" && 
                    q.RankingMetric == "trading_amount")))
                .ReturnsAsync(expectedRankings);

            // Act
            var result = await _controller.GetRankings("daily", "trading_amount", null, 50);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as List<StoreRankingDto>;
            Assert.NotNull(data);
            Assert.Equal(2, data.Count);
            Assert.Equal(1, data[0].RankingPosition);
            Assert.Equal(50000.00m, data[0].TradingAmount);
        }

        #endregion

        #region 統計測試

        [Fact]
        public async Task GetStatistics_ShouldReturnStatistics()
        {
            // Arrange
            var expectedStats = new StoreStatisticsDto
            {
                TotalProducts = 50,
                ProductsInStock = 45,
                TotalOrders = 100,
                CompletedOrders = 85,
                TotalRevenue = 150000.00m,
                TodayOrders = 5,
                TodayRevenue = 7500.00m,
                CategoryStats = new List<CategoryStatsDto>
                {
                    new() { CategoryName = "遊戲", ProductCount = 30, OrderCount = 60, Revenue = 95000.00m },
                    new() { CategoryName = "周邊", ProductCount = 15, OrderCount = 25, Revenue = 35000.00m }
                }
            };

            _mockStoreService
                .Setup(s => s.GetStatisticsAsync())
                .ReturnsAsync(expectedStats);

            // Act
            var result = await _controller.GetStatistics();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as StoreStatisticsDto;
            Assert.NotNull(data);
            Assert.Equal(50, data.TotalProducts);
            Assert.Equal(85.0, data.OrderCompletionRate);
            Assert.Equal(90.0, data.StockRate);
            Assert.Equal(2, data.CategoryStats.Count);
        }

        #endregion

        #region 錯誤處理測試

        [Fact]
        public async Task GetProducts_ShouldReturnServerError_WhenServiceThrows()
        {
            // Arrange
            _mockStoreService
                .Setup(s => s.SearchProductsAsync(It.IsAny<ProductSearchDto>()))
                .ThrowsAsync(new Exception("Database connection failed"));

            // Act
            var result = await _controller.GetProducts();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task AddToCart_ShouldReturnUnauthorized_WhenUserNotLoggedIn()
        {
            // Arrange
            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(); // 清空使用者

            var addToCartDto = new AddToCartDto { ProductId = 1, Quantity = 1 };

            // Act
            var result = await _controller.AddToCart(addToCartDto);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            var response = unauthorizedResult.Value;
            
            var responseType = response.GetType();
            var messageProperty = responseType.GetProperty("message");
            
            Assert.Equal("使用者未登入", messageProperty.GetValue(response));
        }

        #endregion

        #region 邊界條件測試

        [Fact]
        public async Task GetProducts_ShouldLimitPageSize_WhenLargePageSizeRequested()
        {
            // Arrange
            _mockStoreService
                .Setup(s => s.SearchProductsAsync(It.Is<ProductSearchDto>(dto => dto.PageSize == 100)))
                .ReturnsAsync(new PagedResult<ProductListDto>());

            // Act
            await _controller.GetProducts(pageSize: 200); // 超過限制

            // Assert
            _mockStoreService.Verify(s => s.SearchProductsAsync(It.Is<ProductSearchDto>(dto => dto.PageSize == 100)), Times.Once);
        }

        [Fact]
        public async Task GetPopularProducts_ShouldLimitResults_WhenLargeLimitRequested()
        {
            // Arrange
            _mockStoreService
                .Setup(s => s.GetPopularProductsAsync(50))
                .ReturnsAsync(new List<ProductListDto>());

            // Act
            await _controller.GetPopularProducts(100); // 超過限制

            // Assert
            _mockStoreService.Verify(s => s.GetPopularProductsAsync(50), Times.Once);
        }

        [Fact]
        public async Task GetRankings_ShouldLimitResults_WhenLargeLimitRequested()
        {
            // Arrange
            _mockStoreService
                .Setup(s => s.GetRankingsAsync(It.Is<RankingQueryDto>(q => q.Limit == 100)))
                .ReturnsAsync(new List<StoreRankingDto>());

            // Act
            await _controller.GetRankings(limit: 200); // 超過限制

            // Assert
            _mockStoreService.Verify(s => s.GetRankingsAsync(It.Is<RankingQueryDto>(q => q.Limit == 100)), Times.Once);
        }

        #endregion

        #region 管理員功能測試

        [Fact]
        public async Task UpdateOrderStatus_ShouldReturnSuccess_WhenValidRequest()
        {
            // Arrange
            var statusDto = new UpdateOrderStatusDto 
            { 
                OrderStatus = "Shipped", 
                PaymentStatus = "Paid",
                Notes = "已安排出貨"
            };

            var expectedOrder = new OrderDto
            {
                OrderId = 100,
                OrderStatus = "Shipped",
                PaymentStatus = "Paid"
            };

            var expectedResult = ServiceResult<OrderDto>.CreateSuccess(expectedOrder, "訂單狀態更新成功");

            _mockStoreService
                .Setup(s => s.UpdateOrderStatusAsync(100, statusDto))
                .ReturnsAsync(expectedResult);

            // 設定管理員身份
            var adminClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Role, "Admin")
            };
            var adminIdentity = new ClaimsIdentity(adminClaims, "Test");
            var adminPrincipal = new ClaimsPrincipal(adminIdentity);

            _controller.ControllerContext.HttpContext.User = adminPrincipal;

            // Act
            var result = await _controller.UpdateOrderStatus(100, statusDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            
            Assert.True((bool)successProperty.GetValue(response));
            _mockStoreService.Verify(s => s.UpdateOrderStatusAsync(100, statusDto), Times.Once);
        }

        #endregion
    }
}