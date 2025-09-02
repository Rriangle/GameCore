using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// �q�� Repository ���f
    /// </summary>
    public interface IOrderRepository : IRepository<Order>
    {
        /// <summary>
        /// �ھڥΤ�ID����q��
        /// </summary>
        /// <param name="userId">�Τ�ID</param>
        /// <returns>�q��C��</returns>
        Task<IEnumerable<Order>> GetByUserIdAsync(int userId);

        /// <summary>
        /// �ھڪ��A����q��
        /// </summary>
        /// <param name="status">�q�檬�A</param>
        /// <returns>�q��C��</returns>
        Task<IEnumerable<Order>> GetByStatusAsync(string status);

        /// <summary>
        /// ����ݳB�z�q��
        /// </summary>
        /// <returns>�q��C��</returns>
        Task<IEnumerable<Order>> GetPendingOrdersAsync();
    }
} 
