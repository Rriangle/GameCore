using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GameCore.Infrastructure.Repositories
{
    public class StoreRepository : Repository<ProductInfo>, IStoreRepository
    {
        public StoreRepository(GameCoreDbContext context) : base(context)
        {
        }

        public async Task<ProductInfo?> GetProductWithDetailsAsync(int productId)
        {
            return await _context.Set<ProductInfo>()
                .Include(p => p.Store)
                .FirstOrDefaultAsync(p => p.ProductId == productId);
        }

        public async Task<PagedResult<ProductInfo>> SearchProductsAsync(string? searchTerm, string? productType, int page = 1, int pageSize = 20)
        {
            var query = _context.Set<ProductInfo>().AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p => p.ProductName.Contains(searchTerm) || p.Description.Contains(searchTerm));
            }

            if (!string.IsNullOrEmpty(productType))
            {
                query = query.Where(p => p.ProductType == productType);
            }

            var totalCount = await query.CountAsync();
            var products = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<ProductInfo>
            {
                Items = products,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}