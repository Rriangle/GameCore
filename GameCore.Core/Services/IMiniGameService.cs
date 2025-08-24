using GameCore.Core.DTOs;

namespace GameCore.Core.Services
{
    /// <summary>
    /// 小遊戲服務介面 - 完整實現出發冒險系統
    /// 定義關卡戰鬥、每日次數限制、屬性變化、獎勵發放等核心功能
    /// 嚴格按照規格實現Asia/Taipei時區每日重置和寵物健康檢查
    /// </summary>
    public interface IMiniGameService
    {
        #region 遊戲基本管理

        /// <summary>
        /// 檢查使用者當日是否可以開始新遊戲
        /// 檢查每日3次限制、寵物健康狀態等條件
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <returns>是否可以開始遊戲及相關訊息</returns>
        Task<MiniGameEligibilityDto> CheckGameEligibilityAsync(int userId);

        /// <summary>
        /// 開始新的冒險遊戲
        /// 建立MiniGame記錄並檢查所有遊戲前置條件
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <returns>遊戲開始結果</returns>
        Task<ServiceResult<MiniGameSessionDto>> StartGameAsync(int userId);

        /// <summary>
        /// 完成遊戲並結算
        /// 更新寵物屬性、發放獎勵、記錄遊戲結果
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="gameId">遊戲ID</param>
        /// <param name="finishRequest">完成遊戲請求</param>
        /// <returns>遊戲結算結果</returns>
        Task<ServiceResult<MiniGameResultDto>> FinishGameAsync(int userId, int gameId, FinishGameDto finishRequest);

        /// <summary>
        /// 中斷遊戲
        /// 標記遊戲為中斷狀態，不計入每日次數
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="gameId">遊戲ID</param>
        /// <param name="reason">中斷原因</param>
        /// <returns>中斷結果</returns>
        Task<ServiceResult> AbortGameAsync(int userId, int gameId, string reason = "使用者中斷");

        #endregion

        #region 遊戲記錄查詢

        /// <summary>
        /// 取得使用者遊戲記錄
        /// 支援日期範圍和結果篩選
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="request">查詢請求</param>
        /// <returns>遊戲記錄列表</returns>
        Task<PagedResult<MiniGameRecordDto>> GetGameRecordsAsync(int userId, GetGameRecordsDto request);

        /// <summary>
        /// 取得使用者遊戲統計
        /// 包含總遊戲次數、勝率、平均獎勵等
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <returns>遊戲統計資訊</returns>
        Task<MiniGameStatsDto> GetGameStatisticsAsync(int userId);

        /// <summary>
        /// 取得當日遊戲狀態
        /// 包含今日已玩次數、剩餘次數等
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <returns>當日遊戲狀態</returns>
        Task<DailyGameStatusDto> GetDailyGameStatusAsync(int userId);

        #endregion

        #region 關卡設定與管理

        /// <summary>
        /// 取得關卡設定
        /// 包含怪物數、速度倍率、獎勵設定等
        /// </summary>
        /// <returns>關卡設定列表</returns>
        Task<List<GameLevelConfigDto>> GetLevelConfigsAsync();

        /// <summary>
        /// 取得使用者當前關卡
        /// 根據最後一次勝利的關卡決定下一關等級
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <returns>當前可挑戰關卡</returns>
        Task<int> GetUserCurrentLevelAsync(int userId);

        /// <summary>
        /// 計算關卡獎勵
        /// 根據關卡等級和勝負結果計算經驗值和點數
        /// </summary>
        /// <param name="level">關卡等級</param>
        /// <param name="isVictory">是否勝利</param>
        /// <returns>獎勵資訊</returns>
        Task<GameRewardDto> CalculateLevelRewardAsync(int level, bool isVictory);

        #endregion

        #region 寵物屬性影響

        /// <summary>
        /// 計算遊戲對寵物屬性的影響
        /// 根據勝負結果計算5維屬性變化
        /// </summary>
        /// <param name="isVictory">是否勝利</param>
        /// <returns>屬性變化資訊</returns>
        Task<PetStatsChangeDto> CalculatePetStatsChangeAsync(bool isVictory);

        /// <summary>
        /// 應用遊戲結果到寵物
        /// 更新寵物屬性、經驗、等級並執行健康檢查
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="statsChange">屬性變化</param>
        /// <param name="experienceGained">獲得經驗</param>
        /// <returns>應用結果</returns>
        Task<ServiceResult<PetStatsUpdateResultDto>> ApplyGameResultToPetAsync(int userId, PetStatsChangeDto statsChange, int experienceGained);

        #endregion

        #region 每日重置與維護

        /// <summary>
        /// 執行每日重置
        /// 重置遊戲次數限制並執行寵物每日衰減
        /// </summary>
        /// <param name="targetDate">目標日期 (Asia/Taipei時區)</param>
        /// <returns>重置結果</returns>
        Task<ServiceResult<DailyResetResultDto>> ProcessDailyResetAsync(DateTime? targetDate = null);

        /// <summary>
        /// 取得每日重置統計
        /// 查看系統每日重置的執行記錄
        /// </summary>
        /// <param name="days">查詢天數</param>
        /// <returns>重置統計列表</returns>
        Task<List<DailyResetStatsDto>> GetDailyResetStatsAsync(int days = 7);

        #endregion

        #region 管理員功能

        /// <summary>
        /// 取得遊戲系統設定
        /// 包含每日次數限制、關卡設定等
        /// </summary>
        /// <returns>系統設定</returns>
        Task<MiniGameSystemConfigDto> GetSystemConfigAsync();

        /// <summary>
        /// 更新遊戲系統設定
        /// 管理員調整遊戲參數
        /// </summary>
        /// <param name="config">新的系統設定</param>
        /// <returns>更新結果</returns>
        Task<ServiceResult> UpdateSystemConfigAsync(MiniGameSystemConfigDto config);

        /// <summary>
        /// 管理員查詢所有使用者遊戲記錄
        /// 支援多種篩選條件
        /// </summary>
        /// <param name="request">查詢請求</param>
        /// <returns>遊戲記錄列表</returns>
        Task<PagedResult<AdminMiniGameRecordDto>> GetAllGameRecordsAsync(AdminGameRecordsQueryDto request);

        /// <summary>
        /// 手動重置使用者每日次數
        /// 管理員功能，用於特殊情況
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="reason">重置原因</param>
        /// <returns>重置結果</returns>
        Task<ServiceResult> ResetUserDailyCountAsync(int userId, string reason);

        #endregion

        #region 排行榜與統計

        /// <summary>
        /// 取得遊戲排行榜
        /// 支援不同排行類型
        /// </summary>
        /// <param name="rankingType">排行類型</param>
        /// <param name="limit">限制筆數</param>
        /// <returns>排行榜列表</returns>
        Task<List<MiniGameRankingDto>> GetGameRankingsAsync(GameRankingType rankingType, int limit = 50);

        /// <summary>
        /// 取得遊戲整體統計
        /// 包含總遊戲次數、勝率分析等
        /// </summary>
        /// <returns>整體統計資訊</returns>
        Task<MiniGameOverallStatsDto> GetOverallStatisticsAsync();

        /// <summary>
        /// 取得關卡通過率統計
        /// 分析各關卡的難度和通過情況
        /// </summary>
        /// <returns>關卡統計列表</returns>
        Task<List<LevelPassRateDto>> GetLevelPassRatesAsync();

        #endregion
    }

    #region 列舉定義

    /// <summary>
    /// 遊戲結果類型
    /// </summary>
    public enum GameResult
    {
        /// <summary>勝利</summary>
        Victory = 1,
        /// <summary>失敗</summary>
        Defeat = 0,
        /// <summary>中斷</summary>
        Aborted = -1
    }

    /// <summary>
    /// 遊戲排行類型
    /// </summary>
    public enum GameRankingType
    {
        /// <summary>最高關卡</summary>
        HighestLevel,
        /// <summary>總遊戲次數</summary>
        TotalGames,
        /// <summary>勝利次數</summary>
        TotalVictories,
        /// <summary>勝率</summary>
        WinRate,
        /// <summary>總獲得點數</summary>
        TotalPointsEarned,
        /// <summary>總獲得經驗</summary>
        TotalExperienceEarned
    }

    #endregion
}