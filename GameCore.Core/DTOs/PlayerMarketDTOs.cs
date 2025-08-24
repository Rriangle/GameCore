using System.ComponentModel.DataAnnotations;
using GameCore.Core.Enums;

namespace GameCore.Core.DTOs
{
    /// <summary>
    /// 市場商品 DTO
    /// </summary>
    public class MarketItemDto
    {
        public int ProductId { get; set; }
        public string ProductType { get; set; } = string.Empty;
        public string ProductTitle { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public int? OriginalProductId { get; set; }
        public int SellerId { get; set; }
        public string SellerName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<string> Images { get; set; } = new();
        public bool IsAvailable { get; set; }
        public int ViewCount { get; set; }
        public decimal Rating { get; set; }
        public int ReviewCount { get; set; }
    }

    /// <summary>
    /// 市場商品創建請求 DTO
    /// </summary>
    public class MarketItemCreateDto
    {
        [Required(ErrorMessage = "商品類型不能為空")]
        [StringLength(50, ErrorMessage = "商品類型長度不能超過50個字符")]
        public string ProductType { get; set; } = string.Empty;

        [Required(ErrorMessage = "商品標題不能為空")]
        [StringLength(200, ErrorMessage = "商品標題長度不能超過200個字符")]
        public string ProductTitle { get; set; } = string.Empty;

        [Required(ErrorMessage = "商品名稱不能為空")]
        [StringLength(100, ErrorMessage = "商品名稱長度不能超過100個字符")]
        public string ProductName { get; set; } = string.Empty;

        [Required(ErrorMessage = "商品描述不能為空")]
        [StringLength(1000, ErrorMessage = "商品描述長度不能超過1000個字符")]
        public string ProductDescription { get; set; } = string.Empty;

        public int? OriginalProductId { get; set; }

        [Required(ErrorMessage = "價格不能為空")]
        [Range(0.01, 999999.99, ErrorMessage = "價格必須在0.01-999999.99之間")]
        public decimal Price { get; set; }

        public List<string> Images { get; set; } = new();
    }

    /// <summary>
    /// 市場商品創建結果 DTO
    /// </summary>
    public class MarketItemCreateResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public MarketItemDto? Item { get; set; }
    }

    /// <summary>
    /// 市場商品更新請求 DTO
    /// </summary>
    public class MarketItemUpdateDto
    {
        [StringLength(200, ErrorMessage = "商品標題長度不能超過200個字符")]
        public string? ProductTitle { get; set; }

        [StringLength(100, ErrorMessage = "商品名稱長度不能超過100個字符")]
        public string? ProductName { get; set; }

        [StringLength(1000, ErrorMessage = "商品描述長度不能超過1000個字符")]
        public string? ProductDescription { get; set; }

        [Range(0.01, 999999.99, ErrorMessage = "價格必須在0.01-999999.99之間")]
        public decimal? Price { get; set; }

        public List<string>? Images { get; set; }
    }

    /// <summary>
    /// 市場商品更新結果 DTO
    /// </summary>
    public class MarketItemUpdateResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public MarketItemDto? Item { get; set; }
    }

    /// <summary>
    /// 交易 DTO
    /// </summary>
    public class TransactionDto
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int SellerId { get; set; }
        public string SellerName { get; set; } = string.Empty;
        public int BuyerId { get; set; }
        public string BuyerName { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; } = string.Empty;
        public string PaymentStatus { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal OrderTotal { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? SellerTransferredAt { get; set; }
        public DateTime? BuyerReceivedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public decimal PlatformFee { get; set; }
    }

    /// <summary>
    /// 評論 DTO
    /// </summary>
    public class ReviewDto
    {
        public int ReviewId { get; set; }
        public int TransactionId { get; set; }
        public int ReviewerId { get; set; }
        public string ReviewerName { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsVerified { get; set; }
    }

    /// <summary>
    /// 評論創建請求 DTO
    /// </summary>
    public class ReviewCreateDto
    {
        [Required(ErrorMessage = "評分不能為空")]
        [Range(1, 5, ErrorMessage = "評分必須在1-5之間")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "評論內容不能為空")]
        [StringLength(500, ErrorMessage = "評論內容長度不能超過500個字符")]
        public string Content { get; set; } = string.Empty;
    }

    /// <summary>
    /// 評論創建結果 DTO
    /// </summary>
    public class ReviewCreateResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public ReviewDto? Review { get; set; }
    }

    /// <summary>
    /// 商品創建（簡化版）
    /// </summary>
    public class ItemCreate
    {
        public string ProductType { get; set; } = string.Empty;
        public string ProductTitle { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public int? OriginalProductId { get; set; }
        public decimal Price { get; set; }
        public List<string> Images { get; set; } = new();
    }

    /// <summary>
    /// 商品更新（簡化版）
    /// </summary>
    public class ItemUpdate
    {
        public string? ProductTitle { get; set; }
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public decimal? Price { get; set; }
        public List<string>? Images { get; set; }
    }

    /// <summary>
    /// 交易評論（簡化版）
    /// </summary>
    public class TransactionReview
    {
        public int Rating { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}