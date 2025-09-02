using System.ComponentModel.DataAnnotations;

namespace GameCore.Core.DTOs
{
    /// <summary>
    /// 商品 DTO
    /// </summary>
    public class ProductDto
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "商品名稱為必填")]
        [StringLength(200, ErrorMessage = "商品名稱不能超過200個字元")]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(1000, ErrorMessage = "商品描述不能超過1000個字元")]
        public string? Description { get; set; }
        
        [Required(ErrorMessage = "商品價格為必填")]
        [Range(0, double.MaxValue, ErrorMessage = "商品價格必須大於等於0")]
        public decimal Price { get; set; }
        
        [Required(ErrorMessage = "庫存數量為必填")]
        [Range(0, int.MaxValue, ErrorMessage = "庫存數量必須大於等於0")]
        public int StockQuantity { get; set; }
        
        [Required(ErrorMessage = "商品分類為必填")]
        [StringLength(50, ErrorMessage = "商品分類不能超過50個字元")]
        public string Category { get; set; } = string.Empty;
        
        [StringLength(500, ErrorMessage = "圖片URL不能超過500個字元")]
        public string? ImageUrl { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// 商品搜尋條件 DTO
    /// </summary>
    public class ProductSearchDto
    {
        public string? Keyword { get; set; }
        public string? Category { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? SortBy { get; set; } = "name";
        public string? SortOrder { get; set; } = "asc";
    }

    /// <summary>
    /// 購物車項目 DTO
    /// </summary>
    public class CartItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? ProductImage { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal => Price * Quantity;
        public DateTime AddedAt { get; set; }
    }

    /// <summary>
    /// 購物車 DTO
    /// </summary>
    public class CartDto
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public List<CartItemDto> Items { get; set; } = new();
        public decimal TotalAmount => Items.Sum(item => item.Subtotal);
        public int TotalItems => Items.Sum(item => item.Quantity);
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// 訂單項目 DTO
    /// </summary>
    public class OrderItemDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? ProductImage { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal => Price * Quantity;
    }

    /// <summary>
    /// 訂單 DTO
    /// </summary>
    public class OrderDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; } = string.Empty;
        public string ShippingPhone { get; set; } = string.Empty;
        public string ShippingName { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public string? Note { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
        public DateTime OrderDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
    }

    /// <summary>
    /// 建立訂單請求 DTO
    /// </summary>
    public class CreateOrderRequestDto
    {
        [Required(ErrorMessage = "購物車項目為必填")]
        public List<int> CartIds { get; set; } = new();
        
        [Required(ErrorMessage = "收貨地址為必填")]
        [StringLength(500, ErrorMessage = "收貨地址不能超過500個字元")]
        public string ShippingAddress { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "收貨電話為必填")]
        [StringLength(20, ErrorMessage = "收貨電話不能超過20個字元")]
        public string ShippingPhone { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "收貨人姓名為必填")]
        [StringLength(100, ErrorMessage = "收貨人姓名不能超過100個字元")]
        public string ShippingName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "支付方式為必填")]
        public string PaymentMethod { get; set; } = string.Empty;
        
        [StringLength(500, ErrorMessage = "備註不能超過500個字元")]
        public string? Note { get; set; }
    }

    /// <summary>
    /// 建立訂單回應 DTO
    /// </summary>
    public class CreateOrderResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public OrderDto? Order { get; set; }
        public List<string> Errors { get; set; } = new();
    }

    /// <summary>
    /// 商品分類 DTO
    /// </summary>
    public class CategoryDto
    {
        public string Name { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int ProductCount { get; set; }
        public string? Icon { get; set; }
    }

    /// <summary>
    /// 熱門商品 DTO
    /// </summary>
    public class PopularProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? ProductImage { get; set; }
        public decimal Price { get; set; }
        public int SalesCount { get; set; }
        public decimal Rating { get; set; }
    }

    /// <summary>
    /// 銷售排行榜 DTO
    /// </summary>
    public class SalesRankingDto
    {
        public int Rank { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? ProductImage { get; set; }
        public decimal Price { get; set; }
        public int SalesCount { get; set; }
        public decimal Revenue { get; set; }
        public string Period { get; set; } = string.Empty;
    }

    /// <summary>
    /// 購物車項目結果 DTO
    /// </summary>
    public class CartItemResult
    {
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? ProductImage { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public bool IsAvailable { get; set; }
        public string? UnavailableReason { get; set; }
    }

    /// <summary>
    /// 購物車結果 DTO
    /// </summary>
    public class CartResult
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public List<CartItemResult> Items { get; set; } = new();
        public decimal Subtotal { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal TotalAmount { get; set; }
        public int ItemCount { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    /// <summary>
    /// 訂單建立 DTO
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
    /// 訂單結果 DTO
    /// </summary>
    public class OrderResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public OrderDto? Order { get; set; }
        public List<string> Errors { get; set; } = new();
        public List<string> Warnings { get; set; } = new();
    }
} 
