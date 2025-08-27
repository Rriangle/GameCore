using GameCore.Core.Entities;

namespace GameCore.Core.Interfaces
{
    /// <summary>
    /// 錢包倉庫介面
    /// </summary>
    public interface IWalletRepository : IRepository<UserWallet>
    {
        /// <summary>
        /// 根據用戶ID獲取錢包
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns>錢包實體</returns>
        Task<UserWallet?> GetByUserIdAsync(int userId);

        /// <summary>
        /// 更新錢包餘額
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="amount">變動金額</param>
        /// <returns>更新結果</returns>
        Task<bool> UpdateBalanceAsync(int userId, decimal amount);

        /// <summary>
        /// 鎖定錢包進行交易
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns>鎖定結果</returns>
        Task<bool> LockWalletAsync(int userId);

        /// <summary>
        /// 解鎖錢包
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns>解鎖結果</returns>
        Task<bool> UnlockWalletAsync(int userId);
    }
}