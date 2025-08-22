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
    /// 用戶服務實現類
    /// 提供用戶相關的業務邏輯處理
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly INotificationService _notificationService;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IUserRepository userRepository,
            INotificationService notificationService,
            ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _notificationService = notificationService;
            _logger = logger;
        }

        /// <summary>
        /// 獲取用戶資料
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns>用戶資料</returns>
        public async Task<User> GetUserByIdAsync(int userId)
        {
            try
            {
                _logger.LogInformation("獲取用戶資料，用戶ID: {UserId}", userId);
                
                var user = await _userRepository.GetByIdAsync(userId);
                
                if (user == null)
                {
                    _logger.LogWarning("用戶不存在，用戶ID: {UserId}", userId);
                    return null;
                }
                
                _logger.LogInformation("成功獲取用戶資料，用戶ID: {UserId}", userId);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取用戶資料時發生錯誤，用戶ID: {UserId}", userId);
                throw;
            }
        }

        /// <summary>
        /// 更新用戶資料
        /// </summary>
        /// <param name="user">用戶資料</param>
        /// <returns>更新結果</returns>
        public async Task<bool> UpdateUserAsync(User user)
        {
            try
            {
                _logger.LogInformation("更新用戶資料，用戶ID: {UserId}", user.UserId);
                
                var result = await _userRepository.UpdateAsync(user);
                
                if (result)
                {
                    _logger.LogInformation("成功更新用戶資料，用戶ID: {UserId}", user.UserId);
                }
                else
                {
                    _logger.LogWarning("更新用戶資料失敗，用戶ID: {UserId}", user.UserId);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新用戶資料時發生錯誤，用戶ID: {UserId}", user.UserId);
                throw;
            }
        }

        /// <summary>
        /// 獲取積分排行榜
        /// </summary>
        /// <param name="limit">限制數量</param>
        /// <returns>積分排行榜</returns>
        public async Task<List<User>> GetPointsLeaderboardAsync(int limit = 10)
        {
            try
            {
                _logger.LogInformation("獲取積分排行榜，限制: {Limit}", limit);
                
                var leaderboard = await _userRepository.GetPointsLeaderboardAsync(limit);
                
                _logger.LogInformation("成功獲取積分排行榜，共 {Count} 條記錄", leaderboard.Count);
                return leaderboard;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取積分排行榜時發生錯誤");
                throw;
            }
        }

        /// <summary>
        /// 搜尋用戶
        /// </summary>
        /// <param name="searchTerm">搜尋關鍵字</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <returns>搜尋結果</returns>
        public async Task<PagedResult<User>> SearchUsersAsync(string searchTerm, int page = 1, int pageSize = 10)
        {
            try
            {
                _logger.LogInformation("搜尋用戶，關鍵字: {SearchTerm}, 頁碼: {Page}, 每頁大小: {PageSize}", 
                    searchTerm, page, pageSize);
                
                var result = await _userRepository.SearchUsersAsync(searchTerm, page, pageSize);
                
                _logger.LogInformation("成功搜尋用戶，共 {TotalCount} 條記錄", result.TotalCount);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "搜尋用戶時發生錯誤，關鍵字: {SearchTerm}", searchTerm);
                throw;
            }
        }

        /// <summary>
        /// 獲取用戶統計資料
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns>用戶統計資料</returns>
        public async Task<UserStats> GetUserStatsAsync(int userId)
        {
            try
            {
                _logger.LogInformation("獲取用戶統計資料，用戶ID: {UserId}", userId);
                
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning("用戶不存在，無法獲取統計資料，用戶ID: {UserId}", userId);
                    return null;
                }
                
                var stats = new UserStats
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    Email = user.Email,
                    Points = user.Points,
                    Level = user.Level,
                    JoinDate = user.CreatedAt,
                    LastLoginDate = user.LastLoginAt,
                    TotalPosts = user.TotalPosts,
                    TotalLikes = user.TotalLikes,
                    TotalComments = user.TotalComments
                };
                
                _logger.LogInformation("成功獲取用戶統計資料，用戶ID: {UserId}", userId);
                return stats;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取用戶統計資料時發生錯誤，用戶ID: {UserId}", userId);
                throw;
            }
        }

        /// <summary>
        /// 更新用戶最後登入時間
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns>更新結果</returns>
        public async Task<bool> UpdateLastLoginAsync(int userId)
        {
            try
            {
                _logger.LogInformation("更新用戶最後登入時間，用戶ID: {UserId}", userId);
                
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning("用戶不存在，無法更新最後登入時間，用戶ID: {UserId}", userId);
                    return false;
                }
                
                user.LastLoginAt = DateTime.UtcNow;
                var result = await _userRepository.UpdateAsync(user);
                
                if (result)
                {
                    _logger.LogInformation("成功更新用戶最後登入時間，用戶ID: {UserId}", userId);
                }
                else
                {
                    _logger.LogWarning("更新用戶最後登入時間失敗，用戶ID: {UserId}", userId);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新用戶最後登入時間時發生錯誤，用戶ID: {UserId}", userId);
                throw;
            }
        }

        /// <summary>
        /// 檢查用戶是否為管理員
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns>是否為管理員</returns>
        public async Task<bool> IsAdminAsync(int userId)
        {
            try
            {
                _logger.LogInformation("檢查用戶是否為管理員，用戶ID: {UserId}", userId);
                
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning("用戶不存在，無法檢查管理員權限，用戶ID: {UserId}", userId);
                    return false;
                }
                
                var isAdmin = user.Role == "Admin" || user.Role == "Manager";
                
                _logger.LogInformation("用戶 {UserId} 管理員權限檢查結果: {IsAdmin}", userId, isAdmin);
                return isAdmin;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "檢查用戶管理員權限時發生錯誤，用戶ID: {UserId}", userId);
                throw;
            }
        }

        /// <summary>
        /// 獲取用戶活動記錄
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="limit">限制數量</param>
        /// <returns>活動記錄列表</returns>
        public async Task<List<UserActivity>> GetUserActivitiesAsync(int userId, int limit = 20)
        {
            try
            {
                _logger.LogInformation("獲取用戶活動記錄，用戶ID: {UserId}, 限制: {Limit}", userId, limit);
                
                // 這裡可以實現獲取用戶活動記錄的邏輯
                // 例如：發文、評論、點讚等活動
                var activities = new List<UserActivity>();
                
                // 模擬活動記錄
                activities.Add(new UserActivity
                {
                    UserId = userId,
                    ActivityType = "login",
                    Description = "用戶登入系統",
                    CreatedAt = DateTime.UtcNow
                });
                
                _logger.LogInformation("成功獲取用戶活動記錄，用戶ID: {UserId}, 共 {Count} 條記錄", 
                    userId, activities.Count);
                return activities;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取用戶活動記錄時發生錯誤，用戶ID: {UserId}", userId);
                throw;
            }
        }
    }

    /// <summary>
    /// 用戶統計資料 DTO
    /// </summary>
    public class UserStats
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public int Points { get; set; }
        public int Level { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public int TotalPosts { get; set; }
        public int TotalLikes { get; set; }
        public int TotalComments { get; set; }
    }

    /// <summary>
    /// 用戶活動記錄 DTO
    /// </summary>
    public class UserActivity
    {
        public int UserId { get; set; }
        public string ActivityType { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
