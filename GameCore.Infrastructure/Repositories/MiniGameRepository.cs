using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GameCore.Infrastructure.Repositories
{
    /// <summary>
    /// 小冒險遊戲倉庫實作
    /// </summary>
    public class MiniGameRepository : Repository<MiniGameRecord>, IMiniGameRepository
    {
        public MiniGameRepository(GameCoreDbContext context) : base(context)
        {
        }

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