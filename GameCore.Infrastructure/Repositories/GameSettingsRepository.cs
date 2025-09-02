using GameCore.Domain.Entities;
using GameCore.Domain.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameCore.Infrastructure.Repositories
{
    public class GameSettingsRepository : Repository<GameSettings>, IGameSettingsRepository
    {
        private readonly ILogger<GameSettingsRepository> _logger;

        public GameSettingsRepository(GameCoreDbContext context, ILogger<GameSettingsRepository> logger) : base(context)
        {
            _logger = logger;
        }

        public async Task<string?> GetSettingValueAsync(string key)
        {
            var setting = await _dbSet
                .FirstOrDefaultAsync(s => s.Key == key);
            return setting?.Value;
        }

        public async Task<bool> SetSettingValueAsync(string key, string value)
        {
            var setting = await _dbSet
                .FirstOrDefaultAsync(s => s.Key == key);

            if (setting != null)
            {
                setting.Value = value;
                setting.UpdatedAt = DateTime.UtcNow;
                _dbSet.Update(setting);
            }
            else
            {
                var newSetting = new GameSettings
                {
                    Key = key,
                    Value = value,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                await _dbSet.AddAsync(newSetting);
            }
            return true;
        }

        public async Task<IEnumerable<GameSettings>> GetActiveSettingsAsync()
        {
            return await _dbSet
                .Where(s => s.IsActive)
                .OrderBy(s => s.Key)
                .ToListAsync();
        }

        public async Task<bool> SettingExistsAsync(string key)
        {
            return await _dbSet
                .AnyAsync(s => s.Key == key);
        }

        public async Task<bool> DeleteSettingAsync(string key)
        {
            var setting = await _dbSet
                .FirstOrDefaultAsync(s => s.Key == key);

            if (setting != null)
            {
                _dbSet.Remove(setting);
                return true;
            }
            return false;
        }

        public async Task<GameSettings?> GetGameSettingsAsync()
        {
            return await _dbSet
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<MiniGameRecord>> GetUserGameRecordsAsync(int userId, int page = 1, int pageSize = 20)
        {
            return await _context.Set<MiniGameRecord>()
                .Include(r => r.Game)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<MiniGameRecord>> GetPetGameRecordsAsync(int petId, int page = 1, int pageSize = 20)
        {
            return await _context.Set<MiniGameRecord>()
                .Include(r => r.Game)
                .Where(r => r.PetId == petId)
                .OrderByDescending(r => r.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<bool> HasReachedDailyLimitAsync(int userId, int gameId)
        {
            var today = DateTime.UtcNow.Date;
            var count = await _context.Set<MiniGameRecord>()
                .CountAsync(r => r.UserId == userId && 
                                r.GameId == gameId && 
                                r.CreatedAt.Date == today);
            
            var limit = await GetSettingValueAsync($"DailyLimit_{gameId}");
            if (int.TryParse(limit, out var dailyLimit))
            {
                return count >= dailyLimit;
            }
            return false;
        }

        public async Task<int> GetTodayGameCountAsync(int userId, int gameId)
        {
            var today = DateTime.UtcNow.Date;
            return await _context.Set<MiniGameRecord>()
                .CountAsync(r => r.UserId == userId && 
                                r.GameId == gameId && 
                                r.CreatedAt.Date == today);
        }
    }
} 