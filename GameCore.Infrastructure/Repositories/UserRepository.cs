using GameCore.Domain.Interfaces;
using GameCore.Domain.Entities;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace GameCore.Infrastructure.Repositories
{
    /// <summary>
    /// 用戶資料庫操作實作
    /// </summary>
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(GameCoreDbContext context, ILogger<UserRepository> logger) : base(context)
        {
            _logger = logger;
        }

        /// <summary>
        /// 根據ID取得用戶
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
                _logger.LogError(ex, "根據ID取得用戶失敗: {UserId}", userId);
                return null;
            }
        }

        /// <summary>
        /// 根據帳號取得用戶
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
                _logger.LogError(ex, "根據帳號取得用戶失敗: {Account}", account);
                return null;
            }
        }

        /// <summary>
        /// 根據電子郵件取得用戶
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
                _logger.LogError(ex, "根據電子郵件取得用戶失敗: {Email}", email);
                return null;
            }
        }

        /// <summary>
        /// 根據用戶名取得用戶
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
                _logger.LogError(ex, "根據用戶名取得用戶失敗: {UserName}", userName);
                return null;
            }
        }

        /// <summary>
        /// 檢查電子郵件是否存在
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
                _logger.LogError(ex, "檢查電子郵件是否存在失敗: {Email}", email);
                return false;
            }
        }

        /// <summary>
        /// 檢查用戶名是否存在
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
                _logger.LogError(ex, "檢查用戶名是否存在失敗: {UserName}", userName);
                return false;
            }
        }

        /// <summary>
        /// 取得活躍用戶
        /// </summary>
        public async Task<IEnumerable<User>> GetActiveUsersAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.Users
                    .Where(u => u.User_Status == true)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取得活躍用戶失敗");
                return new List<User>();
            }
        }

        /// <summary>
        /// 根據角色取得用戶
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
                _logger.LogError(ex, "根據角色取得用戶失敗: {Role}", role);
                return new List<User>();
            }
        }

        /// <summary>
        /// 搜尋用戶
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
                _logger.LogError(ex, "搜尋用戶失敗: {SearchTerm}", searchTerm);
                return new List<User>();
            }
        }

        /// <summary>
        /// 取得用戶統計資料
        /// </summary>
        public async Task<object> GetUserStatsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var totalUsers = await _context.Users.CountAsync(cancellationToken);
                var activeUsers = await _context.Users.CountAsync(u => u.User_Status == true, cancellationToken);
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
                _logger.LogError(ex, "取得用戶統計資料失敗");
                return new
                {
                    TotalUsers = 0,
                    ActiveUsers = 0,
                    NewUsersThisMonth = 0
                };
            }
        }

        /// <summary>
        /// 取得完整的用戶資訊（包含所有關聯資料）
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
                _logger.LogError(ex, "取得完整用戶資訊失敗: {UserId}", userId);
                return null;
            }
        }

        /// <summary>
        /// 根據暱稱取得用戶
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
                _logger.LogError(ex, "根據暱稱取得用戶失敗: {NickName}", nickName);
                return null;
            }
        }

        /// <summary>
        /// 新增用戶
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
                _logger.LogError(ex, "新增用戶失敗");
                throw;
            }
        }

        /// <summary>
        /// 更新用戶
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
                _logger.LogError(ex, "更新用戶失敗");
                throw;
            }
        }

        /// <summary>
        /// 刪除用戶
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
                _logger.LogError(ex, "刪除用戶失敗: {UserId}", id);
                throw;
            }
        }

        /// <summary>
        /// 檢查用戶是否存在
        /// </summary>
        public override async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.Users.AnyAsync(u => u.User_ID == id, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "檢查用戶是否存在失敗: {UserId}", id);
                return false;
            }
        }

        /// <summary>
        /// 計算用戶數量
        /// </summary>
        public override async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.Users.CountAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "計算用戶數量失敗");
                return 0;
            }
        }

        /// <summary>
        /// 取得分頁用戶
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
                _logger.LogError(ex, "取得分頁用戶失敗");
                return new List<User>();
            }
        }

        /// <summary>
        /// 根據條件取得分頁用戶
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
                _logger.LogError(ex, "根據條件取得分頁用戶失敗");
                return new List<User>();
            }
        }
    }
}