using Microsoft.Extensions.Logging;
using Moq;
using GameCore.Core.Services;
using GameCore.Core.Repositories;
using GameCore.Core.Entities;
using GameCore.Core.DTOs;
using Xunit;

namespace GameCore.Tests.Services
{
    /// <summary>
    /// 商城服務測試
    /// 測試商品管理、購物車、訂單等核心功能
    /// </summary>
    public class StoreServiceTests
    {
        private readonly Mock<IStoreRepository> _mockStoreRepository;
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly Mock<ICartRepository> _mockCartRepository;
        private readonly Mock<ILogger<StoreService>> _mockLogger;
        private readonly StoreService _storeService;

        public StoreServiceTests()
        {
            _mockStoreRepository = new Mock<IStoreRepository>();
            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockCartRepository = new Mock<ICartRepository>();
            _mockLogger = new Mock<ILogger<StoreService>>();
            
            _storeService = new StoreService(
                _mockStoreRepository.Object,
                _mockOrderRepository.Object,
                _mockCartRepository.Object,
                _mockLogger.Object
            );
        }

        #region 商品相關測試

        [Fact]
        public async Task GetProductsAsync_ShouldReturnProducts_WhenRepositoryReturnsData()
        {
            // Arrange
            var expectedProducts = new List<StoreProduct>
            {
                new StoreProduct
                {
                    ProductId = 1,
                    Name = "測試商品1",
                    Price = 1000m,
                    Category = "測試分類",
                    IsActive = true
                },
                new StoreProduct
                {
                    ProductId = 2,
                    Name = "測試商品2",
                    Price = 2000m,
                    Category = "測試分類",
                    IsActive = true
                }
            };

            _mockStoreRepository
                .Setup(x => x.GetProductsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal?>(), It.IsAny<decimal?>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(expectedProducts);

            // Act
            var result = await _storeService.GetProductsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedProducts.Count, result.Count());
            Assert.Equal(expectedProducts[0].Name, result.First().Name);
        }

        [Fact]
        public async Task GetProductByIdAsync_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            var productId = 1;
            var expectedProduct = new StoreProduct
            {
                ProductId = productId,
                Name = "測試商品",
                Price = 1000m,
                Category = "測試分類",
                IsActive = true
            };

            _mockStoreRepository
                .Setup(x => x.GetProductByIdAsync(productId))
                .ReturnsAsync(expectedProduct);

            // Act
            var result = await _storeService.GetProductByIdAsync(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedProduct.ProductId, result.ProductId);
            Assert.Equal(expectedProduct.Name, result.Name);
        }

        [Fact]
        public async Task GetProductByIdAsync_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = 999;
            _mockStoreRepository
                .Setup(x => x.GetProductByIdAsync(productId))
                .ReturnsAsync((StoreProduct)null);

            // Act
            var result = await _storeService.GetProductByIdAsync(productId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetProductsByCategoryAsync_ShouldReturnProducts_WhenCategoryExists()
        {
            // Arrange
            var category = "測試分類";
            var expectedProducts = new List<StoreProduct>
            {
                new StoreProduct
                {
                    ProductId = 1,
                    Name = "測試商品1",
                    Category = category,
                    IsActive = true
                }
            };

            _mockStoreRepository
                .Setup(x => x.GetProductsByCategoryAsync(category, It.IsAny<int>()))
                .ReturnsAsync(expectedProducts);

            // Act
            var result = await _storeService.GetProductsByCategoryAsync(category);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedProducts.Count, result.Count());
            Assert.All(result, p => Assert.Equal(category, p.Category));
        }

        [Fact]
        public async Task GetRelatedProductsAsync_ShouldReturnRelatedProducts_WhenProductExists()
        {
            // Arrange
            var productId = 1;
            var category = "測試分類";
            var product = new StoreProduct
            {
                ProductId = productId,
                Category = category
            };

            var relatedProducts = new List<StoreProduct>
            {
                new StoreProduct { ProductId = 2, Category = category },
                new StoreProduct { ProductId = 3, Category = category },
                new StoreProduct { ProductId = 4, Category = category }
            };

            _mockStoreRepository
                .Setup(x => x.GetProductByIdAsync(productId))
                .ReturnsAsync(product);

            _mockStoreRepository
                .Setup(x => x.GetProductsByCategoryAsync(category, It.IsAny<int>()))
                .ReturnsAsync(relatedProducts);

            // Act
            var result = await _storeService.GetRelatedProductsAsync(productId, 3);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
            Assert.All(result, p => Assert.NotEqual(productId, p.ProductId));
            Assert.All(result, p => Assert.Equal(category, p.Category));
        }

        [Fact]
        public async Task GetRelatedProductsAsync_ShouldReturnEmpty_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = 999;
            _mockStoreRepository
                .Setup(x => x.GetProductByIdAsync(productId))
                .ReturnsAsync((StoreProduct)null);

            // Act
            var result = await _storeService.GetRelatedProductsAsync(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public async Task SearchProductsAsync_ShouldReturnMatchingProducts_WhenQueryIsValid()
        {
            // Arrange
            var query = "測試";
            var expectedProducts = new List<StoreProduct>
            {
                new StoreProduct
                {
                    ProductId = 1,
                    Name = "測試商品",
                    Description = "這是測試商品的描述"
                }
            };

            _mockStoreRepository
                .Setup(x => x.SearchProductsAsync(query, It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(expectedProducts);

            // Act
            var result = await _storeService.SearchProductsAsync(query);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedProducts.Count, result.Count());
            Assert.Contains(result, p => p.Name.Contains(query) || p.Description.Contains(query));
        }

        #endregion

        #region 購物車相關測試

        [Fact]
        public async Task GetCartAsync_ShouldReturnCart_WhenUserHasCart()
        {
            // Arrange
            var userId = 1;
            var expectedCart = new StoreCart
            {
                UserId = userId,
                CartItems = new List<StoreCartItem>
                {
                    new StoreCartItem
                    {
                        CartItemId = 1,
                        ProductId = 1,
                        Quantity = 2
                    }
                }
            };

            _mockCartRepository
                .Setup(x => x.GetCartByUserAsync(userId))
                .ReturnsAsync(expectedCart);

            // Act
            var result = await _storeService.GetCartAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedCart.UserId, result.UserId);
            Assert.Equal(expectedCart.CartItems.Count, result.CartItems.Count());
        }

        [Fact]
        public async Task AddToCartAsync_ShouldAddItem_WhenItemDoesNotExist()
        {
            // Arrange
            var userId = 1;
            var productId = 1;
            var quantity = 2;

            _mockCartRepository
                .Setup(x => x.GetCartByUserAsync(userId))
                .ReturnsAsync((StoreCart)null);

            _mockCartRepository
                .Setup(x => x.CreateCartAsync(It.IsAny<StoreCart>()))
                .ReturnsAsync(new StoreCart { UserId = userId });

            // Act
            var result = await _storeService.AddToCartAsync(userId, productId, quantity);

            // Assert
            Assert.True(result);
            _mockCartRepository.Verify(x => x.CreateCartAsync(It.IsAny<StoreCart>()), Times.Once);
        }

        [Fact]
        public async Task AddToCartAsync_ShouldUpdateQuantity_WhenItemExists()
        {
            // Arrange
            var userId = 1;
            var productId = 1;
            var quantity = 2;

            var existingCart = new StoreCart
            {
                UserId = userId,
                CartItems = new List<StoreCartItem>
                {
                    new StoreCartItem
                    {
                        CartItemId = 1,
                        ProductId = productId,
                        Quantity = 1
                    }
                }
            };

            _mockCartRepository
                .Setup(x => x.GetCartByUserAsync(userId))
                .ReturnsAsync(existingCart);

            // Act
            var result = await _storeService.AddToCartAsync(userId, productId, quantity);

            // Assert
            Assert.True(result);
            _mockCartRepository.Verify(x => x.UpdateCartAsync(It.IsAny<StoreCart>()), Times.Once);
        }

        [Fact]
        public async Task RemoveFromCartAsync_ShouldRemoveItem_WhenItemExists()
        {
            // Arrange
            var userId = 1;
            var productId = 1;

            var existingCart = new StoreCart
            {
                UserId = userId,
                CartItems = new List<StoreCartItem>
                {
                    new StoreCartItem
                    {
                        CartItemId = 1,
                        ProductId = productId,
                        Quantity = 1
                    }
                }
            };

            _mockCartRepository
                .Setup(x => x.GetCartByUserAsync(userId))
                .ReturnsAsync(existingCart);

            // Act
            var result = await _storeService.RemoveFromCartAsync(userId, productId);

            // Assert
            Assert.True(result);
            _mockCartRepository.Verify(x => x.UpdateCartAsync(It.IsAny<StoreCart>()), Times.Once);
        }

        [Fact]
        public async Task UpdateCartItemQuantityAsync_ShouldUpdateQuantity_WhenItemExists()
        {
            // Arrange
            var userId = 1;
            var productId = 1;
            var newQuantity = 3;

            var existingCart = new StoreCart
            {
                UserId = userId,
                CartItems = new List<StoreCartItem>
                {
                    new StoreCartItem
                    {
                        CartItemId = 1,
                        ProductId = productId,
                        Quantity = 1
                    }
                }
            };

            _mockCartRepository
                .Setup(x => x.GetCartByUserAsync(userId))
                .ReturnsAsync(existingCart);

            // Act
            var result = await _storeService.UpdateCartItemQuantityAsync(userId, productId, newQuantity);

            // Assert
            Assert.True(result);
            _mockCartRepository.Verify(x => x.UpdateCartAsync(It.IsAny<StoreCart>()), Times.Once);
        }

        [Fact]
        public async Task ClearCartAsync_ShouldClearCart_WhenCartExists()
        {
            // Arrange
            var userId = 1;
            var existingCart = new StoreCart
            {
                UserId = userId,
                CartItems = new List<StoreCartItem>
                {
                    new StoreCartItem { CartItemId = 1, ProductId = 1, Quantity = 1 }
                }
            };

            _mockCartRepository
                .Setup(x => x.GetCartByUserAsync(userId))
                .ReturnsAsync(existingCart);

            // Act
            var result = await _storeService.ClearCartAsync(userId);

            // Assert
            Assert.True(result);
            _mockCartRepository.Verify(x => x.UpdateCartAsync(It.IsAny<StoreCart>()), Times.Once);
        }

        #endregion

        #region 訂單相關測試

        [Fact]
        public async Task CreateOrderAsync_ShouldCreateOrder_WhenValidRequest()
        {
            // Arrange
            var userId = 1;
            var request = new CreateOrderRequestDto
            {
                ShippingAddress = "測試地址",
                PaymentMethod = "信用卡",
                Notes = "測試備註"
            };

            var cart = new StoreCart
            {
                UserId = userId,
                CartItems = new List<StoreCartItem>
                {
                    new StoreCartItem
                    {
                        ProductId = 1,
                        Quantity = 2
                    }
                }
            };

            var product = new StoreProduct
            {
                ProductId = 1,
                Price = 1000m,
                StockQuantity = 10
            };

            _mockCartRepository
                .Setup(x => x.GetCartByUserAsync(userId))
                .ReturnsAsync(cart);

            _mockStoreRepository
                .Setup(x => x.GetProductByIdAsync(1))
                .ReturnsAsync(product);

            _mockOrderRepository
                .Setup(x => x.CreateOrderAsync(It.IsAny<StoreOrder>()))
                .ReturnsAsync(new StoreOrder { OrderId = 1 });

            // Act
            var result = await _storeService.CreateOrderAsync(userId, request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            _mockOrderRepository.Verify(x => x.CreateOrderAsync(It.IsAny<StoreOrder>()), Times.Once);
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldFail_WhenCartIsEmpty()
        {
            // Arrange
            var userId = 1;
            var request = new CreateOrderRequestDto
            {
                ShippingAddress = "測試地址",
                PaymentMethod = "信用卡"
            };

            _mockCartRepository
                .Setup(x => x.GetCartByUserAsync(userId))
                .ReturnsAsync((StoreCart)null);

            // Act
            var result = await _storeService.CreateOrderAsync(userId, request);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Contains("購物車是空的", result.Message);
        }

        [Fact]
        public async Task GetOrdersByUserAsync_ShouldReturnOrders_WhenUserHasOrders()
        {
            // Arrange
            var userId = 1;
            var expectedOrders = new List<StoreOrder>
            {
                new StoreOrder
                {
                    OrderId = 1,
                    UserId = userId,
                    OrderNumber = "GC20250101001",
                    Status = "completed"
                }
            };

            _mockOrderRepository
                .Setup(x => x.GetOrdersByUserAsync(userId, It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(expectedOrders);

            // Act
            var result = await _storeService.GetOrdersByUserAsync(userId, 1, 10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedOrders.Count, result.Count());
            Assert.Equal(expectedOrders[0].OrderNumber, result.First().OrderNumber);
        }

        [Fact]
        public async Task GetUserOrdersAsync_ShouldReturnOrders_WhenUserHasOrders()
        {
            // Arrange
            var userId = 1;
            var expectedOrders = new List<StoreOrder>
            {
                new StoreOrder
                {
                    OrderId = 1,
                    UserId = userId,
                    OrderNumber = "GC20250101001",
                    Status = "pending"
                }
            };

            _mockOrderRepository
                .Setup(x => x.GetOrdersByUserAsync(userId, It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(expectedOrders);

            .Setup(x => x.GetOrdersByUserAsync(userId, It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(expectedOrders);

            // Act
            var result = await _storeService.GetUserOrdersAsync(userId, 1, 10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedOrders.Count, result.Count());
            Assert.Equal(expectedOrders[0].OrderNumber, result.First().OrderNumber);
        }

        [Fact]
        public async Task GetOrderByIdAsync_ShouldReturnOrder_WhenOrderExists()
        {
            // Arrange
            var orderId = 1;
            var expectedOrder = new StoreOrder
            {
                OrderId = orderId,
                OrderNumber = "GC20250101001",
                Status = "completed"
            };

            _mockOrderRepository
                .Setup(x => x.GetOrderByIdAsync(orderId))
                .ReturnsAsync(expectedOrder);

            // Act
            var result = await _storeService.GetOrderByIdAsync(orderId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedOrder.OrderId, result.OrderId);
            Assert.Equal(expectedOrder.OrderNumber, result.OrderNumber);
        }

        [Fact]
        public async Task GetOrderByIdAsync_ShouldReturnNull_WhenOrderDoesNotExist()
        {
            // Arrange
            var orderId = 999;
            _mockOrderRepository
                .Setup(x => x.GetOrderByIdAsync(orderId))
                .ReturnsAsync((StoreOrder)null);

            // Act
            var result = await _storeService.GetOrderByIdAsync(orderId);

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region 錯誤處理測試

        [Fact]
        public async Task GetProductsAsync_ShouldReturnEmpty_WhenRepositoryThrowsException()
        {
            // Arrange
            _mockStoreRepository
                .Setup(x => x.GetProductsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal?>(), It.IsAny<decimal?>(), It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception("資料庫錯誤"));

            // Act
            var result = await _storeService.GetProductsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetRelatedProductsAsync_ShouldReturnEmpty_WhenRepositoryThrowsException()
        {
            // Arrange
            var productId = 1;
            var product = new StoreProduct { ProductId = productId, Category = "測試" };

            _mockStoreRepository
                .Setup(x => x.GetProductByIdAsync(productId))
                .ReturnsAsync(product);

            _mockStoreRepository
                .Setup(x => x.GetProductsByCategoryAsync(It.IsAny<string>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception("資料庫錯誤"));

            // Act
            var result = await _storeService.GetRelatedProductsAsync(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public async Task CreateOrderAsync_ShouldFail_WhenRepositoryThrowsException()
        {
            // Arrange
            var userId = 1;
            var request = new CreateOrderRequestDto
            {
                ShippingAddress = "測試地址",
                PaymentMethod = "信用卡"
            };

            var cart = new StoreCart
            {
                UserId = userId,
                CartItems = new List<StoreCartItem>
                {
                    new StoreCartItem { ProductId = 1, Quantity = 1 }
                }
            };

            var product = new StoreProduct
            {
                ProductId = 1,
                Price = 1000m,
                StockQuantity = 10
            };

            _mockCartRepository
                .Setup(x => x.GetCartByUserAsync(userId))
                .Setup(x => x.GetCartByUserAsync(userId))
                .ReturnsAsync(cart);

            _mockStoreRepository
                .Setup(x => x.GetProductByIdAsync(1))
                .ReturnsAsync(product);

            _mockOrderRepository
                .Setup(x => x.CreateOrderAsync(It.IsAny<StoreOrder>()))
                .ThrowsAsync(new Exception("資料庫錯誤"));

            // Act
            var result = await _storeService.CreateOrderAsync(userId, request);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Contains("建立訂單失敗", result.Message);
        }

        #endregion
    }
} 