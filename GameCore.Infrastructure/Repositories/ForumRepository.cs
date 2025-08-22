using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GameCore.Infrastructure.Repositories
{
    /// <summary>
    /// 論壇 Repository 實作
    /// </summary>
    public class ForumRepository : Repository<Forum>, IForumRepository
    {
        public ForumRepository(GameCoreDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Forum>> GetAllWithGamesAsync()
        {
            return await _context.Forums.Include(f => f.Game).ToListAsync();
        }

        public async Task<Forum?> GetByGameIdAsync(int gameId)
        {
            return await _context.Forums.FirstOrDefaultAsync(f => f.GameId == gameId);
        }

        public async Task<IEnumerable<Forum>> SearchAsync(string searchTerm)
        {
            return await _context.Forums.Where(f => f.Name!.Contains(searchTerm)).ToListAsync();
        }

        /// <summary>
        /// 取得論壇及其主題
        /// </summary>
        public async Task<Forum?> GetWithThreadsAsync(int forumId)
        {
            return await _context.Forums
                .Include(f => f.Threads)
                .FirstOrDefaultAsync(f => f.ForumId == forumId);
        }

        /// <summary>
        /// 取得所有論壇
        /// </summary>
        public async Task<List<Forum>> GetAllForumsAsync()
        {
            return await _context.Forums
                .Include(f => f.Game)
                .ToListAsync();
        }
    }
}

