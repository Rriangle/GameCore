using GameCore.Core.Entities;

namespace GameCore.Core.Services.Enhanced
{
    public interface IAdvancedStoreService : IProductService
    {
        // Advanced Inventory Management
        Task<InventoryReservationResult> ReserveInventoryAsync(InventoryReservationRequest request);
        Task<bool> ReleaseInventoryReservationAsync(string reservationId);
        Task<InventoryStatus> GetRealTimeInventoryStatusAsync(int productId);
        Task<bool> AutoReplenishInventoryAsync(int productId);
        Task<IEnumerable<LowStockAlert>> GetLowStockAlertsAsync();
        
        // Intelligent Search & Discovery
        Task<SearchResult> AdvancedSearchAsync(AdvancedSearchCriteria criteria);
        Task<IEnumerable<Product>> GetSimilarProductsAsync(int productId, int count = 10);
        Task<IEnumerable<Product>> GetTrendingProductsAsync(TimeSpan period, int count = 20);
        Task<SearchSuggestionResult> GetSearchSuggestionsAsync(string query);
        Task<bool> UpdateSearchIndexAsync(int productId);
        
        // Personalized Recommendations
        Task<IEnumerable<Product>> GetPersonalizedRecommendationsAsync(int userId, int count = 15);
        Task<IEnumerable<Product>> GetRecommendationsBasedOnHistoryAsync(int userId);
        Task<IEnumerable<Product>> GetRecommendationsBasedOnSimilarUsersAsync(int userId);
        Task<bool> UpdateUserPreferencesAsync(int userId, UserPreferences preferences);
        Task<ProductAnalytics> GetProductAnalyticsAsync(int productId, TimeSpan period);
        
        // Dynamic Pricing & Optimization
        Task<PriceOptimizationResult> OptimizePricingAsync(int productId);
        Task<decimal> CalculateDynamicPriceAsync(int productId, int userId);
        Task<IEnumerable<PriceHistory>> GetPriceHistoryAsync(int productId, TimeSpan period);
        Task<bool> ApplyPersonalizedDiscountAsync(int userId, int productId, decimal discountPercentage);
        Task<CompetitivePriceAnalysis> AnalyzeCompetitivePricingAsync(int productId);
        
        // Quality Assurance & Reviews
        Task<QualityScore> CalculateProductQualityScoreAsync(int productId);
        Task<bool> ValidateProductQualityAsync(int productId);
        Task<IEnumerable<QualityIssue>> GetQualityIssuesAsync(int productId);
        Task<ReviewAnalytics> GetReviewAnalyticsAsync(int productId);
        Task<bool> FlagSuspiciousReviewAsync(int reviewId, string reason);
        
        // Mobile & Cross-Platform Optimization
        Task<MobileOptimizedProduct> GetMobileOptimizedProductAsync(int productId);
        Task<IEnumerable<Product>> GetQuickBuyProductsAsync(int userId);
        Task<bool> OptimizeForMobilePerformanceAsync(int productId);
        Task<CrossPlatformSyncResult> SyncProductDataAsync(int productId);
    }

    // Advanced Store Models
    public class InventoryReservationRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int UserId { get; set; }
        public TimeSpan ReservationDuration { get; set; } = TimeSpan.FromMinutes(15);
        public string ReservationType { get; set; } = "purchase";
        public Dictionary<string, object> Metadata { get; set; } = new();
    }

    public class InventoryStatus
    {
        public int ProductId { get; set; }
        public int AvailableQuantity { get; set; }
        public int ReservedQuantity { get; set; }
        public int TotalQuantity { get; set; }
        public List<InventoryReservation> ActiveReservations { get; set; } = new();
        public DateTime LastUpdated { get; set; }
        public InventoryHealth Health { get; set; }
    }

    public class AdvancedSearchCriteria
    {
        public string SearchTerm { get; set; } = string.Empty;
        public List<SearchFilter> Filters { get; set; } = new();
        public SearchSortOptions SortBy { get; set; } = new();
        public PaginationOptions Pagination { get; set; } = new();
        public bool IncludeSuggestions { get; set; } = true;
        public SearchPersonalization Personalization { get; set; } = new();
    }

    public class SearchResult
    {
        public List<Product> Products { get; set; } = new();
        public int TotalCount { get; set; }
        public List<SearchFacet> Facets { get; set; } = new();
        public List<string> Suggestions { get; set; } = new();
        public SearchMetadata Metadata { get; set; } = new();
        public TimeSpan SearchTime { get; set; }
    }

    public class ProductAnalytics
    {
        public int ProductId { get; set; }
        public int ViewCount { get; set; }
        public int PurchaseCount { get; set; }
        public decimal ConversionRate { get; set; }
        public decimal AverageRating { get; set; }
        public int ReviewCount { get; set; }
        public List<PopularSearchTerm> PopularSearchTerms { get; set; } = new();
        public UserDemographics Demographics { get; set; } = new();
    }

    public class PriceOptimizationResult
    {
        public int ProductId { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal OptimizedPrice { get; set; }
        public decimal ExpectedRevenueLift { get; set; }
        public PriceOptimizationReason Reason { get; set; }
        public List<PriceFactor> InfluencingFactors { get; set; } = new();
        public DateTime ValidUntil { get; set; }
    }

    public enum InventoryHealth
    {
        Healthy = 1,
        Low = 2,
        Critical = 3,
        OutOfStock = 4
    }
} 