using GameCore.Core.Entities;

namespace GameCore.Core.Interfaces
{
    /// <summary>
    /// 論壇 Repository 介面
    /// </summary>
    public interface IForumRepository : IRepository<Forum>
    {
        /// <summary>
        /// 取得論壇及其主題
        /// </summary>
        /// <param name="forumId">論壇ID</param>
        /// <returns></returns>
        Task<Forum?> GetWithThreadsAsync(int forumId);

        /// <summary>
        /// 取得所有論壇及其相關遊戲資訊
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Forum>> GetAllWithGamesAsync();

        /// <summary>
        /// 根據遊戲ID取得論壇
        /// </summary>
        /// <param name="gameId">遊戲ID</param>
        /// <returns></returns>
        Task<Forum?> GetByGameIdAsync(int gameId);

        /// <summary>
        /// 搜尋論壇
        /// </summary>
        /// <param name="searchTerm">搜尋關鍵字</param>
        /// <returns></returns>
        Task<IEnumerable<Forum>> SearchAsync(string searchTerm);
    }
}
