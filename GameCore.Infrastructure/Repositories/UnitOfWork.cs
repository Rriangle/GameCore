using GameCore.Core.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace GameCore.Infrastructure.Repositories
{
    /// <summary>
    /// 工作單元實作
    /// 統一管理所有 Repository，確保資料一致性
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GameCoreDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(GameCoreDbContext context)
        {
            _context = context;
        }

        #region Repository 屬性
        public IUserRepository UserRepository => new UserRepository(_context);
        public IPetRepository PetRepository => new PetRepository(_context);
        public ISignInRepository SignInRepository => new SignInRepository(_context);
        public IMiniGameRepository MiniGameRepository => new MiniGameRepository(_context);
        public IGameRepository GameRepository => new GameRepository(_context);
        public IForumRepository ForumRepository => new ForumRepository(_context);
        public IStoreRepository StoreRepository => new StoreRepository(_context);
        public IPlayerMarketRepository PlayerMarketRepository => new PlayerMarketRepository(_context);
        public INotificationRepository NotificationRepository => new NotificationRepository(_context);
        public IChatRepository ChatRepository => new ChatRepository(_context);
        public IManagerRepository ManagerRepository => new ManagerRepository(_context);
        #endregion

        /// <summary>
        /// 儲存所有變更
        /// </summary>
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 開始資料庫交易
        /// </summary>
        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        /// <summary>
        /// 提交交易
        /// </summary>
        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        /// <summary>
        /// 回滾交易
        /// </summary>
        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        /// <summary>
        /// 釋放資源
        /// </summary>
        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}

