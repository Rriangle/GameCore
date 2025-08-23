using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GameCore.Infrastructure.Repositories
{
    /// <summary>
    /// 簽到倉庫實作
    /// </summary>
    public class SignInRepository : Repository<SignInRecord>, ISignInRepository
    {
        public SignInRepository(GameCoreDbContext context) : base(context)
        {
        }

        /// <summary>
        /// 檢查使用者是否已在指定日期簽到
        /// </summary>
        public async Task<bool> HasSignedInTodayAsync(int userId, DateTime date)
        {
            var startOfDay = date.Date;
            var endOfDay = startOfDay.AddDays(1);

            return await _context.SignInRecords
                .AnyAsync(s => s.UserId == userId && 
                               s.SignInDate >= startOfDay && 
                               s.SignInDate < endOfDay);
        }

        /// <summary>
        /// 取得使用者的簽到統計
        /// </summary>
        public async Task<SignInStatistics?> GetStatisticsAsync(int userId, int year, int month)
        {
            return await _context.SignInStatistics
                .FirstOrDefaultAsync(s => s.UserId == userId && 
                                         s.Year == year && 
                                         s.Month == month);
        }

        /// <summary>
        /// 取得使用者的連續簽到天數
        /// </summary>
        public async Task<int> GetConsecutiveDaysAsync(int userId)
        {
            var today = DateTime.UtcNow.Date;
            var consecutiveDays = 0;
            var currentDate = today;

            while (true)
            {
                var hasSignedIn = await HasSignedInTodayAsync(userId, currentDate);
                if (!hasSignedIn)
                    break;

                consecutiveDays++;
                currentDate = currentDate.AddDays(-1);
            }

            return consecutiveDays;
        }

        /// <summary>
        /// 取得使用者的月度簽到記錄
        /// </summary>
        public async Task<IEnumerable<SignInRecord>> GetMonthlyRecordsAsync(int userId, int year, int month)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1);

            return await _context.SignInRecords
                .Where(s => s.UserId == userId && 
                           s.SignInDate >= startDate && 
                           s.SignInDate < endDate)
                .OrderBy(s => s.SignInDate)
                .ToListAsync();
        }

        /// <summary>
        /// 建立或更新簽到統計
        /// </summary>
        public async Task<bool> UpsertStatisticsAsync(SignInStatistics statistics)
        {
            try
            {
                var existing = await _context.SignInStatistics
                    .FirstOrDefaultAsync(s => s.StatisticsId == statistics.StatisticsId);

                if (existing != null)
                {
                    // 更新現有統計
                    existing.TotalSignInDays = statistics.TotalSignInDays;
                    existing.ConsecutiveDays = statistics.ConsecutiveDays;
                    existing.MaxConsecutiveDays = statistics.MaxConsecutiveDays;
                    existing.TotalPointsEarned = statistics.TotalPointsEarned;
                    existing.TotalExperienceEarned = statistics.TotalExperienceEarned;
                    existing.IsMonthlyPerfect = statistics.IsMonthlyPerfect;
                    existing.UpdatedAt = DateTime.UtcNow;

                    _context.SignInStatistics.Update(existing);
                }
                else
                {
                    // 建立新統計
                    statistics.CreatedAt = DateTime.UtcNow;
                    statistics.UpdatedAt = DateTime.UtcNow;
                    _context.SignInStatistics.Add(statistics);
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}