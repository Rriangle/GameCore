using Microsoft.EntityFrameworkCore;
using GameCore.Infrastructure.Data;

namespace GameCore.Tests.Infrastructure
{
    /// <summary>
    /// 測試用資料庫上下文工廠
    /// 提供記憶體內資料庫用於測試
    /// </summary>
    public static class TestDbContextFactory
    {
        /// <summary>
        /// 創建記憶體內測試資料庫
        /// </summary>
        /// <param name="databaseName">資料庫名稱，預設為隨機 GUID</param>
        /// <returns>測試用 DbContext</returns>
        public static GameCoreDbContext CreateInMemoryDatabase(string? databaseName = null)
        {
            var options = new DbContextOptionsBuilder<GameCoreDbContext>()
                .UseInMemoryDatabase(databaseName ?? Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;

            var context = new GameCoreDbContext(options);
            
            // 確保資料庫已創建
            context.Database.EnsureCreated();
            
            return context;
        }

        /// <summary>
        /// 創建 SQLite 測試資料庫（用於更複雜的測試場景）
        /// </summary>
        /// <param name="databaseName">資料庫檔案名稱</param>
        /// <returns>測試用 DbContext</returns>
        public static GameCoreDbContext CreateSqliteDatabase(string? databaseName = null)
        {
            var dbName = databaseName ?? $"test_{Guid.NewGuid()}.db";
            var connectionString = $"Data Source={dbName}";

            var options = new DbContextOptionsBuilder<GameCoreDbContext>()
                .UseSqlite(connectionString)
                .EnableSensitiveDataLogging()
                .Options;

            var context = new GameCoreDbContext(options);
            
            // 確保資料庫已創建並應用遷移
            context.Database.EnsureCreated();
            
            return context;
        }

        /// <summary>
        /// 清理測試資料庫
        /// </summary>
        /// <param name="context">要清理的 DbContext</param>
        public static void CleanupDatabase(GameCoreDbContext context)
        {
            try
            {
                context.Database.EnsureDeleted();
                context.Dispose();
            }
            catch (Exception ex)
            {
                // 記錄清理失敗，但不拋出異常
                Console.WriteLine($"清理測試資料庫時發生錯誤: {ex.Message}");
            }
        }
    }
}
