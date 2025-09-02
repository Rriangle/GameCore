using GameCore.Domain.Entities;
using GameCore.Domain.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameCore.Infrastructure.Repositories
{
    public class MarketReviewRepository : Repository<MarketReview>, IMarketReviewRepository
    {
        private readonly ILogger<MarketReviewRepository> _logger;

        public MarketReviewRepository(GameCoreDbContext context, ILogger<MarketReviewRepository> logger) : base(context)
        {
            _logger = logger;
        }

        public async Task<MarketReview?> GetByTransactionIdAsync(int transactionId)
        {
            return await _dbSet
                .Include(r => r.Reviewer)
                .Include(r => r.Reviewee)
                .FirstOrDefaultAsync(r => r.TransactionId == transactionId);
        }

        public async Task<IEnumerable<MarketReview>> GetByRevieweeIdAsync(int revieweeId)
        {
            return await _dbSet
                .Include(r => r.Reviewer)
                .Where(r => r.RevieweeId == revieweeId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<MarketReview>> GetByReviewerIdAsync(int reviewerId)
        {
            return await _dbSet
                .Include(r => r.Reviewee)
                .Where(r => r.ReviewerId == reviewerId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<decimal> GetAverageRatingByRevieweeIdAsync(int revieweeId)
        {
            var reviews = await _dbSet
                .Where(r => r.RevieweeId == revieweeId)
                .Select(r => r.Rating)
                .ToListAsync();

            return reviews.Any() ? reviews.Average() : 0;
        }

        public async Task<MarketReview> AddAsync(MarketReview review)
        {
            var result = await _dbSet.AddAsync(review);
            return result.Entity;
        }

        public Task UpdateAsync(MarketReview review)
        {
            _dbSet.Update(review);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(MarketReview review)
        {
            _dbSet.Remove(review);
            return Task.CompletedTask;
        }

        public async Task<MarketReview> Add(MarketReview review)
        {
            var result = await _dbSet.AddAsync(review);
            return result.Entity;
        }

        public async Task<IEnumerable<MarketReview>> GetByRevieweeIdAsync(int revieweeId, int page, int pageSize)
        {
            return await _dbSet
                .Include(r => r.Reviewer)
                .Where(r => r.RevieweeId == revieweeId)
                .OrderByDescending(r => r.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
} 