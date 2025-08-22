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
    /// 簽到 Repository 介面
    /// </summary>
    public interface ISignInRepository : IRepository<UserSignInStats>
    {
        /// <summary>
        /// 根據日期取得簽到記錄
        /// </summary>
        /// <param name="userId">使用者 ID</param>
        /// <param name="date">日期</param>
        /// <returns>簽到記錄或 null</returns>
        Task<UserSignInStats?> GetSignInByDateAsync(int userId, DateTime date);

        /// <summary>
        /// 取得最近的簽到記錄
        /// </summary>
        /// <param name="userId">使用者 ID</param>
        /// <param name="days">天數</param>
        /// <returns>簽到記錄列表</returns>
        Task<IEnumerable<UserSignInStats>> GetRecentSignInsAsync(int userId, int days);

        /// <summary>
        /// 取得日期範圍內的簽到記錄
        /// </summary>
        /// <param name="userId">使用者 ID</param>
        /// <param name="startDate">開始日期</param>
        /// <param name="endDate">結束日期</param>
        /// <returns>簽到記錄列表</returns>
        Task<IEnumerable<UserSignInStats>> GetSignInsByDateRangeAsync(int userId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// 取得簽到記錄總數
        /// </summary>
        /// <param name="userId">使用者 ID</param>
        /// <returns>記錄總數</returns>
        Task<int> GetSignInCountAsync(int userId);

        /// <summary>
        /// 取得簽到歷史 (分頁)
        /// </summary>
        /// <param name="userId">使用者 ID</param>
        /// <param name="page">頁數</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <returns>簽到記錄</returns>
        Task<IEnumerable<UserSignInStats>> GetSignInHistoryAsync(int userId, int page, int pageSize);
    }

    /// <summary>
    /// 小遊戲 Repository 介面
    /// </summary>
    public interface IMiniGameRepository : IRepository<MiniGame>
    {
        /// <summary>
        /// 取得使用者今日遊戲次數
        /// </summary>
        /// <param name="userId">使用者 ID</param>
        /// <returns>今日遊戲次數</returns>
        Task<int> GetTodayGameCountAsync(int userId);

        /// <summary>
        /// 取得使用者遊戲記錄
        /// </summary>
        /// <param name="userId">使用者 ID</param>
        /// <param name="page">頁數</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <returns>遊戲記錄</returns>
        Task<PagedResult<MiniGame>> GetUserGameHistoryAsync(int userId, int page = 1, int pageSize = 20);

        /// <summary>
        /// 取得使用者遊戲統計
        /// </summary>
        /// <param name="userId">使用者 ID</param>
        /// <returns>遊戲統計</returns>
        Task<MiniGameStats> GetUserGameStatsAsync(int userId);
    }

    /// <summary>
    /// 遊戲 Repository 介面
    /// </summary>
    public interface IGameRepository : IRepository<Game>
    {
        /// <summary>
        /// 根據類型取得遊戲
        /// </summary>
        /// <param name="genre">遊戲類型</param>
        /// <returns>遊戲列表</returns>
        Task<IEnumerable<Game>> GetByGenreAsync(string genre);

        /// <summary>
        /// 搜尋遊戲
        /// </summary>
        /// <param name="searchTerm">搜尋關鍵字</param>
        /// <returns>遊戲列表</returns>
        Task<IEnumerable<Game>> SearchGamesAsync(string searchTerm);
    }

    /// <summary>
    /// 論壇 Repository 介面
    /// </summary>
    public interface IForumRepository : IRepository<Forum>
    {
        /// <summary>
        /// 取得所有論壇版面 (包含遊戲資訊)
        /// </summary>
        /// <returns>論壇列表</returns>
        Task<IEnumerable<Forum>> GetAllWithGamesAsync();

        /// <summary>
        /// 根據遊戲 ID 取得論壇
        /// </summary>
        /// <param name="gameId">遊戲 ID</param>
        /// <returns>論壇或 null</returns>
        Task<Forum?> GetByGameIdAsync(int gameId);
    }

    /// <summary>
    /// 商城 Repository 介面
    /// </summary>
    public interface IStoreRepository : IRepository<ProductInfo>
    {
        /// <summary>
        /// 取得商品 (包含詳細資訊)
        /// </summary>
        /// <param name="productId">商品 ID</param>
        /// <returns>商品資訊</returns>
        Task<ProductInfo?> GetProductWithDetailsAsync(int productId);

        /// <summary>
        /// 搜尋商品
        /// </summary>
        /// <param name="searchTerm">搜尋關鍵字</param>
        /// <param name="productType">商品類型</param>
        /// <param name="page">頁數</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <returns>商品列表</returns>
        Task<PagedResult<ProductInfo>> SearchProductsAsync(string? searchTerm, string? productType, int page = 1, int pageSize = 20);
    }

    /// <summary>
    /// 玩家市場 Repository 介面
    /// </summary>
    public interface IPlayerMarketRepository : IRepository<PlayerMarketProductInfo>
    {
        /// <summary>
        /// 搜尋玩家市場商品
        /// </summary>
        /// <param name="searchTerm">搜尋關鍵字</param>
        /// <param name="page">頁數</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <returns>商品列表</returns>
        Task<PagedResult<PlayerMarketProductInfo>> SearchProductsAsync(string? searchTerm, int page = 1, int pageSize = 20);
    }

    /// <summary>
    /// 通知 Repository 介面
    /// </summary>
    public interface INotificationRepository : IRepository<Notification>
    {
        /// <summary>
        /// 建立寵物換色通知
        /// </summary>
        /// <param name="userId">使用者 ID</param>
        /// <param name="skinColor">新膚色</param>
        /// <param name="backgroundColor">新背景色</param>
        /// <param name="pointsCost">消耗點數</param>
        /// <returns>Task</returns>
        Task CreatePetColorChangeNotificationAsync(int userId, string skinColor, string backgroundColor, int pointsCost);

        /// <summary>
        /// 取得使用者未讀通知數量
        /// </summary>
        /// <param name="userId">使用者 ID</param>
        /// <returns>未讀通知數量</returns>
        Task<int> GetUnreadCountAsync(int userId);
    }

    /// <summary>
    /// 聊天 Repository 介面
    /// </summary>
    public interface IChatRepository : IRepository<ChatMessage>
    {
        /// <summary>
        /// 取得兩個使用者之間的聊天記錄
        /// </summary>
        /// <param name="userId1">使用者 1</param>
        /// <param name="userId2">使用者 2</param>
        /// <param name="page">頁數</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <returns>聊天記錄</returns>
        Task<PagedResult<ChatMessage>> GetChatHistoryAsync(int userId1, int userId2, int page = 1, int pageSize = 50);
    }

    /// <summary>
    /// 管理者 Repository 介面
    /// </summary>
    public interface IManagerRepository : IRepository<ManagerData>
    {
        /// <summary>
        /// 根據帳號取得管理者
        /// </summary>
        /// <param name="account">管理者帳號</param>
        /// <returns>管理者資料</returns>
        Task<ManagerData?> GetByAccountAsync(string account);

        /// <summary>
        /// 取得管理者權限
        /// </summary>
        /// <param name="managerId">管理者 ID</param>
        /// <returns>權限列表</returns>
        Task<IEnumerable<ManagerRolePermission>> GetPermissionsAsync(int managerId);
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