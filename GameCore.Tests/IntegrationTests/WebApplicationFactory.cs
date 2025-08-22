using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using GameCore.Infrastructure.Data;
using GameCore.Tests.Infrastructure;

namespace GameCore.Tests.IntegrationTests
{
    /// <summary>
    /// 測試用 Web 應用程式工廠
    /// 為整合測試提供配置好的測試環境
    /// </summary>
    public class GameCoreWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // 移除原有的 DbContext 註冊
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<GameCoreDbContext>));
                
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // 註冊測試用的記憶體資料庫
                services.AddDbContext<GameCoreDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDatabase_" + Guid.NewGuid());
                    options.EnableSensitiveDataLogging();
                });

                // 確保資料庫已創建並種植測試資料
                var serviceProvider = services.BuildServiceProvider();
                using var scope = serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<GameCoreDbContext>();
                
                try
                {
                    context.Database.EnsureCreated();
                    TestDataSeeder.SeedBasicData(context);
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<GameCoreWebApplicationFactory>>();
                    logger.LogError(ex, "測試資料庫初始化時發生錯誤");
                }
            });

            builder.UseEnvironment("Testing");
        }
    }
}
