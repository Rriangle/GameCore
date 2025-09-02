using GameCore.Domain.Entities;
using GameCore.Domain.Interfaces;
using GameCore.Infrastructure.Data;
using GameCore.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameCore.Infrastructure.Repositories
{
    /// <summary>
    /// 玩家市場倉庫實作
    /// </summary>
    public class PlayerMarketRepository : Repository<PlayerMarketProductInfo>, IPlayerMarketRepository
    {
        private readonly ILogger<PlayerMarketRepository> _logger;

        public PlayerMarketRepository(GameCoreDbContext context, ILogger<PlayerMarketRepository> logger) : base(context)
        {
            _logger = logger;
        }

        // IPlayerMarketRepository 接口方法實現
        public async Task<IEnumerable<PlayerMarketProductInfo>> GetMarketItemsAsync()
        {
            return await _context.PlayerMarketProductInfos
                .Include(p => p.Seller)
                .Where(p => p.Status == "Active")
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<PlayerMarketProductInfo?> GetMarketItemByIdAsync(int id)
        {
            return await _context.PlayerMarketProductInfos
                .Include(p => p.Seller)
                .FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task<IEnumerable<PlayerMarketProductInfo>> SearchMarketItemsAsync(string keyword)
        {
            return await _context.PlayerMarketProductInfos
                .Include(p => p.Seller)
                .Where(p => p.Status == "Active" && 
                           (p.ProductName.Contains(keyword) || p.Description.Contains(keyword)))
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<PlayerMarketProductInfo>> GetMarketItemsByCategoryAsync(string category)
        {
            return await _context.PlayerMarketProductInfos
                .Include(p => p.Seller)
                .Where(p => p.Status == "Active" && p.Category == category)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<PlayerMarketProductInfo>> GetBySellerIdAsync(int sellerId)
        {
            return await _context.PlayerMarketProductInfos
                .Include(p => p.Seller)
                .Where(p => p.SellerId == sellerId && p.Status == "Active")
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetCategoriesAsync()
        {
            return await _context.PlayerMarketProductInfos
                .Where(p => p.Status == "Active")
                .Select(p => p.Category)
                .Distinct()
                .ToListAsync();
        }

        public async Task<PlayerMarketProductInfo> AddAsync(PlayerMarketProductInfo item)
        {
            await _context.PlayerMarketProductInfos.AddAsync(item);
            return item;
        }

        public async Task<PlayerMarketProductInfo> UpdateAsync(PlayerMarketProductInfo item)
        {
            _context.PlayerMarketProductInfos.Update(item);
            return item;
        }

        public async Task DeleteAsync(int id)
        {
            var item = await GetMarketItemByIdAsync(id);
            if (item != null)
            {
                item.Status = "Deleted";
                _context.PlayerMarketProductInfos.Update(item);
            }
        }

        public async Task<bool> ConfirmTransactionAsync(int transactionId)
        {
            // 這裡需要實現交易確認邏輯
            throw new NotImplementedException();
        }

        public async Task<bool> CancelTransactionAsync(int transactionId)
        {
            // 這裡需要實現交易取消邏輯
            throw new NotImplementedException();
        }

        public async Task<bool> ReviewTransactionAsync(int transactionId, int rating, string comment)
        {
            // 這裡需要實現交易評價邏輯
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PlayerMarketProductInfo>> GetMarketItemsAsync(int page, int pageSize)
        {
            return await _context.PlayerMarketProductInfos
                .Include(p => p.Seller)
                .Where(p => p.Status == "Active")
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<PlayerMarketProductInfo>> SearchMarketItemsAsync(string keyword, int page, int pageSize)
        {
            return await _context.PlayerMarketProductInfos
                .Include(p => p.Seller)
                .Where(p => p.Status == "Active" && 
                           (p.ProductName.Contains(keyword) || p.Description.Contains(keyword)))
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<PlayerMarketProductInfo>> GetMarketItemsByCategoryAsync(string category, int page, int pageSize)
        {
            return await _context.PlayerMarketProductInfos
                .Include(p => p.Seller)
                .Where(p => p.Status == "Active" && p.Category == category)
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<PlayerMarketProductInfo> Add(PlayerMarketProductInfo item)
        {
            return await AddAsync(item);
        }

        public async Task<PlayerMarketProductInfo> Update(PlayerMarketProductInfo item)
        {
            return await UpdateAsync(item);
        }

        public async Task<IEnumerable<PlayerMarketProductInfo>> GetActiveItemsAsync()
        {
            return await GetMarketItemsAsync();
        }

        public async Task<IEnumerable<PlayerMarketProductInfo>> SearchItemsAsync(string keyword)
        {
            return await SearchMarketItemsAsync(keyword);
        }

        public async Task<PlayerMarketProductInfo?> GetByIdAsync(int id)
        {
            return await GetMarketItemByIdAsync(id);
        }

        public async Task<IEnumerable<PlayerMarketProductInfo>> GetBySellerIdAsync(int sellerId, int page, int pageSize)
        {
            return await _context.PlayerMarketProductInfos
                .Include(p => p.Seller)
                .Where(p => p.SellerId == sellerId && p.Status == "Active")
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<bool> ConfirmTransactionAsync(int transactionId, int itemId)
        {
            // 這裡需要實現交易確認邏輯
            throw new NotImplementedException();
        }

        public async Task<bool> CancelTransactionAsync(int transactionId, int itemId)
        {
            // 這裡需要實現交易取消邏輯
            throw new NotImplementedException();
        }

        // 保留原有的 MarketTransaction 相關方法作為內部實現
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
    }
}