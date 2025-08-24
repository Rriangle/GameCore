using System.ComponentModel.DataAnnotations;
using GameCore.Core.Enums;

namespace GameCore.Core.DTOs
{
    /// <summary>
    /// 商品 DTO
    /// </summary>
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductType { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string CurrencyCode { get; set; } = string.Empty;
        public int ShipmentQuantity { get; set; }
        public string ProductCreatedBy { get; set; } = string.Empty;
        public DateTime ProductCreatedAt { get; set; }
        public DateTime ProductUpdatedAt { get; set; }
        public string? Description { get; set; }
        public string? DownloadLink { get; set; }
        public string? SupplierName { get; set; }
        public bool IsAvailable { get; set; }
        public int SalesCount { get; set; }
        public decimal Rating { get; set; }
    }

    /// <summary>
    /// 購物車結果 DTO
    /// </summary>
    public class CartResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<CartItemDto> Items { get; set; } = new();
        public decimal TotalAmount { get; set; }
        public int TotalItems { get; set; }
    }

    /// <summary>
    /// 購物車項目 DTO
    /// </summary>
    public class CartItemDto
    {
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }
        public DateTime AddedAt { get; set; }
        public bool IsAvailable { get; set; }
    }

    /// <summary>
    /// 購物車項目創建請求 DTO
    /// </summary>
    public class CartItemCreateDto
    {
        [Required(ErrorMessage = "商品ID不能為空")]
        [Range(1, int.MaxValue, ErrorMessage = "商品ID必須大於0")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "數量不能為空")]
        [Range(1, 999, ErrorMessage = "數量必須在1-999之間")]
        public int Quantity { get; set; }
    }

    /// <summary>
    /// 購物車項目結果 DTO
    /// </summary>
    public class CartItemResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public CartItemDto? Item { get; set; }
    }

    /// <summary>
    /// 訂單 DTO
    /// </summary>
    public class OrderDto
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; } = string.Empty;
        public string PaymentStatus { get; set; } = string.Empty;
        public decimal OrderTotal { get; set; }
        public DateTime? PaymentAt { get; set; }
        public DateTime? ShippedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
    }

    /// <summary>
    /// 訂單項目 DTO
    /// </summary>
    public class OrderItemDto
    {
        public int ItemId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int LineNo { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }
    }

    /// <summary>
    /// 訂單創建請求 DTO
    /// </summary>
    public class OrderCreateDto
    {
        [Required(ErrorMessage = "訂單項目不能為空")]
        [MinLength(1, ErrorMessage = "至少需要一個訂單項目")]
        public List<OrderItemCreateDto> Items { get; set; } = new();

        [StringLength(200, ErrorMessage = "備註長度不能超過200個字符")]
        public string? Notes { get; set; }

        [StringLength(200, ErrorMessage = "收貨地址長度不能超過200個字符")]
        public string? ShippingAddress { get; set; }
    }

    /// <summary>
    /// 訂單項目創建請求 DTO
    /// </summary>
    public class OrderItemCreateDto
    {
        [Required(ErrorMessage = "商品ID不能為空")]
        [Range(1, int.MaxValue, ErrorMessage = "商品ID必須大於0")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "數量不能為空")]
        [Range(1, 999, ErrorMessage = "數量必須在1-999之間")]
        public int Quantity { get; set; }
    }

    /// <summary>
    /// 訂單結果 DTO
    /// </summary>
    public class OrderResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public OrderDto? Order { get; set; }
    }

    /// <summary>
    /// 訂單更新結果 DTO
    /// </summary>
    public class OrderUpdateResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public OrderDto? Order { get; set; }
    }

    /// <summary>
    /// 購物車項目（簡化版）
    /// </summary>
    public class CartItem
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    /// <summary>
    /// 訂單創建（簡化版）
    /// </summary>
    public class OrderCreate
    {
        public List<OrderItemCreate> Items { get; set; } = new();
        public string? Notes { get; set; }
        public string? ShippingAddress { get; set; }
    }

    /// <summary>
    /// 訂單項目創建（簡化版）
    /// </summary>
    public class OrderItemCreate
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}