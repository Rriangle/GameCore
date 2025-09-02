using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// 用戶錢包 Repository 介面
    /// </summary>
    public interface IUserWalletRepository : IRepository<UserWallet>
    {
        /// <summary>
        /// 根據用戶ID取得錢包
        /// </summary>
        Task<UserWallet?> GetByUserIdAsync(int userId);

        /// <summary>
        /// 新增錢包
        /// </summary>
        Task<UserWallet> AddAsync(UserWallet wallet);

        /// <summary>
        /// 更新錢包
        /// </summary>
        Task UpdateAsync(UserWallet wallet);

        /// <summary>
        /// 刪除錢包
        /// </summary>
        Task DeleteAsync(UserWallet wallet);
    }
} 
