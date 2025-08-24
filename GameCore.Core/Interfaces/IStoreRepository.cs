using GameCore.Core.Entities;

namespace GameCore.Core.Interfaces
{
    /// <summary>
    /// 商城倉庫介面
    /// </summary>
    public interface IStoreRepository : IRepository<ProductInfo>
    {
        /// <summary>
        /// 取得所有活躍的商品
        /// </summary>
        /// <param name="category">商品分類</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>商品列表</returns>
        Task<IEnumerable<ProductInfo>> GetActiveProductsAsync(string? category = null, int page = 1, int pageSize = 20);

        /// <summary>
        /// 搜尋商品
        /// </summary>
        /// <param name="keyword">關鍵字</param>
        /// <param name="category">商品分類</param>
        /// <param name="minPrice">最低價格</param>
        /// <param name="maxPrice">最高價格</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>搜尋結果</returns>
        Task<IEnumerable<ProductInfo>> SearchProductsAsync(string keyword, string? category = null, decimal? minPrice = null, decimal? maxPrice = null, int page = 1, int pageSize = 20);

        /// <summary>
        /// 取得商品分類
        /// </summary>
        /// <returns>分類列表</returns>
        Task<IEnumerable<string>> GetProductCategoriesAsync();

        /// <summary>
        /// 取得熱門商品
        /// </summary>
        /// <param name="limit">數量限制</param>
        /// <returns>熱門商品列表</returns>
        Task<IEnumerable<ProductInfo>> GetPopularProductsAsync(int limit = 10);

        /// <summary>
        /// 取得銷售排行榜
        /// </summary>
        /// <param name="period">期間 (daily, weekly, monthly, yearly)</param>
        /// <param name="limit">數量限制</param>
        /// <returns>排行榜</returns>
        Task<IEnumerable<ProductInfo>> GetSalesRankingAsync(string period, int limit = 10);
    }

    /// <summary>
    /// 訂單倉庫介面
    /// </summary>
    public interface IOrderRepository : IRepository<OrderInfo>
    {
        /// <summary>
        /// 取得使用者的訂單
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>訂單列表</returns>
        Task<IEnumerable<OrderInfo>> GetOrdersByUserAsync(int userId, int page, int pageSize);

        /// <summary>
        /// 取得訂單詳情
        /// </summary>
        /// <param name="orderId">訂單ID</param>
        /// <returns>訂單詳情</returns>
        Task<OrderInfo?> GetOrderWithDetailsAsync(int orderId);

        /// <summary>
        /// 更新訂單狀態
        /// </summary>
        /// <param name="orderId">訂單ID</param>
        /// <param name="status">新狀態</param>
        /// <returns>操作結果</returns>
        Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status);

        /// <summary>
        /// 取得待處理的訂單
        /// </summary>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>訂單列表</returns>
        Task<IEnumerable<OrderInfo>> GetPendingOrdersAsync(int page = 1, int pageSize = 20);
    }

    /// <summary>
    /// 購物車倉庫介面
    /// </summary>
    public interface ICartRepository : IRepository<OrderItem>
    {
        /// <summary>
        /// 取得使用者的購物車
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <returns>購物車項目列表</returns>
        Task<IEnumerable<OrderItem>> GetUserCartAsync(int userId);

        /// <summary>
        /// 檢查商品是否已在購物車中
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="productId">商品ID</param>
        /// <returns>是否已在購物車中</returns>
        Task<bool> IsProductInCartAsync(int userId, int productId);

        /// <summary>
        /// 更新購物車商品數量
        /// </summary>
        /// <param name="cartId">購物車ID</param>
        /// <param name="quantity">新數量</param>
        /// <returns>操作結果</returns>
        Task<bool> UpdateCartQuantityAsync(int cartId, int quantity);

        /// <summary>
        /// 清空使用者的購物車
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <returns>操作結果</returns>
        Task<bool> ClearUserCartAsync(int userId);
    }
}