using System.ComponentModel.DataAnnotations;

namespace GameCore.Core.DTOs
{
    /// <summary>
    /// 論壇版面資訊 DTO
    /// </summary>
    public class ForumInfo
    {
        /// <summary>
        /// 論壇版ID
        /// </summary>
        public int ForumId { get; set; }

        /// <summary>
        /// 關聯的遊戲ID
        /// </summary>
        public int GameId { get; set; }

        /// <summary>
        /// 遊戲名稱
        /// </summary>
        public string GameName { get; set; } = string.Empty;

        /// <summary>
        /// 版面名稱
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 版面說明
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 主題數量
        /// </summary>
        public int ThreadCount { get; set; }

        /// <summary>
        /// 今日新主題數
        /// </summary>
        public int TodayThreadCount { get; set; }

        /// <summary>
        /// 活躍用戶數
        /// </summary>
        public int ActiveUserCount { get; set; }
    }

    /// <summary>
    /// 主題列表項目 DTO
    /// </summary>
    public class ThreadListItem
    {
        /// <summary>
        /// 主題ID
        /// </summary>
        public long ThreadId { get; set; }

        /// <summary>
        /// 主題標題
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 作者用戶ID
        /// </summary>
        public int AuthorUserId { get; set; }

        /// <summary>
        /// 作者暱稱
        /// </summary>
        public string AuthorNickname { get; set; } = string.Empty;

        /// <summary>
        /// 回覆數量
        /// </summary>
        public int ReplyCount { get; set; }

        /// <summary>
        /// 瀏覽次數
        /// </summary>
        public int ViewCount { get; set; }

        /// <summary>
        /// 讚數
        /// </summary>
        public int LikeCount { get; set; }

        /// <summary>
        /// 最後回覆時間
        /// </summary>
        public DateTime? LastReplyTime { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 是否為精華主題
        /// </summary>
        public bool IsPinned { get; set; }

        /// <summary>
        /// 主題狀態
        /// </summary>
        public string Status { get; set; } = string.Empty;
    }

    /// <summary>
    /// 主題詳情 DTO
    /// </summary>
    public class ThreadDetail
    {
        /// <summary>
        /// 主題ID
        /// </summary>
        public long ThreadId { get; set; }

        /// <summary>
        /// 主題標題
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 作者用戶ID
        /// </summary>
        public int AuthorUserId { get; set; }

        /// <summary>
        /// 作者暱稱
        /// </summary>
        public string AuthorNickname { get; set; } = string.Empty;

        /// <summary>
        /// 作者頭像
        /// </summary>
        public string? AuthorAvatar { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// 瀏覽次數
        /// </summary>
        public int ViewCount { get; set; }

        /// <summary>
        /// 讚數
        /// </summary>
        public int LikeCount { get; set; }

        /// <summary>
        /// 收藏數
        /// </summary>
        public int BookmarkCount { get; set; }

        /// <summary>
        /// 主題狀態
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// 是否為精華主題
        /// </summary>
        public bool IsPinned { get; set; }

        /// <summary>
        /// 當前用戶是否已讚
        /// </summary>
        public bool IsLikedByCurrentUser { get; set; }

        /// <summary>
        /// 當前用戶是否已收藏
        /// </summary>
        public bool IsBookmarkedByCurrentUser { get; set; }

        /// <summary>
        /// 回覆列表
        /// </summary>
        public List<ThreadPostDetail> Posts { get; set; } = new List<ThreadPostDetail>();
    }

    /// <summary>
    /// 主題回覆詳情 DTO
    /// </summary>
    public class ThreadPostDetail
    {
        /// <summary>
        /// 回覆ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 作者用戶ID
        /// </summary>
        public int AuthorUserId { get; set; }

        /// <summary>
        /// 作者暱稱
        /// </summary>
        public string AuthorNickname { get; set; } = string.Empty;

        /// <summary>
        /// 作者頭像
        /// </summary>
        public string? AuthorAvatar { get; set; }

        /// <summary>
        /// 回覆內容（HTML格式）
        /// </summary>
        public string ContentHtml { get; set; } = string.Empty;

        /// <summary>
        /// 父回覆ID
        /// </summary>
        public long? ParentPostId { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// 讚數
        /// </summary>
        public int LikeCount { get; set; }

        /// <summary>
        /// 回覆狀態
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// 當前用戶是否已讚
        /// </summary>
        public bool IsLikedByCurrentUser { get; set; }

        /// <summary>
        /// 子回覆列表
        /// </summary>
        public List<ThreadPostDetail> ChildPosts { get; set; } = new List<ThreadPostDetail>();
    }

    /// <summary>
    /// 建立主題請求 DTO
    /// </summary>
    public class CreateThreadRequest
    {
        /// <summary>
        /// 論壇版ID
        /// </summary>
        public int ForumId { get; set; }

        /// <summary>
        /// 主題標題
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 主題內容（Markdown格式）
        /// </summary>
        public string Content { get; set; } = string.Empty;
    }

    /// <summary>
    /// 建立回覆請求 DTO
    /// </summary>
    public class CreatePostRequest
    {
        /// <summary>
        /// 主題ID
        /// </summary>
        public long ThreadId { get; set; }

        /// <summary>
        /// 回覆內容（Markdown格式）
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// 父回覆ID（可選）
        /// </summary>
        public long? ParentPostId { get; set; }
    }

    /// <summary>
    /// 反應請求 DTO
    /// </summary>
    public class ReactionRequest
    {
        /// <summary>
        /// 目標類型
        /// </summary>
        public string TargetType { get; set; } = string.Empty;

        /// <summary>
        /// 目標ID
        /// </summary>
        public long TargetId { get; set; }

        /// <summary>
        /// 反應類型
        /// </summary>
        public string Kind { get; set; } = string.Empty;
    }

    /// <summary>
    /// 收藏請求 DTO
    /// </summary>
    public class BookmarkRequest
    {
        /// <summary>
        /// 目標類型
        /// </summary>
        public string TargetType { get; set; } = string.Empty;

        /// <summary>
        /// 目標ID
        /// </summary>
        public long TargetId { get; set; }
    }

    /// <summary>
    /// 論壇查詢請求 DTO
    /// </summary>
    public class ForumQueryRequest
    {
        /// <summary>
        /// 遊戲ID（可選）
        /// </summary>
        public int? GameId { get; set; }

        /// <summary>
        /// 關鍵字搜尋
        /// </summary>
        public string? Keyword { get; set; }

        /// <summary>
        /// 頁碼
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// 每頁數量
        /// </summary>
        public int PageSize { get; set; } = 20;
    }

    /// <summary>
    /// 主題查詢請求 DTO
    /// </summary>
    public class ThreadQueryRequest
    {
        /// <summary>
        /// 論壇版ID
        /// </summary>
        public int ForumId { get; set; }

        /// <summary>
        /// 關鍵字搜尋
        /// </summary>
        public string? Keyword { get; set; }

        /// <summary>
        /// 排序方式（latest/popular）
        /// </summary>
        public string SortBy { get; set; } = "latest";

        /// <summary>
        /// 頁碼
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// 每頁數量
        /// </summary>
        public int PageSize { get; set; } = 20;
    }

    /// <summary>
    /// 分頁回應 DTO
    /// </summary>
    public class PagedResponse<T>
    {
        /// <summary>
        /// 資料列表
        /// </summary>
        public List<T> Items { get; set; } = new List<T>();

        /// <summary>
        /// 總數量
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 頁碼
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 每頁數量
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 總頁數
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// 是否有上一頁
        /// </summary>
        public bool HasPreviousPage { get; set; }

        /// <summary>
        /// 是否有下一頁
        /// </summary>
        public bool HasNextPage { get; set; }
    }
} 
