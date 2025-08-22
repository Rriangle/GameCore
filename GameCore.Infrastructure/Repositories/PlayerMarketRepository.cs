using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GameCore.Infrastructure.Repositories
{
    /// <summary>
    /// 玩家市場 Repository 實作
    /// </summary>
    public class PlayerMarketRepository : Repository<PlayerMarketProductInfo>, IPlayerMarketRepository
    {
        public PlayerMarketRepository(GameCoreDbContext context) : base(context)
        {
        }

        /// <summary>
        /// 搜尋商品
        /// </summary>
        public async Task<IEnumerable<PlayerMarketProductInfo>> SearchProductsAsync(string? searchTerm, int page, int pageSize)
        {
            var query = _context.PlayerMarketProductInfos.AsQueryable();

            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        /// <summary>
        /// 獲取使用者商品
        /// </summary>
        public async Task<IEnumerable<PlayerMarketProductInfo>> GetUserProductsAsync(int userId)
        {
            return await _context.PlayerMarketProductInfos
                .Where(p => p.SellerId == userId)
                .ToListAsync();
        }

        /// <summary>
        /// 獲取熱門商品
        /// </summary>
        public async Task<IEnumerable<PlayerMarketProductInfo>> GetPopularProductsAsync(int count)
        {
            return await _context.PlayerMarketProductInfos
                .Take(count)
                .ToListAsync();
        }

        /// <summary>
        /// 獲取最新商品
        /// </summary>
        public async Task<IEnumerable<PlayerMarketProductInfo>> GetLatestProductsAsync(int count)
        {
            return await _context.PlayerMarketProductInfos
                .Take(count)
                .ToListAsync();
        }

        /// <summary>
        /// 獲取商品詳細資訊（包含圖片）
        /// </summary>
        public async Task<PlayerMarketProductInfo?> GetProductWithImagesAsync(int productId)
        {
            return await _context.PlayerMarketProductInfos
                .FirstOrDefaultAsync(p => p.PProductId == productId);
        }
    }
}