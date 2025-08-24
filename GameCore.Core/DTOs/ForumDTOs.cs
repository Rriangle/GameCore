using System.ComponentModel.DataAnnotations;

namespace GameCore.Core.DTOs
{
    #region 論壇管理 DTOs

    /// <summary>
    /// 論壇版面 DTO
    /// </summary>
    public class ForumDto
    {
        /// <summary>論壇ID</summary>
        public int ForumId { get; set; }

        /// <summary>遊戲ID</summary>
        public int GameId { get; set; }

        /// <summary>遊戲名稱</summary>
        public string? GameName { get; set; }

        /// <summary>版面名稱</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>版面描述</summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>主題數量</summary>
        public int ThreadCount { get; set; }

        /// <summary>回覆數量</summary>
        public int PostCount { get; set; }

        /// <summary>瀏覽次數</summary>
        public int ViewCount { get; set; }

        /// <summary>是否啟用</summary>
        public bool IsActive { get; set; }

        /// <summary>排序順序</summary>
        public int SortOrder { get; set; }

        /// <summary>建立時間</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>更新時間</summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>最新主題</summary>
        public ThreadSummaryDto? LatestThread { get; set; }

        /// <summary>版主列表</summary>
        public List<ModeratorDto> Moderators { get; set; } = new();
    }

    /// <summary>
    /// 建立論壇版面請求 DTO
    /// </summary>
    public class CreateForumDto
    {
        /// <summary>遊戲ID</summary>
        [Required(ErrorMessage = "遊戲ID為必填")]
        public int GameId { get; set; }

        /// <summary>版面名稱</summary>
        [Required(ErrorMessage = "版面名稱為必填")]
        [StringLength(100, ErrorMessage = "版面名稱長度不能超過100字元")]
        public string Name { get; set; } = string.Empty;

        /// <summary>版面描述</summary>
        [Required(ErrorMessage = "版面描述為必填")]
        [StringLength(500, ErrorMessage = "版面描述長度不能超過500字元")]
        public string Description { get; set; } = string.Empty;

        /// <summary>排序順序</summary>
        [Range(0, int.MaxValue, ErrorMessage = "排序順序不能為負數")]
        public int SortOrder { get; set; } = 0;
    }

    /// <summary>
    /// 更新論壇版面請求 DTO
    /// </summary>
    public class UpdateForumDto
    {
        /// <summary>版面名稱</summary>
        [StringLength(100, ErrorMessage = "版面名稱長度不能超過100字元")]
        public string? Name { get; set; }

        /// <summary>版面描述</summary>
        [StringLength(500, ErrorMessage = "版面描述長度不能超過500字元")]
        public string? Description { get; set; }

        /// <summary>是否啟用</summary>
        public bool? IsActive { get; set; }

        /// <summary>排序順序</summary>
        [Range(0, int.MaxValue, ErrorMessage = "排序順序不能為負數")]
        public int? SortOrder { get; set; }
    }

    /// <summary>
    /// 版主 DTO
    /// </summary>
    public class ModeratorDto
    {
        /// <summary>使用者ID</summary>
        public int UserId { get; set; }

        /// <summary>使用者名稱</summary>
        public string? UserName { get; set; }

        /// <summary>暱稱</summary>
        public string? Nickname { get; set; }

        /// <summary>指派時間</summary>
        public DateTime AssignedAt { get; set; }
    }

    #endregion

    #region 主題管理 DTOs

    /// <summary>
    /// 討論主題 DTO
    /// </summary>
    public class ThreadDto
    {
        /// <summary>主題ID</summary>
        public int ThreadId { get; set; }

        /// <summary>論壇ID</summary>
        public int ForumId { get; set; }

        /// <summary>論壇名稱</summary>
        public string? ForumName { get; set; }

        /// <summary>作者ID</summary>
        public int AuthorId { get; set; }

        /// <summary>作者名稱</summary>
        public string? AuthorName { get; set; }

        /// <summary>作者暱稱</summary>
        public string? AuthorNickname { get; set; }

        /// <summary>主題標題</summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>主題內容</summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>主題狀態</summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>瀏覽次數</summary>
        public int ViewCount { get; set; }

        /// <summary>回覆數量</summary>
        public int PostCount { get; set; }

        /// <summary>讚數</summary>
        public int LikeCount { get; set; }

        /// <summary>是否置頂</summary>
        public bool IsPinned { get; set; }

        /// <summary>是否鎖定</summary>
        public bool IsLocked { get; set; }

        /// <summary>建立時間</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>更新時間</summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>最後回覆時間</summary>
        public DateTime? LastPostAt { get; set; }

        /// <summary>最後回覆者</summary>
        public string? LastPosterName { get; set; }

        /// <summary>標籤列表</summary>
        public List<string> Tags { get; set; } = new();

        /// <summary>使用者是否已按讚</summary>
        public bool IsLikedByUser { get; set; }

        /// <summary>使用者是否已收藏</summary>
        public bool IsBookmarkedByUser { get; set; }

        /// <summary>狀態顯示</summary>
        public string StatusDisplay => Status switch
        {
            "normal" => "正常",
            "hidden" => "隱藏",
            "archived" => "封存",
            "deleted" => "已刪除",
            _ => Status
        };
    }

    /// <summary>
    /// 主題摘要 DTO (用於列表顯示)
    /// </summary>
    public class ThreadSummaryDto
    {
        /// <summary>主題ID</summary>
        public int ThreadId { get; set; }

        /// <summary>主題標題</summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>作者名稱</summary>
        public string? AuthorName { get; set; }

        /// <summary>回覆數量</summary>
        public int PostCount { get; set; }

        /// <summary>瀏覽次數</summary>
        public int ViewCount { get; set; }

        /// <summary>讚數</summary>
        public int LikeCount { get; set; }

        /// <summary>是否置頂</summary>
        public bool IsPinned { get; set; }

        /// <summary>是否鎖定</summary>
        public bool IsLocked { get; set; }

        /// <summary>建立時間</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>最後回覆時間</summary>
        public DateTime? LastPostAt { get; set; }

        /// <summary>最後回覆者</summary>
        public string? LastPosterName { get; set; }
    }

    /// <summary>
    /// 建立主題請求 DTO
    /// </summary>
    public class CreateThreadDto
    {
        /// <summary>論壇ID</summary>
        [Required(ErrorMessage = "論壇ID為必填")]
        public int ForumId { get; set; }

        /// <summary>主題標題</summary>
        [Required(ErrorMessage = "主題標題為必填")]
        [StringLength(200, ErrorMessage = "主題標題長度不能超過200字元")]
        public string Title { get; set; } = string.Empty;

        /// <summary>主題內容</summary>
        [Required(ErrorMessage = "主題內容為必填")]
        [StringLength(10000, ErrorMessage = "主題內容長度不能超過10000字元")]
        public string Content { get; set; } = string.Empty;

        /// <summary>標籤列表</summary>
        public List<string> Tags { get; set; } = new();

        /// <summary>是否置頂</summary>
        public bool IsPinned { get; set; } = false;
    }

    /// <summary>
    /// 更新主題請求 DTO
    /// </summary>
    public class UpdateThreadDto
    {
        /// <summary>主題標題</summary>
        [StringLength(200, ErrorMessage = "主題標題長度不能超過200字元")]
        public string? Title { get; set; }

        /// <summary>主題內容</summary>
        [StringLength(10000, ErrorMessage = "主題內容長度不能超過10000字元")]
        public string? Content { get; set; }

        /// <summary>主題狀態</summary>
        public string? Status { get; set; }

        /// <summary>是否置頂</summary>
        public bool? IsPinned { get; set; }

        /// <summary>是否鎖定</summary>
        public bool? IsLocked { get; set; }

        /// <summary>標籤列表</summary>
        public List<string>? Tags { get; set; }
    }

    /// <summary>
    /// 主題搜尋請求 DTO
    /// </summary>
    public class ThreadSearchDto
    {
        /// <summary>關鍵字</summary>
        public string? Keyword { get; set; }

        /// <summary>論壇ID</summary>
        public int? ForumId { get; set; }

        /// <summary>作者ID</summary>
        public int? AuthorId { get; set; }

        /// <summary>主題狀態</summary>
        public string? Status { get; set; }

        /// <summary>標籤</summary>
        public string? Tag { get; set; }

        /// <summary>是否只顯示置頂</summary>
        public bool? OnlyPinned { get; set; }

        /// <summary>開始日期</summary>
        public DateTime? FromDate { get; set; }

        /// <summary>結束日期</summary>
        public DateTime? ToDate { get; set; }

        /// <summary>頁碼</summary>
        [Range(1, int.MaxValue, ErrorMessage = "頁碼必須大於0")]
        public int Page { get; set; } = 1;

        /// <summary>每頁筆數</summary>
        [Range(1, 100, ErrorMessage = "每頁筆數必須在1到100之間")]
        public int PageSize { get; set; } = 20;

        /// <summary>排序欄位</summary>
        public string SortBy { get; set; } = "LastPostAt";

        /// <summary>排序方向</summary>
        public string SortDirection { get; set; } = "desc";
    }

    #endregion

    #region 回覆管理 DTOs

    /// <summary>
    /// 回覆 DTO
    /// </summary>
    public class ThreadPostDto
    {
        /// <summary>回覆ID</summary>
        public int PostId { get; set; }

        /// <summary>主題ID</summary>
        public int ThreadId { get; set; }

        /// <summary>作者ID</summary>
        public int AuthorId { get; set; }

        /// <summary>作者名稱</summary>
        public string? AuthorName { get; set; }

        /// <summary>作者暱稱</summary>
        public string? AuthorNickname { get; set; }

        /// <summary>作者頭像</summary>
        public string? AuthorAvatar { get; set; }

        /// <summary>回覆內容</summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>回覆狀態</summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>父回覆ID</summary>
        public int? ParentPostId { get; set; }

        /// <summary>父回覆作者</summary>
        public string? ParentAuthorName { get; set; }

        /// <summary>讚數</summary>
        public int LikeCount { get; set; }

        /// <summary>樓層號</summary>
        public int FloorNumber { get; set; }

        /// <summary>建立時間</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>更新時間</summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>子回覆列表</summary>
        public List<ThreadPostDto> ChildPosts { get; set; } = new();

        /// <summary>使用者是否已按讚</summary>
        public bool IsLikedByUser { get; set; }

        /// <summary>是否為作者本人</summary>
        public bool IsAuthor { get; set; }

        /// <summary>狀態顯示</summary>
        public string StatusDisplay => Status switch
        {
            "normal" => "正常",
            "hidden" => "隱藏",
            "archived" => "封存",
            "deleted" => "已刪除",
            _ => Status
        };
    }

    /// <summary>
    /// 建立回覆請求 DTO
    /// </summary>
    public class CreateThreadPostDto
    {
        /// <summary>主題ID</summary>
        [Required(ErrorMessage = "主題ID為必填")]
        public int ThreadId { get; set; }

        /// <summary>回覆內容</summary>
        [Required(ErrorMessage = "回覆內容為必填")]
        [StringLength(5000, ErrorMessage = "回覆內容長度不能超過5000字元")]
        public string Content { get; set; } = string.Empty;

        /// <summary>父回覆ID (回覆特定樓層時使用)</summary>
        public int? ParentPostId { get; set; }
    }

    /// <summary>
    /// 更新回覆請求 DTO
    /// </summary>
    public class UpdateThreadPostDto
    {
        /// <summary>回覆內容</summary>
        [StringLength(5000, ErrorMessage = "回覆內容長度不能超過5000字元")]
        public string? Content { get; set; }

        /// <summary>回覆狀態</summary>
        public string? Status { get; set; }
    }

    #endregion

    #region 互動管理 DTOs

    /// <summary>
    /// 表情反應 DTO
    /// </summary>
    public class ReactionDto
    {
        /// <summary>反應ID</summary>
        public int ReactionId { get; set; }

        /// <summary>目標類型</summary>
        public string TargetType { get; set; } = string.Empty;

        /// <summary>目標ID</summary>
        public int TargetId { get; set; }

        /// <summary>使用者ID</summary>
        public int UserId { get; set; }

        /// <summary>使用者名稱</summary>
        public string? UserName { get; set; }

        /// <summary>反應類型</summary>
        public string ReactionType { get; set; } = string.Empty;

        /// <summary>建立時間</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>反應類型顯示</summary>
        public string ReactionTypeDisplay => ReactionType switch
        {
            "like" => "👍",
            "love" => "❤️",
            "laugh" => "😂",
            "wow" => "😲",
            "sad" => "😢",
            "angry" => "😡",
            _ => "👍"
        };
    }

    /// <summary>
    /// 反應統計 DTO
    /// </summary>
    public class ReactionStatsDto
    {
        /// <summary>反應類型</summary>
        public string ReactionType { get; set; } = string.Empty;

        /// <summary>反應數量</summary>
        public int Count { get; set; }

        /// <summary>使用者列表 (前幾名)</summary>
        public List<string> UserNames { get; set; } = new();

        /// <summary>反應類型顯示</summary>
        public string ReactionTypeDisplay => ReactionType switch
        {
            "like" => "👍",
            "love" => "❤️",
            "laugh" => "😂",
            "wow" => "😲",
            "sad" => "😢",
            "angry" => "😡",
            _ => "👍"
        };
    }

    /// <summary>
    /// 新增反應請求 DTO
    /// </summary>
    public class AddReactionDto
    {
        /// <summary>目標類型</summary>
        [Required(ErrorMessage = "目標類型為必填")]
        public string TargetType { get; set; } = string.Empty;

        /// <summary>目標ID</summary>
        [Required(ErrorMessage = "目標ID為必填")]
        public int TargetId { get; set; }

        /// <summary>反應類型</summary>
        [Required(ErrorMessage = "反應類型為必填")]
        public string ReactionType { get; set; } = string.Empty;
    }

    /// <summary>
    /// 收藏 DTO
    /// </summary>
    public class BookmarkDto
    {
        /// <summary>收藏ID</summary>
        public int BookmarkId { get; set; }

        /// <summary>目標類型</summary>
        public string TargetType { get; set; } = string.Empty;

        /// <summary>目標ID</summary>
        public int TargetId { get; set; }

        /// <summary>使用者ID</summary>
        public int UserId { get; set; }

        /// <summary>收藏標題</summary>
        public string? Title { get; set; }

        /// <summary>收藏描述</summary>
        public string? Description { get; set; }

        /// <summary>建立時間</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>目標類型顯示</summary>
        public string TargetTypeDisplay => TargetType switch
        {
            "thread" => "主題",
            "thread_post" => "回覆",
            "forum" => "版面",
            "game" => "遊戲",
            _ => TargetType
        };
    }

    /// <summary>
    /// 新增收藏請求 DTO
    /// </summary>
    public class AddBookmarkDto
    {
        /// <summary>目標類型</summary>
        [Required(ErrorMessage = "目標類型為必填")]
        public string TargetType { get; set; } = string.Empty;

        /// <summary>目標ID</summary>
        [Required(ErrorMessage = "目標ID為必填")]
        public int TargetId { get; set; }

        /// <summary>收藏備註</summary>
        [StringLength(200, ErrorMessage = "收藏備註長度不能超過200字元")]
        public string? Notes { get; set; }
    }

    #endregion

    #region 統計分析 DTOs

    /// <summary>
    /// 論壇統計 DTO
    /// </summary>
    public class ForumStatisticsDto
    {
        /// <summary>總論壇數</summary>
        public int TotalForums { get; set; }

        /// <summary>活躍論壇數</summary>
        public int ActiveForums { get; set; }

        /// <summary>總主題數</summary>
        public int TotalThreads { get; set; }

        /// <summary>總回覆數</summary>
        public int TotalPosts { get; set; }

        /// <summary>今日新增主題數</summary>
        public int TodayThreads { get; set; }

        /// <summary>今日新增回覆數</summary>
        public int TodayPosts { get; set; }

        /// <summary>活躍使用者數</summary>
        public int ActiveUsers { get; set; }

        /// <summary>總瀏覽次數</summary>
        public long TotalViews { get; set; }

        /// <summary>論壇活躍度排名</summary>
        public List<ForumActivityDto> ForumActivities { get; set; } = new();

        /// <summary>熱門主題</summary>
        public List<ThreadSummaryDto> PopularThreads { get; set; } = new();

        /// <summary>最新主題</summary>
        public List<ThreadSummaryDto> LatestThreads { get; set; } = new();
    }

    /// <summary>
    /// 論壇活躍度 DTO
    /// </summary>
    public class ForumActivityDto
    {
        /// <summary>論壇ID</summary>
        public int ForumId { get; set; }

        /// <summary>論壇名稱</summary>
        public string ForumName { get; set; } = string.Empty;

        /// <summary>遊戲名稱</summary>
        public string? GameName { get; set; }

        /// <summary>主題數量</summary>
        public int ThreadCount { get; set; }

        /// <summary>回覆數量</summary>
        public int PostCount { get; set; }

        /// <summary>瀏覽次數</summary>
        public int ViewCount { get; set; }

        /// <summary>活躍分數</summary>
        public decimal ActivityScore { get; set; }

        /// <summary>最後活動時間</summary>
        public DateTime? LastActivityAt { get; set; }
    }

    #endregion

    #region 分頁結果

    /// <summary>
    /// 分頁結果 DTO
    /// </summary>
    /// <typeparam name="T">資料類型</typeparam>
    public class ForumPagedResult<T>
    {
        /// <summary>當前頁碼</summary>
        public int Page { get; set; }

        /// <summary>每頁筆數</summary>
        public int PageSize { get; set; }

        /// <summary>總筆數</summary>
        public int TotalCount { get; set; }

        /// <summary>總頁數</summary>
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        /// <summary>是否有上一頁</summary>
        public bool HasPreviousPage => Page > 1;

        /// <summary>是否有下一頁</summary>
        public bool HasNextPage => Page < TotalPages;

        /// <summary>資料列表</summary>
        public List<T> Data { get; set; } = new();

        /// <summary>是否為空結果</summary>
        public bool IsEmpty => !Data.Any();
    }

    #endregion

    #region 服務結果

    /// <summary>
    /// 論壇服務執行結果
    /// </summary>
    public class ForumServiceResult
    {
        /// <summary>是否成功</summary>
        public bool Success { get; set; }

        /// <summary>訊息</summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>錯誤清單</summary>
        public List<string> Errors { get; set; } = new();

        /// <summary>建立成功結果</summary>
        public static ForumServiceResult CreateSuccess(string message = "操作成功")
        {
            return new ForumServiceResult { Success = true, Message = message };
        }

        /// <summary>建立失敗結果</summary>
        public static ForumServiceResult CreateFailure(string message, List<string>? errors = null)
        {
            return new ForumServiceResult 
            { 
                Success = false, 
                Message = message, 
                Errors = errors ?? new List<string>() 
            };
        }
    }

    /// <summary>
    /// 帶資料的論壇服務執行結果
    /// </summary>
    /// <typeparam name="T">資料類型</typeparam>
    public class ForumServiceResult<T> : ForumServiceResult
    {
        /// <summary>結果資料</summary>
        public T? Data { get; set; }

        /// <summary>建立成功結果</summary>
        public static ForumServiceResult<T> CreateSuccess(T data, string message = "操作成功")
        {
            return new ForumServiceResult<T> 
            { 
                Success = true, 
                Message = message, 
                Data = data 
            };
        }

        /// <summary>建立失敗結果</summary>
        public static new ForumServiceResult<T> CreateFailure(string message, List<string>? errors = null)
        {
            return new ForumServiceResult<T> 
            { 
                Success = false, 
                Message = message, 
                Errors = errors ?? new List<string>() 
            };
        }
    }

    #endregion
}