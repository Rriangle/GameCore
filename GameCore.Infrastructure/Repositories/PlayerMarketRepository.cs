using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GameCore.Infrastructure.Repositories
{
    public class PlayerMarketRepository : Repository<MarketTransaction>, IPlayerMarketRepository
    {
        public PlayerMarketRepository(GameCoreDbContext context) : base(context)
        {
        }

        // MarketTransaction methods
        public async Task<IEnumerable<MarketTransaction>> GetActiveListingsAsync(int page = 1, int pageSize = 20)
        {
            return await _context.MarketTransactions
                .Include(mt => mt.Seller)
                .Include(mt => mt.Buyer)
                .Where(mt => mt.Status == MarketTransactionStatus.Listed)
                .OrderByDescending(mt => mt.CreateTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<MarketTransaction>> GetUserListingsAsync(int userId, int page = 1, int pageSize = 20)
        {
            return await _context.MarketTransactions
                .Include(mt => mt.Buyer)
                .Where(mt => mt.SellerId == userId)
                .OrderByDescending(mt => mt.CreateTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<MarketTransaction>> GetUserPurchasesAsync(int userId, int page = 1, int pageSize = 20)
        {
            return await _context.MarketTransactions
                .Include(mt => mt.Seller)
                .Where(mt => mt.BuyerId == userId)
                .OrderByDescending(mt => mt.CreateTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<MarketTransaction>> SearchListingsAsync(string keyword, decimal? minPrice = null, decimal? maxPrice = null, int page = 1, int pageSize = 20)
        {
            var query = _context.MarketTransactions
                .Include(mt => mt.Seller)
                .Include(mt => mt.Buyer)
                .Where(mt => mt.Status == MarketTransactionStatus.Listed);

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(mt => mt.ItemName.Contains(keyword) || mt.Description.Contains(keyword));
            }

            if (minPrice.HasValue)
            {
                query = query.Where(mt => mt.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(mt => mt.Price <= maxPrice.Value);
            }

            return await query
                .OrderByDescending(mt => mt.CreateTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<MarketTransaction?> GetTransactionWithDetailsAsync(int transactionId)
        {
            return await _context.MarketTransactions
                .Include(mt => mt.Seller)
                .Include(mt => mt.Buyer)
                .Include(mt => mt.Reviews)
                .FirstOrDefaultAsync(mt => mt.Id == transactionId);
        }

        public async Task<IEnumerable<MarketTransaction>> GetTransactionsByStatusAsync(MarketTransactionStatus status, int page = 1, int pageSize = 20)
        {
            return await _context.MarketTransactions
                .Include(mt => mt.Seller)
                .Include(mt => mt.Buyer)
                .Where(mt => mt.Status == status)
                .OrderByDescending(mt => mt.CreateTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<decimal> GetUserTotalSalesAsync(int userId)
        {
            return await _context.MarketTransactions
                .Where(mt => mt.SellerId == userId && mt.Status == MarketTransactionStatus.Completed)
                .SumAsync(mt => mt.Price);
        }

        public async Task<int> GetUserSalesCountAsync(int userId)
        {
            return await _context.MarketTransactions
                .CountAsync(mt => mt.SellerId == userId && mt.Status == MarketTransactionStatus.Completed);
        }

        public async Task<IEnumerable<MarketTransaction>> GetRecentTransactionsAsync(int days = 7)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-days);
            return await _context.MarketTransactions
                .Include(mt => mt.Seller)
                .Include(mt => mt.Buyer)
                .Where(mt => mt.CreateTime >= cutoffDate)
                .OrderByDescending(mt => mt.CreateTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<MarketTransaction>> GetPopularItemsAsync(int days = 30, int take = 10)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-days);
            return await _context.MarketTransactions
                .Include(mt => mt.Seller)
                .Where(mt => mt.CreateTime >= cutoffDate && mt.Status == MarketTransactionStatus.Completed)
                .GroupBy(mt => mt.ItemName)
                .OrderByDescending(g => g.Count())
                .Take(take)
                .SelectMany(g => g.Take(1))
                .ToListAsync();
        }

        // MarketReview methods
        public async Task<IEnumerable<MarketReview>> GetUserReviewsAsync(int userId, bool asReviewer = true)
        {
            var query = _context.MarketReviews
                .Include(mr => mr.Reviewer)
                .Include(mr => mr.Reviewee)
                .Include(mr => mr.Transaction);

            if (asReviewer)
            {
                query = query.Where(mr => mr.ReviewerId == userId);
            }
            else
            {
                query = query.Where(mr => mr.RevieweeId == userId);
            }

            return await query
                .OrderByDescending(mr => mr.CreateTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<MarketReview>> GetTransactionReviewsAsync(int transactionId)
        {
            return await _context.MarketReviews
                .Include(mr => mr.Reviewer)
                .Include(mr => mr.Reviewee)
                .Where(mr => mr.TransactionId == transactionId)
                .ToListAsync();
        }

        public async Task<double> GetUserAverageRatingAsync(int userId)
        {
            var reviews = await _context.MarketReviews
                .Where(mr => mr.RevieweeId == userId)
                .ToListAsync();

            if (!reviews.Any())
                return 0;

            return reviews.Average(mr => mr.Rating);
        }

        public async Task<int> GetUserReviewCountAsync(int userId)
        {
            return await _context.MarketReviews
                .CountAsync(mr => mr.RevieweeId == userId);
        }

        public async Task<bool> HasUserReviewedTransactionAsync(int userId, int transactionId)
        {
            return await _context.MarketReviews
                .AnyAsync(mr => mr.ReviewerId == userId && mr.TransactionId == transactionId);
        }

        public async Task<MarketReview?> GetReviewAsync(int reviewerId, int transactionId)
        {
            return await _context.MarketReviews
                .Include(mr => mr.Reviewer)
                .Include(mr => mr.Reviewee)
                .Include(mr => mr.Transaction)
                .FirstOrDefaultAsync(mr => mr.ReviewerId == reviewerId && mr.TransactionId == transactionId);
        }

        // Statistics methods
        public async Task<Dictionary<string, decimal>> GetMarketStatsAsync(int days = 30)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-days);
            var transactions = await _context.MarketTransactions
                .Where(mt => mt.CreateTime >= cutoffDate && mt.Status == MarketTransactionStatus.Completed)
                .ToListAsync();

            return new Dictionary<string, decimal>
            {
                ["TotalVolume"] = transactions.Sum(t => t.Price),
                ["TotalTransactions"] = transactions.Count,
                ["AveragePrice"] = transactions.Any() ? transactions.Average(t => t.Price) : 0,
                ["PlatformFee"] = transactions.Sum(t => t.PlatformFee ?? 0)
            };
        }

        public async Task<IEnumerable<object>> GetDailyVolumeAsync(int days = 30)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-days);
            return await _context.MarketTransactions
                .Where(mt => mt.CreateTime >= cutoffDate && mt.Status == MarketTransactionStatus.Completed)
                .GroupBy(mt => mt.CreateTime.Date)
                .Select(g => new 
                {
                    Date = g.Key,
                    Volume = g.Sum(mt => mt.Price),
                    Count = g.Count()
                })
                .OrderBy(x => x.Date)
                .ToListAsync();
        }
    }
}