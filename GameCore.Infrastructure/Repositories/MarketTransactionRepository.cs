using GameCore.Domain.Entities;
using GameCore.Domain.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameCore.Infrastructure.Repositories
{
    public class MarketTransactionRepository : Repository<MarketTransaction>, IMarketTransactionRepository
    {
        private readonly ILogger<MarketTransactionRepository> _logger;

        public MarketTransactionRepository(GameCoreDbContext context, ILogger<MarketTransactionRepository> logger) : base(context)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<MarketTransaction>> GetByUserIdAsync(int userId)
        {
            return await _dbSet
                .Include(t => t.Buyer)
                .Include(t => t.Seller)
                .Include(t => t.Item)
                .Where(t => t.BuyerId == userId || t.SellerId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<MarketTransaction>> GetByStatusAsync(string status)
        {
            return await _dbSet
                .Include(t => t.Buyer)
                .Include(t => t.Seller)
                .Include(t => t.Item)
                .Where(t => t.Status == status)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<MarketTransaction> AddAsync(MarketTransaction transaction)
        {
            var result = await _dbSet.AddAsync(transaction);
            return result.Entity;
        }

        public Task UpdateAsync(MarketTransaction transaction)
        {
            _dbSet.Update(transaction);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(MarketTransaction transaction)
        {
            _dbSet.Remove(transaction);
            return Task.CompletedTask;
        }

        public async Task<MarketTransaction> Add(MarketTransaction transaction)
        {
            var result = await _dbSet.AddAsync(transaction);
            return result.Entity;
        }
    }
} 