using GameCore.Core.DTOs;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// 錢包服務介面
    /// </summary>
    public interface IWalletService
    {
        /// <summary>
        /// 取得用戶錢包餘額
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns>錢包餘額資訊</returns>
        Task<WalletBalanceResponse> GetWalletBalanceAsync(int userId);

        /// <summary>
        /// 查詢點數流水記錄
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="request">查詢條件</param>
        /// <returns>點數流水記錄</returns>
        Task<PointTransactionQueryResponse> GetPointTransactionsAsync(int userId, PointTransactionQueryRequest request);

        /// <summary>
        /// 調整用戶點數（管理員功能）
        /// </summary>
        /// <param name="request">調整請求</param>
        /// <returns>調整結果</returns>
        Task<AdminPointAdjustmentResponse> AdjustUserPointsAsync(AdminPointAdjustmentRequest request);

        /// <summary>
        /// 取得銷售錢包資訊
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns>銷售錢包資訊</returns>
        Task<SalesWalletInfo> GetSalesWalletInfoAsync(int userId);

        /// <summary>
        /// 申請銷售權限
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="request">申請資訊</param>
        /// <returns>申請結果</returns>
        Task<SalesPermissionResponse> ApplySalesPermissionAsync(int userId, SalesPermissionRequest request);

        /// <summary>
        /// 取得銷售權限申請狀態
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns>申請狀態</returns>
        Task<SalesPermissionResponse?> GetSalesPermissionStatusAsync(int userId);

        /// <summary>
        /// 檢查用戶是否有銷售權限
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns>是否有銷售權限</returns>
        Task<bool> HasSalesAuthorityAsync(int userId);

        /// <summary>
        /// 轉移點數到銷售錢包
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="amount">轉移金額</param>
        /// <returns>是否成功</returns>
        Task<bool> TransferToSalesWalletAsync(int userId, int amount);

        /// <summary>
        /// 從銷售錢包提領點數
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="amount">提領金額</param>
        /// <returns>是否成功</returns>
        Task<bool> WithdrawFromSalesWalletAsync(int userId, int amount);
    }
} 
