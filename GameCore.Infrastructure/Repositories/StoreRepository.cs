using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GameCore.Infrastructure.Repositories
{
    /// <summary>
    /// 商店 Repository 實作
    /// </summary>
    public class StoreRepository : Repository<ProductInfo>, IStoreRepository
    {
        public StoreRepository(GameCoreDbContext context) : base(context)
        {
        }

        /// <summary>
        /// 獲取商品詳細資訊
        /// </summary>
        public async Task<ProductInfo?> GetProductWithDetailsAsync(int productId)
        {
            return await _context.ProductInfos
                .FirstOrDefaultAsync(p => p.ProductId == productId);
        }

        /// <summary>
        /// 搜尋商品
        /// </summary>
        public async Task<IEnumerable<ProductInfo>> SearchProductsAsync(string? searchTerm, string? category, int page, int pageSize)
        {
            var query = _context.ProductInfos.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p => p.ProductName.Contains(searchTerm));
            }

            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        /// <summary>
        /// 獲取熱門商品
        /// </summary>
        public async Task<IEnumerable<ProductInfo>> GetPopularProductsAsync(int count)
        {
            return await _context.ProductInfos
                .Take(count)
                .ToListAsync();
        }

        /// <summary>
        /// 獲取新品
        /// </summary>
        public async Task<IEnumerable<ProductInfo>> GetNewProductsAsync(int count)
        {
            return await _context.ProductInfos
                .Take(count)
                .ToListAsync();
        }

        /// <summary>
        /// 獲取精選商品
        /// </summary>
        public async Task<IEnumerable<ProductInfo>> GetFeaturedProductsAsync(int count)
        {
            return await _context.ProductInfos
                .Take(count)
                .ToListAsync();
        }
    }
}