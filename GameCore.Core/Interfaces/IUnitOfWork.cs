using GameCore.Core.Entities;

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
        /// 論壇 Repository
        /// </summary>
        IForumRepository ForumRepository { get; }

        /// <summary>
        /// 遊戲 Repository
        /// </summary>
        IGameRepository GameRepository { get; }

        /// <summary>
        /// 商店 Repository
        /// </summary>
        IStoreRepository StoreRepository { get; }

        /// <summary>
        /// 管理員 Repository
        /// </summary>
        IManagerRepository ManagerRepository { get; }

        /// <summary>
        /// 聊天 Repository
        /// </summary>
        IChatRepository ChatRepository { get; }

        /// <summary>
        /// 通知 Repository
        /// </summary>
        INotificationRepository NotificationRepository { get; }

        /// <summary>
        /// 玩家市場 Repository
        /// </summary>
        IPlayerMarketRepository PlayerMarketRepository { get; }

        /// <summary>
        /// 簽到 Repository
        /// </summary>
        ISignInRepository SignInRepository { get; }

        /// <summary>
        /// 小遊戲 Repository
        /// </summary>
        IMiniGameRepository MiniGameRepository { get; }
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
}