using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using GameCore.Web.Controllers;
using GameCore.Core.Services;
using GameCore.Core.Entities;
using Xunit;

namespace GameCore.Tests.Controllers
{
    /// <summary>
    /// 商城控制器測試
    /// </summary>
    public class StoreControllerTests
    {
        private readonly Mock<IProductService> _mockProductService;
        private readonly Mock<ILogger<StoreController>> _mockLogger;
        private readonly StoreController _controller;

        public StoreControllerTests()
        {
            _mockProductService = new Mock<IProductService>();
            _mockLogger = new Mock<ILogger<StoreController>>();
            _controller = new StoreController(_mockProductService.Object, _mockLogger.Object);
        }

        /// <summary>
        /// 測試取得商品列表成功
        /// </summary>
        [Fact]
        public async Task GetProducts_ShouldReturnOkResult_WhenServiceReturnsData()
        {
            // Arrange
            var mockProducts = new GameCore.Core.Services.PagedResult<GameCore.Core.Entities.Product>
            {
                Items = new List<GameCore.Core.Entities.Product>
                {
                    new GameCore.Core.Entities.Product
                    {
                        ProductId = 1,
                        ProductName = "測試商品",
                        Price = 100,
                        ProductType = "game"
                    }
                },
                TotalCount = 1,
                Page = 1,
                PageSize = 20
            };

            _mockProductService.Setup(x => x.GetProductsAsync(It.IsAny<int?>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(mockProducts);

            // Act
            var result = await _controller.GetProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<dynamic>(okResult.Value);
            Assert.True(response.success);
            Assert.Equal("取得商品列表成功", response.message.ToString());
        }

        /// <summary>
        /// 測試取得商品列表失敗
        /// </summary>
        [Fact]
        public async Task GetProducts_ShouldReturnInternalServerError_WhenServiceThrowsException()
        {
            // Arrange
            _mockProductService.Setup(x => x.GetProductsAsync(It.IsAny<int?>(), It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception("測試異常"));

            // Act
            var result = await _controller.GetProducts();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        /// <summary>
        /// 測試取得商品詳情成功
        /// </summary>
        [Fact]
        public async Task GetProduct_ShouldReturnOkResult_WhenProductExists()
        {
            // Arrange
            var productId = 1;
            var mockProduct = new GameCore.Core.Entities.Product
            {
                ProductId = productId,
                ProductName = "測試商品",
                Price = 100,
                ProductType = "game"
            };

            _mockProductService.Setup(x => x.GetProductByIdAsync(productId))
                .ReturnsAsync(mockProduct);

            // Act
            var result = await _controller.GetProduct(productId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<dynamic>(okResult.Value);
            Assert.True(response.success);
            Assert.Equal("取得商品詳情成功", response.message.ToString());
        }

        /// <summary>
        /// 測試取得商品詳情失敗 - 商品不存在
        /// </summary>
        [Fact]
        public async Task GetProduct_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = 1;
            _mockProductService.Setup(x => x.GetProductByIdAsync(productId))
                .ReturnsAsync((GameCore.Core.Entities.Product?)null);

            // Act
            var result = await _controller.GetProduct(productId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<dynamic>(notFoundResult.Value);
            Assert.False(response.success);
            Assert.Equal("商品不存在", response.message.ToString());
        }

        /// <summary>
        /// 測試搜尋商品成功
        /// </summary>
        [Fact]
        public async Task SearchProducts_ShouldReturnOkResult_WhenServiceReturnsData()
        {
            // Arrange
            var keyword = "測試";
            var mockProducts = new GameCore.Core.Services.PagedResult<GameCore.Core.Entities.Product>
            {
                Items = new List<GameCore.Core.Entities.Product>(),
                TotalCount = 0,
                Page = 1,
                PageSize = 20
            };

            _mockProductService.Setup(x => x.SearchProductsAsync(
                It.IsAny<string?>(), 
                It.IsAny<int?>(), 
                It.IsAny<decimal?>(), 
                It.IsAny<decimal?>(), 
                It.IsAny<int>(), 
                It.IsAny<int>()))
                .ReturnsAsync(mockProducts);

            // Act
            var result = await _controller.SearchProducts(keyword);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<dynamic>(okResult.Value);
            Assert.True(response.success);
            Assert.Equal("搜尋商品成功", response.message.ToString());
        }

        /// <summary>
        /// 測試取得熱門商品成功
        /// </summary>
        [Fact]
        public async Task GetPopularProducts_ShouldReturnOkResult_WhenServiceReturnsData()
        {
            // Arrange
            var limit = 5;
            var mockProducts = new List<GameCore.Core.Entities.Product>
            {
                new GameCore.Core.Entities.Product
                {
                    ProductId = 1,
                    ProductName = "熱門商品1",
                    Price = 100
                },
                new GameCore.Core.Entities.Product
                {
                    ProductId = 2,
                    ProductName = "熱門商品2",
                    Price = 200
                }
            };

            _mockProductService.Setup(x => x.GetPopularProductsAsync(limit))
                .ReturnsAsync(mockProducts);

            // Act
            var result = await _controller.GetPopularProducts(limit);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<dynamic>(okResult.Value);
            Assert.True(response.success);
            Assert.Equal("取得熱門商品成功", response.message.ToString());
        }

        /// <summary>
        /// 測試檢查庫存成功
        /// </summary>
        [Fact]
        public async Task CheckStock_ShouldReturnOkResult_WhenServiceReturnsTrue()
        {
            // Arrange
            var productId = 1;
            var quantity = 5;
            _mockProductService.Setup(x => x.CheckStockAsync(productId, quantity))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.CheckStock(productId, quantity);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<dynamic>(okResult.Value);
            Assert.True(response.success);
            Assert.Equal("庫存充足", response.message.ToString());
        }

        /// <summary>
        /// 測試檢查庫存失敗
        /// </summary>
        [Fact]
        public async Task CheckStock_ShouldReturnOkResult_WhenServiceReturnsFalse()
        {
            // Arrange
            var productId = 1;
            var quantity = 100;
            _mockProductService.Setup(x => x.CheckStockAsync(productId, quantity))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.CheckStock(productId, quantity);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<dynamic>(okResult.Value);
            Assert.True(response.success);
            Assert.Equal("庫存不足", response.message.ToString());
        }

        /// <summary>
        /// 測試取得商品統計成功
        /// </summary>
        [Fact]
        public async Task GetProductStatistics_ShouldReturnOkResult_WhenServiceReturnsData()
        {
            // Arrange
            var productId = 1;
            var mockStatistics = new GameCore.Core.Services.ProductStatistics
            {
                ProductId = productId,
                TotalSales = 100,
                TotalRevenue = 10000,
                AverageRating = 4.5,
                RatingCount = 50,
                ViewCount = 1000,
                FavoriteCount = 200,
                CurrentStock = 50
            };

            _mockProductService.Setup(x => x.GetProductStatisticsAsync(productId))
                .ReturnsAsync(mockStatistics);

            // Act
            var result = await _controller.GetProductStatistics(productId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<dynamic>(okResult.Value);
            Assert.True(response.success);
            Assert.Equal("取得商品統計成功", response.message.ToString());
        }
    }
} 