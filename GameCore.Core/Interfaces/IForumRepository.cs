using GameCore.Core.Entities;

namespace GameCore.Core.Interfaces
{
    /// <summary>
    /// 論壇倉庫介面
    /// </summary>
    public interface IForumRepository : IRepository<Forum>
    {
        /// <summary>
        /// 取得所有活躍的論壇版面
        /// </summary>
        /// <returns>論壇版面列表</returns>
        Task<IEnumerable<Forum>> GetActiveForumsAsync();

        /// <summary>
        /// 取得指定遊戲的論壇版面
        /// </summary>
        /// <param name="gameId">遊戲ID</param>
        /// <returns>論壇版面</returns>
        Task<Forum?> GetForumByGameAsync(int gameId);

        /// <summary>
        /// 更新論壇統計資訊
        /// </summary>
        /// <param name="forumId">論壇ID</param>
        /// <returns>操作結果</returns>
        Task<bool> UpdateForumStatisticsAsync(int forumId);
    }

    /// <summary>
    /// 貼文倉庫介面
    /// </summary>
    public interface IPostRepository : IRepository<Post>
    {
        /// <summary>
        /// 取得論壇的貼文列表
        /// </summary>
        /// <param name="forumId">論壇ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <param name="sortBy">排序方式</param>
        /// <returns>貼文列表</returns>
        Task<IEnumerable<Post>> GetPostsByForumAsync(int forumId, int page, int pageSize, string sortBy = "latest");

        /// <summary>
        /// 搜尋貼文
        /// </summary>
        /// <param name="keyword">關鍵字</param>
        /// <param name="forumId">論壇ID (可選)</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>搜尋結果</returns>
        Task<IEnumerable<Post>> SearchPostsAsync(string keyword, int? forumId = null, int page = 1, int pageSize = 20);

        /// <summary>
        /// 增加貼文瀏覽次數
        /// </summary>
        /// <param name="postId">貼文ID</param>
        /// <returns>操作結果</returns>
        Task<bool> IncrementViewCountAsync(int postId);

        /// <summary>
        /// 取得使用者的貼文
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>貼文列表</returns>
        Task<IEnumerable<Post>> GetPostsByUserAsync(int userId, int page, int pageSize);
    }

    /// <summary>
    /// 貼文回覆倉庫介面
    /// </summary>
    public interface IThreadPostRepository : IRepository<ThreadPost>
    {
        /// <summary>
        /// 取得討論串的回覆列表
        /// </summary>
        /// <param name="threadId">討論串ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>回覆列表</returns>
        Task<IEnumerable<ThreadPost>> GetRepliesByThreadAsync(int threadId, int page, int pageSize);

        /// <summary>
        /// 取得回覆的子回覆
        /// </summary>
        /// <param name="replyId">回覆ID</param>
        /// <returns>子回覆列表</returns>
        Task<IEnumerable<ThreadPost>> GetChildRepliesAsync(int replyId);
    }
}