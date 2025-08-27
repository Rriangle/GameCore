using GameCore.Core.DTOs;
using GameCore.Core.Entities;

namespace GameCore.Core.Services
{
    /// <summary>
    /// 產品服務接口
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// 獲取產品列表
        /// </summary>
        /// <param name="categoryId">分類 ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>產品列表</returns>
        Task<PagedResult<Product>> GetProductsAsync(int? categoryId = null, int page = 1, int pageSize = 20);

        /// <summary>
        /// 獲取產品詳情
        /// </summary>
        /// <param name="productId">產品 ID</param>
        /// <returns>產品詳情</returns>
        Task<Product?> GetProductByIdAsync(int productId);

        /// <summary>
        /// 創建產品
        /// </summary>
        /// <param name="product">產品信息</param>
        /// <returns>創建的產品</returns>
        Task<Product> CreateProductAsync(Product product);

        /// <summary>
        /// 更新產品
        /// </summary>
        /// <param name="product">產品信息</param>
        /// <returns>更新結果</returns>
        Task<bool> UpdateProductAsync(Product product);

        /// <summary>
        /// 刪除產品
        /// </summary>
        /// <param name="productId">產品 ID</param>
        /// <returns>刪除結果</returns>
        Task<bool> DeleteProductAsync(int productId);

        /// <summary>
        /// 搜索產品
        /// </summary>
        /// <param name="keyword">關鍵字</param>
        /// <param name="categoryId">分類 ID</param>
        /// <param name="minPrice">最低價格</param>
        /// <param name="maxPrice">最高價格</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>搜索結果</returns>
        Task<PagedResult<Product>> SearchProductsAsync(
            string? keyword = null,
            int? categoryId = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            int page = 1,
            int pageSize = 20);

        /// <summary>
        /// 獲取熱門產品
        /// </summary>
        /// <param name="limit">數量限制</param>
        /// <returns>熱門產品列表</returns>
        Task<List<Product>> GetPopularProductsAsync(int limit = 10);

        /// <summary>
        /// 獲取推薦產品
        /// </summary>
        /// <param name="userId">用戶 ID</param>
        /// <param name="limit">數量限制</param>
        /// <returns>推薦產品列表</returns>
        Task<List<Product>> GetRecommendedProductsAsync(int userId, int limit = 10);

        /// <summary>
        /// 更新產品庫存
        /// </summary>
        /// <param name="productId">產品 ID</param>
        /// <param name="quantity">數量變化</param>
        /// <returns>更新結果</returns>
        Task<bool> UpdateStockAsync(int productId, int quantity);

        /// <summary>
        /// 檢查產品庫存
        /// </summary>
        /// <param name="productId">產品 ID</param>
        /// <param name="quantity">所需數量</param>
        /// <returns>是否有足夠庫存</returns>
        Task<bool> CheckStockAsync(int productId, int quantity);

        /// <summary>
        /// 獲取產品統計信息
        /// </summary>
        /// <param name="productId">產品 ID</param>
        /// <returns>統計信息</returns>
        Task<ProductStatistics> GetProductStatisticsAsync(int productId);
    }

    /// <summary>
    /// 分頁結果
    /// </summary>
    /// <typeparam name="T">數據類型</typeparam>
    public class PagedResult<T>
    {
        /// <summary>
        /// 數據列表
        /// </summary>
        public List<T> Items { get; set; } = new List<T>();

        /// <summary>
        /// 總數量
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 當前頁碼
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 每頁大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 總頁數
        /// </summary>
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        /// <summary>
        /// 是否有上一頁
        /// </summary>
        public bool HasPreviousPage => Page > 1;

        /// <summary>
        /// 是否有下一頁
        /// </summary>
        public bool HasNextPage => Page < TotalPages;
    }

    /// <summary>
    /// 產品統計信息
    /// </summary>
    public class ProductStatistics
    {
        /// <summary>
        /// 產品 ID
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 總銷量
        /// </summary>
        public int TotalSales { get; set; }

        /// <summary>
        /// 總收入
        /// </summary>
        public decimal TotalRevenue { get; set; }

        /// <summary>
        /// 平均評分
        /// </summary>
        public double AverageRating { get; set; }

        /// <summary>
        /// 評分數量
        /// </summary>
        public int RatingCount { get; set; }

        /// <summary>
        /// 瀏覽次數
        /// </summary>
        public int ViewCount { get; set; }

        /// <summary>
        /// 收藏次數
        /// </summary>
        public int FavoriteCount { get; set; }

        /// <summary>
        /// 當前庫存
        /// </summary>
        public int CurrentStock { get; set; }

        /// <summary>
        /// 統計時間
        /// </summary>
        public DateTime CalculatedAt { get; set; } = DateTime.UtcNow;
    }
} 