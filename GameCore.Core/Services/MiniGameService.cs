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
    /// 小遊戲服務實現類
    /// 提供小遊戲相關的業務邏輯處理
    /// </summary>
    public class MiniGameService : IMiniGameService
    {
        private readonly IMiniGameRepository _miniGameRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotificationService _notificationService;
        private readonly ILogger<MiniGameService> _logger;

        public MiniGameService(
            IMiniGameRepository miniGameRepository,
            IUserRepository userRepository,
            INotificationService notificationService,
            ILogger<MiniGameService> logger)
        {
            _miniGameRepository = miniGameRepository;
            _userRepository = userRepository;
            _notificationService = notificationService;
            _logger = logger;
        }

        /// <summary>
        /// 獲取小遊戲排行榜
        /// </summary>
        /// <param name="gameType">遊戲類型</param>
        /// <param name="limit">限制數量</param>
        /// <returns>排行榜數據</returns>
        public async Task<List<MiniGameStats>> GetLeaderboardAsync(string gameType, int limit = 10)
        {
            try
            {
                _logger.LogInformation("獲取小遊戲排行榜，遊戲類型: {GameType}, 限制: {Limit}", gameType, limit);
                
                var leaderboard = await _miniGameRepository.GetLeaderboardAsync(gameType, limit);
                
                _logger.LogInformation("成功獲取排行榜，共 {Count} 條記錄", leaderboard.Count);
                return leaderboard;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取小遊戲排行榜時發生錯誤，遊戲類型: {GameType}", gameType);
                throw;
            }
        }

        /// <summary>
        /// 記錄遊戲結果
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <param name="gameType">遊戲類型</param>
        /// <param name="score">分數</param>
        /// <param name="duration">遊戲時長（秒）</param>
        /// <returns>遊戲統計數據</returns>
        public async Task<MiniGameStats> RecordGameResultAsync(int userId, string gameType, int score, int duration)
        {
            try
            {
                _logger.LogInformation("記錄遊戲結果，用戶ID: {UserId}, 遊戲類型: {GameType}, 分數: {Score}", 
                    userId, gameType, score);

                // 記錄遊戲結果
                var stats = await _miniGameRepository.RecordGameResultAsync(userId, gameType, score, duration);

                // 檢查是否達到新的里程碑
                await CheckAndCreateMilestoneNotificationAsync(userId, gameType, score, stats);

                // 更新用戶積分
                await UpdateUserPointsAsync(userId, score);

                _logger.LogInformation("成功記錄遊戲結果，用戶ID: {UserId}", userId);
                return stats;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "記錄遊戲結果時發生錯誤，用戶ID: {UserId}, 遊戲類型: {GameType}", 
                    userId, gameType);
                throw;
            }
        }

        /// <summary>
        /// 獲取用戶遊戲統計
        /// </summary>
        /// <param name="userId">用戶ID</param>
        /// <returns>用戶遊戲統計數據</returns>
        public async Task<MiniGameStats> GetUserStatsAsync(int userId)
        {
            try
            {
                _logger.LogInformation("獲取用戶遊戲統計，用戶ID: {UserId}", userId);
                
                var stats = await _miniGameRepository.GetUserStatsAsync(userId);
                
                _logger.LogInformation("成功獲取用戶統計，用戶ID: {UserId}", userId);
                return stats;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取用戶遊戲統計時發生錯誤，用戶ID: {UserId}", userId);
                throw;
            }
        }

        /// <summary>
        /// 獲取遊戲類型列表
        /// </summary>
        /// <returns>遊戲類型列表</returns>
        public async Task<List<string>> GetGameTypesAsync()
        {
            try
            {
                _logger.LogInformation("獲取遊戲類型列表");
                
                var gameTypes = await _miniGameRepository.GetGameTypesAsync();
                
                _logger.LogInformation("成功獲取遊戲類型列表，共 {Count} 種遊戲", gameTypes.Count);
                return gameTypes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取遊戲類型列表時發生錯誤");
                throw;
            }
        }

        /// <summary>
        /// 檢查並創建里程碑通知
        /// </summary>
        private async Task CheckAndCreateMilestoneNotificationAsync(int userId, string gameType, int score, MiniGameStats stats)
        {
            try
            {
                // 檢查是否達到新的最高分
                if (score > stats.TotalGames) // 假設 TotalGames 代表最高分
                {
                    await _notificationService.CreateNotificationAsync(
                        userId,
                        "遊戲成就",
                        $"恭喜！您在 {gameType} 遊戲中達到了新的最高分：{score} 分！",
                        "game_achievement"
                    );
                }

                // 檢查是否達到特定分數里程碑
                var milestones = new[] { 1000, 5000, 10000, 50000, 100000 };
                foreach (var milestone in milestones)
                {
                    if (score >= milestone && stats.TotalGames < milestone) // 首次達到里程碑
                    {
                        await _notificationService.CreateNotificationAsync(
                            userId,
                            "里程碑達成",
                            $"恭喜！您在 {gameType} 遊戲中首次達到了 {milestone} 分里程碑！",
                            "game_milestone"
                        );
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "創建里程碑通知時發生錯誤，用戶ID: {UserId}", userId);
                // 不拋出異常，避免影響主要流程
            }
        }

        /// <summary>
        /// 更新用戶積分
        /// </summary>
        private async Task UpdateUserPointsAsync(int userId, int score)
        {
            try
            {
                // 根據分數計算積分獎勵
                var pointsToAdd = CalculatePointsReward(score);
                
                if (pointsToAdd > 0)
                {
                    await _userRepository.AddPointsAsync(userId, pointsToAdd);
                    _logger.LogInformation("為用戶 {UserId} 添加 {Points} 積分", userId, pointsToAdd);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新用戶積分時發生錯誤，用戶ID: {UserId}", userId);
                // 不拋出異常，避免影響主要流程
            }
        }

        /// <summary>
        /// 計算積分獎勵
        /// </summary>
        private int CalculatePointsReward(int score)
        {
            // 簡單的積分計算邏輯
            if (score >= 10000) return 100;
            if (score >= 5000) return 50;
            if (score >= 1000) return 20;
            if (score >= 500) return 10;
            if (score >= 100) return 5;
            return 1;
        }
    }
}
