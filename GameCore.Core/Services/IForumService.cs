using GameCore.Core.Entities;

namespace GameCore.Core.Services
{
    /// <summary>
    /// 論壇服務介面
    /// </summary>
    public interface IForumService
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
        /// 建立新貼文
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="postCreate">貼文建立</param>
        /// <returns>建立結果</returns>
        Task<PostCreateResult> CreatePostAsync(int userId, PostCreate postCreate);

        /// <summary>
        /// 取得論壇的貼文列表
        /// </summary>
        /// <param name="forumId">論壇ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <param name="sortBy">排序方式</param>
        /// <returns>貼文列表</returns>
        Task<PostListResult> GetPostsByForumAsync(int forumId, int page, int pageSize, string sortBy = "latest");

        /// <summary>
        /// 取得貼文詳情
        /// </summary>
        /// <param name="postId">貼文ID</param>
        /// <returns>貼文詳情</returns>
        Task<Post?> GetPostAsync(int postId);

        /// <summary>
        /// 搜尋貼文
        /// </summary>
        /// <param name="keyword">關鍵字</param>
        /// <param name="forumId">論壇ID (可選)</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>搜尋結果</returns>
        Task<PostListResult> SearchPostsAsync(string keyword, int? forumId = null, int page = 1, int pageSize = 20);

        /// <summary>
        /// 建立貼文回覆
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="replyCreate">回覆建立</param>
        /// <returns>建立結果</returns>
        Task<ReplyCreateResult> CreateReplyAsync(int userId, ReplyCreate replyCreate);

        /// <summary>
        /// 取得貼文的回覆列表
        /// </summary>
        /// <param name="postId">貼文ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>回覆列表</returns>
        Task<IEnumerable<PostReply>> GetRepliesByPostAsync(int postId, int page, int pageSize);

        /// <summary>
        /// 按讚貼文
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="postId">貼文ID</param>
        /// <returns>操作結果</returns>
        Task<bool> LikePostAsync(int userId, int postId);

        /// <summary>
        /// 取消按讚貼文
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="postId">貼文ID</param>
        /// <returns>操作結果</returns>
        Task<bool> UnlikePostAsync(int userId, int postId);

        /// <summary>
        /// 收藏貼文
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="postId">貼文ID</param>
        /// <returns>操作結果</returns>
        Task<bool> BookmarkPostAsync(int userId, int postId);

        /// <summary>
        /// 取消收藏貼文
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="postId">貼文ID</param>
        /// <returns>操作結果</returns>
        Task<bool> UnbookmarkPostAsync(int userId, int postId);

        /// <summary>
        /// 增加貼文瀏覽次數
        /// </summary>
        /// <param name="postId">貼文ID</param>
        /// <returns>操作結果</returns>
        Task<bool> IncrementViewCountAsync(int postId);
    }

    /// <summary>
    /// 貼文建立模型
    /// </summary>
    public class PostCreate
    {
        public int ForumId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new();
    }

    /// <summary>
    /// 回覆建立模型
    /// </summary>
    public class ReplyCreate
    {
        public int PostId { get; set; }
        public int? ParentReplyId { get; set; }
        public string Content { get; set; } = string.Empty;
    }

    /// <summary>
    /// 貼文建立結果模型
    /// </summary>
    public class PostCreateResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public Post? Post { get; set; }
        public List<string> Errors { get; set; } = new();
    }

    /// <summary>
    /// 回覆建立結果模型
    /// </summary>
    public class ReplyCreateResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public PostReply? Reply { get; set; }
        public List<string> Errors { get; set; } = new();
    }

    /// <summary>
    /// 貼文列表結果模型
    /// </summary>
    public class PostListResult
    {
        public IEnumerable<Post> Posts { get; set; } = new List<Post>();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}