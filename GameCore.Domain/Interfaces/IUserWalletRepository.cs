using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// �Τ���] Repository ����
    /// </summary>
    public interface IUserWalletRepository : IRepository<UserWallet>
    {
        /// <summary>
        /// �ھڥΤ�ID���o���]
        /// </summary>
        Task<UserWallet?> GetByUserIdAsync(int userId);

        /// <summary>
        /// �s�W���]
        /// </summary>
        Task<UserWallet> AddAsync(UserWallet wallet);

        /// <summary>
        /// ��s���]
        /// </summary>
        Task UpdateAsync(UserWallet wallet);

        /// <summary>
        /// �R�����]
        /// </summary>
        Task DeleteAsync(UserWallet wallet);
    }
} 
