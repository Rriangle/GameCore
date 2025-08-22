using GameCore.Core.Entities;

namespace GameCore.Core.Interfaces
{
    /// <summary>
    /// 商店 Repository 介面
    /// </summary>
    public interface IStoreRepository : IRepository<ProductInfo>
    {
        /// <summary>
        /// 取得商品及其詳細資訊
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <returns></returns>
        Task<ProductInfo?> GetProductWithDetailsAsync(int productId);

        /// <summary>
        /// 搜尋商品
        /// </summary>
        /// <param name="searchTerm">搜尋關鍵字</param>
        /// <param name="category">分類</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁數量</param>
        /// <returns></returns>
        Task<IEnumerable<ProductInfo>> SearchProductsAsync(string? searchTerm, string? category, int page, int pageSize);

        /// <summary>
        /// 取得熱門商品
        /// </summary>
        /// <param name="count">數量</param>
        /// <returns></returns>
        Task<IEnumerable<ProductInfo>> GetPopularProductsAsync(int count);

        /// <summary>
        /// 取得新品
        /// </summary>
        /// <param name="count">數量</param>
        /// <returns></returns>
        Task<IEnumerable<ProductInfo>> GetNewProductsAsync(int count);

        /// <summary>
        /// 取得特色商品
        /// </summary>
        /// <param name="count">數量</param>
        /// <returns></returns>
        Task<IEnumerable<ProductInfo>> GetFeaturedProductsAsync(int count);
    }
}
