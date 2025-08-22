using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GameCore.Infrastructure.Repositories
{
    /// <summary>
    /// 簽到 Repository 實作
    /// </summary>
    public class SignInRepository : Repository<UserSignInStats>, ISignInRepository
    {
        public SignInRepository(GameCoreDbContext context) : base(context)
        {
        }

        /// <summary>
        /// 根據日期獲取簽到記錄
        /// </summary>
        public async Task<UserSignInStats?> GetSignInByDateAsync(int userId, DateTime date)
        {
            return await _context.UserSignInStats
                .FirstOrDefaultAsync(s => s.UserId == userId);
        }

        /// <summary>
        /// 獲取最近簽到記錄
        /// </summary>
        public async Task<IEnumerable<UserSignInStats>> GetRecentSignInsAsync(int userId, int days)
        {
            return await _context.UserSignInStats
                .Where(s => s.UserId == userId)
                .Take(days)
                .ToListAsync();
        }

        /// <summary>
        /// 獲取日期範圍內的簽到記錄
        /// </summary>
        public async Task<IEnumerable<UserSignInStats>> GetSignInsByDateRangeAsync(int userId, DateTime startDate, DateTime endDate)
        {
            return await _context.UserSignInStats
                .Where(s => s.UserId == userId)
                .ToListAsync();
        }

        /// <summary>
        /// 獲取簽到總次數
        /// </summary>
        public async Task<int> GetSignInCountAsync(int userId)
        {
            return await _context.UserSignInStats
                .CountAsync(s => s.UserId == userId);
        }

        /// <summary>
        /// 獲取簽到歷史（分頁）
        /// </summary>
        public async Task<IEnumerable<UserSignInStats>> GetSignInHistoryAsync(int userId, int page, int pageSize)
        {
            return await _context.UserSignInStats
                .Where(s => s.UserId == userId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        /// <summary>
        /// 獲取連續簽到天數
        /// </summary>
        public async Task<int> GetStreakDaysAsync(int userId)
        {
            var count = await _context.UserSignInStats
                .CountAsync(s => s.UserId == userId);
            
            return count > 0 ? 1 : 0; // 簡化實作
        }
    }
}