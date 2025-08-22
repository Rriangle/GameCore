using GameCore.Core.Interfaces;
using GameCore.Infrastructure.Data;
using GameCore.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;

namespace GameCore.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GameCoreDbContext _context;
        private readonly ILogger<UnitOfWork> _logger;
        private bool _disposed = false;

        // Repository instances
        private IUserRepository? _userRepository;
        private IPetRepository? _petRepository;
        private ISignInRepository? _signInRepository;
        private IMiniGameRepository? _miniGameRepository;
        private IForumRepository? _forumRepository;
        private IPostRepository? _postRepository;
        private IPostReplyRepository? _postReplyRepository;
        private IStoreRepository? _storeRepository;
        private IOrderRepository? _orderRepository;
        private ICartRepository? _cartRepository;
        private IPlayerMarketRepository? _playerMarketRepository;
        private IMarketTransactionRepository? _marketTransactionRepository;
        private IMarketReviewRepository? _marketReviewRepository;
        private INotificationRepository? _notificationRepository;
        private INotificationSourceRepository? _notificationSourceRepository;
        private INotificationActionRepository? _notificationActionRepository;
        private IChatRepository? _chatRepository;
        private IChatMessageRepository? _chatMessageRepository;
        private IPrivateChatRepository? _privateChatRepository;
        private IPrivateMessageRepository? _privateMessageRepository;
        private IManagerRepository? _managerRepository;
        private IManagerRolePermissionRepository? _managerRolePermissionRepository;

        public UnitOfWork(GameCoreDbContext context, ILogger<UnitOfWork> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Repository Properties
        public IUserRepository Users => _userRepository ??= new UserRepository(_context, _logger.CreateLogger<UserRepository>());
        public IPetRepository Pets => _petRepository ??= new PetRepository(_context, _logger.CreateLogger<PetRepository>());
        public ISignInRepository SignIns => _signInRepository ??= new SignInRepository(_context, _logger.CreateLogger<SignInRepository>());
        public IMiniGameRepository MiniGames => _miniGameRepository ??= new MiniGameRepository(_context, _logger.CreateLogger<MiniGameRepository>());
        public IForumRepository Forums => _forumRepository ??= new ForumRepository(_context, _logger.CreateLogger<ForumRepository>());
        public IPostRepository Posts => _postRepository ??= new PostRepository(_context, _logger.CreateLogger<PostRepository>());
        public IPostReplyRepository PostReplies => _postReplyRepository ??= new PostReplyRepository(_context, _logger.CreateLogger<PostReplyRepository>());
        public IStoreRepository Store => _storeRepository ??= new StoreRepository(_context, _logger.CreateLogger<StoreRepository>());
        public IOrderRepository Orders => _orderRepository ??= new OrderRepository(_context, _logger.CreateLogger<OrderRepository>());
        public ICartRepository Carts => _cartRepository ??= new CartRepository(_context, _logger.CreateLogger<CartRepository>());
        public IPlayerMarketRepository PlayerMarket => _playerMarketRepository ??= new PlayerMarketRepository(_context, _logger.CreateLogger<PlayerMarketRepository>());
        public IMarketTransactionRepository MarketTransactions => _marketTransactionRepository ??= new MarketTransactionRepository(_context, _logger.CreateLogger<MarketTransactionRepository>());
        public IMarketReviewRepository MarketReviews => _marketReviewRepository ??= new MarketReviewRepository(_context, _logger.CreateLogger<MarketReviewRepository>());
        public INotificationRepository Notifications => _notificationRepository ??= new NotificationRepository(_context, _logger.CreateLogger<NotificationRepository>());
        public INotificationSourceRepository NotificationSources => _notificationSourceRepository ??= new NotificationSourceRepository(_context, _logger.CreateLogger<NotificationSourceRepository>());
        public INotificationActionRepository NotificationActions => _notificationActionRepository ??= new NotificationActionRepository(_context, _logger.CreateLogger<NotificationActionRepository>());
        public IChatRepository Chats => _chatRepository ??= new ChatRepository(_context, _logger.CreateLogger<ChatRepository>());
        public IChatMessageRepository ChatMessages => _chatMessageRepository ??= new ChatMessageRepository(_context, _logger.CreateLogger<ChatMessageRepository>());
        public IPrivateChatRepository PrivateChats => _privateChatRepository ??= new PrivateChatRepository(_context, _logger.CreateLogger<PrivateChatRepository>());
        public IPrivateMessageRepository PrivateMessages => _privateMessageRepository ??= new PrivateMessageRepository(_context, _logger.CreateLogger<PrivateMessageRepository>());
        public IManagerRepository Managers => _managerRepository ??= new ManagerRepository(_context, _logger.CreateLogger<ManagerRepository>());
        public IManagerRolePermissionRepository ManagerRolePermissions => _managerRolePermissionRepository ??= new ManagerRolePermissionRepository(_context, _logger.CreateLogger<ManagerRolePermissionRepository>());

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while saving changes to database");
                throw;
            }
        }

        public async Task BeginTransactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _context.Database.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while committing transaction");
                await _context.Database.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context?.Dispose();
                _disposed = true;
            }
        }
    }
}