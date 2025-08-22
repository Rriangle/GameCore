using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using GameCore.Core.DTOs;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GameCore.Infrastructure.Repositories
{
    /// <summary>
    /// 小遊戲 Repository 實作
    /// </summary>
    public class MiniGameRepository : Repository<MiniGame>, IMiniGameRepository
    {
        public MiniGameRepository(GameCoreDbContext context) : base(context)
        {
        }

        /// <summary>
        /// 獲取今日遊戲次數
        /// </summary>
        public async Task<int> GetTodayGameCountAsync(int userId)
        {
            var today = DateTime.Today;
            return await _context.MiniGames
                .CountAsync(mg => mg.UserId == userId);
        }

        /// <summary>
        /// 獲取使用者遊戲歷史
        /// </summary>
        public async Task<IEnumerable<MiniGame>> GetUserGameHistoryAsync(int userId, int page, int pageSize)
        {
            return await _context.MiniGames
                .Where(mg => mg.UserId == userId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        /// <summary>
        /// 獲取使用者遊戲統計
        /// </summary>
        public async Task<MiniGameStats> GetUserGameStatsAsync(int userId)
        {
            var games = await _context.MiniGames
                .Where(mg => mg.UserId == userId)
                .ToListAsync();

            return new MiniGameStats
            {
                TotalGames = games.Count,
                Wins = 0,
                TotalExperience = 0,
                TotalPoints = 0
            };
        }

        /// <summary>
        /// 獲取排行榜
        /// </summary>
        public async Task<IEnumerable<MiniGameStats>> GetLeaderboardAsync(string gameType, string period, int limit)
        {
            // 簡化實作，返回空集合
            return new List<MiniGameStats>();
        }

        /// <summary>
        /// 記錄遊戲結果
        /// </summary>
        public async Task<int> RecordGameResultAsync(int userId, string gameType, int score, int reward)
        {
            var game = new MiniGame
            {
                UserId = userId
            };

            await AddAsync(game);
            await _context.SaveChangesAsync();

            return game.GameId;
        }
    }
}