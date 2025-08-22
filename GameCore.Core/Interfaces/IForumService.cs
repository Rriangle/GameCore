using GameCore.Core.Entities;

namespace GameCore.Core.Interfaces
{
    /// <summary>
    /// 論壇服務介面
    /// </summary>
    public interface IForumService
    {
        /// <summary>
        /// 取得所有論壇
        /// </summary>
        Task<List<Forum>> GetAllForumsAsync();

        /// <summary>
        /// 根據遊戲 ID 取得論壇
        /// </summary>
        Task<Forum?> GetForumByGameIdAsync(int gameId);

        /// <summary>
        /// 建立新主題
        /// </summary>
        Task<GameCore.Core.Entities.Thread> CreateThreadAsync(int forumId, int authorId, string title);

        /// <summary>
        /// 回覆主題
        /// </summary>
        Task<ThreadPost> ReplyToThreadAsync(long threadId, int authorId, string content, long? parentPostId = null);
    }
}