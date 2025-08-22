using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GameCore.Infrastructure.Repositories
{
    /// <summary>
    /// 使用者 Repository 實作
    /// 提供完整的使用者資料存取功能
    /// </summary>
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(GameCoreDbContext context) : base(context)
        {
        }

        /// <summary>
        /// 根據帳號取得使用者
        /// </summary>
        public async Task<User?> GetByAccountAsync(string account)
        {
            return await _context.Users
                .Include(u => u.UserIntroduce)
                .Include(u => u.UserRights)
                .Include(u => u.UserWallet)
                .FirstOrDefaultAsync(u => u.UserAccount == account);
        }

        /// <summary>
        /// 根據 Email 取得使用者
        /// </summary>
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.UserIntroduce)
                .Include(u => u.UserRights)
                .Include(u => u.UserWallet)
                .FirstOrDefaultAsync(u => u.UserIntroduce!.Email == email);
        }

        /// <summary>
        /// 取得使用者完整資料（包含所有關聯表）
        /// </summary>
        public async Task<User?> GetUserWithAllDataAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.UserIntroduce)
                .Include(u => u.UserRights)
                .Include(u => u.UserWallet)
                .Include(u => u.MemberSalesProfile)
                .Include(u => u.UserSalesInformation)
                .Include(u => u.Pet)
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        /// <summary>
        /// 取得使用者錢包資料
        /// </summary>
        public async Task<UserWallet?> GetWalletAsync(int userId)
        {
            return await _context.UserWallets
                .FirstOrDefaultAsync(w => w.UserId == userId);
        }

        /// <summary>
        /// 更新使用者錢包
        /// </summary>
        public async Task UpdateWalletAsync(UserWallet wallet)
        {
            _context.UserWallets.Update(wallet);
        }

        /// <summary>
        /// 增加使用者點數
        /// </summary>
        public async Task AddPointsAsync(int userId, int points, string reason)
        {
            var wallet = await GetWalletAsync(userId);
            if (wallet != null)
            {
                wallet.UserPoint += points;
                _context.UserWallets.Update(wallet);
            }
        }

        /// <summary>
        /// 扣除使用者點數
        /// </summary>
        public async Task<bool> DeductPointsAsync(int userId, int points, string reason)
        {
            var wallet = await GetWalletAsync(userId);
            if (wallet != null && wallet.UserPoint >= points)
            {
                wallet.UserPoint -= points;
                _context.UserWallets.Update(wallet);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 取得使用者權限
        /// </summary>
        public async Task<UserRights?> GetUserRightsAsync(int userId)
        {
            return await _context.UserRights
                .FirstOrDefaultAsync(r => r.UserId == userId);
        }

        /// <summary>
        /// 更新使用者權限
        /// </summary>
        public async Task UpdateUserRightsAsync(UserRights userRights)
        {
            _context.UserRights.Update(userRights);
        }

        /// <summary>
        /// 檢查帳號是否存在
        /// </summary>
        public async Task<bool> AccountExistsAsync(string account)
        {
            return await _context.Users
                .AnyAsync(u => u.UserAccount == account);
        }

        /// <summary>
        /// 檢查 Email 是否存在
        /// </summary>
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.UserIntroduces
                .AnyAsync(ui => ui.Email == email);
        }

        /// <summary>
        /// 檢查暱稱是否存在
        /// </summary>
        public async Task<bool> NicknameExistsAsync(string nickname)
        {
            return await _context.UserIntroduces
                .AnyAsync(ui => ui.UserNickName == nickname);
        }

        /// <summary>
        /// 取得點數排行榜
        /// </summary>
        public async Task<IEnumerable<UserWallet>> GetPointsLeaderboardAsync(int top = 10)
        {
            return await _context.UserWallets
                .Include(w => w.User)
                .OrderByDescending(w => w.UserPoint)
                .Take(top)
                .ToListAsync();
        }

        /// <summary>
        /// 搜尋使用者
        /// </summary>
        public async Task<PagedResult<User>> SearchUsersAsync(string searchTerm, int page = 1, int pageSize = 20)
        {
            var query = _context.Users
                .Include(u => u.UserIntroduce)
                .Where(u => u.UserName!.Contains(searchTerm) ||
                           u.UserAccount!.Contains(searchTerm) ||
                           u.UserIntroduce!.UserNickName!.Contains(searchTerm));

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<User>
            {
                Items = items,
                TotalCount = totalCount,
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            };
        }
    }
}

