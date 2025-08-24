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
    /// 自由市場控制器測試類別
    /// 測試所有C2C交易相關API的功能和邊界條件
    /// 驗證商品上架、交易頁面、即時訊息、排行榜查詢等完整C2C功能
    /// </summary>
    public class PlayerMarketControllerTests
    {
        private readonly Mock<IPlayerMarketService> _mockPlayerMarketService;
        private readonly Mock<ILogger<PlayerMarketController>> _mockLogger;
        private readonly PlayerMarketController _controller;

        public PlayerMarketControllerTests()
        {
            _mockPlayerMarketService = new Mock<IPlayerMarketService>();
            _mockLogger = new Mock<ILogger<PlayerMarketController>>();
            _controller = new PlayerMarketController(_mockPlayerMarketService.Object, _mockLogger.Object);

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
            var expectedProducts = new PlayerMarketPagedResult<PlayerMarketProductListDto>
            {
                Page = 1,
                PageSize = 20,
                TotalCount = 50,
                Data = new List<PlayerMarketProductListDto>
                {
                    new() { PProductId = 1, PProductName = "史詩級武器", PProductType = "遊戲道具", Price = 150.00m, PStatus = "active", SellerName = "賣家A" },
                    new() { PProductId = 2, PProductName = "稀有裝備", PProductType = "遊戲道具", Price = 200.00m, PStatus = "active", SellerName = "賣家B" }
                }
            };

            _mockPlayerMarketService
                .Setup(s => s.SearchProductsAsync(It.IsAny<PlayerMarketSearchDto>()))
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
            Assert.Equal("市場商品列表取得成功", messageProperty.GetValue(response));
        }

        [Fact]
        public async Task GetProduct_ShouldReturnProductDetail_WhenProductExists()
        {
            // Arrange
            var expectedProduct = new PlayerMarketProductDetailDto
            {
                PProductId = 1,
                PProductName = "史詩級武器",
                PProductType = "遊戲道具",
                Price = 150.00m,
                PStatus = "active",
                PProductDescription = "精心培養的高品質道具",
                Seller = new MarketSellerDto { SellerId = 1, SellerName = "測試賣家", HasSalesAuthority = true },
                Images = new List<ProductImageDto>
                {
                    new() { PProductImgId = 1, IsMain = true, SortOrder = 1 }
                }
            };

            _mockPlayerMarketService
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
            
            var data = dataProperty.GetValue(response) as PlayerMarketProductDetailDto;
            Assert.NotNull(data);
            Assert.Equal("史詩級武器", data.PProductName);
            Assert.Equal(150.00m, data.Price);
        }

        [Fact]
        public async Task GetProduct_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            _mockPlayerMarketService
                .Setup(s => s.GetProductDetailAsync(999))
                .ReturnsAsync((PlayerMarketProductDetailDto?)null);

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
        public async Task CreateProduct_ShouldReturnSuccess_WhenValidRequest()
        {
            // Arrange
            var createDto = new CreatePlayerMarketProductDto
            {
                PProductType = "遊戲道具",
                PProductTitle = "【超值包】史詩級武器 限時優惠！",
                PProductName = "史詩級武器 +15",
                PProductDescription = "精心培養的高品質道具，屬性完美",
                Price = 150.00m,
                Images = new List<string> { "base64imagedata" }
            };

            var expectedProduct = new PlayerMarketProductDetailDto
            {
                PProductId = 100,
                PProductName = "史詩級武器 +15",
                Price = 150.00m,
                PStatus = "active"
            };

            var expectedResult = PlayerMarketServiceResult<PlayerMarketProductDetailDto>.CreateSuccess(
                expectedProduct, "商品上架成功");

            _mockPlayerMarketService
                .Setup(s => s.CreateProductAsync(123, createDto))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.CreateProduct(createDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var dataProperty = responseType.GetProperty("data");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.True((bool)successProperty.GetValue(response));
            Assert.NotNull(dataProperty.GetValue(response));
            Assert.Equal("商品上架成功", messageProperty.GetValue(response));
        }

        [Fact]
        public async Task CreateProduct_ShouldReturnBadRequest_WhenServiceFails()
        {
            // Arrange
            var createDto = new CreatePlayerMarketProductDto
            {
                PProductType = "遊戲道具",
                PProductName = "測試商品",
                Price = 150.00m
            };

            var expectedResult = PlayerMarketServiceResult<PlayerMarketProductDetailDto>.CreateFailure(
                "您沒有銷售權限", new List<string> { "請先申請銷售權限" });

            _mockPlayerMarketService
                .Setup(s => s.CreateProductAsync(123, createDto))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.CreateProduct(createDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = badRequestResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.False((bool)successProperty.GetValue(response));
            Assert.Equal("您沒有銷售權限", messageProperty.GetValue(response));
        }

        [Fact]
        public async Task UpdateProduct_ShouldReturnSuccess_WhenValidRequest()
        {
            // Arrange
            var updateDto = new UpdatePlayerMarketProductDto
            {
                PProductTitle = "【降價促銷】史詩級武器 +15",
                Price = 120.00m,
                PStatus = "active"
            };

            var expectedProduct = new PlayerMarketProductDetailDto
            {
                PProductId = 1,
                PProductName = "史詩級武器 +15",
                Price = 120.00m
            };

            var expectedResult = PlayerMarketServiceResult<PlayerMarketProductDetailDto>.CreateSuccess(
                expectedProduct, "商品更新成功");

            _mockPlayerMarketService
                .Setup(s => s.UpdateProductAsync(123, 1, updateDto))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.UpdateProduct(1, updateDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            _mockPlayerMarketService.Verify(s => s.UpdateProductAsync(123, 1, updateDto), Times.Once);
        }

        [Fact]
        public async Task UploadProductImages_ShouldReturnSuccess_WhenValidImages()
        {
            // Arrange
            var images = new List<string> { "base64image1", "base64image2" };
            var expectedImages = new List<ProductImageDto>
            {
                new() { PProductImgId = 1, IsMain = true, SortOrder = 1 },
                new() { PProductImgId = 2, IsMain = false, SortOrder = 2 }
            };

            var expectedResult = PlayerMarketServiceResult<List<ProductImageDto>>.CreateSuccess(
                expectedImages, "圖片上傳成功");

            _mockPlayerMarketService
                .Setup(s => s.UploadProductImagesAsync(123, 1, images))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.UploadProductImages(1, images);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            _mockPlayerMarketService.Verify(s => s.UploadProductImagesAsync(123, 1, images), Times.Once);
        }

        [Fact]
        public async Task GetMyProducts_ShouldReturnUserProducts()
        {
            // Arrange
            var expectedProducts = new PlayerMarketPagedResult<PlayerMarketProductListDto>
            {
                Page = 1,
                PageSize = 20,
                TotalCount = 3,
                Data = new List<PlayerMarketProductListDto>
                {
                    new() { PProductId = 1, PProductName = "我的商品1", SellerId = 123 },
                    new() { PProductId = 2, PProductName = "我的商品2", SellerId = 123 }
                }
            };

            _mockPlayerMarketService
                .Setup(s => s.SearchProductsAsync(It.Is<PlayerMarketSearchDto>(dto => dto.SellerId == 123)))
                .ReturnsAsync(expectedProducts);

            // Act
            var result = await _controller.GetMyProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as PlayerMarketPagedResult<PlayerMarketProductListDto>;
            Assert.NotNull(data);
            Assert.Equal(3, data.TotalCount);
        }

        #endregion

        #region 訂單管理測試

        [Fact]
        public async Task CreateOrder_ShouldReturnSuccess_WhenValidRequest()
        {
            // Arrange
            var createOrderDto = new CreatePlayerMarketOrderDto
            {
                PProductId = 1,
                PQuantity = 1,
                Notes = "請盡快交易"
            };

            var expectedOrder = new PlayerMarketOrderDto
            {
                POrderId = 100,
                PProductId = 1,
                BuyerId = 123,
                POrderStatus = "Created",
                PPaymentStatus = "Pending",
                POrderTotal = 150.00m
            };

            var expectedResult = PlayerMarketServiceResult<PlayerMarketOrderDto>.CreateSuccess(
                expectedOrder, "訂單建立成功");

            _mockPlayerMarketService
                .Setup(s => s.CreateOrderAsync(123, createOrderDto))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.CreateOrder(createOrderDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as PlayerMarketOrderDto;
            Assert.NotNull(data);
            Assert.Equal(100, data.POrderId);
            Assert.Equal(150.00m, data.POrderTotal);
        }

        [Fact]
        public async Task GetOrder_ShouldReturnOrder_WhenOrderExists()
        {
            // Arrange
            var expectedOrder = new PlayerMarketOrderDto
            {
                POrderId = 100,
                PProductId = 1,
                BuyerId = 123,
                SellerId = 456,
                POrderStatus = "Trading",
                POrderTotal = 150.00m
            };

            _mockPlayerMarketService
                .Setup(s => s.GetOrderAsync(123, 100))
                .ReturnsAsync(expectedOrder);

            // Act
            var result = await _controller.GetOrder(100);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as PlayerMarketOrderDto;
            Assert.NotNull(data);
            Assert.Equal(100, data.POrderId);
            Assert.Equal("Trading", data.POrderStatus);
        }

        [Fact]
        public async Task GetOrders_ShouldReturnOrderList_WhenValidRequest()
        {
            // Arrange
            var expectedOrders = new PlayerMarketPagedResult<PlayerMarketOrderDto>
            {
                Page = 1,
                PageSize = 20,
                TotalCount = 5,
                Data = new List<PlayerMarketOrderDto>
                {
                    new() { POrderId = 100, BuyerId = 123, POrderStatus = "Completed" },
                    new() { POrderId = 101, SellerId = 123, POrderStatus = "Trading" }
                }
            };

            _mockPlayerMarketService
                .Setup(s => s.GetUserOrdersAsync(123, "buyer", null, 1, 20))
                .ReturnsAsync(expectedOrders);

            // Act
            var result = await _controller.GetOrders("buyer");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as PlayerMarketPagedResult<PlayerMarketOrderDto>;
            Assert.NotNull(data);
            Assert.Equal(5, data.TotalCount);
            Assert.Equal(2, data.Data.Count);
        }

        #endregion

        #region 交易頁面測試

        [Fact]
        public async Task CreateTradepage_ShouldReturnSuccess_WhenValidRequest()
        {
            // Arrange
            var expectedTradepage = new PlayerMarketTradepageDto
            {
                POrderTradepageId = 200,
                POrderId = 100,
                PProductId = 1,
                POrderPlatformFee = 7.50m,
                TradeStatus = "交易中"
            };

            var expectedResult = PlayerMarketServiceResult<PlayerMarketTradepageDto>.CreateSuccess(
                expectedTradepage, "交易頁面建立成功");

            _mockPlayerMarketService
                .Setup(s => s.CreateTradepageAsync(123, 100))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.CreateTradepage(100);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as PlayerMarketTradepageDto;
            Assert.NotNull(data);
            Assert.Equal(200, data.POrderTradepageId);
            Assert.Equal(7.50m, data.POrderPlatformFee);
        }

        [Fact]
        public async Task GetTradepage_ShouldReturnTradepage_WhenExists()
        {
            // Arrange
            var expectedTradepage = new PlayerMarketTradepageDto
            {
                POrderTradepageId = 200,
                POrderId = 100,
                Messages = new List<TradeMessageDto>
                {
                    new() { TradeMsgId = 1, MsgFrom = "seller", MessageText = "商品已準備好" },
                    new() { TradeMsgId = 2, MsgFrom = "buyer", MessageText = "我已經在線上了" }
                }
            };

            _mockPlayerMarketService
                .Setup(s => s.GetTradepageAsync(123, 200))
                .ReturnsAsync(expectedTradepage);

            // Act
            var result = await _controller.GetTradepage(200);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as PlayerMarketTradepageDto;
            Assert.NotNull(data);
            Assert.Equal(200, data.POrderTradepageId);
            Assert.Equal(2, data.Messages.Count);
        }

        [Fact]
        public async Task SendTradeMessage_ShouldReturnSuccess_WhenValidMessage()
        {
            // Arrange
            var messageDto = new SendTradeMessageDto
            {
                MessageText = "請問什麼時候可以交易呢？"
            };

            var expectedMessage = new TradeMessageDto
            {
                TradeMsgId = 10,
                POrderTradepageId = 200,
                MsgFrom = "buyer",
                MessageText = "請問什麼時候可以交易呢？",
                CreatedAt = DateTime.UtcNow
            };

            var expectedResult = PlayerMarketServiceResult<TradeMessageDto>.CreateSuccess(
                expectedMessage, "訊息發送成功");

            _mockPlayerMarketService
                .Setup(s => s.SendTradeMessageAsync(123, 200, messageDto))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.SendTradeMessage(200, messageDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as TradeMessageDto;
            Assert.NotNull(data);
            Assert.Equal(10, data.TradeMsgId);
            Assert.Equal("buyer", data.MsgFrom);
        }

        [Fact]
        public async Task ConfirmSellerTransfer_ShouldReturnSuccess_WhenValidTransfer()
        {
            // Arrange
            var confirmDto = new ConfirmTransferDto
            {
                Notes = "道具已移交完成"
            };

            var expectedTradepage = new PlayerMarketTradepageDto
            {
                POrderTradepageId = 200,
                SellerTransferredAt = DateTime.UtcNow,
                TradeStatus = "等待買家確認"
            };

            var expectedResult = PlayerMarketServiceResult<PlayerMarketTradepageDto>.CreateSuccess(
                expectedTradepage, "賣家移交確認成功");

            _mockPlayerMarketService
                .Setup(s => s.ConfirmSellerTransferAsync(123, 200, confirmDto))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.ConfirmSellerTransfer(200, confirmDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as PlayerMarketTradepageDto;
            Assert.NotNull(data);
            Assert.Equal(200, data.POrderTradepageId);
            Assert.NotNull(data.SellerTransferredAt);
        }

        [Fact]
        public async Task ConfirmBuyerReceived_ShouldReturnSuccess_WhenValidReceive()
        {
            // Arrange
            var confirmDto = new ConfirmTransferDto
            {
                Notes = "商品已收到，謝謝！"
            };

            var expectedTradepage = new PlayerMarketTradepageDto
            {
                POrderTradepageId = 200,
                SellerTransferredAt = DateTime.UtcNow.AddHours(-1),
                BuyerReceivedAt = DateTime.UtcNow,
                CompletedAt = DateTime.UtcNow,
                TradeStatus = "已完成"
            };

            var expectedResult = PlayerMarketServiceResult<PlayerMarketTradepageDto>.CreateSuccess(
                expectedTradepage, "買家接收確認成功，交易完成");

            _mockPlayerMarketService
                .Setup(s => s.ConfirmBuyerReceivedAsync(123, 200, confirmDto))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.ConfirmBuyerReceived(200, confirmDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as PlayerMarketTradepageDto;
            Assert.NotNull(data);
            Assert.Equal(200, data.POrderTradepageId);
            Assert.NotNull(data.CompletedAt);
            Assert.Equal("已完成", data.TradeStatus);
        }

        #endregion

        #region 排行榜測試

        [Fact]
        public async Task GetRankings_ShouldReturnRankings_WhenValidRequest()
        {
            // Arrange
            var expectedRankings = new List<PlayerMarketRankingDto>
            {
                new() { PRankingId = 1, PPeriodType = "daily", PRankingMetric = "trading_amount", PRankingPosition = 1, ProductName = "熱門商品1", PTradingAmount = 5000.00m },
                new() { PRankingId = 2, PPeriodType = "daily", PRankingMetric = "trading_amount", PRankingPosition = 2, ProductName = "熱門商品2", PTradingAmount = 3500.00m }
            };

            _mockPlayerMarketService
                .Setup(s => s.GetRankingsAsync(It.Is<PlayerMarketRankingQueryDto>(q => 
                    q.PPeriodType == "daily" && 
                    q.PRankingMetric == "trading_amount")))
                .ReturnsAsync(expectedRankings);

            // Act
            var result = await _controller.GetRankings("daily", "trading_amount", null, 50);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as List<PlayerMarketRankingDto>;
            Assert.NotNull(data);
            Assert.Equal(2, data.Count);
            Assert.Equal(1, data[0].PRankingPosition);
            Assert.Equal(5000.00m, data[0].PTradingAmount);
        }

        #endregion

        #region 統計測試

        [Fact]
        public async Task GetStatistics_ShouldReturnStatistics()
        {
            // Arrange
            var expectedStats = new PlayerMarketStatisticsDto
            {
                TotalProducts = 200,
                ActiveProducts = 150,
                TotalOrders = 80,
                CompletedOrders = 60,
                TotalTradingAmount = 25000.00m,
                TotalPlatformFees = 1250.00m,
                ActiveSellers = 25,
                ActiveBuyers = 35,
                TodayOrders = 5,
                TodayTradingAmount = 750.00m,
                CategoryStats = new List<MarketCategoryStatsDto>
                {
                    new() { CategoryName = "遊戲道具", ProductCount = 80, OrderCount = 35, TradingAmount = 12000.00m },
                    new() { CategoryName = "遊戲帳號", ProductCount = 50, OrderCount = 20, TradingAmount = 8000.00m }
                }
            };

            _mockPlayerMarketService
                .Setup(s => s.GetStatisticsAsync())
                .ReturnsAsync(expectedStats);

            // Act
            var result = await _controller.GetStatistics();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as PlayerMarketStatisticsDto;
            Assert.NotNull(data);
            Assert.Equal(200, data.TotalProducts);
            Assert.Equal(75.0, data.OrderCompletionRate); // 60/80 * 100
            Assert.Equal(75.0, data.ProductActiveRate); // 150/200 * 100
            Assert.Equal(2, data.CategoryStats.Count);
        }

        #endregion

        #region 搜尋測試

        [Fact]
        public async Task SearchProducts_ShouldReturnSearchResults_WhenValidSearchDto()
        {
            // Arrange
            var searchDto = new PlayerMarketSearchDto
            {
                Keyword = "史詩",
                PProductType = "遊戲道具",
                MinPrice = 100,
                MaxPrice = 300,
                PStatus = "active",
                Page = 1,
                PageSize = 10
            };

            var expectedResults = new PlayerMarketPagedResult<PlayerMarketProductListDto>
            {
                Page = 1,
                PageSize = 10,
                TotalCount = 15,
                Data = new List<PlayerMarketProductListDto>
                {
                    new() { PProductId = 1, PProductName = "史詩級武器", PProductType = "遊戲道具", Price = 150.00m },
                    new() { PProductId = 2, PProductName = "史詩防具", PProductType = "遊戲道具", Price = 200.00m }
                }
            };

            _mockPlayerMarketService
                .Setup(s => s.SearchProductsAsync(It.Is<PlayerMarketSearchDto>(dto => 
                    dto.Keyword == "史詩" && 
                    dto.PProductType == "遊戲道具" && 
                    dto.MinPrice == 100)))
                .ReturnsAsync(expectedResults);

            // Act
            var result = await _controller.SearchProducts(searchDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var dataProperty = responseType.GetProperty("data");
            
            var data = dataProperty.GetValue(response) as PlayerMarketPagedResult<PlayerMarketProductListDto>;
            Assert.NotNull(data);
            Assert.Equal(15, data.TotalCount);
            Assert.Equal(2, data.Data.Count);
        }

        #endregion

        #region 錯誤處理測試

        [Fact]
        public async Task GetProducts_ShouldReturnServerError_WhenServiceThrows()
        {
            // Arrange
            _mockPlayerMarketService
                .Setup(s => s.SearchProductsAsync(It.IsAny<PlayerMarketSearchDto>()))
                .ThrowsAsync(new Exception("Database connection failed"));

            // Act
            var result = await _controller.GetProducts();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task CreateProduct_ShouldReturnUnauthorized_WhenUserNotLoggedIn()
        {
            // Arrange
            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(); // 清空使用者

            var createDto = new CreatePlayerMarketProductDto
            {
                PProductName = "測試商品",
                Price = 100.00m
            };

            // Act
            var result = await _controller.CreateProduct(createDto);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            var response = unauthorizedResult.Value;
            
            var responseType = response.GetType();
            var messageProperty = responseType.GetProperty("message");
            
            Assert.Equal("使用者未登入", messageProperty.GetValue(response));
        }

        [Fact]
        public async Task CreateProduct_ShouldReturnBadRequest_WhenInvalidModel()
        {
            // Arrange
            var createDto = new CreatePlayerMarketProductDto(); // 空的DTO，缺少必填欄位

            _controller.ModelState.AddModelError("PProductName", "商品名稱為必填");

            // Act
            var result = await _controller.CreateProduct(createDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = badRequestResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            var messageProperty = responseType.GetProperty("message");
            
            Assert.False((bool)successProperty.GetValue(response));
            Assert.Equal("商品資料格式錯誤", messageProperty.GetValue(response));
        }

        [Fact]
        public async Task UploadProductImages_ShouldReturnBadRequest_WhenNoImages()
        {
            // Arrange
            var emptyImages = new List<string>();

            // Act
            var result = await _controller.UploadProductImages(1, emptyImages);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = badRequestResult.Value;
            
            var responseType = response.GetType();
            var messageProperty = responseType.GetProperty("message");
            
            Assert.Equal("請選擇要上傳的圖片", messageProperty.GetValue(response));
        }

        #endregion

        #region 邊界條件測試

        [Fact]
        public async Task GetProducts_ShouldLimitPageSize_WhenLargePageSizeRequested()
        {
            // Arrange
            _mockPlayerMarketService
                .Setup(s => s.SearchProductsAsync(It.Is<PlayerMarketSearchDto>(dto => dto.PageSize == 100)))
                .ReturnsAsync(new PlayerMarketPagedResult<PlayerMarketProductListDto>());

            // Act
            await _controller.GetProducts(pageSize: 200); // 超過限制

            // Assert
            _mockPlayerMarketService.Verify(s => s.SearchProductsAsync(It.Is<PlayerMarketSearchDto>(dto => dto.PageSize == 100)), Times.Once);
        }

        [Fact]
        public async Task GetRankings_ShouldLimitResults_WhenLargeLimitRequested()
        {
            // Arrange
            _mockPlayerMarketService
                .Setup(s => s.GetRankingsAsync(It.Is<PlayerMarketRankingQueryDto>(q => q.Limit == 100)))
                .ReturnsAsync(new List<PlayerMarketRankingDto>());

            // Act
            await _controller.GetRankings(limit: 200); // 超過限制

            // Assert
            _mockPlayerMarketService.Verify(s => s.GetRankingsAsync(It.Is<PlayerMarketRankingQueryDto>(q => q.Limit == 100)), Times.Once);
        }

        #endregion

        #region 管理員功能測試

        [Fact]
        public async Task UpdateProductStatus_ShouldReturnSuccess_WhenValidRequest()
        {
            // Arrange
            var statusDto = new UpdateProductStatusDto 
            { 
                PStatus = "suspended", 
                Reason = "違反平台規則",
                Notes = "商品描述不實"
            };

            var expectedProduct = new PlayerMarketProductDetailDto
            {
                PProductId = 1,
                PStatus = "suspended"
            };

            var expectedResult = PlayerMarketServiceResult<PlayerMarketProductDetailDto>.CreateSuccess(
                expectedProduct, "商品狀態更新成功");

            _mockPlayerMarketService
                .Setup(s => s.UpdateProductStatusAsync(1, statusDto))
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
            var result = await _controller.UpdateProductStatus(1, statusDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;
            
            var responseType = response.GetType();
            var successProperty = responseType.GetProperty("success");
            
            Assert.True((bool)successProperty.GetValue(response));
            _mockPlayerMarketService.Verify(s => s.UpdateProductStatusAsync(1, statusDto), Times.Once);
        }

        #endregion
    }
}