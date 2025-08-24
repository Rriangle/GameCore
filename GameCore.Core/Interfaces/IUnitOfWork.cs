namespace GameCore.Core.Interfaces
{
    /// <summary>
    /// 工作單元介面
    /// 統一管理所有 Repository，確保資料一致性
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        #region Repository 屬性
        /// <summary>
        /// 使用者 Repository
        /// </summary>
        IUserRepository UserRepository { get; }

        /// <summary>
        /// 寵物 Repository
        /// </summary>
        IPetRepository PetRepository { get; }

        /// <summary>
        /// 簽到 Repository
        /// </summary>
        ISignInRepository SignInRepository { get; }

        /// <summary>
        /// 小遊戲 Repository
        /// </summary>
        IMiniGameRepository MiniGameRepository { get; }

        /// <summary>
        /// 遊戲 Repository
        /// </summary>
        IGameRepository GameRepository { get; }

        /// <summary>
        /// 論壇 Repository
        /// </summary>
        IForumRepository ForumRepository { get; }

        /// <summary>
        /// 商城 Repository
        /// </summary>
        IStoreRepository StoreRepository { get; }

        /// <summary>
        /// 玩家市場 Repository
        /// </summary>
        IPlayerMarketRepository PlayerMarketRepository { get; }

        /// <summary>
        /// 通知 Repository
        /// </summary>
        INotificationRepository NotificationRepository { get; }

        /// <summary>
        /// 聊天 Repository
        /// </summary>
        IChatRepository ChatRepository { get; }

        /// <summary>
        /// 管理者 Repository
        /// </summary>
        IManagerRepository ManagerRepository { get; }
        #endregion

        /// <summary>
        /// 儲存所有變更
        /// </summary>
        /// <returns>受影響的記錄數</returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// 開始資料庫交易
        /// </summary>
        /// <returns>交易物件</returns>
        Task BeginTransactionAsync();

        /// <summary>
        /// 提交交易
        /// </summary>
        /// <returns>Task</returns>
        Task CommitTransactionAsync();

        /// <summary>
        /// 回滾交易
        /// </summary>
        /// <returns>Task</returns>
        Task RollbackTransactionAsync();
    }

    /// <summary>
    /// 小遊戲統計
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
        public int WinCount { get; set; }

        /// <summary>
        /// 失敗次數
        /// </summary>
        public int LoseCount { get; set; }

        /// <summary>
        /// 中途退出次數
        /// </summary>
        public int AbortCount { get; set; }

        /// <summary>
        /// 勝率
        /// </summary>
        public double WinRate => TotalGames > 0 ? (double)WinCount / TotalGames * 100 : 0;

        /// <summary>
        /// 總獲得經驗
        /// </summary>
        public int TotalExperienceGained { get; set; }

        /// <summary>
        /// 總獲得點數
        /// </summary>
        public int TotalPointsGained { get; set; }

        /// <summary>
        /// 最高關卡
        /// </summary>
        public int HighestLevel { get; set; }
    }
}