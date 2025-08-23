using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GameCore.Infrastructure.Repositories
{
    public class ForumRepository : Repository<Forum>, IForumRepository
    {
        public ForumRepository(GameCoreDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Forum>> GetAllWithGamesAsync()
        {
            return await _context.Set<Forum>()
                .Include(f => f.Game)
                .ToListAsync();
        }

        public async Task<Forum?> GetByGameIdAsync(int gameId)
        {
            return await _context.Set<Forum>()
                .Include(f => f.Game)
                .FirstOrDefaultAsync(f => f.GameId == gameId);
        }
    }
}