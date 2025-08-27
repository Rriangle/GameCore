using GameCore.Core.Entities;
using GameCore.Core.DTOs;

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
        Task<IEnumerable<ForumDto>> GetActiveForumsAsync();

        /// <summary>
        /// 取得指定遊戲的論壇版面
        /// </summary>
        /// <param name="gameId">遊戲ID</param>
        /// <returns>論壇版面</returns>
        Task<ForumDto?> GetForumByGameAsync(int gameId);

        /// <summary>
        /// 建立新貼文
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="postCreate">貼文建立</param>
        /// <returns>建立結果</returns>
        Task<PostDto> CreatePostAsync(int userId, PostCreateDto postCreate);

        /// <summary>
        /// 取得論壇的貼文列表
        /// </summary>
        /// <param name="forumId">論壇ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <param name="sortBy">排序方式</param>
        /// <returns>貼文列表</returns>
        Task<IEnumerable<PostDto>> GetPostsByForumAsync(int forumId, int page = 1, int pageSize = 20, string sortBy = "Latest");

        /// <summary>
        /// 取得貼文詳情
        /// </summary>
        /// <param name="postId">貼文ID</param>
        /// <returns>貼文詳情</returns>
        Task<PostDto?> GetPostByIdAsync(int postId);

        /// <summary>
        /// 搜尋貼文
        /// </summary>
        /// <param name="keyword">關鍵字</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>搜尋結果</returns>
        Task<IEnumerable<PostDto>> SearchPostsAsync(string keyword, int page = 1, int pageSize = 20);

        /// <summary>
        /// 建立貼文回覆
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="postId">貼文ID</param>
        /// <param name="content">回覆內容</param>
        /// <returns>建立結果</returns>
        Task<ReplyDto> CreateReplyAsync(int userId, int postId, string content);

        /// <summary>
        /// 取得貼文的回覆列表
        /// </summary>
        /// <param name="postId">貼文ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>回覆列表</returns>
        Task<IEnumerable<ReplyDto>> GetPostRepliesAsync(int postId, int page = 1, int pageSize = 20);

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

        /// <summary>
        /// 取得所有貼文
        /// </summary>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>貼文列表</returns>
        Task<IEnumerable<PostDto>> GetAllPostsAsync(int page = 1, int pageSize = 20);

        /// <summary>
        /// 更新貼文
        /// </summary>
        /// <param name="postId">貼文ID</param>
        /// <param name="userId">使用者ID</param>
        /// <param name="title">標題</param>
        /// <param name="content">內容</param>
        /// <param name="tags">標籤</param>
        /// <returns>操作結果</returns>
        Task<bool> UpdatePostAsync(int postId, int userId, string title, string content, string tags);

        /// <summary>
        /// 刪除貼文
        /// </summary>
        /// <param name="postId">貼文ID</param>
        /// <param name="userId">使用者ID</param>
        /// <returns>操作結果</returns>
        Task<bool> DeletePostAsync(int postId, int userId);



        /// <summary>
        /// 取消收藏貼文
        /// </summary>
        /// <param name="postId">貼文ID</param>
        /// <param name="userId">使用者ID</param>
        /// <returns>操作結果</returns>
        Task<bool> RemoveBookmarkAsync(int postId, int userId);

        /// <summary>
        /// 取得熱門貼文
        /// </summary>
        /// <param name="take">取得數量</param>
        /// <returns>熱門貼文列表</returns>
        Task<IEnumerable<PostDto>> GetTrendingPostsAsync(int take = 10);

        /// <summary>
        /// 取得使用者貼文
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>使用者貼文列表</returns>
        Task<IEnumerable<PostDto>> GetUserPostsAsync(int userId, int page = 1, int pageSize = 20);

        /// <summary>
        /// 取得使用者收藏
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>使用者收藏列表</returns>
        Task<IEnumerable<PostDto>> GetUserBookmarksAsync(int userId, int page = 1, int pageSize = 20);

        /// <summary>
        /// 取得論壇列表
        /// </summary>
        /// <returns>論壇列表</returns>
        Task<IEnumerable<ForumDto>> GetForumsAsync();

        /// <summary>
        /// 取得論壇詳情
        /// </summary>
        /// <param name="id">論壇ID</param>
        /// <returns>論壇詳情</returns>
        Task<ForumDto?> GetForumByIdAsync(int id);

        /// <summary>
        /// 創建論壇
        /// </summary>
        /// <param name="name">論壇名稱</param>
        /// <param name="description">論壇描述</param>
        /// <param name="category">論壇分類</param>
        /// <param name="createdBy">創建者ID</param>
        /// <returns>創建的論壇</returns>
        Task<ForumDto> CreateForumAsync(string name, string description, string category, int createdBy);

        /// <summary>
        /// 更新論壇
        /// </summary>
        /// <param name="id">論壇ID</param>
        /// <param name="name">論壇名稱</param>
        /// <param name="description">論壇描述</param>
        /// <param name="category">論壇分類</param>
        /// <param name="isActive">是否啟用</param>
        /// <returns>操作結果</returns>
        Task<bool> UpdateForumAsync(int id, string name, string description, string category, bool isActive);

        /// <summary>
        /// 刪除論壇
        /// </summary>
        /// <param name="id">論壇ID</param>
        /// <returns>操作結果</returns>
        Task<bool> DeleteForumAsync(int id);

        /// <summary>
        /// 取得論壇統計
        /// </summary>
        /// <param name="id">論壇ID</param>
        /// <returns>論壇統計</returns>
        Task<ForumStatsDto> GetForumStatsAsync(int id);

        /// <summary>
        /// 訂閱論壇
        /// </summary>
        /// <param name="forumId">論壇ID</param>
        /// <param name="userId">使用者ID</param>
        /// <returns>操作結果</returns>
        Task<bool> SubscribeToForumAsync(int forumId, int userId);

        /// <summary>
        /// 取消訂閱論壇
        /// </summary>
        /// <param name="forumId">論壇ID</param>
        /// <param name="userId">使用者ID</param>
        /// <returns>操作結果</returns>
        Task<bool> UnsubscribeFromForumAsync(int forumId, int userId);
    }
}