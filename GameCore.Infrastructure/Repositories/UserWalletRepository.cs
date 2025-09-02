using GameCore.Domain.Entities;
using GameCore.Domain.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameCore.Infrastructure.Repositories
{
    public class UserWalletRepository : Repository<UserWallet>, IUserWalletRepository
    {
        private readonly ILogger<UserWalletRepository> _logger;

        public UserWalletRepository(GameCoreDbContext context, ILogger<UserWalletRepository> logger) : base(context)
        {
            _logger = logger;
        }

        public async Task<UserWallet?> GetByUserIdAsync(int userId)
        {
            return await _dbSet
                .Include(w => w.User)
                .FirstOrDefaultAsync(w => w.UserId == userId);
        }

        public async Task<UserWallet> AddAsync(UserWallet wallet)
        {
            var result = await _dbSet.AddAsync(wallet);
            return result.Entity;
        }

        public Task UpdateAsync(UserWallet wallet)
        {
            _dbSet.Update(wallet);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(UserWallet wallet)
        {
            _dbSet.Remove(wallet);
            return Task.CompletedTask;
        }
    }
} 