using GameCore.Domain.Entities;
using GameCore.Domain.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameCore.Infrastructure.Repositories
{
    public class UserRightsRepository : Repository<UserRights>, IUserRightsRepository
    {
        private readonly ILogger<UserRightsRepository> _logger;

        public UserRightsRepository(GameCoreDbContext context, ILogger<UserRightsRepository> logger) : base(context)
        {
            _logger = logger;
        }

        public async Task<UserRights?> GetByUserIdAsync(int userId)
        {
            return await _dbSet
                .Include(r => r.User)
                .Include(r => r.RightType)
                .FirstOrDefaultAsync(r => r.UserId == userId);
        }

        public async Task<IEnumerable<UserRights>> GetByRightTypeAsync(string rightType)
        {
            return await _dbSet
                .Include(r => r.User)
                .Include(r => r.RightType)
                .Where(r => r.RightType == rightType)
                .ToListAsync();
        }

        public async Task<UserRights> AddAsync(UserRights userRights)
        {
            var result = await _dbSet.AddAsync(userRights);
            return result.Entity;
        }

        public Task UpdateAsync(UserRights userRights)
        {
            _dbSet.Update(userRights);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(UserRights userRights)
        {
            _dbSet.Remove(userRights);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<UserRights>> GetAll()
        {
            return await _dbSet
                .Include(r => r.User)
                .Include(r => r.RightType)
                .OrderBy(r => r.UserId)
                .ToListAsync();
        }
    }
} 
