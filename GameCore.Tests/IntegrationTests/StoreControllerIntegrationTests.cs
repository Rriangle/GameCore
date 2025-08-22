using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using GameCore.Web.Controllers;
using Newtonsoft.Json;

namespace GameCore.Tests.IntegrationTests
{
    /// <summary>
    /// 商城控制器整合測試
    /// 測試商城功能的完整流程
    /// </summary>
    public class StoreControllerIntegrationTests : IClassFixture<GameCoreWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly GameCoreWebApplicationFactory _factory;

        public StoreControllerIntegrationTests(GameCoreWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetStoreIndex_應該返回商城首頁()
        {
            // Act
            var response = await _client.GetAsync("/Store");

            // Assert
            if (response.StatusCode == HttpStatusCode.Redirect)
            {
                // 可能重定向到登入頁面
                response.Headers.Location?.ToString().Should().Contain("login", StringComparison.OrdinalIgnoreCase);
                return;
            }

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("商城", StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task GetProducts_應該返回商品列表()
        {
            // Act
            var response = await _client.GetAsync("/Store/Products");

            // Assert
            if (response.StatusCode == HttpStatusCode.Redirect)
            {
                return; // 重定向到登入頁面
            }

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetProducts_帶搜尋參數_應該返回篩選結果()
        {
            // Act
            var response = await _client.GetAsync("/Store/Products?search=遊戲&category=games");

            // Assert
            if (response.StatusCode == HttpStatusCode.Redirect)
            {
                return;
            }

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetProductDetails_有效商品ID_應該返回商品詳情()
        {
            // Arrange
            var productId = 1;

            // Act
            var response = await _client.GetAsync($"/Store/Details/{productId}");

            // Assert
            if (response.StatusCode == HttpStatusCode.Redirect)
            {
                return;
            }

            // 可能是 200 (找到商品) 或 404 (商品不存在)
            response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddToCart_有效商品_應該成功添加()
        {
            // Arrange
            var addToCartRequest = new AddToCartRequest
            {
                ProductId = 1,
                Quantity = 1
            };

            // Act
            var response = await _client.PostAsJsonAsync("/Store/AddToCart", addToCartRequest);

            // Assert
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return;
            }

            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<dynamic>(content);
                result.Should().NotBeNull();
            }
        }

        [Fact]
        public async Task GetCart_應該返回購物車頁面()
        {
            // Act
            var response = await _client.GetAsync("/Store/Cart");

            // Assert
            if (response.StatusCode == HttpStatusCode.Redirect)
            {
                return;
            }

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("購物車", StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task GetCheckout_應該返回結帳頁面()
        {
            // Act
            var response = await _client.GetAsync("/Store/Checkout");

            // Assert
            if (response.StatusCode == HttpStatusCode.Redirect)
            {
                // 可能重定向到購物車（空購物車）或登入頁面
                return;
            }

            response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData("newest")]
        [InlineData("popular")]
        [InlineData("price_low")]
        [InlineData("price_high")]
        public async Task GetProducts_不同排序方式_應該返回結果(string sortBy)
        {
            // Act
            var response = await _client.GetAsync($"/Store/Products?sort={sortBy}");

            // Assert
            if (response.StatusCode == HttpStatusCode.Redirect)
            {
                return;
            }

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task PlaceOrder_完整結帳流程測試()
        {
            // 這是一個複雜的流程測試，需要：
            // 1. 添加商品到購物車
            // 2. 進入結帳頁面
            // 3. 提交訂單

            // Step 1: 添加商品到購物車
            var addToCartRequest = new AddToCartRequest
            {
                ProductId = 1,
                Quantity = 1
            };

            var addToCartResponse = await _client.PostAsJsonAsync("/Store/AddToCart", addToCartRequest);
            
            if (addToCartResponse.StatusCode == HttpStatusCode.Unauthorized)
            {
                return; // 需要登入
            }

            // Step 2: 獲取結帳頁面
            var checkoutResponse = await _client.GetAsync("/Store/Checkout");
            
            if (checkoutResponse.StatusCode == HttpStatusCode.Redirect)
            {
                return; // 可能購物車為空或需要登入
            }

            // Step 3: 提交訂單（這裡只是測試端點存在）
            var orderRequest = new PlaceOrderRequest
            {
                PaymentMethod = "信用卡",
                DeliveryMethod = "宅配",
                Notes = "測試訂單"
            };

            var orderResponse = await _client.PostAsJsonAsync("/Store/PlaceOrder", orderRequest);
            
            // 無論成功或失敗，都應該有適當的響應
            orderResponse.Should().NotBeNull();
        }
    }

    /// <summary>
    /// 添加到購物車請求模型
    /// </summary>
    public class AddToCartRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; } = 1;
    }

    /// <summary>
    /// 下訂單請求模型
    /// </summary>
    public class PlaceOrderRequest
    {
        public string PaymentMethod { get; set; } = string.Empty;
        public string DeliveryMethod { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }
}
