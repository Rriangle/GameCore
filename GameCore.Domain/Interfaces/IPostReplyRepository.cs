using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// 文章回覆倉庫接口
    /// </summary>
    public interface IPostReplyRepository : IRepository<PostReply>
    {
        /// <summary>
        /// 根據文章ID獲取回覆列表
        /// </summary>
        /// <param name="postId">文章ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>回覆列表</returns>
        Task<IEnumerable<PostReply>> GetByPostIdAsync(int postId, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根據用戶ID獲取回覆列表
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>回覆列表</returns>
        Task<IEnumerable<PostReply>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// 根據狀態獲取回覆列表
        /// </summary>
        /// <param name="status">回覆狀態</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>回覆列表</returns>
        Task<IEnumerable<PostReply>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);

        /// <summary>
        /// 獲取回覆數量
        /// </summary>
        /// <param name="postId">文章ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>回覆數量</returns>
        Task<int> GetReplyCountAsync(int postId, CancellationToken cancellationToken = default);

        /// <summary>
        /// 更新回覆狀態
        /// </summary>
        /// <param name="replyId">回覆ID</param>
        /// <param name="status">新狀態</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>更新結果</returns>
        Task<bool> UpdateStatusAsync(int replyId, string status, CancellationToken cancellationToken = default);

        /// <summary>
        /// 獲取回覆樹結構
        /// </summary>
        /// <param name="postId">文章ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>回覆樹結構</returns>
        Task<IEnumerable<PostReply>> GetReplyTreeAsync(int postId, CancellationToken cancellationToken = default);
    }
} 