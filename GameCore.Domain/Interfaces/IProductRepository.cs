using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// 產品 Repository 接口
    /// </summary>
    public interface IProductRepository : IRepository<Product>
    {
        /// <summary>
        /// 根據分類獲取產品
        /// </summary>
        /// <param name="category">分類</param>
        /// <returns>產品列表</returns>
        Task<IEnumerable<Product>> GetByCategoryAsync(string category);

        /// <summary>
        /// 搜索產品
        /// </summary>
        /// <param name="keyword">關鍵字</param>
        /// <returns>產品列表</returns>
        Task<IEnumerable<Product>> SearchAsync(string keyword);

        /// <summary>
        /// 獲取熱門產品
        /// </summary>
        /// <param name="limit">數量限制</param>
        /// <returns>產品列表</returns>
        Task<IEnumerable<Product>> GetPopularAsync(int limit);
    }
} 
