using GameCore.Core.Entities;

namespace GameCore.Core.Services
{
    /// <summary>
    /// 商城服務介面
    /// </summary>
    public interface IStoreService
    {
        /// <summary>
        /// 取得所有活躍的商品
        /// </summary>
        /// <param name="category">商品分類</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>商品列表</returns>
        Task<IEnumerable<StoreProduct>> GetActiveProductsAsync(string? category = null, int page = 1, int pageSize = 20);

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
        Task<IEnumerable<StoreProduct>> SearchProductsAsync(string keyword, string? category = null, decimal? minPrice = null, decimal? maxPrice = null, int page = 1, int pageSize = 20);

        /// <summary>
        /// 取得商品詳情
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <returns>商品詳情</returns>
        Task<StoreProduct?> GetProductAsync(int productId);

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
        Task<IEnumerable<StoreProduct>> GetPopularProductsAsync(int limit = 10);

        /// <summary>
        /// 取得銷售排行榜
        /// </summary>
        /// <param name="period">期間 (daily, weekly, monthly, yearly)</param>
        /// <param name="limit">數量限制</param>
        /// <returns>排行榜</returns>
        Task<IEnumerable<StoreProduct>> GetSalesRankingAsync(string period, int limit = 10);

        /// <summary>
        /// 加入購物車
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="cartItem">購物車項目</param>
        /// <returns>操作結果</returns>
        Task<bool> AddToCartAsync(int userId, CartItem cartItem);

        /// <summary>
        /// 取得使用者的購物車
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <returns>購物車項目列表</returns>
        Task<IEnumerable<ShoppingCart>> GetUserCartAsync(int userId);

        /// <summary>
        /// 更新購物車商品數量
        /// </summary>
        /// <param name="cartId">購物車ID</param>
        /// <param name="quantity">新數量</param>
        /// <returns>操作結果</returns>
        Task<bool> UpdateCartQuantityAsync(int cartId, int quantity);

        /// <summary>
        /// 從購物車移除商品
        /// </summary>
        /// <param name="cartId">購物車ID</param>
        /// <returns>操作結果</returns>
        Task<bool> RemoveFromCartAsync(int cartId);

        /// <summary>
        /// 清空使用者的購物車
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <returns>操作結果</returns>
        Task<bool> ClearUserCartAsync(int userId);

        /// <summary>
        /// 建立訂單
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="orderCreate">訂單建立</param>
        /// <returns>建立結果</returns>
        Task<OrderCreateResult> CreateOrderAsync(int userId, OrderCreate orderCreate);

        /// <summary>
        /// 取得使用者的訂單
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>訂單列表</returns>
        Task<IEnumerable<StoreOrder>> GetOrdersByUserAsync(int userId, int page, int pageSize);

        /// <summary>
        /// 取得訂單詳情
        /// </summary>
        /// <param name="orderId">訂單ID</param>
        /// <returns>訂單詳情</returns>
        Task<StoreOrder?> GetOrderAsync(int orderId);

        /// <summary>
        /// 取消訂單
        /// </summary>
        /// <param name="orderId">訂單ID</param>
        /// <param name="userId">使用者ID</param>
        /// <returns>操作結果</returns>
        Task<bool> CancelOrderAsync(int orderId, int userId);
    }

    /// <summary>
    /// 購物車項目模型
    /// </summary>
    public class CartItem
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    /// <summary>
    /// 訂單建立模型
    /// </summary>
    public class OrderCreate
    {
        public List<int> CartIds { get; set; } = new();
        public string ShippingAddress { get; set; } = string.Empty;
        public string ShippingPhone { get; set; } = string.Empty;
        public string ShippingName { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public string? Note { get; set; }
    }

    /// <summary>
    /// 訂單建立結果模型
    /// </summary>
    public class OrderCreateResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public StoreOrder? Order { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}