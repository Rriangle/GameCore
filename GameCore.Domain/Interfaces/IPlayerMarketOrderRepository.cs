using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// ���a�����q�� Repository ����
    /// </summary>
    public interface IPlayerMarketOrderRepository : IRepository<PlayerMarketOrderInfo>
    {
        /// <summary>
        /// �ھڶR�aID���o�q��
        /// </summary>
        Task<IEnumerable<PlayerMarketOrderInfo>> GetByBuyerIdAsync(int buyerId);

        /// <summary>
        /// �ھڽ�aID���o�q��
        /// </summary>
        Task<IEnumerable<PlayerMarketOrderInfo>> GetBySellerIdAsync(int sellerId);

        /// <summary>
        /// �ھڪ��A���o�q��
        /// </summary>
        Task<IEnumerable<PlayerMarketOrderInfo>> GetByStatusAsync(string status);

        /// <summary>
        /// �s�W�q��
        /// </summary>
        Task<PlayerMarketOrderInfo> AddAsync(PlayerMarketOrderInfo order);

        /// <summary>
        /// ��s�q��
        /// </summary>
        Task UpdateAsync(PlayerMarketOrderInfo order);

        /// <summary>
        /// �R���q��
        /// </summary>
        Task DeleteAsync(PlayerMarketOrderInfo order);

        /// <summary>
        /// ���o�Ҧ��q��
        /// </summary>
        Task<IEnumerable<PlayerMarketOrderInfo>> GetAll();
    }
} 
