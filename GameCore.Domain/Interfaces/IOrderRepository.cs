using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// 訂單 Repository 接口
    /// </summary>
    public interface IOrderRepository : IRepository<Order>
    {
        /// <summary>
        /// 根據用戶ID獲取訂單
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns>訂單列表</returns>
        Task<IEnumerable<Order>> GetByUserIdAsync(int userId);

        /// <summary>
        /// 根據狀態獲取訂單
        /// </summary>
        /// <param name="status">訂單狀態</param>
        /// <returns>訂單列表</returns>
        Task<IEnumerable<Order>> GetByStatusAsync(string status);

        /// <summary>
        /// 獲取待處理訂單
        /// </summary>
        /// <returns>訂單列表</returns>
        Task<IEnumerable<Order>> GetPendingOrdersAsync();
    }
} 
