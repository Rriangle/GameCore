using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// 交易 Repository 介面
    /// </summary>
    public interface ITransactionRepository : IRepository<Transaction>
    {
        /// <summary>
        /// 根據使用者ID取得交易記錄
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁數量</param>
        /// <returns>交易記錄列表</returns>
        Task<IEnumerable<Transaction>> GetByUserIdAsync(int userId, int page = 1, int pageSize = 20);

        /// <summary>
        /// 根據交易類型取得交易記錄
        /// </summary>
        /// <param name="transactionType">交易類型</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁數量</param>
        /// <returns>交易記錄列表</returns>
        Task<IEnumerable<Transaction>> GetByTypeAsync(string transactionType, int page = 1, int pageSize = 20);

        /// <summary>
        /// 根據日期範圍取得交易記錄
        /// </summary>
        /// <param name="startDate">開始日期</param>
        /// <param name="endDate">結束日期</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁數量</param>
        /// <returns>交易記錄列表</returns>
        Task<IEnumerable<Transaction>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, int page = 1, int pageSize = 20);

        /// <summary>
        /// 根據金額範圍取得交易記錄
        /// </summary>
        /// <param name="minAmount">最小金額</param>
        /// <param name="maxAmount">最大金額</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁數量</param>
        /// <returns>交易記錄列表</returns>
        Task<IEnumerable<Transaction>> GetByAmountRangeAsync(decimal minAmount, decimal maxAmount, int page = 1, int pageSize = 20);

        /// <summary>
        /// 取得使用者的總交易金額
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <returns>總金額</returns>
        Task<decimal> GetTotalAmountByUserAsync(int userId);

        /// <summary>
        /// 取得使用者的交易統計
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <returns>交易統計</returns>
        Task<object> GetTransactionStatsAsync(int userId);
    }
} 
