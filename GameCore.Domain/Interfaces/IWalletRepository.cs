using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// ���] Repository ����
    /// </summary>
    public interface IWalletRepository : IRepository<UserWallet>
    {
        /// <summary>
        /// �ھڨϥΪ�ID���o���]
        /// </summary>
        /// <param name="userId">�ϥΪ�ID</param>
        /// <returns>���]����</returns>
        Task<UserWallet?> GetByUserIdAsync(int userId);

        /// <summary>
        /// ��s�l�B
        /// </summary>
        /// <param name="userId">�ϥΪ�ID</param>
        /// <param name="amount">���B</param>
        /// <returns>�ާ@���G</returns>
        Task<bool> UpdateBalanceAsync(int userId, decimal amount);

        /// <summary>
        /// �ᵲ�l�B
        /// </summary>
        /// <param name="userId">�ϥΪ�ID</param>
        /// <param name="amount">���B</param>
        /// <returns>�ާ@���G</returns>
        Task<bool> FreezeBalanceAsync(int userId, decimal amount);

        /// <summary>
        /// �ѭ�l�B
        /// </summary>
        /// <param name="userId">�ϥΪ�ID</param>
        /// <param name="amount">���B</param>
        /// <returns>�ާ@���G</returns>
        Task<bool> UnfreezeBalanceAsync(int userId, decimal amount);

        /// <summary>
        /// �ˬd�l�B�O�_����
        /// </summary>
        /// <param name="userId">�ϥΪ�ID</param>
        /// <param name="amount">���B</param>
        /// <returns>�O�_����</returns>
        Task<bool> CheckBalanceAsync(int userId, decimal amount);
    }
} 
