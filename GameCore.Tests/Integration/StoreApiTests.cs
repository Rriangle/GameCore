using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using GameCore.Infrastructure.Data;
using GameCore.Core.Services;
using GameCore.Core.Repositories;
using GameCore.Core.Entities;
using GameCore.Core.DTOs;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace GameCore.Tests.Integration
{
    /// <summary>
    /// 商城 API 整合測試
    /// 測試完整的 API 端點和資料庫互動
    /// </summary>
    public class StoreApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private readonly GameCoreDbContext _context;

        public StoreApiTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // 使用記憶體資料庫進行測試
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<GameCoreDbContext>));

                    if (descriptor != null)
                        services.Remove(descriptor);

                    services.AddDbContext<GameCoreDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("TestStoreDb");
                    });

                    // 註冊測試服務
                    services.AddScoped<IStoreService, StoreService>();
                    services.AddScoped<IStoreRepository, StoreRepository>();
                    services.AddScoped<IOrderRepository, OrderRepository>();
                    services.AddScoped<ICartRepository, CartRepository>();
                });
            });

            _client = _factory.CreateClient();
            _context = _factory.Services.GetRequiredService<GameCoreDbContext>();
            
            // 建立測試資料庫
            _context.Database.EnsureCreated();
            SeedTestData();
        }

        #region 商品相關測試

        [Fact]
        public async Task GetProducts_ShouldReturnProducts_WhenDatabaseHasData()
        {
            // Act
            var response = await _client.GetAsync("/api/Store/products");
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Contains("測試商品", content);
        }

        [Fact]
        public async Task GetProduct_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            var productId = 1;

            // Act
            var response = await _client.GetAsync($"/api/Store/products/{productId}");
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Contains("測試商品1", content);
        }

        [Fact]
        public async Task GetProduct_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = 999;

            // Act
            var response = await _client.GetAsync($"/api/Store/products/{productId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task SearchProducts_ShouldReturnMatchingProducts_WhenQueryIsValid()
        {
            // Arrange
            var query = "測試";

            // Act
            var response = await _client.GetAsync($"/api/Store/products/search?query={query}");
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Contains("測試商品", content);
        }

        [Fact]
        public async Task SearchProducts_ShouldReturnBadRequest_WhenQueryIsEmpty()
        {
            // Act
            var response = await _client.GetAsync("/api/Store/products/search?query=");

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetRelatedProducts_ShouldReturnRelatedProducts_WhenProductExists()
        {
            // Arrange
            var productId = 1;

            // Act
            var response = await _client.GetAsync($"/api/Store/products/{productId}/related");
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(content);
        }

        [Fact]
        public async Task GetCategories_ShouldReturnCategories_WhenDatabaseHasData()
        {
            // Act
            var response = await _client.GetAsync("/api/Store/categories");
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Contains("遊戲相關", content);
            Assert.Contains("硬體設備", content);
        }

        [Fact]
        public async Task GetCategoryProducts_ShouldReturnProducts_WhenCategoryExists()
        {
            // Arrange
            var category = "遊戲相關";

            // Act
            var response = await _client.GetAsync($"/api/Store/categories/{category}/products");
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Contains("測試商品", content);
        }

        #endregion

        #region 購物車相關測試

        [Fact]
        public async Task GetCart_ShouldReturnUnauthorized_WhenUserNotAuthenticated()
        {
            // Act
            var response = await _client.GetAsync("/api/Store/cart");

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task AddToCart_ShouldReturnUnauthorized_WhenUserNotAuthenticated()
        {
            // Arrange
            var request = new { ProductId = 1, Quantity = 2 };
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/Store/cart/add", content);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task UpdateCartItem_ShouldReturnUnauthorized_WhenUserNotAuthenticated()
        {
            // Arrange
            var request = new { ProductId = 1, Quantity = 3 };
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("/api/Store/cart/update", content);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task RemoveFromCart_ShouldReturnUnauthorized_WhenUserNotAuthenticated()
        {
            // Act
            var response = await _client.DeleteAsync("/api/Store/cart/remove/1");

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task ClearCart_ShouldReturnUnauthorized_WhenUserNotAuthenticated()
        {
            // Act
            var response = await _client.DeleteAsync("/api/Store/cart/clear");

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        #endregion

        #region 訂單相關測試

        [Fact]
        public async Task CreateOrder_ShouldReturnUnauthorized_WhenUserNotAuthenticated()
        {
            // Arrange
            var request = new CreateOrderRequestDto
            {
                ShippingAddress = "測試地址",
                PaymentMethod = "信用卡",
                Notes = "測試備註"
            };
            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/Store/orders/create", content);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetOrder_ShouldReturnUnauthorized_WhenUserNotAuthenticated()
        {
            // Act
            var response = await _client.GetAsync("/api/Store/orders/1");

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetUserOrders_ShouldReturnUnauthorized_WhenUserNotAuthenticated()
        {
            // Act
            var response = await _client.GetAsync("/api/Store/orders/user");

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task CancelOrder_ShouldReturnUnauthorized_WhenUserNotAuthenticated()
        {
            // Act
            var response = await _client.PutAsync("/api/Store/orders/1/cancel", null);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        #endregion

        #region 熱門商品測試

        [Fact]
        public async Task GetPopularProducts_ShouldReturnProducts_WhenDatabaseHasData()
        {
            // Act
            var response = await _client.GetAsync("/api/Store/products/popular");
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(content);
        }

        [Fact]
        public async Task GetSalesRanking_ShouldReturnRanking_WhenDatabaseHasData()
        {
            // Act
            var response = await _client.GetAsync("/api/Store/products/sales-ranking");
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(content);
        }

        #endregion

        #region 商品評價測試

        [Fact]
        public async Task GetProductReviews_ShouldReturnReviews_WhenProductExists()
        {
            // Arrange
            var productId = 1;

            // Act
            var response = await _client.GetAsync($"/api/Store/products/{productId}/reviews");
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(content);
        }

        [Fact]
        public async Task AddProductReview_ShouldReturnUnauthorized_WhenUserNotAuthenticated()
        {
            // Arrange
            var productId = 1;
            var review = new { Rating = 5, Title = "測試評價", Content = "這是測試評價內容" };
            var json = JsonSerializer.Serialize(review);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync($"/api/Store/products/{productId}/reviews", content);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        #endregion

        #region 輔助方法

        /// <summary>
        /// 建立測試資料
        /// </summary>
        private void SeedTestData()
        {
            // 建立測試商品
            var products = new List<StoreProduct>
            {
                new StoreProduct
                {
                    ProductId = 1,
                    Name = "測試商品1",
                    Description = "這是測試商品1的描述",
                    Price = 1000m,
                    OriginalPrice = 1200m,
                    StockQuantity = 100,
                    Category = "遊戲相關",
                    ImageUrl = "/images/test1.jpg",
                    IsActive = true,
                    IsFeatured = true,
                    Rating = 4.5m,
                    ReviewCount = 10,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new StoreProduct
                {
                    ProductId = 2,
                    Name = "測試商品2",
                    Description = "這是測試商品2的描述",
                    Price = 2000m,
                    OriginalPrice = 2500m,
                    StockQuantity = 50,
                    Category = "硬體設備",
                    ImageUrl = "/images/test2.jpg",
                    IsActive = true,
                    IsFeatured = false,
                    Rating = 4.0m,
                    ReviewCount = 5,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new StoreProduct
                {
                    ProductId = 3,
                    Name = "測試商品3",
                    Description = "這是測試商品3的描述",
                    Price = 500m,
                    OriginalPrice = 600m,
                    StockQuantity = 200,
                    Category = "遊戲相關",
                    ImageUrl = "/images/test3.jpg",
                    IsActive = true,
                    IsFeatured = false,
                    Rating = 4.8m,
                    ReviewCount = 15,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };

            _context.StoreProducts.AddRange(products);

            // 建立測試分類
            var categories = new List<StoreCategory>
            {
                new StoreCategory
                {
                    CategoryId = 1,
                    Name = "遊戲相關",
                    Description = "遊戲軟體、DLC、季票等數位商品",
                    Icon = "fas fa-gamepad",
                    SortOrder = 1,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new StoreCategory
                {
                    CategoryId = 2,
                    Name = "硬體設備",
                    Description = "遊戲主機、電腦配件、外設等",
                    Icon = "fas fa-desktop",
                    SortOrder = 2,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };

            _context.StoreCategories.AddRange(categories);

            // 建立測試購物車
            var carts = new List<StoreCart>
            {
                new StoreCart
                {
                    CartId = 1,
                    UserId = 1,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };

            _context.StoreCarts.AddRange(carts);

            // 建立測試購物車項目
            var cartItems = new List<StoreCartItem>
            {
                new StoreCartItem
                {
                    CartItemId = 1,
                    CartId = 1,
                    ProductId = 1,
                    Quantity = 2,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };

            _context.StoreCartItems.AddRange(cartItems);

            // 建立測試訂單
            var orders = new List<StoreOrder>
            {
                new StoreOrder
                {
                    OrderId = 1,
                    UserId = 1,
                    OrderNumber = "GC20250101001",
                    Status = "completed",
                    Subtotal = 2000m,
                    ShippingFee = 100m,
                    DiscountAmount = 0m,
                    TotalAmount = 2100m,
                    ShippingAddress = "測試地址",
                    PaymentMethod = "信用卡",
                    Notes = "測試備註",
                    CreatedAt = DateTime.UtcNow.AddDays(-7),
                    UpdatedAt = DateTime.UtcNow.AddDays(-1)
                }
            };

            _context.StoreOrders.AddRange(orders);

            // 建立測試訂單項目
            var orderItems = new List<StoreOrderItem>
            {
                new StoreOrderItem
                {
                    OrderItemId = 1,
                    OrderId = 1,
                    ProductId = 1,
                    ProductName = "測試商品1",
                    ProductImageUrl = "/images/test1.jpg",
                    UnitPrice = 1000m,
                    Quantity = 2,
                    TotalPrice = 2000m,
                    CreatedAt = DateTime.UtcNow.AddDays(-7),
                    UpdatedAt = DateTime.UtcNow.AddDays(-7)
                }
            };

            _context.StoreOrderItems.AddRange(orderItems);

            // 建立測試商品評價
            var reviews = new List<StoreProductReview>
            {
                new StoreProductReview
                {
                    ReviewId = 1,
                    ProductId = 1,
                    UserId = 1,
                    Rating = 5,
                    Title = "很好的商品",
                    Content = "商品品質很好，很滿意",
                    IsVerified = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-5),
                    UpdatedAt = DateTime.UtcNow.AddDays(-5)
                },
                new StoreProductReview
                {
                    ReviewId = 2,
                    ProductId = 1,
                    UserId = 2,
                    Rating = 4,
                    Title = "還不錯",
                    Content = "商品還不錯，可以推薦",
                    IsVerified = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-3),
                    UpdatedAt = DateTime.UtcNow.AddDays(-3)
                }
            };

            _context.StoreProductReviews.AddRange(reviews);

            // 建立測試熱門商品統計
            var popularProducts = new List<StorePopularProduct>
            {
                new StorePopularProduct
                {
                    PopularProductId = 1,
                    ProductId = 1,
                    ViewCount = 1000,
                    SaleCount = 100,
                    WishlistCount = 50,
                    Rating = 4.5m,
                    ReviewCount = 10,
                    LastUpdated = DateTime.UtcNow
                },
                new StorePopularProduct
                {
                    PopularProductId = 2,
                    ProductId = 2,
                    ViewCount = 800,
                    SaleCount = 80,
                    WishlistCount = 40,
                    Rating = 4.0m,
                    ReviewCount = 5,
                    LastUpdated = DateTime.UtcNow
                }
            };

            _context.StorePopularProducts.AddRange(popularProducts);

            _context.SaveChanges();
        }

        #endregion

        #region 清理資源

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
            _client.Dispose();
        }

        #endregion
    }
} 