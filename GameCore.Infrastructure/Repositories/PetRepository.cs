using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameCore.Infrastructure.Repositories
{
    public class PetRepository : IPetRepository
    {
        private readonly GameCoreDbContext _context;
        private readonly ILogger<PetRepository> _logger;

        public PetRepository(GameCoreDbContext context, ILogger<PetRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Pet> GetByIdAsync(int id)
        {
            return await _context.Pets
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Pet>> GetByOwnerIdAsync(int ownerId)
        {
            return await _context.Pets
                .Include(p => p.Owner)
                .Where(p => p.OwnerId == ownerId && p.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pet>> GetBySpeciesAsync(string species)
        {
            return await _context.Pets
                .Include(p => p.Owner)
                .Where(p => p.Species == species && p.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pet>> GetByColorAsync(string color)
        {
            return await _context.Pets
                .Include(p => p.Owner)
                .Where(p => p.Color == color && p.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pet>> GetAllAsync()
        {
            return await _context.Pets
                .Include(p => p.Owner)
                .Where(p => p.IsActive)
                .ToListAsync();
        }

        public async Task<Pet> AddAsync(Pet pet)
        {
            await _context.Pets.AddAsync(pet);
            return pet;
        }

        public async Task UpdateAsync(Pet pet)
        {
            _context.Pets.Update(pet);
        }

        public async Task DeleteAsync(int id)
        {
            var pet = await GetByIdAsync(id);
            if (pet != null)
            {
                pet.IsActive = false;
                pet.UpdatedAt = DateTime.UtcNow;
                _context.Pets.Update(pet);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Pets.AnyAsync(p => p.Id == id && p.IsActive);
        }

        public async Task<int> GetPetCountAsync()
        {
            return await _context.Pets.CountAsync(p => p.IsActive);
        }

        public async Task<int> GetPetCountByOwnerAsync(int ownerId)
        {
            return await _context.Pets.CountAsync(p => p.OwnerId == ownerId && p.IsActive);
        }

        public async Task<IEnumerable<Pet>> GetPetsByLevelAsync(int minLevel, int maxLevel)
        {
            return await _context.Pets
                .Include(p => p.Owner)
                .Where(p => p.Level >= minLevel && p.Level <= maxLevel && p.IsActive)
                .OrderByDescending(p => p.Level)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pet>> GetPetsByAgeAsync(int minAge, int maxAge)
        {
            return await _context.Pets
                .Include(p => p.Owner)
                .Where(p => p.Age >= minAge && p.Age <= maxAge && p.IsActive)
                .OrderBy(p => p.Age)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pet>> GetPetsByHealthAsync(int minHealth)
        {
            return await _context.Pets
                .Include(p => p.Owner)
                .Where(p => p.Health >= minHealth && p.IsActive)
                .OrderByDescending(p => p.Health)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pet>> GetPetsByHappinessAsync(int minHappiness)
        {
            return await _context.Pets
                .Include(p => p.Owner)
                .Where(p => p.Happiness >= minHappiness && p.IsActive)
                .OrderByDescending(p => p.Happiness)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pet>> GetPetsByEnergyAsync(int minEnergy)
        {
            return await _context.Pets
                .Include(p => p.Owner)
                .Where(p => p.Energy >= minEnergy && p.IsActive)
                .OrderByDescending(p => p.Energy)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pet>> GetPetsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Pets
                .Include(p => p.Owner)
                .Where(p => p.CreatedAt >= startDate && p.CreatedAt <= endDate && p.IsActive)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pet>> GetPetsByLastFeedAsync(DateTime lastFeed)
        {
            return await _context.Pets
                .Include(p => p.Owner)
                .Where(p => p.LastFeedTime <= lastFeed && p.IsActive)
                .OrderBy(p => p.LastFeedTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pet>> GetPetsByLastCleanAsync(DateTime lastClean)
        {
            return await _context.Pets
                .Include(p => p.Owner)
                .Where(p => p.LastCleanTime <= lastClean && p.IsActive)
                .OrderBy(p => p.LastCleanTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pet>> GetPetsByLastPlayAsync(DateTime lastPlay)
        {
            return await _context.Pets
                .Include(p => p.Owner)
                .Where(p => p.LastPlayTime <= lastPlay && p.IsActive)
                .OrderBy(p => p.LastPlayTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pet>> GetHungryPetsAsync()
        {
            var threshold = DateTime.UtcNow.AddHours(-6);
            return await _context.Pets
                .Include(p => p.Owner)
                .Where(p => p.LastFeedTime <= threshold && p.IsActive)
                .OrderBy(p => p.LastFeedTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pet>> GetDirtyPetsAsync()
        {
            var threshold = DateTime.UtcNow.AddHours(-12);
            return await _context.Pets
                .Include(p => p.Owner)
                .Where(p => p.LastCleanTime <= threshold && p.IsActive)
                .OrderBy(p => p.LastCleanTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pet>> GetBoredPetsAsync()
        {
            var threshold = DateTime.UtcNow.AddHours(-8);
            return await _context.Pets
                .Include(p => p.Owner)
                .Where(p => p.LastPlayTime <= threshold && p.IsActive)
                .OrderBy(p => p.LastPlayTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pet>> GetTiredPetsAsync()
        {
            return await _context.Pets
                .Include(p => p.Owner)
                .Where(p => p.Energy <= 20 && p.IsActive)
                .OrderBy(p => p.Energy)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pet>> GetSickPetsAsync()
        {
            return await _context.Pets
                .Include(p => p.Owner)
                .Where(p => p.Health <= 30 && p.IsActive)
                .OrderBy(p => p.Health)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pet>> GetSadPetsAsync()
        {
            return await _context.Pets
                .Include(p => p.Owner)
                .Where(p => p.Happiness <= 30 && p.IsActive)
                .OrderBy(p => p.Happiness)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pet>> SearchPetsAsync(string searchTerm)
        {
            return await _context.Pets
                .Include(p => p.Owner)
                .Where(p => p.IsActive && 
                           (p.Name.Contains(searchTerm) || 
                            p.Species.Contains(searchTerm) ||
                            p.Color.Contains(searchTerm) ||
                            p.Owner.Username.Contains(searchTerm)))
                .ToListAsync();
        }

        public async Task<Pet> GetActivePetByOwnerAsync(int ownerId)
        {
            return await _context.Pets
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(p => p.OwnerId == ownerId && p.IsActive);
        }

        public async Task<IEnumerable<Pet>> GetTopPetsByLevelAsync(int count)
        {
            return await _context.Pets
                .Include(p => p.Owner)
                .Where(p => p.IsActive)
                .OrderByDescending(p => p.Level)
                .ThenByDescending(p => p.Experience)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pet>> GetRecentlyCreatedPetsAsync(int count)
        {
            return await _context.Pets
                .Include(p => p.Owner)
                .Where(p => p.IsActive)
                .OrderByDescending(p => p.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        public async Task UpdatePetStatsAsync(int petId, int health, int happiness, int energy)
        {
            var pet = await GetByIdAsync(petId);
            if (pet != null)
            {
                pet.Health = Math.Max(0, Math.Min(100, health));
                pet.Happiness = Math.Max(0, Math.Min(100, happiness));
                pet.Energy = Math.Max(0, Math.Min(100, energy));
                pet.UpdatedAt = DateTime.UtcNow;
                _context.Pets.Update(pet);
            }
        }

        public async Task UpdatePetActivityAsync(int petId, string activityType)
        {
            var pet = await GetByIdAsync(petId);
            if (pet != null)
            {
                var now = DateTime.UtcNow;
                switch (activityType.ToLower())
                {
                    case "feed":
                        pet.LastFeedTime = now;
                        break;
                    case "clean":
                        pet.LastCleanTime = now;
                        break;
                    case "play":
                        pet.LastPlayTime = now;
                        break;
                    case "rest":
                        pet.LastRestTime = now;
                        break;
                }
                pet.UpdatedAt = now;
                _context.Pets.Update(pet);
            }
        }

        public async Task AddExperienceAsync(int petId, int experience)
        {
            var pet = await GetByIdAsync(petId);
            if (pet != null)
            {
                pet.Experience += experience;
                
                // Level up logic
                var requiredExp = pet.Level * 100;
                while (pet.Experience >= requiredExp)
                {
                    pet.Experience -= requiredExp;
                    pet.Level++;
                    requiredExp = pet.Level * 100;
                }
                
                pet.UpdatedAt = DateTime.UtcNow;
                _context.Pets.Update(pet);
            }
        }
    }
}