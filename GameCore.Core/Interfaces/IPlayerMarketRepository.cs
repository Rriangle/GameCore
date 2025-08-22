using GameCore.Core.Entities;

namespace GameCore.Core.Interfaces
{
    /// <summary>
    /// 玩家市場 Repository 介面
    /// </summary>
    public interface IPlayerMarketRepository : IRepository<PlayerMarketProductInfo>
    {
        /// <summary>
        /// 搜尋商品
        /// </summary>
        /// <param name="searchTerm">搜尋關鍵字</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁數量</param>
        /// <returns></returns>
        Task<IEnumerable<PlayerMarketProductInfo>> SearchProductsAsync(string? searchTerm, int page, int pageSize);

        /// <summary>
        /// 取得用戶商品
        /// </summary>
        /// <param name="sellerId">賣家ID</param>
        /// <returns></returns>
        Task<IEnumerable<PlayerMarketProductInfo>> GetUserProductsAsync(int sellerId);

        /// <summary>
        /// 取得熱門商品
        /// </summary>
        /// <param name="count">數量</param>
        /// <returns></returns>
        Task<IEnumerable<PlayerMarketProductInfo>> GetPopularProductsAsync(int count);

        /// <summary>
        /// 取得最新商品
        /// </summary>
        /// <param name="count">數量</param>
        /// <returns></returns>
        Task<IEnumerable<PlayerMarketProductInfo>> GetLatestProductsAsync(int count);

        /// <summary>
        /// 取得商品及其圖片
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <returns></returns>
        Task<PlayerMarketProductInfo?> GetProductWithImagesAsync(int productId);
    }
}
