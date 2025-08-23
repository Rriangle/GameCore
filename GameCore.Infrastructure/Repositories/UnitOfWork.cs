using GameCore.Core.Interfaces;
using GameCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace GameCore.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GameCoreDbContext _context;
        private readonly Dictionary<Type, object> _repositories;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(GameCoreDbContext context)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();
        }

        public IUserRepository UserRepository => GetRepository<IUserRepository>();
        public IPetRepository PetRepository => GetRepository<IPetRepository>();
        public ISignInRepository SignInRepository => GetRepository<ISignInRepository>();
        public IMiniGameRepository MiniGameRepository => GetRepository<IMiniGameRepository>();
        public IGameRepository GameRepository => GetRepository<IGameRepository>();
        public IForumRepository ForumRepository => GetRepository<IForumRepository>();
        public IStoreRepository StoreRepository => GetRepository<IStoreRepository>();
        public IPlayerMarketRepository PlayerMarketRepository => GetRepository<IPlayerMarketRepository>();
        public INotificationRepository NotificationRepository => GetRepository<INotificationRepository>();
        public IChatRepository ChatRepository => GetRepository<IChatRepository>();
        public IManagerRepository ManagerRepository => GetRepository<IManagerRepository>();

        private T GetRepository<T>() where T : class
        {
            var type = typeof(T);
            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = Activator.CreateInstance(type, _context);
            }
            return (T)_repositories[type];
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context?.Dispose();
        }
    }
}