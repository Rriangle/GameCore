using GameCore.Core.Entities;

namespace GameCore.Core.Interfaces
{
    public interface IPetRepository : IRepository<Pet>
    {
        Task<Pet?> GetByUserIdAsync(int userId);
        Task<bool> AddAsync(Pet pet);
        Task<bool> UpdateAsync(Pet pet);
        Task<bool> AddInteractionAsync(PetInteraction interaction);
        Task<PetInteraction?> GetLastInteractionAsync(int userId, PetInteractionType interactionType);
        Task<IEnumerable<PetInteraction>> GetInteractionsByUserIdAsync(int userId, int page = 1, int pageSize = 20);
        Task<bool> UpdateLastInteractionAsync(int userId, PetInteractionType interactionType, DateTime interactionTime);
    }

    public class PetHealthStats
    {
        public int PetId { get; set; }
        public int Health { get; set; }
        public int Hunger { get; set; }
        public int Mood { get; set; }
        public int Stamina { get; set; }
        public int Cleanliness { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}