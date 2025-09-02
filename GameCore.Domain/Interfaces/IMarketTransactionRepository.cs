using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// ������� Repository ����
    /// </summary>
    public interface IMarketTransactionRepository : IRepository<MarketTransaction>
    {
        /// <summary>
        /// �ھڥΤ�ID���o���
        /// </summary>
        Task<IEnumerable<MarketTransaction>> GetByUserIdAsync(int userId);

        /// <summary>
        /// �ھڪ��A���o���
        /// </summary>
        Task<IEnumerable<MarketTransaction>> GetByStatusAsync(string status);

        /// <summary>
        /// �s�W���
        /// </summary>
        Task<MarketTransaction> AddAsync(MarketTransaction transaction);

        /// <summary>
        /// ��s���
        /// </summary>
        Task UpdateAsync(MarketTransaction transaction);

        /// <summary>
        /// �R�����
        /// </summary>
        Task DeleteAsync(MarketTransaction transaction);

        /// <summary>
        /// �s�W���
        /// </summary>
        Task<MarketTransaction> Add(MarketTransaction transaction);
    }
} 
