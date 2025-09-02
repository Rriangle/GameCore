using GameCore.Domain.Interfaces;
using GameCore.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameCore.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GameCoreDbContext _context;
        private readonly ILoggerFactory _loggerFactory;
        private bool _disposed = false;

        // Repository instances
        private IUserRepository? _userRepository;
        private IPetRepository? _petRepository;
        private ISignInRepository? _signInRepository;
        private IMiniGameRepository? _miniGameRepository;
        private IGameRepository? _gameRepository;
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
        private IReplyRepository? _replyRepository;
        private IMemberSalesProfileRepository? _memberSalesProfileRepository;
        private IUserSalesInformationRepository? _userSalesInformationRepository;
        private IUserWalletRepository? _userWalletRepository;
        private IUserRightsRepository? _userRightsRepository;
        private IPlayerMarketOrderRepository? _playerMarketOrderRepository;
        private IProductRepository? _productRepository;
        private IGameSettingsRepository? _gameSettingsRepository;
        private IManagerDataRepository? _managerDataRepository;
        private IMarketItemRepository? _marketItemRepository;

        public UnitOfWork(GameCoreDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _loggerFactory = loggerFactory;
        }

        // Repository Properties - 實現 IUnitOfWork 接口
        public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context, _loggerFactory.CreateLogger<UserRepository>());
        public IPetRepository PetRepository => _petRepository ??= new PetRepository(_context, _loggerFactory.CreateLogger<PetRepository>());
        public ISignInRepository SignInRepository => _signInRepository ??= new SignInRepository(_context, _loggerFactory.CreateLogger<SignInRepository>());
        public IMiniGameRepository MiniGameRepository => _miniGameRepository ??= new MiniGameRepository(_context, _loggerFactory.CreateLogger<MiniGameRepository>());
        public IGameRepository GameRepository => _gameRepository ??= new GameRepository(_context, _loggerFactory.CreateLogger<GameRepository>());
        public IForumRepository ForumRepository => _forumRepository ??= new ForumRepository(_context, _loggerFactory.CreateLogger<ForumRepository>());
        public IPostRepository PostRepository => _postRepository ??= new PostRepository(_context, _loggerFactory.CreateLogger<PostRepository>());
        public IPostReplyRepository PostReplyRepository => _postReplyRepository ??= new PostReplyRepository(_context, _loggerFactory.CreateLogger<PostReplyRepository>());
        public IStoreRepository StoreRepository => _storeRepository ??= new StoreRepository(_context, _loggerFactory.CreateLogger<StoreRepository>());
        public IOrderRepository OrderRepository => _orderRepository ??= new OrderRepository(_context, _loggerFactory.CreateLogger<OrderRepository>());
        public ICartRepository CartRepository => _cartRepository ??= new CartRepository(_context, _loggerFactory.CreateLogger<CartRepository>());
        public IPlayerMarketRepository PlayerMarketRepository => _playerMarketRepository ??= new PlayerMarketRepository(_context, _loggerFactory.CreateLogger<PlayerMarketRepository>());
        public IMarketTransactionRepository MarketTransactionRepository => _marketTransactionRepository ??= new MarketTransactionRepository(_context, _loggerFactory.CreateLogger<MarketTransactionRepository>());
        public IMarketReviewRepository MarketReviewRepository => _marketReviewRepository ??= new MarketReviewRepository(_context, _loggerFactory.CreateLogger<MarketReviewRepository>());
        public INotificationRepository NotificationRepository => _notificationRepository ??= new NotificationRepository(_context, _loggerFactory.CreateLogger<NotificationRepository>());
        public INotificationSourceRepository NotificationSourceRepository => _notificationSourceRepository ??= new NotificationSourceRepository(_context, _loggerFactory.CreateLogger<NotificationSourceRepository>());
        public INotificationActionRepository NotificationActionRepository => _notificationActionRepository ??= new NotificationActionRepository(_context, _loggerFactory.CreateLogger<NotificationActionRepository>());
        public IChatRepository ChatRepository => _chatRepository ??= new ChatRepository(_context, _loggerFactory.CreateLogger<ChatRepository>());
        public IChatMessageRepository ChatMessageRepository => _chatMessageRepository ??= new ChatMessageRepository(_context, _loggerFactory.CreateLogger<ChatMessageRepository>());
        public IPrivateChatRepository PrivateChatRepository => _privateChatRepository ??= new PrivateChatRepository(_context, _loggerFactory.CreateLogger<PrivateChatRepository>());
        public IPrivateMessageRepository PrivateMessageRepository => _privateMessageRepository ??= new PrivateMessageRepository(_context, _loggerFactory.CreateLogger<PrivateMessageRepository>());
        public IManagerRepository ManagerRepository => _managerRepository ??= new ManagerRepository(_context, _loggerFactory.CreateLogger<ManagerRepository>());
        public IManagerRolePermissionRepository ManagerRolePermissionRepository => _managerRolePermissionRepository ??= new ManagerRolePermissionRepository(_context, _loggerFactory.CreateLogger<ManagerRolePermissionRepository>());
        public IReplyRepository ReplyRepository => _replyRepository ??= new ReplyRepository(_context, _loggerFactory.CreateLogger<ReplyRepository>());
        public IMemberSalesProfileRepository MemberSalesProfileRepository => _memberSalesProfileRepository ??= new MemberSalesProfileRepository(_context, _loggerFactory.CreateLogger<MemberSalesProfileRepository>());
        public IUserSalesInformationRepository UserSalesInformationRepository => _userSalesInformationRepository ??= new UserSalesInformationRepository(_context, _loggerFactory.CreateLogger<UserSalesInformationRepository>());
        public IUserWalletRepository UserWalletRepository => _userWalletRepository ??= new UserWalletRepository(_context, _loggerFactory.CreateLogger<UserWalletRepository>());
        public IUserRightsRepository UserRightsRepository => _userRightsRepository ??= new UserRightsRepository(_context, _loggerFactory.CreateLogger<UserRightsRepository>());
        public IPlayerMarketOrderRepository PlayerMarketOrderRepository => _playerMarketOrderRepository ??= new PlayerMarketOrderRepository(_context, _loggerFactory.CreateLogger<PlayerMarketOrderRepository>());
        public IProductRepository ProductRepository => _productRepository ??= new ProductRepository(_context, _loggerFactory.CreateLogger<ProductRepository>());
        public IGameSettingsRepository GameSettingsRepository => _gameSettingsRepository ??= new GameSettingsRepository(_context, _loggerFactory.CreateLogger<GameSettingsRepository>());
        public IManagerDataRepository ManagerDataRepository => _managerDataRepository ??= new ManagerDataRepository(_context, _loggerFactory.CreateLogger<ManagerDataRepository>());
        public IMarketItemRepository MarketItemRepository => _marketItemRepository ??= new MarketItemRepository(_context, _loggerFactory.CreateLogger<MarketItemRepository>());

        // 保????屬?以???�容
        public IUserRepository Users => UserRepository;
        public IPetRepository Pets => PetRepository;
        public ISignInRepository SignIns => SignInRepository;
        public IMiniGameRepository MiniGames => MiniGameRepository;
        public IForumRepository Forums => ForumRepository;
        public IPostRepository Posts => PostRepository;
        public IPostReplyRepository PostReplies => PostReplyRepository;
        public IStoreRepository Store => StoreRepository;
        public IOrderRepository Orders => OrderRepository;
        public ICartRepository Carts => CartRepository;
        public IPlayerMarketRepository PlayerMarket => PlayerMarketRepository;
        public IMarketTransactionRepository MarketTransactions => MarketTransactionRepository;
        public IMarketReviewRepository MarketReviews => MarketReviewRepository;
        public INotificationRepository Notifications => NotificationRepository;
        public INotificationSourceRepository NotificationSources => NotificationSourceRepository;
        public INotificationActionRepository NotificationActions => NotificationActionRepository;
        public IChatRepository Chats => ChatRepository;
        public IChatMessageRepository ChatMessages => ChatMessageRepository;
        public IPrivateChatRepository PrivateChats => PrivateChatRepository;
        public IPrivateMessageRepository PrivateMessages => PrivateMessageRepository;
        public IManagerRepository Managers => ManagerRepository;
        public IManagerRolePermissionRepository ManagerRolePermissions => ManagerRolePermissionRepository;

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _loggerFactory.CreateLogger<UnitOfWork>().LogError(ex, "Error occurred while saving changes to database");
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
                _loggerFactory.CreateLogger<UnitOfWork>().LogError(ex, "Error occurred while committing transaction");
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
