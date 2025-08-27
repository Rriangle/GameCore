namespace GameCore.Core.DTOs
{
    // 錢包相關
    public class TransactionResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public decimal NewBalance { get; set; }
        public string TransactionId { get; set; } = string.Empty;
    }

    public class EscrowResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int EscrowId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class EscrowStatusResult
    {
        public int EscrowId { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ReleasedAt { get; set; }
    }

    public class RiskAssessment
    {
        public int UserId { get; set; }
        public decimal RiskScore { get; set; }
        public string RiskLevel { get; set; } = string.Empty;
        public List<string> RiskFactors { get; set; } = new();
    }

    // 商店相關
    public class InventoryReservationResult
    {
        public bool Success { get; set; }
        public string ReservationId { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpiresAt { get; set; }
    }

    public class LowStockAlert
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int CurrentStock { get; set; }
        public int MinStockLevel { get; set; }
        public string AlertLevel { get; set; } = string.Empty;
    }

    public class SearchSuggestionResult
    {
        public string Query { get; set; } = string.Empty;
        public List<string> Suggestions { get; set; } = new();
        public List<string> PopularSearches { get; set; } = new();
    }

    public class PriceHistory
    {
        public int ProductId { get; set; }
        public List<PricePoint> PricePoints { get; set; } = new();
        public decimal AveragePrice { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
    }

    public class PricePoint
    {
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
    }

    public class CompetitivePriceAnalysis
    {
        public int ProductId { get; set; }
        public decimal OurPrice { get; set; }
        public decimal AverageMarketPrice { get; set; }
        public decimal LowestMarketPrice { get; set; }
        public decimal HighestMarketPrice { get; set; }
        public string Recommendation { get; set; } = string.Empty;
    }

    public class QualityScore
    {
        public int ProductId { get; set; }
        public decimal Score { get; set; }
        public List<string> Factors { get; set; } = new();
        public string Grade { get; set; } = string.Empty;
    }

    public class QualityIssue
    {
        public int ProductId { get; set; }
        public string IssueType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;
        public DateTime ReportedAt { get; set; }
    }

    public class ReviewAnalytics
    {
        public int ProductId { get; set; }
        public decimal AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public Dictionary<int, int> RatingDistribution { get; set; } = new();
        public List<string> CommonKeywords { get; set; } = new();
    }

    public class MobileOptimizedProduct
    {
        public int ProductId { get; set; }
        public string OptimizedImageUrl { get; set; } = string.Empty;
        public string MobileDescription { get; set; } = string.Empty;
        public List<string> MobileFeatures { get; set; } = new();
    }

    public class CrossPlatformSyncResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int SyncedItems { get; set; }
        public List<string> Errors { get; set; } = new();
    }

    // 寵物相關
    public class PetBehaviorPrediction
    {
        public int PetId { get; set; }
        public string PredictedBehavior { get; set; } = string.Empty;
        public decimal Confidence { get; set; }
        public List<string> Factors { get; set; } = new();
    }

    public class InteractionFeedback
    {
        public int PetId { get; set; }
        public string InteractionType { get; set; } = string.Empty;
        public string Response { get; set; } = string.Empty;
        public decimal Satisfaction { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class PetEvolution
    {
        public int PetId { get; set; }
        public string CurrentForm { get; set; } = string.Empty;
        public string NextForm { get; set; } = string.Empty;
        public int Progress { get; set; }
        public List<string> Requirements { get; set; } = new();
    }

    public class GrowthMilestone
    {
        public int PetId { get; set; }
        public string MilestoneName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool Achieved { get; set; }
        public DateTime? AchievedAt { get; set; }
    }

    public class SpecialAbility
    {
        public int PetId { get; set; }
        public string AbilityName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool Unlocked { get; set; }
        public int PowerLevel { get; set; }
    }

    public class AdventureResult
    {
        public int PetId { get; set; }
        public string AdventureType { get; set; } = string.Empty;
        public bool Success { get; set; }
        public List<string> Rewards { get; set; } = new();
        public int ExperienceGained { get; set; }
    }

    public class EventChoice
    {
        public int EventId { get; set; }
        public string ChoiceText { get; set; } = string.Empty;
        public string Outcome { get; set; } = string.Empty;
        public List<string> Consequences { get; set; } = new();
    }

    public class AdventureEvent
    {
        public int EventId { get; set; }
        public string EventType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<EventChoice> Choices { get; set; } = new();
    }

    public class Adventure
    {
        public int AdventureId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
        public int Duration { get; set; }
        public List<string> Requirements { get; set; } = new();
    }

    public class PetStats
    {
        public int PetId { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int Energy { get; set; }
        public int MaxEnergy { get; set; }
    }

    public class AdventureLoot
    {
        public int AdventureId { get; set; }
        public List<string> Items { get; set; } = new();
        public int Experience { get; set; }
        public int Gold { get; set; }
        public List<string> SpecialRewards { get; set; } = new();
    }

    public class AdventureOutcome
    {
        public int AdventureId { get; set; }
        public bool Success { get; set; }
        public string Outcome { get; set; } = string.Empty;
        public List<string> Rewards { get; set; } = new();
        public List<string> Penalties { get; set; } = new();
    }

    public class PetPlaydate
    {
        public int PetId { get; set; }
        public int PartnerPetId { get; set; }
        public string Activity { get; set; } = string.Empty;
        public int HappinessGained { get; set; }
        public DateTime ScheduledAt { get; set; }
    }

    public class PetCompetition
    {
        public int CompetitionId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<string> Prizes { get; set; } = new();
    }

    public class PetBreeding
    {
        public int BreedingId { get; set; }
        public int Parent1Id { get; set; }
        public int Parent2Id { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? CompletionDate { get; set; }
    }

    public class CreateGuildRequest
    {
        public string GuildName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int LeaderPetId { get; set; }
        public string GuildType { get; set; } = string.Empty;
    }

    public class PetGuild
    {
        public int GuildId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int MemberCount { get; set; }
        public int MaxMembers { get; set; }
        public string GuildType { get; set; } = string.Empty;
    }

    public class GlobalEvent
    {
        public int EventId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<string> Rewards { get; set; } = new();
    }

    public class SeasonalBonus
    {
        public string Season { get; set; } = string.Empty;
        public string BonusType { get; set; } = string.Empty;
        public decimal BonusAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class DailyChallenge
    {
        public int ChallengeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int TargetValue { get; set; }
        public int CurrentProgress { get; set; }
        public List<string> Rewards { get; set; } = new();
    }

    public class PersonalityEvolution
    {
        public int PetId { get; set; }
        public string CurrentPersonality { get; set; } = string.Empty;
        public string EvolutionPath { get; set; } = string.Empty;
        public int Progress { get; set; }
        public List<string> Triggers { get; set; } = new();
    }

    public class ResponseType
    {
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Probability { get; set; }
    }

    public class EmotionalReaction
    {
        public string Emotion { get; set; } = string.Empty;
        public string Trigger { get; set; } = string.Empty;
        public string Response { get; set; } = string.Empty;
        public int Intensity { get; set; }
    }

    public class PetAction
    {
        public string ActionType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int EnergyCost { get; set; }
        public List<string> Requirements { get; set; } = new();
    }

    public class AIReasoningPath
    {
        public string Question { get; set; } = string.Empty;
        public List<string> ReasoningSteps { get; set; } = new();
        public string Conclusion { get; set; } = string.Empty;
        public decimal Confidence { get; set; }
    }

    public class PetCurrentState
    {
        public int PetId { get; set; }
        public string Mood { get; set; } = string.Empty;
        public string Energy { get; set; } = string.Empty;
        public string Health { get; set; } = string.Empty;
        public List<string> ActiveEffects { get; set; } = new();
    }

    public class AdventureType
    {
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
        public int MinLevel { get; set; }
        public List<string> Rewards { get; set; } = new();
    }

    public class EvolutionRequirement
    {
        public string RequirementType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int CurrentValue { get; set; }
        public int RequiredValue { get; set; }
        public bool Met { get; set; }
    }

    public class EvolutionPath
    {
        public string PathName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<EvolutionRequirement> Requirements { get; set; } = new();
        public List<string> Benefits { get; set; } = new();
    }

    public class AdventureConfiguration
    {
        public string AdventureType { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
        public int Duration { get; set; }
        public List<string> SpecialConditions { get; set; } = new();
    }

    public class DifficultyLevel
    {
        public string Level { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int MinLevel { get; set; }
        public decimal SuccessRate { get; set; }
    }

    public class MonitoringLevel
    {
        public string Level { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Features { get; set; } = new();
    }

    public class AtomicTransactionRequest
    {
        public int UserId { get; set; }
        public List<TransactionStep> Steps { get; set; } = new();
        public string Description { get; set; } = string.Empty;
    }

    public class TransactionStep
    {
        public string Type { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    public class ConcurrentTransactionRequest
    {
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public string TransactionType { get; set; } = string.Empty;
        public string SessionId { get; set; } = string.Empty;
    }

    public class ConcurrencyResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string TransactionId { get; set; } = string.Empty;
        public decimal NewBalance { get; set; }
    }

    public class WalletBalance
    {
        public int UserId { get; set; }
        public decimal AvailableBalance { get; set; }
        public decimal ReservedBalance { get; set; }
        public decimal TotalBalance { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    public class TransactionAnalysisRequest
    {
        public int UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string AnalysisType { get; set; } = string.Empty;
    }

    public class SuspiciousTransaction
    {
        public int TransactionId { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public string Reason { get; set; } = string.Empty;
        public decimal RiskScore { get; set; }
        public DateTime DetectedAt { get; set; }
    }

    public class RiskScore
    {
        public int UserId { get; set; }
        public decimal Score { get; set; }
        public string Level { get; set; } = string.Empty;
        public List<string> Factors { get; set; } = new();
        public DateTime CalculatedAt { get; set; }
    }

    public class EscrowRequest
    {
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public string Purpose { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
    }

    public class EscrowReleaseReason
    {
        public string Reason { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool RequiresApproval { get; set; }
    }

    public class BalanceReconciliationResult
    {
        public int UserId { get; set; }
        public bool Success { get; set; }
        public decimal Discrepancy { get; set; }
        public string Details { get; set; } = string.Empty;
        public DateTime ReconciledAt { get; set; }
    }

    public class WalletAuditTrail
    {
        public int UserId { get; set; }
        public List<AuditEntry> Entries { get; set; } = new();
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class AuditEntry
    {
        public DateTime Timestamp { get; set; }
        public string Action { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public decimal BalanceBefore { get; set; }
        public decimal BalanceAfter { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    public class BalanceSnapshot
    {
        public int UserId { get; set; }
        public decimal Balance { get; set; }
        public DateTime SnapshotDate { get; set; }
        public string Notes { get; set; } = string.Empty;
    }

    public class WalletSecurityReport
    {
        public int UserId { get; set; }
        public decimal SecurityScore { get; set; }
        public List<string> Vulnerabilities { get; set; } = new();
        public List<string> Recommendations { get; set; } = new();
        public DateTime GeneratedAt { get; set; }
    }

    public class SecurityAlert
    {
        public int AlertId { get; set; }
        public int UserId { get; set; }
        public string AlertType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;
        public DateTime DetectedAt { get; set; }
        public bool Resolved { get; set; }
    }

    public class InventoryReservationRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int UserId { get; set; }
        public DateTime ExpiryDate { get; set; }
    }

    public class AdvancedSearchCriteria
    {
        public string Keyword { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public List<string> Tags { get; set; } = new();
        public string SortBy { get; set; } = string.Empty;
        public bool Ascending { get; set; }
    }

    public class SearchFilter
    {
        public string Field { get; set; } = string.Empty;
        public string Operator { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }

    public class SearchSortOptions
    {
        public string Field { get; set; } = string.Empty;
        public bool Ascending { get; set; }
        public int Priority { get; set; }
    }

    public class PaginationOptions
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
    }

    public class SearchPersonalization
    {
        public int UserId { get; set; }
        public List<string> Preferences { get; set; } = new();
        public List<string> SearchHistory { get; set; } = new();
        public Dictionary<string, decimal> CategoryWeights { get; set; } = new();
    }

    public class EscrowCondition
    {
        public string Condition { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool Met { get; set; }
        public DateTime? MetAt { get; set; }
    }

    public class SearchFacet
    {
        public string Field { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class SearchMetadata
    {
        public int TotalResults { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public TimeSpan SearchTime { get; set; }
    }

    public class PopularSearchTerm
    {
        public string Term { get; set; } = string.Empty;
        public int Count { get; set; }
        public DateTime LastSearched { get; set; }
    }

    public class UserDemographics
    {
        public int UserId { get; set; }
        public string AgeGroup { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public List<string> Interests { get; set; } = new();
    }

    public class PriceOptimizationReason
    {
        public string Reason { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Impact { get; set; }
    }

    public class PriceFactor
    {
        public string Factor { get; set; } = string.Empty;
        public decimal Weight { get; set; }
        public string Description { get; set; } = string.Empty;
    }

    public class UserPreferences
    {
        public int UserId { get; set; }
        public List<string> PreferredCategories { get; set; } = new();
        public decimal? MaxPrice { get; set; }
        public List<string> ExcludedTags { get; set; } = new();
        public string SortPreference { get; set; } = string.Empty;
    }

    // 購買相關DTO
    public class PurchaseResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? OrderId { get; set; }
        public decimal TotalAmount { get; set; }
    }

    // 市場交易相關DTO
    public class MarketTransaction
    {
        public int TransactionId { get; set; }
        public int BuyerId { get; set; }
        public int SellerId { get; set; }
        public int ItemId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }

    // 通知相關DTO
    public class NotificationSource
    {
        public int SourceId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsEnabled { get; set; }
    }

    public class NotificationAction
    {
        public int ActionId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ActionType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    // 商店產品相關DTO
    public class StoreProduct
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string Category { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
} 