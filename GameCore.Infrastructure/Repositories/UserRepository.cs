using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GameCore.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(GameCoreDbContext context) : base(context)
        {
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(u => u.Pets)
                .FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Pets)
                .FirstOrDefaultAsync(u => u.UserAccount == email);
        }

        public async Task<User?> GetByAccountAsync(string account)
        {
            return await _context.Users
                .Include(u => u.Pets)
                .FirstOrDefaultAsync(u => u.UserAccount == account);
        }

        public async Task<IEnumerable<User>> GetActiveUsersAsync()
        {
            return await _context.Users
                .Include(u => u.Pets)
                .ToListAsync();
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.UserName == username);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.UserAccount == email);
        }

        public async Task<User?> GetUserWithAllDataAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.Pets)
                .Include(u => u.UserIntroduce)
                .Include(u => u.UserRights)
                .Include(u => u.UserWallet)
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<UserWallet?> GetWalletAsync(int userId)
        {
            return await _context.Set<UserWallet>()
                .FirstOrDefaultAsync(w => w.UserId == userId);
        }

        public async Task UpdateWalletAsync(UserWallet wallet)
        {
            _context.Set<UserWallet>().Update(wallet);
        }

        public async Task AddPointsAsync(int userId, int points, string reason)
        {
            var wallet = await GetWalletAsync(userId);
            if (wallet != null)
            {
                wallet.Points += points;
                _context.Set<UserWallet>().Update(wallet);
            }
        }

        public async Task<bool> DeductPointsAsync(int userId, int points, string reason)
        {
            var wallet = await GetWalletAsync(userId);
            if (wallet != null && wallet.Points >= points)
            {
                wallet.Points -= points;
                _context.Set<UserWallet>().Update(wallet);
                return true;
            }
            return false;
        }

        public async Task<UserRights?> GetUserRightsAsync(int userId)
        {
            return await _context.Set<UserRights>()
                .FirstOrDefaultAsync(r => r.UserId == userId);
        }

        public async Task UpdateUserRightsAsync(UserRights rights)
        {
            _context.Set<UserRights>().Update(rights);
        }

        public async Task<bool> AccountExistsAsync(string account)
        {
            return await _context.Users.AnyAsync(u => u.UserAccount == account);
        }

        public async Task<bool> NicknameExistsAsync(string nickname)
        {
            return await _context.Set<UserIntroduce>()
                .AnyAsync(u => u.UserNickName == nickname);
        }

        public async Task<IEnumerable<UserWallet>> GetPointsLeaderboardAsync(int count)
        {
            return await _context.Set<UserWallet>()
                .Include(w => w.User)
                .OrderByDescending(w => w.Points)
                .Take(count)
                .ToListAsync();
        }

        public async Task<PagedResult<User>> SearchUsersAsync(string searchTerm, int page, int pageSize)
        {
            var query = _context.Users
                .Include(u => u.UserIntroduce)
                .Where(u => u.UserName.Contains(searchTerm) || 
                           u.UserAccount.Contains(searchTerm) ||
                           u.UserIntroduce.UserNickName.Contains(searchTerm));

            var totalCount = await query.CountAsync();
            var users = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<User>
            {
                Items = users,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}