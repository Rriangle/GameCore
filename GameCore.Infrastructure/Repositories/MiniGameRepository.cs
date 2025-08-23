using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GameCore.Infrastructure.Repositories
{
    public class MiniGameRepository : Repository<MiniGame>, IMiniGameRepository
    {
        public MiniGameRepository(GameCoreDbContext context) : base(context)
        {
        }

        public async Task<int> GetTodayGameCountAsync(int userId)
        {
            var today = DateTime.Today;
            return await _context.Set<MiniGame>()
                .CountAsync(g => g.UserId == userId && g.ExpGainedTime.HasValue && g.ExpGainedTime.Value.Date == today);
        }

        public async Task<PagedResult<MiniGame>> GetUserGameHistoryAsync(int userId, int page = 1, int pageSize = 20)
        {
            var query = _context.Set<MiniGame>()
                .Where(g => g.UserId == userId)
                .OrderByDescending(g => g.ExpGainedTime);

            var totalCount = await query.CountAsync();
            var games = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<MiniGame>
            {
                Items = games,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<MiniGameStats> GetUserGameStatsAsync(int userId)
        {
            var games = await _context.Set<MiniGame>()
                .Where(g => g.UserId == userId)
                .ToListAsync();

            var stats = new MiniGameStats
            {
                TotalGames = games.Count,
                WinCount = games.Count(g => g.Result == "Win"),
                LoseCount = games.Count(g => g.Result == "Lose"),
                AbortCount = games.Count(g => g.Result == "Abort"),
                TotalExperienceGained = games.Sum(g => g.ExpGained),
                TotalPointsGained = games.Sum(g => g.PointsChanged),
                HighestLevel = games.Max(g => g.Level)
            };

            return stats;
        }
    }
}