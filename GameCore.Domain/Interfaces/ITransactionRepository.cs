using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// ��� Repository ����
    /// </summary>
    public interface ITransactionRepository : IRepository<Transaction>
    {
        /// <summary>
        /// �ھڨϥΪ�ID���o����O��
        /// </summary>
        /// <param name="userId">�ϥΪ�ID</param>
        /// <param name="page">���X</param>
        /// <param name="pageSize">�C���ƶq</param>
        /// <returns>����O���C��</returns>
        Task<IEnumerable<Transaction>> GetByUserIdAsync(int userId, int page = 1, int pageSize = 20);

        /// <summary>
        /// �ھڥ���������o����O��
        /// </summary>
        /// <param name="transactionType">�������</param>
        /// <param name="page">���X</param>
        /// <param name="pageSize">�C���ƶq</param>
        /// <returns>����O���C��</returns>
        Task<IEnumerable<Transaction>> GetByTypeAsync(string transactionType, int page = 1, int pageSize = 20);

        /// <summary>
        /// �ھڤ���d����o����O��
        /// </summary>
        /// <param name="startDate">�}�l���</param>
        /// <param name="endDate">�������</param>
        /// <param name="page">���X</param>
        /// <param name="pageSize">�C���ƶq</param>
        /// <returns>����O���C��</returns>
        Task<IEnumerable<Transaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, int page = 1, int pageSize = 20);

        /// <summary>
        /// �ھڪ��B�d����o����O��
        /// </summary>
        /// <param name="minAmount">�̤p���B</param>
        /// <param name="maxAmount">�̤j���B</param>
        /// <param name="page">���X</param>
        /// <param name="pageSize">�C���ƶq</param>
        /// <returns>����O���C��</returns>
        Task<IEnumerable<Transaction>> GetByAmountRangeAsync(decimal minAmount, decimal maxAmount, int page = 1, int pageSize = 20);

        /// <summary>
        /// ���o�ϥΪ̪��`������B
        /// </summary>
        /// <param name="userId">�ϥΪ�ID</param>
        /// <returns>�`���B</returns>
        Task<decimal> GetTotalAmountByUserAsync(int userId);

        /// <summary>
        /// ���o�ϥΪ̪�����έp
        /// </summary>
        /// <param name="userId">�ϥΪ�ID</param>
        /// <returns>����έp</returns>
        Task<object> GetTransactionStatsAsync(int userId);
    }
} 
