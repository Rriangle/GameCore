using GameCore.Domain.Entities;
using GameCore.Domain.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameCore.Infrastructure.Repositories
{
    /// <summary>
    /// 小冒險遊戲倉庫實作
    /// </summary>
    public class MiniGameRepository : Repository<MiniGame>, IMiniGameRepository
    {
        private readonly ILogger<MiniGameRepository> _logger;

        public MiniGameRepository(GameCoreDbContext context, ILogger<MiniGameRepository> logger) : base(context)
        {
            _logger = logger;
        }

        // 實現 IMiniGameRepository 接口的缺少方法
        public async Task<MiniGame?> GetByIdAsync(int id)
        {
            return await _context.MiniGames
                .Include(m => m.User)
                .Include(m => m.Pet)
                .FirstOrDefaultAsync(m => m.PlayId == id);
        }

        public async Task<IEnumerable<MiniGame>> GetByUserIdAsync(int userId)
        {
            return await _context.MiniGames
                .Include(m => m.Pet)
                .Where(m => m.UserId == userId)
                .OrderByDescending(m => m.ExpGainedTime ?? DateTime.MinValue)
                .ToListAsync();
        }

        public async Task<MiniGame> AddAsync(MiniGame miniGame)
        {
            _context.MiniGames.Add(miniGame);
            await _context.SaveChangesAsync();
            return miniGame;
        }

        public async Task<MiniGame> UpdateAsync(MiniGame miniGame)
        {
            _context.MiniGames.Update(miniGame);
            await _context.SaveChangesAsync();
            return miniGame;
        }

        public async Task DeleteAsync(int id)
        {
            var miniGame = await _context.MiniGames.FindAsync(id);
            if (miniGame != null)
            {
                _context.MiniGames.Remove(miniGame);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<MiniGame> Add(MiniGame miniGame)
        {
            _context.MiniGames.Add(miniGame);
            await _context.SaveChangesAsync();
            return miniGame;
        }

        public async Task<bool> HasReachedDailyLimitAsync(int userId)
        {
            var today = DateTime.Today;
            var gameCount = await _context.MiniGames
                .CountAsync(g => g.UserId == userId && 
                                 g.ExpGainedTime.HasValue && 
                                 g.ExpGainedTime.Value.Date == today);

            return gameCount >= 3; // 每日最多 3 次
        }

        public async Task<int> GetTodayGameCountAsync(int userId)
        {
            var today = DateTime.Today;
            return await _context.MiniGames
                .CountAsync(g => g.UserId == userId && 
                                 g.ExpGainedTime.HasValue && 
                                 g.ExpGainedTime.Value.Date == today);
        }

        public async Task<GameSettings?> GetGameSettingsAsync()
        {
            // 返回默認遊戲設定
            return new GameSettings
            {
                GameLevel = 1,
                IsActive = true,
                MaxMonsters = 10,
                SpeedMultiplier = 1.0
            };
        }

        public async Task<GameSettings> GetDefaultGameSettingsAsync()
        {
            return new GameSettings
            {
                GameLevel = 1,
                IsActive = true,
                MaxMonsters = 10,
                SpeedMultiplier = 1.0
            };
        }

        public async Task<IEnumerable<MiniGame>> GetUserGameRecordsAsync(int userId)
        {
            return await _context.MiniGames
                .Include(m => m.Pet)
                .Where(g => g.UserId == userId)
                .OrderByDescending(g => g.ExpGainedTime ?? DateTime.MinValue)
                .ToListAsync();
        }

        public async Task<IEnumerable<MiniGame>> GetPetGameRecordsAsync(int petId)
        {
            return await _context.MiniGames
                .Include(m => m.User)
                .Where(g => g.PetId == petId)
                .OrderByDescending(g => g.ExpGainedTime ?? DateTime.MinValue)
                .ToListAsync();
        }

        // 保留原有的 MiniGameRecord 相關方法（用於向後兼容）
        /// <summary>
        /// 檢查使用者今日是否已達到遊戲次數限制
        /// </summary>
        public async Task<bool> HasReachedDailyLimitAsync(int userId, DateTime date)
        {
            var startOfDay = date.Date;
            var endOfDay = startOfDay.AddDays(1);

            var gameCount = await _context.MiniGameRecords
                .CountAsync(g => g.UserId == userId && 
                                 g.GameDate >= startOfDay && 
                                 g.GameDate < endOfDay);

            return gameCount >= 3; // 每日最多 3 次
        }

        /// <summary>
        /// 取得使用者今日的遊戲次數
        /// </summary>
        public async Task<int> GetTodayGameCountAsync(int userId, DateTime date)
        {
            var startOfDay = date.Date;
            var endOfDay = startOfDay.AddDays(1);

            return await _context.MiniGameRecords
                .CountAsync(g => g.UserId == userId && 
                                 g.GameDate >= startOfDay && 
                                 g.GameDate < endOfDay);
        }

        /// <summary>
        /// 取得遊戲設定
        /// </summary>
        public async Task<MiniGameSettings?> GetGameSettingsAsync(int gameLevel)
        {
            return await _context.MiniGameSettings
                .FirstOrDefaultAsync(s => s.GameLevel == gameLevel && s.IsActive);
        }

        /// <summary>
        /// 取得使用者的遊戲記錄
        /// </summary>
        public async Task<IEnumerable<MiniGameRecord>> GetUserGameRecordsAsync(int userId, int page, int pageSize)
        {
            var skip = (page - 1) * pageSize;

            return await _context.MiniGameRecords
                .Where(g => g.UserId == userId)
                .Include(g => g.Pet)
                .OrderByDescending(g => g.GameDate)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();
        }

        /// <summary>
        /// 取得寵物的遊戲記錄
        /// </summary>
        public async Task<IEnumerable<MiniGameRecord>> GetPetGameRecordsAsync(int petId, int page, int pageSize)
        {
            var skip = (page - 1) * pageSize;

            return await _context.MiniGameRecords
                .Where(g => g.PetId == petId)
                .Include(g => g.User)
                .OrderByDescending(g => g.GameDate)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}