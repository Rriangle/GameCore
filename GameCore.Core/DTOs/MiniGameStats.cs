namespace GameCore.Core.DTOs
{
    /// <summary>
    /// 小遊戲統計資料
    /// </summary>
    public class MiniGameStats
    {
        /// <summary>
        /// 總遊戲次數
        /// </summary>
        public int TotalGames { get; set; }

        /// <summary>
        /// 勝利次數
        /// </summary>
        public int Wins { get; set; }

        /// <summary>
        /// 總獲得經驗值
        /// </summary>
        public int TotalExperience { get; set; }

        /// <summary>
        /// 總獲得點數
        /// </summary>
        public int TotalPoints { get; set; }

        /// <summary>
        /// 勝率
        /// </summary>
        public double WinRate => TotalGames > 0 ? (double)Wins / TotalGames * 100 : 0;
    }
}

