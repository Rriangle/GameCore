using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// 市場商品倉庫介面
    /// </summary>
    public interface IMarketItemRepository : IRepository<MarketItem>
    {
        /// <summary>
        /// 取得活躍的市場商品
        /// </summary>
        /// <param name="category">商品分類</param>
        /// <param name="minPrice">最低價格</param>
        /// <param name="maxPrice">最高價格</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>市場商品列表</returns>
        Task<IEnumerable<MarketItem>> GetActiveItemsAsync(string? category = null, decimal? minPrice = null, decimal? maxPrice = null, int page = 1, int pageSize = 20);

        /// <summary>
        /// 搜尋市場商品
        /// </summary>
        /// <param name="keyword">關鍵字</param>
        /// <param name="category">商品分類</param>
        /// <param name="minPrice">最低價格</param>
        /// <param name="maxPrice">最高價格</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>搜尋結果</returns>
        Task<IEnumerable<MarketItem>> SearchItemsAsync(string keyword, string? category = null, decimal? minPrice = null, decimal? maxPrice = null, int page = 1, int pageSize = 20);

        /// <summary>
        /// 取得賣家的商品
        /// </summary>
        /// <param name="sellerId">賣家ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>商品列表</returns>
        Task<IEnumerable<MarketItem>> GetBySellerAsync(int sellerId, int page = 1, int pageSize = 20);

        /// <summary>
        /// 取得商品詳情
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <returns>商品詳情</returns>
        Task<MarketItem?> GetByIdWithDetailsAsync(int productId);

        /// <summary>
        /// 更新商品狀態
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <param name="status">新狀態</param>
        /// <returns>更新結果</returns>
        Task<bool> UpdateStatusAsync(int productId, string status);

        /// <summary>
        /// 取得熱門商品
        /// </summary>
        /// <param name="limit">數量限制</param>
        /// <returns>熱門商品列表</returns>
        Task<IEnumerable<MarketItem>> GetPopularItemsAsync(int limit = 10);
    }
} 
