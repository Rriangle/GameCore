using GameCore.Core.Entities;

namespace GameCore.Core.Interfaces
{
    /// <summary>
    /// 玩家市場服務介面
    /// </summary>
    public interface IPlayerMarketService
    {
        /// <summary>
        /// 取得使用者的商品
        /// </summary>
        Task<List<PlayerMarketProductInfo>> GetUserProductsAsync(int userId);

        /// <summary>
        /// 上架商品
        /// </summary>
        Task<PlayerMarketProductInfo> ListProductAsync(int sellerId, string title, string name, decimal price, string? description = null);

        /// <summary>
        /// 下單購買
        /// </summary>
        Task<PlayerMarketOrderInfo> CreateOrderAsync(int productId, int buyerId, int quantity);

        /// <summary>
        /// 取得熱門商品
        /// </summary>
        Task<List<PlayerMarketProductInfo>> GetPopularProductsAsync(int count = 10);
    }
}

