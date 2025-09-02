using GameCore.Domain.Entities;
using GameCore.Domain.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameCore.Infrastructure.Repositories
{
    public class PrivateChatRepository : Repository<PrivateChat>, IPrivateChatRepository
    {
        private readonly ILogger<PrivateChatRepository> _logger;

        public PrivateChatRepository(GameCoreDbContext context, ILogger<PrivateChatRepository> logger) : base(context)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<PrivateChat>> GetByUserIdAsync(int userId)
        {
            return await _dbSet
                .Include(c => c.User1)
                .Include(c => c.User2)
                .Where(c => c.User1Id == userId || c.User2Id == userId)
                .OrderByDescending(c => c.LastMessageAt)
                .ToListAsync();
        }

        public async Task<PrivateChat?> GetByUsersAsync(int user1Id, int user2Id)
        {
            return await _dbSet
                .Include(c => c.User1)
                .Include(c => c.User2)
                .FirstOrDefaultAsync(c => 
                    (c.User1Id == user1Id && c.User2Id == user2Id) ||
                    (c.User1Id == user2Id && c.User2Id == user1Id));
        }

        public async Task<PrivateChat> AddAsync(PrivateChat chat)
        {
            var result = await _dbSet.AddAsync(chat);
            return result.Entity;
        }

        public Task UpdateAsync(PrivateChat chat)
        {
            _dbSet.Update(chat);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(PrivateChat chat)
        {
            _dbSet.Remove(chat);
            return Task.CompletedTask;
        }

        public async Task<PrivateChat?> GetPrivateChatAsync(int user1Id, int user2Id)
        {
            return await GetByUsersAsync(user1Id, user2Id);
        }

        public async Task<PrivateChat> Add(PrivateChat chat)
        {
            var result = await _dbSet.AddAsync(chat);
            return result.Entity;
        }

        public Task Update(PrivateChat chat)
        {
            _dbSet.Update(chat);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<PrivateChat>> GetPrivateChatsByUserIdAsync(int userId)
        {
            return await GetByUserIdAsync(userId);
        }
    }
} 