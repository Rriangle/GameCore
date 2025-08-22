using GameCore.Core.Entities;

namespace GameCore.Core.Interfaces
{
    /// <summary>
    /// 商城服務介面
    /// </summary>
    public interface IStoreService
    {
        /// <summary>
        /// 取得熱門商品
        /// </summary>
        Task<List<ProductInfo>> GetPopularProductsAsync(int count = 10);

        /// <summary>
        /// 根據類型取得商品
        /// </summary>
        Task<List<ProductInfo>> GetProductsByTypeAsync(string productType);

        /// <summary>
        /// 建立訂單
        /// </summary>
        Task<OrderInfo> CreateOrderAsync(int userId, List<OrderItem> items);

        /// <summary>
        /// 處理付款
        /// </summary>
        Task<bool> ProcessPaymentAsync(int orderId, decimal amount);
    }
}

