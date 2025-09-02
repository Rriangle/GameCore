using GameCore.Domain.Entities;
using GameCore.Domain.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameCore.Infrastructure.Repositories
{
    public class MemberSalesProfileRepository : Repository<MemberSalesProfile>, IMemberSalesProfileRepository
    {
        private readonly ILogger<MemberSalesProfileRepository> _logger;

        public MemberSalesProfileRepository(GameCoreDbContext context, ILogger<MemberSalesProfileRepository> logger) : base(context)
        {
            _logger = logger;
        }

        public async Task<MemberSalesProfile?> GetByUserIdAsync(int userId)
        {
            return await _dbSet
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public async Task<IEnumerable<MemberSalesProfile>> GetByStatusAsync(string status)
        {
            return await _dbSet
                .Include(p => p.User)
                .Where(p => p.Status == status)
                .OrderBy(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<MemberSalesProfile> AddAsync(MemberSalesProfile profile)
        {
            var result = await _dbSet.AddAsync(profile);
            return result.Entity;
        }

        public Task UpdateAsync(MemberSalesProfile profile)
        {
            _dbSet.Update(profile);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(MemberSalesProfile profile)
        {
            _dbSet.Remove(profile);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<MemberSalesProfile>> GetAll()
        {
            return await _dbSet
                .Include(p => p.User)
                .OrderBy(p => p.CreatedAt)
                .ToListAsync();
        }
    }
} 