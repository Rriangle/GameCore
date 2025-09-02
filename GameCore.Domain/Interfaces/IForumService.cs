using GameCore.Core.DTOs;

namespace GameCore.Domain.Interfaces
{
    /// <summary>
    /// 論壇服務介面
    /// 提供論壇相關的業務邏輯
    /// </summary>
    public interface IForumService
    {
        /// <summary>
        /// 取得論壇版面列表
        /// </summary>
        /// <param name="request">查詢條件</param>
        /// <returns>論壇版面列表</returns>
        Task<PagedResponse<ForumInfo>> GetForumsAsync(ForumQueryRequest request);

        /// <summary>
        /// 取得論壇版面詳情
        /// </summary>
        /// <param name="forumId">論壇版ID</param>
        /// <returns>論壇版面詳情</returns>
        Task<ForumInfo?> GetForumByIdAsync(int forumId);

        /// <summary>
        /// 取得主題列表
        /// </summary>
        /// <param name="request">查詢條件</param>
        /// <returns>主題列表</returns>
        Task<PagedResponse<ThreadListItem>> GetThreadsAsync(ThreadQueryRequest request);

        /// <summary>
        /// 取得主題詳情
        /// </summary>
        /// <param name="threadId">主題ID</param>
        /// <param name="currentUserId">當前用戶ID</param>
        /// <returns>主題詳情</returns>
        Task<ThreadDetail?> GetThreadByIdAsync(long threadId, int currentUserId);

        /// <summary>
        /// 建立新主題
        /// </summary>
        /// <param name="request">建立主題請求</param>
        /// <param name="currentUserId">當前用戶ID</param>
        /// <returns>新建立的主題ID</returns>
        Task<long> CreateThreadAsync(CreateThreadRequest request, int currentUserId);

        /// <summary>
        /// 建立新回覆
        /// </summary>
        /// <param name="request">建立回覆請求</param>
        /// <param name="currentUserId">當前用戶ID</param>
        /// <returns>新建立的回覆ID</returns>
        Task<long> CreatePostAsync(CreatePostRequest request, int currentUserId);

        /// <summary>
        /// 更新主題
        /// </summary>
        /// <param name="threadId">主題ID</param>
        /// <param name="title">新標題</param>
        /// <param name="currentUserId">當前用戶ID</param>
        /// <returns>是否成功</returns>
        Task<bool> UpdateThreadAsync(long threadId, string title, int currentUserId);

        /// <summary>
        /// 更新回覆
        /// </summary>
        /// <param name="postId">回覆ID</param>
        /// <param name="content">新內容</param>
        /// <param name="currentUserId">當前用戶ID</param>
        /// <returns>是否成功</returns>
        Task<bool> UpdatePostAsync(long postId, string content, int currentUserId);

        /// <summary>
        /// 刪除主題
        /// </summary>
        /// <param name="threadId">主題ID</param>
        /// <param name="currentUserId">當前用戶ID</param>
        /// <returns>是否成功</returns>
        Task<bool> DeleteThreadAsync(long threadId, int currentUserId);

        /// <summary>
        /// 刪除回覆
        /// </summary>
        /// <param name="postId">回覆ID</param>
        /// <param name="currentUserId">當前用戶ID</param>
        /// <returns>是否成功</returns>
        Task<bool> DeletePostAsync(long postId, int currentUserId);

        /// <summary>
        /// 新增反應（讚、表情等）
        /// </summary>
        /// <param name="request">反應請求</param>
        /// <param name="currentUserId">當前用戶ID</param>
        /// <returns>是否成功</returns>
        Task<bool> AddReactionAsync(ReactionRequest request, int currentUserId);

        /// <summary>
        /// 移除反應
        /// </summary>
        /// <param name="request">反應請求</param>
        /// <param name="currentUserId">當前用戶ID</param>
        /// <returns>是否成功</returns>
        Task<bool> RemoveReactionAsync(ReactionRequest request, int currentUserId);

        /// <summary>
        /// 新增收藏
        /// </summary>
        /// <param name="request">收藏請求</param>
        /// <param name="currentUserId">當前用戶ID</param>
        /// <returns>是否成功</returns>
        Task<bool> AddBookmarkAsync(BookmarkRequest request, int currentUserId);

        /// <summary>
        /// 移除收藏
        /// </summary>
        /// <param name="request">收藏請求</param>
        /// <param name="currentUserId">當前用戶ID</param>
        /// <returns>是否成功</returns>
        Task<bool> RemoveBookmarkAsync(BookmarkRequest request, int currentUserId);

        /// <summary>
        /// 取得用戶的收藏列表
        /// </summary>
        /// <param name="currentUserId">當前用戶ID</param>
        /// <param name="targetType">目標類型</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁數量</param>
        /// <returns>收藏列表</returns>
        Task<PagedResponse<object>> GetUserBookmarksAsync(int currentUserId, string targetType, int page = 1, int pageSize = 20);

        /// <summary>
        /// 搜尋主題和回覆
        /// </summary>
        /// <param name="keyword">關鍵字</param>
        /// <param name="forumId">論壇版ID（可選）</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁數量</param>
        /// <returns>搜尋結果</returns>
        Task<PagedResponse<object>> SearchAsync(string keyword, int? forumId = null, int page = 1, int pageSize = 20);

        /// <summary>
        /// 增加主題瀏覽次數
        /// </summary>
        /// <param name="threadId">主題ID</param>
        /// <returns>是否成功</returns>
        Task<bool> IncrementViewCountAsync(long threadId);

        /// <summary>
        /// 取得熱門主題
        /// </summary>
        /// <param name="forumId">論壇版ID（可選）</param>
        /// <param name="limit">數量限制</param>
        /// <returns>熱門主題列表</returns>
        Task<List<ThreadListItem>> GetPopularThreadsAsync(int? forumId = null, int limit = 10);

        /// <summary>
        /// 取得最新主題
        /// </summary>
        /// <param name="forumId">論壇版ID（可選）</param>
        /// <param name="limit">數量限制</param>
        /// <returns>最新主題列表</returns>
        Task<List<ThreadListItem>> GetLatestThreadsAsync(int? forumId = null, int limit = 10);
    }
} 
