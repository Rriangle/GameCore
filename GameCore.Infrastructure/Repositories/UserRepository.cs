using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameCore.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly GameCoreDbContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(GameCoreDbContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Pets)
                .Include(u => u.Orders)
                .Include(u => u.MarketItems)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(u => u.Pets)
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Pets)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
                .Include(u => u.Pets)
                .ToListAsync();
        }

        public async Task<User> AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            return user;
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
        }

        public async Task DeleteAsync(int id)
        {
            var user = await GetByIdAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Users.AnyAsync(u => u.Id == id);
        }

        public async Task<bool> ExistsByUsernameAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<int> GetUserCountAsync()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<IEnumerable<User>> GetUsersByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Users
                .Include(u => u.Pets)
                .Where(u => u.CreatedAt >= startDate && u.CreatedAt <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUsersByLevelAsync(int minLevel, int maxLevel)
        {
            return await _context.Users
                .Include(u => u.Pets)
                .Where(u => u.Level >= minLevel && u.Level <= maxLevel)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUsersByPointsAsync(int minPoints)
        {
            return await _context.Users
                .Include(u => u.Pets)
                .Where(u => u.Points >= minPoints)
                .OrderByDescending(u => u.Points)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetActiveUsersAsync()
        {
            return await _context.Users
                .Include(u => u.Pets)
                .Where(u => u.LastLoginAt >= DateTime.UtcNow.AddDays(-30))
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> SearchUsersAsync(string searchTerm)
        {
            return await _context.Users
                .Include(u => u.Pets)
                .Where(u => u.Username.Contains(searchTerm) || 
                           u.Email.Contains(searchTerm) ||
                           u.DisplayName.Contains(searchTerm))
                .ToListAsync();
        }
    }
}