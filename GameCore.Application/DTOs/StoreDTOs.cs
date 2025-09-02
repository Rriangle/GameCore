using System.ComponentModel.DataAnnotations;

namespace GameCore.Application.DTOs
{
    /// <summary>
    /// 商品回應
    /// </summary>
    public class ProductResponse
    {
        /// <summary>
        /// 商品 ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 商品描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 價格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 原價
        /// </summary>
        public decimal? OriginalPrice { get; set; }

        /// <summary>
        /// 庫存數量
        /// </summary>
        public int StockQuantity { get; set; }

        /// <summary>
        /// 商品類別
        /// </summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// 商品圖片 URL
        /// </summary>
        public string? ImageUrl { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 創建時間
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }

    /// <summary>
    /// 商品 DTO (舊版本相容性)
    /// </summary>
    public class ProductDto : ProductResponse
    {
    }

    /// <summary>
    /// 購物車回應
    /// </summary>
    public class CartResponse
    {
        /// <summary>
        /// 購物車 ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用戶 ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 購物車項目
        /// </summary>
        public List<CartItemResponse> Items { get; set; } = new List<CartItemResponse>();

        /// <summary>
        /// 總金額
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 項目數量
        /// </summary>
        public int ItemCount { get; set; }

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime LastUpdated { get; set; }
    }

    /// <summary>
    /// 購物車 DTO (舊版本相容性)
    /// </summary>
    public class CartDto : CartResponse
    {
    }

    /// <summary>
    /// 購物車項目回應
    /// </summary>
    public class CartItemResponse
    {
        /// <summary>
        /// 購物車項目 ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 商品 ID
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// 數量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 單價
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 小計
        /// </summary>
        public decimal Subtotal { get; set; }

        /// <summary>
        /// 商品圖片 URL
        /// </summary>
        public string? ProductImageUrl { get; set; }
    }

    /// <summary>
    /// 購物車項目 DTO (舊版本相容性)
    /// </summary>
    public class CartItemDto : CartItemResponse
    {
    }

    /// <summary>
    /// 訂單回應
    /// </summary>
    public class OrderResponse
    {
        /// <summary>
        /// 訂單 ID
        /// </summary>
        public int Id { get; set; }

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
    /// 訂單 DTO (舊版本相容性)
    /// </summary>
    public class OrderDto : OrderResponse
    {
    }

    /// <summary>
    /// 訂單項目回應
    /// </summary>
    public class OrderItemResponse
    {
        /// <summary>
        /// 訂單項目 ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 商品 ID
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// 數量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 單價
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 小計
        /// </summary>
        public decimal Subtotal { get; set; }
    }

    /// <summary>
    /// 新增購物車請求
    /// </summary>
    public class AddToCartRequest
    {
        /// <summary>
        /// 用戶 ID
        /// </summary>
        [Required(ErrorMessage = "用戶 ID 為必填")]
        public int UserId { get; set; }

        /// <summary>
        /// 商品 ID
        /// </summary>
        [Required(ErrorMessage = "商品 ID 為必填")]
        public int ProductId { get; set; }

        /// <summary>
        /// 數量
        /// </summary>
        [Required(ErrorMessage = "數量為必填")]
        [Range(1, int.MaxValue, ErrorMessage = "數量必須大於 0")]
        public int Quantity { get; set; }
    }

    /// <summary>
    /// 更新購物車項目請求
    /// </summary>
    public class UpdateCartItemRequest
    {
        /// <summary>
        /// 購物車項目 ID
        /// </summary>
        [Required(ErrorMessage = "購物車項目 ID 為必填")]
        public int CartItemId { get; set; }

        /// <summary>
        /// 數量
        /// </summary>
        [Required(ErrorMessage = "數量為必填")]
        [Range(1, int.MaxValue, ErrorMessage = "數量必須大於 0")]
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
        [Required(ErrorMessage = "用戶 ID 為必填")]
        public int UserId { get; set; }

        /// <summary>
        /// 購物車 ID
        /// </summary>
        [Required(ErrorMessage = "購物車 ID 為必填")]
        public int CartId { get; set; }

        /// <summary>
        /// 收貨地址
        /// </summary>
        [StringLength(500, ErrorMessage = "收貨地址長度不能超過 500 字元")]
        public string? ShippingAddress { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        [StringLength(1000, ErrorMessage = "備註長度不能超過 1000 字元")]
        public string? Notes { get; set; }
    }

    /// <summary>
    /// 搜尋商品請求
    /// </summary>
    public class SearchProductsRequest
    {
        /// <summary>
        /// 關鍵字
        /// </summary>
        public string? Keyword { get; set; }

        /// <summary>
        /// 類別
        /// </summary>
        public string? Category { get; set; }

        /// <summary>
        /// 最低價格
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "最低價格不能為負數")]
        public decimal? MinPrice { get; set; }

        /// <summary>
        /// 最高價格
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "最高價格不能為負數")]
        public decimal? MaxPrice { get; set; }

        /// <summary>
        /// 頁碼
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "頁碼必須大於 0")]
        public int Page { get; set; } = 1;

        /// <summary>
        /// 每頁大小
        /// </summary>
        [Range(1, 100, ErrorMessage = "每頁大小必須在 1-100 之間")]
        public int PageSize { get; set; } = 10;
    }
} 