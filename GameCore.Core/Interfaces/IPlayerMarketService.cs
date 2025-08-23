using GameCore.Core.Entities;

namespace GameCore.Core.Interfaces
{
    /// <summary>
    /// 玩家市場服務介面
    /// </summary>
    public interface IPlayerMarketService
    {
        Task<PlayerMarketProductInfo> CreateProductAsync(PlayerMarketProductInfo product);
        Task<PlayerMarketProductInfo?> GetProductAsync(int productId);
        Task<IEnumerable<PlayerMarketProductInfo>> GetUserProductsAsync(int userId);
        Task<bool> UpdateProductAsync(PlayerMarketProductInfo product);
        Task<bool> DeleteProductAsync(int productId);
        Task<IEnumerable<PlayerMarketProductInfo>> SearchProductsAsync(string searchTerm);
    }
}