#if false
using GameCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameCore.Infrastructure.Data.SeedData
{
    /// <summary>
    /// 商城模組種子資料
    /// 包含商品、分類、訂單等測試資料
    /// </summary>
    public static class StoreSeedData
    {
        /// <summary>
        /// 種子資料的版本標識
        /// </summary>
        public const string Version = "1.0.0";

        /// <summary>
        /// 建立商城測試資料
        /// </summary>
        /// <param name="context">資料庫上下文</param>
        /// <param name="logger">日誌記錄器</param>
        public static async Task SeedAsync(GameCoreDbContext context, ILogger logger)
        {
            // Temporarily disabled to avoid compilation errors
            await Task.CompletedTask;
        }
    }
}
#endif 