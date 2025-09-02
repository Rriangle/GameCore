using GameCore.Domain.Entities;
using GameCore.Domain.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameCore.Infrastructure.Repositories
{
    public class ReplyRepository : Repository<Reply>, IReplyRepository
    {
        private readonly ILogger<ReplyRepository> _logger;

        public ReplyRepository(GameCoreDbContext context, ILogger<ReplyRepository> logger) : base(context)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<Reply>> GetByThreadIdAsync(long threadId)
        {
            return await _dbSet
                .Include(r => r.Author)
                .Include(r => r.Thread)
                .Where(r => r.ThreadId == threadId)
                .OrderBy(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reply>> GetByAuthorIdAsync(int authorId)
        {
            return await _dbSet
                .Include(r => r.Thread)
                .Where(r => r.AuthorId == authorId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<Reply> AddAsync(Reply reply)
        {
            var result = await _dbSet.AddAsync(reply);
            return result.Entity;
        }

        public Task UpdateAsync(Reply reply)
        {
            _dbSet.Update(reply);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Reply reply)
        {
            _dbSet.Remove(reply);
            return Task.CompletedTask;
        }
    }
} 