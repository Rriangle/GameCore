using GameCore.Application.Common;
using GameCore.Application.DTOs;

namespace GameCore.Application.Services
{
    /// <summary>
    /// 商店服務介面
    /// </summary>
    public interface IStoreService
    {
        /// <summary>
        /// 取得商品列表
        /// </summary>
        /// <param name="categoryId">分類 ID</param>
        /// <param name="pageNumber">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>商品列表</returns>
        Task<Result<PagedResult<ProductResponse>>> GetProductsAsync(int? categoryId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);

        /// <summary>
        /// 取得商品詳情
        /// </summary>
        /// <param name="productId">商品 ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>商品詳情</returns>
        Task<Result<ProductResponse>> GetProductAsync(int productId, CancellationToken cancellationToken = default);

        /// <summary>
        /// 搜尋商品
        /// </summary>
        /// <param name="searchTerm">搜尋關鍵字</param>
        /// <param name="categoryId">分類 ID</param>
        /// <param name="minPrice">最低價格</param>
        /// <param name="maxPrice">最高價格</param>
        /// <param name="pageNumber">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>商品列表</returns>
        Task<Result<PagedResult<ProductResponse>>> SearchProductsAsync(string? searchTerm, int? categoryId, decimal? minPrice, decimal? maxPrice, int pageNumber, int pageSize, CancellationToken cancellationToken = default);

        /// <summary>
        /// 取得熱門商品
        /// </summary>
        /// <param name="limit">限制數量</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>熱門商品列表</returns>
        Task<Result<List<ProductResponse>>> GetPopularProductsAsync(int limit, CancellationToken cancellationToken = default);

        /// <summary>
        /// 取得推薦商品
        /// </summary>
        /// <param name="userId">用戶 ID</param>
        /// <param name="limit">限制數量</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>推薦商品列表</returns>
        Task<Result<List<ProductResponse>>> GetRecommendedProductsAsync(int userId, int limit, CancellationToken cancellationToken = default);

        /// <summary>
        /// 取得購物車
        /// </summary>
        /// <param name="userId">用戶 ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>購物車</returns>
        Task<Result<CartResponse>> GetCartAsync(int userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// 加入購物車
        /// </summary>
        /// <param name="request">加入購物車請求</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>加入結果</returns>
        Task<Result<CartItemResponse>> AddToCartAsync(AddToCartRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// 更新購物車項目
        /// </summary>
        /// <param name="request">更新購物車請求</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>更新結果</returns>
        Task<Result<CartItemResponse>> UpdateCartItemAsync(UpdateCartItemRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// 移除購物車項目
        /// </summary>
        /// <param name="userId">用戶 ID</param>
        /// <param name="itemId">項目 ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>移除結果</returns>
        Task<OperationResult> RemoveFromCartAsync(int userId, int itemId, CancellationToken cancellationToken = default);

        /// <summary>
        /// 清空購物車
        /// </summary>
        /// <param name="userId">用戶 ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>清空結果</returns>
        Task<OperationResult> ClearCartAsync(int userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// 建立訂單
        /// </summary>
        /// <param name="request">建立訂單請求</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>訂單資訊</returns>
        Task<Result<OrderResponse>> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// 取得用戶訂單
        /// </summary>
        /// <param name="userId">用戶 ID</param>
        /// <param name="pageNumber">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>訂單列表</returns>
        Task<Result<PagedResult<OrderResponse>>> GetUserOrdersAsync(int userId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);

        /// <summary>
        /// 取得訂單詳情
        /// </summary>
        /// <param name="orderId">訂單 ID</param>
        /// <param name="userId">用戶 ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>訂單詳情</returns>
        Task<Result<OrderResponse>> GetOrderAsync(int orderId, int userId, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// 商品回應
    /// </summary>
    public class ProductResponse
    {
        /// <summary>
        /// 商品 ID
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// 商品描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 分類 ID
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// 分類名稱
        /// </summary>
        public string CategoryName { get; set; } = string.Empty;

        /// <summary>
        /// 價格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 庫存數量
        /// </summary>
        public int StockQuantity { get; set; }

        /// <summary>
        /// 圖片 URL
        /// </summary>
        public string ImageUrl { get; set; } = string.Empty;

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// 購物車回應
    /// </summary>
    public class CartResponse
    {
        /// <summary>
        /// 用戶 ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 購物車項目
        /// </summary>
        public List<CartItemResponse> Items { get; set; } = new List<CartItemResponse>();

        /// <summary>
        /// 總數量
        /// </summary>
        public int TotalQuantity { get; set; }

        /// <summary>
        /// 總金額
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime LastUpdated { get; set; }
    }

    /// <summary>
    /// 購物車項目回應
    /// </summary>
    public class CartItemResponse
    {
        /// <summary>
        /// 項目 ID
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// 商品 ID
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// 單價
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 數量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 小計
        /// </summary>
        public decimal Subtotal { get; set; }

        /// <summary>
        /// 圖片 URL
        /// </summary>
        public string ImageUrl { get; set; } = string.Empty;
    }

    /// <summary>
    /// 加入購物車請求
    /// </summary>
    public class AddToCartRequest
    {
        /// <summary>
        /// 用戶 ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 商品 ID
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 數量
        /// </summary>
        public int Quantity { get; set; }
    }

    /// <summary>
    /// 更新購物車請求
    /// </summary>
    public class UpdateCartItemRequest
    {
        /// <summary>
        /// 用戶 ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 項目 ID
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// 數量
        /// </summary>
        public int Quantity { get; set; }
    }

    /// <summary>
    /// 建立訂單請求
    /// </summary>
    public class CreateOrderRequest
    {
        /// <summary>
        /// 用戶 ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 收貨地址
        /// </summary>
        public string ShippingAddress { get; set; } = string.Empty;

        /// <summary>
        /// 付款方式
        /// </summary>
        public string PaymentMethod { get; set; } = string.Empty;
    }

    /// <summary>
    /// 訂單回應
    /// </summary>
    public class OrderResponse
    {
        /// <summary>
        /// 訂單 ID
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// 用戶 ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 訂單編號
        /// </summary>
        public string OrderNumber { get; set; } = string.Empty;

        /// <summary>
        /// 訂單狀態
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// 總金額
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 收貨地址
        /// </summary>
        public string ShippingAddress { get; set; } = string.Empty;

        /// <summary>
        /// 付款方式
        /// </summary>
        public string PaymentMethod { get; set; } = string.Empty;

        /// <summary>
        /// 訂單項目
        /// </summary>
        public List<OrderItemResponse> Items { get; set; } = new List<OrderItemResponse>();

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }

    /// <summary>
    /// 訂單項目回應
    /// </summary>
    public class OrderItemResponse
    {
        /// <summary>
        /// 項目 ID
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// 商品 ID
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// 單價
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 數量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 小計
        /// </summary>
        public decimal Subtotal { get; set; }
    }
} 