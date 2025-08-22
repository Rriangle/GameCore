using GameCore.Core.Entities;

namespace GameCore.Core.Interfaces
{
    /// <summary>
    /// 遊戲 Repository 介面
    /// </summary>
    public interface IGameRepository : IRepository<Game>
    {
        /// <summary>
        /// 根據類型取得遊戲
        /// </summary>
        /// <param name="genre">遊戲類型</param>
        /// <returns></returns>
        Task<IEnumerable<Game>> GetByGenreAsync(string genre);

        /// <summary>
        /// 搜尋遊戲
        /// </summary>
        /// <param name="searchTerm">搜尋關鍵字</param>
        /// <returns></returns>
        Task<IEnumerable<Game>> SearchGamesAsync(string searchTerm);

        /// <summary>
        /// 取得熱門遊戲
        /// </summary>
        /// <param name="count">數量</param>
        /// <returns></returns>
        Task<IEnumerable<Game>> GetPopularGamesAsync(int count);

        /// <summary>
        /// 取得遊戲及其指標
        /// </summary>
        /// <param name="gameId">遊戲ID</param>
        /// <returns></returns>
        Task<Game?> GetWithMetricsAsync(int gameId);
    }
}
