using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GameCore.Infrastructure.Repositories
{
    public class PlayerMarketRepository : Repository<PlayerMarketProductInfo>, IPlayerMarketRepository
    {
        public PlayerMarketRepository(GameCoreDbContext context) : base(context)
        {
        }

        public async Task<PagedResult<PlayerMarketProductInfo>> SearchProductsAsync(string? searchTerm, int page = 1, int pageSize = 20)
        {
            var query = _context.Set<PlayerMarketProductInfo>().AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p => p.ProductName.Contains(searchTerm) || p.Description.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();
            var products = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<PlayerMarketProductInfo>
            {
                Items = products,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}