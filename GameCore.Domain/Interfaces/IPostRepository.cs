using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// 文章倉庫接口
    /// </summary>
    public interface IPostRepository : IRepository<Post>
    {
        /// <summary>
        /// 根據論壇ID獲取文章列表
        /// </summary>
        /// <param name="forumId">論壇ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>文章列表</returns>
        Task<IEnumerable<Post>> GetByForumIdAsync(int forumId, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根據用戶ID獲取文章列表
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>文章列表</returns>
        Task<IEnumerable<Post>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根據狀態獲取文章列表
        /// </summary>
        /// <param name="status">文章狀態</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>文章列表</returns>
        Task<IEnumerable<Post>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);

        /// <summary>
        /// 搜索文章
        /// </summary>
        /// <param name="keyword">關鍵字</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>文章列表</returns>
        Task<IEnumerable<Post>> SearchAsync(string keyword, CancellationToken cancellationToken = default);

        /// <summary>
        /// 獲取熱門文章
        /// </summary>
        /// <param name="limit">限制數量</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>文章列表</returns>
        Task<IEnumerable<Post>> GetPopularPostsAsync(int limit, CancellationToken cancellationToken = default);

        /// <summary>
        /// 更新文章狀態
        /// </summary>
        /// <param name="postId">文章ID</param>
        /// <param name="status">新狀態</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>更新結果</returns>
        Task<bool> UpdateStatusAsync(int postId, string status, CancellationToken cancellationToken = default);

        /// <summary>
        /// 增加文章瀏覽次數
        /// </summary>
        /// <param name="postId">文章ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>更新結果</returns>
        Task<bool> IncrementViewCountAsync(int postId, CancellationToken cancellationToken = default);
    }
} 