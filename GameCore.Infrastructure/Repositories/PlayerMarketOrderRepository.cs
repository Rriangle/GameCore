using GameCore.Domain.Entities;
using GameCore.Domain.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameCore.Infrastructure.Repositories
{
    public class PlayerMarketOrderRepository : Repository<PlayerMarketOrderInfo>, IPlayerMarketOrderRepository
    {
        private readonly ILogger<PlayerMarketOrderRepository> _logger;

        public PlayerMarketOrderRepository(GameCoreDbContext context, ILogger<PlayerMarketOrderRepository> logger) : base(context)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<PlayerMarketOrderInfo>> GetByBuyerIdAsync(int buyerId)
        {
            return await _dbSet
                .Include(o => o.Buyer)
                .Include(o => o.Seller)
                .Include(o => o.Product)
                .Where(o => o.BuyerId == buyerId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<PlayerMarketOrderInfo>> GetBySellerIdAsync(int sellerId)
        {
            return await _dbSet
                .Include(o => o.Buyer)
                .Include(o => o.Seller)
                .Include(o => o.Product)
                .Where(o => o.SellerId == sellerId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<PlayerMarketOrderInfo>> GetByStatusAsync(string status)
        {
            return await _dbSet
                .Include(o => o.Buyer)
                .Include(o => o.Seller)
                .Include(o => o.Product)
                .Where(o => o.Status == status)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<PlayerMarketOrderInfo> AddAsync(PlayerMarketOrderInfo order)
        {
            var result = await _dbSet.AddAsync(order);
            return result.Entity;
        }

        public Task UpdateAsync(PlayerMarketOrderInfo order)
        {
            _dbSet.Update(order);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(PlayerMarketOrderInfo order)
        {
            _dbSet.Remove(order);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<PlayerMarketOrderInfo>> GetAll()
        {
            return await _dbSet
                .Include(o => o.Buyer)
                .Include(o => o.Seller)
                .Include(o => o.Product)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }
    }
} 