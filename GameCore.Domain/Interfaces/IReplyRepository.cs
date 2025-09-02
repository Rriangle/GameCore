using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// 回覆 Repository 介面
    /// </summary>
    public interface IReplyRepository : IRepository<Reply>
    {
        /// <summary>
        /// 根據主題ID取得回覆
        /// </summary>
        Task<IEnumerable<Reply>> GetByThreadIdAsync(long threadId);

        /// <summary>
        /// 根據作者ID取得回覆
        /// </summary>
        Task<IEnumerable<Reply>> GetByAuthorIdAsync(int authorId);

        /// <summary>
        /// 新增回覆
        /// </summary>
        Task<Reply> AddAsync(Reply reply);

        /// <summary>
        /// 更新回覆
        /// </summary>
        Task UpdateAsync(Reply reply);

        /// <summary>
        /// 刪除回覆
        /// </summary>
        Task DeleteAsync(Reply reply);
    }
} 
