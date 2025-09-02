using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// 玩家市場訂單 Repository 介面
    /// </summary>
    public interface IPlayerMarketOrderRepository : IRepository<PlayerMarketOrderInfo>
    {
        /// <summary>
        /// 根據買家ID取得訂單
        /// </summary>
        Task<IEnumerable<PlayerMarketOrderInfo>> GetByBuyerIdAsync(int buyerId);

        /// <summary>
        /// 根據賣家ID取得訂單
        /// </summary>
        Task<IEnumerable<PlayerMarketOrderInfo>> GetBySellerIdAsync(int sellerId);

        /// <summary>
        /// 根據狀態取得訂單
        /// </summary>
        Task<IEnumerable<PlayerMarketOrderInfo>> GetByStatusAsync(string status);

        /// <summary>
        /// 新增訂單
        /// </summary>
        Task<PlayerMarketOrderInfo> AddAsync(PlayerMarketOrderInfo order);

        /// <summary>
        /// 更新訂單
        /// </summary>
        Task UpdateAsync(PlayerMarketOrderInfo order);

        /// <summary>
        /// 刪除訂單
        /// </summary>
        Task DeleteAsync(PlayerMarketOrderInfo order);

        /// <summary>
        /// 取得所有訂單
        /// </summary>
        Task<IEnumerable<PlayerMarketOrderInfo>> GetAll();
    }
} 
