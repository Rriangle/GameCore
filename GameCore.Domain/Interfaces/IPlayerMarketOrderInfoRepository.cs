using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// 玩家市場訂單倉庫介面
    /// </summary>
    public interface IPlayerMarketOrderInfoRepository : IRepository<PlayerMarketOrderInfo>
    {
        /// <summary>
        /// 取得買家的訂單
        /// </summary>
        /// <param name="buyerId">買家ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>訂單列表</returns>
        Task<IEnumerable<PlayerMarketOrderInfo>> GetByBuyerAsync(int buyerId, int page = 1, int pageSize = 20);

        /// <summary>
        /// 取得賣家的訂單
        /// </summary>
        /// <param name="sellerId">賣家ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>訂單列表</returns>
        Task<IEnumerable<PlayerMarketOrderInfo>> GetBySellerAsync(int sellerId, int page = 1, int pageSize = 20);

        /// <summary>
        /// 取得訂單詳情
        /// </summary>
        /// <param name="orderId">訂單ID</param>
        /// <returns>訂單詳情</returns>
        Task<PlayerMarketOrderInfo?> GetByIdWithDetailsAsync(int orderId);

        /// <summary>
        /// 更新訂單狀態
        /// </summary>
        /// <param name="orderId">訂單ID</param>
        /// <param name="status">新狀態</param>
        /// <returns>更新結果</returns>
        Task<bool> UpdateStatusAsync(int orderId, string status);

        /// <summary>
        /// 取得待處理的訂單
        /// </summary>
        /// <param name="sellerId">賣家ID</param>
        /// <returns>待處理訂單列表</returns>
        Task<IEnumerable<PlayerMarketOrderInfo>> GetPendingOrdersAsync(int sellerId);

        /// <summary>
        /// 取得已完成的訂單
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>已完成訂單列表</returns>
        Task<IEnumerable<PlayerMarketOrderInfo>> GetCompletedOrdersAsync(int userId, int page = 1, int pageSize = 20);
    }
} 
