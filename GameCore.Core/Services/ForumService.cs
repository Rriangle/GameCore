using GameCore.Core.DTOs;
using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameCore.Core.Services
{
    /// <summary>
    /// 論壇服務實現類
    /// 提供論壇相關的業務邏輯處理
    /// </summary>
    public class ForumService : IForumService
    {
        private readonly IForumRepository _forumRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotificationService _notificationService;
        private readonly ILogger<ForumService> _logger;

        public ForumService(
            IForumRepository forumRepository,
            IUserRepository userRepository,
            INotificationService notificationService,
            ILogger<ForumService> logger)
        {
            _forumRepository = forumRepository;
            _userRepository = userRepository;
            _notificationService = notificationService;
            _logger = logger;
        }

        /// <summary>
        /// 獲取所有論壇
        /// </summary>
        /// <returns>論壇列表</returns>
        public async Task<List<Forum>> GetAllForumsAsync()
        {
            try
            {
                _logger.LogInformation("獲取所有論壇");
                
                var forums = await _forumRepository.GetAllForumsAsync();
                
                _logger.LogInformation("成功獲取論壇列表，共 {Count} 個論壇", forums.Count);
                return forums;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取論壇列表時發生錯誤");
                throw;
            }
        }

        /// <summary>
        /// 獲取論壇詳情
        /// </summary>
        /// <param name="forumId">論壇ID</param>
        /// <returns>論壇詳情</returns>
        public async Task<Forum> GetForumByIdAsync(int forumId)
        {
            try
            {
                _logger.LogInformation("獲取論壇詳情，論壇ID: {ForumId}", forumId);
                
                var forum = await _forumRepository.GetForumByIdAsync(forumId);
                
                if (forum == null)
                {
                    _logger.LogWarning("論壇不存在，論壇ID: {ForumId}", forumId);
                    return null;
                }
                
                _logger.LogInformation("成功獲取論壇詳情，論壇ID: {ForumId}", forumId);
                return forum;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取論壇詳情時發生錯誤，論壇ID: {ForumId}", forumId);
                throw;
            }
        }

        /// <summary>
        /// 獲取論壇文章列表
        /// </summary>
        /// <param name="forumId">論壇ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <param name="sortBy">排序方式</param>
        /// <returns>文章列表</returns>
        public async Task<PagedResult<Post>> GetForumPostsAsync(int forumId, int page = 1, int pageSize = 20, string sortBy = "latest")
        {
            try
            {
                _logger.LogInformation("獲取論壇文章列表，論壇ID: {ForumId}, 頁碼: {Page}, 每頁大小: {PageSize}, 排序: {SortBy}", 
                    forumId, page, pageSize, sortBy);
                
                var posts = await _forumRepository.GetForumPostsAsync(forumId, page, pageSize, sortBy);
                
                _logger.LogInformation("成功獲取論壇文章列表，論壇ID: {ForumId}, 共 {TotalCount} 篇文章", 
                    forumId, posts.TotalCount);
                return posts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取論壇文章列表時發生錯誤，論壇ID: {ForumId}", forumId);
                throw;
            }
        }

        /// <summary>
        /// 創建新文章
        /// </summary>
        /// <param name="post">文章資料</param>
        /// <returns>創建結果</returns>
        public async Task<bool> CreatePostAsync(Post post)
        {
            try
            {
                _logger.LogInformation("創建新文章，標題: {Title}, 作者ID: {AuthorId}", post.Title, post.AuthorId);
                
                // 設置創建時間
                post.CreatedAt = DateTime.UtcNow;
                post.UpdatedAt = DateTime.UtcNow;
                
                var result = await _forumRepository.CreatePostAsync(post);
                
                if (result)
                {
                    // 更新用戶統計資料
                    await UpdateUserPostStatsAsync(post.AuthorId);
                    
                    // 發送通知給論壇管理員
                    await NotifyForumModeratorsAsync(post.ForumId, post);
                    
                    _logger.LogInformation("成功創建新文章，文章ID: {PostId}", post.PostId);
                }
                else
                {
                    _logger.LogWarning("創建新文章失敗，標題: {Title}", post.Title);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "創建新文章時發生錯誤，標題: {Title}", post.Title);
                throw;
            }
        }

        /// <summary>
        /// 更新文章
        /// </summary>
        /// <param name="post">文章資料</param>
        /// <returns>更新結果</returns>
        public async Task<bool> UpdatePostAsync(Post post)
        {
            try
            {
                _logger.LogInformation("更新文章，文章ID: {PostId}", post.PostId);
                
                // 設置更新時間
                post.UpdatedAt = DateTime.UtcNow;
                
                var result = await _forumRepository.UpdatePostAsync(post);
                
                if (result)
                {
                    _logger.LogInformation("成功更新文章，文章ID: {PostId}", post.PostId);
                }
                else
                {
                    _logger.LogWarning("更新文章失敗，文章ID: {PostId}", post.PostId);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新文章時發生錯誤，文章ID: {PostId}", post.PostId);
                throw;
            }
        }

        /// <summary>
        /// 刪除文章
        /// </summary>
        /// <param name="postId">文章ID</param>
        /// <param name="userId">操作者ID</param>
        /// <returns>刪除結果</returns>
        public async Task<bool> DeletePostAsync(int postId, int userId)
        {
            try
            {
                _logger.LogInformation("刪除文章，文章ID: {PostId}, 操作者ID: {UserId}", postId, userId);
                
                // 檢查權限
                var post = await _forumRepository.GetPostByIdAsync(postId);
                if (post == null)
                {
                    _logger.LogWarning("文章不存在，無法刪除，文章ID: {PostId}", postId);
                    return false;
                }
                
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning("用戶不存在，無法刪除文章，用戶ID: {UserId}", userId);
                    return false;
                }
                
                // 檢查是否有權限刪除（作者或管理員）
                if (post.AuthorId != userId && user.Role != "Admin" && user.Role != "Manager")
                {
                    _logger.LogWarning("用戶無權限刪除文章，文章ID: {PostId}, 用戶ID: {UserId}", postId, userId);
                    return false;
                }
                
                var result = await _forumRepository.DeletePostAsync(postId);
                
                if (result)
                {
                    // 更新用戶統計資料
                    await UpdateUserPostStatsAsync(post.AuthorId);
                    
                    _logger.LogInformation("成功刪除文章，文章ID: {PostId}", postId);
                }
                else
                {
                    _logger.LogWarning("刪除文章失敗，文章ID: {PostId}", postId);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "刪除文章時發生錯誤，文章ID: {PostId}", postId);
                throw;
            }
        }

        /// <summary>
        /// 搜尋文章
        /// </summary>
        /// <param name="searchTerm">搜尋關鍵字</param>
        /// <param name="forumId">論壇ID（可選）</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>搜尋結果</returns>
        public async Task<PagedResult<Post>> SearchPostsAsync(string searchTerm, int? forumId = null, int page = 1, int pageSize = 20)
        {
            try
            {
                _logger.LogInformation("搜尋文章，關鍵字: {SearchTerm}, 論壇ID: {ForumId}, 頁碼: {Page}", 
                    searchTerm, forumId, page);
                
                var result = await _forumRepository.SearchPostsAsync(searchTerm, forumId, page, pageSize);
                
                _logger.LogInformation("成功搜尋文章，關鍵字: {SearchTerm}, 共 {TotalCount} 篇結果", 
                    searchTerm, result.TotalCount);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "搜尋文章時發生錯誤，關鍵字: {SearchTerm}", searchTerm);
                throw;
            }
        }

        /// <summary>
        /// 獲取熱門文章
        /// </summary>
        /// <param name="forumId">論壇ID（可選）</param>
        /// <param name="limit">限制數量</param>
        /// <returns>熱門文章列表</returns>
        public async Task<List<Post>> GetPopularPostsAsync(int? forumId = null, int limit = 10)
        {
            try
            {
                _logger.LogInformation("獲取熱門文章，論壇ID: {ForumId}, 限制: {Limit}", forumId, limit);
                
                var posts = await _forumRepository.GetPopularPostsAsync(forumId, limit);
                
                _logger.LogInformation("成功獲取熱門文章，共 {Count} 篇", posts.Count);
                return posts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取熱門文章時發生錯誤");
                throw;
            }
        }

        /// <summary>
        /// 更新用戶發文統計
        /// </summary>
        private async Task UpdateUserPostStatsAsync(int userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user != null)
                {
                    // 這裡可以實現更新用戶發文統計的邏輯
                    // 例如：增加發文數量、更新最後發文時間等
                    _logger.LogInformation("更新用戶發文統計，用戶ID: {UserId}", userId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新用戶發文統計時發生錯誤，用戶ID: {UserId}", userId);
                // 不拋出異常，避免影響主要流程
            }
        }

        /// <summary>
        /// 通知論壇管理員
        /// </summary>
        private async Task NotifyForumModeratorsAsync(int forumId, Post post)
        {
            try
            {
                // 這裡可以實現通知論壇管理員的邏輯
                // 例如：發送通知給論壇管理員有新文章發布
                _logger.LogInformation("通知論壇管理員新文章發布，論壇ID: {ForumId}, 文章ID: {PostId}", 
                    forumId, post.PostId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "通知論壇管理員時發生錯誤，論壇ID: {ForumId}", forumId);
                // 不拋出異常，避免影響主要流程
            }
        }
    }
}
