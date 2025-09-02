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

namespace GameCore.Tests.E2E
{
    /// <summary>
    /// 商城端對端測試
    /// 測試完整的購物流程，從瀏覽商品到完成訂單
    /// </summary>
    public class StoreE2ETests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private readonly GameCoreDbContext _context;

        public StoreE2ETests(WebApplicationFactory<Program> factory)
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
                        options.UseInMemoryDatabase("TestStoreE2EDb");
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

        #region 完整購物流程測試

        [Fact]
        public async Task CompleteShoppingFlow_ShouldWorkEndToEnd()
        {
            // 1. 瀏覽商品列表
            var productsResponse = await _client.GetAsync("/api/Store/products");
            productsResponse.EnsureSuccessStatusCode();
            var productsContent = await productsResponse.Content.ReadAsStringAsync();
            Assert.Contains("測試商品", productsContent);

            // 2. 查看商品詳情
            var productResponse = await _client.GetAsync("/api/Store/products/1");
            productResponse.EnsureSuccessStatusCode();
            var productContent = await productResponse.Content.ReadAsStringAsync();
            Assert.Contains("測試商品1", productContent);

            // 3. 搜尋商品
            var searchResponse = await _client.GetAsync("/api/Store/products/search?query=測試");
            searchResponse.EnsureSuccessStatusCode();
            var searchContent = await searchResponse.Content.ReadAsStringAsync();
            Assert.Contains("測試商品", searchContent);

            // 4. 查看商品分類
            var categoriesResponse = await _client.GetAsync("/api/Store/categories");
            categoriesResponse.EnsureSuccessStatusCode();
            var categoriesContent = await categoriesResponse.Content.ReadAsStringAsync();
            Assert.Contains("遊戲相關", categoriesContent);

            // 5. 查看分類商品
            var categoryProductsResponse = await _client.GetAsync("/api/Store/categories/遊戲相關/products");
            categoryProductsResponse.EnsureSuccessStatusCode();
            var categoryProductsContent = await categoryProductsResponse.Content.ReadAsStringAsync();
            Assert.Contains("測試商品", categoryProductsContent);

            // 6. 查看相關商品推薦
            var relatedProductsResponse = await _client.GetAsync("/api/Store/products/1/related");
            relatedProductsResponse.EnsureSuccessStatusCode();

            // 7. 查看商品評價
            var reviewsResponse = await _client.GetAsync("/api/Store/products/1/reviews");
            reviewsResponse.EnsureSuccessStatusCode();

            // 8. 查看熱門商品
            var popularProductsResponse = await _client.GetAsync("/api/Store/products/popular");
            popularProductsResponse.EnsureSuccessStatusCode();

            // 9. 查看銷售排行榜
            var salesRankingResponse = await _client.GetAsync("/api/Store/products/sales-ranking");
            salesRankingResponse.EnsureSuccessStatusCode();
        }

        #endregion

        #region 購物車流程測試

        [Fact]
        public async Task ShoppingCartFlow_ShouldWorkCorrectly()
        {
            // 注意：這些測試需要身份驗證，在實際環境中需要模擬登入狀態
            
            // 1. 嘗試取得購物車（應該返回未授權）
            var getCartResponse = await _client.GetAsync("/api/Store/cart");
            Assert.Equal(HttpStatusCode.Unauthorized, getCartResponse.StatusCode);

            // 2. 嘗試加入購物車（應該返回未授權）
            var addToCartRequest = new { ProductId = 1, Quantity = 2 };
            var addToCartJson = JsonSerializer.Serialize(addToCartRequest);
            var addToCartContent = new StringContent(addToCartJson, Encoding.UTF8, "application/json");
            
            var addToCartResponse = await _client.PostAsync("/api/Store/cart/add", addToCartContent);
            Assert.Equal(HttpStatusCode.Unauthorized, addToCartResponse.StatusCode);

            // 3. 嘗試更新購物車（應該返回未授權）
            var updateCartRequest = new { ProductId = 1, Quantity = 3 };
            var updateCartJson = JsonSerializer.Serialize(updateCartRequest);
            var updateCartContent = new StringContent(updateCartJson, Encoding.UTF8, "application/json");
            
            var updateCartResponse = await _client.PutAsync("/api/Store/cart/update", updateCartContent);
            Assert.Equal(HttpStatusCode.Unauthorized, updateCartResponse.StatusCode);

            // 4. 嘗試移除購物車商品（應該返回未授權）
            var removeFromCartResponse = await _client.DeleteAsync("/api/Store/cart/remove/1");
            Assert.Equal(HttpStatusCode.Unauthorized, removeFromCartResponse.StatusCode);

            // 5. 嘗試清空購物車（應該返回未授權）
            var clearCartResponse = await _client.DeleteAsync("/api/Store/cart/clear");
            Assert.Equal(HttpStatusCode.Unauthorized, clearCartResponse.StatusCode);
        }

        #endregion

        #region 訂單流程測試

        [Fact]
        public async Task OrderFlow_ShouldWorkCorrectly()
        {
            // 注意：這些測試需要身份驗證，在實際環境中需要模擬登入狀態
            
            // 1. 嘗試建立訂單（應該返回未授權）
            var createOrderRequest = new CreateOrderRequestDto
            {
                ShippingAddress = "測試地址",
                PaymentMethod = "信用卡",
                Notes = "測試備註"
            };
            var createOrderJson = JsonSerializer.Serialize(createOrderRequest);
            var createOrderContent = new StringContent(createOrderJson, Encoding.UTF8, "application/json");
            
            var createOrderResponse = await _client.PostAsync("/api/Store/orders/create", createOrderContent);
            Assert.Equal(HttpStatusCode.Unauthorized, createOrderResponse.StatusCode);

            // 2. 嘗試取得訂單詳情（應該返回未授權）
            var getOrderResponse = await _client.GetAsync("/api/Store/orders/1");
            Assert.Equal(HttpStatusCode.Unauthorized, getOrderResponse.StatusCode);

            // 3. 嘗試取得用戶訂單列表（應該返回未授權）
            var getUserOrdersResponse = await _client.GetAsync("/api/Store/orders/user");
            Assert.Equal(HttpStatusCode.Unauthorized, getUserOrdersResponse.StatusCode);

            // 4. 嘗試取消訂單（應該返回未授權）
            var cancelOrderResponse = await _client.PutAsync("/api/Store/orders/1/cancel", null);
            Assert.Equal(HttpStatusCode.Unauthorized, cancelOrderResponse.StatusCode);
        }

        #endregion

        #region 商品評價流程測試

        [Fact]
        public async Task ProductReviewFlow_ShouldWorkCorrectly()
        {
            // 1. 查看商品評價（允許匿名）
            var getReviewsResponse = await _client.GetAsync("/api/Store/products/1/reviews");
            getReviewsResponse.EnsureSuccessStatusCode();

            // 2. 嘗試新增商品評價（應該返回未授權）
            var addReviewRequest = new { Rating = 5, Title = "測試評價", Content = "這是測試評價內容" };
            var addReviewJson = JsonSerializer.Serialize(addReviewRequest);
            var addReviewContent = new StringContent(addReviewJson, Encoding.UTF8, "application/json");
            
            var addReviewResponse = await _client.PostAsync("/api/Store/products/1/reviews", addReviewContent);
            Assert.Equal(HttpStatusCode.Unauthorized, addReviewResponse.StatusCode);
        }

        #endregion

        #region 錯誤處理測試

        [Fact]
        public async Task ErrorHandling_ShouldWorkCorrectly()
        {
            // 1. 測試不存在的商品
            var notFoundResponse = await _client.GetAsync("/api/Store/products/999");
            Assert.Equal(HttpStatusCode.NotFound, notFoundResponse.StatusCode);

            // 2. 測試空的搜尋關鍵字
            var badRequestResponse = await _client.GetAsync("/api/Store/products/search?query=");
            Assert.Equal(HttpStatusCode.BadRequest, badRequestResponse.StatusCode);

            // 3. 測試無效的 API 端點
            var notFoundEndpointResponse = await _client.GetAsync("/api/Store/invalid");
            Assert.Equal(HttpStatusCode.NotFound, notFoundEndpointResponse.StatusCode);
        }

        #endregion

        #region 效能測試

        [Fact]
        public async Task Performance_ShouldBeAcceptable()
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            // 執行多個 API 呼叫
            var tasks = new List<Task<HttpResponseMessage>>
            {
                _client.GetAsync("/api/Store/products"),
                _client.GetAsync("/api/Store/categories"),
                _client.GetAsync("/api/Store/products/1"),
                _client.GetAsync("/api/Store/products/popular"),
                _client.GetAsync("/api/Store/products/sales-ranking")
            };

            await Task.WhenAll(tasks);
            stopwatch.Stop();

            // 檢查所有回應都成功
            foreach (var task in tasks)
            {
                var response = await task;
                response.EnsureSuccessStatusCode();
            }

            // 檢查回應時間在可接受範圍內（5秒內）
            Assert.True(stopwatch.ElapsedMilliseconds < 5000, 
                $"API 回應時間過長: {stopwatch.ElapsedMilliseconds}ms");
        }

        #endregion

        #region 資料一致性測試

        [Fact]
        public async Task DataConsistency_ShouldBeMaintained()
        {
            // 1. 檢查商品資料一致性
            var productsResponse = await _client.GetAsync("/api/Store/products");
            var productsContent = await productsResponse.Content.ReadAsStringAsync();
            
            // 檢查商品數量
            var products = JsonSerializer.Deserialize<List<ProductDto>>(productsContent, 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.NotNull(products);
            Assert.True(products.Count > 0);

            // 2. 檢查分類資料一致性
            var categoriesResponse = await _client.GetAsync("/api/Store/categories");
            var categoriesContent = await categoriesResponse.Content.ReadAsStringAsync();
            
            var categories = JsonSerializer.Deserialize<List<CategoryDto>>(categoriesContent, 
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.NotNull(categories);
            Assert.True(categories.Count > 0);

            // 3. 檢查商品與分類的關聯性
            foreach (var product in products)
            {
                Assert.True(categories.Any(c => c.Name == product.Category), 
                    $"商品 {product.Name} 的分類 {product.Category} 不存在於分類列表中");
            }
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
                },
                new StoreProduct
                {
                    ProductId = 4,
                    Name = "測試商品4",
                    Description = "這是測試商品4的描述",
                    Price = 1500m,
                    OriginalPrice = 1800m,
                    StockQuantity = 75,
                    Category = "配件周邊",
                    ImageUrl = "/images/test4.jpg",
                    IsActive = true,
                    IsFeatured = true,
                    Rating = 4.2m,
                    ReviewCount = 8,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new StoreProduct
                {
                    ProductId = 5,
                    Name = "測試商品5",
                    Description = "這是測試商品5的描述",
                    Price = 3000m,
                    OriginalPrice = 3500m,
                    StockQuantity = 25,
                    Category = "收藏品",
                    ImageUrl = "/images/test5.jpg",
                    IsActive = true,
                    IsFeatured = false,
                    Rating = 4.9m,
                    ReviewCount = 20,
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
                },
                new StoreCategory
                {
                    CategoryId = 3,
                    Name = "配件周邊",
                    Description = "鍵盤、滑鼠、耳機、手把等遊戲配件",
                    Icon = "fas fa-headphones",
                    SortOrder = 3,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new StoreCategory
                {
                    CategoryId = 4,
                    Name = "收藏品",
                    Description = "遊戲手辦、模型、藝術設定集等收藏品",
                    Icon = "fas fa-trophy",
                    SortOrder = 4,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };

            _context.StoreCategories.AddRange(categories);

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
                },
                new StoreProductReview
                {
                    ReviewId = 3,
                    ProductId = 2,
                    UserId = 3,
                    Rating = 4,
                    Title = "品質不錯",
                    Content = "硬體品質不錯，值得購買",
                    IsVerified = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-2),
                    UpdatedAt = DateTime.UtcNow.AddDays(-2)
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
                },
                new StorePopularProduct
                {
                    PopularProductId = 3,
                    ProductId = 3,
                    ViewCount = 1200,
                    SaleCount = 120,
                    WishlistCount = 60,
                    Rating = 4.8m,
                    ReviewCount = 15,
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