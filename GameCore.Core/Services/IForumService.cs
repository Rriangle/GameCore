using GameCore.Core.DTOs;

namespace GameCore.Core.Services
{
    /// <summary>
    /// 論壇服務介面 - 完整實現論壇與互動功能
    /// 提供版面管理、主題討論、回覆互動、反應收藏等完整論壇功能
    /// 嚴格按照規格要求實現論壇業務邏輯和內容狀態管理
    /// </summary>
    public interface IForumService
    {
        #region 論壇版面管理

        /// <summary>
        /// 取得論壇版面列表
        /// 支援遊戲篩選和啟用狀態篩選
        /// </summary>
        /// <param name="gameId">遊戲ID篩選</param>
        /// <param name="activeOnly">是否只顯示啟用的版面</param>
        /// <returns>論壇版面列表</returns>
        Task<List<ForumDto>> GetForumsAsync(int? gameId = null, bool activeOnly = true);

        /// <summary>
        /// 取得論壇版面詳細資訊
        /// 包含基本資訊、統計數據、版主列表、最新主題等
        /// </summary>
        /// <param name="forumId">論壇ID</param>
        /// <returns>論壇版面詳細資訊</returns>
        Task<ForumDto?> GetForumDetailAsync(int forumId);

        /// <summary>
        /// 建立論壇版面 (管理員限定)
        /// 檢查遊戲存在性，建立版面記錄，設定預設權限
        /// </summary>
        /// <param name="createDto">建立論壇版面請求</param>
        /// <returns>操作結果和版面資訊</returns>
        Task<ForumServiceResult<ForumDto>> CreateForumAsync(CreateForumDto createDto);

        /// <summary>
        /// 更新論壇版面 (管理員限定)
        /// 更新版面資訊，記錄異動日誌
        /// </summary>
        /// <param name="forumId">論壇ID</param>
        /// <param name="updateDto">更新論壇版面請求</param>
        /// <returns>操作結果和更新後版面資訊</returns>
        Task<ForumServiceResult<ForumDto>> UpdateForumAsync(int forumId, UpdateForumDto updateDto);

        #endregion

        #region 主題管理

        /// <summary>
        /// 取得主題列表
        /// 支援多維度搜尋、篩選、排序和分頁
        /// </summary>
        /// <param name="searchDto">搜尋條件</param>
        /// <param name="userId">使用者ID (用於個人化顯示)</param>
        /// <returns>分頁主題列表</returns>
        Task<ForumPagedResult<ThreadDto>> GetThreadsAsync(ThreadSearchDto searchDto, int? userId = null);

        /// <summary>
        /// 取得主題詳細資訊
        /// 包含基本資訊、作者資料、互動統計、使用者狀態等
        /// </summary>
        /// <param name="threadId">主題ID</param>
        /// <param name="userId">使用者ID (用於個人化顯示)</param>
        /// <returns>主題詳細資訊</returns>
        Task<ThreadDto?> GetThreadDetailAsync(int threadId, int? userId = null);

        /// <summary>
        /// 建立主題
        /// 檢查版面權限、建立主題記錄、更新版面統計
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="createDto">建立主題請求</param>
        /// <returns>操作結果和主題資訊</returns>
        Task<ForumServiceResult<ThreadDto>> CreateThreadAsync(int userId, CreateThreadDto createDto);

        /// <summary>
        /// 更新主題 (作者或管理員)
        /// 檢查編輯權限、更新主題資訊、記錄異動日誌
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="threadId">主題ID</param>
        /// <param name="updateDto">更新主題請求</param>
        /// <returns>操作結果和更新後主題資訊</returns>
        Task<ForumServiceResult<ThreadDto>> UpdateThreadAsync(int userId, int threadId, UpdateThreadDto updateDto);

        /// <summary>
        /// 增加主題瀏覽次數
        /// 記錄使用者瀏覽行為，防止重複計算
        /// </summary>
        /// <param name="threadId">主題ID</param>
        /// <param name="userId">使用者ID</param>
        /// <returns>增加結果</returns>
        Task<ForumServiceResult> IncrementThreadViewAsync(int threadId, int userId);

        #endregion

        #region 回覆管理

        /// <summary>
        /// 取得主題回覆列表
        /// 支援分頁和層級結構顯示
        /// </summary>
        /// <param name="threadId">主題ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <param name="userId">使用者ID (用於個人化顯示)</param>
        /// <returns>分頁回覆列表</returns>
        Task<ForumPagedResult<ThreadPostDto>> GetThreadPostsAsync(int threadId, int page, int pageSize, int? userId = null);

        /// <summary>
        /// 建立回覆
        /// 檢查主題狀態、建立回覆記錄、更新主題統計
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="createDto">建立回覆請求</param>
        /// <returns>操作結果和回覆資訊</returns>
        Task<ForumServiceResult<ThreadPostDto>> CreateThreadPostAsync(int userId, CreateThreadPostDto createDto);

        /// <summary>
        /// 更新回覆 (作者或管理員)
        /// 檢查編輯權限、更新回覆內容、記錄異動日誌
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="postId">回覆ID</param>
        /// <param name="updateDto">更新回覆請求</param>
        /// <returns>操作結果和更新後回覆資訊</returns>
        Task<ForumServiceResult<ThreadPostDto>> UpdateThreadPostAsync(int userId, int postId, UpdateThreadPostDto updateDto);

        /// <summary>
        /// 刪除回覆 (作者或管理員)
        /// 軟刪除回覆、更新主題統計、通知相關使用者
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="postId">回覆ID</param>
        /// <returns>刪除結果</returns>
        Task<ForumServiceResult> DeleteThreadPostAsync(int userId, int postId);

        #endregion

        #region 互動管理

        /// <summary>
        /// 新增反應
        /// 檢查目標存在性、建立反應記錄、更新統計
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="reactionDto">反應請求</param>
        /// <returns>操作結果和反應資訊</returns>
        Task<ForumServiceResult<ReactionDto>> AddReactionAsync(int userId, AddReactionDto reactionDto);

        /// <summary>
        /// 移除反應
        /// 刪除反應記錄、更新統計
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="targetType">目標類型</param>
        /// <param name="targetId">目標ID</param>
        /// <param name="reactionType">反應類型</param>
        /// <returns>移除結果</returns>
        Task<ForumServiceResult> RemoveReactionAsync(int userId, string targetType, int targetId, string reactionType);

        /// <summary>
        /// 取得反應統計
        /// 計算各種反應的數量和使用者列表
        /// </summary>
        /// <param name="targetType">目標類型</param>
        /// <param name="targetId">目標ID</param>
        /// <returns>反應統計列表</returns>
        Task<List<ReactionStatsDto>> GetReactionStatsAsync(string targetType, int targetId);

        /// <summary>
        /// 新增收藏
        /// 建立收藏記錄、檢查重複收藏
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="bookmarkDto">收藏請求</param>
        /// <returns>操作結果和收藏資訊</returns>
        Task<ForumServiceResult<BookmarkDto>> AddBookmarkAsync(int userId, AddBookmarkDto bookmarkDto);

        /// <summary>
        /// 移除收藏
        /// 檢查擁有者權限、刪除收藏記錄
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="bookmarkId">收藏ID</param>
        /// <returns>移除結果</returns>
        Task<ForumServiceResult> RemoveBookmarkAsync(int userId, int bookmarkId);

        /// <summary>
        /// 取得使用者收藏列表
        /// 支援類型篩選和分頁
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="targetType">目標類型篩選</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <returns>分頁收藏列表</returns>
        Task<ForumPagedResult<BookmarkDto>> GetUserBookmarksAsync(int userId, string? targetType, int page, int pageSize);

        #endregion

        #region 統計分析

        /// <summary>
        /// 取得論壇統計資訊
        /// 計算版面數量、主題回覆統計、活躍度指標等
        /// </summary>
        /// <returns>論壇統計資訊</returns>
        Task<ForumStatisticsDto> GetStatisticsAsync();

        /// <summary>
        /// 取得熱門主題
        /// 根據時間範圍和熱門度算法排序
        /// </summary>
        /// <param name="period">時間範圍</param>
        /// <param name="limit">限制筆數</param>
        /// <returns>熱門主題列表</returns>
        Task<List<ThreadSummaryDto>> GetPopularThreadsAsync(string period, int limit);

        /// <summary>
        /// 取得論壇活躍度排名
        /// 計算各版面的活躍分數和排名
        /// </summary>
        /// <param name="limit">限制筆數</param>
        /// <returns>活躍度排名列表</returns>
        Task<List<ForumActivityDto>> GetForumActivityRankingAsync(int limit);

        #endregion

        #region 管理員功能

        /// <summary>
        /// 管理員更新主題狀態
        /// 更新主題狀態、記錄管理操作、通知相關使用者
        /// </summary>
        /// <param name="threadId">主題ID</param>
        /// <param name="statusDto">狀態更新請求</param>
        /// <returns>操作結果和更新後主題資訊</returns>
        Task<ForumServiceResult<ThreadDto>> UpdateThreadStatusAsync(int threadId, UpdateThreadStatusDto statusDto);

        /// <summary>
        /// 管理員更新回覆狀態
        /// 更新回覆狀態、記錄管理操作、通知相關使用者
        /// </summary>
        /// <param name="postId">回覆ID</param>
        /// <param name="statusDto">狀態更新請求</param>
        /// <returns>操作結果和更新後回覆資訊</returns>
        Task<ForumServiceResult<ThreadPostDto>> UpdatePostStatusAsync(int postId, UpdatePostStatusDto statusDto);

        /// <summary>
        /// 管理員批量處理內容
        /// 批量更新主題或回覆狀態
        /// </summary>
        /// <param name="targetType">目標類型</param>
        /// <param name="targetIds">目標ID列表</param>
        /// <param name="action">操作動作</param>
        /// <param name="reason">操作原因</param>
        /// <returns>批量處理結果</returns>
        Task<ForumServiceResult> BatchModerationAsync(string targetType, List<int> targetIds, string action, string reason);

        #endregion

        #region 狀態機驗證

        /// <summary>
        /// 驗證主題狀態轉換
        /// 按照狀態機規則驗證狀態轉換的合法性
        /// </summary>
        /// <param name="currentStatus">當前狀態</param>
        /// <param name="newStatus">新狀態</param>
        /// <returns>是否允許轉換</returns>
        bool ValidateThreadStatusTransition(string currentStatus, string newStatus);

        /// <summary>
        /// 驗證回覆狀態轉換
        /// 按照業務規則驗證回覆狀態轉換的合法性
        /// </summary>
        /// <param name="currentStatus">當前狀態</param>
        /// <param name="newStatus">新狀態</param>
        /// <returns>是否允許轉換</returns>
        bool ValidatePostStatusTransition(string currentStatus, string newStatus);

        #endregion

        #region 權限檢查

        /// <summary>
        /// 檢查使用者版面權限
        /// 驗證使用者是否有權限在指定版面發表內容
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="forumId">版面ID</param>
        /// <param name="action">操作類型</param>
        /// <returns>是否有權限</returns>
        Task<bool> CheckForumPermissionAsync(int userId, int forumId, string action);

        /// <summary>
        /// 檢查主題編輯權限
        /// 驗證使用者是否可編輯指定主題
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="threadId">主題ID</param>
        /// <returns>是否可編輯</returns>
        Task<bool> CheckThreadEditPermissionAsync(int userId, int threadId);

        /// <summary>
        /// 檢查回覆編輯權限
        /// 驗證使用者是否可編輯指定回覆
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="postId">回覆ID</param>
        /// <returns>是否可編輯</returns>
        Task<bool> CheckPostEditPermissionAsync(int userId, int postId);

        #endregion

        #region 通知整合

        /// <summary>
        /// 發送論壇相關通知
        /// 整合通知系統發送新回覆、反應、收藏等通知
        /// </summary>
        /// <param name="type">通知類型</param>
        /// <param name="userId">接收者ID</param>
        /// <param name="data">通知資料</param>
        /// <returns>發送結果</returns>
        Task<ForumServiceResult> SendForumNotificationAsync(string type, int userId, object data);

        #endregion

        #region 內容檢查

        /// <summary>
        /// 檢查內容合規性
        /// 使用過濾規則檢查主題和回覆內容
        /// </summary>
        /// <param name="content">內容文字</param>
        /// <param name="contentType">內容類型</param>
        /// <returns>檢查結果</returns>
        Task<ContentModerationResult> CheckContentModerationAsync(string content, string contentType);

        /// <summary>
        /// 自動標記可疑內容
        /// 根據關鍵字和規則自動標記需要審核的內容
        /// </summary>
        /// <param name="content">內容文字</param>
        /// <returns>是否需要審核</returns>
        Task<bool> RequiresModerationAsync(string content);

        #endregion
    }

    #region 輔助類別

    /// <summary>
    /// 內容審核結果
    /// </summary>
    public class ContentModerationResult
    {
        /// <summary>是否通過審核</summary>
        public bool IsApproved { get; set; }

        /// <summary>審核原因</summary>
        public string? Reason { get; set; }

        /// <summary>建議動作</summary>
        public string? SuggestedAction { get; set; }

        /// <summary>敏感詞彙</summary>
        public List<string> DetectedKeywords { get; set; } = new();
    }

    /// <summary>
    /// 更新主題狀態請求 DTO
    /// </summary>
    public class UpdateThreadStatusDto
    {
        /// <summary>主題狀態</summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>狀態變更原因</summary>
        public string? Reason { get; set; }

        /// <summary>備註</summary>
        public string? Notes { get; set; }
    }

    /// <summary>
    /// 更新回覆狀態請求 DTO
    /// </summary>
    public class UpdatePostStatusDto
    {
        /// <summary>回覆狀態</summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>狀態變更原因</summary>
        public string? Reason { get; set; }

        /// <summary>備註</summary>
        public string? Notes { get; set; }
    }

    #endregion
}