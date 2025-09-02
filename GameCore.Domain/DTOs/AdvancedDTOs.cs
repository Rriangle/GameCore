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

    public class AdventureResult
    {
        public int PetId { get; set; }
        public string AdventureType { get; set; } = string.Empty;
        public bool Success { get; set; }
        public List<string> Rewards { get; set; } = new();
        public int ExperienceGained { get; set; }
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
} 
