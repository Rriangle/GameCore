using GameCore.Core.Entities;

namespace GameCore.Core.Interfaces
{
    public interface IPetService
    {
        Task<Pet> GetOrCreatePetAsync(int userId);
        Task<PetInteractionResult> InteractWithPetAsync(int userId, PetInteractionType interactionType);
        Task<PetColorChangeResult> ChangePetColorAsync(int userId, string color);
        Task<bool> ApplyDailyDecayAsync(int userId);
        Task<bool> AddExperienceAsync(Pet pet, int experience);
        Task<bool> CanPlayMiniGameAsync(int userId);
        int CalculateRequiredExperience(int currentLevel);
    }

    public enum PetInteractionType
    {
        Feed,
        Bath,
        Play,
        Rest
    }

    public class PetInteractionResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public Pet? Pet { get; set; }
        public bool HealthRestored { get; set; }
    }

    public class PetColorChangeResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int PointsUsed { get; set; }
        public Pet? Pet { get; set; }
        public int RemainingPoints { get; set; }
    }
}