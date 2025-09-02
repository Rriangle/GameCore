using GameCore.Application.Common;
using GameCore.Application.DTOs;

namespace GameCore.Application.Services
{
    /// <summary>
    /// 小遊戲服務介面
    /// </summary>
    public interface IMiniGameService
    {
        /// <summary>
        /// 開始遊戲
        /// </summary>
        /// <param name="request">開始遊戲請求</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>遊戲結果</returns>
        Task<Result<GameResultResponse>> StartGameAsync(StartGameRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// 取得用戶遊戲記錄
        /// </summary>
        /// <param name="userId">用戶 ID</param>
        /// <param name="pageNumber">頁碼</param>
        /// <param name="pageSize">每頁大小</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>遊戲記錄</returns>
        Task<Result<PagedResult<GameRecordResponse>>> GetUserGameRecordsAsync(int userId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);

        /// <summary>
        /// 取得遊戲排行榜
        /// </summary>
        /// <param name="gameType">遊戲類型</param>
        /// <param name="limit">限制數量</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>排行榜</returns>
        Task<Result<IEnumerable<LeaderboardEntryResponse>>> GetLeaderboardAsync(string gameType, int limit, CancellationToken cancellationToken = default);

        /// <summary>
        /// 取得用戶遊戲統計
        /// </summary>
        /// <param name="userId">用戶 ID</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>遊戲統計</returns>
        Task<Result<GameStatsResponse>> GetUserGameStatsAsync(int userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// 取得遊戲設定
        /// </summary>
        /// <param name="gameType">遊戲類型</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>遊戲設定</returns>
        Task<Result<GameSettingsResponse>> GetGameSettingsAsync(string gameType, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// 開始遊戲請求
    /// </summary>
    public class StartGameRequest
    {
        /// <summary>
        /// 用戶 ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 遊戲類型
        /// </summary>
        public string GameType { get; set; } = string.Empty;

        /// <summary>
        /// 遊戲參數
        /// </summary>
        public Dictionary<string, object> GameParameters { get; set; } = new Dictionary<string, object>();
    }

    /// <summary>
    /// 遊戲結果回應
    /// </summary>
    public class GameResultResponse
    {
        /// <summary>
        /// 遊戲記錄 ID
        /// </summary>
        public int GameRecordId { get; set; }

        /// <summary>
        /// 用戶 ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 遊戲類型
        /// </summary>
        public string GameType { get; set; } = string.Empty;

        /// <summary>
        /// 分數
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// 結果
        /// </summary>
        public string Result { get; set; } = string.Empty;

        /// <summary>
        /// 遊戲時間（秒）
        /// </summary>
        public int GameDuration { get; set; }

        /// <summary>
        /// 獎勵點數
        /// </summary>
        public int RewardPoints { get; set; }

        /// <summary>
        /// 遊戲時間
        /// </summary>
        public DateTime GameTime { get; set; }

        /// <summary>
        /// 遊戲詳細資料
        /// </summary>
        public Dictionary<string, object> GameDetails { get; set; } = new Dictionary<string, object>();
    }

    /// <summary>
    /// 遊戲記錄回應
    /// </summary>
    public class GameRecordResponse
    {
        /// <summary>
        /// 遊戲記錄 ID
        /// </summary>
        public int GameRecordId { get; set; }

        /// <summary>
        /// 用戶 ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用戶名稱
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// 遊戲類型
        /// </summary>
        public string GameType { get; set; } = string.Empty;

        /// <summary>
        /// 分數
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// 結果
        /// </summary>
        public string Result { get; set; } = string.Empty;

        /// <summary>
        /// 遊戲時間（秒）
        /// </summary>
        public int GameDuration { get; set; }

        /// <summary>
        /// 獎勵點數
        /// </summary>
        public int RewardPoints { get; set; }

        /// <summary>
        /// 遊戲時間
        /// </summary>
        public DateTime GameTime { get; set; }
    }

    /// <summary>
    /// 排行榜項目回應
    /// </summary>
    public class LeaderboardEntryResponse
    {
        /// <summary>
        /// 排名
        /// </summary>
        public int Rank { get; set; }

        /// <summary>
        /// 用戶 ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 用戶名稱
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// 分數
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// 遊戲時間
        /// </summary>
        public DateTime GameTime { get; set; }
    }

    /// <summary>
    /// 遊戲統計回應
    /// </summary>
    public class GameStatsResponse
    {
        /// <summary>
        /// 用戶 ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 總遊戲次數
        /// </summary>
        public int TotalGamesPlayed { get; set; }

        /// <summary>
        /// 總分數
        /// </summary>
        public int TotalScore { get; set; }

        /// <summary>
        /// 平均分數
        /// </summary>
        public double AverageScore { get; set; }

        /// <summary>
        /// 最高分數
        /// </summary>
        public int HighestScore { get; set; }

        /// <summary>
        /// 總遊戲時間（秒）
        /// </summary>
        public int TotalGameTime { get; set; }

        /// <summary>
        /// 總獎勵點數
        /// </summary>
        public int TotalRewardPoints { get; set; }

        /// <summary>
        /// 勝利次數
        /// </summary>
        public int WinCount { get; set; }

        /// <summary>
        /// 失敗次數
        /// </summary>
        public int LossCount { get; set; }

        /// <summary>
        /// 勝率
        /// </summary>
        public double WinRate { get; set; }

        /// <summary>
        /// 各遊戲類型統計
        /// </summary>
        public Dictionary<string, GameTypeStats> GameTypeStats { get; set; } = new Dictionary<string, GameTypeStats>();
    }

    /// <summary>
    /// 遊戲類型統計
    /// </summary>
    public class GameTypeStats
    {
        /// <summary>
        /// 遊戲次數
        /// </summary>
        public int GamesPlayed { get; set; }

        /// <summary>
        /// 最高分數
        /// </summary>
        public int HighestScore { get; set; }

        /// <summary>
        /// 平均分數
        /// </summary>
        public double AverageScore { get; set; }

        /// <summary>
        /// 勝利次數
        /// </summary>
        public int WinCount { get; set; }

        /// <summary>
        /// 勝率
        /// </summary>
        public double WinRate { get; set; }
    }

    /// <summary>
    /// 遊戲設定回應
    /// </summary>
    public class GameSettingsResponse
    {
        /// <summary>
        /// 遊戲類型
        /// </summary>
        public string GameType { get; set; } = string.Empty;

        /// <summary>
        /// 遊戲名稱
        /// </summary>
        public string GameName { get; set; } = string.Empty;

        /// <summary>
        /// 遊戲描述
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 每日遊戲限制
        /// </summary>
        public int DailyGameLimit { get; set; }

        /// <summary>
        /// 基礎獎勵點數
        /// </summary>
        public int BaseRewardPoints { get; set; }

        /// <summary>
        /// 遊戲參數
        /// </summary>
        public Dictionary<string, object> GameParameters { get; set; } = new Dictionary<string, object>();
    }
} 