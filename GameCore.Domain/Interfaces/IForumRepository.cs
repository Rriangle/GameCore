using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// 論壇 Repository 接口
    /// </summary>
    public interface IForumRepository : IRepository<Forum>
    {
        /// <summary>
        /// 獲取活躍論壇
        /// </summary>
        /// <param name="limit">數量限制</param>
        /// <returns>論壇列表</returns>
        Task<IEnumerable<Forum>> GetActiveForumsAsync(int limit);

        /// <summary>
        /// 根據分類獲取論壇
        /// </summary>
        /// <param name="category">分類</param>
        /// <returns>論壇列表</returns>
        Task<IEnumerable<Forum>> GetByCategoryAsync(string category);
    }
} 
