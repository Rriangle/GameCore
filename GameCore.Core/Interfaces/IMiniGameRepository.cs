using GameCore.Core.Entities;
using GameCore.Core.DTOs;

namespace GameCore.Core.Interfaces
{
    /// <summary>
    /// 小遊戲 Repository 介面
    /// </summary>
    public interface IMiniGameRepository : IRepository<MiniGame>
    {
        /// <summary>
        /// 取得今日遊戲次數
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns></returns>
        Task<int> GetTodayGameCountAsync(int userId);

        /// <summary>
        /// 取得用戶遊戲歷史
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁數量</param>
        /// <returns></returns>
        Task<IEnumerable<MiniGame>> GetUserGameHistoryAsync(int userId, int page, int pageSize);

        /// <summary>
        /// 取得用戶遊戲統計
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns></returns>
        Task<MiniGameStats> GetUserGameStatsAsync(int userId);

        /// <summary>
        /// 取得排行榜
        /// </summary>
        /// <param name="gameType">遊戲類型</param>
        /// <param name="period">時間範圍</param>
        /// <param name="count">數量</param>
        /// <returns></returns>
        Task<IEnumerable<MiniGameStats>> GetLeaderboardAsync(string gameType, string period, int count);

        /// <summary>
        /// 記錄遊戲結果
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="gameType">遊戲類型</param>
        /// <param name="score">分數</param>
        /// <param name="reward">獎勵</param>
        /// <returns></returns>
        Task<int> RecordGameResultAsync(int userId, string gameType, int score, int reward);
    }
}
