using System;
using System.Collections.Generic;

namespace GameCore.Core.Types
{
    /// <summary>
    /// 分頁結果
    /// </summary>
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
    }

    /// <summary>
    /// 商品統計
    /// </summary>
    public class ProductStatistics
    {
        public int TotalProducts { get; set; }
        public int ActiveProducts { get; set; }
        public int LowStockProducts { get; set; }
        public int OutOfStockProducts { get; set; }
        public decimal TotalValue { get; set; }
        public decimal AveragePrice { get; set; }
        public Dictionary<string, int> CategoryDistribution { get; set; } = new();
        public Dictionary<string, decimal> SalesByCategory { get; set; } = new();
        public List<TopProduct> TopSellingProducts { get; set; } = new();
        public DateTime LastUpdated { get; set; }
    }

    /// <summary>
    /// 熱門商品
    /// </summary>
    public class TopProduct
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int SalesCount { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal AverageRating { get; set; }
        public int ReviewCount { get; set; }
    }

    /// <summary>
    /// 商品搜尋結果
    /// </summary>
    public class ProductSearchResult
    {
        public List<Product> Products { get; set; } = new();
        public int TotalCount { get; set; }
        public Dictionary<string, int> Facets { get; set; } = new();
        public List<string> Suggestions { get; set; } = new();
        public TimeSpan SearchTime { get; set; }
    }

    /// <summary>
    /// 商品
    /// </summary>
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductType { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string CurrencyCode { get; set; } = "TWD";
        public int ShipmentQuantity { get; set; }
        public string ProductCreatedBy { get; set; } = string.Empty;
        public DateTime ProductCreatedAt { get; set; }
        public string ProductUpdatedBy { get; set; } = string.Empty;
        public DateTime ProductUpdatedAt { get; set; }
        public int UserId { get; set; }
        public bool IsActive { get; set; } = true;
        public string Description { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new();
        public decimal? DiscountPrice { get; set; }
        public DateTime? DiscountEndDate { get; set; }
    }



    /// <summary>
    /// 購物車項目
    /// </summary>
    public class CartItem
    {
        /// <summary>
        /// 購物車項目ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用戶ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 數量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 單價
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 總價
        /// </summary>
        public decimal TotalPrice => UnitPrice * Quantity;

        /// <summary>
        /// 加入時間
        /// </summary>
        public DateTime AddedAt { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// 商品資訊
        /// </summary>
        public Product? Product { get; set; }
    }

    /// <summary>
    /// 購物車
    /// </summary>
    public class Cart
    {
        /// <summary>
        /// 購物車ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用戶ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 購物車項目列表
        /// </summary>
        public List<CartItem> Items { get; set; } = new();

        /// <summary>
        /// 總項目數
        /// </summary>
        public int TotalItems => Items.Sum(item => item.Quantity);

        /// <summary>
        /// 總價格
        /// </summary>
        public decimal TotalPrice => Items.Sum(item => item.TotalPrice);

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// 是否為訪客購物車
        /// </summary>
        public bool IsGuestCart { get; set; }

        /// <summary>
        /// 訪客ID
        /// </summary>
        public string? GuestId { get; set; }
    }

    /// <summary>
    /// 訂單
    /// </summary>
    public class Order
    {
        /// <summary>
        /// 訂單ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 訂單編號
        /// </summary>
        public string OrderNumber { get; set; } = string.Empty;

        /// <summary>
        /// 用戶ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 訂單狀態
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// 訂單項目列表
        /// </summary>
        public List<OrderItem> Items { get; set; } = new();

        /// <summary>
        /// 訂單總價
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 運費
        /// </summary>
        public decimal ShippingFee { get; set; }

        /// <summary>
        /// 稅金
        /// </summary>
        public decimal Tax { get; set; }

        /// <summary>
        /// 折扣金額
        /// </summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// 最終金額
        /// </summary>
        public decimal FinalAmount => TotalAmount + ShippingFee + Tax - DiscountAmount;

        /// <summary>
        /// 付款方式
        /// </summary>
        public string PaymentMethod { get; set; } = string.Empty;

        /// <summary>
        /// 付款狀態
        /// </summary>
        public string PaymentStatus { get; set; } = string.Empty;

        /// <summary>
        /// 收貨地址
        /// </summary>
        public string ShippingAddress { get; set; } = string.Empty;

        /// <summary>
        /// 聯絡電話
        /// </summary>
        public string ContactPhone { get; set; } = string.Empty;

        /// <summary>
        /// 備註
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// 付款時間
        /// </summary>
        public DateTime? PaidAt { get; set; }

        /// <summary>
        /// 出貨時間
        /// </summary>
        public DateTime? ShippedAt { get; set; }

        /// <summary>
        /// 完成時間
        /// </summary>
        public DateTime? CompletedAt { get; set; }
    }

    /// <summary>
    /// 訂單項目
    /// </summary>
    public class OrderItem
    {
        /// <summary>
        /// 訂單項目ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 訂單ID
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// 商品ID
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
        /// 總價
        /// </summary>
        public decimal TotalPrice => UnitPrice * Quantity;

        /// <summary>
        /// 商品資訊
        /// </summary>
        public Product? Product { get; set; }
    }

    /// <summary>
    /// 庫存狀態
    /// </summary>
    public class InventoryStatus
    {
        /// <summary>
        /// 商品編號
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// 當前庫存
        /// </summary>
        public int CurrentStock { get; set; }

        /// <summary>
        /// 預留庫存
        /// </summary>
        public int ReservedStock { get; set; }

        /// <summary>
        /// 可用庫存
        /// </summary>
        public int AvailableStock { get; set; }

        /// <summary>
        /// 庫存狀態
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// 庫存警告閾值
        /// </summary>
        public int WarningThreshold { get; set; }

        /// <summary>
        /// 是否庫存不足
        /// </summary>
        public bool IsLowStock { get; set; }
    }

    /// <summary>
    /// 進階搜尋條件
    /// </summary>
    public class AdvancedSearchCriteria
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
        /// 品牌
        /// </summary>
        public string? Brand { get; set; }

        /// <summary>
        /// 最低價格
        /// </summary>
        public decimal? MinPrice { get; set; }

        /// <summary>
        /// 最高價格
        /// </summary>
        public decimal? MaxPrice { get; set; }

        /// <summary>
        /// 評分
        /// </summary>
        public int? MinRating { get; set; }

        /// <summary>
        /// 庫存狀態
        /// </summary>
        public string? StockStatus { get; set; }

        /// <summary>
        /// 排序方式
        /// </summary>
        public string? SortBy { get; set; }

        /// <summary>
        /// 排序順序
        /// </summary>
        public string? SortOrder { get; set; }

        /// <summary>
        /// 頁碼
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// 每頁數量
        /// </summary>
        public int PageSize { get; set; } = 20;
    }

    /// <summary>
    /// 搜尋結果
    /// </summary>
    public class SearchResult
    {
        /// <summary>
        /// 商品列表
        /// </summary>
        public List<object> Products { get; set; } = new List<object>();

        /// <summary>
        /// 總數量
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 頁碼
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 每頁數量
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 總頁數
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// 搜尋時間
        /// </summary>
        public TimeSpan SearchTime { get; set; }

        /// <summary>
        /// 篩選條件
        /// </summary>
        public Dictionary<string, object> AppliedFilters { get; set; } = new Dictionary<string, object>();
    }

    /// <summary>
    /// 商品分析
    /// </summary>
    public class ProductAnalytics
    {
        /// <summary>
        /// 商品編號
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// 總銷售額
        /// </summary>
        public decimal TotalSales { get; set; }

        /// <summary>
        /// 總銷售數量
        /// </summary>
        public int TotalQuantity { get; set; }

        /// <summary>
        /// 平均評分
        /// </summary>
        public decimal AverageRating { get; set; }

        /// <summary>
        /// 總評論數
        /// </summary>
        public int TotalReviews { get; set; }

        /// <summary>
        /// 瀏覽次數
        /// </summary>
        public int ViewCount { get; set; }

        /// <summary>
        /// 收藏次數
        /// </summary>
        public int FavoriteCount { get; set; }

        /// <summary>
        /// 轉換率
        /// </summary>
        public decimal ConversionRate { get; set; }

        /// <summary>
        /// 分析時間範圍
        /// </summary>
        public TimeSpan AnalysisPeriod { get; set; }
    }

    /// <summary>
    /// 價格優化結果
    /// </summary>
    public class PriceOptimizationResult
    {
        /// <summary>
        /// 商品編號
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// 當前價格
        /// </summary>
        public decimal CurrentPrice { get; set; }

        /// <summary>
        /// 建議價格
        /// </summary>
        public decimal RecommendedPrice { get; set; }

        /// <summary>
        /// 價格調整幅度
        /// </summary>
        public decimal PriceAdjustment { get; set; }

        /// <summary>
        /// 調整原因
        /// </summary>
        public string AdjustmentReason { get; set; } = string.Empty;

        /// <summary>
        /// 預期影響
        /// </summary>
        public string ExpectedImpact { get; set; } = string.Empty;

        /// <summary>
        /// 信心度
        /// </summary>
        public decimal Confidence { get; set; }

        /// <summary>
        /// 建議實施時間
        /// </summary>
        public DateTime? RecommendedImplementationTime { get; set; }
    }

    /// <summary>
    /// 價格歷史
    /// </summary>
    public class PriceHistory
    {
        /// <summary>
        /// 記錄編號
        /// </summary>
        public int RecordId { get; set; }

        /// <summary>
        /// 商品編號
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 價格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 價格類型
        /// </summary>
        public string PriceType { get; set; } = string.Empty;

        /// <summary>
        /// 變更原因
        /// </summary>
        public string ChangeReason { get; set; } = string.Empty;

        /// <summary>
        /// 變更時間
        /// </summary>
        public DateTime ChangedAt { get; set; }

        /// <summary>
        /// 變更者
        /// </summary>
        public string ChangedBy { get; set; } = string.Empty;
    }

    /// <summary>
    /// 快速購買商品
    /// </summary>
    public class QuickBuyProduct
    {
        /// <summary>
        /// 商品編號
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// 商品圖片
        /// </summary>
        public string ImageUrl { get; set; } = string.Empty;

        /// <summary>
        /// 價格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 庫存狀態
        /// </summary>
        public string StockStatus { get; set; } = string.Empty;

        /// <summary>
        /// 評分
        /// </summary>
        public decimal Rating { get; set; }

        /// <summary>
        /// 評論數
        /// </summary>
        public int ReviewCount { get; set; }

        /// <summary>
        /// 是否可快速購買
        /// </summary>
        public bool IsQuickBuyEnabled { get; set; }

        /// <summary>
        /// 快速購買限制
        /// </summary>
        public int QuickBuyLimit { get; set; }
    }
} 
