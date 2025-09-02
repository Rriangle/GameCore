using GameCore.Domain.Entities;

namespace GameCore.Domain.Interfaces
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

        /// <summary>
        /// 聊天訊息 Repository
        /// </summary>
        IChatMessageRepository ChatMessageRepository { get; }

        /// <summary>
        /// 私人聊天 Repository
        /// </summary>
        IPrivateChatRepository PrivateChatRepository { get; }

        /// <summary>
        /// 私人訊息 Repository
        /// </summary>
        IPrivateMessageRepository PrivateMessageRepository { get; }

        /// <summary>
        /// 回覆 Repository
        /// </summary>
        IReplyRepository ReplyRepository { get; }

        /// <summary>
        /// 會員銷售資料 Repository
        /// </summary>
        IMemberSalesProfileRepository MemberSalesProfileRepository { get; }

        /// <summary>
        /// 用戶銷售資訊 Repository
        /// </summary>
        IUserSalesInformationRepository UserSalesInformationRepository { get; }

        /// <summary>
        /// 用戶錢包 Repository
        /// </summary>
        IUserWalletRepository UserWalletRepository { get; }

        /// <summary>
        /// 用戶權限 Repository
        /// </summary>
        IUserRightsRepository UserRightsRepository { get; }

        /// <summary>
        /// 玩家市場訂單 Repository
        /// </summary>
        IPlayerMarketOrderRepository PlayerMarketOrderRepository { get; }

        /// <summary>
        /// 市場交易 Repository
        /// </summary>
        IMarketTransactionRepository MarketTransactionRepository { get; }

        /// <summary>
        /// 市場評價 Repository
        /// </summary>
        IMarketReviewRepository MarketReviewRepository { get; }

        /// <summary>
        /// 管理者角色權限 Repository
        /// </summary>
        IManagerRolePermissionRepository ManagerRolePermissionRepository { get; }

        /// <summary>
        /// 產品 Repository
        /// </summary>
        IProductRepository ProductRepository { get; }

        /// <summary>
        /// 訂單 Repository
        /// </summary>
        IOrderRepository OrderRepository { get; }

        /// <summary>
        /// 購物車 Repository
        /// </summary>
        ICartRepository CartRepository { get; }

        /// <summary>
        /// 遊戲設定 Repository
        /// </summary>
        IGameSettingsRepository GameSettingsRepository { get; }

        /// <summary>
        /// 管理者資料 Repository
        /// </summary>
        IManagerDataRepository ManagerDataRepository { get; }

        /// <summary>
        /// 市場商品 Repository
        /// </summary>
        IMarketItemRepository MarketItemRepository { get; }
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
