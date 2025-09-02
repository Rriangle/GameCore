using GameCore.Domain.Interfaces;
using GameCore.Domain.Entities;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace GameCore.Infrastructure.Repositories
{
    /// <summary>
    /// ?®Êà∂Ë≥áÊ?Â∫´Ê?‰ΩúÂØ¶‰Ω?
    /// </summary>
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(GameCoreDbContext context, ILogger<UserRepository> logger) : base(context)
        {
            _logger = logger;
        }

        /// <summary>
        /// ?πÊ?ID?ñÂ??®Êà∂
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
                _logger.LogError(ex, "?πÊ?ID?ñÂ??®Êà∂Â§±Ê?: {UserId}", userId);
                return null;
            }
        }

        /// <summary>
        /// ?πÊ?Â∏≥Ë??ñÂ??®Êà∂
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
                _logger.LogError(ex, "?πÊ?Â∏≥Ë??ñÂ??®Êà∂Â§±Ê?: {Account}", account);
                return null;
            }
        }

        /// <summary>
        /// ?πÊ??ªÂ??µ‰ª∂?ñÂ??®Êà∂
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
                _logger.LogError(ex, "?πÊ??ªÂ??µ‰ª∂?ñÂ??®Êà∂Â§±Ê?: {Email}", email);
                return null;
            }
        }

        /// <summary>
        /// ?πÊ??®Êà∂?çÂ?ÂæóÁî®??
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
                _logger.LogError(ex, "?πÊ??®Êà∂?çÂ?ÂæóÁî®?∂Â§±?? {UserName}", userName);
                return null;
            }
        }

        /// <summary>
        /// Ê™¢Êü•?ªÂ??µ‰ª∂?ØÂê¶Â≠òÂú®
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
                _logger.LogError(ex, "Ê™¢Êü•?ªÂ??µ‰ª∂?ØÂê¶Â≠òÂú®Â§±Ê?: {Email}", email);
                return false;
            }
        }

        /// <summary>
        /// Ê™¢Êü•?®Êà∂?çÊòØ?¶Â???
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
                _logger.LogError(ex, "Ê™¢Êü•?®Êà∂?çÊòØ?¶Â??®Â§±?? {UserName}", userName);
                return false;
            }
        }

        /// <summary>
        /// ?ñÂ?Ê¥ªË??®Êà∂
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
                _logger.LogError(ex, "?ñÂ?Ê¥ªË??®Êà∂Â§±Ê?");
                return new List<User>();
            }
        }

        /// <summary>
        /// ?πÊ?ËßíËâ≤?ñÂ??®Êà∂
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
                _logger.LogError(ex, "?πÊ?ËßíËâ≤?ñÂ??®Êà∂Â§±Ê?: {Role}", role);
                return new List<User>();
            }
        }

        /// <summary>
        /// ?úÂ??®Êà∂
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
                _logger.LogError(ex, "?úÂ??®Êà∂Â§±Ê?: {SearchTerm}", searchTerm);
                return new List<User>();
            }
        }

        /// <summary>
        /// ?ñÂ??®Êà∂Áµ±Ë?Ë≥áÊ?
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
                _logger.LogError(ex, "?ñÂ??®Êà∂Áµ±Ë?Ë≥áÊ?Â§±Ê?");
                return new
                {
                    TotalUsers = 0,
                    ActiveUsers = 0,
                    NewUsersThisMonth = 0
                };
            }
        }

        /// <summary>
        /// ?ñÂ?ÂÆåÊï¥?ÑÁî®?∂Ë?Ë®äÔ??ÖÂê´?Ä?âÈ??ØË??ôÔ?
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
                _logger.LogError(ex, "?ñÂ?ÂÆåÊï¥?®Êà∂Ë≥áË?Â§±Ê?: {UserId}", userId);
                return null;
            }
        }

        /// <summary>
        /// ?πÊ??±Á®±?ñÂ??®Êà∂
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
                _logger.LogError(ex, "?πÊ??±Á®±?ñÂ??®Êà∂Â§±Ê?: {NickName}", nickName);
                return null;
            }
        }

        /// <summary>
        /// ?∞Â??®Êà∂
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
                _logger.LogError(ex, "?∞Â??®Êà∂Â§±Ê?");
                throw;
            }
        }

        /// <summary>
        /// ?¥Êñ∞?®Êà∂
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
                _logger.LogError(ex, "?¥Êñ∞?®Êà∂Â§±Ê?");
                throw;
            }
        }

        /// <summary>
        /// ?™Èô§?®Êà∂
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
                _logger.LogError(ex, "?™Èô§?®Êà∂Â§±Ê?: {UserId}", id);
                throw;
            }
        }

        /// <summary>
        /// Ê™¢Êü•?®Êà∂?ØÂê¶Â≠òÂú®
        /// </summary>
        public override async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.Users.AnyAsync(u => u.User_ID == id, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ê™¢Êü•?®Êà∂?ØÂê¶Â≠òÂú®Â§±Ê?: {UserId}", id);
                return false;
            }
        }

        /// <summary>
        /// Ë®àÁ??®Êà∂?∏È?
        /// </summary>
        public override async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.Users.CountAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ë®àÁ??®Êà∂?∏È?Â§±Ê?");
                return 0;
            }
        }

        /// <summary>
        /// ?ñÂ??ÜÈ??®Êà∂
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
                _logger.LogError(ex, "?ñÂ??ÜÈ??®Êà∂Â§±Ê?");
                return new List<User>();
            }
        }

        /// <summary>
        /// ?πÊ?Ê¢ù‰ª∂?ñÂ??ÜÈ??®Êà∂
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
                _logger.LogError(ex, "?πÊ?Ê¢ù‰ª∂?ñÂ??ÜÈ??®Êà∂Â§±Ê?");
                return new List<User>();
            }
        }
    }
}
