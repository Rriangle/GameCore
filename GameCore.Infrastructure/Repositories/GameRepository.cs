using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GameCore.Infrastructure.Repositories
{
    /// <summary>
    /// 遊戲 Repository 實作
    /// </summary>
    public class GameRepository : Repository<Game>, IGameRepository
    {
        public GameRepository(GameCoreDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Game>> GetByGenreAsync(string genre)
        {
            return await _context.Games.Where(g => g.Genre == genre).ToListAsync();
        }

        public async Task<IEnumerable<Game>> SearchGamesAsync(string searchTerm)
        {
            return await _context.Games.Where(g => g.Name!.Contains(searchTerm)).ToListAsync();
        }

        public async Task<IEnumerable<Game>> GetPopularGamesAsync(int count)
        {
            return await _context.Games
                .Take(count)
                .ToListAsync();
        }

        public async Task<Game?> GetWithMetricsAsync(int gameId)
        {
            return await _context.Games.Include(g => g.GameMetricDailies).FirstOrDefaultAsync(g => g.GameId == gameId);
        }

        /// <summary>
        /// 根據分類取得遊戲
        /// </summary>
        public async Task<List<Game>> GetGamesByCategoryAsync(string category)
        {
            return await _context.Games
                .Where(g => g.Genre == category)
                .ToListAsync();
        }
    }
}

