using GameCore.Core.Interfaces;
using GameCore.Infrastructure.Repositories;

namespace GameCore.Infrastructure.Data
{
    /// <summary>
    /// 工作單元實作
    /// 統一管理所有 Repository，確保資料一致性
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GameCoreDbContext _context;
        private bool _disposed;

        // Repository 實例
        private IUserRepository? _userRepository;
        private IPetRepository? _petRepository;
        private IForumRepository? _forumRepository;
        private IGameRepository? _gameRepository;
        private IStoreRepository? _storeRepository;
        private IManagerRepository? _managerRepository;
        private IChatRepository? _chatRepository;
        private INotificationRepository? _notificationRepository;
        private IPlayerMarketRepository? _playerMarketRepository;
        private ISignInRepository? _signInRepository;
        private IMiniGameRepository? _miniGameRepository;

        public UnitOfWork(GameCoreDbContext context)
        {
            _context = context;
        }

        #region Repository 屬性

        public IUserRepository UserRepository
        {
            get { return _userRepository ??= new UserRepository(_context); }
        }

        public IPetRepository PetRepository
        {
            get { return _petRepository ??= new PetRepository(_context); }
        }

        public IForumRepository ForumRepository
        {
            get { return _forumRepository ??= new ForumRepository(_context); }
        }

        public IGameRepository GameRepository
        {
            get { return _gameRepository ??= new GameRepository(_context); }
        }

        public IStoreRepository StoreRepository
        {
            get { return _storeRepository ??= new StoreRepository(_context); }
        }

        public IManagerRepository ManagerRepository
        {
            get { return _managerRepository ??= new ManagerRepository(_context); }
        }

        public IChatRepository ChatRepository
        {
            get { return _chatRepository ??= new ChatRepository(_context); }
        }

        public INotificationRepository NotificationRepository
        {
            get { return _notificationRepository ??= new NotificationRepository(_context); }
        }

        public IPlayerMarketRepository PlayerMarketRepository
        {
            get { return _playerMarketRepository ??= new PlayerMarketRepository(_context); }
        }

        public ISignInRepository SignInRepository
        {
            get { return _signInRepository ??= new SignInRepository(_context); }
        }

        public IMiniGameRepository MiniGameRepository
        {
            get { return _miniGameRepository ??= new MiniGameRepository(_context); }
        }

        #endregion

        #region 交易管理

        /// <summary>
        /// 開始交易
        /// </summary>
        /// <returns></returns>
        public async Task BeginTransactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }

        /// <summary>
        /// 提交交易
        /// </summary>
        /// <returns></returns>
        public async Task CommitTransactionAsync()
        {
            await _context.Database.CommitTransactionAsync();
        }

        /// <summary>
        /// 回滾交易
        /// </summary>
        /// <returns></returns>
        public async Task RollbackTransactionAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }

        /// <summary>
        /// 儲存變更
        /// </summary>
        /// <returns>影響的記錄數</returns>
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 儲存變更 (同步版本)
        /// </summary>
        /// <returns>影響的記錄數</returns>
        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        #endregion

        #region IDisposable

        /// <summary>
        /// 釋放資源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 釋放資源
        /// </summary>
        /// <param name="disposing">是否正在釋放</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
                _disposed = true;
            }
        }

        #endregion
    }
}
