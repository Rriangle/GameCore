using GameCore.Core.Entities;
using GameCore.Core.DTOs;

namespace GameCore.Core.Interfaces
{
    /// <summary>
    /// 錢包服務介面
    /// </summary>
    public interface IWalletService
    {
        /// <summary>
        /// 獲取用戶錢包
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns>錢包資訊</returns>
        Task<UserWallet?> GetWalletAsync(int userId);

        /// <summary>
        /// 執行交易
        /// </summary>
        /// <param name="request">交易請求</param>
        /// <returns>交易結果</returns>
        Task<WalletTransactionResult> ExecuteTransactionAsync(WalletTransactionRequest request);

        /// <summary>
        /// 獲取交易歷史
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="page">頁數</param>
        /// <param name="pageSize">每頁數量</param>
        /// <returns>交易歷史</returns>
        Task<IEnumerable<WalletTransaction>> GetTransactionHistoryAsync(int userId, int page = 1, int pageSize = 20);

        /// <summary>
        /// 檢查餘額是否足夠
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="amount">金額</param>
        /// <returns>是否足夠</returns>
        Task<bool> HasSufficientBalanceAsync(int userId, decimal amount);

        /// <summary>
        /// 更新錢包餘額
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="amount">變動金額</param>
        /// <param name="reason">變動原因</param>
        /// <returns>更新結果</returns>
        Task<bool> UpdateBalanceAsync(int userId, decimal amount, string reason);
    }
}