using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// 市場評價 Repository 介面
    /// </summary>
    public interface IMarketReviewRepository : IRepository<MarketReview>
    {
        /// <summary>
        /// 根據交易ID取得評價
        /// </summary>
        Task<MarketReview?> GetByTransactionIdAsync(int transactionId);

        /// <summary>
        /// 根據被評價者ID取得評價
        /// </summary>
        Task<IEnumerable<MarketReview>> GetByRevieweeIdAsync(int revieweeId);

        /// <summary>
        /// 根據評價者ID取得評價
        /// </summary>
        Task<IEnumerable<MarketReview>> GetByReviewerIdAsync(int reviewerId);

        /// <summary>
        /// 取得被評價者的平均評分
        /// </summary>
        Task<decimal> GetAverageRatingByRevieweeIdAsync(int revieweeId);

        /// <summary>
        /// 新增評價
        /// </summary>
        Task<MarketReview> AddAsync(MarketReview review);

        /// <summary>
        /// 更新評價
        /// </summary>
        Task UpdateAsync(MarketReview review);

        /// <summary>
        /// 刪除評價
        /// </summary>
        Task DeleteAsync(MarketReview review);

        /// <summary>
        /// 新增評價
        /// </summary>
        Task<MarketReview> Add(MarketReview review);

        /// <summary>
        /// 根據被評價者ID取得評價（帶分頁）
        /// </summary>
        Task<IEnumerable<MarketReview>> GetByRevieweeIdAsync(int revieweeId, int page, int pageSize);
    }
} 
