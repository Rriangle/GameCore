using GameCore.Core.Entities;

namespace GameCore.Core.Interfaces
{
    /// <summary>
    /// 遊戲倉庫介面
    /// </summary>
    public interface IGameRepository : IRepository<Game>
    {
        /// <summary>
        /// 取得所有遊戲
        /// </summary>
        /// <returns>遊戲列表</returns>
        Task<IEnumerable<Game>> GetAllGamesAsync();

        /// <summary>
        /// 根據分類取得遊戲
        /// </summary>
        /// <param name="category">遊戲分類</param>
        /// <returns>遊戲列表</returns>
        Task<IEnumerable<Game>> GetGamesByCategoryAsync(string category);

        /// <summary>
        /// 搜尋遊戲
        /// </summary>
        /// <param name="keyword">關鍵字</param>
        /// <returns>搜尋結果</returns>
        Task<IEnumerable<Game>> SearchGamesAsync(string keyword);

        /// <summary>
        /// 取得熱門遊戲
        /// </summary>
        /// <param name="limit">數量限制</param>
        /// <returns>熱門遊戲列表</returns>
        Task<IEnumerable<Game>> GetPopularGamesAsync(int limit = 10);

        /// <summary>
        /// 取得最新遊戲
        /// </summary>
        /// <param name="limit">數量限制</param>
        /// <returns>最新遊戲列表</returns>
        Task<IEnumerable<Game>> GetLatestGamesAsync(int limit = 10);
    }
} 