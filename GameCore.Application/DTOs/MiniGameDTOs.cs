using System.ComponentModel.DataAnnotations;

namespace GameCore.Application.DTOs
{
    /// <summary>
    /// 遊戲會話回應
    /// </summary>
    public class GameSessionResponse
    {
        /// <summary>
        /// 會話 ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用戶 ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 遊戲類型
        /// </summary>
        public string GameType { get; set; } = string.Empty;

        /// <summary>
        /// 開始時間
        /// </summary>
        public DateTime StartedAt { get; set; }

        /// <summary>
        /// 結束時間
        /// </summary>
        public DateTime? EndedAt { get; set; }

        /// <summary>
        /// 遊戲狀態
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// 遊戲配置
        /// </summary>
        public string? GameConfig { get; set; }
    }

    /// <summary>
    /// 遊戲會話 DTO (舊版本相容性)
    /// </summary>
    public class GameSessionDto : GameSessionResponse
    {
    }

    /// <summary>
    /// 遊戲結果回應
    /// </summary>
    public class GameResultResponse
    {
        /// <summary>
        /// 結果 ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 會話 ID
        /// </summary>
        public int SessionId { get; set; }

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
        /// 遊戲時間 (秒)
        /// </summary>
        public int GameTime { get; set; }

        /// <summary>
        /// 完成時間
        /// </summary>
        public DateTime CompletedAt { get; set; }

        /// <summary>
        /// 是否勝利
        /// </summary>
        public bool IsWin { get; set; }

        /// <summary>
        /// 獎勵
        /// </summary>
        public string? Rewards { get; set; }
    }

    /// <summary>
    /// 遊戲記錄回應
    /// </summary>
    public class GameRecordResponse
    {
        /// <summary>
        /// 記錄 ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用戶 ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 遊戲類型
        /// </summary>
        public string GameType { get; set; } = string.Empty;

        /// <summary>
        /// 最高分數
        /// </summary>
        public int HighScore { get; set; }

        /// <summary>
        /// 總遊戲次數
        /// </summary>
        public int TotalGames { get; set; }

        /// <summary>
        /// 勝利次數
        /// </summary>
        public int WinCount { get; set; }

        /// <summary>
        /// 總遊戲時間 (秒)
        /// </summary>
        public int TotalGameTime { get; set; }

        /// <summary>
        /// 最後遊戲時間
        /// </summary>
        public DateTime? LastPlayedAt { get; set; }
    }

    /// <summary>
    /// 遊戲設定回應
    /// </summary>
    public class GameSettingsResponse
    {
        /// <summary>
        /// 設定 ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 遊戲類型
        /// </summary>
        public string GameType { get; set; } = string.Empty;

        /// <summary>
        /// 設定名稱
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 設定值
        /// </summary>
        public string Value { get; set; } = string.Empty;

        /// <summary>
        /// 設定描述
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// 遊戲設定 DTO (舊版本相容性)
    /// </summary>
    public class GameSettingsDto : GameSettingsResponse
    {
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
        /// 遊戲類型
        /// </summary>
        public string GameType { get; set; } = string.Empty;

        /// <summary>
        /// 分數
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// 遊戲時間 (秒)
        /// </summary>
        public int GameTime { get; set; }

        /// <summary>
        /// 完成時間
        /// </summary>
        public DateTime CompletedAt { get; set; }
    }

    /// <summary>
    /// 排行榜項目 DTO (舊版本相容性)
    /// </summary>
    public class LeaderboardEntryDto : LeaderboardEntryResponse
    {
    }

    /// <summary>
    /// 開始遊戲請求
    /// </summary>
    public class StartGameRequest
    {
        /// <summary>
        /// 用戶 ID
        /// </summary>
        [Required(ErrorMessage = "用戶 ID 為必填")]
        public int UserId { get; set; }

        /// <summary>
        /// 遊戲類型
        /// </summary>
        [Required(ErrorMessage = "遊戲類型為必填")]
        public string GameType { get; set; } = string.Empty;

        /// <summary>
        /// 遊戲配置
        /// </summary>
        public string? GameConfig { get; set; }
    }

    /// <summary>
    /// 結束遊戲請求
    /// </summary>
    public class EndGameRequest
    {
        /// <summary>
        /// 會話 ID
        /// </summary>
        [Required(ErrorMessage = "會話 ID 為必填")]
        public int SessionId { get; set; }

        /// <summary>
        /// 分數
        /// </summary>
        [Required(ErrorMessage = "分數為必填")]
        [Range(0, int.MaxValue, ErrorMessage = "分數不能為負數")]
        public int Score { get; set; }

        /// <summary>
        /// 遊戲時間 (秒)
        /// </summary>
        [Required(ErrorMessage = "遊戲時間為必填")]
        [Range(0, int.MaxValue, ErrorMessage = "遊戲時間不能為負數")]
        public int GameTime { get; set; }

        /// <summary>
        /// 是否勝利
        /// </summary>
        public bool IsWin { get; set; }

        /// <summary>
        /// 獎勵
        /// </summary>
        public string? Rewards { get; set; }
    }

    /// <summary>
    /// 遊戲統計回應
    /// </summary>
    public class GameStatisticsResponse
    {
        /// <summary>
        /// 總遊戲次數
        /// </summary>
        public int TotalGames { get; set; }

        /// <summary>
        /// 總勝利次數
        /// </summary>
        public int TotalWins { get; set; }

        /// <summary>
        /// 勝率
        /// </summary>
        public double WinRate { get; set; }

        /// <summary>
        /// 平均分數
        /// </summary>
        public double AverageScore { get; set; }

        /// <summary>
        /// 最高分數
        /// </summary>
        public int HighScore { get; set; }

        /// <summary>
        /// 總遊戲時間 (小時)
        /// </summary>
        public double TotalGameTimeHours { get; set; }
    }

    /// <summary>
    /// 遊戲結果 DTO (舊版本相容性)
    /// </summary>
    public class GameResultDto : GameResultResponse
    {
    }

    /// <summary>
    /// 遊戲記錄 DTO (舊版本相容性)
    /// </summary>
    public class GameRecordDto : GameRecordResponse
    {
    }

    /// <summary>
    /// 遊戲統計 DTO (舊版本相容性)
    /// </summary>
    public class GameStatisticsDto : GameStatisticsResponse
    {
    }
} 