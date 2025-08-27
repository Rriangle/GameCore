using GameCore.Core.Entities;
using GameCore.Core.DTOs;

namespace GameCore.Core.Interfaces
{
    /// <summary>
    /// 交易倉庫介面
    /// </summary>
    public interface ITransactionRepository : IRepository<WalletTransaction>
    {
        /// <summary>
        /// 根據用戶ID獲取交易記錄
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="page">頁數</param>
        /// <param name="pageSize">每頁數量</param>
        /// <returns>交易記錄</returns>
        Task<IEnumerable<WalletTransaction>> GetByUserIdAsync(int userId, int page = 1, int pageSize = 20);

        /// <summary>
        /// 根據類型獲取交易記錄
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="transactionType">交易類型</param>
        /// <param name="page">頁數</param>
        /// <param name="pageSize">每頁數量</param>
        /// <returns>交易記錄</returns>
        Task<IEnumerable<WalletTransaction>> GetByTypeAsync(int userId, string transactionType, int page = 1, int pageSize = 20);
    }
}