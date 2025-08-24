using GameCore.Core.Entities;

namespace GameCore.Core.Interfaces
{
    public interface IStoreRepository : IRepository<StoreProduct>
    {
        Task<IEnumerable<StoreProduct>> GetProductsAsync(string? category = null, bool activeOnly = true);
        Task<StoreProduct?> GetProductByIdAsync(int productId);
        Task<bool> UpdateProductStockAsync(int productId, int quantityChange);
        Task<StoreOrder?> GetOrderByIdAsync(int orderId);
        Task<IEnumerable<StoreOrder>> GetUserOrdersAsync(int userId, string? status = null);
        Task<bool> CreateOrderAsync(StoreOrder order);
        Task<bool> CreateOrderItemAsync(StoreOrderItem orderItem);
        Task<bool> UpdateOrderAsync(StoreOrder order);
        Task<ShoppingCartItem?> GetCartItemAsync(int userId, int productId);
        Task<IEnumerable<ShoppingCartItem>> GetCartItemsAsync(int userId);
        Task<bool> AddToCartAsync(ShoppingCartItem cartItem);
        Task<bool> UpdateCartItemAsync(ShoppingCartItem cartItem);
        Task<bool> RemoveFromCartAsync(int userId, int productId);
    }

    /// <summary>
    /// 訂單倉庫介面
    /// </summary>
    public interface IOrderRepository : IRepository<StoreOrder>
    {
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
        Task<StoreOrder?> GetOrderWithDetailsAsync(int orderId);

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
        Task<IEnumerable<StoreOrder>> GetPendingOrdersAsync(int page = 1, int pageSize = 20);
    }

    /// <summary>
    /// 購物車倉庫介面
    /// </summary>
    public interface ICartRepository : IRepository<ShoppingCart>
    {
        /// <summary>
        /// 取得使用者的購物車
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <returns>購物車項目列表</returns>
        Task<IEnumerable<ShoppingCart>> GetUserCartAsync(int userId);

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