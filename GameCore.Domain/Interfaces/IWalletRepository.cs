using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// 錢包 Repository 介面
    /// </summary>
    public interface IWalletRepository : IRepository<UserWallet>
    {
        /// <summary>
        /// 根據使用者ID取得錢包
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <returns>錢包實體</returns>
        Task<UserWallet?> GetByUserIdAsync(int userId);

        /// <summary>
        /// 更新餘額
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="amount">金額</param>
        /// <returns>操作結果</returns>
        Task<bool> UpdateBalanceAsync(int userId, decimal amount);

        /// <summary>
        /// 凍結餘額
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="amount">金額</param>
        /// <returns>操作結果</returns>
        Task<bool> FreezeBalanceAsync(int userId, decimal amount);

        /// <summary>
        /// 解凍餘額
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="amount">金額</param>
        /// <returns>操作結果</returns>
        Task<bool> UnfreezeBalanceAsync(int userId, decimal amount);

        /// <summary>
        /// 檢查餘額是否足夠
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="amount">金額</param>
        /// <returns>是否足夠</returns>
        Task<bool> CheckBalanceAsync(int userId, decimal amount);
    }
} 
