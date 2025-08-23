using GameCore.Core.Entities;

namespace GameCore.Core.Interfaces
{
    /// <summary>
    /// 小遊戲服務介面
    /// </summary>
    public interface IMiniGameService
    {
        Task<MiniGame> StartGameAsync(int userId, int petId, int level);
        Task<bool> EndGameAsync(int playId, string result, int expGained, int pointsChanged);
        Task<IEnumerable<MiniGame>> GetUserGameHistoryAsync(int userId, int page = 1, int pageSize = 20);
        Task<MiniGameStats> GetUserGameStatsAsync(int userId);
        Task<IEnumerable<MiniGame>> GetTopScoresAsync(int count = 10);
    }
}