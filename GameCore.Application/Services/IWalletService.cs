using GameCore.Application.Common;
using GameCore.Application.DTOs;

namespace GameCore.Application.Services
{
    /// <summary>
    /// 錢包服務介面
    /// </summary>
    public interface IWalletService
    {
        /// <summary>
        /// 取得用戶錢包餘額
        /// </summary>
        /// <param name="userId">用戶 ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>錢包餘額</returns>
        Task<Result<BalanceResponse>> GetBalanceAsync(int userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// 存款
        /// </summary>
        /// <param name="userId">用戶 ID</param>
        /// <param name="amount">金額</param>
        /// <param name="description">描述</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>存款結果</returns>
        Task<Result<TransactionResponse>> DepositAsync(int userId, decimal amount, string description, CancellationToken cancellationToken = default);

        /// <summary>
        /// 提款
        /// </summary>
        /// <param name="userId">用戶 ID</param>
        /// <param name="amount">金額</param>
        /// <param name="description">描述</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>提款結果</returns>
        Task<Result<TransactionResponse>> WithdrawAsync(int userId, decimal amount, string description, CancellationToken cancellationToken = default);

        /// <summary>
        /// 取得交易記錄
        /// </summary>
        /// <param name="userId">用戶 ID</param>
        /// <param name="pageNumber">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>交易記錄</returns>
        Task<Result<PagedResult<TransactionResponse>>> GetTransactionHistoryAsync(int userId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);

        /// <summary>
        /// 轉帳
        /// </summary>
        /// <param name="fromUserId">轉出用戶 ID</param>
        /// <param name="toUserId">轉入用戶 ID</param>
        /// <param name="amount">金額</param>
        /// <param name="description">描述</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>轉帳結果</returns>
        Task<Result<TransactionResponse>> TransferAsync(int fromUserId, int toUserId, decimal amount, string description, CancellationToken cancellationToken = default);

        /// <summary>
        /// 取得交易統計
        /// </summary>
        /// <param name="userId">用戶 ID</param>
        /// <param name="startDate">開始日期</param>
        /// <param name="endDate">結束日期</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>交易統計</returns>
        Task<Result<TransactionStatsResponse>> GetTransactionStatsAsync(int userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// 餘額回應
    /// </summary>
    public class BalanceResponse
    {
        /// <summary>
        /// 用戶 ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 當前餘額
        /// </summary>
        public decimal CurrentBalance { get; set; }

        /// <summary>
        /// 總存款
        /// </summary>
        public decimal TotalDeposits { get; set; }

        /// <summary>
        /// 總提款
        /// </summary>
        public decimal TotalWithdrawals { get; set; }

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime LastUpdated { get; set; }
    }

    /// <summary>
    /// 交易回應
    /// </summary>
    public class TransactionResponse
    {
        /// <summary>
        /// 交易 ID
        /// </summary>
        public int TransactionId { get; set; }

        /// <summary>
        /// 用戶 ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 交易類型
        /// </summary>
        public string TransactionType { get; set; } = string.Empty;

        /// <summary>
        /// 金額
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 餘額
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 交易時間
        /// </summary>
        public DateTime TransactionTime { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public string Status { get; set; } = string.Empty;
    }

    /// <summary>
    /// 交易統計回應
    /// </summary>
    public class TransactionStatsResponse
    {
        /// <summary>
        /// 總交易數
        /// </summary>
        public int TotalTransactions { get; set; }

        /// <summary>
        /// 總存款
        /// </summary>
        public decimal TotalDeposits { get; set; }

        /// <summary>
        /// 總提款
        /// </summary>
        public decimal TotalWithdrawals { get; set; }

        /// <summary>
        /// 淨變化
        /// </summary>
        public decimal NetChange { get; set; }

        /// <summary>
        /// 平均交易金額
        /// </summary>
        public decimal AverageTransactionAmount { get; set; }

        /// <summary>
        /// 最大交易金額
        /// </summary>
        public decimal MaxTransactionAmount { get; set; }

        /// <summary>
        /// 最小交易金額
        /// </summary>
        public decimal MinTransactionAmount { get; set; }
    }
} 