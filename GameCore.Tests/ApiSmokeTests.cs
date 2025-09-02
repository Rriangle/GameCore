using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Xunit;

namespace GameCore.Tests
{
    /// <summary>
    /// API 冒煙測試
    /// </summary>
    public class ApiSmokeTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        /// <summary>
        /// 建構函式
        /// </summary>
        /// <param name="factory">Web 應用程式工廠</param>
        public ApiSmokeTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// 測試健康檢查端點
        /// </summary>
        [Fact]
        [Trait("Smoke", "API")]
        public async Task HealthCheck_ShouldReturnOk()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/health");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Healthy", content);
        }

        /// <summary>
        /// 測試詳細健康檢查端點
        /// </summary>
        [Fact]
        [Trait("Smoke", "API")]
        public async Task DetailedHealthCheck_ShouldReturnOk()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/health/detailed");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Healthy", content);
            Assert.Contains("Version", content);
        }

        /// <summary>
        /// 測試就緒檢查端點
        /// </summary>
        [Fact]
        [Trait("Smoke", "API")]
        public async Task ReadyCheck_ShouldReturnOk()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/health/ready");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Ready", content);
        }

        /// <summary>
        /// 測試存活檢查端點
        /// </summary>
        [Fact]
        [Trait("Smoke", "API")]
        public async Task LiveCheck_ShouldReturnOk()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/health/live");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Alive", content);
        }

        /// <summary>
        /// 測試不存在的端點應該回傳 404
        /// </summary>
        [Fact]
        [Trait("Smoke", "API")]
        public async Task NonExistentEndpoint_ShouldReturnNotFound()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/nonexistent");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        /// <summary>
        /// 測試根路徑應該回傳 404（因為沒有根控制器）
        /// </summary>
        [Fact]
        [Trait("Smoke", "API")]
        public async Task RootPath_ShouldReturnNotFound()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        // TODO: 添加更多端點測試
        // [Fact]
        // [Trait("Smoke", "API")]
        // public async Task AuthEndpoints_ShouldReturnNotImplemented()
        // {
        //     // 測試認證端點回傳 501 Not Implemented
        // }
        // 
        // [Fact]
        // [Trait("Smoke", "API")]
        // public async Task UserEndpoints_ShouldReturnNotImplemented()
        // {
        //     // 測試用戶端點回傳 501 Not Implemented
        // }
        // 
        // [Fact]
        // [Trait("Smoke", "API")]
        // public async Task WalletEndpoints_ShouldReturnNotImplemented()
        // {
        //     // 測試錢包端點回傳 501 Not Implemented
        // }
    }
} 