using GameCore.Domain.Entities;
using GameCore.Domain.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameCore.Infrastructure.Repositories
{
    public class GameRepository : Repository<Game>, IGameRepository
    {
        private readonly ILogger<GameRepository> _logger;

        public GameRepository(GameCoreDbContext context, ILogger<GameRepository> logger) : base(context)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<Game>> GetAllGamesAsync()
        {
            return await _dbSet
                .Include(g => g.Category)
                .OrderBy(g => g.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Game>> GetGamesByCategoryAsync(string category)
        {
            return await _dbSet
                .Include(g => g.Category)
                .Where(g => g.Category.Name == category)
                .OrderBy(g => g.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Game>> SearchGamesAsync(string keyword)
        {
            return await _dbSet
                .Include(g => g.Category)
                .Where(g => g.Name.Contains(keyword) || g.Description.Contains(keyword))
                .OrderBy(g => g.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Game>> GetPopularGamesAsync(int limit = 10)
        {
            return await _dbSet
                .Include(g => g.Category)
                .OrderByDescending(g => g.PlayCount)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<IEnumerable<Game>> GetLatestGamesAsync(int limit = 10)
        {
            return await _dbSet
                .Include(g => g.Category)
                .OrderByDescending(g => g.CreatedAt)
                .Take(limit)
                .ToListAsync();
        }
    }
} 