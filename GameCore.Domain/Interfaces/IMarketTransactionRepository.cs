using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// 市場交易 Repository 介面
    /// </summary>
    public interface IMarketTransactionRepository : IRepository<MarketTransaction>
    {
        /// <summary>
        /// 根據用戶ID取得交易
        /// </summary>
        Task<IEnumerable<MarketTransaction>> GetByUserIdAsync(int userId);

        /// <summary>
        /// 根據狀態取得交易
        /// </summary>
        Task<IEnumerable<MarketTransaction>> GetByStatusAsync(string status);

        /// <summary>
        /// 新增交易
        /// </summary>
        Task<MarketTransaction> AddAsync(MarketTransaction transaction);

        /// <summary>
        /// 更新交易
        /// </summary>
        Task UpdateAsync(MarketTransaction transaction);

        /// <summary>
        /// 刪除交易
        /// </summary>
        Task DeleteAsync(MarketTransaction transaction);

        /// <summary>
        /// 新增交易
        /// </summary>
        Task<MarketTransaction> Add(MarketTransaction transaction);
    }
} 
