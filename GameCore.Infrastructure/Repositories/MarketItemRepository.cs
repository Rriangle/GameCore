using GameCore.Domain.Entities;
using GameCore.Domain.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameCore.Infrastructure.Repositories
{
    public class MarketItemRepository : Repository<MarketItem>, IMarketItemRepository
    {
        private readonly ILogger<MarketItemRepository> _logger;

        public MarketItemRepository(GameCoreDbContext context, ILogger<MarketItemRepository> logger) : base(context)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<MarketItem>> GetActiveItemsAsync(string? category = null, decimal? minPrice = null, decimal? maxPrice = null, int page = 1, int pageSize = 20)
        {
            var query = _dbSet
                .Include(i => i.Seller)
                .Include(i => i.Category)
                .Where(i => i.Status == "Active");

            if (!string.IsNullOrEmpty(category))
                query = query.Where(i => i.Category.Name == category);

            if (minPrice.HasValue)
                query = query.Where(i => i.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(i => i.Price <= maxPrice.Value);

            return await query
                .OrderByDescending(i => i.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<MarketItem>> SearchItemsAsync(string keyword, string? category = null, decimal? minPrice = null, decimal? maxPrice = null, int page = 1, int pageSize = 20)
        {
            var query = _dbSet
                .Include(i => i.Seller)
                .Include(i => i.Category)
                .Where(i => i.Status == "Active" && 
                           (i.Name.Contains(keyword) || i.Description.Contains(keyword)));

            if (!string.IsNullOrEmpty(category))
                query = query.Where(i => i.Category.Name == category);

            if (minPrice.HasValue)
                query = query.Where(i => i.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(i => i.Price <= maxPrice.Value);

            return await query
                .OrderByDescending(i => i.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<MarketItem>> GetBySellerAsync(int sellerId, int page = 1, int pageSize = 20)
        {
            return await _dbSet
                .Include(i => i.Category)
                .Where(i => i.SellerId == sellerId)
                .OrderByDescending(i => i.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<MarketItem?> GetByIdWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(i => i.Seller)
                .Include(i => i.Category)
                .Include(i => i.Images)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<bool> UpdateStatusAsync(int id, string status)
        {
            var item = await _dbSet.FindAsync(id);
            if (item != null)
            {
                item.Status = status;
                item.UpdatedAt = DateTime.UtcNow;
                _dbSet.Update(item);
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<MarketItem>> GetPopularItemsAsync(int limit = 10)
        {
            return await _dbSet
                .Include(i => i.Seller)
                .Include(i => i.Category)
                .Where(i => i.Status == "Active")
                .OrderByDescending(i => i.ViewCount)
                .Take(limit)
                .ToListAsync();
        }
    }
} 