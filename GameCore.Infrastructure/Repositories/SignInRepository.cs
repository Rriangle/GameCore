using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GameCore.Infrastructure.Repositories
{
    public class SignInRepository : Repository<UserSignInStats>, ISignInRepository
    {
        public SignInRepository(GameCoreDbContext context) : base(context)
        {
        }

        public async Task<UserSignInStats?> GetSignInByDateAsync(int userId, DateTime date)
        {
            return await _context.Set<UserSignInStats>()
                .FirstOrDefaultAsync(s => s.UserId == userId && s.SignInDate.Date == date.Date);
        }

        public async Task<IEnumerable<UserSignInStats>> GetRecentSignInsAsync(int userId, int days)
        {
            var startDate = DateTime.UtcNow.AddDays(-days);
            return await _context.Set<UserSignInStats>()
                .Where(s => s.UserId == userId && s.SignInDate >= startDate)
                .OrderByDescending(s => s.SignInDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserSignInStats>> GetSignInsByDateRangeAsync(int userId, DateTime startDate, DateTime endDate)
        {
            return await _context.Set<UserSignInStats>()
                .Where(s => s.UserId == userId && s.SignInDate >= startDate && s.SignInDate <= endDate)
                .OrderBy(s => s.SignInDate)
                .ToListAsync();
        }

        public async Task<int> GetSignInCountAsync(int userId)
        {
            return await _context.Set<UserSignInStats>()
                .CountAsync(s => s.UserId == userId);
        }

        public async Task<IEnumerable<UserSignInStats>> GetSignInHistoryAsync(int userId, int page, int pageSize)
        {
            return await _context.Set<UserSignInStats>()
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.SignInDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}