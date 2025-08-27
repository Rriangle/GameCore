using System.ComponentModel.DataAnnotations;

namespace GameCore.Core.DTOs
{
    /// <summary>
    /// 通用結果 DTO
    /// </summary>
    public class ResultDto<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
    }

    /// <summary>
    /// 分頁結果 DTO
    /// </summary>
    public class PagedResultDto<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }

    /// <summary>
    /// 搜尋條件 DTO
    /// </summary>
    public class SearchCriteriaDto
    {
        public string? Keyword { get; set; }
        public string? Category { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    /// <summary>
    /// 排序選項 DTO
    /// </summary>
    public class SortOptionsDto
    {
        public string Field { get; set; } = "created_at";
        public bool Ascending { get; set; } = false;
    }

    /// <summary>
    /// 過濾條件 DTO
    /// </summary>
    public class FilterOptionsDto
    {
        public string? Category { get; set; }
        public string? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    /// <summary>
    /// 統計資料 DTO
    /// </summary>
    public class StatisticsDto
    {
        public int TotalCount { get; set; }
        public int ActiveCount { get; set; }
        public int InactiveCount { get; set; }
        public decimal TotalValue { get; set; }
        public decimal AverageValue { get; set; }
    }

    // 通知、訂單狀態相關定義已移至 BulkDTOs.cs

    /// <summary>
    /// 支付方式列舉
    /// </summary>
    public enum PaymentMethod
    {
        CreditCard,
        DebitCard,
        PayPal,
        BankTransfer,
        DigitalWallet
    }

    /// <summary>
    /// 用戶狀態列舉
    /// </summary>
    public enum UserStatus
    {
        Active,
        Inactive,
        Suspended,
        Banned
    }

    /// <summary>
    /// 產品類別列舉
    /// </summary>
    public enum ProductCategory
    {
        Electronics,
        Clothing,
        Books,
        Sports,
        Home,
        Beauty,
        Toys,
        Food,
        Other
    }

    /// <summary>
    /// 市場項目狀態列舉
    /// </summary>
    public enum MarketItemStatus
    {
        Available,
        Sold,
        Reserved,
        Expired
    }

    /// <summary>
    /// 交易狀態列舉
    /// </summary>
    public enum TransactionStatus
    {
        Pending,
        Completed,
        Failed,
        Cancelled,
        Refunded
    }

    /// <summary>
    /// 評分列舉
    /// </summary>
    public enum Rating
    {
        OneStar = 1,
        TwoStars = 2,
        ThreeStars = 3,
        FourStars = 4,
        FiveStars = 5
    }
} 