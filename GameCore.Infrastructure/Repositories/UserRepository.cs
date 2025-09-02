using GameCore.Domain.Interfaces;
using GameCore.Domain.Entities;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace GameCore.Infrastructure.Repositories
{
    /// <summary>
    /// ?�戶資�?庫�?作實�?
    /// </summary>
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(GameCoreDbContext context, ILogger<UserRepository> logger) : base(context)
        {
            _logger = logger;
        }

        /// <summary>
        /// ?��?ID?��??�戶
        /// </summary>
        public override async Task<User?> GetByIdAsync(int userId, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.Users
                    .FirstOrDefaultAsync(u => u.User_ID == userId, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "?��?ID?��??�戶失�?: {UserId}", userId);
                return null;
            }
        }

        /// <summary>
        /// ?��?帳�??��??�戶
        /// </summary>
        public async Task<User?> GetByAccountAsync(string account)
        {
            try
            {
                return await _context.Users
                    .FirstOrDefaultAsync(u => u.User_Account == account);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "?��?帳�??��??�戶失�?: {Account}", account);
                return null;
            }
        }

        /// <summary>
        /// ?��??��??�件?��??�戶
        /// </summary>
        public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.Users
                    .Include(u => u.UserIntroduce)
                    .FirstOrDefaultAsync(u => u.UserIntroduce != null && u.UserIntroduce.Email == email, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "?��??��??�件?��??�戶失�?: {Email}", email);
                return null;
            }
        }

        /// <summary>
        /// ?��??�戶?��?得用??
        /// </summary>
        public async Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.Users
                    .Include(u => u.UserIntroduce)
                    .FirstOrDefaultAsync(u => u.UserIntroduce != null && u.UserIntroduce.User_NickName == userName, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "?��??�戶?��?得用?�失?? {UserName}", userName);
                return null;
            }
        }

        /// <summary>
        /// 檢查?��??�件?�否存在
        /// </summary>
        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.Users
                    .Include(u => u.UserIntroduce)
                    .AnyAsync(u => u.UserIntroduce != null && u.UserIntroduce.Email == email, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "檢查?��??�件?�否存在失�?: {Email}", email);
                return false;
            }
        }

        /// <summary>
        /// 檢查?�戶?�是?��???
        /// </summary>
        public async Task<bool> ExistsByUserNameAsync(string userName, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.Users
                    .Include(u => u.UserIntroduce)
                    .AnyAsync(u => u.UserIntroduce != null && u.UserIntroduce.User_NickName == userName, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "檢查?�戶?�是?��??�失?? {UserName}", userName);
                return false;
            }
        }

        /// <summary>
        /// ?��?活�??�戶
        /// </summary>
        public async Task<IEnumerable<User>> GetActiveUsersAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.Users
                    .Where(u => u.User_Status == "Active")
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "?��?活�??�戶失�?");
                return new List<User>();
            }
        }

        /// <summary>
        /// ?��?角色?��??�戶
        /// </summary>
        public async Task<IEnumerable<User>> GetUsersByRoleAsync(string role, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.Users
                    .Include(u => u.UserRights)
                    .Where(u => u.UserRights != null && u.UserRights.User_Role == role)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "?��?角色?��??�戶失�?: {Role}", role);
                return new List<User>();
            }
        }

        /// <summary>
        /// ?��??�戶
        /// </summary>
        public async Task<IEnumerable<User>> SearchUsersAsync(string searchTerm, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            try
            {
                var query = _context.Users
                    .Include(u => u.UserIntroduce)
                    .Where(u => u.User_Account.Contains(searchTerm) || 
                               (u.UserIntroduce != null && u.UserIntroduce.User_NickName.Contains(searchTerm)));

                return await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "?��??�戶失�?: {SearchTerm}", searchTerm);
                return new List<User>();
            }
        }

        /// <summary>
        /// ?��??�戶統�?資�?
        /// </summary>
        public async Task<object> GetUserStatsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var totalUsers = await _context.Users.CountAsync(cancellationToken);
                var activeUsers = await _context.Users.CountAsync(u => u.User_Status == "Active", cancellationToken);
                var newUsersThisMonth = await _context.Users
                    .CountAsync(u => u.User_CreatedAt >= DateTime.Now.AddDays(-30), cancellationToken);

                return new
                {
                    TotalUsers = totalUsers,
                    ActiveUsers = activeUsers,
                    NewUsersThisMonth = newUsersThisMonth
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "?��??�戶統�?資�?失�?");
                return new
                {
                    TotalUsers = 0,
                    ActiveUsers = 0,
                    NewUsersThisMonth = 0
                };
            }
        }

        /// <summary>
        /// ?��?完整?�用?��?訊�??�含?�?��??��??��?
        /// </summary>
        public async Task<User?> GetFullUserInfoAsync(int userId)
        {
            try
            {
                return await _context.Users
                    .Include(u => u.UserIntroduce)
                    .Include(u => u.UserRights)
                    .Include(u => u.UserWallet)
                    .Include(u => u.UserSalesInformation)
                    .Include(u => u.Pets)
                    .FirstOrDefaultAsync(u => u.User_ID == userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "?��?完整?�戶資�?失�?: {UserId}", userId);
                return null;
            }
        }

        /// <summary>
        /// ?��??�稱?��??�戶
        /// </summary>
        public async Task<User?> GetByNickNameAsync(string nickName)
        {
            try
            {
                return await _context.Users
                    .Include(u => u.UserIntroduce)
                    .FirstOrDefaultAsync(u => u.UserIntroduce != null && u.UserIntroduce.User_NickName == nickName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "?��??�稱?��??�戶失�?: {NickName}", nickName);
                return null;
            }
        }

        /// <summary>
        /// ?��??�戶
        /// </summary>
        public override async Task<User> AddAsync(User entity, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _context.Users.AddAsync(entity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return result.Entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "?��??�戶失�?");
                throw;
            }
        }

        /// <summary>
        /// ?�新?�戶
        /// </summary>
        public override Task UpdateAsync(User entity, CancellationToken cancellationToken = default)
        {
            try
            {
                _context.Users.Update(entity);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "?�新?�戶失�?");
                throw;
            }
        }

        /// <summary>
        /// ?�除?�戶
        /// </summary>
        public override async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var entity = await GetByIdAsync(id, cancellationToken);
                if (entity != null)
                {
                    _context.Users.Remove(entity);
                    await _context.SaveChangesAsync(cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "?�除?�戶失�?: {UserId}", id);
                throw;
            }
        }

        /// <summary>
        /// 檢查?�戶?�否存在
        /// </summary>
        public override async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.Users.AnyAsync(u => u.User_ID == id, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "檢查?�戶?�否存在失�?: {UserId}", id);
                return false;
            }
        }

        /// <summary>
        /// 計�??�戶?��?
        /// </summary>
        public override async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.Users.CountAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "計�??�戶?��?失�?");
                return 0;
            }
        }

        /// <summary>
        /// ?��??��??�戶
        /// </summary>
        public override async Task<IEnumerable<User>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.Users
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "?��??��??�戶失�?");
                return new List<User>();
            }
        }

        /// <summary>
        /// ?��?條件?��??��??�戶
        /// </summary>
        public override async Task<IEnumerable<User>> GetPagedAsync(Expression<Func<User, bool>> predicate, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.Users
                    .Where(predicate)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "?��?條件?��??��??�戶失�?");
                return new List<User>();
            }
        }
    }
}
