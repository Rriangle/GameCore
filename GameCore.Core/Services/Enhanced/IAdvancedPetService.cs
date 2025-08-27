using GameCore.Core.Entities;

namespace GameCore.Core.Services.Enhanced
{
    public interface IAdvancedPetService : IPetService
    {
        // Advanced Pet AI & Personality
        Task<PetPersonality> GeneratePetPersonalityAsync(int petId);
        Task<PetAIResponse> ProcessPetInteractionAsync(PetInteractionContext context);
        Task<EmotionalState> UpdatePetEmotionalStateAsync(int petId, InteractionType interaction);
        Task<PetBehaviorPrediction> PredictPetBehaviorAsync(int petId, TimeSpan duration);
        Task<bool> LearnFromInteractionAsync(int petId, InteractionFeedback feedback);
        
        // Dynamic Evolution & Growth
        Task<EvolutionResult> CheckEvolutionEligibilityAsync(int petId);
        Task<PetEvolution> EvolvePetAsync(int petId, EvolutionPath path);
        Task<IEnumerable<EvolutionPath>> GetAvailableEvolutionPathsAsync(int petId);
        Task<GrowthMilestone> CheckGrowthMilestonesAsync(int petId);
        Task<SpecialAbility> UnlockSpecialAbilityAsync(int petId, string abilityCode);
        
        // Advanced Adventure System
        Task<AdventureResult> StartAdvancedAdventureAsync(int petId, AdventureConfiguration config);
        Task<AdventureEvent> ProcessAdventureEventAsync(int adventureId, EventChoice choice);
        Task<IEnumerable<Adventure>> GetAvailableAdventuresAsync(int petId, DifficultyLevel difficulty);
        Task<AdventureLoot> CalculateAdventureLootAsync(Adventure adventure, PetStats petStats);
        Task<bool> CompleteAdventureAsync(int adventureId, AdventureOutcome outcome);
        
        // Social Pet Features
        Task<PetPlaydate> ArrangePetPlaydateAsync(int petId1, int petId2);
        Task<PetCompetition> JoinPetCompetitionAsync(int petId, int competitionId);
        Task<PetBreeding> InitiatePetBreedingAsync(int parentPet1, int parentPet2);
        Task<PetGuild> CreatePetGuildAsync(CreateGuildRequest request);
        Task<bool> JoinPetGuildAsync(int petId, int guildId);
        
        // Time Zone & Global Events
        Task<IEnumerable<GlobalEvent>> GetActiveGlobalEventsAsync(TimeZoneInfo userTimeZone);
        Task<bool> ParticipateInGlobalEventAsync(int petId, int eventId);
        Task<SeasonalBonus> CalculateSeasonalBonusAsync(int petId, DateTime currentDate);
        Task<DailyChallenge> GetDailyChallengeAsync(int userId, DateTime date);
        Task<bool> CompleteDailyChallengeAsync(int petId, int challengeId);
    }

    // Pet Service Models
    public class PetPersonality
    {
        public int PetId { get; set; }
        public PersonalityType PrimaryType { get; set; }
        public List<PersonalityTrait> Traits { get; set; } = new();
        public Dictionary<string, double> TraitScores { get; set; } = new();
        public PersonalityEvolution Evolution { get; set; } = new();
        public DateTime GeneratedAt { get; set; }
        public double StabilityScore { get; set; }
    }

    public class PetAIResponse
    {
        public string Response { get; set; } = string.Empty;
        public ResponseType Type { get; set; }
        public EmotionalReaction EmotionalReaction { get; set; } = new();
        public List<PetAction> SuggestedActions { get; set; } = new();
        public double ConfidenceScore { get; set; }
        public AIReasoningPath ReasoningPath { get; set; } = new();
    }

    public class PetInteractionContext
    {
        public int PetId { get; set; }
        public int UserId { get; set; }
        public InteractionType Type { get; set; }
        public string Context { get; set; } = string.Empty;
        public PetCurrentState CurrentState { get; set; } = new();
        public EnvironmentalFactors Environment { get; set; } = new();
        public DateTime InteractionTime { get; set; }
        public Dictionary<string, object>? Metadata { get; set; }
    }

    public class EvolutionResult
    {
        public bool CanEvolve { get; set; }
        public List<EvolutionRequirement> Requirements { get; set; } = new();
        public List<EvolutionPath> AvailablePaths { get; set; } = new();
        public TimeSpan EstimatedTimeToEvolution { get; set; }
        public double EvolutionProgress { get; set; }
    }

    public class AdventureConfiguration
    {
        public DifficultyLevel Difficulty { get; set; }
        public AdventureType Type { get; set; }
        public TimeSpan Duration { get; set; }
        public List<string> PreferredBiomes { get; set; } = new();
        public bool AllowRandomEvents { get; set; } = true;
        public Dictionary<string, object> CustomParameters { get; set; } = new();
    }

    public enum PersonalityType
    {
        Friendly = 1,
        Energetic = 2,
        Calm = 3,
        Playful = 4,
        Loyal = 5,
        Independent = 6,
        Curious = 7,
        Protective = 8
    }

    public enum InteractionType
    {
        Feed = 1,
        Play = 2,
        Bath = 3,
        Rest = 4,
        Train = 5,
        Adventure = 6
    }

    public enum DifficultyLevel
    {
        Beginner = 1,
        Easy = 2,
        Normal = 3,
        Hard = 4,
        Expert = 5,
        Master = 6
    }
} 