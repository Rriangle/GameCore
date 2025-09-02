using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// ���a�����q��ܮw����
    /// </summary>
    public interface IPlayerMarketOrderInfoRepository : IRepository<PlayerMarketOrderInfo>
    {
        /// <summary>
        /// ���o�R�a���q��
        /// </summary>
        /// <param name="buyerId">�R�aID</param>
        /// <param name="page">���X</param>
        /// <param name="pageSize">�C���j�p</param>
        /// <returns>�q��C��</returns>
        Task<IEnumerable<PlayerMarketOrderInfo>> GetByBuyerAsync(int buyerId, int page = 1, int pageSize = 20);

        /// <summary>
        /// ���o��a���q��
        /// </summary>
        /// <param name="sellerId">��aID</param>
        /// <param name="page">���X</param>
        /// <param name="pageSize">�C���j�p</param>
        /// <returns>�q��C��</returns>
        Task<IEnumerable<PlayerMarketOrderInfo>> GetBySellerAsync(int sellerId, int page = 1, int pageSize = 20);

        /// <summary>
        /// ���o�q��Ա�
        /// </summary>
        /// <param name="orderId">�q��ID</param>
        /// <returns>�q��Ա�</returns>
        Task<PlayerMarketOrderInfo?> GetByIdWithDetailsAsync(int orderId);

        /// <summary>
        /// ��s�q�檬�A
        /// </summary>
        /// <param name="orderId">�q��ID</param>
        /// <param name="status">�s���A</param>
        /// <returns>��s���G</returns>
        Task<bool> UpdateStatusAsync(int orderId, string status);

        /// <summary>
        /// ���o�ݳB�z���q��
        /// </summary>
        /// <param name="sellerId">��aID</param>
        /// <returns>�ݳB�z�q��C��</returns>
        Task<IEnumerable<PlayerMarketOrderInfo>> GetPendingOrdersAsync(int sellerId);

        /// <summary>
        /// ���o�w�������q��
        /// </summary>
        /// <param name="userId">�Τ�ID</param>
        /// <param name="page">���X</param>
        /// <param name="pageSize">�C���j�p</param>
        /// <returns>�w�����q��C��</returns>
        Task<IEnumerable<PlayerMarketOrderInfo>> GetCompletedOrdersAsync(int userId, int page = 1, int pageSize = 20);
    }
} 
