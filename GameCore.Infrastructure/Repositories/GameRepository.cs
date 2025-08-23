using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GameCore.Infrastructure.Repositories
{
    public class GameRepository : Repository<Game>, IGameRepository
    {
        public GameRepository(GameCoreDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Game>> GetByGenreAsync(string genre)
        {
            return await _context.Set<Game>()
                .Where(g => g.Genre == genre)
                .ToListAsync();
        }

        public async Task<IEnumerable<Game>> SearchGamesAsync(string searchTerm)
        {
            return await _context.Set<Game>()
                .Where(g => g.Title.Contains(searchTerm) || g.Description.Contains(searchTerm))
                .ToListAsync();
        }
    }
}