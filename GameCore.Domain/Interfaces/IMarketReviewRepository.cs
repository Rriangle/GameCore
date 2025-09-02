using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// �������� Repository ����
    /// </summary>
    public interface IMarketReviewRepository : IRepository<MarketReview>
    {
        /// <summary>
        /// �ھڥ��ID���o����
        /// </summary>
        Task<MarketReview?> GetByTransactionIdAsync(int transactionId);

        /// <summary>
        /// �ھڳQ������ID���o����
        /// </summary>
        Task<IEnumerable<MarketReview>> GetByRevieweeIdAsync(int revieweeId);

        /// <summary>
        /// �ھڵ�����ID���o����
        /// </summary>
        Task<IEnumerable<MarketReview>> GetByReviewerIdAsync(int reviewerId);

        /// <summary>
        /// ���o�Q�����̪���������
        /// </summary>
        Task<decimal> GetAverageRatingByRevieweeIdAsync(int revieweeId);

        /// <summary>
        /// �s�W����
        /// </summary>
        Task<MarketReview> AddAsync(MarketReview review);

        /// <summary>
        /// ��s����
        /// </summary>
        Task UpdateAsync(MarketReview review);

        /// <summary>
        /// �R������
        /// </summary>
        Task DeleteAsync(MarketReview review);

        /// <summary>
        /// �s�W����
        /// </summary>
        Task<MarketReview> Add(MarketReview review);

        /// <summary>
        /// �ھڳQ������ID���o�����]�a�����^
        /// </summary>
        Task<IEnumerable<MarketReview>> GetByRevieweeIdAsync(int revieweeId, int page, int pageSize);
    }
} 
