using System;
using System.Collections.Generic;

namespace GameCore.Core.DTOs
{
    /// <summary>
    /// 玩家市場商品 DTO
    /// </summary>
    public class PlayerMarketItem
    {
        public int ItemId { get; set; }
        public int SellerId { get; set; }
        public string SellerName { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Condition { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public DateTime ListedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public string Status { get; set; } = string.Empty;
        public List<string> Images { get; set; } = new List<string>();
        public int ViewCount { get; set; }
        public int FavoriteCount { get; set; }
    }

    /// <summary>
    /// 創建市場商品 DTO
    /// </summary>
    public class CreateMarketItemDto
    {
        public string ItemName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Condition { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public List<string> Images { get; set; } = new List<string>();
    }

    /// <summary>
    /// 市場商品創建結果
    /// </summary>
    public class CreateMarketItemResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? ItemId { get; set; }
        public PlayerMarketItem? Item { get; set; }
    }

    /// <summary>
    /// 更新市場商品 DTO
    /// </summary>
    public class UpdateMarketItemDto
    {
        public string ItemName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Condition { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public List<string> Images { get; set; } = new List<string>();
    }

    /// <summary>
    /// 市場商品更新結果
    /// </summary>
    public class UpdateMarketItemResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public PlayerMarketItem? Item { get; set; }
    }

    /// <summary>
    /// 市場商品搜尋 DTO
    /// </summary>
    public class MarketItemSearchDto
    {
        public string? Keyword { get; set; }
        public string? Category { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? Condition { get; set; }
        public string? SortBy { get; set; }
        public string? SortOrder { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    /// <summary>
    /// 市場商品搜尋結果
    /// </summary>
    public class MarketItemSearchResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<PlayerMarketItem> Items { get; set; } = new List<PlayerMarketItem>();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }

    /// <summary>
    /// 購買市場商品 DTO
    /// </summary>
    public class PurchaseMarketItemDto
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
    }

    /// <summary>
    /// 購買市場商品結果
    /// </summary>
    public class PurchaseMarketItemResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? TransactionId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PlatformFee { get; set; }
        public DateTime PurchaseTime { get; set; }
    }

    /// <summary>
    /// 市場交易記錄 DTO
    /// </summary>
    public class MarketTransactionDto
    {
        public int TransactionId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public int SellerId { get; set; }
        public string SellerName { get; set; } = string.Empty;
        public int BuyerId { get; set; }
        public string BuyerName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PlatformFee { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime TransactionTime { get; set; }
        public DateTime? CompletedTime { get; set; }
    }

    /// <summary>
    /// 市場評價 DTO
    /// </summary>
    public class MarketReviewDto
    {
        public int ReviewId { get; set; }
        public int TransactionId { get; set; }
        public int ReviewerId { get; set; }
        public string ReviewerName { get; set; } = string.Empty;
        public int RevieweeId { get; set; }
        public string RevieweeName { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime ReviewTime { get; set; }
        public string ReviewType { get; set; } = string.Empty; // Buyer/Seller
    }

    /// <summary>
    /// 創建市場評價 DTO
    /// </summary>
    public class CreateMarketReviewDto
    {
        public int TransactionId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
    }

    /// <summary>
    /// 市場評價創建結果
    /// </summary>
    public class CreateMarketReviewResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? ReviewId { get; set; }
        public MarketReviewDto? Review { get; set; }
    }

    /// <summary>
    /// 市場統計 DTO
    /// </summary>
    public class MarketStatsDto
    {
        public int TotalItems { get; set; }
        public int ActiveItems { get; set; }
        public int TotalTransactions { get; set; }
        public decimal TotalVolume { get; set; }
        public decimal PlatformRevenue { get; set; }
        public int TotalUsers { get; set; }
        public List<CategoryStats> CategoryStats { get; set; } = new List<CategoryStats>();
    }

    /// <summary>
    /// 類別統計
    /// </summary>
    public class CategoryStats
    {
        public string Category { get; set; } = string.Empty;
        public int ItemCount { get; set; }
        public decimal TotalValue { get; set; }
        public int TransactionCount { get; set; }
    }

    // 商品狀態和交易狀態已移至 CommonDTOs.cs

    /// <summary>
    /// 商品條件
    /// </summary>
    public enum ItemCondition
    {
        New,
        LikeNew,
        Good,
        Fair,
        Poor
    }
} 